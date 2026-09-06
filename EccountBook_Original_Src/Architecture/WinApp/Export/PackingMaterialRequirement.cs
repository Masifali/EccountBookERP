using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class PackingMaterialRequirement : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable table = new DataTable();

	private DataTable dtmain = new DataTable();

	public static int Id;

	private int RowIndexgrd;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabForm;

	private CtrlGrdBar ctrlGrdBar1;

	private TabPage tabHistory;

	private Panel panel4;

	internal GridEX grdhistory;

	private Panel panel1;

	internal GridEX grdPMRequirement;

	private ToolStrip btn;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripSeparator toolStripSeparator;

	internal GridEX grdSalesContractDetail;

	private Label label6;

	internal UltraCombo cmbItemUOM;

	internal UltraCombo CmbMainItem;

	private TextBox txtAvailableStock;

	private TextBox txtTotalOrderQty;

	private Label label3;

	internal UltraCombo cmbPMItemUom;

	private Button BtnAdd;

	internal UltraCombo cmbPMitem;

	private Label label12;

	private Label label11;

	private Label label8;

	private TextBox txtReqQty;

	private Label lblReqQty;

	private Label label10;

	private TextBox txtItemQty;

	private Label label14;

	internal UltraCombo ultraCombo2;

	internal UltraCombo cmbItem;

	internal GridEX grdMain;

	internal UltraCombo CmbSalesContract;

	private Label label2;

	internal UltraCombo CmbCustomerName;

	private Label label1;

	private Panel panel5;

	private Panel panel8;

	private Panel panel7;

	private Panel panel6;

	private Panel panel3;

	private Panel panel2;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	public PackingMaterialRequirement(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PackingMaterialRequirement_Load(object sender, EventArgs e)
	{
		try
		{
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			table.Columns.Add("ItemId");
			table.Columns.Add("ItemName");
			table.Columns.Add("PackUomId");
			table.Columns.Add("PackUom");
			table.Columns.Add("Qty", typeof(double));
			table.Columns.Add("PmItemId");
			table.Columns.Add("PackingItem");
			table.Columns.Add("PmUomId");
			table.Columns.Add("PmPackUom");
			table.Columns.Add("ReqQty", typeof(double));
			grdPMRequirement.DataSource = table;
			grdPMRequirement.RetrieveStructure();
			GridSettingPackingMaterial();
			dtmain = table.Clone();
			CmbCustomerName.Focus();
			ItemBind();
			CustomerBind();
			BtnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemBind()
	{
		try
		{
			DataTable dtItem = CommonServices.GetItemByItemTypeId("14");
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbPMitem, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbPMitem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerBind()
	{
		try
		{
			DataTable dt = CommonServices.ReadByOrganizationCompanyIdForExportServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCustomerName, "Id", "CompanyName", "Customer Name", false);
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SalesContractBind()
	{
		try
		{
			DataTable dt = CommonServices.GetLcOrderNoBySupplierCustomerId(Conversion.ToInt(CmbCustomerName.Value));
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbSalesContract, "Id", "LcOrderNo", "Sales Contract", false);
				((UltraGridBase)CmbSalesContract).DisplayLayout.Bands[0].Columns[1].Width = 150;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SalesContractDetailByContractId()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImLcOrder.GetItemsbySalesContract(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(CmbSalesContract.Value)
			});
			if (dt.Rows.Count > 0)
			{
				grdSalesContractDetail.DataSource = dt;
				grdSalesContractDetail.RetrieveStructure();
				GridSettingSalesContractDetail();
			}
			else
			{
				grdSalesContractDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingSalesContractDetail()
	{
		try
		{
			grdSalesContractDetail.AllowDelete = (InheritableBoolean)2;
			grdSalesContractDetail.AllowEdit = (InheritableBoolean)2;
			grdSalesContractDetail.RootTable.Columns["ItemId"].Visible = false;
			grdSalesContractDetail.RootTable.Columns["ItemUomId"].Visible = false;
			grdSalesContractDetail.RootTable.Columns["ItemName"].Width = 300;
			grdSalesContractDetail.RootTable.Columns.Add("Load");
			grdSalesContractDetail.RootTable.Columns["Load"].Caption = "Load";
			grdSalesContractDetail.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdSalesContractDetail.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
			grdSalesContractDetail.RootTable.Columns["Load"].ButtonText = "Load";
			grdSalesContractDetail.RootTable.Columns["Load"].Width = 40;
			grdSalesContractDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdSalesContractDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdSalesContractDetail.RootTable.Columns["Qty"].FormatString = "#,#";
			grdSalesContractDetail.RootTable.Columns["Qty"].TotalFormatString = "#,#";
			grdSalesContractDetail.ColumnAutoResize = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingPackingMaterial()
	{
		try
		{
			grdPMRequirement.AllowDelete = (InheritableBoolean)2;
			grdPMRequirement.AllowEdit = (InheritableBoolean)2;
			grdPMRequirement.RootTable.Columns["ItemId"].Visible = false;
			grdPMRequirement.RootTable.Columns["PmItemId"].Visible = false;
			grdPMRequirement.RootTable.Columns["PackUomId"].Visible = false;
			grdPMRequirement.RootTable.Columns["PmUomId"].Visible = false;
			grdPMRequirement.RootTable.Columns["ItemName"].Width = 300;
			grdPMRequirement.RootTable.Columns["PackingItem"].Width = 250;
			grdPMRequirement.RootTable.Columns["ReqQty"].AggregateFunction = (AggregateFunction)2;
			grdPMRequirement.RootTable.Columns["ReqQty"].TextAlignment = (TextAlignment)3;
			grdPMRequirement.RootTable.Columns["ReqQty"].FormatString = "#,#";
			grdPMRequirement.RootTable.Columns["ReqQty"].TotalFormatString = "#,#";
			grdPMRequirement.ColumnAutoResize = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingMain()
	{
		try
		{
			grdMain.AllowDelete = (InheritableBoolean)2;
			grdMain.AllowEdit = (InheritableBoolean)2;
			grdMain.RootTable.Columns["ItemId"].Visible = false;
			grdMain.RootTable.Columns["PmItemId"].Visible = false;
			grdMain.RootTable.Columns["PackUomId"].Visible = false;
			grdMain.RootTable.Columns["PmUomId"].Visible = false;
			grdMain.RootTable.Columns["ItemName"].Width = 300;
			grdMain.RootTable.Columns["PackingItem"].Width = 250;
			grdMain.RootTable.Columns["ReqQty"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["ReqQty"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["ReqQty"].FormatString = "#,#";
			grdMain.RootTable.Columns["ReqQty"].TotalFormatString = "#,#";
			grdMain.ColumnAutoResize = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom(int ID)
	{
		try
		{
			DataTable dtuomsch = CommonServices.GetUomScheduleByItemId(ID);
			if (dtuomsch.Rows.Count > 0)
			{
				DDL.BindDDL(dtuomsch, cmbPMItemUom, "Id", "UOMCode", "PackUom", ZeroIndex: false);
				((UltraGridBase)cmbPMItemUom).DisplayLayout.Bands[0].Columns[2].Hidden = true;
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
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			table.Rows.Clear();
			dtmain.Rows.Clear();
			((Control)(object)CmbSalesContract).Enabled = true;
			((Control)(object)CmbCustomerName).Text = string.Empty;
			((Control)(object)CmbSalesContract).Text = string.Empty;
			grdSalesContractDetail.ClearStructure();
			grdPMRequirement.ClearStructure();
			grdMain.ClearStructure();
			BtnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
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
			((Control)(object)cmbPMitem).Text = string.Empty;
			((Control)(object)cmbPMItemUom).Text = string.Empty;
			txtReqQty.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
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

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)cmbPMitem).ActiveRow == null)
		{
			MessageBox.Show("Packing Item Field is Required");
			cmbPMitem.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPMItemUom).ActiveRow == null)
		{
			MessageBox.Show("Packing Item Uom Field is Required");
			cmbPMItemUom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtReqQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Required Qty Field is Required");
			txtReqQty.Focus();
			return false;
		}
		return true;
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			if (grdPMRequirement.GetRows().Length != 0)
			{
				double ReqQty = Conversion.ToDouble(grdPMRequirement.GetTotal(grdPMRequirement.RootTable.Columns["ReqQty"], (AggregateFunction)2));
				double ItemQty = 0.0;
				bool flag = false;
				ReqQty += Conversion.ToDouble((object)txtReqQty.Text.Trim());
				GridEXRow[] rows = grdPMRequirement.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (flag)
					{
						break;
					}
					if (Conversion.ToInt(item.Cells["ItemId"].Value) == Conversion.ToInt(CmbMainItem.Value))
					{
						ItemQty = Conversion.ToDouble(item.Cells["Qty"].Value);
						flag = true;
					}
				}
				if (ReqQty > ItemQty)
				{
					throw new Exception("Required Qty cannot be greater than ItemQty");
				}
			}
			table.Rows.Add(CmbMainItem.Value, ((Control)(object)CmbMainItem).Text.Trim(), cmbItemUOM.Value, ((Control)(object)cmbItemUOM).Text.Trim(), txtItemQty.Text.Trim(), cmbPMitem.Value, ((Control)(object)cmbPMitem).Text.Trim(), cmbPMItemUom.Value, ((Control)(object)cmbPMItemUom).Text.Trim(), txtReqQty.Text.Trim());
			grdPMRequirement.DataSource = table;
			grdPMRequirement.RetrieveStructure();
			GridSettingPackingMaterial();
			int Count = table.Rows.Count - 1;
			DataRow dr = table.Rows[Count];
			dtmain.Rows.Add(dr.ItemArray);
			grdMain.DataSource = dtmain;
			grdMain.RetrieveStructure();
			GridSettingMain();
			ResetDetail();
			cmbPMitem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void Insert()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				ExImLcOrderMrpHeader obj = new ExImLcOrderMrpHeader();
				obj.ExImLcOrderId = Conversion.ToInt(CmbSalesContract.Value);
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.EntryUser = UserAccount.ID;
				obj.ModifyUser = UserAccount.ID;
				obj.ExImLcOrderMrpDetailsList = new List<ExImLcOrderMrpDetail>();
				GridEXRow[] rows = grdMain.GetRows();
				foreach (GridEXRow item in rows)
				{
					ExImLcOrderMrpDetail detail = new ExImLcOrderMrpDetail();
					detail.SortNo = item.RowIndex + 1;
					detail.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
					detail.ItemUomId = Conversion.ToInt(item.Cells["PackUomId"].Value);
					detail.ItemIdPm = Conversion.ToInt(item.Cells["PmItemId"].Value);
					detail.ItemUomIdPm = Conversion.ToInt(item.Cells["PmUomId"].Value);
					detail.ExImLcOrderId = Conversion.ToInt(CmbSalesContract.Value);
					detail.ReqQty = Conversion.ToDecimal(item.Cells["ReqQty"].Value);
					obj.ExImLcOrderMrpDetailsList.Add(detail);
				}
				if (ExImLcOrderMrpHeader.Save(obj) > 0)
				{
					MessageBox.Show("Record Saved Successfully");
				}
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			Id = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int ID)
	{
		try
		{
			ExImLcOrderMrpHeader obj = ExImLcOrderMrpHeader.GetByID(ID);
			if (obj != null)
			{
				btnSave.Visible = false;
				btnUpdate.Visible = true;
				tabControl1.SelectedIndex = 0;
				CmbSalesContract.Value = obj.ExImLcOrderId;
				SalesContractDetailByContractId();
				dtmain.Rows.Clear();
				for (int i = 0; i < obj.ExImLcOrderMrpDetailsList.Count; i++)
				{
					dtmain.Rows.Add(obj.ExImLcOrderMrpDetailsList[i].ItemId, obj.ExImLcOrderMrpDetailsList[i].ItemName, obj.ExImLcOrderMrpDetailsList[i].ItemUomId, obj.ExImLcOrderMrpDetailsList[i].PackUom, obj.ExImLcOrderMrpDetailsList[i].ReqQty, obj.ExImLcOrderMrpDetailsList[i].ItemIdPm, obj.ExImLcOrderMrpDetailsList[i].ItemNamePm, obj.ExImLcOrderMrpDetailsList[i].ItemUomIdPm, obj.ExImLcOrderMrpDetailsList[i].PackUomPm, obj.ExImLcOrderMrpDetailsList[i].ReqQty);
				}
				grdMain.DataSource = dtmain;
				grdMain.RetrieveStructure();
				GridSettingMain();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbitem_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom(Conversion.ToInt(cmbPMitem.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCustomerName_Leave(object sender, EventArgs e)
	{
		try
		{
			SalesContractBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSalesContract_Leave(object sender, EventArgs e)
	{
		try
		{
			SalesContractDetailByContractId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSalesContractDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (!(e.Column.Key == "Load"))
			{
				return;
			}
			((Control)(object)CmbSalesContract).Enabled = false;
			((Control)(object)CmbMainItem).Text = string.Empty;
			((Control)(object)cmbItemUOM).Text = string.Empty;
			txtItemQty.Text = "0";
			((UltraGridBase)CmbMainItem).DataSource = null;
			((UltraGridBase)cmbItemUOM).DataSource = null;
			cmbPMitem.Focus();
			GridEXRow item = grdSalesContractDetail.CurrentRow;
			if (item == null)
			{
				return;
			}
			DataTable dt = CommonServices.MakeDataTable(Conversion.ToString(item.Cells["ItemId"].Value), Conversion.ToString(item.Cells["ItemName"].Value));
			DDL.BindDDL(dt, CmbMainItem, "PropertyName", "PropertyValue", "ItemName", ZeroIndex: false);
			((UltraGridBase)CmbMainItem).Rows[0].Activate();
			DataTable dtUom = CommonServices.MakeDataTable(Conversion.ToString(item.Cells["ItemUomId"].Value), Conversion.ToString(item.Cells["PackSize"].Value));
			DDL.BindDDL(dtUom, cmbItemUOM, "PropertyName", "PropertyValue", "PackSize", ZeroIndex: false);
			((UltraGridBase)cmbItemUOM).Rows[0].Activate();
			txtItemQty.Text = Conversion.ToString(item.Cells["Qty"].Value);
			bool flag = false;
			for (int j = table.Rows.Count - 1; j >= 0; j--)
			{
				DataRow dr2 = table.Rows[j];
				if (Conversion.ToInt(dr2["ItemId"]) != Conversion.ToInt(item.Cells["ItemId"].Value))
				{
					flag = true;
					dr2.Delete();
				}
			}
			table.AcceptChanges();
			if (!flag)
			{
				table.Rows.Clear();
			}
			for (int i = 0; i < dtmain.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtmain.Rows[i]["ItemId"]) == Conversion.ToInt(item.Cells["ItemId"].Value))
				{
					DataRow dr = dtmain.Rows[i];
					table.Rows.Add(dr.ItemArray);
				}
			}
			grdPMRequirement.DataSource = table;
			grdPMRequirement.RetrieveStructure();
			GridSettingPackingMaterial();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtReqQty_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				HistoryBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
			ReadById(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImLcOrderMrpHeader.GetAll(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				grdhistory.DataSource = dt;
				grdhistory.RetrieveStructure();
				grdhistory.RootTable.Columns["Id"].Visible = false;
				grdhistory.AllowDelete = (InheritableBoolean)2;
				grdhistory.AllowEdit = (InheritableBoolean)2;
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

	private void grdPMRequirement_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdPMRequirement.CurrentRow;
			if (item != null)
			{
				RowIndexgrd = item.RowIndex;
				DataTable dt = CommonServices.MakeDataTable(Conversion.ToString(item.Cells["ItemId"].Value), Conversion.ToString(item.Cells["ItemName"].Value));
				DDL.BindDDL(dt, CmbMainItem, "PropertyName", "PropertyValue", "ItemName", ZeroIndex: false);
				((UltraGridBase)CmbMainItem).Rows[0].Activate();
				DataTable dtUom = CommonServices.MakeDataTable(Conversion.ToString(item.Cells["PackUomId"].Value), Conversion.ToString(item.Cells["PackUom"].Value));
				DDL.BindDDL(dtUom, cmbItemUOM, "PropertyName", "PropertyValue", "PackUom", ZeroIndex: false);
				((UltraGridBase)cmbItemUOM).Rows[0].Activate();
				txtItemQty.Text = Conversion.ToString(item.Cells["Qty"].Value);
				cmbPMitem.Value = Conversion.ToInt(item.Cells["PmItemId"].Value);
				((Control)(object)cmbPMitem).Text = Conversion.ToString(item.Cells["PackingItem"].Value);
				cmbPMItemUom.Value = Conversion.ToInt(item.Cells["PmUomId"].Value);
				((Control)(object)cmbPMItemUom).Text = Conversion.ToString(item.Cells["PmPackUom"].Value);
				txtReqQty.Text = Conversion.ToString(item.Cells["ReqQty"].Value);
				BtnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				cmbPMitem.Focus();
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
			table.Rows[RowIndexgrd]["ItemId"] = CmbMainItem.Value;
			table.Rows[RowIndexgrd]["ItemName"] = ((Control)(object)CmbMainItem).Text;
			table.Rows[RowIndexgrd]["PackUomId"] = cmbItemUOM.Value;
			table.Rows[RowIndexgrd]["PackUom"] = ((Control)(object)cmbItemUOM).Text;
			table.Rows[RowIndexgrd]["Qty"] = txtItemQty.Text;
			table.Rows[RowIndexgrd]["PmItemId"] = cmbPMitem.Value;
			table.Rows[RowIndexgrd]["PackingItem"] = ((Control)(object)cmbPMitem).Text;
			table.Rows[RowIndexgrd]["PmUomId"] = cmbPMItemUom.Value;
			table.Rows[RowIndexgrd]["PmPackUom"] = ((Control)(object)cmbPMItemUom).Text;
			table.Rows[RowIndexgrd]["ReqQty"] = txtReqQty.Text;
			for (int i = 0; i < dtmain.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtmain.Rows[i]["ItemId"]) == Conversion.ToInt(table.Rows[RowIndexgrd]["ItemId"]) && Conversion.ToInt(dtmain.Rows[i]["PackUomId"]) == Conversion.ToInt(table.Rows[RowIndexgrd]["PackUomId"]) && Conversion.ToInt(dtmain.Rows[i]["PmItemId"]) == Conversion.ToInt(table.Rows[RowIndexgrd]["PmItemId"]) && Conversion.ToInt(dtmain.Rows[i]["PmUomId"]) == Conversion.ToInt(table.Rows[RowIndexgrd]["PmUomId"]))
				{
					DataRow dr = table.Rows[RowIndexgrd];
					dtmain.Rows[i].ItemArray = dr.ItemArray;
				}
			}
			BtnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			ResetDetail();
			BtnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
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
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Expected O, but got Unknown
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Expected O, but got Unknown
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Expected O, but got Unknown
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Expected O, but got Unknown
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Expected O, but got Unknown
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Expected O, but got Unknown
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Expected O, but got Unknown
		//IL_271e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2728: Expected O, but got Unknown
		GridEXLayout grdMain_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPMRequirement_DesignTimeLayout = new GridEXLayout();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
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
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		GridEXLayout grdSalesContractDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.PackingMaterialRequirement));
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdMain = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdPMRequirement = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtAvailableStock = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtTotalOrderQty = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbItemUOM = new UltraCombo();
		this.cmbPMItemUom = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.BtnAdd = new System.Windows.Forms.Button();
		this.CmbMainItem = new UltraCombo();
		this.cmbPMitem = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.txtItemQty = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.lblReqQty = new System.Windows.Forms.Label();
		this.txtReqQty = new System.Windows.Forms.TextBox();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdSalesContractDetail = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.CmbCustomerName = new UltraCombo();
		this.CmbSalesContract = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btn = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.ultraCombo2 = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMain).BeginInit();
		this.panel5.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPMRequirement).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPMItemUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMainItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPMitem).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSalesContractDetail).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalesContract).BeginInit();
		this.btn.SuspendLayout();
		this.tabHistory.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1140, 699);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 1;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add(this.panel1);
		this.tabForm.Controls.Add(this.btn);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1132, 673);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 30);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1126, 640);
		this.panel1.TabIndex = 38;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdMain);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(799, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(327, 640);
		this.panel2.TabIndex = 57;
		this.grdMain.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdMain).BackColor = System.Drawing.Color.White;
		grdMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdMain.DesignTimeLayout = grdMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdMain).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMain.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdMain).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdMain).Name = "grdMain";
		((ScrollableBase)this.grdMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMain).Size = new System.Drawing.Size(327, 640);
		((System.Windows.Forms.Control)(object)this.grdMain).TabIndex = 53;
		this.grdMain.TotalRow = (InheritableBoolean)1;
		this.grdMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMain).VisualStyle = (VisualStyle)4;
		this.panel5.Controls.Add(this.panel3);
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(799, 640);
		this.panel5.TabIndex = 56;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdPMRequirement);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 331);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(799, 309);
		this.panel3.TabIndex = 3;
		this.grdPMRequirement.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdPMRequirement).BackColor = System.Drawing.Color.White;
		grdPMRequirement_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPMRequirement.DesignTimeLayout = grdPMRequirement_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPMRequirement).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPMRequirement).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPMRequirement.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPMRequirement).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPMRequirement).Name = "grdPMRequirement";
		((ScrollableBase)this.grdPMRequirement).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPMRequirement).Size = new System.Drawing.Size(799, 309);
		((System.Windows.Forms.Control)(object)this.grdPMRequirement).TabIndex = 3;
		this.grdPMRequirement.TotalRow = (InheritableBoolean)1;
		this.grdPMRequirement.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPMRequirement).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdPMRequirement).DoubleClick += new System.EventHandler(grdPMRequirement_DoubleClick);
		this.panel8.Controls.Add(this.btnCancelDetail);
		this.panel8.Controls.Add(this.btnUpdateDetail);
		this.panel8.Controls.Add(this.txtAvailableStock);
		this.panel8.Controls.Add(this.label6);
		this.panel8.Controls.Add(this.txtTotalOrderQty);
		this.panel8.Controls.Add(this.label10);
		this.panel8.Controls.Add(this.label3);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemUOM);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPMItemUom);
		this.panel8.Controls.Add(this.label14);
		this.panel8.Controls.Add(this.BtnAdd);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMainItem);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPMitem);
		this.panel8.Controls.Add(this.label12);
		this.panel8.Controls.Add(this.txtItemQty);
		this.panel8.Controls.Add(this.label11);
		this.panel8.Controls.Add(this.label8);
		this.panel8.Controls.Add(this.lblReqQty);
		this.panel8.Controls.Add(this.txtReqQty);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 226);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(799, 105);
		this.panel8.TabIndex = 2;
		this.btnCancelDetail.BackColor = System.Drawing.Color.White;
		this.btnCancelDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.Location = new System.Drawing.Point(742, 75);
		this.btnCancelDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(51, 26);
		this.btnCancelDetail.TabIndex = 52;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(690, 75);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(51, 26);
		this.btnUpdateDetail.TabIndex = 51;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtAvailableStock.AccessibleName = "";
		this.txtAvailableStock.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvailableStock.Location = new System.Drawing.Point(539, 77);
		this.txtAvailableStock.Name = "txtAvailableStock";
		this.txtAvailableStock.ReadOnly = true;
		this.txtAvailableStock.Size = new System.Drawing.Size(116, 23);
		this.txtAvailableStock.TabIndex = 4;
		this.txtAvailableStock.TabStop = false;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(292, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(61, 15);
		this.label6.TabIndex = 1;
		this.label6.Text = "Pack Uom";
		this.txtTotalOrderQty.AccessibleName = "";
		this.txtTotalOrderQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalOrderQty.Location = new System.Drawing.Point(433, 77);
		this.txtTotalOrderQty.Name = "txtTotalOrderQty";
		this.txtTotalOrderQty.ReadOnly = true;
		this.txtTotalOrderQty.Size = new System.Drawing.Size(103, 23);
		this.txtTotalOrderQty.TabIndex = 3;
		this.txtTotalOrderQty.TabStop = false;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(5, 3);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(65, 15);
		this.label10.TabIndex = 0;
		this.label10.Text = "Item Name";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(262, 55);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(35, 15);
		this.label3.TabIndex = 1;
		this.label3.Text = "UOM";
		this.cmbItemUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbItemUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Location = new System.Drawing.Point(296, 23);
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Name = "cmbItemUOM";
		this.cmbItemUOM.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Size = new System.Drawing.Size(76, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).TabStop = false;
		this.cmbPMItemUom.AllowNull = (DefaultableBoolean)1;
		this.cmbPMItemUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPMItemUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPMItemUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPMItemUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.cmbPMItemUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPMItemUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPMItemUom).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPMItemUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPMItemUom).Location = new System.Drawing.Point(265, 75);
		((System.Windows.Forms.Control)(object)this.cmbPMItemUom).Name = "cmbPMItemUom";
		((System.Windows.Forms.Control)(object)this.cmbPMItemUom).Size = new System.Drawing.Size(83, 26);
		((System.Windows.Forms.Control)(object)this.cmbPMItemUom).TabIndex = 1;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(375, 5);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(53, 15);
		this.label14.TabIndex = 45;
		this.label14.Text = "Item Qty";
		this.BtnAdd.BackColor = System.Drawing.Color.White;
		this.BtnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAdd.Location = new System.Drawing.Point(657, 75);
		this.BtnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.BtnAdd.Name = "BtnAdd";
		this.BtnAdd.Size = new System.Drawing.Size(29, 26);
		this.BtnAdd.TabIndex = 5;
		this.BtnAdd.Text = "+";
		this.BtnAdd.UseVisualStyleBackColor = false;
		this.BtnAdd.Click += new System.EventHandler(Add_Click);
		this.CmbMainItem.AllowNull = (DefaultableBoolean)1;
		this.CmbMainItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMainItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMainItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMainItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbMainItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance16).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMainItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMainItem).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMainItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMainItem).Location = new System.Drawing.Point(8, 23);
		((System.Windows.Forms.Control)(object)this.CmbMainItem).Name = "CmbMainItem";
		this.CmbMainItem.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbMainItem).Size = new System.Drawing.Size(285, 26);
		((System.Windows.Forms.Control)(object)this.CmbMainItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbMainItem).TabStop = false;
		this.cmbPMitem.AllowNull = (DefaultableBoolean)1;
		this.cmbPMitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPMitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPMitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPMitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.cmbPMitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPMitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPMitem).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPMitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPMitem).Location = new System.Drawing.Point(9, 75);
		((System.Windows.Forms.Control)(object)this.cmbPMitem).Name = "cmbPMitem";
		((System.Windows.Forms.Control)(object)this.cmbPMitem).Size = new System.Drawing.Size(253, 26);
		((System.Windows.Forms.Control)(object)this.cmbPMitem).TabIndex = 0;
		this.cmbPMitem.ValueChanged += new System.EventHandler(cmbitem_ValueChanged);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(536, 55);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(91, 15);
		this.label12.TabIndex = 50;
		this.label12.Text = "Available Stock";
		this.txtItemQty.AccessibleName = "txtVoucherCode";
		this.txtItemQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQty.Location = new System.Drawing.Point(376, 25);
		this.txtItemQty.Name = "txtItemQty";
		this.txtItemQty.ReadOnly = true;
		this.txtItemQty.Size = new System.Drawing.Size(78, 23);
		this.txtItemQty.TabIndex = 2;
		this.txtItemQty.TabStop = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(430, 55);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(91, 15);
		this.label11.TabIndex = 49;
		this.label11.Text = "Total Order Qty";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(6, 55);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(80, 15);
		this.label8.TabIndex = 0;
		this.label8.Text = "Packing Item ";
		this.lblReqQty.AutoSize = true;
		this.lblReqQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblReqQty.Location = new System.Drawing.Point(348, 55);
		this.lblReqQty.Name = "lblReqQty";
		this.lblReqQty.Size = new System.Drawing.Size(49, 15);
		this.lblReqQty.TabIndex = 47;
		this.lblReqQty.Text = "Req Qty";
		this.txtReqQty.AccessibleName = "";
		this.txtReqQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReqQty.Location = new System.Drawing.Point(351, 77);
		this.txtReqQty.Name = "txtReqQty";
		this.txtReqQty.Size = new System.Drawing.Size(79, 23);
		this.txtReqQty.TabIndex = 2;
		this.txtReqQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtReqQty_KeyPress);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdSalesContractDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 37);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(799, 189);
		this.panel7.TabIndex = 1;
		this.grdSalesContractDetail.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdSalesContractDetail).BackColor = System.Drawing.Color.White;
		grdSalesContractDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSalesContractDetail.DesignTimeLayout = grdSalesContractDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSalesContractDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdSalesContractDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSalesContractDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSalesContractDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSalesContractDetail).Name = "grdSalesContractDetail";
		((ScrollableBase)this.grdSalesContractDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSalesContractDetail).Size = new System.Drawing.Size(799, 189);
		((System.Windows.Forms.Control)(object)this.grdSalesContractDetail).TabIndex = 2;
		this.grdSalesContractDetail.TotalRow = (InheritableBoolean)1;
		this.grdSalesContractDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSalesContractDetail).VisualStyle = (VisualStyle)4;
		this.grdSalesContractDetail.ColumnButtonClick += new ColumnActionEventHandler(grdSalesContractDetail_ColumnButtonClick);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerName);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSalesContract);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Controls.Add(this.label1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(799, 37);
		this.panel6.TabIndex = 0;
		this.CmbCustomerName.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Location = new System.Drawing.Point(112, 6);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Name = "CmbCustomerName";
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Leave += new System.EventHandler(CmbCustomerName_Leave);
		this.CmbSalesContract.AllowNull = (DefaultableBoolean)1;
		this.CmbSalesContract.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSalesContract.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSalesContract.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Appearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalesContract).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalesContract).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSalesContract).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSalesContract).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSalesContract.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSalesContract).Location = new System.Drawing.Point(436, 6);
		((System.Windows.Forms.Control)(object)this.CmbSalesContract).Name = "CmbSalesContract";
		((System.Windows.Forms.Control)(object)this.CmbSalesContract).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.CmbSalesContract).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbSalesContract).Leave += new System.EventHandler(CmbSalesContract_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(337, 11);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(86, 15);
		this.label2.TabIndex = 55;
		this.label2.Text = "Sales Contract";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(10, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(93, 15);
		this.label1.TabIndex = 53;
		this.label1.Text = "Customer Name";
		this.btn.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btn.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnUpdate, this.btnSave, this.toolStripSeparator });
		this.btn.Location = new System.Drawing.Point(3, 3);
		this.btn.Name = "btn";
		this.btn.Size = new System.Drawing.Size(1126, 27);
		this.btn.TabIndex = 34;
		this.btn.Text = "toolStrip1";
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(55, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.tabHistory.Controls.Add(this.panel4);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1132, 673);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1126, 667);
		this.panel4.TabIndex = 1;
		this.grdhistory.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1126, 667);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 2;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		this.ultraCombo2.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo2.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ultraCombo2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Location = new System.Drawing.Point(240, 42);
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Name = "ultraCombo2";
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Size = new System.Drawing.Size(61, 26);
		((System.Windows.Forms.Control)(object)this.ultraCombo2).TabIndex = 51;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(15, 42);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 51;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1140, 699);
		base.Controls.Add(this.tabControl1);
		base.Name = "PackingMaterialRequirement";
		this.Text = "Packing Material Requirement";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(PackingMaterialRequirement_Load);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.tabForm.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdMain).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPMRequirement).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPMItemUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMainItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPMitem).EndInit();
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSalesContractDetail).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalesContract).EndInit();
		this.btn.ResumeLayout(false);
		this.btn.PerformLayout();
		this.tabHistory.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		base.ResumeLayout(false);
	}
}
