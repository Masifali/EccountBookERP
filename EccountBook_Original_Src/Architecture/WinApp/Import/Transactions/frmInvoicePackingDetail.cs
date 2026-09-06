using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Import;
using Architecture.BLL.Import.Transaction;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Export;
using Architecture.Model.Import;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Import.Transactions;

public class frmInvoicePackingDetail : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtPmDetail = new DataTable();

	private long RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private int updateDetailIndexPmDetail = -1;

	private bool FlagFin;

	private bool ItemSearchWithNameOrCode;

	private DataTable dtPMItem = new DataTable();

	private DataTable dtBrandForPMItem = new DataTable();

	private List<invoicePackingList> LstRemoveRecordDetail = new List<invoicePackingList>();

	private List<ExImLcOrderPackingDetail> CurrentOrderDetailRecord = new List<ExImLcOrderPackingDetail>();

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

	private ToolStripButton Print;

	private Panel panel17;

	private ToolStrip toolStrip3;

	private ToolStripButton btnUpdate;

	private Label label28;

	private ToolStripButton BtnRefresh;

	private Panel panel5;

	private TabControl tabControl2;

	private CtrlGrdBar ctrlGrdBar1;

	private TabPage tabPackingMaterialDetail;

	private Panel panel15;

	internal UltraCombo CmbItem;

	private Label label64;

	private Label label63;

	private Label label62;

	private Label label61;

	private Label label60;

	private Label label58;

	internal UltraCombo CmbBrandPackingTypePm;

	private TextBox txtWeight;

	private TextBox txtPackingAmount;

	private TextBox txtPackingItemRate;

	private TextBox txtPackingRemarks;

	private Button btnPackingCancel;

	private Button btnPackingUpdate;

	private Button btnPackingAdd;

	internal GridEX grdPackingMaterialDetail;

	private Panel panel28;

	private Label label66;

	private RadioButton rdPackingItemCode;

	private RadioButton rdPackingItemName;

	private ToolStripButton btnShortCutKey;

	internal UltraCombo cmbBrandForPmEntry;

	private Label label47;

	private RadioButton radBrandCode;

	private RadioButton radBrandName;

	private Panel panel30;

	private GroupBox groupBox1;

	private Label label52;

	internal GridEX DataGridHistory;

	private Panel panel29;

	private Label label65;

	private Panel panel18;

	private GroupBox groupBox5;

	private RadioButton rdapproveddate;

	private Button btnShowHistory;

	private RadioButton rdmodifydate;

	private Label label67;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label73;

	private Label label89;

	private Label label90;

	private Label label91;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private CheckBox ChkPreview;

	private TabControl tabControl3;

	private TabPage tabPmH;

	internal GridEX grdPMHistory;

	private Panel panel32;

	private Label label75;

	private TextBox txtBrandInnerQtyPm;

	private TextBox txtBrandOuterQtyPm;

	internal UltraCombo CmbBrandUomPm;

	private Label label77;

	private Label label79;

	private Label label78;

	private TextBox txtQty;

	internal UltraCombo CmbPackUom;

	private Label label80;

	private Label label81;

	private Panel panel16;

	private Label label56;

	internal UltraCombo CmbInvoiceNo;

	private Label label2;

	private TextBox txtItemDescription;

	private Label label1;

	private TextBox txtContainerNo;

	private Label label94;

	internal UltraCombo CmbJobLot;

	private ToolStripButton btnSave;

	public frmInvoicePackingDetail(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmInvoicePackingDetail_Load(object sender, EventArgs e)
	{
		try
		{
			InitializeEssentialComponents();
			Task.Run(() => LoadNonEssentialDataAsync());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeEssentialComponents()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 750);
			ConfigureRights();
			ConfigureControls();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ConfigureRights()
	{
		try
		{
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnUpdate.Enabled = formrights.DoHaveUpdateRights || formrights.DoHaveSaveRight;
			Print.Enabled = formrights.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ItemSearchByCode"));
			if (ItemSearchWithNameOrCode)
			{
				radBrandName.Checked = true;
				rdPackingItemCode.Checked = true;
			}
			else
			{
				radBrandCode.Checked = true;
				rdPackingItemName.Checked = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ConfigureControls()
	{
		try
		{
			dtPMItem.Columns.Add("Id", typeof(int));
			dtPMItem.Columns.Add("ItemName");
			dtPMItem.Columns.Add("ItemCode");
			dtBrandForPMItem.Columns.Add("Id", typeof(int));
			dtBrandForPMItem.Columns.Add("ItemName");
			dtBrandForPMItem.Columns.Add("ItemCode");
			dtBrandForPMItem.Columns.Add("PackingTypeId", typeof(int));
			dtBrandForPMItem.Columns.Add("PackingType", typeof(string));
			dtBrandForPMItem.Columns.Add("PackUomId", typeof(int));
			dtBrandForPMItem.Columns.Add("PackUom", typeof(string));
			dtBrandForPMItem.Columns.Add("OuterQty", typeof(double));
			dtBrandForPMItem.Columns.Add("InnerQty", typeof(double));
			dtBrandForPMItem.Columns.Add("M_Ton", typeof(double));
			dtPmDetail.Columns.Add("Id", typeof(int));
			dtPmDetail.Columns.Add("ContainerNo");
			dtPmDetail.Columns.Add("ItemId", typeof(int));
			dtPmDetail.Columns.Add("ItemCode");
			dtPmDetail.Columns.Add("ItemName");
			dtPmDetail.Columns.Add("ItemDescription");
			dtPmDetail.Columns.Add("JobLotId", typeof(int));
			dtPmDetail.Columns.Add("JobLot", typeof(string));
			dtPmDetail.Columns.Add("PackUomId", typeof(int));
			dtPmDetail.Columns.Add("PackUom");
			dtPmDetail.Columns.Add("Qty", typeof(double));
			dtPmDetail.Columns.Add("Weight", typeof(double));
			dtPmDetail.Columns.Add("Remarks");
			grdPackingMaterialDetail.DataSource = dtPmDetail;
			grdPackingMaterialDetail.RetrieveStructure();
			grdSettingsPM();
			tabControl2.SelectedIndex = 0;
			CmbInvoiceNo.Focus();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private async Task LoadNonEssentialDataAsync()
	{
		try
		{
			await Task.Run(delegate
			{
				BindData();
			});
			Invoke((Action)delegate
			{
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindData()
	{
		try
		{
			InvoiceNoFill();
			PackingItemFill();
			JobLotFill();
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RunOnUiThread(Action action)
	{
		if (base.InvokeRequired)
		{
			Invoke(action);
		}
		else
		{
			action();
		}
	}

	private void InvoiceNoFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				int pmId = Conversion.ToInt(CmbInvoiceNo.Value);
				int id = clsGlobalVariables.ActiveYr.Id;
				DataTable dataTable = new DataTable
				{
					Columns = 
					{
						{
							"Id",
							typeof(long)
						},
						{
							"InvoiceNo",
							typeof(string)
						}
					}
				};
				DataTable invoiceNo = invoicePackingList.GetInvoiceNo(UserAccount.OrganizationId, UserAccount.CompanyId, 902, id, RecId);
				if (invoiceNo.Rows.Count > 0)
				{
					for (int i = 0; i < invoiceNo.Rows.Count; i++)
					{
						dataTable.Rows.Add(invoiceNo.Rows[i]["invoiceMasterId"], invoiceNo.Rows[i]["invoiceMasterNo"]);
					}
				}
				if (dataTable.Rows.Count > 0)
				{
					DDL.BindDDL(dataTable, CmbInvoiceNo, "Id", "InvoiceNo", "Invoice No", ZeroIndex: false);
					if (pmId > 0)
					{
						if (dataTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == pmId))
						{
							CmbInvoiceNo.Value = pmId;
						}
						else
						{
							((Control)(object)CmbInvoiceNo).Text = string.Empty;
						}
					}
				}
				else
				{
					((UltraGridBase)CmbInvoiceNo).DataSource = null;
					((Control)(object)CmbInvoiceNo).Text = string.Empty;
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingItemFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				int pmId = Conversion.ToInt(CmbItem.Value);
				dtPMItem.Rows.Clear();
				DataTable itemByItemTypeId = CommonServices.GetItemByItemTypeId("14");
				if (itemByItemTypeId.Rows.Count > 0)
				{
					for (int i = 0; i < itemByItemTypeId.Rows.Count; i++)
					{
						dtPMItem.Rows.Add(itemByItemTypeId.Rows[i]["Id"], itemByItemTypeId.Rows[i]["ItemName"], itemByItemTypeId.Rows[i]["ItemCodeNew"]);
					}
				}
				if (dtPMItem.Rows.Count > 0)
				{
					if (rdPackingItemName.Checked)
					{
						DDL.BindDDL(dtPMItem, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
					}
					else
					{
						DDL.BindDDL(dtPMItem, CmbItem, "Id", "ItemCode", "Item Name", ZeroIndex: false);
					}
					((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
					if (pmId > 0)
					{
						if (dtPMItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == pmId))
						{
							CmbItem.Value = pmId;
						}
						else
						{
							((Control)(object)CmbItem).Text = string.Empty;
						}
					}
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Expected O, but got Unknown
				DataTable jobLotsAllocatedToBranchByBranchId = JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					BranchesId = UserAccount.BranchesId
				});
				if (jobLotsAllocatedToBranchByBranchId.Rows.Count > 0)
				{
					DDL.BindDDLNew(jobLotsAllocatedToBranchByBranchId, CmbJobLot, "Id", "JobLotDescription", "Job Lot", false);
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdPackingItemName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtPMItem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(CmbItem.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbItem.Value);
				}
				if (rdPackingItemName.Checked)
				{
					DDL.BindDDL(dtPMItem, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtPMItem, CmbItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				CmbItem.Value = ItemId;
				CmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FilldtBrandForPMItem()
	{
		try
		{
			if (CurrentOrderDetailRecord.Count <= 0)
			{
				return;
			}
			dtBrandForPMItem.Clear();
			Dictionary<(int, int, int), (double, double)> accumulatedData = new Dictionary<(int, int, int), (double, double)>();
			Dictionary<(int, int, int), ExImLcOrderPackingDetail> rowCache = new Dictionary<(int, int, int), ExImLcOrderPackingDetail>();
			foreach (ExImLcOrderPackingDetail r2 in CurrentOrderDetailRecord)
			{
				int itemId = Conversion.ToInt((object)r2.ExImItemId);
				int packTypeId = Conversion.ToInt((object)r2.InvPackingMaterialTypeId);
				int packSizeId = Conversion.ToInt((object)r2.UOMScheduleIdOuter);
				double noOfBags = Conversion.ToDouble((object)r2.NoOfUnit);
				double mTon = Conversion.ToDouble((object)r2.OuterQty);
				(int, int, int) key = (itemId, packTypeId, packSizeId);
				if (!accumulatedData.ContainsKey(key))
				{
					accumulatedData[key] = (noOfBags, mTon);
					rowCache[key] = r2;
				}
				else
				{
					accumulatedData[key] = (accumulatedData[key].Item1 + noOfBags, accumulatedData[key].Item2 + mTon);
				}
			}
			foreach (KeyValuePair<(int, int, int), (double, double)> entry in accumulatedData)
			{
				ExImLcOrderPackingDetail r = rowCache[entry.Key];
				dtBrandForPMItem.Rows.Add(entry.Key.Item1, Conversion.ToString((object)r.ItemName), Conversion.ToString((object)r.ItemCode), entry.Key.Item2, Conversion.ToString((object)r.ExImPackMaterialTypeName), entry.Key.Item3, Conversion.ToString((object)r.PackUom), entry.Value.Item1, entry.Value.Item1 * Conversion.ToDouble((object)r.QtyEquivalent), entry.Value.Item2);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindBrandItemsFromOutPutGrid()
	{
		try
		{
			FilldtBrandForPMItem();
			int id = Conversion.ToInt(cmbBrandForPmEntry.Value);
			if (dtBrandForPMItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtBrandForPMItem, cmbBrandForPmEntry, "Id", "ItemName", "Brand Item", ZeroIndex: true);
				((UltraGridBase)cmbBrandForPmEntry).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)cmbBrandForPmEntry).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				if (id > 0)
				{
					if (dtBrandForPMItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						cmbBrandForPmEntry.Value = id;
					}
					else
					{
						((Control)(object)cmbBrandForPmEntry).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)cmbBrandForPmEntry).DataSource = null;
				((Control)(object)cmbBrandForPmEntry).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rabBrandName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtBrandForPMItem.Rows.Count > 0)
			{
				int ItemId = Conversion.ToInt(cmbBrandForPmEntry.Value);
				if (Conversion.ToInt(cmbBrandForPmEntry.Value) > 0)
				{
					ItemId = Conversion.ToInt(cmbBrandForPmEntry.Value);
				}
				if (radBrandCode.Checked)
				{
					DDL.BindDDL(dtBrandForPMItem, cmbBrandForPmEntry, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtBrandForPMItem, cmbBrandForPmEntry, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				cmbBrandForPmEntry.Value = ItemId;
				cmbBrandForPmEntry.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPMItem_Leave(object sender, EventArgs e)
	{
		try
		{
			BindPmItemPackUom();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindPmItemPackUom()
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUom).Text;
			((Control)(object)CmbPackUom).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(CmbItem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbPackUom, "Id", "UOMCode", "PackUom", ZeroIndex: true);
				if (Packuom != string.Empty)
				{
					if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
					{
						((Control)(object)CmbPackUom).Text = Packuom;
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSalesContractNo_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbInvoiceNo.Value) != 0 && (grdPackingMaterialDetail.GetDataRows().Count() <= 0 || MessageBox.Show("Grid already have Records.Do you want To Refresh Data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No))
			{
				RecId = Conversion.ToInt(CmbInvoiceNo.Value);
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (Conversion.ToInt(CmbInvoiceNo.Value) == 0)
		{
			MessageBox.Show("Sale Invoice Is required");
			CmbInvoiceNo.Focus();
			return false;
		}
		return true;
	}

	private void PmGridOuterQtyValidation()
	{
		try
		{
			GridEXRow[] dataRows = grdPackingMaterialDetail.GetDataRows();
			if (dataRows.Count() <= 0)
			{
				return;
			}
			HashSet<(int, int, int)> uniqueIds = new HashSet<(int, int, int)>();
			Dictionary<(int, int, int), double> brandOuterQtyDict = (from row in dtBrandForPMItem.AsEnumerable()
				group row by (BrandId: Conversion.ToInt((object)row.Field<int>("Id")), BrandUomId: Conversion.ToInt((object)row.Field<int>("PackUomId")), PackingTypeId: Conversion.ToInt((object)row.Field<int>("PackingTypeId")))).ToDictionary((IGrouping<(int BrandId, int BrandUomId, int PackingTypeId), DataRow> group) => group.Key, (IGrouping<(int BrandId, int BrandUomId, int PackingTypeId), DataRow> group) => group.Sum((DataRow row) => Conversion.ToDouble((object)row.Field<double>("OuterQty"))));
			GridEXRow[] array = dataRows;
			foreach (GridEXRow row2 in array)
			{
				int selectedBrandId = Conversion.ToInt(row2.Cells["BrandId"].Value);
				int selectedBrandUomId = Conversion.ToInt(row2.Cells["BrandUomId"].Value);
				int selectedPackingTypeId = Conversion.ToInt(row2.Cells["PackingTypeId"].Value);
				if (!uniqueIds.Contains((selectedBrandId, selectedBrandUomId, selectedPackingTypeId)))
				{
					if (!brandOuterQtyDict.TryGetValue((selectedBrandId, selectedBrandUomId, selectedPackingTypeId), out var brandOuterQty))
					{
						string selectedBrand2 = Conversion.ToString(row2.Cells["BrandName"].Value);
						string selectedBrandUom2 = Conversion.ToString(row2.Cells["BrandUom"].Value);
						string selectedPackingType2 = Conversion.ToString(row2.Cells["PackingType"].Value);
						throw new Exception("Brand outer quantity for the selected combination (Brand Item '" + selectedBrand2 + "' with UOM '" + selectedBrandUom2 + "' and PackingType '" + selectedPackingType2 + "') could not be found.");
					}
					double gridOuterQtyPm = (from GridEXRow r in grdPackingMaterialDetail.GetRows()
						where Conversion.ToInt(r.Cells["BrandId"].Value) == selectedBrandId && Conversion.ToInt(r.Cells["BrandUomId"].Value) == selectedBrandUomId && Conversion.ToInt(r.Cells["PackingTypeId"].Value) == selectedPackingTypeId
						select r).Sum((GridEXRow r) => Conversion.ToDouble(r.Cells["PmItemOuterQty"].Value));
					if (gridOuterQtyPm > brandOuterQty)
					{
						double difference = gridOuterQtyPm - brandOuterQty;
						string selectedBrand = Conversion.ToString(row2.Cells["BrandName"].Value);
						string selectedBrandUom = Conversion.ToString(row2.Cells["BrandUom"].Value);
						string selectedPackingType = Conversion.ToString(row2.Cells["PackingType"].Value);
						throw new Exception("PmOuterQty can't be greater than Brand OuterQty.\n" + $"Brand OuterQty is {brandOuterQty}\n" + $"PmOuterQty is {gridOuterQtyPm}\n" + $"Difference is {difference}\n" + "For Brand Item '" + selectedBrand + "' with UOM '" + selectedBrandUom + "' and PackingType '" + selectedPackingType + "'");
					}
				}
				uniqueIds.Add((selectedBrandId, selectedBrandUomId, selectedPackingTypeId));
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PmGridInnerQtyValidation()
	{
		try
		{
			GridEXRow[] dataRows = grdPackingMaterialDetail.GetDataRows();
			if (dataRows.Count() <= 0)
			{
				return;
			}
			HashSet<(int, int, int)> uniqueIds = new HashSet<(int, int, int)>();
			Dictionary<(int, int, int), double> brandOuterQtyDict = (from row in dtBrandForPMItem.AsEnumerable()
				group row by (BrandId: Conversion.ToInt((object)row.Field<int>("Id")), BrandUomId: Conversion.ToInt((object)row.Field<int>("PackUomId")), PackingTypeId: Conversion.ToInt((object)row.Field<int>("PackingTypeId")))).ToDictionary((IGrouping<(int BrandId, int BrandUomId, int PackingTypeId), DataRow> group) => group.Key, (IGrouping<(int BrandId, int BrandUomId, int PackingTypeId), DataRow> group) => group.Sum((DataRow row) => Conversion.ToDouble((object)row.Field<double>("InnerQty"))));
			GridEXRow[] array = dataRows;
			foreach (GridEXRow row2 in array)
			{
				int selectedBrandId = Conversion.ToInt(row2.Cells["BrandId"].Value);
				int selectedBrandUomId = Conversion.ToInt(row2.Cells["BrandUomId"].Value);
				int selectedPackingTypeId = Conversion.ToInt(row2.Cells["PackingTypeId"].Value);
				if (!uniqueIds.Contains((selectedBrandId, selectedBrandUomId, selectedPackingTypeId)))
				{
					if (!brandOuterQtyDict.TryGetValue((selectedBrandId, selectedBrandUomId, selectedPackingTypeId), out var brandOuterQty))
					{
						string selectedBrand2 = Conversion.ToString(row2.Cells["BrandName"].Value);
						string selectedBrandUom2 = Conversion.ToString(row2.Cells["BrandUom"].Value);
						string selectedPackingType2 = Conversion.ToString(row2.Cells["PackingType"].Value);
						throw new Exception("Brand Inner quantity for the selected combination (Brand Item '" + selectedBrand2 + "' with UOM '" + selectedBrandUom2 + "' and PackingType '" + selectedPackingType2 + "') could not be found.");
					}
					double gridOuterQtyPm = (from GridEXRow r in grdPackingMaterialDetail.GetRows()
						where Conversion.ToInt(r.Cells["BrandId"].Value) == selectedBrandId && Conversion.ToInt(r.Cells["BrandUomId"].Value) == selectedBrandUomId && Conversion.ToInt(r.Cells["PackingTypeId"].Value) == selectedPackingTypeId
						select r).Sum((GridEXRow r) => Conversion.ToDouble(r.Cells["PmItemInnerQty"].Value));
					if (gridOuterQtyPm > brandOuterQty)
					{
						double difference = gridOuterQtyPm - brandOuterQty;
						string selectedBrand = Conversion.ToString(row2.Cells["BrandName"].Value);
						string selectedBrandUom = Conversion.ToString(row2.Cells["BrandUom"].Value);
						string selectedPackingType = Conversion.ToString(row2.Cells["PackingType"].Value);
						throw new Exception("PmInnerQty can't be greater than Brand InnerQty.\n" + $"Brand InnerQty is {brandOuterQty}\n" + $"PmInnerQty is {gridOuterQtyPm}\n" + $"Difference is {difference}\n" + "For Brand Item '" + selectedBrand + "' with UOM '" + selectedBrandUom + "' and PackingType '" + selectedPackingType + "'");
					}
				}
				uniqueIds.Add((selectedBrandId, selectedBrandUomId, selectedPackingTypeId));
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ValidatePmQty(bool Update = false)
	{
		try
		{
			double brandOuterQty = Conversion.ToDouble((object)txtBrandOuterQtyPm.Text);
			double brandInnerQty = Conversion.ToDouble((object)txtBrandInnerQtyPm.Text);
			int selectedBrandId = Conversion.ToInt(cmbBrandForPmEntry.Value);
			int selectedBrandUomId = Conversion.ToInt(CmbBrandUomPm.Value);
			int selectedPackingTypeId = Conversion.ToInt(CmbBrandPackingTypePm.Value);
			double currentOuterQtyPm = Conversion.ToDouble((object)txtQty.Text);
			double currentInnerQtyPm = Conversion.ToDouble((object)txtWeight.Text);
			double gridOuterQtyPm = (from GridEXRow row in grdPackingMaterialDetail.GetRows()
				where Conversion.ToInt(row.Cells["BrandId"].Value) == selectedBrandId && Conversion.ToInt(row.Cells["BrandUomId"].Value) == selectedBrandUomId && Conversion.ToInt(row.Cells["PackingTypeId"].Value) == selectedPackingTypeId && (!Update || row.RowIndex != updateDetailIndexPmDetail)
				select row).Sum((GridEXRow row) => Conversion.ToDouble(row.Cells["PmItemOuterQty"].Value));
			double totalOuterQtyPm = gridOuterQtyPm + currentOuterQtyPm;
			if (Conversion.ToDouble((object)txtQty.Text) > 0.0 && totalOuterQtyPm > brandOuterQty)
			{
				double difference = totalOuterQtyPm - brandOuterQty;
				if (gridOuterQtyPm > 0.0)
				{
					throw new Exception("PmOuterQty can't be greater than Brand OuterQty.\n" + $"Brand OuterQty is {brandOuterQty}\n" + $"PmOuterQty is {currentOuterQtyPm}\n" + $"PmOuterQty in Grid is {gridOuterQtyPm}\n" + $"Difference is {difference}");
				}
				throw new Exception("PmOuterQty can't be greater than Brand OuterQty.\n" + $"Brand OuterQty is {brandOuterQty}\n" + $"PmOuterQty is {currentOuterQtyPm}\n" + $"Difference is {difference}");
			}
			double gridInnerQtyPm = (from GridEXRow row in grdPackingMaterialDetail.GetRows()
				where Conversion.ToInt(row.Cells["BrandId"].Value) == selectedBrandId && Conversion.ToInt(row.Cells["BrandUomId"].Value) == selectedBrandUomId && Conversion.ToInt(row.Cells["PackingTypeId"].Value) == selectedPackingTypeId && (!Update || row.RowIndex != updateDetailIndexPmDetail)
				select row).Sum((GridEXRow row) => Conversion.ToDouble(row.Cells["PmItemInnerQty"].Value));
			double totalInnerQtyPm = gridInnerQtyPm + currentInnerQtyPm;
			if (Conversion.ToDouble((object)txtWeight.Text) > 0.0 && totalInnerQtyPm > brandInnerQty)
			{
				double difference2 = totalInnerQtyPm - brandInnerQty;
				if (gridInnerQtyPm > 0.0)
				{
					throw new Exception("PmInnerQty can't be greater than Brand InnerQty.\n" + $"Brand InnerQty is {brandInnerQty}\n" + $"PmInnerQty is {currentInnerQtyPm}\n" + $"PmInnerQty in Grid is {gridInnerQtyPm}\n" + $"Difference is {difference2}");
				}
				throw new Exception("PmInnerQty can't be greater than Brand InnerQty.\n" + $"Brand InnerQty is {brandInnerQty}\n" + $"PmInnerQty is {currentInnerQtyPm}\n" + $"Difference is {difference2}");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool PackingMaterialFormValidation()
	{
		if (Conversion.ToInt(CmbItem.Value) == 0 || ((UltraGridBase)CmbItem).ActiveRow == null)
		{
			MessageBox.Show("Item Is required");
			CmbItem.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbPackUom.Value) == 0 || ((UltraGridBase)CmbPackUom).ActiveRow == null)
		{
			MessageBox.Show("Pack Uom Is required");
			CmbPackUom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty is required");
			txtQty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Weight is required");
			txtWeight.Focus();
			return false;
		}
		return true;
	}

	private void grdSettingsPM()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPackingMaterialDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdPackingMaterialDetail.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdPackingMaterialDetail.RootTable.Columns["Id"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["ItemId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["PackUomId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["ContainerNo"].Width = 80;
			grdPackingMaterialDetail.RootTable.Columns["ItemCode"].Width = 45;
			grdPackingMaterialDetail.RootTable.Columns["ItemName"].Width = 200;
			grdPackingMaterialDetail.RootTable.Columns["ItemDescription"].Width = 200;
			grdPackingMaterialDetail.RootTable.Columns["JobLot"].Width = 120;
			grdPackingMaterialDetail.RootTable.Columns["PackUom"].Width = 60;
			grdPackingMaterialDetail.RootTable.Columns["Qty"].Width = 60;
			grdPackingMaterialDetail.RootTable.Columns["Weight"].Width = 60;
			grdPackingMaterialDetail.RootTable.Columns["Remarks"].Width = 200;
			grdPackingMaterialDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialDetail.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialDetail.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns.Add("Delete");
			grdPackingMaterialDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdPackingMaterialDetail.RootTable.Columns["Delete"].Caption = "X";
			grdPackingMaterialDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterialDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterialDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdPackingMaterialDetail.RootTable.Columns["Delete"].Width = 20;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPackingAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (PackingMaterialFormValidation())
			{
				dtPmDetail.Rows.Add(0, Conversion.ToString((object)txtContainerNo.Text.Trim()), Conversion.ToInt(CmbItem.Value), Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemName"].Value), Conversion.ToString((object)txtItemDescription.Text.Trim()), Conversion.ToInt(CmbJobLot.Value), Conversion.ToString((object)((Control)(object)CmbJobLot).Text), Conversion.ToInt(CmbPackUom.Value), Conversion.ToString((object)((Control)(object)CmbPackUom).Text), Conversion.ToDouble((object)txtQty.Text), Conversion.ToDouble((object)txtWeight.Text), Conversion.ToString((object)txtPackingRemarks.Text));
				grdPackingMaterialDetail.DataSource = dtPmDetail;
				grdPackingMaterialDetail.RetrieveStructure();
				grdSettingsPM();
				ResetPmDetail();
				cmbBrandForPmEntry.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnPackingUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (PackingMaterialFormValidation())
			{
				dtPmDetail.Rows[updateDetailIndexPmDetail]["ContainerNo"] = Conversion.ToString((object)txtContainerNo.Text.Trim());
				dtPmDetail.Rows[updateDetailIndexPmDetail]["ItemId"] = Conversion.ToInt(CmbItem.Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemCode"].Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["ItemName"] = Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemName"].Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["ItemDescription"] = Conversion.ToString((object)txtItemDescription.Text.Trim());
				dtPmDetail.Rows[updateDetailIndexPmDetail]["JobLotId"] = Conversion.ToInt(CmbJobLot.Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["JobLot"] = Conversion.ToString((object)((Control)(object)CmbJobLot).Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PackUomId"] = Conversion.ToInt(CmbPackUom.Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PackUom"] = Conversion.ToString((object)((Control)(object)CmbPackUom).Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["Qty"] = Conversion.ToDouble((object)txtQty.Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["Weight"] = Conversion.ToDouble((object)txtWeight.Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["Remarks"] = Conversion.ToString((object)txtPackingRemarks.Text);
				grdPackingMaterialDetail.DataSource = dtPmDetail;
				grdPackingMaterialDetail.RetrieveStructure();
				btnPackingCancel.Visible = false;
				ResetPmDetail();
				CmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnPackingCancel_Click(object sender, EventArgs e)
	{
		try
		{
			ResetPmDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPackingAdd_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "rdPackingItemName")
			{
				CmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPackingCancel_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "rdPackingItemName")
			{
				CmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterialDetail_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPackingMaterialDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				updateDetailIndexPmDetail = item.RowIndex;
				txtContainerNo.Text = Conversion.ToString(item.Cells["ContainerNo"].Value);
				CmbItem.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
				CmbPMItem_Leave(null, null);
				txtItemDescription.Text = Conversion.ToString(item.Cells["ItemDescription"].Value);
				CmbJobLot.Value = Conversion.ToInt(item.Cells["JobLotId"].Value);
				CmbPackUom.Value = Conversion.ToInt(item.Cells["PackUomId"].Value);
				txtQty.Text = Conversion.ToDouble(item.Cells["Qty"].Value).ToString("#,##0.###");
				txtWeight.Text = Conversion.ToDouble(item.Cells["Weight"].Value).ToString("#,##0.###");
				txtPackingRemarks.Text = item.Cells["Remarks"].Value.ToString();
				btnPackingAdd.Visible = false;
				btnPackingUpdate.Visible = true;
				btnPackingCancel.Visible = true;
				cmbBrandForPmEntry.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterialDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPackingMaterialDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRecord(item);
				}
				grdPackingMaterialDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterialDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdPackingMaterialDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdPackingMaterialDetail_DoubleClick(null, null);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (grdPackingMaterialDetail.CurrentColumn.Key == "Delete")
				{
					if (updateDetailIndexPmDetail != -1)
					{
						throw new Exception("Reset  the Detail first...");
					}
					dtPmDetail.Rows[item.RowIndex].Delete();
					item.Delete();
				}
				grdPackingMaterialDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRecord(GridEXRow r)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		try
		{
			if (updateDetailIndexPmDetail == -1)
			{
				if ((long)Conversion.ToInt(r.Cells["Id"].Value) > 0L)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						invoicePackingList vd = new invoicePackingList();
						vd.invoicePackingListId = Conversion.ToInt(r.Cells["Id"].Value);
						vd.appActionId = 3;
						vd.containerNo = Conversion.ToString(r.Cells["ContainerNo"].Value);
						vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.ItemDescription = Conversion.ToString(r.Cells["ItemDescription"].Value);
						vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						vd.packSizeIdOuter = Conversion.ToInt(r.Cells["PackUomId"].Value);
						vd.packSizeIdinner = Conversion.ToInt(r.Cells["PackUomId"].Value);
						vd.qtyOuter = Conversion.ToDouble(r.Cells["Qty"].Value);
						vd.qtyInner = Conversion.ToDouble(r.Cells["Qty"].Value);
						vd.netWeightOuter = Conversion.ToDouble(r.Cells["Weight"].Value);
						vd.netWeightInner = Conversion.ToDouble(r.Cells["Weight"].Value);
						vd.NetWeightKgs = Conversion.ToDouble(r.Cells["Weight"].Value);
						vd.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
						vd.createdUserId = UserAccount.ID;
						vd.lastModifiedUserId = UserAccount.ID;
						LstRemoveRecordDetail.Add(vd);
						dtPmDetail.Rows[r.RowIndex].Delete();
						r.Delete();
					}
				}
				else
				{
					dtPmDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				return;
			}
			throw new Exception("Reset the Detail first...");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbBrandForPmEntry_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbBrandForPmEntry).ActiveRow != null && Conversion.ToInt(cmbBrandForPmEntry.Value) > 0)
			{
				int BrandUomId = Conversion.ToInt(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["PackUomId"].Value);
				string BrandUom = Conversion.ToString(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["PackUom"].Value);
				int PackingTypeId = Conversion.ToInt(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["PackingTypeId"].Value);
				string PackingType = Conversion.ToString(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["PackingType"].Value);
				double BrandOuterQty = Conversion.ToDouble(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["OuterQty"].Value);
				double BrandInnerQty = Conversion.ToDouble(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["InnerQty"].Value);
				DataTable dtUom = new DataTable();
				dtUom.Columns.Add("BrandUomId", typeof(int));
				dtUom.Columns.Add("BrandUom", typeof(string));
				dtUom.Rows.Add(BrandUomId, BrandUom);
				DDL.BindDDL(dtUom, CmbBrandUomPm, "BrandUomId", "BrandUom", "BrandUom", ZeroIndex: false);
				((UltraGridBase)CmbBrandUomPm).Rows[0].Activate();
				DataTable dtPackingType = new DataTable();
				dtPackingType.Columns.Add("PackingTypeId", typeof(int));
				dtPackingType.Columns.Add("PackingType", typeof(string));
				dtPackingType.Rows.Add(PackingTypeId, PackingType);
				DDL.BindDDL(dtPackingType, CmbBrandPackingTypePm, "PackingTypeId", "PackingType", "PackingType", ZeroIndex: false);
				((UltraGridBase)CmbBrandPackingTypePm).Rows[0].Activate();
				txtBrandOuterQtyPm.Text = BrandOuterQty.ToString("#,##.##");
				txtBrandInnerQtyPm.Text = BrandInnerQty.ToString("#,##.##");
			}
			else
			{
				((UltraGridBase)CmbBrandUomPm).DataSource = null;
				((Control)(object)CmbBrandUomPm).Text = "";
				((UltraGridBase)CmbBrandPackingTypePm).DataSource = null;
				((Control)(object)CmbBrandPackingTypePm).Text = "";
				txtBrandOuterQtyPm.Text = 0.ToString("#,##.##");
				txtBrandInnerQtyPm.Text = 0.ToString("#,##.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void CheckBomHeaderIds(List<ExImLcOrderPackingDetail> dtDetail, DataTable dtPmDetail)
	{
		try
		{
			HashSet<int> DetailIds = (from row in dtDetail.AsEnumerable()
				where row.ExImItemId > 0
				select row.ExImItemId).ToHashSet();
			HashSet<int> PmIds = (from row in dtPmDetail.AsEnumerable()
				where row.Field<int>("BrandId") > 0
				select row.Field<int>("BrandId")).ToHashSet();
			List<int> missingInDetailIds = PmIds.Except(DetailIds).ToList();
			if (missingInDetailIds.Any())
			{
				List<string> missingBrandNames = (from row in dtPmDetail.AsEnumerable()
					where missingInDetailIds.Contains(row.Field<int>("BrandId"))
					select row.Field<string>("BrandName")).ToList();
				throw new Exception("Brand Items '" + string.Join(", ", missingBrandNames) + "' found in Packing Material Grid but not in Contract Detail Grid.");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (RecId == 0L)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			invoicePackingList obj = new invoicePackingList();
			if (RecId > 0)
			{
				obj.invoicePackingListId = RecId;
			}
			obj.invoicePackingListDetail = new List<invoicePackingList>();
			obj.invoiceMasterId = Conversion.ToInt(CmbInvoiceNo.Value);
			if (grdPackingMaterialDetail.RowCount > 0)
			{
				GridEXRow[] rows = grdPackingMaterialDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					invoicePackingList vd = new invoicePackingList();
					vd.invoicePackingListId = Conversion.ToInt(r.Cells["Id"].Value);
					vd.appActionId = ((vd.invoicePackingListId <= 0) ? 1 : 2);
					if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
					{
						throw new Exception($"Item Is Required In Packing Material Grid in Row No {r.RowIndex + 1}");
					}
					if (Conversion.ToInt(r.Cells["PackUomId"].Value) == 0)
					{
						throw new Exception($"PackUomId Is Required In Packing Material Grid in Row No {r.RowIndex + 1}");
					}
					if (Conversion.ToInt(r.Cells["Qty"].Value) == 0)
					{
						throw new Exception($"Qty Is Required In Packing Material Grid in Row No {r.RowIndex + 1}");
					}
					vd.containerNo = Conversion.ToString(r.Cells["ContainerNo"].Value);
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.ItemDescription = Conversion.ToString(r.Cells["ItemDescription"].Value);
					vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					vd.packSizeIdOuter = Conversion.ToInt(r.Cells["PackUomId"].Value);
					vd.packSizeIdinner = Conversion.ToInt(r.Cells["PackUomId"].Value);
					vd.qtyOuter = Conversion.ToDouble(r.Cells["Qty"].Value);
					vd.qtyInner = Conversion.ToDouble(r.Cells["Qty"].Value);
					vd.netWeightOuter = Conversion.ToDouble(r.Cells["Weight"].Value);
					vd.netWeightInner = Conversion.ToDouble(r.Cells["Weight"].Value);
					vd.NetWeightKgs = Conversion.ToDouble(r.Cells["Weight"].Value);
					vd.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.createdUserId = UserAccount.ID;
					vd.lastModifiedUserId = UserAccount.ID;
					obj.invoicePackingListDetail.Add(vd);
				}
				object success = invoicePackingList.Save(obj, (ParameterObject)null);
				if (RecId == 0L)
				{
					MessageBox.Show("Record Save Successfully");
				}
				else
				{
					MessageBox.Show("Record Update Successfully");
				}
				Reset();
				if (ChkPreview.Checked)
				{
					CommonServices.SaleContractReportsSlipNew501(Conversion.ToInt(success));
				}
				return;
			}
			throw new Exception("Packing Material Grid record not found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0L;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(long ID)
	{
		try
		{
			RecId = ID;
			List<invoicePackingList> lst = invoicePackingList.GetById(ID);
			if (lst != null && lst.Count > 0)
			{
				btnSave.Visible = false;
				btnUpdate.Visible = true;
				tabControl1.SelectedIndex = 0;
				CmbInvoiceNo.Value = ID;
				dtPmDetail.Rows.Clear();
				for (int i = 0; i < lst.Count; i++)
				{
					dtPmDetail.Rows.Add(lst[i].invoicePackingListId, lst[i].containerNo, lst[i].ItemId, lst[i].ItemCode, lst[i].ItemName, lst[i].ItemDescription, lst[i].JobLotId, lst[i].JobLot, lst[i].packSizeIdOuter, lst[i].PackUom, lst[i].qtyOuter, lst[i].NetWeightKgs, lst[i].RemarksDetail);
				}
				grdPackingMaterialDetail.DataSource = dtPmDetail;
				grdPackingMaterialDetail.RetrieveStructure();
				grdSettingsPM();
			}
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
			if (RecId == 0L)
			{
				throw new Exception("RecId not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmInvoicePackingDetail_FormClosing(object sender, FormClosingEventArgs e)
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

	private void Reset()
	{
		try
		{
			CurrentOrderDetailRecord.Clear();
			RecId = 0L;
			tabControl2.SelectedIndex = 0;
			((Control)(object)CmbInvoiceNo).Text = string.Empty;
			dtPmDetail.Rows.Clear();
			((UltraGridBase)cmbBrandForPmEntry).DataSource = null;
			ResetPmDetail();
			CmbInvoiceNo.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetPmDetail()
	{
		try
		{
			updateDetailIndexPmDetail = -1;
			((Control)(object)cmbBrandForPmEntry).Text = string.Empty;
			((UltraGridBase)CmbBrandUomPm).DataSource = null;
			((Control)(object)CmbBrandUomPm).Text = "";
			((UltraGridBase)CmbBrandPackingTypePm).DataSource = null;
			((Control)(object)CmbBrandPackingTypePm).Text = "";
			txtBrandOuterQtyPm.Text = 0.ToString("#,##.##");
			txtBrandInnerQtyPm.Text = 0.ToString("#,##.##");
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtWeight.Text = string.Empty;
			txtPackingItemRate.Text = string.Empty;
			txtPackingAmount.Text = string.Empty;
			txtPackingRemarks.Text = string.Empty;
			btnPackingAdd.Visible = true;
			btnPackingUpdate.Visible = false;
			btnPackingCancel.Visible = false;
			cmbBrandForPmEntry.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
		ResetPmDetail();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			InvoiceNoFill();
			PackingItemFill();
			JobLotFill();
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
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
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
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.ExporterId = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dt = invoiceMaster.SEARCHHistory(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("InvoiceNo", typeof(string));
				dtcol.Columns.Add("InvoiceDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser");
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser");
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtcol.Rows.Add(dt.Rows[i]["invoiceMasterId"], dt.Rows[i]["invoiceMasterNo"], Conversion.ToDateTime(dt.Rows[i]["docDate"]).ToShortDateString(), dt.Rows[i]["CreatedUserName"], dt.Rows[i]["CreatedOn"], dt.Rows[i]["LastModifiedUserName"], dt.Rows[i]["lastModifiedOn"]);
				}
				DataGridHistory.DataSource = dtcol;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DataGridHistory.DataSource = null;
				grdPMHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["InvoiceDate"].FormatString = "dd-MMM-yy";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["InvoiceNo"].Width = 130;
			DataGridHistory.RootTable.Columns["InvoiceDate"].Width = 80;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = 135;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = 145;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = 135;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = 145;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 40;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (int)r.RowType != 5 && r.Cells.Count > 0)
			{
				RecId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (int)r.RowType != 5 && r.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(long Id)
	{
		try
		{
			List<invoicePackingList> lst = invoicePackingList.GetById(Id);
			if (lst != null && lst.Count > 0)
			{
				DataTable dtPmH = new DataTable();
				dtPmH.Columns.Add("ContainerNo");
				dtPmH.Columns.Add("ItemCode");
				dtPmH.Columns.Add("ItemName");
				dtPmH.Columns.Add("ItemDescription");
				dtPmH.Columns.Add("JobLot");
				dtPmH.Columns.Add("PackUom");
				dtPmH.Columns.Add("Qty", typeof(double));
				dtPmH.Columns.Add("Weight", typeof(double));
				dtPmH.Columns.Add("Remarks");
				dtPmH.Clear();
				for (int i = 0; i < lst.Count; i++)
				{
					dtPmH.Rows.Add(lst[i].containerNo, lst[i].ItemCode, lst[i].ItemName, lst[i].ItemDescription, lst[i].JobLot, lst[i].PackUom, lst[i].qtyOuter, lst[i].NetWeightKgs, lst[i].RemarksDetail);
				}
				grdPMHistory.DataSource = dtPmH;
				grdPMHistory.RetrieveStructure();
				grdSettingsPMHistory();
			}
			else
			{
				grdPMHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPMHistory()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPMHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdPMHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdPMHistory.RootTable.Columns["ContainerNo"].Width = 75;
			grdPMHistory.RootTable.Columns["ItemCode"].Width = 65;
			grdPMHistory.RootTable.Columns["ItemName"].Width = 130;
			grdPMHistory.RootTable.Columns["ItemDescription"].Width = 130;
			grdPMHistory.RootTable.Columns["JobLot"].Width = 130;
			grdPMHistory.RootTable.Columns["PackUom"].Width = 70;
			grdPMHistory.RootTable.Columns["Qty"].Width = 70;
			grdPMHistory.RootTable.Columns["Weight"].Width = 70;
			grdPMHistory.RootTable.Columns["Remarks"].Width = 200;
			grdPMHistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
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
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 202);
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
			FromDateHistory.Focus();
			tabControl3.SelectedIndex = 0;
		}
		else
		{
			cmbBrandForPmEntry.Focus();
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			DataGridHistory.DataSource = null;
			grdPMHistory.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryCombosFill();
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
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryCombosFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Expected O, but got Unknown
				DataTable dataTable = new DataTable
				{
					Columns = 
					{
						{
							"Id",
							typeof(int)
						},
						{
							"Customer",
							typeof(string)
						}
					}
				};
				DataTable dataForDropDownFrom_ImportRelated = invoiceMaster.GetDataForDropDownFrom_ImportRelated(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				if (dataForDropDownFrom_ImportRelated.Rows.Count > 0)
				{
					for (int i = 0; i < dataForDropDownFrom_ImportRelated.Rows.Count; i++)
					{
						if (Conversion.ToString(dataForDropDownFrom_ImportRelated.Rows[i]["Activity"]) == "Customer")
						{
							dataTable.Rows.Add(dataForDropDownFrom_ImportRelated.Rows[i]["Id"], dataForDropDownFrom_ImportRelated.Rows[i]["name"]);
						}
					}
				}
				DDL.BindDDL(dataTable, CmbCustomerHistory, "Id", "Customer", "Party", ZeroIndex: false);
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (int)r.RowType != 5 && r.Cells.Count > 0)
			{
				_ = e.Column.Key == "Print";
				if (e.Column.Key == "Edit")
				{
					RecId = Conversion.ToInt(r.Cells["Id"].Value);
					ReadById(RecId);
				}
				_ = e.Column.Key == "Print523";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
	}

	private void frmInvoicePackingDetail_KeyDown(object sender, KeyEventArgs e)
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
					tabControl2.SelectedIndex = 0;
					cmbBrandForPmEntry.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (((e.Control && e.KeyCode == Keys.S) || (e.Control && e.KeyCode == Keys.U)) && btnUpdate.Visible && RecId > 0)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					Print_Click(null, null);
				}
				if (e.Control && e.KeyCode != Keys.NumPad1)
				{
					_ = e.KeyCode;
					_ = 49;
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					CmbInvoiceNo.Focus();
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbItem.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdPackingMaterialDetail).Focus();
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShowHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnResetHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Left)
			{
				if (((Control)(object)DataGridHistory).Focused)
				{
					((Control)(object)grdPMHistory).Focus();
				}
				else
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
			Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formrights.DoHaveUpdateRights)
				{
					Reset();
					RecId = Conversion.ToInt(r.Cells["Id"].Value);
					ReadById(RecId);
				}
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (DataGridHistory.CurrentColumn.Key == "Edit" && formrights.DoHaveUpdateRights)
				{
					Reset();
					RecId = Conversion.ToInt(r.Cells["Id"].Value);
					ReadById(RecId);
				}
				if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					if (!formrights.DoHavePrintRights)
					{
					}
				}
				else if (!(DataGridHistory.CurrentColumn.Key == "Print523") && DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 202);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+F1", "For Open Delivery Term Popup");
			dt.Rows.Add("Ctrl+F2", "For Open Payterm Popup");
			dt.Rows.Add("Ctrl+F3", "For Open Sea Ports Popup");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+L", "For Load All Records");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "For Moving In Detail Grids");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On First Column in Detail for Entry");
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

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl2.SelectedIndex == 2)
			{
				BindBrandItemsFromOutPutGrid();
				cmbBrandForPmEntry.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateWeight()
	{
		try
		{
			double Qty = Conversion.ToDouble((object)txtQty.Text);
			double PackUom = 0.0;
			if (((UltraGridBase)CmbPackUom).ActiveRow != null)
			{
				PackUom = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value);
			}
			if (Qty > 0.0 && PackUom > 0.0)
			{
				txtWeight.Text = (Qty * PackUom).ToString("#,##0.###");
			}
			else
			{
				txtWeight.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtIPackingtemQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double Qty = Conversion.ToDouble((object)txtWeight.Text);
			CalculatePackingAmount(Qty);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingItemRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double Qty = 0.0;
			Qty = ((!(Conversion.ToDouble((object)txtQty.Text) > 0.0)) ? Conversion.ToDouble((object)txtWeight.Text) : Conversion.ToDouble((object)txtQty.Text));
			CalculatePackingAmount(Qty);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculatePackingAmount(double Qty)
	{
		try
		{
			if (Qty > 0.0 && Conversion.ToDouble((object)txtPackingItemRate.Text) > 0.0)
			{
				double amount = Qty * Conversion.ToDouble((object)txtPackingItemRate.Text);
				txtPackingAmount.Text = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtPackingAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
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
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingItemRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtInnerQtyPm_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBrandInnerQtyPm_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBrandOuterQtyPm_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0L;
			Insert();
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
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
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
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Expected O, but got Unknown
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Expected O, but got Unknown
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Expected O, but got Unknown
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Expected O, but got Unknown
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Expected O, but got Unknown
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Expected O, but got Unknown
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Expected O, but got Unknown
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Expected O, but got Unknown
		//IL_0da2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dac: Expected O, but got Unknown
		//IL_4be9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bf3: Expected O, but got Unknown
		//IL_4c17: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c21: Expected O, but got Unknown
		GridEXLayout grdPackingMaterialDetail_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Import.Transactions.frmInvoicePackingDetail));
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
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
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
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
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPMHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPackingMaterialDetail = new System.Windows.Forms.TabPage();
		this.grdPackingMaterialDetail = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label56 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label94 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.txtItemDescription = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtContainerNo = new System.Windows.Forms.TextBox();
		this.btnPackingAdd = new System.Windows.Forms.Button();
		this.cmbBrandForPmEntry = new UltraCombo();
		this.panel30 = new System.Windows.Forms.Panel();
		this.radBrandCode = new System.Windows.Forms.RadioButton();
		this.label47 = new System.Windows.Forms.Label();
		this.radBrandName = new System.Windows.Forms.RadioButton();
		this.CmbItem = new UltraCombo();
		this.rdPackingItemCode = new System.Windows.Forms.RadioButton();
		this.label58 = new System.Windows.Forms.Label();
		this.rdPackingItemName = new System.Windows.Forms.RadioButton();
		this.label60 = new System.Windows.Forms.Label();
		this.btnPackingCancel = new System.Windows.Forms.Button();
		this.label61 = new System.Windows.Forms.Label();
		this.btnPackingUpdate = new System.Windows.Forms.Button();
		this.label62 = new System.Windows.Forms.Label();
		this.txtPackingRemarks = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.txtPackingAmount = new System.Windows.Forms.TextBox();
		this.label64 = new System.Windows.Forms.Label();
		this.txtPackingItemRate = new System.Windows.Forms.TextBox();
		this.label81 = new System.Windows.Forms.Label();
		this.txtBrandInnerQtyPm = new System.Windows.Forms.TextBox();
		this.label78 = new System.Windows.Forms.Label();
		this.txtBrandOuterQtyPm = new System.Windows.Forms.TextBox();
		this.label79 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label77 = new System.Windows.Forms.Label();
		this.txtWeight = new System.Windows.Forms.TextBox();
		this.CmbBrandPackingTypePm = new UltraCombo();
		this.CmbPackUom = new UltraCombo();
		this.label80 = new System.Windows.Forms.Label();
		this.CmbBrandUomPm = new UltraCombo();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label28 = new System.Windows.Forms.Label();
		this.CmbInvoiceNo = new UltraCombo();
		this.panel28 = new System.Windows.Forms.Panel();
		this.ChkPreview = new System.Windows.Forms.CheckBox();
		this.label66 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabPmH = new System.Windows.Forms.TabPage();
		this.grdPMHistory = new GridEX();
		this.panel32 = new System.Windows.Forms.Panel();
		this.label75 = new System.Windows.Forms.Label();
		this.panel29 = new System.Windows.Forms.Panel();
		this.label65 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel18 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label67 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label73 = new System.Windows.Forms.Label();
		this.label89 = new System.Windows.Forms.Label();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label52 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPackingMaterialDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialDetail).BeginInit();
		this.panel16.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBrandForPmEntry).BeginInit();
		this.panel30.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandPackingTypePm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomPm).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).BeginInit();
		this.panel28.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.tabControl3.SuspendLayout();
		this.tabPmH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPMHistory).BeginInit();
		this.panel32.SuspendLayout();
		this.panel29.SuspendLayout();
		this.panel18.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel17.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(875, 711);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(875, 711);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(867, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(861, 676);
		this.panel2.TabIndex = 0;
		this.panel5.Controls.Add(this.tabControl2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 105);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(861, 571);
		this.panel5.TabIndex = 2;
		this.tabControl2.Controls.Add(this.tabPackingMaterialDetail);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(861, 571);
		this.tabControl2.TabIndex = 0;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabPackingMaterialDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail);
		this.tabPackingMaterialDetail.Controls.Add(this.panel16);
		this.tabPackingMaterialDetail.Controls.Add(this.panel15);
		this.tabPackingMaterialDetail.Location = new System.Drawing.Point(4, 25);
		this.tabPackingMaterialDetail.Margin = new System.Windows.Forms.Padding(2);
		this.tabPackingMaterialDetail.Name = "tabPackingMaterialDetail";
		this.tabPackingMaterialDetail.Size = new System.Drawing.Size(853, 542);
		this.tabPackingMaterialDetail.TabIndex = 3;
		this.tabPackingMaterialDetail.Text = "Packing Material Detail";
		this.tabPackingMaterialDetail.UseVisualStyleBackColor = true;
		this.grdPackingMaterialDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPackingMaterialDetail).BackColor = System.Drawing.Color.White;
		this.grdPackingMaterialDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPackingMaterialDetail_DesignTimeLayout.LayoutString = resources.GetString("grdPackingMaterialDetail_DesignTimeLayout.LayoutString");
		this.grdPackingMaterialDetail.DesignTimeLayout = grdPackingMaterialDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPackingMaterialDetail.FilterMode = (FilterMode)2;
		this.grdPackingMaterialDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterialDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Location = new System.Drawing.Point(0, 126);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Name = "grdPackingMaterialDetail";
		((ScrollableBase)this.grdPackingMaterialDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Size = new System.Drawing.Size(853, 416);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).TabIndex = 2;
		this.grdPackingMaterialDetail.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterialDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterialDetail).VisualStyle = (VisualStyle)4;
		this.grdPackingMaterialDetail.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterialDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).DoubleClick += new System.EventHandler(grdPackingMaterialDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPackingMaterialDetail_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label56);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 105);
		this.panel16.Margin = new System.Windows.Forms.Padding(2);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(853, 21);
		this.panel16.TabIndex = 1;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label56.ForeColor = System.Drawing.Color.White;
		this.label56.Location = new System.Drawing.Point(3, 2);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(46, 17);
		this.label56.TabIndex = 560;
		this.label56.Text = "Detail";
		this.panel15.Controls.Add(this.label94);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel15.Controls.Add(this.label2);
		this.panel15.Controls.Add(this.txtItemDescription);
		this.panel15.Controls.Add(this.label1);
		this.panel15.Controls.Add(this.txtContainerNo);
		this.panel15.Controls.Add(this.btnPackingAdd);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBrandForPmEntry);
		this.panel15.Controls.Add(this.panel30);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.panel15.Controls.Add(this.rdPackingItemCode);
		this.panel15.Controls.Add(this.label58);
		this.panel15.Controls.Add(this.rdPackingItemName);
		this.panel15.Controls.Add(this.label60);
		this.panel15.Controls.Add(this.btnPackingCancel);
		this.panel15.Controls.Add(this.label61);
		this.panel15.Controls.Add(this.btnPackingUpdate);
		this.panel15.Controls.Add(this.label62);
		this.panel15.Controls.Add(this.txtPackingRemarks);
		this.panel15.Controls.Add(this.label63);
		this.panel15.Controls.Add(this.txtPackingAmount);
		this.panel15.Controls.Add(this.label64);
		this.panel15.Controls.Add(this.txtPackingItemRate);
		this.panel15.Controls.Add(this.label81);
		this.panel15.Controls.Add(this.txtBrandInnerQtyPm);
		this.panel15.Controls.Add(this.label78);
		this.panel15.Controls.Add(this.txtBrandOuterQtyPm);
		this.panel15.Controls.Add(this.label79);
		this.panel15.Controls.Add(this.txtQty);
		this.panel15.Controls.Add(this.label77);
		this.panel15.Controls.Add(this.txtWeight);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePm);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.panel15.Controls.Add(this.label80);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandUomPm);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Margin = new System.Windows.Forms.Padding(2);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(853, 105);
		this.panel15.TabIndex = 0;
		this.label94.AutoSize = true;
		this.label94.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label94.Location = new System.Drawing.Point(526, 6);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(46, 15);
		this.label94.TabIndex = 582;
		this.label94.Text = "Job Lot";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance180;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(524, 27);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(211, 25);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(318, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(68, 15);
		this.label2.TabIndex = 580;
		this.label2.Text = "Description";
		this.txtItemDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemDescription.Location = new System.Drawing.Point(315, 28);
		this.txtItemDescription.Name = "txtItemDescription";
		this.txtItemDescription.Size = new System.Drawing.Size(208, 23);
		this.txtItemDescription.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(77, 15);
		this.label1.TabIndex = 578;
		this.label1.Text = "Container No";
		this.txtContainerNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContainerNo.Location = new System.Drawing.Point(6, 28);
		this.txtContainerNo.Name = "txtContainerNo";
		this.txtContainerNo.Size = new System.Drawing.Size(104, 23);
		this.txtContainerNo.TabIndex = 0;
		this.btnPackingAdd.BackColor = System.Drawing.Color.Teal;
		this.btnPackingAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPackingAdd.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPackingAdd.ForeColor = System.Drawing.Color.White;
		this.btnPackingAdd.Location = new System.Drawing.Point(701, 75);
		this.btnPackingAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnPackingAdd.Name = "btnPackingAdd";
		this.btnPackingAdd.Size = new System.Drawing.Size(34, 23);
		this.btnPackingAdd.TabIndex = 8;
		this.btnPackingAdd.Text = "+";
		this.btnPackingAdd.UseVisualStyleBackColor = false;
		this.btnPackingAdd.Click += new System.EventHandler(btnPackingAdd_Click);
		this.btnPackingAdd.Leave += new System.EventHandler(btnPackingAdd_Leave);
		this.cmbBrandForPmEntry.AllowNull = (DefaultableBoolean)1;
		this.cmbBrandForPmEntry.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBrandForPmEntry.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBrandForPmEntry.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBrandForPmEntry).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBrandForPmEntry.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBrandForPmEntry).Location = new System.Drawing.Point(6, 214);
		((System.Windows.Forms.Control)(object)this.cmbBrandForPmEntry).Name = "cmbBrandForPmEntry";
		((System.Windows.Forms.Control)(object)this.cmbBrandForPmEntry).Size = new System.Drawing.Size(215, 26);
		((System.Windows.Forms.Control)(object)this.cmbBrandForPmEntry).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBrandForPmEntry).Leave += new System.EventHandler(cmbBrandForPmEntry_Leave);
		this.panel30.Controls.Add(this.radBrandCode);
		this.panel30.Controls.Add(this.label47);
		this.panel30.Controls.Add(this.radBrandName);
		this.panel30.Location = new System.Drawing.Point(7, 188);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(183, 24);
		this.panel30.TabIndex = 576;
		this.radBrandCode.AutoSize = true;
		this.radBrandCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radBrandCode.Location = new System.Drawing.Point(99, 4);
		this.radBrandCode.Name = "radBrandCode";
		this.radBrandCode.Size = new System.Drawing.Size(52, 19);
		this.radBrandCode.TabIndex = 0;
		this.radBrandCode.Text = "Code";
		this.radBrandCode.UseVisualStyleBackColor = true;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(3, 6);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(38, 15);
		this.label47.TabIndex = 560;
		this.label47.Text = "Brand";
		this.radBrandName.AutoSize = true;
		this.radBrandName.Checked = true;
		this.radBrandName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radBrandName.Location = new System.Drawing.Point(43, 4);
		this.radBrandName.Name = "radBrandName";
		this.radBrandName.Size = new System.Drawing.Size(57, 19);
		this.radBrandName.TabIndex = 574;
		this.radBrandName.TabStop = true;
		this.radBrandName.Text = "Name";
		this.radBrandName.UseVisualStyleBackColor = true;
		this.radBrandName.CheckedChanged += new System.EventHandler(rabBrandName_CheckedChanged);
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(112, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbItem).Leave += new System.EventHandler(CmbPMItem_Leave);
		this.rdPackingItemCode.AutoSize = true;
		this.rdPackingItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdPackingItemCode.Location = new System.Drawing.Point(217, 4);
		this.rdPackingItemCode.Name = "rdPackingItemCode";
		this.rdPackingItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdPackingItemCode.TabIndex = 575;
		this.rdPackingItemCode.Text = "Code";
		this.rdPackingItemCode.UseVisualStyleBackColor = true;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(112, 6);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(32, 15);
		this.label58.TabIndex = 560;
		this.label58.Text = "Item";
		this.rdPackingItemName.AutoSize = true;
		this.rdPackingItemName.Checked = true;
		this.rdPackingItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdPackingItemName.Location = new System.Drawing.Point(148, 4);
		this.rdPackingItemName.Name = "rdPackingItemName";
		this.rdPackingItemName.Size = new System.Drawing.Size(57, 19);
		this.rdPackingItemName.TabIndex = 574;
		this.rdPackingItemName.TabStop = true;
		this.rdPackingItemName.Text = "Name";
		this.rdPackingItemName.UseVisualStyleBackColor = true;
		this.rdPackingItemName.CheckedChanged += new System.EventHandler(rdPackingItemName_CheckedChanged);
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(218, 56);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(52, 15);
		this.label60.TabIndex = 562;
		this.label60.Text = "Remarks";
		this.btnPackingCancel.BackColor = System.Drawing.Color.Teal;
		this.btnPackingCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPackingCancel.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPackingCancel.ForeColor = System.Drawing.Color.White;
		this.btnPackingCancel.Location = new System.Drawing.Point(767, 75);
		this.btnPackingCancel.Margin = new System.Windows.Forms.Padding(2);
		this.btnPackingCancel.Name = "btnPackingCancel";
		this.btnPackingCancel.Size = new System.Drawing.Size(66, 23);
		this.btnPackingCancel.TabIndex = 10;
		this.btnPackingCancel.Text = "Cancel";
		this.btnPackingCancel.UseVisualStyleBackColor = false;
		this.btnPackingCancel.Visible = false;
		this.btnPackingCancel.Click += new System.EventHandler(btnPackingCancel_Click);
		this.btnPackingCancel.Leave += new System.EventHandler(btnPackingCancel_Leave);
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(801, 199);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(51, 15);
		this.label61.TabIndex = 563;
		this.label61.Text = "Amount";
		this.btnPackingUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnPackingUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPackingUpdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPackingUpdate.ForeColor = System.Drawing.Color.White;
		this.btnPackingUpdate.Location = new System.Drawing.Point(701, 75);
		this.btnPackingUpdate.Margin = new System.Windows.Forms.Padding(2);
		this.btnPackingUpdate.Name = "btnPackingUpdate";
		this.btnPackingUpdate.Size = new System.Drawing.Size(67, 23);
		this.btnPackingUpdate.TabIndex = 9;
		this.btnPackingUpdate.Text = "Update";
		this.btnPackingUpdate.UseVisualStyleBackColor = false;
		this.btnPackingUpdate.Visible = false;
		this.btnPackingUpdate.Click += new System.EventHandler(btnPackingUpdate_Click);
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(695, 199);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(58, 15);
		this.label62.TabIndex = 564;
		this.label62.Text = "Item Rate";
		this.txtPackingRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingRemarks.Location = new System.Drawing.Point(217, 75);
		this.txtPackingRemarks.Name = "txtPackingRemarks";
		this.txtPackingRemarks.Size = new System.Drawing.Size(484, 23);
		this.txtPackingRemarks.TabIndex = 7;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(10, 56);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(26, 15);
		this.label63.TabIndex = 565;
		this.label63.Text = "Qty";
		this.txtPackingAmount.Enabled = false;
		this.txtPackingAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingAmount.Location = new System.Drawing.Point(801, 216);
		this.txtPackingAmount.Name = "txtPackingAmount";
		this.txtPackingAmount.Size = new System.Drawing.Size(109, 23);
		this.txtPackingAmount.TabIndex = 10;
		this.txtPackingAmount.Text = "0";
		this.txtPackingAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(326, 193);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(111, 15);
		this.label64.TabIndex = 566;
		this.label64.Text = "Brand Packing Type\r\n";
		this.txtPackingItemRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingItemRate.Location = new System.Drawing.Point(695, 216);
		this.txtPackingItemRate.Name = "txtPackingItemRate";
		this.txtPackingItemRate.Size = new System.Drawing.Size(104, 23);
		this.txtPackingItemRate.TabIndex = 9;
		this.txtPackingItemRate.Text = "0";
		this.txtPackingItemRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingItemRate.TextChanged += new System.EventHandler(txtPackingItemRate_TextChanged);
		this.txtPackingItemRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackingItemRate_KeyPress);
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label81.Location = new System.Drawing.Point(114, 56);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(46, 15);
		this.label81.TabIndex = 565;
		this.label81.Text = "Weight";
		this.txtBrandInnerQtyPm.Enabled = false;
		this.txtBrandInnerQtyPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrandInnerQtyPm.Location = new System.Drawing.Point(581, 216);
		this.txtBrandInnerQtyPm.Name = "txtBrandInnerQtyPm";
		this.txtBrandInnerQtyPm.Size = new System.Drawing.Size(111, 23);
		this.txtBrandInnerQtyPm.TabIndex = 4;
		this.txtBrandInnerQtyPm.Text = "0";
		this.txtBrandInnerQtyPm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBrandInnerQtyPm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBrandInnerQtyPm_KeyPress);
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.Location = new System.Drawing.Point(468, 193);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(93, 15);
		this.label78.TabIndex = 565;
		this.label78.Text = "Brand Outer Qty";
		this.txtBrandOuterQtyPm.Enabled = false;
		this.txtBrandOuterQtyPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrandOuterQtyPm.Location = new System.Drawing.Point(468, 216);
		this.txtBrandOuterQtyPm.Name = "txtBrandOuterQtyPm";
		this.txtBrandOuterQtyPm.Size = new System.Drawing.Size(111, 23);
		this.txtBrandOuterQtyPm.TabIndex = 3;
		this.txtBrandOuterQtyPm.Text = "0";
		this.txtBrandOuterQtyPm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBrandOuterQtyPm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBrandOuterQtyPm_KeyPress);
		this.label79.AutoSize = true;
		this.label79.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label79.Location = new System.Drawing.Point(581, 193);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(91, 15);
		this.label79.TabIndex = 565;
		this.label79.Text = "Brand Inner Qty";
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(6, 75);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(104, 23);
		this.txtQty.TabIndex = 5;
		this.txtQty.Text = "0";
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(225, 193);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(67, 15);
		this.label77.TabIndex = 566;
		this.label77.Text = "Brand Uom";
		this.txtWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeight.Location = new System.Drawing.Point(111, 75);
		this.txtWeight.Name = "txtWeight";
		this.txtWeight.Size = new System.Drawing.Size(104, 23);
		this.txtWeight.TabIndex = 6;
		this.txtWeight.Text = "0";
		this.txtWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeight.TextChanged += new System.EventHandler(txtIPackingtemQty_TextChanged);
		this.txtWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtInnerQtyPm_KeyPress);
		this.CmbBrandPackingTypePm.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandPackingTypePm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandPackingTypePm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandPackingTypePm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance147;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance152).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePm).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandPackingTypePm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePm).Location = new System.Drawing.Point(324, 214);
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePm).Name = "CmbBrandPackingTypePm";
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePm).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePm).TabIndex = 2;
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance195;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(736, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(97, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TextChanged += new System.EventHandler(CmbPackUom_TextChanged);
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label80.Location = new System.Drawing.Point(738, 6);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(61, 15);
		this.label80.TabIndex = 566;
		this.label80.Text = "Pack Uom";
		this.CmbBrandUomPm.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandUomPm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandUomPm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandUomPm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance159;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPm).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandUomPm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPm).Location = new System.Drawing.Point(223, 214);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPm).Name = "CmbBrandUomPm";
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPm).Size = new System.Drawing.Size(99, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPm).TabIndex = 1;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.panel28);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(861, 75);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNo);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(7, 28);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(463, 44);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(7, 19);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(65, 15);
		this.label28.TabIndex = 465;
		this.label28.Text = "Invoice No";
		this.CmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Location = new System.Drawing.Point(74, 13);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Name = "CmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Size = new System.Drawing.Size(384, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Leave += new System.EventHandler(CmbSalesContractNo_Leave);
		this.panel28.BackColor = System.Drawing.Color.Teal;
		this.panel28.Controls.Add(this.ChkPreview);
		this.panel28.Controls.Add(this.label66);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel28.Location = new System.Drawing.Point(0, 0);
		this.panel28.Margin = new System.Windows.Forms.Padding(2);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(861, 26);
		this.panel28.TabIndex = 1;
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(774, 3);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(84, 20);
		this.ChkPreview.TabIndex = 1;
		this.ChkPreview.Text = "Preview";
		this.ChkPreview.UseVisualStyleBackColor = true;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.ForeColor = System.Drawing.Color.White;
		this.label66.Location = new System.Drawing.Point(2, 3);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(221, 19);
		this.label66.TabIndex = 561;
		this.label66.Text = "Packing Detail By Invoice ";
		this.panel3.Controls.Add(this.toolstrip);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(861, 30);
		this.panel3.TabIndex = 0;
		this.toolstrip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnnew, this.BtnRefresh, this.btnSave, this.btnUpdate, this.Print, this.btnShortCutKey });
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolstrip.Size = new System.Drawing.Size(861, 27);
		this.toolstrip.TabIndex = 5;
		this.toolstrip.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "501-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.tabControl3);
		this.tabPage2.Controls.Add(this.panel29);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Controls.Add(this.panel17);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1226, 682);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = resources.GetString("DataGridHistory_DesignTimeLayout.LayoutString");
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 152);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1220, 270);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.tabControl3.Controls.Add(this.tabPmH);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tabControl3.Location = new System.Drawing.Point(3, 422);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1220, 257);
		this.tabControl3.TabIndex = 349;
		this.tabPmH.Controls.Add((System.Windows.Forms.Control)(object)this.grdPMHistory);
		this.tabPmH.Controls.Add(this.panel32);
		this.tabPmH.Location = new System.Drawing.Point(4, 25);
		this.tabPmH.Name = "tabPmH";
		this.tabPmH.Size = new System.Drawing.Size(1212, 228);
		this.tabPmH.TabIndex = 2;
		this.tabPmH.Text = "Packing Material";
		this.tabPmH.UseVisualStyleBackColor = true;
		this.grdPMHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPMHistory).BackColor = System.Drawing.Color.White;
		this.grdPMHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPMHistory_DesignTimeLayout.LayoutString = resources.GetString("grdPMHistory_DesignTimeLayout.LayoutString");
		this.grdPMHistory.DesignTimeLayout = grdPMHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPMHistory.FilterMode = (FilterMode)2;
		this.grdPMHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPMHistory.GroupByBoxVisible = false;
		this.grdPMHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Name = "grdPMHistory";
		((ScrollableBase)this.grdPMHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Size = new System.Drawing.Size(1212, 203);
		((System.Windows.Forms.Control)(object)this.grdPMHistory).TabIndex = 347;
		this.grdPMHistory.TotalRow = (InheritableBoolean)1;
		this.grdPMHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPMHistory).VisualStyle = (VisualStyle)4;
		this.panel32.BackColor = System.Drawing.Color.Teal;
		this.panel32.Controls.Add(this.label75);
		this.panel32.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel32.Location = new System.Drawing.Point(0, 0);
		this.panel32.Name = "panel32";
		this.panel32.Size = new System.Drawing.Size(1212, 25);
		this.panel32.TabIndex = 348;
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label75.ForeColor = System.Drawing.Color.White;
		this.label75.Location = new System.Drawing.Point(5, 5);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(343, 17);
		this.label75.TabIndex = 562;
		this.label75.Text = "Packing Material Detail of the selected above row";
		this.panel29.BackColor = System.Drawing.Color.Teal;
		this.panel29.Controls.Add(this.label65);
		this.panel29.Controls.Add(this.ctrlGrdBar1);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel29.Location = new System.Drawing.Point(3, 128);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(1220, 24);
		this.panel29.TabIndex = 348;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.White;
		this.label65.Location = new System.Drawing.Point(4, 2);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(142, 19);
		this.label65.TabIndex = 562;
		this.label65.Text = "Filtered Records";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1169, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(48, 23);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel18.Controls.Add(this.groupBox5);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 56);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1220, 72);
		this.panel18.TabIndex = 0;
		this.groupBox5.Controls.Add(this.rdapproveddate);
		this.groupBox5.Controls.Add(this.btnShowHistory);
		this.groupBox5.Controls.Add(this.rdmodifydate);
		this.groupBox5.Controls.Add(this.label67);
		this.groupBox5.Controls.Add(this.drdocdate);
		this.groupBox5.Controls.Add(this.rdentrydate);
		this.groupBox5.Controls.Add(this.label73);
		this.groupBox5.Controls.Add(this.label89);
		this.groupBox5.Controls.Add(this.label90);
		this.groupBox5.Controls.Add(this.label91);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox5.Controls.Add(this.ToDocNoHistory);
		this.groupBox5.Controls.Add(this.FromDocNoHistory);
		this.groupBox5.Controls.Add(this.ToDateHistory);
		this.groupBox5.Controls.Add(this.FromDateHistory);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(4, 0);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(923, 71);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(816, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdapproveddate.Visible = false;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(668, 40);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(816, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(376, 20);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(51, 15);
		this.label67.TabIndex = 87;
		this.label67.Text = "Exporter";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(735, 18);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(735, 44);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(213, 46);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(64, 15);
		this.label73.TabIndex = 86;
		this.label73.Text = "To Doc No";
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(213, 20);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(79, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "From Doc No";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(3, 46);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(3, 20);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance183;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance188).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(434, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(296, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(294, 42);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(294, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(69, 42);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label52);
		this.panel17.Controls.Add(this.toolStrip3);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 3);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1220, 53);
		this.panel17.TabIndex = 0;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.White;
		this.label52.Location = new System.Drawing.Point(5, 30);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(285, 19);
		this.label52.TabIndex = 562;
		this.label52.Text = "Invoice History For Packing Detail";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip3.Size = new System.Drawing.Size(1220, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(875, 711);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmInvoicePackingDetail";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmInvoicePackingDetail_FormClosing);
		base.Load += new System.EventHandler(frmInvoicePackingDetail_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmInvoicePackingDetail_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPackingMaterialDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialDetail).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBrandForPmEntry).EndInit();
		this.panel30.ResumeLayout(false);
		this.panel30.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandPackingTypePm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomPm).EndInit();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).EndInit();
		this.panel28.ResumeLayout(false);
		this.panel28.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.tabControl3.ResumeLayout(false);
		this.tabPmH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPMHistory).EndInit();
		this.panel32.ResumeLayout(false);
		this.panel32.PerformLayout();
		this.panel29.ResumeLayout(false);
		this.panel29.PerformLayout();
		this.panel18.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
