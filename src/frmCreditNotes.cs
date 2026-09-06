using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Pos;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.POS;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.WholeSale
{

public class frmCreditNotes : Form
{
	private bool DoHaveSaveRight;

	private bool DoHavePrintRights;

	private bool DoHaveUpdateRights;

	private bool DoHaveCanViewAllRecordRights;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtdetail = new DataTable();

	private int RecId;

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

	private ToolStripButton btnsave;

	private Panel panel13;

	private Panel panel17;

	private ToolStrip toolStrip3;

	private ToolStripButton btnUpdate;

	private Label label3;

	private DateTimePicker txtDocDate;

	private ToolStripButton toolStripButton1;

	private Panel panel6;

	private Label label18;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdDetails;

	internal GridEX DataGridHistory;

	private TextBox txtRemarks;

	private Label label5;

	private Panel panel7;

	internal GridEX grdHistoryDetail;

	private Panel panel18;

	private Panel panel5;

	private Label label51;

	private TextBox txtDocNo;

	private Label label8;

	private Label label12;

	private DateTimePicker txtToDate;

	private Label label11;

	private DateTimePicker txtFromDate;

	private Label label4;

	internal UltraCombo cmbDebitAccount;

	private Button btnShowDetail;

	private CtrlGrdBar ctrlGrdBar2;

	public frmCreditNotes(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void FrmExportSalesContract_Load(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			List<tblUserRights> lstRights = tblUserRights.GetByUserId(new tblUserRights
			{
				UserId = UserAccount.ID,
				ScreenName = base.Name,
				RightName = UserAccount.RoleName
			});
			if (lstRights.Count > 0)
			{
				List<tblUserRights> newList = lstRights.Where((tblUserRights x) => x.RightName == "Save").ToList();
				DoHaveSaveRight = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Print").ToList();
				DoHavePrintRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Update").ToList();
				DoHaveUpdateRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "CanView AllRecord").ToList();
				DoHaveCanViewAllRecordRights = Conversion.ToBool((object)newList[0].Value.ToString());
			}
			GenerateCode();
			DebitAccountFill();
			dtdetail.Columns.Add("PolicyId");
			dtdetail.Columns.Add("SupplierCustomerId");
			dtdetail.Columns.Add("SupplierCustomer");
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("TargetBaseTypeId");
			dtdetail.Columns.Add("TargetBaseType");
			dtdetail.Columns.Add("TargetFrom");
			dtdetail.Columns.Add("TargetTo");
			dtdetail.Columns.Add("TargetRangeFrom");
			dtdetail.Columns.Add("TargetRangeTo");
			dtdetail.Columns.Add("IncentiveTypeId");
			dtdetail.Columns.Add("IncentiveType");
			dtdetail.Columns.Add("TotalInvoices", typeof(double));
			dtdetail.Columns.Add("TotalQty", typeof(double));
			dtdetail.Columns.Add("TotalWeight", typeof(double));
			dtdetail.Columns.Add("TotalItemAmount", typeof(double));
			dtdetail.Columns.Add("TotalNetAmount", typeof(double));
			dtdetail.Columns.Add("AvgRate", typeof(double));
			dtdetail.Columns.Add("IncentiveRate", typeof(double));
			dtdetail.Columns.Add("IncentiveCalcOnId");
			dtdetail.Columns.Add("IncentiveCalcOn");
			dtdetail.Columns.Add("IncentiveAmount", typeof(double));
			dtdetail.Columns.Add("RemarksDetail");
			grdDetails.DataSource = dtdetail;
			grdDetails.RetrieveStructure();
			grdSettings();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			txtDocDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DebitAccountFill()
	{
		try
		{
			DataTable dtDebitAccount = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("11,13,20,21");
			if (dtDebitAccount.Rows.Count > 0)
			{
				DDL.BindDDL(dtDebitAccount, cmbDebitAccount, "Id", "AccountTitle", "Debit Account", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		try
		{
			int Code = WsRmCreditNoteHeader.GenerateCode(new WsRmCreditNoteHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 115
			});
			if (Code > 0)
			{
				txtDocNo.Text = Conversion.ToString((object)Code);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grdDetails.RootTable.Columns["TotalInvoices"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["TotalInvoices"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["TotalInvoices"].FormatString = "0,0";
			grdDetails.RootTable.Columns["TotalInvoices"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["TotalQty"].FormatString = "0,0";
			grdDetails.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns["TotalItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["TotalItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["TotalItemAmount"].FormatString = "0,0";
			grdDetails.RootTable.Columns["TotalItemAmount"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns["TotalNetAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["TotalNetAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["TotalNetAmount"].FormatString = "0,0";
			grdDetails.RootTable.Columns["TotalNetAmount"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns["TotalWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["TotalWeight"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["TotalWeight"].FormatString = "0,0";
			grdDetails.RootTable.Columns["TotalWeight"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["AvgRate"].FormatString = "0,0";
			grdDetails.RootTable.Columns["TargetBaseType"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["TargetFrom"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["TargetTo"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["IncentiveType"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["IncentiveRate"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["IncentiveCalcOn"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["IncentiveAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["SupplierCustomer"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["TargetRangeFrom"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["TargetRangeTo"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["TotalInvoices"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["TotalQty"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["TotalItemAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["TotalNetAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["AvgRate"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["TotalWeight"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["PolicyId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["TargetBaseTypeId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["IncentiveTypeId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["IncentiveCalcOnId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["SupplierCustomerId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["RemarksDetail"].EditType = (EditType)1;
			grdDetails.RootTable.Columns["PolicyId"].Visible = false;
			grdDetails.RootTable.Columns["ItemId"].Visible = false;
			grdDetails.RootTable.Columns["TargetBaseTypeId"].Visible = false;
			grdDetails.RootTable.Columns["IncentiveTypeId"].Visible = false;
			grdDetails.RootTable.Columns["IncentiveCalcOnId"].Visible = false;
			grdDetails.RootTable.Columns["SupplierCustomerId"].Visible = false;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetails_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdDetails.CurrentRow;
				dtdetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			grdDetails.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtDocNo.Text == string.Empty)
		{
			MessageBox.Show("DocNo Required");
			txtDocNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDebitAccount).ActiveRow == null)
		{
			MessageBox.Show("Debit Account Required");
			cmbDebitAccount.Focus();
			return false;
		}
		return true;
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		try
		{
			if (RecId == 0)
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
			if (!FormValidation())
			{
				return;
			}
			WsRmCreditNoteHeader obj = new WsRmCreditNoteHeader();
			if (RecId > 0)
			{
				obj.Id = RecId;
			}
			obj.DocNo = Conversion.ToInt((object)txtDocNo.Text);
			obj.DocDate = Conversion.ToDateTime((object)txtDocDate.Value);
			obj.PeriodFrom = Conversion.ToDateTime((object)txtFromDate.Value);
			obj.PeriodTo = Conversion.ToDateTime((object)txtToDate.Value);
			obj.DebitAccountId = Conversion.ToInt(cmbDebitAccount.Value);
			obj.RemarksHeader = Conversion.ToString((object)txtRemarks.Text);
			obj.DocumentTypeId = 115;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.ApprovedDate = DateTime.Now;
			obj.ApprovedUserId = UserAccount.ID;
			obj.IsApproved = false;
			obj.WsRmCreditNoteDetailsList = new List<WsRmCreditNoteDetail>();
			if (grdDetails.RowCount > 0)
			{
				GridEXRow[] rows = grdDetails.GetRows();
				foreach (GridEXRow r in rows)
				{
					WsRmCreditNoteDetail vd = new WsRmCreditNoteDetail();
					if (Conversion.ToInt(r.Cells["TargetBaseTypeId"].Value) == 0)
					{
						MessageBox.Show("Target Type Required");
						return;
					}
					vd.TargetTypeId = Conversion.ToInt(r.Cells["TargetBaseTypeId"].Value);
					Conversion.ToDateTime(r.Cells["TargetFrom"].Value);
					vd.TargetFrom = Conversion.ToDateTime(r.Cells["TargetFrom"].Value);
					Conversion.ToDateTime(r.Cells["TargetTo"].Value);
					vd.TargetTo = Conversion.ToDateTime(r.Cells["TargetTo"].Value);
					if (Conversion.ToInt(r.Cells["IncentiveTypeId"].Value) == 0)
					{
						MessageBox.Show("Incentive Type Required");
						return;
					}
					vd.IncentiveTypeId = Conversion.ToInt(r.Cells["IncentiveTypeId"].Value);
					if (Conversion.ToDouble(r.Cells["IncentiveRate"].Value) == 0.0)
					{
						MessageBox.Show("Incentive Rate Required");
						return;
					}
					vd.InCentiveRate = Conversion.ToDouble(r.Cells["IncentiveRate"].Value);
					if (Conversion.ToInt(r.Cells["IncentiveCalcOnId"].Value) == 0)
					{
						MessageBox.Show("Incentive Calculation On Required");
						return;
					}
					vd.CalcOn = Conversion.ToInt(r.Cells["IncentiveCalcOnId"].Value);
					if (Conversion.ToDouble(r.Cells["IncentiveAmount"].Value) == 0.0)
					{
						MessageBox.Show("Incentive Amount Required");
						return;
					}
					vd.IncentiveAmount = Conversion.ToDouble(r.Cells["IncentiveAmount"].Value);
					if (Conversion.ToInt(r.Cells["SupplierCustomerId"].Value) == 0)
					{
						MessageBox.Show("Customer Required");
						return;
					}
					vd.CreditAccountId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					if (Conversion.ToInt(r.Cells["PolicyId"].Value) == 0)
					{
						MessageBox.Show("Customer Required");
						return;
					}
					vd.PolicyId = Conversion.ToInt(r.Cells["PolicyId"].Value);
					vd.RemarksDetail = Conversion.ToString((object)r.Cells["RemarksDetail"].Text);
					obj.WsRmCreditNoteDetailsList.Add(vd);
				}
				WsRmCreditNoteHeader.Save(obj);
				if (RecId == 0)
				{
					MessageBox.Show("Receod Save Successfully");
				}
				else
				{
					MessageBox.Show("Receod Update Successfully");
				}
				Reset();
				return;
			}
			throw new Exception("Grid record not found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadById(int ID)
	{
		try
		{
			RecId = ID;
			WsRmCreditNoteHeader Obj = WsRmCreditNoteHeader.GetByID(ID);
			if (Obj != null)
			{
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				tabControl1.SelectedIndex = 0;
				txtDocDate.Value = Obj.DocDate;
				txtDocNo.Text = Conversion.ToString((object)Obj.DocNo);
				txtFromDate.Value = Obj.PeriodFrom;
				txtToDate.Value = Obj.PeriodTo;
				cmbDebitAccount.Value = Obj.DebitAccountId;
				txtRemarks.Text = Obj.RemarksHeader;
				dtdetail.Rows.Clear();
				for (int i = 0; i < Obj.WsRmCreditNoteDetailsList.Count; i++)
				{
					dtdetail.Rows.Add(Obj.WsRmCreditNoteDetailsList[i].PolicyId, Obj.WsRmCreditNoteDetailsList[i].CreditAccountId, Obj.WsRmCreditNoteDetailsList[i].CompanyName, Obj.WsRmCreditNoteDetailsList[i].TargetTypeId, Obj.WsRmCreditNoteDetailsList[i].TargetType, Obj.WsRmCreditNoteDetailsList[i].TargetFrom, Obj.WsRmCreditNoteDetailsList[i].TargetTo, Obj.WsRmCreditNoteDetailsList[i].TargetAcheived, Obj.WsRmCreditNoteDetailsList[i].IncentiveTypeId, Obj.WsRmCreditNoteDetailsList[i].IncentiveType, Obj.WsRmCreditNoteDetailsList[i].InCentiveRate, Obj.WsRmCreditNoteDetailsList[i].CalcOn, Obj.WsRmCreditNoteDetailsList[i].IncentiveCalculationOn, Obj.WsRmCreditNoteDetailsList[i].IncentiveAmount, Obj.WsRmCreditNoteDetailsList[i].RemarksDetail);
				}
				grdDetails.DataSource = dtdetail;
				grdDetails.RetrieveStructure();
				grdSettings();
			}
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
			txtDocNo.Text = string.Empty;
			txtDocDate.Value = DateTime.Now;
			txtFromDate.Value = DateTime.Now;
			txtToDate.Value = DateTime.Now;
			((Control)(object)cmbDebitAccount).Text = string.Empty;
			txtRemarks.Text = string.Empty;
			RecId = 0;
			dtdetail.Rows.Clear();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			txtDocDate.Focus();
			GenerateCode();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryFill()
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DocDate");
			dt.Columns.Add("DocNo");
			dt.Columns.Add("PeriodFrom");
			dt.Columns.Add("PeriodTo");
			dt.Columns.Add("DebitAccount");
			dt.Columns.Add("IncentiveAmount");
			dt.Columns.Add("Remarks");
			dt.Columns.Add("EntryUser");
			DataTable dtHistory = WsRmCreditNoteHeader.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 115,
				NoOfRecords = 21,
				CanViewAllRecord = false,
				EntryUser = UserAccount.ID
			});
			if (dtHistory.Rows.Count > 0)
			{
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["Id"], Conversion.ToDateTime(dtHistory.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtHistory.Rows[i]["DocNo"], Conversion.ToDateTime(dtHistory.Rows[i]["PeriodFrom"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dtHistory.Rows[i]["PeriodTo"]).ToString("dd-MMM-yyyy"), dtHistory.Rows[i]["AccountTitle"], dtHistory.Rows[i]["IncentiveAmount"], dtHistory.Rows[i]["RemarksHeader"], dtHistory.Rows[i]["UserName"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				DataGridHistorySetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns.Add("Detail");
			DataGridHistory.RootTable.Columns["Detail"].Caption = "Detail";
			DataGridHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			DataGridHistory.RootTable.Columns["Detail"].Width = 50;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			ctrlGrdBar1_Load(null, null);
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

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		btnsave_Click(null, null);
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			HistoryFill();
		}
	}

	private void FrmExportSalesContract_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				btnnew_Click(null, null);
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			DebitAccountFill();
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DataGridHistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DataGridHistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
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
			if (!(e.Column.Key == "Detail"))
			{
				return;
			}
			WsRmCreditNoteHeader Obj = WsRmCreditNoteHeader.GetByID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			if (Obj != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("SupplierCustomer");
				dt.Columns.Add("TargetBaseType");
				dt.Columns.Add("TargetFrom");
				dt.Columns.Add("TargetTo");
				dt.Columns.Add("TargetAchieved");
				dt.Columns.Add("IncentiveType");
				dt.Columns.Add("IncentiveRate", typeof(double));
				dt.Columns.Add("IncentiveCalcOn");
				dt.Columns.Add("IncentiveAmount", typeof(double));
				dt.Columns.Add("RemarksDetail");
				dt.Rows.Clear();
				for (int i = 0; i < Obj.WsRmCreditNoteDetailsList.Count; i++)
				{
					dt.Rows.Add(Obj.WsRmCreditNoteDetailsList[i].CompanyName, Obj.WsRmCreditNoteDetailsList[i].TargetType, Obj.WsRmCreditNoteDetailsList[i].TargetFrom, Obj.WsRmCreditNoteDetailsList[i].TargetTo, Obj.WsRmCreditNoteDetailsList[i].TargetAcheived, Obj.WsRmCreditNoteDetailsList[i].IncentiveType, Obj.WsRmCreditNoteDetailsList[i].InCentiveRate, Obj.WsRmCreditNoteDetailsList[i].IncentiveCalculationOn, Obj.WsRmCreditNoteDetailsList[i].IncentiveAmount, Obj.WsRmCreditNoteDetailsList[i].RemarksDetail);
				}
				grdHistoryDetail.DataSource = dt;
				grdHistoryDetail.RetrieveStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowDetail_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		try
		{
			if (RecId > 0)
			{
				return;
			}
			WsRmIncentivePolicyHeader val = new WsRmIncentivePolicyHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				SchemeDateFrom = Conversion.ToDateTime((object)txtFromDate.Value),
				SchemeDateTo = Conversion.ToDateTime((object)txtToDate.Value)
			};
			dtdetail.Rows.Clear();
			DataTable dt = WsRmIncentivePolicyHeader.CalculateIncentivesForCreditNotes(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtdetail.Rows.Add(dt.Rows[i]["PolicyId"], dt.Rows[i]["GlAccountId"], dt.Rows[i]["CustomerName"], dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["TargetBaseTypeId"], dt.Rows[i]["IncentiveBaseType"], Conversion.ToDateTime(dt.Rows[i]["SchemeDateFrom"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dt.Rows[i]["SchemeDateTo"]).ToString("dd-MMM-yyyy"), dt.Rows[i]["TargetRangeFrom"], dt.Rows[i]["TargetRangeTo"], dt.Rows[i]["IncentiveTypeId"], dt.Rows[i]["IncentiveTypeName"], dt.Rows[i]["TotalInvoices"], dt.Rows[i]["TotalQty"], dt.Rows[i]["TotalBillWeight"], dt.Rows[i]["TotalItemAmount"], dt.Rows[i]["TotalNetAmount"], dt.Rows[i]["AvgRate"], dt.Rows[i]["IncentiveRate"], dt.Rows[i]["IncentiveCalcTypeId"], dt.Rows[i]["IncentiveCalType"], dt.Rows[i]["TotalIncentive"], "");
				}
				grdDetails.DataSource = dtdetail;
				grdDetails.RetrieveStructure();
				grdSettings();
			}
			else
			{
				grdDetails.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DataGridHistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DataGridHistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected O, but got Unknown
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Expected O, but got Unknown
		//IL_1a80: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a8a: Expected O, but got Unknown
		GridEXLayout grdDetails_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.WholeSale.frmCreditNotes));
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdDetails = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.btnShowDetail = new System.Windows.Forms.Button();
		this.cmbDebitAccount = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdHistoryDetail = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label51 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetails).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDebitAccount).BeginInit();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel5.SuspendLayout();
		this.panel17.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1328, 664);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1328, 664);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1320, 638);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetails);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1314, 632);
		this.panel2.TabIndex = 0;
		grdDetails_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetails.DesignTimeLayout = grdDetails_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetails).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetails.DynamicFiltering = true;
		this.grdDetails.FilterMode = (FilterMode)2;
		this.grdDetails.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetails).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetails.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetails).Location = new System.Drawing.Point(0, 178);
		((System.Windows.Forms.Control)(object)this.grdDetails).Name = "grdDetails";
		((ScrollableBase)this.grdDetails).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetails).Size = new System.Drawing.Size(1314, 454);
		((System.Windows.Forms.Control)(object)this.grdDetails).TabIndex = 0;
		this.grdDetails.TotalRow = (InheritableBoolean)1;
		this.grdDetails.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetails).VisualStyle = (VisualStyle)4;
		this.grdDetails.ColumnButtonClick += new ColumnActionEventHandler(grdDetails_ColumnButtonClick);
		this.panel4.Controls.Add(this.btnShowDetail);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDebitAccount);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.txtToDate);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.txtFromDate);
		this.panel4.Controls.Add(this.txtDocNo);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.txtRemarks);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtDocDate);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1314, 148);
		this.panel4.TabIndex = 1;
		this.btnShowDetail.Location = new System.Drawing.Point(875, 106);
		this.btnShowDetail.Name = "btnShowDetail";
		this.btnShowDetail.Size = new System.Drawing.Size(75, 23);
		this.btnShowDetail.TabIndex = 6;
		this.btnShowDetail.Text = "Show Detail";
		this.btnShowDetail.UseVisualStyleBackColor = true;
		this.btnShowDetail.Click += new System.EventHandler(btnShowDetail_Click);
		this.cmbDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDebitAccount).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDebitAccount).Location = new System.Drawing.Point(638, 58);
		((System.Windows.Forms.Control)(object)this.cmbDebitAccount).Name = "cmbDebitAccount";
		((System.Windows.Forms.Control)(object)this.cmbDebitAccount).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.cmbDebitAccount).TabIndex = 4;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(635, 40);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(83, 15);
		this.label4.TabIndex = 516;
		this.label4.Text = "Debit Account";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(469, 40);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(47, 15);
		this.label12.TabIndex = 514;
		this.label12.Text = "To Date";
		this.txtToDate.CustomFormat = "dd-MMM-yyyy";
		this.txtToDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDate.Location = new System.Drawing.Point(469, 58);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(165, 23);
		this.txtToDate.TabIndex = 3;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(303, 40);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(63, 15);
		this.label11.TabIndex = 512;
		this.label11.Text = "From Date";
		this.txtFromDate.CustomFormat = "dd-MMM-yyyy";
		this.txtFromDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDate.Location = new System.Drawing.Point(303, 58);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(165, 23);
		this.txtFromDate.TabIndex = 2;
		this.txtDocNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(14, 58);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.ReadOnly = true;
		this.txtDocNo.Size = new System.Drawing.Size(121, 23);
		this.txtDocNo.TabIndex = 0;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(14, 40);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(52, 15);
		this.label8.TabIndex = 510;
		this.label8.Text = "Doc No#";
		this.txtRemarks.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(14, 106);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(859, 23);
		this.txtRemarks.TabIndex = 5;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(14, 86);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(54, 15);
		this.label5.TabIndex = 508;
		this.label5.Text = "Remarks";
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label18);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.ForeColor = System.Drawing.Color.White;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1314, 36);
		this.panel6.TabIndex = 504;
		this.label18.AutoSize = true;
		this.label18.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(9, 5);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(193, 29);
		this.label18.TabIndex = 25;
		this.label18.Text = "CREDIT NOTES";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(137, 40);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(56, 15);
		this.label3.TabIndex = 463;
		this.label3.Text = "Doc Date";
		this.txtDocDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(137, 58);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(165, 23);
		this.txtDocDate.TabIndex = 1;
		this.panel3.Controls.Add(this.ctrlGrdBar2);
		this.panel3.Controls.Add(this.toolstrip);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1314, 30);
		this.panel3.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1263, 6);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetails;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar2.TabIndex = 47;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnsave, this.btnUpdate, this.toolStripButton1 });
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Size = new System.Drawing.Size(1314, 27);
		this.toolstrip.TabIndex = 5;
		this.toolstrip.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(50, 24);
		this.toolStripButton1.Text = "Refresh";
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1320, 638);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel13.Controls.Add(this.panel7);
		this.panel13.Controls.Add(this.panel18);
		this.panel13.Controls.Add(this.panel17);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1314, 632);
		this.panel13.TabIndex = 0;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 403);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1314, 229);
		this.panel7.TabIndex = 2;
		this.grdHistoryDetail.AllowDelete = (InheritableBoolean)1;
		this.grdHistoryDetail.ColumnAutoResize = true;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1314, 229);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 4;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel18.Controls.Add(this.panel5);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 27);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1314, 376);
		this.panel18.TabIndex = 1;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		this.DataGridHistory.ColumnAutoResize = true;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 33);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1314, 343);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 3;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label51);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.ForeColor = System.Drawing.Color.White;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1314, 33);
		this.panel5.TabIndex = 505;
		this.label51.AutoSize = true;
		this.label51.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label51.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f);
		this.label51.ForeColor = System.Drawing.Color.White;
		this.label51.Location = new System.Drawing.Point(3, 3);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(306, 29);
		this.label51.TabIndex = 25;
		this.label51.Text = "CREDIT NOTES HISTORY";
		this.panel17.Controls.Add(this.ctrlGrdBar1);
		this.panel17.Controls.Add(this.toolStrip3);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1314, 27);
		this.panel17.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1269, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 46;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1314, 25);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1328, 664);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmCreditNotes";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(FrmExportSalesContract_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(FrmExportSalesContract_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetails).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDebitAccount).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
