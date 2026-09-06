// using Architecture.Reports;
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
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.Export
{

public class frmExportContractSchedule : Form
{
	private bool UpdateModeMain;

	private bool FormInitialized;

	private bool UpdateModeDetail;

	private bool ShipmentDocumentsFeature;

	private bool skipFCLContractValidationOnSchedule;

	private DataTable dtPort = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable tableScheduleMain = new DataTable();

	private int CurrContractId;

	private int RecordIdentityId;

	private double MtonByAttentiveDate;

	private DateTime LastScheduleLoadingDate = DateTime.Now;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string ScreenName = "frmExportContractSchedule";

	private LoadContractForSchedule Loader;

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private List<ExportContractSchedule> LstRemoveRecordMain = new List<ExportContractSchedule>();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private Panel panel12;

	private Panel panel13;

	private Label label14;

	private ToolStripButton btnattachment;

	private TabPage tabPage3;

	internal GridEX DataGridHistory;

	private Panel panel24;

	private CtrlGrdBar ctrlGrdBar7;

	private Label label5;

	private ToolStrip toolStrip4;

	private ToolStripButton btnLoadAllHistoryNew;

	private ToolStripButton BtnHistoryPrint;

	private Panel panel25;

	private GroupBox groupBox2;

	private Label label10;

	internal UltraCombo cmbPort;

	private DateTimePicker txtdateto;

	private Label label50;

	private Label label11;

	internal UltraCombo cmbSupplierName;

	private DateTimePicker txtdatefrom;

	private Label label19;

	private Label label51;

	internal UltraCombo CmbItemHistory;

	private Button btnShow;

	private Panel panel26;

	private Panel panel27;

	private ToolStripButton btnRefreshHistoryCombos;

	private ToolStripButton BtnPrint551_01;

	private ToolStripButton BtnRefreshScheduleMain;

	private ToolStripButton btnLoad;

	internal GridEX grdContractShedule;

	private CtrlGrdBar ctrlGrdBar1;

	private RadioButton rdmodifydate;

	private RadioButton rdLoadingDate;

	private RadioButton rdentrydate;

	public frmExportContractSchedule(UserAccount userAccount)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			FormInitialized = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 800);
			tableScheduleMain.Columns.Add("Id", typeof(int));
			tableScheduleMain.Columns.Add("ContractId", typeof(int));
			tableScheduleMain.Columns.Add("ContractDetailId", typeof(int));
			tableScheduleMain.Columns.Add("ScheduleNo", typeof(string));
			tableScheduleMain.Columns.Add("SupplierCustomerId", typeof(int));
			tableScheduleMain.Columns.Add("CustomerName", typeof(string));
			tableScheduleMain.Columns.Add("ContractNo", typeof(string));
			tableScheduleMain.Columns.Add("ItemId", typeof(int));
			tableScheduleMain.Columns.Add("Product", typeof(string));
			tableScheduleMain.Columns.Add("WeightM_Ton", typeof(double));
			tableScheduleMain.Columns.Add("PackUomId", typeof(int));
			tableScheduleMain.Columns.Add("PackUom", typeof(string));
			tableScheduleMain.Columns.Add("PackEquivalent", typeof(double));
			tableScheduleMain.Columns.Add("NoOfBags", typeof(double));
			tableScheduleMain.Columns.Add("NoOfContainers", typeof(double));
			tableScheduleMain.Columns.Add("PackingType", typeof(string));
			tableScheduleMain.Columns.Add("DestinationPortId", typeof(int));
			tableScheduleMain.Columns.Add("AttentiveLoadingDate", typeof(DateTime));
			tableScheduleMain.Columns.Add("Remarks", typeof(string));
			tableScheduleMain.Columns.Add("BalMTon", typeof(double));
			tableScheduleMain.Columns.Add("Status", typeof(string));
			tableScheduleMain.Columns.Add("ContractMTon", typeof(double));
			tableScheduleMain.Columns.Add("ContractFcl", typeof(double));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			DataTable historyComboData = HistoryComboDbCall();
			GenerateIdentity();
			return new
			{
				HistoryComboData = historyComboData
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
				btnsave.Enabled = formright.DoHaveSaveRight;
				ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
				ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
				ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
				ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
				ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
				ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
				GetConfigurationsFromGlobal();
				dtPortFill();
				if (tableScheduleMain != null && tableScheduleMain.Columns.Count > 0)
				{
					grdContractShedule.DataSource = tableScheduleMain;
					grdContractShedule.RetrieveStructure();
					grdContractSheduleSettings();
				}
				ComboBindForHistory(result.HistoryComboData);
				txtdatefrom.Value = DateTime.Now.AddDays(-3.0);
				txtdateto.Value = DateTime.Now;
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			skipFCLContractValidationOnSchedule = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("skipFCLContractValidationOnSchedule"));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void frmSaleContractSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1450, 700);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GenerateIdentity()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			int Id = ExportContractSchedule.GenerateRecordIdentityId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 247
			});
			if (Id > 0)
			{
				RecordIdentityId = Id;
			}
			else
			{
				RecordIdentityId = 0;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemPackingTypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			ExImPackMaterilaType.Getall(new ExImPackMaterilaType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dtPortFill()
	{
		try
		{
			dtPort = CommonServices.GetSeaPortForComboServiceBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private DataTable HistoryComboDbCall()
	{
		try
		{
			return ExportContractSchedule.GetDataForDropDownFromSchedule(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null, (string)null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ComboBindForHistory(DataTable dt)
	{
		try
		{
			DataTable dtPort = new DataTable();
			dtPort.Columns.Add("Id", typeof(int));
			dtPort.Columns.Add("Name");
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("Name");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "DestinationPort")
					{
						dtPort.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "CustomerName")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
					{
						dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtPort, cmbPort, "Id", "Name", "Port Name", ZeroIndex: false);
			DDL.BindDDL(dtCustomer, cmbSupplierName, "Id", "Name", "Customer Name", ZeroIndex: false);
			DDL.BindDDL(dtItem, CmbItemHistory, "Id", "Name", "Item Name", ZeroIndex: false);
			((UltraGridBase)cmbPort).DisplayLayout.Bands[0].Columns[1].Width = 250;
			((UltraGridBase)cmbSupplierName).DisplayLayout.Bands[0].Columns[1].Width = 250;
			((UltraGridBase)CmbItemHistory).DisplayLayout.Bands[0].Columns[1].Width = 250;
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
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FormReset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			RecordIdentityId = 0;
			GenerateIdentity();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			btnsave.Visible = true;
			UpdateModeMain = false;
			grdContractShedule.DataSource = null;
			tableScheduleMain.Clear();
			LstRemoveRecordMain.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnNewMain_Click(object sender, EventArgs e)
	{
		try
		{
			AT = new Attachment();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnRefreshScheduleMain_Click(object sender, EventArgs e)
	{
		try
		{
			skipFCLContractValidationOnSchedule = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("skipFCLContractValidationOnSchedule"));
			dtPortFill();
			ContractScheduleGridComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnRefreshScheduleDetail_Click(object sender, EventArgs e)
	{
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ContractScheduleGridComboFill()
	{
		try
		{
			grdContractShedule.RootTable.Columns["DestinationPortId"].ColumnType = (ColumnType)0;
			grdContractShedule.RootTable.Columns["DestinationPortId"].HasValueList = true;
			grdContractShedule.RootTable.Columns["DestinationPortId"].LimitToList = true;
			grdContractShedule.RootTable.Columns["DestinationPortId"].Caption = "Destination Port";
			grdContractShedule.RootTable.Columns["DestinationPortId"].ValueList.PopulateValueList((IEnumerable)dtPort.DefaultView, "Id", "PortName");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdContractSheduleSettings()
	{
		try
		{
			ContractScheduleGridComboFill();
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"DestinationPortId",
					(EditType)4
				},
				{
					"WeightM_Ton",
					(EditType)1
				},
				{
					"NoOfBags",
					(EditType)1
				},
				{
					"NoOfContainers",
					(EditType)1
				},
				{
					"AttentiveLoadingDate",
					(EditType)2
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			List<string> hiddenColumns = new List<string> { "Id", "ContractId", "ContractDetailId", "SupplierCustomerId", "ItemId", "PackUomId", "PackEquivalent", "BalMTon", "ContractMTon", "ContractFcl" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdContractShedule, 2, 3, hiddenColumns, editableColumns);
			grdContractShedule.RootTable.Columns["ScheduleNo"].Caption = "Record Identity";
			grdContractShedule.RootTable.Columns["ScheduleNo"].Width = Constants.ExportConstants.DocNoConstant + 20;
			grdContractShedule.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.PartyName;
			grdContractShedule.RootTable.Columns["ContractNo"].Width = Constants.ExportConstants.ContractNoConstant;
			grdContractShedule.RootTable.Columns["Product"].Width = Constants.ExportConstants.ItemNameConstants;
			grdContractShedule.RootTable.Columns["WeightM_Ton"].Width = Constants.ExportConstants.QtyConstant;
			grdContractShedule.RootTable.Columns["PackUom"].Width = Constants.ExportConstants.UomConstant + 25;
			grdContractShedule.RootTable.Columns["NoOfBags"].Width = Constants.ExportConstants.QtyConstant;
			grdContractShedule.RootTable.Columns["NoOfContainers"].Width = Constants.ExportConstants.NoOfContainers;
			grdContractShedule.RootTable.Columns["PackingType"].Width = Constants.ExportConstants.PackingTypeConstant;
			grdContractShedule.RootTable.Columns["DestinationPortId"].Width = Constants.ExportConstants.PortConstant;
			grdContractShedule.RootTable.Columns["AttentiveLoadingDate"].Width = Constants.ExportConstants.DateConstant;
			grdContractShedule.RootTable.Columns["Remarks"].Width = Constants.ExportConstants.RemarksConstants;
			grdContractShedule.RootTable.Columns["Status"].Width = Constants.ExportConstants.StatusConstant;
			grdContractShedule.RootTable.Columns["WeightM_Ton"].Caption = "MTon";
			grdContractShedule.RootTable.Columns["AttentiveLoadingDate"].Caption = "Loading Date";
			grdContractShedule.RootTable.Columns["AttentiveLoadingDate"].FormatString = "dd-MMM-yyyy";
			grdContractShedule.RootTable.Columns["NoOfContainers"].AggregateFunction = (AggregateFunction)2;
			grdContractShedule.RootTable.Columns["NoOfContainers"].TextAlignment = (TextAlignment)3;
			grdContractShedule.RootTable.Columns["NoOfContainers"].FormatString = "#,##0.##";
			grdContractShedule.RootTable.Columns["NoOfContainers"].TotalFormatString = "#,##0.##";
			grdContractShedule.RootTable.Columns["WeightM_Ton"].AggregateFunction = (AggregateFunction)2;
			grdContractShedule.RootTable.Columns["WeightM_Ton"].TextAlignment = (TextAlignment)3;
			grdContractShedule.RootTable.Columns["WeightM_Ton"].FormatString = "#,##0.###";
			grdContractShedule.RootTable.Columns["WeightM_Ton"].TotalFormatString = "#,##0.###";
			grdContractShedule.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grdContractShedule.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grdContractShedule.RootTable.Columns["NoOfBags"].FormatString = "#,##0.###";
			grdContractShedule.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.###";
			GridEX_Helper.AddButton(grdContractShedule, "Delete", "X", 20, 0);
			grdContractShedule.FrozenColumns = 1;
			((Control)(object)grdContractShedule).Refresh();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdContractShedule_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdContractShedule.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Delete")
			{
				DeleteRecordFromGrid(r);
			}
			_ = e.Column.Key == "Add";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DeleteRecordFromGrid(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			Conversion.ToString(r.Cells["Status"].Value);
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ExportContractSchedule exIm = new ExportContractSchedule();
				exIm.Id = Conversion.ToInt(r.Cells["Id"].Value);
				exIm.ContractId = Conversion.ToInt(r.Cells["ContractId"].Value);
				exIm.ContractDetailId = Conversion.ToInt(r.Cells["ContractDetailId"].Value);
				exIm.CustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				exIm.RecordIdentityId = Conversion.ToInt(r.Cells["ScheduleNo"].Value);
				exIm.NoOfContainers = Conversion.ToDecimal(r.Cells["NoOfContainers"].Value);
				exIm.MTon = Conversion.ToDecimal(r.Cells["WeightM_Ton"].Value);
				exIm.ItemQty = Conversion.ToDecimal(r.Cells["NoOfBags"].Value);
				exIm.NetWeight = Conversion.ToDecimal(r.Cells["WeightM_Ton"].Value) * 1000m;
				exIm.LoadingDate = Conversion.ToDateTime(r.Cells["AttentiveLoadingDate"].Value);
				exIm.DestinationPortId = Conversion.ToInt(r.Cells["DestinationPortId"].Value);
				exIm.EntryDate = DateTime.Now;
				exIm.EntryUserId = UserAccount.ID;
				exIm.ModifyDate = DateTime.Now;
				exIm.ModifyUserId = UserAccount.ID;
				exIm.OrganizationId = UserAccount.OrganizationId;
				exIm.CompanyId = UserAccount.CompanyId;
				exIm.DocumentTypeId = 247;
				exIm.ActionId = 3;
				LstRemoveRecordMain.Add(exIm);
				tableScheduleMain.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			else if (r != null && (int)r.RowType == 0)
			{
				r.Delete();
			}
			grdContractShedule.UpdateData();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddRowInScheduleMainGrid()
	{
		try
		{
			double TotalWeightM_Ton = 0.0;
			double TotalNoOfContainers = 0.0;
			Conversion.ToInt(grdContractShedule.CurrentRow.Cells["ContractId"].Value);
			double WeightM_Ton = 0.0;
			double NoOfContainers = 0.0;
			if (grdContractShedule.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdContractShedule.GetRows();
				foreach (GridEXRow item in rows)
				{
					NoOfContainers += Conversion.ToDouble(item.Cells["NoOfContainers"].Value);
					WeightM_Ton += Conversion.ToDouble(item.Cells["WeightM_Ton"].Value);
				}
			}
			double GrossWeightM_Ton = TotalWeightM_Ton - WeightM_Ton;
			double GrossNoOfContainers = TotalNoOfContainers - NoOfContainers;
			if (GrossWeightM_Ton > 0.0 || GrossNoOfContainers > 0.0)
			{
				if (GrossWeightM_Ton > 0.0)
				{
					tableScheduleMain.Rows.Add(0, grdContractShedule.CurrentRow.Cells["ContractId"].Value, RecordIdentityId, grdContractShedule.CurrentRow.Cells["SupplierCustomerId"].Value, grdContractShedule.CurrentRow.Cells["CustomerName"].Value, "", GrossNoOfContainers, GrossWeightM_Ton, grdContractShedule.CurrentRow.Cells["AttentiveLoadingDate"].Value, grdContractShedule.CurrentRow.Cells["DestinationPortId"].Value, grdContractShedule.CurrentRow.Cells["AttentiveProductionDate"].Value, grdContractShedule.CurrentRow.Cells["AttentiveInspectionDate"].Value, grdContractShedule.CurrentRow.Cells["PackingMaterialDate"].Value, grdContractShedule.CurrentRow.Cells["CustomGroupId"].Value, grdContractShedule.CurrentRow.Cells["Remarks"].Value, "");
					grdContractShedule.DataSource = tableScheduleMain;
				}
				else
				{
					MessageBox.Show("Please Check Grid WeightM_Ton  and TotalWeightM_Ton of Contract ");
				}
			}
			else
			{
				MessageBox.Show("Please Check Grid WeightM_Ton  and TotalWeightM_Ton of Contract ");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdContractShedule_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdContractShedule.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "NoOfContainers")
				{
					double NoOfContainers3 = Conversion.ToDouble(item.Cells["NoOfContainers"].Value);
					double ContractFcl3 = Conversion.ToDouble(item.Cells["ContractFcl"].Value);
					if (!skipFCLContractValidationOnSchedule && NoOfContainers3 > ContractFcl3)
					{
						MessageBox.Show("No Of Containers can not be Greater than total No Of Containers of this Contract..");
						item.Cells["NoOfContainers"].Value = Math.Round(ContractFcl3, 2);
					}
					else
					{
						item.Cells["NoOfContainers"].Value = Math.Round(NoOfContainers3, 2);
					}
				}
				if (e.Column.Key == "WeightM_Ton" && Conversion.ToDouble(item.Cells["WeightM_Ton"].Value) > 0.0)
				{
					double ContractFcl2 = Conversion.ToDouble(item.Cells["ContractFcl"].Value);
					double ContractMTon2 = Conversion.ToDouble(item.Cells["ContractMTon"].Value);
					double NoOfContainers2 = Conversion.ToDouble(item.Cells["NoOfContainers"].Value);
					double WeightM_Ton2 = Conversion.ToDouble(item.Cells["WeightM_Ton"].Value);
					double BalMton2 = Conversion.ToDouble(item.Cells["BalMTon"].Value);
					double PackEquivalent2 = Conversion.ToDouble(item.Cells["PackEquivalent"].Value);
					if (WeightM_Ton2 > BalMton2)
					{
						MessageBox.Show($"MTon:{WeightM_Ton2} can not be Greater than Bal:{BalMton2} ...");
						item.Cells["WeightM_Ton"].Value = Math.Round(BalMton2, 3);
						double NoOfBags2 = Math.Round(BalMton2, 3) * 1000.0 / PackEquivalent2;
						item.Cells["NoOfBags"].Value = NoOfBags2;
						item.Cells["NoOfContainers"].Value = NoOfContainers2;
					}
					else
					{
						double AvgNoOfContainers2 = ContractMTon2 / ContractFcl2;
						double NoOfCont2 = Math.Round(WeightM_Ton2 / AvgNoOfContainers2, 0);
						WeightM_Ton2 = Math.Round(WeightM_Ton2, 3);
						item.Cells["WeightM_Ton"].Value = WeightM_Ton2;
						double NoOfBags3 = WeightM_Ton2 * 1000.0 / PackEquivalent2;
						item.Cells["NoOfBags"].Value = NoOfBags3;
						item.Cells["NoOfContainers"].Value = NoOfCont2;
					}
				}
				if (e.Column.Key == "NoOfBags")
				{
					double NoOfBags = Conversion.ToDouble(item.Cells["NoOfBags"].Value);
					double WeightM_Ton = 0.0;
					double BalMton = Conversion.ToDouble(item.Cells["BalMTon"].Value);
					double PackEquivalent = Conversion.ToDouble(item.Cells["PackEquivalent"].Value);
					double ContractFcl = Conversion.ToDouble(item.Cells["ContractFcl"].Value);
					double ContractMTon = Conversion.ToDouble(item.Cells["ContractMTon"].Value);
					double NoOfContainers = Conversion.ToDouble(item.Cells["NoOfContainers"].Value);
					WeightM_Ton = NoOfBags * PackEquivalent / 1000.0;
					WeightM_Ton = Math.Round(WeightM_Ton, 3);
					if (WeightM_Ton > BalMton)
					{
						item.Cells["WeightM_Ton"].Value = Math.Round(BalMton, 3);
						NoOfBags = Math.Round(BalMton, 3) * 1000.0 / PackEquivalent;
						item.Cells["NoOfBags"].Value = NoOfBags;
						MessageBox.Show($"MTon:{WeightM_Ton} can not be Greater than Bal:{BalMton} ...");
						item.Cells["WeightM_Ton"].Value = Math.Round(BalMton, 3);
						item.Cells["NoOfContainers"].Value = NoOfContainers;
					}
					else
					{
						WeightM_Ton = Math.Round(WeightM_Ton, 3);
						item.Cells["WeightM_Ton"].Value = WeightM_Ton;
						double AvgNoOfContainers = ContractMTon / ContractFcl;
						double NoOfCont = Math.Round(WeightM_Ton / AvgNoOfContainers, 0);
						item.Cells["NoOfContainers"].Value = NoOfCont;
					}
				}
				_ = e.Column.Key == "AttentiveLoadingDate";
			}
			grdContractShedule.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			InsertScheduleMain();
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
			btnsave_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InsertScheduleMain()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		try
		{
			bool Update = false;
			GridEXRow[] rows = grdContractShedule.GetRows();
			for (int j = 0; j < rows.Length; j++)
			{
				if (Conversion.ToInt(rows[j].Cells["Id"].Value) > 0)
				{
					Update = true;
					break;
				}
			}
			ExportContractSchedule obj = new ExportContractSchedule();
			if (Update)
			{
				if (MessageBox.Show("Are you sure to Update Schedule?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save Schedule?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.EntryDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.ModifyUserId = UserAccount.ID;
			obj.ExportContractScheduleList = new List<ExportContractSchedule>();
			if (LstRemoveRecordMain.Count > 0)
			{
				for (int i = 0; i < LstRemoveRecordMain.Count; i++)
				{
					obj.ExportContractScheduleList.Add(LstRemoveRecordMain[i]);
				}
			}
			if (grdContractShedule.RowCount > 0)
			{
				double Weight_Mton_Detail = 0.0;
				rows = grdContractShedule.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.CheckDateTimeNull(Conversion.ToDateTime(r.Cells["AttentiveLoadingDate"].Value)))
					{
						MessageBox.Show("Loading Date cannot be null in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					ExportContractSchedule exIm = new ExportContractSchedule();
					exIm.Id = Conversion.ToInt(r.Cells["Id"].Value);
					exIm.ContractId = Conversion.ToInt(r.Cells["ContractId"].Value);
					exIm.ContractDetailId = Conversion.ToInt(r.Cells["ContractDetailId"].Value);
					exIm.CustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					if (Conversion.ToDouble(r.Cells["NoOfContainers"].Value) == 0.0)
					{
						MessageBox.Show("NoOfContainers Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					exIm.NoOfContainers = Conversion.ToInt(r.Cells["NoOfContainers"].Value);
					if (Conversion.ToDecimal(r.Cells["WeightM_Ton"].Value) == 0m)
					{
						MessageBox.Show("MTon Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					Weight_Mton_Detail += Conversion.ToDouble(r.Cells["WeightM_Ton"].Value);
					exIm.MTon = Conversion.ToDecimal(r.Cells["WeightM_Ton"].Value);
					if (exIm.MTon > Conversion.ToDecimal(r.Cells["BalMTon"].Value))
					{
						MessageBox.Show(string.Format("MTon can't be greater than {0} in Row No:", Conversion.ToDecimal(r.Cells["BalMTon"].Value)) + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					exIm.NetWeight = Conversion.ToDecimal(r.Cells["WeightM_Ton"].Value) * 1000m;
					exIm.LoadingDate = Conversion.ToDateTime(r.Cells["AttentiveLoadingDate"].Value);
					if (Conversion.ToInt(r.Cells["DestinationPortId"].Value) == 0)
					{
						MessageBox.Show("DestinationPort Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					exIm.DestinationPortId = Conversion.ToInt(r.Cells["DestinationPortId"].Value);
					exIm.ItemQty = Conversion.ToDecimal(r.Cells["NoOfBags"].Value);
					if (exIm.ItemQty <= 0m)
					{
						throw new Exception($"NoOfBags required in row No:{r.RowIndex + 1}");
					}
					exIm.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					exIm.PackUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					exIm.RecordIdentityId = Conversion.ToInt(r.Cells["ScheduleNo"].Value);
					exIm.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					if (exIm.RecordIdentityId <= 0)
					{
						throw new Exception("Record Identity No Required in Grid");
					}
					exIm.EntryDate = DateTime.Now;
					exIm.EntryUserId = UserAccount.ID;
					exIm.ModifyDate = DateTime.Now;
					exIm.ApprovedDate = DateTime.Now;
					exIm.ModifyUserId = UserAccount.ID;
					exIm.ApprovedUserId = UserAccount.ID;
					exIm.OrganizationId = UserAccount.OrganizationId;
					exIm.CompanyId = UserAccount.CompanyId;
					exIm.DocumentTypeId = 247;
					exIm.IsApproved = false;
					exIm.ActionId = ((exIm.Id <= 0) ? 1 : 2);
					exIm.Status = "Open";
					obj.ExportContractScheduleList.Add(exIm);
				}
				obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<ExportContractSchedule>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				obj.AttachmentsValues = AttachmentsValues;
				obj.CustomAttachmentsValues = CustomAttachmentsValues;
				ExportContractSchedule.Save(obj);
				if (!Update)
				{
					MessageBox.Show("Schedule Save SuccessFully ");
					FormReset();
				}
				else
				{
					MessageBox.Show("Schedule Update SuccessFully");
					FormHelper.DeleteAllAttachmentsFromDb(AT, obj.ExportContractScheduleList[0].RecordIdentityId, obj.DocumentTypeId, base.Name);
					FormReset();
				}
			}
			else
			{
				MessageBox.Show("Schedule Grid have no row", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadbyMainScheduleByContractId(int Id)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			FormReset();
			CurrContractId = Id;
			ExportContractSchedule obj = new ExportContractSchedule();
			obj.ExportContractScheduleList = ExportContractSchedule.GetByID(Id);
			if (obj.ExportContractScheduleList != null && obj.ExportContractScheduleList.Count > 0)
			{
				UpdateModeMain = true;
				tableScheduleMain.Rows.Clear();
				AttachmentsValues = obj.ExportContractScheduleList[0].AttachmentsValues;
				CustomAttachmentsValues = obj.ExportContractScheduleList[0].CustomAttachmentsValues;
				for (int i = 0; i < obj.ExportContractScheduleList.Count; i++)
				{
					tableScheduleMain.Rows.Add(obj.ExportContractScheduleList[i].Id, obj.ExportContractScheduleList[i].ContractId, obj.ExportContractScheduleList[i].ContractDetailId, obj.ExportContractScheduleList[i].RecordIdentityId, obj.ExportContractScheduleList[i].CustomerId, obj.ExportContractScheduleList[i].CustomerName, obj.ExportContractScheduleList[i].ContractNo, obj.ExportContractScheduleList[i].ItemId, obj.ExportContractScheduleList[i].ItemName, obj.ExportContractScheduleList[i].MTon, obj.ExportContractScheduleList[i].PackUomId, obj.ExportContractScheduleList[i].PackUom, obj.ExportContractScheduleList[i].PackEquivalent, obj.ExportContractScheduleList[i].ItemQty, obj.ExportContractScheduleList[i].NoOfContainers, obj.ExportContractScheduleList[i].PackingType, obj.ExportContractScheduleList[i].DestinationPortId, obj.ExportContractScheduleList[i].LoadingDate, obj.ExportContractScheduleList[i].Remarks, obj.ExportContractScheduleList[i].BalMTon, obj.ExportContractScheduleList[i].Status, obj.ExportContractScheduleList[i].ContractMTon, obj.ExportContractScheduleList[i].ContractNoOfContainers);
				}
				grdContractShedule.DataSource = tableScheduleMain;
				grdContractShedule.RetrieveStructure();
				grdContractSheduleSettings();
				FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateMtonDetail()
	{
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				DataGridHistoryFill(50);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistoryFill(int NoOfRecords = 0)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.UserId = UserAccount.ID;
			}
			obj.DocumentTypeId = 247;
			if (rdLoadingDate.Checked)
			{
				if (txtdatefrom.Checked)
				{
					obj.FromDate = txtdatefrom.Value;
				}
				if (txtdateto.Checked)
				{
					obj.ToDate = txtdateto.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (txtdatefrom.Checked)
				{
					obj.EntryDateFrom = txtdatefrom.Value;
				}
				if (txtdateto.Checked)
				{
					obj.EntryDateTo = txtdateto.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (txtdatefrom.Checked)
				{
					obj.ModifyDateFrom = txtdatefrom.Value;
				}
				if (txtdateto.Checked)
				{
					obj.ModifyDateTo = txtdateto.Value;
				}
			}
			obj.DestinationPortId = Conversion.ToInt(cmbPort.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierName.Value);
			obj.ItemId = Conversion.ToInt(CmbItemHistory.Value);
			dtHistory = ExportContractSchedule.FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("RecordIdentity", typeof(int));
				dtcol.Columns.Add("ContractId", typeof(int));
				dtcol.Columns.Add("ContractNo", typeof(string));
				dtcol.Columns.Add("ContractDate", typeof(DateTime));
				dtcol.Columns.Add("SupplierCustomerId", typeof(int));
				dtcol.Columns.Add("CustomerName", typeof(string));
				dtcol.Columns.Add("Product", typeof(string));
				dtcol.Columns.Add("MTon", typeof(decimal));
				dtcol.Columns.Add("PackUom", typeof(string));
				dtcol.Columns.Add("NoOfBags", typeof(decimal));
				dtcol.Columns.Add("NoOfContainers", typeof(int));
				dtcol.Columns.Add("DestinationPort", typeof(string));
				dtcol.Columns.Add("LoadingDate", typeof(DateTime));
				dtcol.Columns.Add("Remarks", typeof(string));
				dtcol.Columns.Add("Status", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtHistory.Rows[i]["RecordNo"], dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["RecordIdentityId"], dtHistory.Rows[i]["ContractId"], dtHistory.Rows[i]["ContractNo"], dtHistory.Rows[i]["ContractDate"], dtHistory.Rows[i]["CustomerId"], dtHistory.Rows[i]["CustomerName"], dtHistory.Rows[i]["ItemName"], dtHistory.Rows[i]["MTon"], dtHistory.Rows[i]["PackUom"], dtHistory.Rows[i]["ItemQty"], dtHistory.Rows[i]["NoOfContainers"], dtHistory.Rows[i]["DestinationPort"], Conversion.ToDateTime(dtHistory.Rows[i]["LoadingDate"]).ToShortDateString(), dtHistory.Rows[i]["Remarks"], dtHistory.Rows[i]["Status"], dtHistory.Rows[i]["EntryDate"], dtHistory.Rows[i]["EntryUser"], dtHistory.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtcol;
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "SupplierCustomerId", "ContractId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"RecordNo",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 2, hiddenColumns, editableColumns);
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["LoadingDate"].FormatString = "dd-MMM-yyyy";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ContractNo"].Width = Constants.ExportConstants.ContractNoConstant + 20;
			DataGridHistory.RootTable.Columns["ContractDate"].Width = Constants.ExportConstants.DateConstant;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.PartyName + 20;
			DataGridHistory.RootTable.Columns["Product"].Width = Constants.ExportConstants.ItemNameConstants;
			DataGridHistory.RootTable.Columns["MTon"].Width = Constants.ExportConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["PackUom"].Width = Constants.ExportConstants.UomConstant + 2;
			DataGridHistory.RootTable.Columns["NoOfBags"].Width = Constants.ExportConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["NoOfContainers"].Width = Constants.ExportConstants.NoOfContainers;
			DataGridHistory.RootTable.Columns["DestinationPort"].Width = Constants.ExportConstants.PortConstant;
			DataGridHistory.RootTable.Columns["LoadingDate"].Width = Constants.ExportConstants.DateConstant + 15;
			DataGridHistory.RootTable.Columns["Remarks"].Width = Constants.ExportConstants.RemarksConstants;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.ExportConstants.EntryUserConstants + 15;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.ExportConstants.NoOfAttachmentsConstants;
			if (formright.DoHaveUpdateRights)
			{
				GridEX_Helper.AddButton(DataGridHistory, "Edit", "Edit", 40, 0);
			}
			if (formright.DoHavePrintRights)
			{
				GridEX_Helper.AddButton(DataGridHistory, "Print", "Print", 50, 1);
			}
			if (formright.DoHavePrintRights && formright.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns["Edit"].Position = 0;
				DataGridHistory.RootTable.Columns["Print"].Position = 1;
				DataGridHistory.FrozenColumns = 2;
			}
			else if (formright.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns["Edit"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			else if (formright.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns["Print"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			ctrlGrdBar7_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnLoadAllHistoryNew_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbPort).Text = string.Empty;
			((Control)(object)cmbSupplierName).Text = string.Empty;
			((Control)(object)CmbItemHistory).Text = string.Empty;
			DataGridHistory.ClearStructure();
			txtdatefrom.Focus();
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
				int RecordIdentityId = Conversion.ToInt(r.Cells["RecordIdentity"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(RecordIdentityId);
				}
				else if (e.Column.Key == "Print")
				{
					CommonServices.ExportContractScheduleSlip(RecordIdentityId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		try
		{
			tabControl1.SelectedIndex = 0;
			UpdateModeMain = false;
			ReadbyMainScheduleByContractId(Id);
			if (!UpdateModeMain)
			{
				grdContractShedule.DataSource = null;
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
			CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["RecordIdentity"].Value), base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnHistoryPrint_Click(object sender, EventArgs e)
	{
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable selectedRows = new DataTable();
			selectedRows.Columns.Add("Id", typeof(int));
			selectedRows.Columns.Add("RecordNo", typeof(int));
			GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
			GridEXRow[] array = ((checkedRows != null && checkedRows.Length != 0) ? checkedRows : DataGridHistory.GetDataRows());
			foreach (GridEXRow row in array)
			{
				selectedRows.Rows.Add(Conversion.ToInt(row.Cells["Id"].Value), Conversion.ToInt(row.Cells["RecordNo"].Value));
			}
			if (selectedRows.Rows.Count == 0)
			{
				MessageBox.Show("No record selected for printing.");
				return;
			}
			IEnumerable<DataRow> filteredQuery = from historyRow in dtHistory.AsEnumerable()
				join selectedRow in selectedRows.AsEnumerable() on historyRow.Field<int>("Id") equals selectedRow.Field<int>("Id")
				orderby selectedRow.Field<int>("RecordNo")
				select historyRow;
			if (!filteredQuery.Any())
			{
				MessageBox.Show("No record found for display");
				return;
			}
			DataTable reportData = filteredQuery.CopyToDataTable();
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(reportData, "551-ContractSchedule_FormHistory.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdContractShedule, tableScheduleMain);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar7_Load(object sender, EventArgs e)
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

	private void frmSaleContractSchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				BtnNewMain_Click(null, null);
				if (((Control)(object)grdContractShedule).Focused)
				{
					btnnew_Click(null, null);
				}
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
			if (e.Control && e.KeyCode == Keys.L)
			{
				_ = tabControl1.SelectedIndex;
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (formright.DoHaveSaveRight && tabControl1.SelectedIndex == 0 && ((Control)(object)grdContractShedule).Focused)
				{
					btnsave_Click(null, null);
				}
				if (tabControl1.SelectedIndex == 1)
				{
					btnShow_Click(null, null);
				}
			}
			if (e.Alt && e.KeyCode == Keys.P)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					BtnContractWiseSlip_Click(null, null);
				}
				else
				{
					BtnHistoryPrint_Click(null, null);
				}
			}
			if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1))
				{
					BtnHistoryPrint_Click(null, null);
				}
				else if (e.Control && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2))
				{
					BtnPrint551_01_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtdatefrom.Focus();
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 38;
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grdContractShedule).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnattachment_Click(null, null);
			}
			if (e.Control && e.Alt)
			{
				BtnShortCutkeys_Click(null, null);
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 82;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			dt.Rows.Add("Ctrl+S", "For Save of Focused Grid");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New of Focused Grid");
			dt.Rows.Add("Ctrl+L", "For Load All");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+1", "For Print 551 in History");
			dt.Rows.Add("Alt+2", "For Print 551_01 in History");
			dt.Rows.Add("Ctrl+F5", "For Focus on 'Sale Contract for Schedule' Grid");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Right", "For Move focus From One Grid to Next");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Schedule Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date Combo in Detail Box");
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

	private void grdContractShedule_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdContractShedule.CurrentRow;
			_ = ((Control)(object)grdContractShedule).Focused;
			if (e.Control && e.KeyCode == Keys.Space && row != null && (int)row.RowType == 0 && grdContractShedule.CurrentColumn != null)
			{
				if (grdContractShedule.CurrentColumn.Key == "Delete")
				{
					DeleteRecordFromGrid(row);
				}
				if (grdContractShedule.CurrentColumn.Key == "Add")
				{
					AddRowInScheduleMainGrid();
				}
			}
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
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = DataGridHistory.CurrentRow;
				if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
				{
					if (DataGridHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
					{
						tabControl1.SelectedIndex = 0;
						UpdateModeDetail = false;
						UpdateModeMain = false;
						ReadbyMainScheduleByContractId(Conversion.ToInt(r.Cells["ExImLcOrderId"].Value));
						if (!UpdateModeMain)
						{
							grdContractShedule.DataSource = null;
						}
						CurrContractId = Conversion.ToInt(r.Cells["ExImLcOrderId"].Value);
					}
					if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
					{
						DataGridHistory_LinkClicked(null, null);
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				DataGridHistory_ColumnButtonClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtMTonDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateMtonDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistoryCombos_Click(object sender, EventArgs e)
	{
		try
		{
			dtPortFill();
			ContractScheduleGridComboFill();
			ComboBindForHistory(HistoryComboDbCall());
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
			DataGridHistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnContractWiseSlip_Click(object sender, EventArgs e)
	{
	}

	private void frmSaleContractSchedule_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnPrint551_01_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dtReport = ExImLcOrderShipmentSchedule.ContractSchedule_StatusReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = txtdatefrom.Value,
				ToDate = txtdateto.Value,
				DestinationPortId = Conversion.ToInt(cmbPort.Value),
				SupplierCustomerId = Conversion.ToInt(cmbSupplierName.Value),
				ItemId = Conversion.ToInt(CmbItemHistory.Value)
			});
			if (dtReport == null || dtReport.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtReport, "551_01_ContractSchedule_StatusReport.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdContractShedule_Click(object sender, EventArgs e)
	{
		try
		{
			_ = grdContractShedule.CurrentRow;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractShedule_UpdatingRecord(object sender, CancelEventArgs e)
	{
	}

	private void btnLoad_Click(object sender, EventArgs e)
	{
		try
		{
			Loader = new LoadContractForSchedule(UserAccount);
			Loader.ShowDialog();
			Loader.BringToFront();
			LoadInGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGrid()
	{
		try
		{
			DataTable loader = Loader.dtLoader.Copy();
			if (loader.Rows.Count <= 0)
			{
				return;
			}
			bool flag = false;
			int RecordIdentityInUpdate = 0;
			for (int i = 0; i < loader.Rows.Count; i++)
			{
				if (grdContractShedule.GetRows().Length != 0)
				{
					GridEXRow[] rows = grdContractShedule.GetRows();
					foreach (GridEXRow obj in rows)
					{
						if (Conversion.ToInt(obj.Cells["ContractId"].Value) != Conversion.ToInt(loader.Rows[i]["Id"]))
						{
							throw new Exception("Data against another Contract Already Exist!");
						}
						if (Conversion.ToInt(obj.Cells["ContractDetailId"].Value) == Conversion.ToInt(loader.Rows[i]["DetailId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (UpdateModeMain)
				{
					RecordIdentityInUpdate = Conversion.ToInt(grdContractShedule.GetRow(0).Cells["ScheduleNo"].Value);
				}
				if (!flag)
				{
					double AvgNoOfContainers = Conversion.ToDouble(loader.Rows[i]["TotalContractMton"]) / Conversion.ToDouble(loader.Rows[i]["NoOfContainers"]);
					double NoOfCont = Math.Round(Conversion.ToDouble(loader.Rows[i]["BalQty"]) / AvgNoOfContainers, 0);
					tableScheduleMain.Rows.Add(0, loader.Rows[i]["Id"], loader.Rows[i]["DetailId"], UpdateModeMain ? RecordIdentityInUpdate : RecordIdentityId, loader.Rows[i]["SupplierCustomerId"], loader.Rows[i]["CustomerName"], loader.Rows[i]["ContractNo"], loader.Rows[i]["ExImItemId"], loader.Rows[i]["ItemName"], loader.Rows[i]["BalQty"], loader.Rows[i]["ItemUomId"], loader.Rows[i]["PackUomCode"], loader.Rows[i]["PackUomEquivalent"], loader.Rows[i]["BalNoOfUnit"], NoOfCont, loader.Rows[i]["PackingType"], loader.Rows[i]["DestinationPortId"], DateTime.Now, "", loader.Rows[i]["BalQty"], "", loader.Rows[i]["TotalContractMton"], loader.Rows[i]["NoOfContainers"]);
				}
				flag = false;
			}
			grdContractShedule.DataSource = tableScheduleMain;
			grdContractShedule.RetrieveStructure();
			grdContractSheduleSettings();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Expected O, but got Unknown
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Expected O, but got Unknown
		//IL_0ee3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eed: Expected O, but got Unknown
		//IL_0efa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f04: Expected O, but got Unknown
		GridEXLayout grdContractShedule_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.frmExportContractSchedule));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdContractShedule = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label14 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshScheduleMain = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnLoad = new System.Windows.Forms.ToolStripButton();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel26 = new System.Windows.Forms.Panel();
		this.panel27 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel24 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel25 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdLoadingDate = new System.Windows.Forms.RadioButton();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbPort = new UltraCombo();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.txtdateto = new System.Windows.Forms.DateTimePicker();
		this.label50 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbSupplierName = new UltraCombo();
		this.txtdatefrom = new System.Windows.Forms.DateTimePicker();
		this.label19 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.CmbItemHistory = new UltraCombo();
		this.btnShow = new System.Windows.Forms.Button();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAllHistoryNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistoryCombos = new System.Windows.Forms.ToolStripButton();
		this.BtnHistoryPrint = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint551_01 = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContractShedule).BeginInit();
		this.panel13.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel26.SuspendLayout();
		this.panel27.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel24.SuspendLayout();
		this.panel25.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).BeginInit();
		this.toolStrip4.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1137, 593);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel12);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1129, 564);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdContractShedule);
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1123, 558);
		this.panel12.TabIndex = 3;
		grdContractShedule_DesignTimeLayout.LayoutString = resources.GetString("grdContractShedule_DesignTimeLayout.LayoutString");
		this.grdContractShedule.DesignTimeLayout = grdContractShedule_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdContractShedule.DynamicFiltering = true;
		this.grdContractShedule.FilterMode = (FilterMode)2;
		this.grdContractShedule.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContractShedule.GroupByBoxVisible = false;
		this.grdContractShedule.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Location = new System.Drawing.Point(0, 51);
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Name = "grdContractShedule";
		((ScrollableBase)this.grdContractShedule).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Size = new System.Drawing.Size(1123, 507);
		((System.Windows.Forms.Control)(object)this.grdContractShedule).TabIndex = 7;
		this.grdContractShedule.TotalRow = (InheritableBoolean)1;
		this.grdContractShedule.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContractShedule).VisualStyle = (VisualStyle)4;
		this.grdContractShedule.CellUpdated += new ColumnActionEventHandler(grdContractShedule_CellUpdated);
		this.grdContractShedule.ColumnButtonClick += new ColumnActionEventHandler(grdContractShedule_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdContractShedule).KeyDown += new System.Windows.Forms.KeyEventHandler(grdContractShedule_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.ctrlGrdBar1);
		this.panel13.Controls.Add(this.label14);
		this.panel13.Controls.Add(this.panel3);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1123, 51);
		this.panel13.TabIndex = 6;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1083, 28);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar1.MyGrid = this.grdContractShedule;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 57;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(3, 30);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(135, 17);
		this.label14.TabIndex = 54;
		this.label14.Text = "Contract Schedule";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1123, 27);
		this.panel3.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.BtnRefreshScheduleMain, this.btnsave, this.btnattachment, this.btnLoad });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1123, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.BtnRefreshScheduleMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshScheduleMain.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshScheduleMain.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshScheduleMain.Name = "BtnRefreshScheduleMain";
		this.BtnRefreshScheduleMain.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshScheduleMain.Text = "&Refresh";
		this.BtnRefreshScheduleMain.Click += new System.EventHandler(BtnRefreshScheduleMain_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(117, 24);
		this.btnsave.Text = "&Save/Update";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnLoad.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoad.Image = Architecture.WinApp.Properties.Resources.download;
		this.btnLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoad.Name = "btnLoad";
		this.btnLoad.Size = new System.Drawing.Size(161, 24);
		this.btnLoad.Text = "Load Contract Data";
		this.btnLoad.Click += new System.EventHandler(btnLoad_Click);
		this.tabPage3.Controls.Add(this.panel26);
		this.tabPage3.Controls.Add(this.panel25);
		this.tabPage3.Controls.Add(this.toolStrip4);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1129, 564);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel26.Controls.Add(this.panel27);
		this.panel26.Controls.Add(this.panel24);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel26.Location = new System.Drawing.Point(0, 110);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1129, 454);
		this.panel26.TabIndex = 55;
		this.panel27.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel27.Location = new System.Drawing.Point(0, 27);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1129, 427);
		this.panel27.TabIndex = 54;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1129, 427);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 53;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel24.BackColor = System.Drawing.Color.Teal;
		this.panel24.Controls.Add(this.ctrlGrdBar7);
		this.panel24.Controls.Add(this.label5);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel24.Location = new System.Drawing.Point(0, 0);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1129, 27);
		this.panel24.TabIndex = 52;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(1087, 3);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar7.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar7.TabIndex = 56;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(6, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 19);
		this.label5.TabIndex = 55;
		this.label5.Text = "History";
		this.panel25.Controls.Add(this.groupBox2);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel25.Location = new System.Drawing.Point(0, 27);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1129, 83);
		this.panel25.TabIndex = 54;
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.rdLoadingDate);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPort);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.txtdateto);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierName);
		this.groupBox2.Controls.Add(this.txtdatefrom);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemHistory);
		this.groupBox2.Controls.Add(this.btnShow);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(831, 78);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(729, 51);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdLoadingDate.AutoSize = true;
		this.rdLoadingDate.Checked = true;
		this.rdLoadingDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdLoadingDate.Location = new System.Drawing.Point(729, 19);
		this.rdLoadingDate.Name = "rdLoadingDate";
		this.rdLoadingDate.Size = new System.Drawing.Size(96, 19);
		this.rdLoadingDate.TabIndex = 6;
		this.rdLoadingDate.TabStop = true;
		this.rdLoadingDate.Text = "Loading Date";
		this.rdLoadingDate.UseVisualStyleBackColor = true;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(254, 26);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(29, 15);
		this.label10.TabIndex = 577;
		this.label10.Text = "Port";
		this.cmbPort.AllowNull = (DefaultableBoolean)1;
		this.cmbPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance147;
		((SpecialBoxBase)((UltraGridBase)this.cmbPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance152).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPort).Location = new System.Drawing.Point(314, 20);
		((System.Windows.Forms.Control)(object)this.cmbPort).Name = "cmbPort";
		((System.Windows.Forms.Control)(object)this.cmbPort).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.cmbPort).TabIndex = 2;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(729, 35);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.txtdateto.CustomFormat = "dd-MMM-yy";
		this.txtdateto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdateto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdateto.Location = new System.Drawing.Point(111, 49);
		this.txtdateto.Name = "txtdateto";
		this.txtdateto.ShowCheckBox = true;
		this.txtdateto.Size = new System.Drawing.Size(136, 23);
		this.txtdateto.TabIndex = 1;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(254, 53);
		this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(58, 15);
		this.label50.TabIndex = 16;
		this.label50.Text = "Customer";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(4, 53);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(91, 15);
		this.label11.TabIndex = 575;
		this.label11.Text = "loading Date To";
		this.cmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance159;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Location = new System.Drawing.Point(314, 47);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Name = "cmbSupplierName";
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).TabIndex = 3;
		this.txtdatefrom.CustomFormat = "dd-MMM-yy";
		this.txtdatefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdatefrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdatefrom.Location = new System.Drawing.Point(111, 22);
		this.txtdatefrom.Name = "txtdatefrom";
		this.txtdatefrom.ShowCheckBox = true;
		this.txtdatefrom.Size = new System.Drawing.Size(136, 23);
		this.txtdatefrom.TabIndex = 0;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(504, 26);
		this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(32, 15);
		this.label19.TabIndex = 18;
		this.label19.Text = "Item";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(4, 26);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(109, 15);
		this.label51.TabIndex = 572;
		this.label51.Text = "Loading Date From";
		this.CmbItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance176).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Location = new System.Drawing.Point(543, 20);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Name = "CmbItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).TabIndex = 4;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(668, 47);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 26);
		this.btnShow.TabIndex = 5;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnLoadAllHistoryNew, this.btnRefreshHistoryCombos, this.BtnHistoryPrint, this.BtnPrint551_01 });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1129, 27);
		this.toolStrip4.TabIndex = 8;
		this.toolStrip4.Text = "toolStrip4";
		this.btnLoadAllHistoryNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAllHistoryNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnLoadAllHistoryNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAllHistoryNew.Name = "btnLoadAllHistoryNew";
		this.btnLoadAllHistoryNew.Size = new System.Drawing.Size(59, 24);
		this.btnLoadAllHistoryNew.Text = "New";
		this.btnLoadAllHistoryNew.Click += new System.EventHandler(btnLoadAllHistoryNew_Click);
		this.btnRefreshHistoryCombos.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistoryCombos.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistoryCombos.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistoryCombos.Name = "btnRefreshHistoryCombos";
		this.btnRefreshHistoryCombos.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistoryCombos.Text = "&Refresh";
		this.btnRefreshHistoryCombos.Click += new System.EventHandler(btnRefreshHistoryCombos_Click);
		this.BtnHistoryPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnHistoryPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnHistoryPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnHistoryPrint.Name = "BtnHistoryPrint";
		this.BtnHistoryPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnHistoryPrint.Text = "551-&Print";
		this.BtnHistoryPrint.Visible = false;
		this.BtnHistoryPrint.Click += new System.EventHandler(BtnHistoryPrint_Click);
		this.BtnPrint551_01.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint551_01.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint551_01.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint551_01.Name = "BtnPrint551_01";
		this.BtnPrint551_01.Size = new System.Drawing.Size(116, 24);
		this.BtnPrint551_01.Text = "551_01-&Print";
		this.BtnPrint551_01.Visible = false;
		this.BtnPrint551_01.Click += new System.EventHandler(BtnPrint551_01_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1137, 593);
		base.Controls.Add(this.tabControl1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmExportContractSchedule";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmSaleContractSchedule_FormClosed);
		base.Load += new System.EventHandler(frmSaleContractSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleContractSchedule_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContractShedule).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.tabPage3.PerformLayout();
		this.panel26.ResumeLayout(false);
		this.panel27.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel24.ResumeLayout(false);
		this.panel24.PerformLayout();
		this.panel25.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).EndInit();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
