using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Mfg;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Mfg;
using Architecture.WinApp.Common;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Mfg;

public class frmProductionRouting : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtOperationProcess = new DataTable();

	private DataTable dtHistory = new DataTable();

	private int RecId;

	private int updateDetailIndex = -1;

	private bool FormInitialized;

	private List<productionRouting> lstRemoveRecord = new List<productionRouting>();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnPrint;

	internal UltraCombo combranches;

	private Label label45;

	internal UltraCombo comproject;

	private Label label46;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator1;

	private Label label21;

	private Panel PanelForm;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnDelete;

	private ToolStripButton btnshortcutkeys;

	private Panel panel38;

	private Label label32;

	internal GridEX DatagridHistory;

	private Panel panel15;

	private Label label17;

	private Panel panel13;

	private GroupBox groupBox1;

	internal UltraCombo CmbroutingNameHistory;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdOperationsDetail;

	private Panel panel16;

	internal UltraCombo cmbWorkStationOperation;

	private Button BtnCancelOp;

	private Button BtnUpdateOp;

	private Button BtnAddOp;

	private Label label3;

	private Label label8;

	private TextBox txtCapacityQty;

	private Label label2;

	private Panel panel1;

	private Label label1;

	internal UltraCombo CmbRountingName;

	private CheckBox chkIsActive;

	private Button BtnLookup;

	private Label label5;

	private TextBox txtMinutes;

	private Label label4;

	private TextBox txtHours;

	private Label label6;

	private TextBox txtSeqNo;

	public frmProductionRouting(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmBillOfMaterial_Manufacturing_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 750);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			FormInitialized = true;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			RoutingNameBind();
			AllWorkStationOperationFill();
			dtOperationProcess.Columns.Add("Id", typeof(int));
			dtOperationProcess.Columns.Add("OperationId", typeof(int));
			dtOperationProcess.Columns.Add("OperationName", typeof(string));
			dtOperationProcess.Columns.Add("WorkStation", typeof(string));
			dtOperationProcess.Columns.Add("CapacityQty", typeof(decimal));
			dtOperationProcess.Columns.Add("Hours", typeof(int));
			dtOperationProcess.Columns.Add("Minutes", typeof(int));
			dtOperationProcess.Columns.Add("SeqNo", typeof(int));
			dtOperationProcess.Columns.Add("Active", typeof(bool));
			grdOperationsDetail.DataSource = dtOperationProcess;
			grdOperationsDetail.RetrieveStructure();
			grdOperationsDetailSetting();
			txtCapacityQty.Enabled = true;
			btnDelete.Visible = false;
			CmbRountingName.Focus();
			HistoryRoutingNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryRoutingNameBind()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		try
		{
			int Idname = Conversion.ToInt(CmbroutingNameHistory.Value);
			DataTable dtBOMName = new DataTable();
			dtBOMName.Columns.Add("Id", typeof(int));
			dtBOMName.Columns.Add("Name");
			DataTable dtcombo = productionRouting.GetDataForDropDownFromProductionRouting(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "RoutingName"
			});
			if (dtcombo.Rows.Count > 0)
			{
				for (int i = 0; i < dtcombo.Rows.Count; i++)
				{
					dtBOMName.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtBOMName.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtBOMName, CmbroutingNameHistory, "Id", "Name", "Routing Name", ZeroIndex: false);
			if (Idname > 0)
			{
				if (dtBOMName.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idname))
				{
					CmbroutingNameHistory.Value = Idname;
				}
				else
				{
					((Control)(object)CmbroutingNameHistory).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RoutingNameBind()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbRountingName.Value);
			DataTable dtBOMName = new DataTable();
			dtBOMName.Columns.Add("Id", typeof(int));
			dtBOMName.Columns.Add("Name");
			DataTable dtcombo = ProductionRoutingName.FormHistory(new ProductionRouting
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtcombo.Rows.Count > 0)
			{
				for (int i = 0; i < dtcombo.Rows.Count; i++)
				{
					dtBOMName.Rows.Add(dtcombo.Rows[i]["ProductionRoutingNameId"], dtcombo.Rows[i]["ProductionRoutingName"]);
				}
			}
			if (dtBOMName.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtBOMName, CmbRountingName, "Id", "Name", "Routing Name", ZeroIndex: false);
			((UltraGridBase)CmbRountingName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Id > 0)
			{
				if (dtBOMName.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbRountingName.Value = Id;
				}
				else
				{
					((Control)(object)CmbRountingName).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllWorkStationOperationFill()
	{
		try
		{
			int Id = Conversion.ToInt(cmbWorkStationOperation.Value);
			DataTable dtWorkStationOpertaion = WorkStationOperation.GetWorkStationOpertaionByWSOId(UserAccount.OrganizationId, UserAccount.CompanyId, 0);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Description", typeof(string));
			dt.Columns.Add("WorkStationName", typeof(string));
			dt.Columns.Add("OperationName", typeof(string));
			if (dtWorkStationOpertaion.Rows.Count > 0)
			{
				dt.Clear();
				foreach (DataRow r in dtWorkStationOpertaion.Rows)
				{
					dt.Rows.Add(r["Id"], r["Description"], r["WorkStationName"], r["OperationName"]);
				}
			}
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dt, cmbWorkStationOperation, "Id", "Description", "Work Station/Operation", ZeroIndex: true);
			((UltraGridBase)cmbWorkStationOperation).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbWorkStationOperation).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			((UltraGridBase)cmbWorkStationOperation).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbWorkStationOperation.Value = Id;
				}
				else
				{
					((Control)(object)cmbWorkStationOperation).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbWorkStationOperation_Leave(object sender, EventArgs e)
	{
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)cmbWorkStationOperation).ActiveRow == null || Conversion.ToInt(cmbWorkStationOperation.Value) == 0)
		{
			MessageBox.Show("Operation Name Field is Required");
			cmbWorkStationOperation.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCapacityQty.Text) == 0.0)
		{
			MessageBox.Show("CapacityQty Field is Required");
			txtCapacityQty.Focus();
			return false;
		}
		return true;
	}

	private void BtnAddOp_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			int OperationId = Conversion.ToInt(cmbWorkStationOperation.Value);
			if (grdOperationsDetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdOperationsDetail.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (OperationId == Conversion.ToInt(item.Cells["OperationId"].Value))
					{
						throw new Exception("Operation Already Exist in Grid!");
					}
				}
			}
			string WorkStationName = "";
			string OperationName = "";
			DataTable dt = (DataTable)((UltraGridBase)cmbWorkStationOperation).DataSource;
			if (dt != null && dt.Rows.Count > 0)
			{
				DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == OperationId);
				WorkStationName = ((foundRow != null) ? Conversion.ToString(foundRow["WorkStationName"]) : "");
				OperationName = ((foundRow != null) ? Conversion.ToString(foundRow["OperationName"]) : "");
			}
			dtOperationProcess.Rows.Add(0, OperationId, OperationName, WorkStationName, Conversion.ToDecimal((object)txtCapacityQty.Text.Trim()), Conversion.ToInt((object)txtHours.Text), Conversion.ToInt((object)txtMinutes.Text), Conversion.ToInt((object)txtSeqNo.Text), Conversion.ToBool((object)chkIsActive.Checked));
			grdOperationsDetail.DataSource = dtOperationProcess;
			grdOperationsDetail.RetrieveStructure();
			grdOperationsDetailSetting();
			FromResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateOp_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			int OperationId = Conversion.ToInt(cmbWorkStationOperation.Value);
			if (grdOperationsDetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdOperationsDetail.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (OperationId == Conversion.ToInt(item.Cells["OperationId"].Value) && updateDetailIndex != item.RowIndex)
					{
						throw new Exception("WorkStation Operation Already Exist in Grid!");
					}
				}
			}
			string WorkStationName = "";
			string OperationName = "";
			DataTable dt = (DataTable)((UltraGridBase)cmbWorkStationOperation).DataSource;
			if (dt != null && dt.Rows.Count > 0)
			{
				DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == OperationId);
				WorkStationName = ((foundRow != null) ? Conversion.ToString(foundRow["WorkStationName"]) : "");
				OperationName = ((foundRow != null) ? Conversion.ToString(foundRow["OperationName"]) : "");
			}
			dtOperationProcess.Rows[updateDetailIndex]["OperationId"] = Conversion.ToInt(cmbWorkStationOperation.Value);
			dtOperationProcess.Rows[updateDetailIndex]["OperationName"] = OperationName;
			dtOperationProcess.Rows[updateDetailIndex]["WorkStation"] = WorkStationName;
			dtOperationProcess.Rows[updateDetailIndex]["CapacityQty"] = Conversion.ToDecimal((object)txtCapacityQty.Text);
			dtOperationProcess.Rows[updateDetailIndex]["Hours"] = Conversion.ToInt((object)txtHours.Text);
			dtOperationProcess.Rows[updateDetailIndex]["Minutes"] = Conversion.ToInt((object)txtMinutes.Text);
			dtOperationProcess.Rows[updateDetailIndex]["SeqNo"] = Conversion.ToInt((object)txtSeqNo.Text);
			dtOperationProcess.Rows[updateDetailIndex]["Active"] = Conversion.ToBool((object)chkIsActive.Checked);
			grdOperationsDetail.DataSource = dtOperationProcess;
			FromResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelOp_Click(object sender, EventArgs e)
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

	private void grdOperationsDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdOperationsDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				cmbWorkStationOperation.Value = Conversion.ToInt(item.Cells["OperationId"].Value);
				txtCapacityQty.Text = Conversion.ToDecimal(item.Cells["CapacityQty"].Value).ToString("#,##.###");
				txtHours.Text = Conversion.ToInt(item.Cells["Hours"].Value).ToString();
				txtMinutes.Text = Conversion.ToInt(item.Cells["Minutes"].Value).ToString();
				txtSeqNo.Text = Conversion.ToInt(item.Cells["SeqNo"].Value).ToString();
				chkIsActive.Checked = Conversion.ToBool(item.Cells["Active"].Value);
				BtnAddOp.Visible = false;
				BtnUpdateOp.Visible = true;
				BtnCancelOp.Visible = true;
				cmbWorkStationOperation.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOperationsDetailSetting()
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOperationsDetail.RootTable.Columns["Id"].Visible = false;
			grdOperationsDetail.RootTable.Columns["OperationId"].Visible = false;
			grdOperationsDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdOperationsDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			CommonServices.GridColumnSettings(grdOperationsDetail.RootTable.Columns["CapacityQty"]);
			grdOperationsDetail.AllowEdit = (InheritableBoolean)1;
			grdOperationsDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdOperationsDetail.RootTable.Columns["OperationId"].EditType = (EditType)0;
			grdOperationsDetail.RootTable.Columns["OperationName"].EditType = (EditType)0;
			grdOperationsDetail.RootTable.Columns["WorkStation"].EditType = (EditType)0;
			grdOperationsDetail.RootTable.Columns["CapacityQty"].EditType = (EditType)1;
			grdOperationsDetail.RootTable.Columns["Hours"].EditType = (EditType)1;
			grdOperationsDetail.RootTable.Columns["Minutes"].EditType = (EditType)1;
			grdOperationsDetail.RootTable.Columns["SeqNo"].EditType = (EditType)1;
			grdOperationsDetail.RootTable.Columns["Active"].EditType = (EditType)8;
			grdOperationsDetail.RootTable.Columns["OperationName"].Width = 130;
			grdOperationsDetail.RootTable.Columns["WorkStation"].Width = 150;
			grdOperationsDetail.RootTable.Columns["CapacityQty"].Width = 70;
			grdOperationsDetail.RootTable.Columns["Hours"].Width = 45;
			grdOperationsDetail.RootTable.Columns["Minutes"].Width = 60;
			grdOperationsDetail.RootTable.Columns["SeqNo"].Width = 45;
			grdOperationsDetail.RootTable.Columns["Active"].Width = 50;
			grdOperationsDetail.RootTable.Columns.Add("Delete");
			grdOperationsDetail.RootTable.Columns["Delete"].Caption = "X";
			grdOperationsDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOperationsDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOperationsDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdOperationsDetail.RootTable.Columns["Delete"].Width = 20;
			grdOperationsDetail.RootTable.Columns["Delete"].Position = 0;
			grdOperationsDetail.RootTable.Columns.Add("Edit");
			grdOperationsDetail.RootTable.Columns["Edit"].Caption = "Edit";
			grdOperationsDetail.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOperationsDetail.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdOperationsDetail.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdOperationsDetail.RootTable.Columns["Edit"].Width = 40;
			grdOperationsDetail.RootTable.Columns["Edit"].Position = 1;
			grdOperationsDetail.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOperationsDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdOperationsDetail.UpdateData();
			GridEXRow item = grdOperationsDetail.CurrentRow;
			if (e.Column.Key == "Minutes" && Conversion.ToInt(item.Cells["Minutes"].Value) > 60)
			{
				item.Cells["Minutes"].Value = 0;
			}
			grdOperationsDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOperationsDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdOperationsDetail.CurrentRow;
			if (e.Column.Key == "Delete")
			{
				DeleteDetailRow(r);
			}
			if (e.Column.Key == "Edit")
			{
				grdOperationsDetail_DoubleClick(null, null);
			}
			grdOperationsDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				if (updateDetailIndex != -1)
				{
					throw new Exception("Please Reset the Detail First...");
				}
				if (Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) != 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					productionRouting d1 = new productionRouting();
					d1.productionRoutingId = Conversion.ToInt(r.Cells["Id"].Value);
					d1.WorkStationOperationId = Conversion.ToInt(r.Cells["OperationId"].Value);
					d1.CapacityQty = Conversion.ToDecimal((object)r.Cells["CapacityQty"].Text);
					d1.sequenceNo = Conversion.ToInt((object)r.Cells["SeqNo"].Text);
					d1.operationTime = Conversion.ToString(r.Cells["OperationTime"].Value);
					d1.isActive = Conversion.ToBool(r.Cells["Active"].Value);
					int h = Conversion.ToInt(r.Cells["Hours"].Value);
					int i = Conversion.ToInt(r.Cells["Minutes"].Value);
					string timeString = new TimeSpan(h, i, 0).ToString("hh\\:mm\\:ss");
					d1.operationTime = timeString;
					d1.ActionId = 3;
					d1.CompanyId = UserAccount.CompanyId;
					d1.BranchId = UserAccount.BranchesId;
					d1.OrganizationId = UserAccount.OrganizationId;
					d1.EntryUserId = UserAccount.ID;
					d1.EntryDate = DateTime.Now;
					d1.ModifyDate = DateTime.Now;
					d1.ModifyUserId = UserAccount.ID;
					lstRemoveRecord.Add(d1);
					dtOperationProcess.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					dtOperationProcess.Rows[r.RowIndex].Delete();
					r.Delete();
				}
			}
			grdOperationsDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		FromReset();
	}

	private void FromReset()
	{
		try
		{
			RecId = 0;
			lstRemoveRecord.Clear();
			FromResetDetail();
			dtOperationProcess.Rows.Clear();
			grdOperationsDetail.ClearStructure();
			((Control)(object)CmbRountingName).Text = string.Empty;
			((Control)(object)cmbWorkStationOperation).Text = string.Empty;
			txtCapacityQty.Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			CmbRountingName.Focus();
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
			((Control)(object)cmbWorkStationOperation).Text = string.Empty;
			txtCapacityQty.Text = string.Empty;
			txtHours.Text = string.Empty;
			txtMinutes.Text = string.Empty;
			txtSeqNo.Text = string.Empty;
			BtnAddOp.Visible = true;
			BtnCancelOp.Visible = false;
			BtnUpdateOp.Visible = false;
			cmbWorkStationOperation.Focus();
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
			RoutingNameBind();
			AllWorkStationOperationFill();
			HistoryRoutingNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmProductionRouting_FormClosed(object sender, FormClosedEventArgs e)
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

	private void BtnCancelOp_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "tabControl1")
			{
				cmbWorkStationOperation.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddOp_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "cmbWorkStationOperation")
			{
				cmbWorkStationOperation.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private List<int> GetMissingSequence(DataTable table)
	{
		try
		{
			List<int> seqNos = (from row in table.AsEnumerable()
				select row.Field<int>("SeqNo")).ToList();
			seqNos.Sort();
			int totalRows = table.Rows.Count;
			List<int> missingSeqNos = new List<int>();
			for (int i = 1; i <= totalRows; i++)
			{
				if (!seqNos.Contains(i))
				{
					missingSeqNos.Add(i);
				}
			}
			return missingSeqNos;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		try
		{
			if (string.IsNullOrEmpty(((Control)(object)CmbRountingName).Text))
			{
				MessageBox.Show("Routing Name is Required...");
				CmbRountingName.Focus();
				return;
			}
			if (grdOperationsDetail.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found...\nGrid Should have atleast one record");
				return;
			}
			productionRouting Or = new productionRouting();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				Or.WorkStationOperationId = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			Or.CompanyId = UserAccount.CompanyId;
			Or.BranchId = UserAccount.BranchesId;
			Or.OrganizationId = UserAccount.OrganizationId;
			Or.EntryUserId = UserAccount.ID;
			Or.EntryDate = DateTime.Now;
			Or.ModifyDate = DateTime.Now;
			Or.ModifyUserId = UserAccount.ID;
			Or.ProductionRoutingList = new List<productionRouting>();
			DataTable dt = (DataTable)grdOperationsDetail.DataSource;
			List<int> missingSeqNos = GetMissingSequence(dt);
			string seqNos = "";
			if (missingSeqNos.Count > 0)
			{
				foreach (int item in missingSeqNos)
				{
					seqNos = seqNos + item + ",";
				}
				throw new Exception("Missing sequence numbers:" + seqNos);
			}
			if (lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					Or.ProductionRoutingList.Add(lstRemoveRecord[i]);
				}
			}
			GridEXRow[] rows = grdOperationsDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["OperationId"].Value) != 0)
				{
					productionRouting d1 = new productionRouting();
					if (Conversion.ToInt(r.Cells["OperationId"].Value) == 0)
					{
						MessageBox.Show("WorkStationOperation Field Required in Grid Row No: " + (r.RowIndex + 1));
						return;
					}
					if (Conversion.ToDouble((object)r.Cells["CapacityQty"].Text) == 0.0)
					{
						MessageBox.Show("CapacityQty Field Required in Grid Row No: " + (r.RowIndex + 1));
						return;
					}
					d1.productionRoutingId = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					d1.ActionId = ((d1.productionRoutingId <= 0) ? 1 : 2);
					d1.sequenceNo = Conversion.ToInt(r.Cells["SeqNo"].Value);
					d1.RoutingNameId = Conversion.ToInt(CmbRountingName.Value);
					d1.routingName = ((Control)(object)CmbRountingName).Text.Trim();
					d1.WorkStationOperationId = Conversion.ToInt(r.Cells["OperationId"].Value);
					d1.CapacityQty = Conversion.ToDecimal((object)r.Cells["CapacityQty"].Text);
					int h = Conversion.ToInt(r.Cells["Hours"].Value);
					int j = Conversion.ToInt(r.Cells["Minutes"].Value);
					if (h == 0 && j == 0)
					{
						throw new Exception("Hours Or Minutes Required in Grid Row No:" + (r.RowIndex + 1));
					}
					string timeString = new TimeSpan(h, j, 0).ToString("hh\\:mm\\:ss");
					d1.operationTime = timeString;
					d1.isActive = Conversion.ToBool(r.Cells["Active"].Value);
					d1.CompanyId = UserAccount.CompanyId;
					d1.BranchId = UserAccount.BranchesId;
					d1.OrganizationId = UserAccount.OrganizationId;
					d1.EntryUserId = UserAccount.ID;
					d1.EntryDate = DateTime.Now;
					d1.ModifyDate = DateTime.Now;
					d1.ModifyUserId = UserAccount.ID;
					Or.ProductionRoutingList.Add(d1);
				}
			}
			int success = productionRouting.Save(Or);
			Or.productionRoutingId = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + Or.routingName);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + Or.routingName);
			}
			FromReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("RecId Not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(DataTable Or)
	{
		try
		{
			dtOperationProcess.Clear();
			grdOperationsDetail.ClearStructure();
			if (Or != null && Or.Rows.Count > 0)
			{
				RecId = Conversion.ToInt(Or.Rows[0]["productionRoutingId"]);
				foreach (DataRow r in Or.Rows)
				{
					TimeSpan timeSpan = TimeSpan.ParseExact(Conversion.ToString(r["operationTime"]), "hh\\:mm\\:ss", null);
					int hours = timeSpan.Hours;
					int minutes = timeSpan.Minutes;
					_ = timeSpan.Seconds;
					dtOperationProcess.Rows.Add(r["productionRoutingId"], r["WorkStationOperationId"], r["OperationName"], r["WorkStationName"], r["CapacityQty"], hours, minutes, r["sequenceNo"], r["isActive"]);
				}
				grdOperationsDetail.DataSource = dtOperationProcess;
				grdOperationsDetail.RetrieveStructure();
				grdOperationsDetailSetting();
				tabControl1.SelectedIndex = 0;
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				btnDelete.Visible = true;
				return;
			}
			throw new Exception("Record not found...");
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
				if (!string.IsNullOrEmpty(((Control)(object)CmbRountingName).Text) || Conversion.ToInt(CmbRountingName.Value) == 0)
				{
					MessageBox.Show("Please Select RoutingName...");
					CmbRountingName.Focus();
				}
				else if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					productionRouting.DeleteByName(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, Conversion.ToInt(CmbRountingName.Value));
					MessageBox.Show("Delete Record Successfully");
					FromReset();
				}
			}
			else
			{
				MessageBox.Show("No record found to Delete");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRountingName_Leave(object sender, EventArgs e)
	{
		try
		{
			int RoutingNameId = Conversion.ToInt(CmbRountingName.Value);
			if (RoutingNameId > 0)
			{
				DataTable Or = productionRouting.ReadByRoutingNameId(RoutingNameId);
				if (Or != null && Or.Rows.Count > 0 && formright.DoHaveUpdateRights && (grdOperationsDetail.GetRows().Count() <= 0 || MessageBox.Show("Are you sure to Refresh Operations Grid?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No))
				{
					ReadById(Or);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtQtyBOM_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtQtyDetail_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1 && Conversion.ToInt((object)((DisposableObjectCollectionBase)((UltraGridBase)CmbroutingNameHistory).Rows).Count) == 0)
		{
			RoutingNameBind();
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

	private void HistoryGridFill()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtHistory.Clear();
			ReportsParameters header = new ReportsParameters();
			header.CompanyId = UserAccount.CompanyId;
			header.OrganizationId = UserAccount.OrganizationId;
			header.BranchesId = UserAccount.BranchesId;
			header.Id = Conversion.ToInt(CmbroutingNameHistory.Value);
			header.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (header.CanViewAllRecord)
			{
				header.EntryUser = UserAccount.ID;
			}
			dtHistory = productionRouting.FormHistory(header);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtProductionRouting = new DataTable();
				dtProductionRouting.Columns.Add("Id", typeof(int));
				dtProductionRouting.Columns.Add("RoutingNameId", typeof(string));
				dtProductionRouting.Columns.Add("RoutingName", typeof(string));
				dtProductionRouting.Columns.Add("SequenceNo", typeof(int));
				dtProductionRouting.Columns.Add("OperationId", typeof(int));
				dtProductionRouting.Columns.Add("OperationName", typeof(string));
				dtProductionRouting.Columns.Add("WorkStation", typeof(string));
				dtProductionRouting.Columns.Add("CapacityQty", typeof(double));
				dtProductionRouting.Columns.Add("Hours", typeof(int));
				dtProductionRouting.Columns.Add("Minutes", typeof(int));
				dtProductionRouting.Columns.Add("Active", typeof(bool));
				dtProductionRouting.Rows.Clear();
				foreach (DataRow row in dtHistory.Rows)
				{
					TimeSpan timeSpan = TimeSpan.ParseExact(Conversion.ToString(row["operationTime"]), "hh\\:mm\\:ss", null);
					int hours = timeSpan.Hours;
					int minutes = timeSpan.Minutes;
					dtProductionRouting.Rows.Add(row["ProductionRoutingId"], row["RoutingNameId"], row["RoutingName"], row["SequenceNo"], row["WorkStationOperationId"], row["OperationName"], row["WorkStationName"], row["CapacityQty"], hours, minutes, row["isActive"]);
				}
				DatagridHistory.DataSource = dtProductionRouting;
				DatagridHistory.RetrieveStructure();
				HistoryGridSetting();
			}
			else
			{
				DatagridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSetting()
	{
		try
		{
			DatagridHistory.RootTable.Columns["Id"].Visible = false;
			DatagridHistory.RootTable.Columns["RoutingNameId"].Visible = false;
			DatagridHistory.RootTable.Columns["OperationId"].Visible = false;
			DatagridHistory.RootTable.Columns["RoutingName"].Visible = false;
			DatagridHistory.RootTable.Groups.Add("RoutingName");
			DatagridHistory.GroupTotals = (GroupTotals)2;
			CommonServices.GridWrappingAndColumnSettings(DatagridHistory, 2);
			DatagridHistory.RootTable.Columns["SequenceNo"].Caption = "Seq No";
			DatagridHistory.RootTable.Columns["RoutingName"].Width = 120;
			DatagridHistory.RootTable.Columns["SequenceNo"].Width = 60;
			DatagridHistory.RootTable.Columns["OperationName"].Width = 120;
			DatagridHistory.RootTable.Columns["CapacityQty"].Width = 70;
			DatagridHistory.RootTable.Columns["Hours"].Width = 50;
			DatagridHistory.RootTable.Columns["Minutes"].Width = 60;
			DatagridHistory.RootTable.Columns["Active"].Width = 45;
			DatagridHistory.RootTable.Columns.Add("Edit");
			DatagridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Width = 40;
			DatagridHistory.RootTable.Columns["Edit"].Position = 0;
			DatagridHistory.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int RoutingNameId = Conversion.ToInt(r.Cells["RoutingNameId"].Value);
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				DataTable Or = productionRouting.ReadByRoutingNameId(RoutingNameId);
				ReadById(Or);
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
			if (r == null || (int)r.RowType != 0 || DatagridHistory.CurrentColumn == null)
			{
				return;
			}
			int RoutingNameId = Conversion.ToInt(r.Cells["RoutingNameId"].Value);
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				DataTable Or = productionRouting.ReadByRoutingNameId(RoutingNameId);
				ReadById(Or);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
	}

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbroutingNameHistory).Text = string.Empty;
			CmbroutingNameHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmBillOfMaterial_Manufacturing_KeyDown(object sender, KeyEventArgs e)
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
					CmbRountingName.Focus();
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
			if (tabControl1.SelectedIndex != 0)
			{
				return;
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.Delete)
			{
				if (formright.DoHaveCanDelete && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				if (btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
			}
			else if (e.Alt && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.U)
			{
				if (btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.P)
			{
				if (btnPrint.Visible && btnPrint.Enabled)
				{
					btnPrint_Click(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbRountingName.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdOperationsDetail).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				CmbRountingName.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdOperationsDetail).Focused)
				{
					((Control)(object)DatagridHistory).Focus();
				}
				else if (((Control)(object)DatagridHistory).Focused)
				{
					((Control)(object)grdOperationsDetail).Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save ");
			dt.Rows.Add("Alt+S", "For Show Data on History");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on RoutingName");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on RoutingName");
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

	private void grdOperationsDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOperationsDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOperationsDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdOperationsDetail.CurrentColumn != null && grdOperationsDetail.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (grdOperationsDetail.CurrentColumn != null && grdOperationsDetail.CurrentColumn.Key == "Edit")
				{
					grdOperationsDetail_DoubleClick(null, null);
				}
				grdOperationsDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdOperationsDetail_DoubleClick(null, null);
			}
			_ = e.KeyCode;
			_ = 112;
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
			int RoutingNameId = Conversion.ToInt(r.Cells["RoutingNameId"].Value);
			if (e.Control && e.KeyCode == Keys.Space && DatagridHistory.CurrentColumn.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				DataTable Or2 = productionRouting.ReadByRoutingNameId(RoutingNameId);
				ReadById(Or2);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				DataTable Or = productionRouting.ReadByRoutingNameId(RoutingNameId);
				ReadById(Or);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, DatagridHistory, dtHistory);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCapacityQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void BtnLookup_Click(object sender, EventArgs e)
	{
		try
		{
			new ProductionRoutingName(UserAccount).ShowDialog();
			RoutingNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtHours_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtMinutes_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSeqNo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtMinutes_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)txtMinutes.Text) > 60)
			{
				txtMinutes.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOperationsDetail_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "SeqNo" || e.Column.Key == "Hours" || e.Column.Key == "Minutes" || e.Column.Key == "CapacityQty") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
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
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Expected O, but got Unknown
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_07dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e7: Expected O, but got Unknown
		//IL_07f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Expected O, but got Unknown
		//IL_1494: Unknown result type (might be due to invalid IL or missing references)
		//IL_149e: Expected O, but got Unknown
		//IL_14ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b5: Expected O, but got Unknown
		//IL_14c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14cc: Expected O, but got Unknown
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		GridEXLayout grdOperationsDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.DatagridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbroutingNameHistory = new UltraCombo();
		this.panel38 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label32 = new System.Windows.Forms.Label();
		this.PanelForm = new System.Windows.Forms.Panel();
		this.grdOperationsDetail = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.txtSeqNo = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtMinutes = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtHours = new System.Windows.Forms.TextBox();
		this.cmbWorkStationOperation = new UltraCombo();
		this.BtnCancelOp = new System.Windows.Forms.Button();
		this.chkIsActive = new System.Windows.Forms.CheckBox();
		this.BtnAddOp = new System.Windows.Forms.Button();
		this.BtnUpdateOp = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtCapacityQty = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.BtnLookup = new System.Windows.Forms.Button();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbRountingName = new UltraCombo();
		this.combranches = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.comproject = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.label21 = new System.Windows.Forms.Label();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbroutingNameHistory).BeginInit();
		this.panel38.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.PanelForm.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOperationsDetail).BeginInit();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWorkStationOperation).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRountingName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1072, 711);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Controls.Add(this.PanelForm);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1064, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.panel1.Controls.Add(this.panel15);
		this.panel1.Controls.Add(this.panel13);
		this.panel1.Controls.Add(this.panel38);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(626, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(435, 676);
		this.panel1.TabIndex = 2;
		this.DatagridHistory.AllowEdit = (InheritableBoolean)2;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(0, 121);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(435, 555);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 0;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.DatagridHistory.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DatagridHistory_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label17);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 98);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(435, 23);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(395, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Visible = false;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(8, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(112, 16);
		this.label17.TabIndex = 127;
		this.label17.Text = "Filtered Records";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 52);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(435, 46);
		this.panel13.TabIndex = 0;
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbroutingNameHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(434, 45);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(367, 14);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 25);
		this.btnshow.TabIndex = 1;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(84, 15);
		this.label1.TabIndex = 131;
		this.label1.Text = "Routing Name";
		this.CmbroutingNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbroutingNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbroutingNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbroutingNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbroutingNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbroutingNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbroutingNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbroutingNameHistory).Location = new System.Drawing.Point(89, 13);
		((System.Windows.Forms.Control)(object)this.CmbroutingNameHistory).Name = "CmbroutingNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbroutingNameHistory).Size = new System.Drawing.Size(273, 26);
		((System.Windows.Forms.Control)(object)this.CmbroutingNameHistory).TabIndex = 0;
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.toolStrip2);
		this.panel38.Controls.Add(this.label32);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(435, 52);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.toolStripSeparator1 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(435, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnHistoryRefresh_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(5, 31);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(239, 19);
		this.label32.TabIndex = 127;
		this.label32.Text = "Production Routing History  ";
		this.PanelForm.Controls.Add((System.Windows.Forms.Control)(object)this.grdOperationsDetail);
		this.PanelForm.Controls.Add(this.panel16);
		this.PanelForm.Controls.Add(this.panel2);
		this.PanelForm.Controls.Add(this.panel3);
		this.PanelForm.Dock = System.Windows.Forms.DockStyle.Left;
		this.PanelForm.Location = new System.Drawing.Point(3, 3);
		this.PanelForm.Name = "PanelForm";
		this.PanelForm.Size = new System.Drawing.Size(623, 676);
		this.PanelForm.TabIndex = 1;
		this.grdOperationsDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdOperationsDetail).BackColor = System.Drawing.Color.White;
		grdOperationsDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOperationsDetail.DesignTimeLayout = grdOperationsDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOperationsDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).Location = new System.Drawing.Point(0, 144);
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).Name = "grdOperationsDetail";
		((ScrollableBase)this.grdOperationsDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).Size = new System.Drawing.Size(623, 532);
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).TabIndex = 2;
		this.grdOperationsDetail.TotalRow = (InheritableBoolean)1;
		this.grdOperationsDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOperationsDetail).VisualStyle = (VisualStyle)4;
		this.grdOperationsDetail.CellUpdated += new ColumnActionEventHandler(grdOperationsDetail_CellUpdated);
		this.grdOperationsDetail.UpdatingCell += new UpdatingCellEventHandler(grdOperationsDetail_UpdatingCell);
		this.grdOperationsDetail.ColumnButtonClick += new ColumnActionEventHandler(grdOperationsDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).DoubleClick += new System.EventHandler(grdOperationsDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOperationsDetail_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.White;
		this.panel16.Controls.Add(this.label6);
		this.panel16.Controls.Add(this.txtSeqNo);
		this.panel16.Controls.Add(this.label5);
		this.panel16.Controls.Add(this.txtMinutes);
		this.panel16.Controls.Add(this.label4);
		this.panel16.Controls.Add(this.txtHours);
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWorkStationOperation);
		this.panel16.Controls.Add(this.BtnCancelOp);
		this.panel16.Controls.Add(this.chkIsActive);
		this.panel16.Controls.Add(this.BtnAddOp);
		this.panel16.Controls.Add(this.BtnUpdateOp);
		this.panel16.Controls.Add(this.label2);
		this.panel16.Controls.Add(this.label3);
		this.panel16.Controls.Add(this.txtCapacityQty);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 90);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(623, 54);
		this.panel16.TabIndex = 1;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(504, 5);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(46, 15);
		this.label6.TabIndex = 137;
		this.label6.Text = "Seq No";
		this.txtSeqNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSeqNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSeqNo.Location = new System.Drawing.Point(506, 27);
		this.txtSeqNo.Name = "txtSeqNo";
		this.txtSeqNo.Size = new System.Drawing.Size(44, 23);
		this.txtSeqNo.TabIndex = 5;
		this.txtSeqNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSeqNo_KeyPress);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(448, 5);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(50, 15);
		this.label5.TabIndex = 135;
		this.label5.Text = "Minutes";
		this.txtMinutes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMinutes.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMinutes.Location = new System.Drawing.Point(447, 27);
		this.txtMinutes.Name = "txtMinutes";
		this.txtMinutes.Size = new System.Drawing.Size(58, 23);
		this.txtMinutes.TabIndex = 4;
		this.txtMinutes.TextChanged += new System.EventHandler(txtMinutes_TextChanged);
		this.txtMinutes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMinutes_KeyPress);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(395, 5);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(39, 15);
		this.label4.TabIndex = 133;
		this.label4.Text = "Hours";
		this.txtHours.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtHours.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtHours.Location = new System.Drawing.Point(394, 27);
		this.txtHours.Name = "txtHours";
		this.txtHours.Size = new System.Drawing.Size(52, 23);
		this.txtHours.TabIndex = 3;
		this.txtHours.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtHours_KeyPress);
		this.cmbWorkStationOperation.AllowNull = (DefaultableBoolean)1;
		this.cmbWorkStationOperation.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWorkStationOperation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWorkStationOperation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWorkStationOperation).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWorkStationOperation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWorkStationOperation).Location = new System.Drawing.Point(5, 25);
		((System.Windows.Forms.Control)(object)this.cmbWorkStationOperation).Name = "cmbWorkStationOperation";
		((System.Windows.Forms.Control)(object)this.cmbWorkStationOperation).Size = new System.Drawing.Size(306, 26);
		((System.Windows.Forms.Control)(object)this.cmbWorkStationOperation).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbWorkStationOperation).Leave += new System.EventHandler(cmbWorkStationOperation_Leave);
		this.BtnCancelOp.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelOp.FlatAppearance.BorderSize = 0;
		this.BtnCancelOp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelOp.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelOp.ForeColor = System.Drawing.Color.White;
		this.BtnCancelOp.Location = new System.Drawing.Point(553, 3);
		this.BtnCancelOp.Name = "BtnCancelOp";
		this.BtnCancelOp.Size = new System.Drawing.Size(64, 22);
		this.BtnCancelOp.TabIndex = 8;
		this.BtnCancelOp.Text = "Cancel";
		this.BtnCancelOp.UseVisualStyleBackColor = false;
		this.BtnCancelOp.Visible = false;
		this.BtnCancelOp.Click += new System.EventHandler(BtnCancelOp_Click);
		this.BtnCancelOp.Leave += new System.EventHandler(BtnCancelOp_Leave);
		this.chkIsActive.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkIsActive.AutoSize = true;
		this.chkIsActive.Checked = true;
		this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkIsActive.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIsActive.ForeColor = System.Drawing.Color.Black;
		this.chkIsActive.Location = new System.Drawing.Point(249, 3);
		this.chkIsActive.Name = "chkIsActive";
		this.chkIsActive.Size = new System.Drawing.Size(59, 19);
		this.chkIsActive.TabIndex = 1;
		this.chkIsActive.Text = "Active";
		this.chkIsActive.UseVisualStyleBackColor = true;
		this.BtnAddOp.BackColor = System.Drawing.Color.Teal;
		this.BtnAddOp.FlatAppearance.BorderSize = 0;
		this.BtnAddOp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddOp.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddOp.ForeColor = System.Drawing.Color.White;
		this.BtnAddOp.Location = new System.Drawing.Point(553, 27);
		this.BtnAddOp.Name = "BtnAddOp";
		this.BtnAddOp.Size = new System.Drawing.Size(38, 22);
		this.BtnAddOp.TabIndex = 6;
		this.BtnAddOp.Text = "+";
		this.BtnAddOp.UseVisualStyleBackColor = false;
		this.BtnAddOp.Click += new System.EventHandler(BtnAddOp_Click);
		this.BtnAddOp.Leave += new System.EventHandler(BtnAddOp_Leave);
		this.BtnUpdateOp.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateOp.FlatAppearance.BorderSize = 0;
		this.BtnUpdateOp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateOp.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateOp.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateOp.Location = new System.Drawing.Point(553, 27);
		this.BtnUpdateOp.Name = "BtnUpdateOp";
		this.BtnUpdateOp.Size = new System.Drawing.Size(64, 22);
		this.BtnUpdateOp.TabIndex = 7;
		this.BtnUpdateOp.Text = "Update";
		this.BtnUpdateOp.UseVisualStyleBackColor = false;
		this.BtnUpdateOp.Visible = false;
		this.BtnUpdateOp.Click += new System.EventHandler(BtnUpdateOp_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(314, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(74, 15);
		this.label2.TabIndex = 131;
		this.label2.Text = "Capacity Qty";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(7, 5);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(146, 15);
		this.label3.TabIndex = 19;
		this.label3.Text = "Work Station / Operations";
		this.txtCapacityQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCapacityQty.Enabled = false;
		this.txtCapacityQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCapacityQty.Location = new System.Drawing.Point(313, 27);
		this.txtCapacityQty.Name = "txtCapacityQty";
		this.txtCapacityQty.Size = new System.Drawing.Size(80, 23);
		this.txtCapacityQty.TabIndex = 2;
		this.txtCapacityQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCapacityQty_KeyPress);
		this.panel2.Controls.Add(this.BtnLookup);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRountingName);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel2.Controls.Add(this.label45);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel2.Controls.Add(this.label46);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 55);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(623, 35);
		this.panel2.TabIndex = 0;
		this.BtnLookup.BackColor = System.Drawing.Color.White;
		this.BtnLookup.Font = new System.Drawing.Font("Verdana", 11.75f);
		this.BtnLookup.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnLookup.Location = new System.Drawing.Point(392, 3);
		this.BtnLookup.Margin = new System.Windows.Forms.Padding(2);
		this.BtnLookup.Name = "BtnLookup";
		this.BtnLookup.Size = new System.Drawing.Size(28, 27);
		this.BtnLookup.TabIndex = 132;
		this.BtnLookup.TabStop = false;
		this.BtnLookup.UseVisualStyleBackColor = false;
		this.BtnLookup.Click += new System.EventHandler(BtnLookup_Click);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(5, 9);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(84, 15);
		this.label8.TabIndex = 131;
		this.label8.Text = "Routing Name";
		this.CmbRountingName.AllowNull = (DefaultableBoolean)1;
		this.CmbRountingName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRountingName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRountingName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRountingName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.CmbRountingName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRountingName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRountingName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbRountingName).Location = new System.Drawing.Point(91, 3);
		((System.Windows.Forms.Control)(object)this.CmbRountingName).Name = "CmbRountingName";
		((System.Windows.Forms.Control)(object)this.CmbRountingName).Size = new System.Drawing.Size(301, 26);
		((System.Windows.Forms.Control)(object)this.CmbRountingName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbRountingName).Leave += new System.EventHandler(CmbRountingName_Leave);
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance49;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance31).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1240, 45);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(58, 27);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(1186, 50);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(47, 17);
		this.label45.TabIndex = 127;
		this.label45.Text = "Branch";
		this.label45.Visible = false;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance18).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1240, 78);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(58, 27);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(1186, 83);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(48, 17);
		this.label46.TabIndex = 128;
		this.label46.Text = "Project";
		this.label46.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(623, 55);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnPrint, this.toolStripSeparator, this.btnshortcutkeys, this.btnDelete });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(623, 27);
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
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(68, 24);
		this.btnPrint.Text = "-&Print";
		this.btnPrint.Visible = false;
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(6, 32);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(166, 19);
		this.label21.TabIndex = 126;
		this.label21.Text = "Production Routing";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1072, 711);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "frmProductionRouting";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmProductionRouting_FormClosed);
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbroutingNameHistory).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.PanelForm.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOperationsDetail).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWorkStationOperation).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRountingName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
