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
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Production;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Production;

public class ProductionOutputAllocationWithExportInvoice : Form
{
	private int RecId;

	private string ScreenName = "";

	private bool FormInitialized;

	private DataTable dtdetail = new DataTable();

	private DataTable dtInvoice = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formRights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private List<ProductionOutputAllocationWithExportInvoice> lstRemoveRecord = new List<ProductionOutputAllocationWithExportInvoice>();

	private IContainer components;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnsave;

	private Panel panel5;

	private Label label23;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label7;

	internal UltraCombo CmbJobOrder;

	internal GridEX grd;

	private Panel panel6;

	private Label label3;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel1;

	private Label label1;

	private ToolStrip toolStrip2;

	private ToolStripButton btnResetHistory;

	private Panel panel20;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label86;

	internal UltraCombo CmbJobOrderNoHistory;

	private Label label88;

	private Label label2;

	private Button btnShow;

	private Panel panel11;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label5;

	internal GridEX DataGridHistory;

	private ToolStripButton btnRefreshHistory;

	private ToolStripButton btnPrint627;

	public ProductionOutputAllocationWithExportInvoice(UserAccount user)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
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
			ScreenName = Conversion.ToString((object)base.Name);
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemName", typeof(string));
			dtdetail.Columns.Add("OutPutQty", typeof(double));
			dtdetail.Columns.Add("OutPutWeight", typeof(double));
			dtdetail.Columns.Add("Qty", typeof(double));
			dtdetail.Columns.Add("NetWeightKg", typeof(double));
			dtdetail.Columns.Add("ExportInvoiceId", typeof(int));
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			CmbJobOrder.Focus();
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
			formRights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			DataTable jobOrderNoData = GetJobOrderNoData();
			DataTable historyComboData = GetHistoryComboData();
			DataTable exportInvoiceNo = CommonServices.GetExportInvoiceNo();
			return new
			{
				JobOrderData = jobOrderNoData,
				HistoryData = historyComboData,
				InvoiceData = exportInvoiceNo
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
				btnsave.Enabled = formRights.DoHaveSaveRight;
				btnUpdate.Enabled = formRights.DoHaveUpdateRights;
				ctrlGrdBar1.mGridChooseFielder.Enabled = formRights.DoHaveFieldChooserRights;
				ctrlGrdBar1.mGridSaveLayouts.Enabled = formRights.DoHaveSaveLayoutRights;
				ctrlGrdBar1.mGridPrint.Enabled = formRights.DoHaveGridPrintRights;
				ctrlGrdBar1.mGridExport.Enabled = formRights.DoHaveGridExportRights;
				ctrlGrdBar1.GroupCollaps.Enabled = formRights.DoHaveGridGroupCollapseRights;
				ctrlGrdBar1.GroupExpand.Enabled = formRights.DoHaveGridGroupExpandRights;
				bindJobOrderNo(result.JobOrderData);
				bindHistoryCombo(result.HistoryData);
				dtInvoice = result.InvoiceData;
				if (dtdetail != null && dtdetail.Columns.Count > 0)
				{
					grd.DataSource = dtdetail;
					grd.RetrieveStructure();
					grdSettings();
				}
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void DefineCountry_Load(object sender, EventArgs e)
	{
	}

	private DataTable GetJobOrderNoData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		try
		{
			return InvProductionJobOrder.GetJobOrderAll(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				ActionId = 1
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void bindJobOrderNo(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbJobOrder, "Id", "PlanCode", "Job Order No", ZeroIndex: false);
				return;
			}
			((Control)(object)CmbJobOrder).Text = string.Empty;
			((UltraGridBase)CmbJobOrder).DataSource = null;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable GetHistoryComboData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return ProductionOutputAllocationWithExportInvoice.GetDataForDropDownFromProductionOutputAllocationWithExportInvoice(new ReportsParameters
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

	private void bindHistoryCombo(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rows = from r in dt.AsEnumerable()
					where Conversion.ToString(r["Activity"]) == "JobOrder"
					select r;
				DataTable job = (rows.Any() ? rows.CopyToDataTable() : dt.Clone());
				if (job.Rows.Count > 0)
				{
					DDL.BindDDL(job, CmbJobOrderNoHistory, "Id", "ReferenceName", "Job Order No", ZeroIndex: false);
					((UltraGridBase)CmbJobOrderNoHistory).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				else
				{
					((Control)(object)CmbJobOrderNoHistory).Text = string.Empty;
					((UltraGridBase)CmbJobOrderNoHistory).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbJobOrderNoHistory).Text = string.Empty;
				((UltraGridBase)CmbJobOrderNoHistory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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

	public void Insert()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			ProductionOutputAllocationWithExportInvoice obj = new ProductionOutputAllocationWithExportInvoice();
			obj.ProductionOutputAllocationWithExportInvoicesList = new List<ProductionOutputAllocationWithExportInvoice>();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item2 in rows)
			{
				double sumOfQty = Conversion.ToDouble(dtdetail.Compute("Sum(Qty)", "ItemId = '" + Conversion.ToString(item2.Cells["ItemId"].Value) + "'"));
				double sumOfWeight = Conversion.ToDouble(dtdetail.Compute("Sum(NetWeightKg)", "ItemId = '" + Conversion.ToString(item2.Cells["ItemId"].Value) + "'"));
				double outputQty = Conversion.ToDouble(item2.Cells["OutPutQty"].Value);
				double outputWeight = Conversion.ToDouble(item2.Cells["OutPutWeight"].Value);
				string ItemName = Conversion.ToString(item2.Cells["ItemName"].Value);
				if (sumOfQty > outputQty || sumOfWeight > outputWeight)
				{
					if (sumOfQty > outputQty)
					{
						throw new Exception($"Total Qty of Item:{ItemName} can't be greater than output qty which is {outputQty}");
					}
					if (sumOfWeight > outputWeight)
					{
						throw new Exception($"Total Weigh of Item:{ItemName} can't be greater than output weight which is {outputWeight}");
					}
				}
				ProductionOutputAllocationWithExportInvoice d = new ProductionOutputAllocationWithExportInvoice();
				d.ActionId = ((RecId == 0) ? 1 : 2);
				d.OrganizationId = UserAccount.OrganizationId;
				d.CompanyId = UserAccount.CompanyId;
				d.BranchesId = UserAccount.BranchesId;
				d.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				d.EntryUserId = UserAccount.ID;
				d.ModifyUserId = UserAccount.ID;
				d.InvProductionJobOrderId = Conversion.ToInt(CmbJobOrder.Value);
				d.Id = Conversion.ToInt(item2.Cells["Id"].Value);
				d.ItemId = Conversion.ToInt(item2.Cells["ItemId"].Value);
				d.ExImInvoiceId = Conversion.ToInt(item2.Cells["ExportInvoiceId"].Value);
				if (d.ExImInvoiceId == 0)
				{
					throw new Exception("Invoice No found in row#" + (item2.RowIndex + 1));
				}
				d.Qty = Conversion.ToDouble(item2.Cells["Qty"].Value);
				if (d.Qty == 0.0)
				{
					throw new Exception("Qty should be greater than 0 in row#" + (item2.RowIndex + 1));
				}
				d.NetWeight = Conversion.ToDouble(item2.Cells["NetWeightKg"].Value);
				if (d.NetWeight == 0.0)
				{
					throw new Exception("Net weight should be greater than 0 in row#" + (item2.RowIndex + 1));
				}
				obj.ProductionOutputAllocationWithExportInvoicesList.Add(d);
			}
			if (lstRemoveRecord.Count > 0)
			{
				foreach (ProductionOutputAllocationWithExportInvoice item in lstRemoveRecord)
				{
					obj.ProductionOutputAllocationWithExportInvoicesList.Add(item);
				}
			}
			ProductionOutputAllocationWithExportInvoice.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully");
			}
			else
			{
				MessageBox.Show("Record Save Successfully");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Reset()
	{
		try
		{
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			lstRemoveRecord.Clear();
			((Control)(object)CmbJobOrder).Enabled = true;
			((Control)(object)CmbJobOrder).Text = string.Empty;
			dtdetail.Rows.Clear();
			grd.DataSource = null;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["OutputQty"].Visible = false;
			grd.RootTable.Columns["OutputWeight"].Visible = false;
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["OutPutQty"].EditType = (EditType)0;
			grd.RootTable.Columns["OutPutWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["Qty"].EditType = (EditType)1;
			grd.RootTable.Columns["NetWeightKg"].EditType = (EditType)1;
			grd.RootTable.Columns["ExportInvoiceId"].EditType = (EditType)5;
			grd.RootTable.Columns["ItemName"].Width = 250;
			grd.RootTable.Columns["Qty"].Width = 70;
			grd.RootTable.Columns["NetWeightKg"].Width = 100;
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "#,##0.00";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,##0.00";
			grd.RootTable.Columns["NetWeightKg"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeightKg"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeightKg"].FormatString = "#,##0.000";
			grd.RootTable.Columns["NetWeightKg"].TotalFormatString = "#,##0.000";
			GrdComboBind();
			GridEX_Helper.AddButton(grd, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grd, "Add", "+", 20, 1);
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdComboBind()
	{
		try
		{
			grd.RootTable.Columns["ExportInvoiceId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["ExportInvoiceId"].HasValueList = true;
			grd.RootTable.Columns["ExportInvoiceId"].LimitToList = true;
			grd.RootTable.Columns["ExportInvoiceId"].Caption = "Export Invoice";
			grd.RootTable.Columns["ExportInvoiceId"].ValueList.PopulateValueList((IEnumerable)dtInvoice.DefaultView, "Id", "InvoiceNo");
			grd.RootTable.Columns["ExportInvoiceId"].Width = 150;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				double OutPutQty = Conversion.ToDouble(r.Cells["OutPutQty"].Value);
				double OutPutWeight = Conversion.ToDouble(r.Cells["OutPutWeight"].Value);
				double uom = OutPutWeight / OutPutQty;
				double Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
				double NetWeightKg = Conversion.ToDouble(r.Cells["NetWeightKg"].Value);
				double SumOfQty = Conversion.ToDouble(dtdetail.Compute("Sum(Qty)", "ItemId = '" + Conversion.ToString(r.Cells["ItemId"].Value) + "'"));
				double SumOfWeight = Conversion.ToDouble(dtdetail.Compute("Sum(NetWeightKg)", "ItemId = '" + Conversion.ToString(r.Cells["ItemId"].Value) + "'"));
				if (SumOfQty > OutPutQty || SumOfWeight > OutPutWeight)
				{
					double Rem = OutPutQty - SumOfQty;
					Qty = ((Rem < 0.0) ? 0.0 : Rem);
					r.Cells["Qty"].Value = Qty;
					r.Cells["NetWeightKg"].Value = 0;
				}
				if (e.Column.Key == "Qty")
				{
					double weight = Qty * uom;
					r.Cells["NetWeightKg"].Value = weight;
				}
				else if (e.Column.Key == "NetWeightKg")
				{
					double qty = NetWeightKg / uom;
					r.Cells["Qty"].Value = qty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DefineGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		if (InvProductionPlant.GetAll(new InvProductionPlant
		{
			OrganizationId = UserAccount.OrganizationId,
			CompanyId = UserAccount.CompanyId,
			BranchId = UserAccount.BranchesId
		}).Rows.Count > 0)
		{
			grd.RetrieveStructure();
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbJobOrder).ActiveRow == null)
		{
			MessageBox.Show("Job Order Field Required");
			CmbJobOrder.Focus();
			return false;
		}
		return true;
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
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

	private void DefineCountry_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnnew_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.S)
		{
			btnsave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.U)
		{
			btnUpdate_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DefineProductionPlant_FormClosing(object sender, FormClosingEventArgs e)
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

	private void CmbJobOrder_Leave(object sender, EventArgs e)
	{
		try
		{
			int JobOrderId = Conversion.ToInt(CmbJobOrder.Value);
			if (JobOrderId > 0)
			{
				GetDataAgainstJobOrderFromProduction(JobOrderId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDataAgainstJobOrderFromProduction(int JobOrderId)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		try
		{
			dtdetail.Rows.Clear();
			DataTable dt = ProductionOutputAllocationWithExportInvoice.GetDataAgainstJobOrder(new ReportsParameters
			{
				JobOrderId = JobOrderId,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt != null && dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtdetail.Rows.Add(0, dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["Qty"], dt.Rows[i]["Weight"], dt.Rows[i]["Qty"], dt.Rows[i]["Weight"], null);
				}
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings();
			}
			else
			{
				grd.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (dtdetail.Rows.Count > 1)
				{
					DeleteRow(r);
				}
			}
			else
			{
				if (!(e.Column.Key == "Add"))
				{
					return;
				}
				double OutputQty = Conversion.ToDouble(r.Cells["OutPutQty"].Value);
				double num = Conversion.ToDouble(r.Cells["OutPutWeight"].Value);
				double sumOfQty = Conversion.ToDouble(dtdetail.Compute("Sum(Qty)", "ItemId = '" + Conversion.ToString(r.Cells["ItemId"].Value) + "'"));
				double sumOfWeight = Conversion.ToDouble(dtdetail.Compute("Sum(NetWeightKg)", "ItemId = '" + Conversion.ToString(r.Cells["ItemId"].Value) + "'"));
				double Rem = OutputQty - sumOfQty;
				double RemWeight = num - sumOfWeight;
				if (Rem > 0.0 && RemWeight > 0.0)
				{
					DataRow dr = dtdetail.Rows[r.RowIndex];
					dtdetail.Rows.Add(dr.ItemArray);
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						dtdetail.Rows[dtdetail.Rows.Count - 1]["Id"] = 0;
					}
					dtdetail.Rows[dtdetail.Rows.Count - 1]["Qty"] = Rem;
					dtdetail.Rows[dtdetail.Rows.Count - 1]["NetWeightKg"] = RemWeight;
				}
				else
				{
					MessageBox.Show("Can't break further because Total Qty is Equal To Output Qty:" + OutputQty.ToString("#,##0.###") + ".\n If you want to break further first break any row of same Detail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteRow(GridEXRow r)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				ProductionOutputAllocationWithExportInvoice d = new ProductionOutputAllocationWithExportInvoice();
				d.ActionId = 3;
				d.OrganizationId = UserAccount.OrganizationId;
				d.CompanyId = UserAccount.CompanyId;
				d.BranchesId = UserAccount.BranchesId;
				d.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				d.EntryUserId = UserAccount.ID;
				d.ModifyUserId = UserAccount.ID;
				d.InvProductionJobOrderId = Conversion.ToInt(CmbJobOrder.Value);
				d.Id = Conversion.ToInt(r.Cells["Id"].Value);
				d.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				d.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
				d.NetWeight = Conversion.ToDouble(r.Cells["NetWeightKg"].Value);
				d.ExImInvoiceId = Conversion.ToInt(r.Cells["ExportInvoiceId"].Value);
				lstRemoveRecord.Add(d);
				r.Delete();
				grd.UpdateData();
			}
			else
			{
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			GetHistoryData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetHistoryData()
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
			obj.CanViewAllRecord = formRights.DoHaveCanViewAllRecordRights;
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
			obj.JobOrderId = Conversion.ToInt(CmbJobOrderNoHistory.Value);
			DataTable dtHistory = ProductionOutputAllocationWithExportInvoice.FormHistory(obj);
			if (dtHistory != null && dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("JobOrderId", typeof(int));
				dt.Columns.Add("JobOrderNo", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("InvoiceId", typeof(int));
				dt.Columns.Add("InvoiceNo", typeof(string));
				dt.Columns.Add("InvoiceDate", typeof(DateTime));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["InvProductionJobOrderId"], dtHistory.Rows[i]["JobOrderNo"], dtHistory.Rows[i]["ItemId"], dtHistory.Rows[i]["ItemName"], dtHistory.Rows[i]["Qty"], dtHistory.Rows[i]["NetWeight"], dtHistory.Rows[i]["ExImInvoiceId"], dtHistory.Rows[i]["InvoiceNo"], dtHistory.Rows[i]["InvoiceDate"], dtHistory.Rows[i]["EntryDate"], dtHistory.Rows[i]["EntryUser"], dtHistory.Rows[i]["ModifyDate"], dtHistory.Rows[i]["ModifyUser"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				HistoryGridSetting();
			}
			else
			{
				DataGridHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryGridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["JobOrderId"].Visible = false;
			DataGridHistory.RootTable.Columns["ItemId"].Visible = false;
			DataGridHistory.RootTable.Columns["InvoiceId"].Visible = false;
			DataGridHistory.RootTable.Columns["InvoiceDate"].FormatString = "dd-MMM-yy";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			CommonServices.GridWrappingAndColumnSettings(DataGridHistory, 2);
			DataGridHistory.RootTable.Columns["JobOrderNo"].Width = 120;
			DataGridHistory.RootTable.Columns["ItemName"].Width = 180;
			DataGridHistory.RootTable.Columns["Qty"].Width = 75;
			DataGridHistory.RootTable.Columns["NetWeight"].Width = 80;
			DataGridHistory.RootTable.Columns["InvoiceNo"].Width = 120;
			DataGridHistory.RootTable.Columns["InvoiceDate"].Width = 80;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = 135;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = 120;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = 135;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = 120;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Key = "Print";
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int JobOrderId = Conversion.ToInt(r.Cells["JobOrderId"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(JobOrderId);
				}
				else if (e.Column.Key == "Print")
				{
					Slip(JobOrderId);
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
			int CurrId = Conversion.ToInt(r.Cells["JobOrderId"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn != null && DataGridHistory.CurrentColumn.Key == "Edit")
				{
					ReadById(CurrId);
				}
				else if (DataGridHistory.CurrentColumn != null)
				{
					_ = DataGridHistory.CurrentColumn.Key == "Print";
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int JobOrderId)
	{
		try
		{
			Reset();
			List<ProductionOutputAllocationWithExportInvoice> lst = new List<ProductionOutputAllocationWithExportInvoice>();
			RecId = JobOrderId;
			lst = ProductionOutputAllocationWithExportInvoice.GetByID(JobOrderId);
			if (lst != null && lst.Count > 0)
			{
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				tabControl1.SelectedIndex = 0;
				((Control)(object)CmbJobOrder).Enabled = false;
				CmbJobOrder.Value = lst[0].InvProductionJobOrderId;
				for (int i = 0; i < lst.Count; i++)
				{
					dtdetail.Rows.Add(lst[i].Id, lst[i].ItemId, lst[i].ItemName, lst[i].OutPutQty, lst[i].OutPutWeight, lst[i].Qty, lst[i].NetWeight, lst[i].ExImInvoiceId);
				}
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			((Control)(object)CmbJobOrderNoHistory).Text = string.Empty;
			DataGridHistory.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			bindHistoryCombo(GetHistoryComboData());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint627_Click(object sender, EventArgs e)
	{
		try
		{
			int JobOrderId = Conversion.ToInt(CmbJobOrder.Value);
			Slip(JobOrderId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Slip(int JobOrderId)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (JobOrderId > 0)
			{
				DataTable dt = ProductionOutputAllocationWithExportInvoice.ProductionOutputAllocationWithExportInvoice_SlipAndRegister(JobOrderId, 0);
				if (dt.Rows.Count > 0 && dt != null)
				{
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTable(dt, "627-ProductionOutputAllocationWithExportInvoice.rpt");
				}
				else
				{
					MessageBox.Show("Record Not Found For DisPlay");
				}
			}
			else
			{
				MessageBox.Show("Record Not Found For DisPlay");
			}
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
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.KeyCode == Keys.F1)
			{
				int Id = 0;
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "ExportInvoiceId")
				{
					GrdPopUp grdPopUp = new GrdPopUp(dtInvoice, "Id", "InvoiceNo");
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					grdPopUp.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp.ReturnId);
					Conversion.ToString((object)grdPopUp.ReturnName);
					r.Cells["ExportInvoiceId"].Value = Id;
				}
				grd.UpdateData();
			}
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
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Expected O, but got Unknown
		//IL_1229: Unknown result type (might be due to invalid IL or missing references)
		//IL_1233: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		this.grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbJobOrder = new UltraCombo();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnPrint627 = new System.Windows.Forms.ToolStripButton();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel20 = new System.Windows.Forms.Panel();
		this.btnShow = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label86 = new System.Windows.Forms.Label();
		this.CmbJobOrderNoHistory = new UltraCombo();
		this.label88 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrder).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel11.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoHistory).BeginInit();
		this.panel1.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.grd.AllowDelete = (InheritableBoolean)1;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 125);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(595, 437);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 4;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 96);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(595, 29);
		this.panel6.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(555, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 11;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(3, 5);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(375, 19);
		this.label3.TabIndex = 25;
		this.label3.Text = "Output Items Information Against Job Order ";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.label7);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrder);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 59);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(595, 37);
		this.panel5.TabIndex = 2;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(7, 11);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(59, 15);
		this.label7.TabIndex = 5;
		this.label7.Text = "Job Order";
		this.CmbJobOrder.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrder.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Location = new System.Drawing.Point(92, 5);
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Name = "CmbJobOrder";
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Leave += new System.EventHandler(CmbJobOrder_Leave);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 30);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(595, 29);
		this.panel3.TabIndex = 2;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(2, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(411, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Production Output Allocation With Export Invoice";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnsave, this.btnUpdate, this.btnPrint627 });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(595, 27);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
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
		this.btnPrint627.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint627.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint627.Name = "btnPrint627";
		this.btnPrint627.Size = new System.Drawing.Size(92, 24);
		this.btnPrint627.Text = "Print-627";
		this.btnPrint627.Click += new System.EventHandler(btnPrint627_Click);
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(609, 591);
		this.tabControl1.TabIndex = 1;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.toolStrip1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(601, 565);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add(this.panel20);
		this.tabPage2.Controls.Add(this.panel1);
		this.tabPage2.Controls.Add(this.toolStrip2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(601, 565);
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 138);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(595, 424);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 72;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar2);
		this.panel11.Controls.Add(this.label5);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 112);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(595, 26);
		this.panel11.TabIndex = 71;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(552, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(7);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar2.TabIndex = 211;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 2);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(132, 18);
		this.label5.TabIndex = 69;
		this.label5.Text = "Filtered Records";
		this.panel20.BackColor = System.Drawing.Color.White;
		this.panel20.Controls.Add(this.btnShow);
		this.panel20.Controls.Add(this.label2);
		this.panel20.Controls.Add(this.ToDateHistory);
		this.panel20.Controls.Add(this.FromDateHistory);
		this.panel20.Controls.Add(this.label86);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderNoHistory);
		this.panel20.Controls.Add(this.label88);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(3, 59);
		this.panel20.Margin = new System.Windows.Forms.Padding(2);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(595, 53);
		this.panel20.TabIndex = 70;
		this.btnShow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShow.FlatAppearance.BorderSize = 0;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShow.Location = new System.Drawing.Point(519, 23);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(68, 26);
		this.btnShow.TabIndex = 148;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(144, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 147;
		this.label2.Text = "Date To";
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(144, 25);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(132, 23);
		this.ToDateHistory.TabIndex = 2;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(7, 25);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(135, 23);
		this.FromDateHistory.TabIndex = 1;
		this.label86.AutoSize = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label86.Location = new System.Drawing.Point(7, 4);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(66, 15);
		this.label86.TabIndex = 146;
		this.label86.Text = "Date From ";
		this.CmbJobOrderNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoHistory).Location = new System.Drawing.Point(278, 23);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoHistory).Name = "CmbJobOrderNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoHistory).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoHistory).TabIndex = 5;
		this.label88.AutoSize = true;
		this.label88.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label88.Location = new System.Drawing.Point(278, 4);
		this.label88.Name = "label88";
		this.label88.Size = new System.Drawing.Size(78, 15);
		this.label88.TabIndex = 144;
		this.label88.Text = "Job Order No";
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 30);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(595, 29);
		this.panel1.TabIndex = 5;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(2, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(67, 19);
		this.label1.TabIndex = 25;
		this.label1.Text = "History";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(595, 27);
		this.toolStrip2.TabIndex = 4;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(609, 591);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "ProductionOutputAllocationWithExportInvoice";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DefineProductionPlant_FormClosing);
		base.Load += new System.EventHandler(DefineCountry_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DefineCountry_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrder).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoHistory).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
