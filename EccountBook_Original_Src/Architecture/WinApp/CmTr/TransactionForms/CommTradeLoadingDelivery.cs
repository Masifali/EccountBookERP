using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.CmTr.TransactionForm;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.CmTr.TransactionForm;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.CmTr.TransactionForms;

public class CommTradeLoadingDelivery : Form
{
	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtDetail = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtMain = new DataTable();

	private DataTable dtgrid = new DataTable();

	private int RecId;

	private int updateDetailIndex;

	private Rightsobjects formright = new Rightsobjects();

	private bool ItemSearchWithNameOrCode;

	private DataTable lst = new DataTable();

	private bool UpdateMode;

	private List<CommTradeLoadingDeliveryDetail> lstRemoveRecord = new List<CommTradeLoadingDeliveryDetail>();

	private IContainer components;

	private Panel panel1;

	private GroupBox groupBox1;

	private Label label6;

	private TextBox txtmanualno;

	private Label label5;

	private DateTimePicker txtdocdate;

	private TextBox txtdocno;

	private RichTextBox txtRemarksHeader;

	private Label label33;

	private Label lbl;

	private TextBox txtBiltyNo;

	private Label label48;

	private TextBox txtvehicleno;

	private DateTimePicker txtbiltydate;

	private Label label2;

	internal UltraCombo cmbTransporter;

	private Label label29;

	private Label label3;

	private TextBox txtfrieght;

	private Label label4;

	private TextBox txtvehicleweight;

	private Label label1;

	private TextBox txtbiltyexpense;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private GroupBox groupBox3;

	internal UltraCombo cmbitem;

	private Label labItemName;

	internal UltraCombo cmbuom;

	private Label label11;

	internal UltraCombo cmbsupplier;

	private Label label12;

	private Label label10;

	private TextBox txtItemQty;

	private Label label13;

	private TextBox txtsupgrosswt;

	private Label label14;

	private TextBox txtSupPckMtrlWeight;

	private Label label18;

	private TextBox txtsupremarks;

	private Label label16;

	private TextBox txtSupNetWeight;

	private Label label15;

	private TextBox txtSupAdLsWt;

	internal UltraCombo cmbBroker;

	private Label label27;

	private GroupBox groupBox4;

	private Label label34;

	private TextBox txtcustremarks;

	private Label label35;

	private TextBox txtcustnetwt;

	private TextBox txtcustadlswt;

	private Label label37;

	private TextBox txtcustpckmtrlwt;

	private Label label38;

	private TextBox txtcustgrosswt;

	internal UltraCombo cmbcustomer;

	private Label label39;

	private Panel panel2;

	private Panel panel4;

	private Label label46;

	private GroupBox groupBox2;

	private Label label8;

	private Label label7;

	internal UltraCombo CmbPackingType;

	internal UltraCombo cmbcropYear;

	private Label label9;

	private Button btnAddDetail;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private GroupBox groupBox5;

	private Label label17;

	private Label label23;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnPrint;

	private ToolStripButton btnDelete;

	private ToolStripButton btnshortcutkeys;

	internal GridEX grd;

	private Panel panel5;

	private ToolStrip miniToolStrip;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistoryDetail;

	private Panel panel3;

	private Label label19;

	internal GridEX grdHistory;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoaddAllHistory;

	private Label label20;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	public CommTradeLoadingDelivery(UserAccount userAccount)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void CommTradeLoadingDelivery_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1025, 700);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnPrint.Enabled = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ItemSearchWithNameOrCode = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ItemSearchByCode"));
			if (ItemSearchWithNameOrCode)
			{
				rdSearchByCode.Checked = true;
			}
			else
			{
				rdSearchByName.Checked = true;
			}
			lst.Columns.Add("Id", typeof(int));
			lst.Columns.Add("ItemName", typeof(string));
			lst.Columns.Add("ItemCode", typeof(string));
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("OrderId", typeof(int));
			dtDetail.Columns.Add("OrderDetailId", typeof(int));
			dtDetail.Columns.Add("ItemId", typeof(int));
			dtDetail.Columns.Add("ItemCode", typeof(string));
			dtDetail.Columns.Add("ItemName", typeof(string));
			dtDetail.Columns.Add("CropYearId", typeof(int));
			dtDetail.Columns.Add("CropYear", typeof(string));
			dtDetail.Columns.Add("PackingTypeId", typeof(int));
			dtDetail.Columns.Add("PackingType", typeof(string));
			dtDetail.Columns.Add("UomId", typeof(int));
			dtDetail.Columns.Add("UOM", typeof(string));
			dtDetail.Columns.Add("Qty", typeof(int));
			dtDetail.Columns.Add("SupplierId", typeof(int));
			dtDetail.Columns.Add("SupplierName", typeof(string));
			dtDetail.Columns.Add("SupGrossWeight", typeof(decimal));
			dtDetail.Columns.Add("SupPckMtrlWeight", typeof(decimal));
			dtDetail.Columns.Add("SupAdLsWt", typeof(decimal));
			dtDetail.Columns.Add("SupNetWeight", typeof(decimal));
			dtDetail.Columns.Add("SupRemarks", typeof(string));
			dtDetail.Columns.Add("CustomerId", typeof(int));
			dtDetail.Columns.Add("CustomerName", typeof(string));
			dtDetail.Columns.Add("CusGrossWeight", typeof(decimal));
			dtDetail.Columns.Add("CusPckMtrlWeight", typeof(decimal));
			dtDetail.Columns.Add("CusAdLsWt", typeof(decimal));
			dtDetail.Columns.Add("CusNetWeight", typeof(decimal));
			dtDetail.Columns.Add("CusRemarks", typeof(string));
			DocumentNoFill();
			Itemfill();
			PackingTypefill();
			PackUOMFill();
			Cropfill();
			suppliercustomer();
			TransportFill();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			txtdocno.Focus();
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
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
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			RecId = 0;
			txtdocno.Text = string.Empty;
			txtdocdate.Value = DateTime.Now;
			txtmanualno.Text = string.Empty;
			txtRemarksHeader.Text = string.Empty;
			txtvehicleno.Text = string.Empty;
			txtBiltyNo.Text = string.Empty;
			txtbiltydate.Value = DateTime.Now;
			((Control)(object)cmbTransporter).Text = string.Empty;
			txtfrieght.Text = string.Empty;
			txtbiltyexpense.Text = string.Empty;
			((Control)(object)cmbBroker).Text = string.Empty;
			txtvehicleweight.Text = string.Empty;
			txtdocno.Focus();
			btnSave.Enabled = true;
			btnUpdate.Visible = false;
			lstRemoveRecord.Clear();
			DocumentNoFill();
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
			((Control)(object)cmbitem).Text = string.Empty;
			((Control)(object)cmbuom).Text = string.Empty;
			txtItemQty.Text = string.Empty;
			((Control)(object)cmbsupplier).Text = string.Empty;
			txtsupgrosswt.Text = string.Empty;
			txtSupAdLsWt.Text = string.Empty;
			txtSupPckMtrlWeight.Text = string.Empty;
			txtSupNetWeight.Text = string.Empty;
			txtsupremarks.Text = string.Empty;
			((Control)(object)cmbcustomer).Text = string.Empty;
			txtcustgrosswt.Text = string.Empty;
			txtcustnetwt.Text = string.Empty;
			txtcustpckmtrlwt.Text = string.Empty;
			txtcustadlswt.Text = string.Empty;
			txtcustremarks.Text = string.Empty;
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			cmbitem.Focus();
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
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
			ResetDetail();
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
			Itemfill();
			PackingTypefill();
			Cropfill();
			suppliercustomer();
			TransportFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Itemfill()
	{
		try
		{
			int idits = Conversion.ToInt(cmbitem.Value);
			DataTable dt = CommonServices.ItemGetForComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					lst.Rows.Add(Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToString(dt.Rows[i]["ItemName"]), Conversion.ToString(dt.Rows[i]["ItemCode"]));
				}
			}
			if (rdSearchByName.Checked)
			{
				DDL.BindDDL(lst, cmbitem, "Id", "ItemName", "Item", ZeroIndex: true);
			}
			else
			{
				DDL.BindDDL(lst, cmbitem, "Id", "ItemCode", "Item", ZeroIndex: true);
			}
			((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			if (idits > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == idits))
				{
					cmbitem.Value = idits;
				}
				else
				{
					((Control)(object)cmbitem).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypefill()
	{
		try
		{
			int idits = Conversion.ToInt(CmbPackingType.Value);
			DataTable lst = CommonServices.InvPackingTypeGetAllService();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, CmbPackingType, "Id", "PackTypeDesc", "Packing Type", true);
				((UltraGridBase)CmbPackingType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (idits > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == idits))
				{
					CmbPackingType.Value = idits;
				}
				else
				{
					((Control)(object)CmbPackingType).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Cropfill()
	{
		try
		{
			int idits = Conversion.ToInt(cmbcropYear.Value);
			DataTable lst = CommonServices.CropYearGetAllService();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDL(lst, cmbcropYear, "Id", "CropYear", "Crop Year", ZeroIndex: true);
				((UltraGridBase)cmbcropYear).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (idits > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == idits))
				{
					cmbcropYear.Value = idits;
				}
				else
				{
					((Control)(object)cmbcropYear).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackUOMFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			int OrderId = Conversion.ToInt(cmbuom.Value);
			UOMSchedule val = new UOMSchedule
			{
				ItemId = Conversion.ToInt(cmbitem.Value),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtsearchby = new DataTable();
			dtsearchby = UOMSchedule.SearchByObject(val);
			if (dtsearchby.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtsearchby, cmbuom, "Id", "Equivalent", "UOM", true);
				((UltraGridBase)cmbuom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (OrderId > 0)
			{
				if (dtsearchby.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
				{
					cmbuom.Value = OrderId;
				}
				else
				{
					((Control)(object)cmbuom).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void suppliercustomer()
	{
		try
		{
			int suppId = Conversion.ToInt(cmbsupplier.Value);
			int custId = Conversion.ToInt(cmbcustomer.Value);
			int bcustId = Conversion.ToInt(cmbBroker.Value);
			dtSupplier = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, cmbsupplier, "Id", "CompanyName", "Supplier", true);
				DDL.BindDDLNew(dtSupplier, cmbcustomer, "Id", "CompanyName", "Customer", true);
				DDL.BindDDLNew(dtSupplier, cmbBroker, "Id", "CompanyName", "Broker", true);
				((UltraGridBase)cmbsupplier).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbcustomer).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbBroker).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
			if (suppId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == suppId))
				{
					cmbsupplier.Value = suppId;
				}
				else
				{
					((Control)(object)cmbsupplier).Text = string.Empty;
				}
			}
			if (custId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == custId))
				{
					cmbcustomer.Value = custId;
				}
				else
				{
					((Control)(object)cmbcustomer).Text = string.Empty;
				}
			}
			if (bcustId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == bcustId))
				{
					cmbBroker.Value = bcustId;
				}
				else
				{
					((Control)(object)cmbBroker).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransportFill()
	{
		try
		{
			int Id = Conversion.ToInt(cmbTransporter.Value);
			dtAccountlst = CommonServices.CoaAllocationGetAllServiceBind();
			if (dtAccountlst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("AccountTitle");
				for (int i = 0; i < dtAccountlst.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtAccountlst.Rows[i]["AccountTypeId"]) != 2 && Conversion.ToInt(dtAccountlst.Rows[i]["AccountTypeId"]) != 15 && Conversion.ToInt(dtAccountlst.Rows[i]["AccountTypeId"]) != 11)
					{
						dt.Rows.Add(dtAccountlst.Rows[i]["Id"], dtAccountlst.Rows[i]["AccountTitle"]);
					}
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbTransporter, "Id", "AccountTitle", "Transporter", ZeroIndex: true);
				}
			}
			if (Id > 0)
			{
				if (dtAccountlst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbTransporter.Value = Id;
				}
				else
				{
					((Control)(object)cmbTransporter).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNoFill()
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
			int code = CommTradeLoadingDelivery.GenerateCode(new CommTradeLoadingDelivery
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1701,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			txtdocno.Text = code.ToString();
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
			int OrderId = Conversion.ToInt(cmbitem.Value);
			if (lst.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(cmbitem.Value) > 0)
				{
					ItemId = Conversion.ToInt(cmbitem.Value);
				}
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(lst, cmbitem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(lst, cmbitem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				cmbitem.Value = ItemId;
				cmbitem.Focus();
			}
			if (OrderId > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
				{
					cmbitem.Value = OrderId;
				}
				else
				{
					((Control)(object)cmbitem).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "")
			{
				cmbitem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbitem_Leave(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			int OrderId = Conversion.ToInt(cmbuom.Value);
			UOMSchedule val = new UOMSchedule
			{
				ItemId = Conversion.ToInt(cmbitem.Value),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtsearchby = new DataTable();
			dtsearchby = UOMSchedule.SearchByObject(val);
			if (dtsearchby.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtsearchby, cmbuom, "Id", "Equivalent", "UOM", true);
				((UltraGridBase)cmbuom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (OrderId > 0)
			{
				if (dtsearchby.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
				{
					cmbuom.Value = OrderId;
				}
				else
				{
					((Control)(object)cmbuom).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (txtvehicleweight.Text == string.Empty)
			{
				MessageBox.Show("Vehicle Weight Required");
			}
			if (((Control)(object)cmbsupplier).Text == ((Control)(object)cmbcustomer).Text)
			{
				MessageBox.Show("Supplier And Customer Not Same!");
			}
			if (FormValidationDetail())
			{
				dtDetail.Rows.Add(0, 0, 0, cmbitem.Value, ((UltraDropDownBase)cmbitem).SelectedRow.Cells["ItemName"].Text, ((UltraDropDownBase)cmbitem).SelectedRow.Cells["ItemCode"].Text, cmbcropYear.Value, ((Control)(object)cmbcropYear).Text, CmbPackingType.Value, ((Control)(object)CmbPackingType).Text, cmbuom.Value, ((Control)(object)cmbuom).Text, Conversion.ToInt((object)txtItemQty.Text.Trim()), cmbsupplier.Value, ((Control)(object)cmbsupplier).Text, Conversion.ToDecimal((object)txtsupgrosswt.Text.Trim()), Conversion.ToDecimal((object)txtSupPckMtrlWeight.Text.Trim()), Conversion.ToDecimal((object)txtSupAdLsWt.Text.Trim()), Conversion.ToDecimal((object)txtSupNetWeight.Text.Trim()), Conversion.ToString((object)txtsupremarks.Text.Trim()), cmbcustomer.Value, ((Control)(object)cmbcustomer).Text, Conversion.ToDecimal((object)txtcustgrosswt.Text.Trim()), Conversion.ToDecimal((object)txtcustpckmtrlwt.Text.Trim()), Conversion.ToDecimal((object)txtcustadlswt.Text.Trim()), Conversion.ToDecimal((object)txtcustnetwt.Text.Trim()), Conversion.ToString((object)txtcustremarks.Text.Trim()));
				grd.DataSource = dtDetail;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetail();
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
			grd.RootTable.Columns["OrderId"].Visible = false;
			grd.RootTable.Columns["OrderDetailId"].Visible = false;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["CropYearId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["UomId"].Visible = false;
			grd.RootTable.Columns["SupplierId"].Visible = false;
			grd.RootTable.Columns["CustomerId"].Visible = false;
			grd.RootTable.Columns["SupGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["SupGrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["SupGrossWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["SupGrossWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["SupPckMtrlWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["SupPckMtrlWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["SupPckMtrlWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["SupPckMtrlWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["SupAdLsWt"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["SupAdLsWt"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["SupAdLsWt"].FormatString = "#,##0.###";
			grd.RootTable.Columns["SupAdLsWt"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["SupNetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["SupNetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["SupNetWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["SupNetWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["CusGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CusGrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CusGrossWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["CusGrossWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["CusPckMtrlWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CusPckMtrlWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CusPckMtrlWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["CusPckMtrlWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["CusAdLsWt"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CusAdLsWt"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CusAdLsWt"].FormatString = "#,##0.###";
			grd.RootTable.Columns["CusAdLsWt"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["CusNetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CusNetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CusNetWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["CusNetWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Key = "Edit";
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 50;
			grd.RootTable.Columns["Edit"].Position = 0;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 50;
			grd.RootTable.Columns["Delete"].Position = 1;
			grd.FrozenColumns = 2;
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
			if (txtvehicleweight.Text == string.Empty)
			{
				MessageBox.Show("Vehicle Weight Required");
			}
			if (FormValidationDetail())
			{
				dtDetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(cmbitem.Value);
				dtDetail.Rows[updateDetailIndex]["ItemName"] = ((UltraDropDownBase)cmbitem).SelectedRow.Cells["ItemName"].Value;
				dtDetail.Rows[updateDetailIndex]["ItemCode"] = ((UltraDropDownBase)cmbitem).SelectedRow.Cells["ItemCode"].Value;
				dtDetail.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(cmbcropYear.Value);
				dtDetail.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)cmbcropYear).Text);
				dtDetail.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(CmbPackingType.Value);
				dtDetail.Rows[updateDetailIndex]["PackingType"] = Conversion.ToString((object)((Control)(object)CmbPackingType).Text);
				dtDetail.Rows[updateDetailIndex]["UomId"] = Conversion.ToInt(cmbuom.Value);
				dtDetail.Rows[updateDetailIndex]["Uom"] = Conversion.ToString((object)((Control)(object)cmbuom).Text);
				dtDetail.Rows[updateDetailIndex]["Qty"] = Conversion.ToInt((object)txtItemQty.Text);
				dtDetail.Rows[updateDetailIndex]["SupplierId"] = Conversion.ToInt(cmbsupplier.Value);
				dtDetail.Rows[updateDetailIndex]["SupplierName"] = Conversion.ToString((object)((Control)(object)cmbsupplier).Text);
				dtDetail.Rows[updateDetailIndex]["SupGrossWeight"] = Conversion.ToDecimal((object)txtsupgrosswt.Text);
				dtDetail.Rows[updateDetailIndex]["SupPckMtrlWeight"] = Conversion.ToDecimal((object)txtSupPckMtrlWeight.Text);
				dtDetail.Rows[updateDetailIndex]["SupAdLsWt"] = Conversion.ToDecimal((object)txtSupAdLsWt.Text);
				dtDetail.Rows[updateDetailIndex]["SupNetWeight"] = Conversion.ToDecimal((object)txtSupNetWeight.Text);
				dtDetail.Rows[updateDetailIndex]["SupRemarks"] = Conversion.ToString((object)txtsupremarks.Text);
				dtDetail.Rows[updateDetailIndex]["CustomerId"] = Conversion.ToInt(cmbcustomer.Value);
				dtDetail.Rows[updateDetailIndex]["CustomerName"] = Conversion.ToString((object)((Control)(object)cmbcustomer).Text);
				dtDetail.Rows[updateDetailIndex]["CusGrossWeight"] = Conversion.ToDecimal((object)txtcustgrosswt.Text);
				dtDetail.Rows[updateDetailIndex]["CusPckMtrlWeight"] = Conversion.ToDecimal((object)txtcustpckmtrlwt.Text);
				dtDetail.Rows[updateDetailIndex]["CusAdLsWt"] = Conversion.ToDecimal((object)txtcustadlswt.Text);
				dtDetail.Rows[updateDetailIndex]["CusNetWeight"] = Conversion.ToDecimal((object)txtcustnetwt.Text);
				dtDetail.Rows[updateDetailIndex]["CusRemarks"] = Conversion.ToString((object)txtcustremarks.Text);
				grd.DataSource = dtDetail;
				btnAddDetail.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelDetail.Visible = false;
				btnSave.Visible = false;
				btnUpdate.Visible = true;
				ResetDetail();
			}
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
			if (item != null && (int)item.RowType == 0 && grd.CurrentColumn != null)
			{
				updateDetailIndex = item.RowIndex;
				cmbitem.Value = Conversion.ToString(item.Cells["ItemId"].Value);
				cmbcropYear.Value = item.Cells["CropYearId"].Value.ToString();
				CmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				cmbuom.Value = item.Cells["UomId"].Value.ToString();
				txtItemQty.Text = item.Cells["Qty"].Value.ToString();
				cmbsupplier.Value = item.Cells["SupplierId"].Value.ToString();
				txtsupgrosswt.Text = item.Cells["SupGrossWeight"].Value.ToString();
				txtSupPckMtrlWeight.Text = item.Cells["SupPckMtrlWeight"].Value.ToString();
				txtSupAdLsWt.Text = item.Cells["SupAdLsWt"].Value.ToString();
				txtSupNetWeight.Text = item.Cells["SupNetWeight"].Value.ToString();
				txtsupremarks.Text = item.Cells["SupRemarks"].Value.ToString();
				cmbcustomer.Value = item.Cells["CustomerId"].Value.ToString();
				txtcustgrosswt.Text = item.Cells["CusGrossWeight"].Value.ToString();
				txtcustpckmtrlwt.Text = item.Cells["CusPckMtrlWeight"].Value.ToString();
				txtcustadlswt.Text = item.Cells["CusAdLsWt"].Value.ToString();
				txtcustnetwt.Text = item.Cells["CusNetWeight"].Value.ToString();
				txtcustremarks.Text = item.Cells["CusRemarks"].Value.ToString();
				btnAddDetail.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				cmbitem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Edit")
			{
				grd_DoubleClick(null, null);
			}
			if (!(e.Column.Key == "Delete"))
			{
				return;
			}
			GridEXRow r = grd.CurrentRow;
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					CommTradeLoadingDeliveryDetail vd = new CommTradeLoadingDeliveryDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.BrokerId = Conversion.ToInt(cmbBroker.Value);
					vd.TransporterId = Conversion.ToInt(cmbTransporter.Value);
					vd.BiltyNo = Conversion.ToString((object)txtBiltyNo.Text);
					vd.BiltyExpense = Conversion.ToDecimal((object)txtbiltyexpense.Text);
					vd.BiltyDate = Conversion.ToDateTime((object)txtbiltydate.Value);
					vd.DocDateDt = DateTime.Now;
					vd.VehicleNo = Conversion.ToString((object)txtvehicleno.Text);
					vd.BiltyFreight = Conversion.ToDecimal((object)txtfrieght.Text);
					vd.VehicleGrossWeight = Conversion.ToDecimal((object)txtvehicleweight.Text);
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					vd.PackingTypeId = Conversion.ToInt((object)r.Cells["PackingTypeId"].Text);
					vd.UomId = Conversion.ToInt((object)r.Cells["UomId"].Text);
					vd.Qty = Conversion.ToInt(r.Cells["Qty"].Value);
					vd.SupplierId = Conversion.ToInt(r.Cells["SupplierId"].Value);
					vd.SupGrossWeight = Conversion.ToDecimal(r.Cells["SupGrossWeight"].Value);
					vd.SupPckMtrlWeight = Conversion.ToDecimal(r.Cells["SupPckMtrlWeight"].Value);
					vd.SupAdLsWt = Conversion.ToDecimal(r.Cells["SupAdLsWt"].Value);
					vd.SupNetWeight = Conversion.ToDecimal(r.Cells["SupNetWeight"].Value);
					vd.SupRemarks = Conversion.ToString(r.Cells["SupRemarks"].Value);
					vd.CustomerId = Conversion.ToInt(r.Cells["CustomerId"].Value);
					vd.CusGrossWeight = Conversion.ToDecimal(r.Cells["CusGrossWeight"].Value);
					vd.CusPckMtrlWeight = Conversion.ToDecimal(r.Cells["CusPckMtrlWeight"].Value);
					vd.CusAdLsWt = Conversion.ToDecimal(r.Cells["CusAdLsWt"].Value);
					vd.CusNetWeight = Conversion.ToDecimal(r.Cells["CusNetWeight"].Value);
					vd.CusRemarks = Conversion.ToString(r.Cells["CusRemarks"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					dtDetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grd.UpdateData();
				}
			}
			else if (r != null && (int)r.RowType == 0)
			{
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)cmbitem).ActiveRow == null || Conversion.ToInt(cmbitem.Value) == 0)
		{
			MessageBox.Show("Item Field is Required");
			cmbitem.Focus();
			return false;
		}
		if (((UltraGridBase)cmbcropYear).ActiveRow == null || Conversion.ToInt(cmbcropYear.Value) == 0)
		{
			MessageBox.Show("Crop Year Field is Required");
			cmbcropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingType).ActiveRow == null || Conversion.ToInt(CmbPackingType.Value) == 0)
		{
			MessageBox.Show("Packing Type Field is Required");
			CmbPackingType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbuom).ActiveRow == null || Conversion.ToInt(cmbuom.Value) == 0)
		{
			MessageBox.Show("UOM Field is Required");
			cmbuom.Focus();
			return false;
		}
		if (txtItemQty.Text.Trim() == "" || Conversion.ToDouble((object)txtItemQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtItemQty.Focus();
			return false;
		}
		if (((UltraGridBase)cmbsupplier).ActiveRow == null || Conversion.ToInt(cmbsupplier.Value) == 0)
		{
			MessageBox.Show("Supplier Field is Required");
			cmbsupplier.Focus();
			return false;
		}
		if (txtsupgrosswt.Text.Trim() == "" || Conversion.ToDouble((object)txtsupgrosswt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Supplier Gross Weight Field is Required");
			txtsupgrosswt.Focus();
			return false;
		}
		if (txtSupNetWeight.Text.Trim() == "" || Conversion.ToDouble((object)txtSupNetWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Supplier Net Weught Field is Required");
			txtSupNetWeight.Focus();
			return false;
		}
		if (((UltraGridBase)cmbcustomer).ActiveRow == null || Conversion.ToInt(cmbcustomer.Value) == 0)
		{
			MessageBox.Show("Customer Field is Required");
			cmbcustomer.Focus();
			return false;
		}
		if (txtcustgrosswt.Text.Trim() == "" || Conversion.ToDouble((object)txtcustgrosswt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Customer Gross Weight Field is Required");
			txtcustgrosswt.Focus();
			return false;
		}
		if (txtcustnetwt.Text.Trim() == "" || Conversion.ToDouble((object)txtcustnetwt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Customer Net Weught Field is Required");
			txtcustnetwt.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
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
			CommTradeLoadingDelivery ct = new CommTradeLoadingDelivery();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				ct.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			ct.OrganizationId = UserAccount.OrganizationId;
			ct.CompanyId = UserAccount.CompanyId;
			ct.BranchesId = UserAccount.BranchesId;
			ct.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			ct.DocumentTypeId = 1701;
			ct.ModifyUserId = UserAccount.ID;
			ct.EntryUserId = UserAccount.ID;
			ct.EntryDate = DateTime.Now;
			ct.ModifyDate = DateTime.Now;
			ct.ApprovedDate = DateTime.Now;
			ct.DocDate = DateTime.Now;
			ct.ManualRefNo = txtmanualno.Text;
			ct.ApprovedUserId = UserAccount.ID;
			ct.ActionId = UserAccount.ID;
			ct.DocNo = Conversion.ToInt((object)txtdocno.Text);
			ct.IsApproved = true;
			ct.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			ct.CommTradeLoadingDeliveryDetailList = new List<CommTradeLoadingDeliveryDetail>();
			if (grd.RowCount > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					CommTradeLoadingDeliveryDetail vd = new CommTradeLoadingDeliveryDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					if (vd.Id > 0)
					{
						vd.ActionTypeId = 2;
					}
					else
					{
						vd.ActionTypeId = 1;
					}
					vd.BrokerId = Conversion.ToInt(cmbBroker.Value);
					vd.TransporterId = Conversion.ToInt(cmbTransporter.Value);
					vd.BiltyNo = Conversion.ToString((object)txtBiltyNo.Text);
					vd.BiltyExpense = Conversion.ToDecimal((object)txtbiltyexpense.Text);
					vd.BiltyDate = Conversion.ToDateTime((object)txtbiltydate.Value);
					vd.DocDateDt = DateTime.Now;
					vd.VehicleNo = Conversion.ToString((object)txtvehicleno.Text);
					vd.BiltyFreight = Conversion.ToDecimal((object)txtfrieght.Text);
					vd.VehicleGrossWeight = Conversion.ToDecimal((object)txtvehicleweight.Text);
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					vd.PackingTypeId = Conversion.ToInt((object)r.Cells["PackingTypeId"].Text);
					vd.UomId = Conversion.ToInt((object)r.Cells["UomId"].Text);
					vd.Qty = Conversion.ToInt(r.Cells["Qty"].Value);
					vd.SupplierId = Conversion.ToInt(r.Cells["SupplierId"].Value);
					vd.SupGrossWeight = Conversion.ToDecimal(r.Cells["SupGrossWeight"].Value);
					vd.SupPckMtrlWeight = Conversion.ToDecimal(r.Cells["SupPckMtrlWeight"].Value);
					vd.SupAdLsWt = Conversion.ToDecimal(r.Cells["SupAdLsWt"].Value);
					vd.SupNetWeight = Conversion.ToDecimal(r.Cells["SupNetWeight"].Value);
					vd.SupRemarks = Conversion.ToString(r.Cells["SupRemarks"].Value);
					vd.CustomerId = Conversion.ToInt(r.Cells["CustomerId"].Value);
					vd.CusGrossWeight = Conversion.ToDecimal(r.Cells["CusGrossWeight"].Value);
					vd.CusPckMtrlWeight = Conversion.ToDecimal(r.Cells["CusPckMtrlWeight"].Value);
					vd.CusAdLsWt = Conversion.ToDecimal(r.Cells["CusAdLsWt"].Value);
					vd.CusNetWeight = Conversion.ToDecimal(r.Cells["CusNetWeight"].Value);
					vd.CusRemarks = Conversion.ToString(r.Cells["CusRemarks"].Value);
					ct.CommTradeLoadingDeliveryDetailList.Add(vd);
				}
				if (RecId > 0 && lstRemoveRecord.Count > 0)
				{
					for (int i = 0; i < lstRemoveRecord.Count; i++)
					{
						ct.CommTradeLoadingDeliveryDetailList.Add(lstRemoveRecord[i]);
					}
				}
				ct.AttachmentsList = new List<DMSAttachments>();
				ct.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<CommTradeLoadingDelivery>(ct, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = CommTradeLoadingDelivery.Save(ct);
				ct.Id = success;
				if (RecId > 0)
				{
					MessageBox.Show("Data Update Successfully....  " + ct.DocNo);
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						DMSAttachments.RemoveByIdAndNames(RecId, base.Name, ct.DocumentTypeId, 0);
					}
				}
				else
				{
					MessageBox.Show("Data Save Successfully....  " + ct.DocNo);
				}
				Reset();
				ResetDetail();
				grd.DataSource = null;
				btnSave.Visible = true;
			}
			else
			{
				MessageBox.Show("Grid Record Not Found");
			}
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int ID)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		try
		{
			lstRemoveRecord.Clear();
			RecId = Conversion.ToInt((object)ID);
			CommTradeLoadingDelivery po = new CommTradeLoadingDelivery();
			po = CommTradeLoadingDelivery.ReadById(RecId);
			tabControl1.SelectedIndex = 0;
			txtdocdate.Value = po.DocDate;
			txtdocno.Text = Conversion.ToString((object)po.DocNo);
			txtmanualno.Text = Conversion.ToString((object)po.ManualRefNo);
			txtRemarksHeader.Text = Conversion.ToString((object)po.RemarksHeader);
			if (po.CommTradeLoadingDeliveryDetailList.Count > 0)
			{
				cmbBroker.Value = Conversion.ToInt((object)po.CommTradeLoadingDeliveryDetailList[0].BrokerId);
				cmbTransporter.Value = Conversion.ToInt((object)po.CommTradeLoadingDeliveryDetailList[0].TransporterId);
				txtBiltyNo.Text = Conversion.ToString((object)po.CommTradeLoadingDeliveryDetailList[0].BiltyNo);
				txtbiltyexpense.Text = Conversion.ToDecimal((object)po.CommTradeLoadingDeliveryDetailList[0].BiltyExpense).ToString();
				txtbiltydate.Value = Conversion.ToDateTime((object)po.CommTradeLoadingDeliveryDetailList[0].BiltyDate);
				txtvehicleno.Text = Conversion.ToString((object)po.CommTradeLoadingDeliveryDetailList[0].VehicleNo);
				txtfrieght.Text = Conversion.ToDecimal((object)po.CommTradeLoadingDeliveryDetailList[0].BiltyFreight).ToString();
				txtvehicleweight.Text = Conversion.ToString((object)po.CommTradeLoadingDeliveryDetailList[0].VehicleGrossWeight);
				dtDetail.Clear();
				for (int i = 0; i < po.CommTradeLoadingDeliveryDetailList.Count; i++)
				{
					dtDetail.Rows.Add(po.CommTradeLoadingDeliveryDetailList[i].Id, po.CommTradeLoadingDeliveryDetailList[i].OrderId, po.CommTradeLoadingDeliveryDetailList[i].OrderDetailId, po.CommTradeLoadingDeliveryDetailList[i].ItemId, po.CommTradeLoadingDeliveryDetailList[i].ItemCode, po.CommTradeLoadingDeliveryDetailList[i].ItemName, Conversion.ToInt((object)po.CommTradeLoadingDeliveryDetailList[i].CropYearId), po.CommTradeLoadingDeliveryDetailList[i].CropYear, po.CommTradeLoadingDeliveryDetailList[i].PackingTypeId, po.CommTradeLoadingDeliveryDetailList[i].PackingType, po.CommTradeLoadingDeliveryDetailList[i].UomId, po.CommTradeLoadingDeliveryDetailList[i].PackUom, po.CommTradeLoadingDeliveryDetailList[i].Qty, po.CommTradeLoadingDeliveryDetailList[i].SupplierId, po.CommTradeLoadingDeliveryDetailList[i].SupplierName, po.CommTradeLoadingDeliveryDetailList[i].SupGrossWeight, po.CommTradeLoadingDeliveryDetailList[i].SupPckMtrlWeight, po.CommTradeLoadingDeliveryDetailList[i].SupAdLsWt, po.CommTradeLoadingDeliveryDetailList[i].SupNetWeight, po.CommTradeLoadingDeliveryDetailList[i].SupRemarks, po.CommTradeLoadingDeliveryDetailList[i].CustomerId, po.CommTradeLoadingDeliveryDetailList[i].CustomerName, po.CommTradeLoadingDeliveryDetailList[i].CusGrossWeight, po.CommTradeLoadingDeliveryDetailList[i].CusPckMtrlWeight, po.CommTradeLoadingDeliveryDetailList[i].CusAdLsWt, po.CommTradeLoadingDeliveryDetailList[i].CusNetWeight, po.CommTradeLoadingDeliveryDetailList[i].CusRemarks);
				}
				grd.DataSource = dtDetail;
				grd.RetrieveStructure();
				grdSettings();
			}
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
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
					CommTradeLoadingDelivery.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Seccessfully");
					Reset();
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				gridfill(50);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridfill(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocumentTypeId = 1701;
			obj.EntryUser = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.NoOfRecords = NoOfRecords;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			dtgrid = CommTradeLoadingDelivery.FormHistory(obj);
			if (dtgrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("RecordNo", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(string));
				dt.Columns.Add("ManualRefNo", typeof(string));
				dt.Columns.Add("IsApproved", typeof(bool));
				dt.Columns.Add("ApprovedUserId", typeof(int));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("TotalQty", typeof(int));
				dt.Columns.Add("TotalWeight", typeof(decimal));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Clear();
				for (int i = 0; i < dtgrid.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dtgrid.Rows[i]["RecordNo"]), Conversion.ToInt(dtgrid.Rows[i]["DocumentTypeId"]), Conversion.ToInt(dtgrid.Rows[i]["Id"]), Conversion.ToDateTime(dtgrid.Rows[i]["DocDate"]), Conversion.ToString(dtgrid.Rows[i]["DocNo"]), Conversion.ToString(dtgrid.Rows[i]["ManualRefNo"]), Conversion.ToBool(dtgrid.Rows[i]["IsApproved"]), Conversion.ToInt(dtgrid.Rows[i]["ApprovedUserId"]), Conversion.ToDateTime(dtgrid.Rows[i]["ApprovedDate"]), Conversion.ToDecimal(dtgrid.Rows[i]["TotalQty"]), Conversion.ToDecimal(dtgrid.Rows[i]["TotalWeight"]), Conversion.ToDateTime(dtgrid.Rows[i]["EntryDate"]), Conversion.ToString(dtgrid.Rows[i]["EntryUserName"]), Conversion.ToDateTime(dtgrid.Rows[i]["ModifyDate"]), Conversion.ToString(dtgrid.Rows[i]["ModifyUserName"]), Conversion.ToString(dtgrid.Rows[i]["RemarksHeader"]), Conversion.ToInt(dtgrid.Rows[i]["NoOfAttachments"]));
				}
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				grdsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsetting()
	{
		try
		{
			grdHistory.RootTable.Columns["RecordNo"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["ApprovedUserId"].Visible = false;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["TotalWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["TotalWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["TotalWeight"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["TotalWeight"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["TotalQty"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdHistory);
			if (formright.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns.Add("Edit");
				grdHistory.RootTable.Columns["Edit"].Key = "Edit";
				grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
				grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				grdHistory.RootTable.Columns["Edit"].Width = 50;
			}
			if (formright.DoHavePrintRights)
			{
				grdHistory.RootTable.Columns.Add("Print");
				grdHistory.RootTable.Columns["Print"].Key = "Print";
				grdHistory.RootTable.Columns["Print"].Caption = "Print";
				grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Print"].ButtonText = "Print";
				grdHistory.RootTable.Columns["Print"].Width = 50;
			}
			if (formright.DoHaveUpdateRights && formright.DoHavePrintRights)
			{
				grdHistory.RootTable.Columns["Print"].Position = 0;
				grdHistory.RootTable.Columns["Edit"].Position = 1;
				grdHistory.FrozenColumns = 2;
			}
			else if (formright.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns["Edit"].Position = 0;
				grdHistory.FrozenColumns = 1;
			}
			else if (formright.DoHavePrintRights)
			{
				grdHistory.RootTable.Columns["Print"].Position = 0;
				grdHistory.FrozenColumns = 1;
			}
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (formright.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
				txtdocdate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit" && formright.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
				txtdocdate.Focus();
			}
			if (e.Column.Key == "Print" && formright.DoHavePrintRights)
			{
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				GenerateSlip556Print(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailbind(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new CommTradeLoadingDelivery();
			DataTable dtdeta = CommTradeLoadingDelivery.FormHistoryDetail(Id);
			if (dtdeta.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("OrderId", typeof(int));
				dt.Columns.Add("OrderDetailId", typeof(int));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("CropYearId", typeof(int));
				dt.Columns.Add("CropYear", typeof(string));
				dt.Columns.Add("PackingTypeId", typeof(int));
				dt.Columns.Add("PackingType", typeof(string));
				dt.Columns.Add("UomId", typeof(int));
				dt.Columns.Add("UOM", typeof(string));
				dt.Columns.Add("Qty", typeof(int));
				dt.Columns.Add("SupplierId", typeof(int));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("SupGrossWeight", typeof(decimal));
				dt.Columns.Add("SupPckMtrlWeight", typeof(decimal));
				dt.Columns.Add("SupAdLsWt", typeof(decimal));
				dt.Columns.Add("SupNetWeight", typeof(decimal));
				dt.Columns.Add("SupRemarks", typeof(string));
				dt.Columns.Add("CustomerId", typeof(int));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("CusGrossWeight", typeof(decimal));
				dt.Columns.Add("CusPckMtrlWeight", typeof(decimal));
				dt.Columns.Add("CusAdLsWt", typeof(decimal));
				dt.Columns.Add("CusNetWeight", typeof(decimal));
				dt.Columns.Add("CusRemarks", typeof(string));
				dt.Clear();
				for (int i = 0; i < dtdeta.Rows.Count; i++)
				{
					dt.Rows.Add(dtdeta.Rows[i]["Id"], dtdeta.Rows[i]["OrderId"], dtdeta.Rows[i]["OrderDetailId"], dtdeta.Rows[i]["ItemId"], dtdeta.Rows[i]["ItemCode"], dtdeta.Rows[i]["ItemName"], dtdeta.Rows[i]["CropYearId"], dtdeta.Rows[i]["CropYear"], dtdeta.Rows[i]["PackingTypeId"], dtdeta.Rows[i]["PackingType"], dtdeta.Rows[i]["UomId"], dtdeta.Rows[i]["PackUom"], dtdeta.Rows[i]["Qty"], dtdeta.Rows[i]["SupplierId"], dtdeta.Rows[i]["SupplierName"], dtdeta.Rows[i]["SupGrossWeight"], dtdeta.Rows[i]["SupPckMtrlWeight"], dtdeta.Rows[i]["SupAdLsWt"], dtdeta.Rows[i]["SupNetWeight"], dtdeta.Rows[i]["SupRemarks"], dtdeta.Rows[i]["CustomerId"], dtdeta.Rows[i]["CustomerName"], dtdeta.Rows[i]["CusGrossWeight"], dtdeta.Rows[i]["CusPckMtrlWeight"], dtdeta.Rows[i]["CusAdLsWt"], dtdeta.Rows[i]["CusNetWeight"], dtdeta.Rows[i]["CusRemarks"]);
				}
				grdHistoryDetail.DataSource = dt;
				grdHistoryDetail.RetrieveStructure();
				grddetailsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailsetting()
	{
		try
		{
			grdHistoryDetail.RootTable.Columns["OrderId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["OrderDetailId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["Id"].Visible = false;
			grdHistoryDetail.RootTable.Columns["CropYearId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["UomId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["SupplierId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["CustomerId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupPckMtrlWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupPckMtrlWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupPckMtrlWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupPckMtrlWeight"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupAdLsWt"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupAdLsWt"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupAdLsWt"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupAdLsWt"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusPckMtrlWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusPckMtrlWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusPckMtrlWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusPckMtrlWeight"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusAdLsWt"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusAdLsWt"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusAdLsWt"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusAdLsWt"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			List<DMSAttachments> dMSList = DMSAttachments.GetByID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment);
				}
			}
			else
			{
				dtViewAttachments.Clear();
			}
			new AttachmentView(dtViewAttachments).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				grddetailbind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoaddAllHistory_Click(object sender, EventArgs e)
	{
		try
		{
			gridfill();
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

	private void CommTradeLoadingDelivery_KeyDown(object sender, KeyEventArgs e)
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
					txtdocdate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)grdHistory).Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (formright.DoHaveCanDelete && e.Control && e.Shift && e.KeyCode == Keys.Delete)
			{
				btnDelete_Click(null, null);
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnSave.Visible && btnSave.Enabled && tabControl1.SelectedIndex == 0)
					{
						btnSave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnUpdate.Visible && btnUpdate.Enabled && (UpdateMode = true))
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
					if (tabControl1.SelectedIndex == 0)
					{
						btnNew_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					txtdocdate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbitem.Focus();
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistoryDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdHistory).Focus();
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
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow item2 = grd.CurrentRow;
				if (item2 != null && (int)item2.RowType == 0 && grd.CurrentColumn != null)
				{
					Conversion.ToInt((object)item2.Cells["Id"].Value.ToString());
					if (grd.CurrentColumn.Key == "Edit")
					{
						grd_DoubleClick(null, null);
					}
					if (grd.CurrentColumn.Key == "Delete")
					{
						CommTradeLoadingDeliveryDetail vd = new CommTradeLoadingDeliveryDetail();
						vd.BrokerId = Conversion.ToInt(cmbBroker.Value);
						vd.TransporterId = Conversion.ToInt(cmbTransporter.Value);
						vd.BiltyNo = Conversion.ToString((object)txtBiltyNo.Text);
						vd.BiltyExpense = Conversion.ToDecimal((object)txtbiltyexpense.Text);
						vd.BiltyDate = Conversion.ToDateTime((object)txtbiltydate.Value);
						vd.DocDateDt = DateTime.Now;
						vd.VehicleNo = Conversion.ToString((object)txtvehicleno.Text);
						vd.BiltyFreight = Conversion.ToDecimal((object)txtfrieght.Text);
						vd.VehicleGrossWeight = Conversion.ToDecimal((object)txtvehicleweight.Text);
						vd.ItemId = Conversion.ToInt(item2.Cells["ItemId"].Value);
						vd.CropYearId = Conversion.ToInt(item2.Cells["CropYearId"].Value);
						vd.PackingTypeId = Conversion.ToInt((object)item2.Cells["PackingTypeId"].Text);
						vd.UomId = Conversion.ToInt((object)item2.Cells["UomId"].Text);
						vd.Qty = Conversion.ToInt(item2.Cells["Qty"].Value);
						vd.SupplierId = Conversion.ToInt(item2.Cells["SupplierId"].Value);
						vd.SupGrossWeight = Conversion.ToDecimal(item2.Cells["SupGrossWeight"].Value);
						vd.SupPckMtrlWeight = Conversion.ToDecimal(item2.Cells["SupPckMtrlWeight"].Value);
						vd.SupAdLsWt = Conversion.ToDecimal(item2.Cells["SupAdLsWt"].Value);
						vd.SupNetWeight = Conversion.ToDecimal(item2.Cells["SupNetWeight"].Value);
						vd.SupRemarks = Conversion.ToString(item2.Cells["SupRemarks"].Value);
						vd.CustomerId = Conversion.ToInt(item2.Cells["CustomerId"].Value);
						vd.CusGrossWeight = Conversion.ToDecimal(item2.Cells["CusGrossWeight"].Value);
						vd.CusPckMtrlWeight = Conversion.ToDecimal(item2.Cells["CusPckMtrlWeight"].Value);
						vd.CusAdLsWt = Conversion.ToDecimal(item2.Cells["CusAdLsWt"].Value);
						vd.CusNetWeight = Conversion.ToDecimal(item2.Cells["CusNetWeight"].Value);
						vd.CusRemarks = Conversion.ToString(item2.Cells["CusRemarks"].Value);
						vd.ActionTypeId = 3;
						lstRemoveRecord.Add(vd);
						dtDetail.Rows[item2.RowIndex].Delete();
						if (item2 != null && (int)item2.RowType == 0)
						{
							item2.Delete();
						}
						grd.UpdateData();
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && (int)item.RowType == 0 && grd.CurrentColumn != null)
				{
					grd_DoubleClick(null, null);
				}
			}
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
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				int Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(Id);
					txtdocdate.Focus();
				}
				if (grdHistory.CurrentColumn.Key == "Print")
				{
					Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					GenerateSlip556Print(Id);
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt((object)Id.ToString()), 1701);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formright.DoHaveUpdateRights)
				{
					RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
					txtdocdate.Focus();
				}
			}
			else if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				RecId = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				GenerateSlip556Print(RecId);
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtbiltyexpense_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtfrieght_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtvehicleweight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtItemQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtsupgrosswt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSupPckMtrlWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSupAdLsWt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSupNetWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcustgrosswt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcustpckmtrlwt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcustadlswt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcustnetwt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void DetailGrossWeightCalculation()
	{
		try
		{
			decimal PackUOM = default(decimal);
			decimal Qty = default(decimal);
			decimal GrossWeight = default(decimal);
			if (!string.IsNullOrEmpty(txtItemQty.Text))
			{
				Qty = Conversion.ToDecimal((object)txtItemQty.Text.ToString());
			}
			if (((UltraGridBase)cmbuom).ActiveRow != null && Conversion.ToInt(cmbuom.Value) != 0)
			{
				PackUOM = Conversion.ToDecimal((object)((Control)(object)cmbuom).Text.ToString());
			}
			GrossWeight = Qty * PackUOM;
			txtsupgrosswt.Text = GrossWeight.ToString();
			txtcustgrosswt.Text = GrossWeight.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailNetWeightCalculation()
	{
		try
		{
			decimal CusGrossWeight = default(decimal);
			decimal SupGrossWeight = default(decimal);
			decimal NetWeightSup = default(decimal);
			decimal NetWeightCus = default(decimal);
			decimal AddLessSup = default(decimal);
			decimal WeightCutSup = default(decimal);
			decimal AddLessCus = default(decimal);
			decimal WeightCutCus = default(decimal);
			if (txtcustadlswt.Text != string.Empty && txtcustadlswt.Text != null)
			{
				AddLessCus = Conversion.ToDecimal((object)txtcustadlswt.Text.Trim().ToString());
			}
			if (txtcustpckmtrlwt.Text != string.Empty && txtcustpckmtrlwt.Text != null)
			{
				WeightCutCus = Conversion.ToDecimal((object)txtcustpckmtrlwt.Text.Trim().ToString());
			}
			if (txtSupAdLsWt.Text != string.Empty && txtSupAdLsWt.Text != null)
			{
				AddLessSup = Conversion.ToDecimal((object)txtSupAdLsWt.Text.Trim().ToString());
			}
			if (txtSupPckMtrlWeight.Text != string.Empty && txtSupPckMtrlWeight.Text != null)
			{
				WeightCutSup = Conversion.ToDecimal((object)txtSupPckMtrlWeight.Text.Trim().ToString());
			}
			if (txtsupgrosswt.Text != string.Empty && txtsupgrosswt.Text != null)
			{
				SupGrossWeight = Conversion.ToDecimal((object)txtsupgrosswt.Text.Trim().ToString());
			}
			if (txtcustgrosswt.Text != string.Empty && txtcustgrosswt.Text != null)
			{
				CusGrossWeight = Conversion.ToDecimal((object)txtcustgrosswt.Text.Trim().ToString());
			}
			txtsupgrosswt.Text = SupGrossWeight.ToString();
			txtcustgrosswt.Text = CusGrossWeight.ToString();
			NetWeightSup = SupGrossWeight - WeightCutSup + AddLessSup;
			NetWeightCus = CusGrossWeight - WeightCutCus + AddLessCus;
			txtcustnetwt.Text = Conversion.ToDecimal((object)NetWeightCus).ToString();
			txtSupNetWeight.Text = Conversion.ToDecimal((object)NetWeightSup).ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbuom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailGrossWeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailGrossWeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtsupgrosswt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailNetWeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupAdLsWt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailNetWeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupPckMtrlWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailNetWeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcustgrosswt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailNetWeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcustpckmtrlwt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailNetWeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcustadlswt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailNetWeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateSlip556Print(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSlip556Print(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = CommTradeLoadingDelivery.CommissionTradeLoadingDeliverySlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "556-CommissionTradeLoadingDelivery_Slip.rpt");
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
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
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Expected O, but got Unknown
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Expected O, but got Unknown
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Expected O, but got Unknown
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Expected O, but got Unknown
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Expected O, but got Unknown
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Expected O, but got Unknown
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Expected O, but got Unknown
		//IL_068b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0695: Expected O, but got Unknown
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Expected O, but got Unknown
		//IL_48c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_48d3: Expected O, but got Unknown
		//IL_5eff: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f09: Expected O, but got Unknown
		//IL_5f2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f37: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.CmTr.TransactionForms.CommTradeLoadingDelivery));
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
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
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
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
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbTransporter = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbBroker = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.txtRemarksHeader = new System.Windows.Forms.RichTextBox();
		this.txtbiltyexpense = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtfrieght = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtvehicleweight = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.txtbiltydate = new System.Windows.Forms.DateTimePicker();
		this.txtmanualno = new System.Windows.Forms.TextBox();
		this.lbl = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtBiltyNo = new System.Windows.Forms.TextBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label34 = new System.Windows.Forms.Label();
		this.txtcustremarks = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtcustnetwt = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtcustadlswt = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.txtcustpckmtrlwt = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.txtcustgrosswt = new System.Windows.Forms.TextBox();
		this.cmbcustomer = new UltraCombo();
		this.label39 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label18 = new System.Windows.Forms.Label();
		this.txtsupremarks = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtSupNetWeight = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtSupAdLsWt = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtSupPckMtrlWeight = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtsupgrosswt = new System.Windows.Forms.TextBox();
		this.cmbsupplier = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtItemQty = new System.Windows.Forms.TextBox();
		this.cmbuom = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbitem = new UltraCombo();
		this.labItemName = new System.Windows.Forms.Label();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label46 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnAddDetail = new System.Windows.Forms.Button();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.cmbcropYear = new UltraCombo();
		this.CmbPackingType = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdHistoryDetail = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.grdHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoaddAllHistory = new System.Windows.Forms.ToolStripButton();
		this.label20 = new System.Windows.Forms.Label();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTransporter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBroker).BeginInit();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcustomer).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		this.panel4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label23);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(995, 52);
		this.panel1.TabIndex = 29;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 28);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(300, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Commission Trade Loading Delivery";
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnNew, this.btnRefresh, this.btnUpdate, this.btnSave, this.btnAttachment, this.btnPrint, this.btnDelete, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(995, 25);
		this.toolStrip1.TabIndex = 28;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(88, 22);
		this.btnPrint.Text = "556-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(53, 22);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnshortcutkeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = (System.Drawing.Image)resources.GetObject("btnshortcutkeys.Image");
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(106, 22);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTransporter);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBroker);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.txtRemarksHeader);
		this.groupBox1.Controls.Add(this.txtbiltyexpense);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtfrieght);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtvehicleweight);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.txtbiltydate);
		this.groupBox1.Controls.Add(this.txtmanualno);
		this.groupBox1.Controls.Add(this.lbl);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtdocdate);
		this.groupBox1.Controls.Add(this.txtBiltyNo);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.txtvehicleno);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(988, 131);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Infomation";
		this.cmbTransporter.AllowNull = (DefaultableBoolean)1;
		this.cmbTransporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTransporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTransporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTransporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance69;
		((SpecialBoxBase)((UltraGridBase)this.cmbTransporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance90).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTransporter).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbTransporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTransporter).Location = new System.Drawing.Point(662, 16);
		((System.Windows.Forms.Control)(object)this.cmbTransporter).Name = "cmbTransporter";
		((System.Windows.Forms.Control)(object)this.cmbTransporter).Size = new System.Drawing.Size(319, 26);
		((System.Windows.Forms.Control)(object)this.cmbTransporter).TabIndex = 5;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(780, 50);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(76, 15);
		this.label1.TabIndex = 107;
		this.label1.Text = "Bilty Expense\r\n";
		this.cmbBroker.AllowNull = (DefaultableBoolean)1;
		this.cmbBroker.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBroker.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBroker.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbBroker).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBroker).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBroker).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBroker).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.cmbBroker).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBroker).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbBroker).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBroker).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbBroker).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBroker).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBroker).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBroker).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbBroker.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBroker).Location = new System.Drawing.Point(69, 98);
		((System.Windows.Forms.Control)(object)this.cmbBroker).Name = "cmbBroker";
		((System.Windows.Forms.Control)(object)this.cmbBroker).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.cmbBroker).TabIndex = 3;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(780, 104);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(87, 15);
		this.label4.TabIndex = 109;
		this.label4.Text = "Vehicle Weight";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(3, 104);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(41, 15);
		this.label27.TabIndex = 127;
		this.label27.Text = "Broker";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(592, 22);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(67, 15);
		this.label29.TabIndex = 103;
		this.label29.Text = "Transporter";
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(287, 45);
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(299, 80);
		this.txtRemarksHeader.TabIndex = 4;
		this.txtRemarksHeader.Text = "";
		this.txtbiltyexpense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbiltyexpense.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbiltyexpense.Location = new System.Drawing.Point(867, 46);
		this.txtbiltyexpense.Name = "txtbiltyexpense";
		this.txtbiltyexpense.Size = new System.Drawing.Size(114, 23);
		this.txtbiltyexpense.TabIndex = 9;
		this.txtbiltyexpense.TabStop = false;
		this.txtbiltyexpense.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtbiltyexpense.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtbiltyexpense_KeyPress);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(3, 76);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(66, 15);
		this.label6.TabIndex = 95;
		this.label6.Text = "Manual No";
		this.txtfrieght.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfrieght.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfrieght.Location = new System.Drawing.Point(662, 100);
		this.txtfrieght.Name = "txtfrieght";
		this.txtfrieght.ShortcutsEnabled = false;
		this.txtfrieght.Size = new System.Drawing.Size(113, 23);
		this.txtfrieght.TabIndex = 8;
		this.txtfrieght.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfrieght.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfrieght_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(592, 76);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 15);
		this.label2.TabIndex = 99;
		this.label2.Text = "Bilty Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(592, 104);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(70, 15);
		this.label3.TabIndex = 105;
		this.label3.Text = "Bilty Frieght";
		this.txtvehicleweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleweight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehicleweight.Location = new System.Drawing.Point(867, 100);
		this.txtvehicleweight.Name = "txtvehicleweight";
		this.txtvehicleweight.ShortcutsEnabled = false;
		this.txtvehicleweight.Size = new System.Drawing.Size(114, 23);
		this.txtvehicleweight.TabIndex = 11;
		this.txtvehicleweight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtvehicleweight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtvehicleweight_KeyPress);
		this.label33.BackColor = System.Drawing.Color.LightGray;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(287, 18);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(299, 23);
		this.label33.TabIndex = 20;
		this.label33.Text = "Remarks";
		this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtbiltydate.CustomFormat = "dd-MMM-yy";
		this.txtbiltydate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbiltydate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtbiltydate.Location = new System.Drawing.Point(662, 72);
		this.txtbiltydate.Name = "txtbiltydate";
		this.txtbiltydate.Size = new System.Drawing.Size(113, 23);
		this.txtbiltydate.TabIndex = 7;
		this.txtmanualno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtmanualno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtmanualno.Location = new System.Drawing.Point(69, 72);
		this.txtmanualno.Name = "txtmanualno";
		this.txtmanualno.Size = new System.Drawing.Size(214, 23);
		this.txtmanualno.TabIndex = 2;
		this.lbl.AutoSize = true;
		this.lbl.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl.Location = new System.Drawing.Point(592, 50);
		this.lbl.Name = "lbl";
		this.lbl.Size = new System.Drawing.Size(49, 15);
		this.lbl.TabIndex = 74;
		this.lbl.Text = "Bilty No";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(3, 50);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(57, 15);
		this.label17.TabIndex = 96;
		this.label17.Text = "Doc Date";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(3, 22);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 96;
		this.label5.Text = "Doc No";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(69, 46);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(214, 23);
		this.txtdocdate.TabIndex = 1;
		this.txtBiltyNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyNo.Location = new System.Drawing.Point(662, 46);
		this.txtBiltyNo.Name = "txtBiltyNo";
		this.txtBiltyNo.Size = new System.Drawing.Size(113, 23);
		this.txtBiltyNo.TabIndex = 6;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(69, 18);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(214, 23);
		this.txtdocno.TabIndex = 0;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(780, 76);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(64, 15);
		this.label48.TabIndex = 72;
		this.label48.Text = "Vehicle No";
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehicleno.Location = new System.Drawing.Point(867, 72);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(114, 23);
		this.txtvehicleno.TabIndex = 10;
		this.groupBox4.Controls.Add(this.label34);
		this.groupBox4.Controls.Add(this.txtcustremarks);
		this.groupBox4.Controls.Add(this.label35);
		this.groupBox4.Controls.Add(this.txtcustnetwt);
		this.groupBox4.Controls.Add(this.label9);
		this.groupBox4.Controls.Add(this.txtcustadlswt);
		this.groupBox4.Controls.Add(this.label37);
		this.groupBox4.Controls.Add(this.txtcustpckmtrlwt);
		this.groupBox4.Controls.Add(this.label38);
		this.groupBox4.Controls.Add(this.txtcustgrosswt);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcustomer);
		this.groupBox4.Controls.Add(this.label39);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox4.Location = new System.Drawing.Point(664, 21);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(320, 132);
		this.groupBox4.TabIndex = 2;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Customer Information";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(4, 107);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(52, 15);
		this.label34.TabIndex = 121;
		this.label34.Text = "Remarks";
		this.txtcustremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustremarks.Location = new System.Drawing.Point(62, 103);
		this.txtcustremarks.Name = "txtcustremarks";
		this.txtcustremarks.Size = new System.Drawing.Size(254, 23);
		this.txtcustremarks.TabIndex = 16;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(168, 80);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(45, 15);
		this.label35.TabIndex = 119;
		this.label35.Text = "Net Wt";
		this.txtcustnetwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustnetwt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustnetwt.Location = new System.Drawing.Point(214, 76);
		this.txtcustnetwt.Name = "txtcustnetwt";
		this.txtcustnetwt.ShortcutsEnabled = false;
		this.txtcustnetwt.Size = new System.Drawing.Size(102, 23);
		this.txtcustnetwt.TabIndex = 15;
		this.txtcustnetwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcustnetwt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcustnetwt_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(4, 80);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(52, 15);
		this.label9.TabIndex = 117;
		this.label9.Text = "AdLs Wt";
		this.txtcustadlswt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustadlswt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustadlswt.Location = new System.Drawing.Point(62, 76);
		this.txtcustadlswt.Name = "txtcustadlswt";
		this.txtcustadlswt.ShortcutsEnabled = false;
		this.txtcustadlswt.Size = new System.Drawing.Size(103, 23);
		this.txtcustadlswt.TabIndex = 14;
		this.txtcustadlswt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcustadlswt.TextChanged += new System.EventHandler(txtcustadlswt_TextChanged);
		this.txtcustadlswt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcustadlswt_KeyPress);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(168, 52);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(45, 15);
		this.label37.TabIndex = 115;
		this.label37.Text = "P.M Wt";
		this.txtcustpckmtrlwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustpckmtrlwt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustpckmtrlwt.Location = new System.Drawing.Point(214, 48);
		this.txtcustpckmtrlwt.Name = "txtcustpckmtrlwt";
		this.txtcustpckmtrlwt.ShortcutsEnabled = false;
		this.txtcustpckmtrlwt.Size = new System.Drawing.Size(102, 23);
		this.txtcustpckmtrlwt.TabIndex = 13;
		this.txtcustpckmtrlwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcustpckmtrlwt.TextChanged += new System.EventHandler(txtcustpckmtrlwt_TextChanged);
		this.txtcustpckmtrlwt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcustpckmtrlwt_KeyPress);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(4, 52);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(55, 15);
		this.label38.TabIndex = 113;
		this.label38.Text = "Gross Wt";
		this.txtcustgrosswt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustgrosswt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustgrosswt.Location = new System.Drawing.Point(62, 48);
		this.txtcustgrosswt.Name = "txtcustgrosswt";
		this.txtcustgrosswt.ShortcutsEnabled = false;
		this.txtcustgrosswt.Size = new System.Drawing.Size(103, 23);
		this.txtcustgrosswt.TabIndex = 12;
		this.txtcustgrosswt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcustgrosswt.TextChanged += new System.EventHandler(txtcustgrosswt_TextChanged);
		this.txtcustgrosswt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcustgrosswt_KeyPress);
		this.cmbcustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbcustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Location = new System.Drawing.Point(62, 17);
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Name = "cmbcustomer";
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.cmbcustomer).TabIndex = 11;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(4, 23);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(58, 15);
		this.label39.TabIndex = 111;
		this.label39.Text = "Customer";
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add(this.txtsupremarks);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.txtSupNetWeight);
		this.groupBox3.Controls.Add(this.label15);
		this.groupBox3.Controls.Add(this.txtSupAdLsWt);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add(this.txtSupPckMtrlWeight);
		this.groupBox3.Controls.Add(this.label13);
		this.groupBox3.Controls.Add(this.txtsupgrosswt);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupplier);
		this.groupBox3.Controls.Add(this.label12);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(346, 21);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(318, 132);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Supplier Information";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(3, 107);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(52, 15);
		this.label18.TabIndex = 121;
		this.label18.Text = "Remarks";
		this.txtsupremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsupremarks.Location = new System.Drawing.Point(58, 103);
		this.txtsupremarks.Name = "txtsupremarks";
		this.txtsupremarks.Size = new System.Drawing.Size(255, 23);
		this.txtsupremarks.TabIndex = 10;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(165, 80);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(45, 15);
		this.label16.TabIndex = 119;
		this.label16.Text = "Net Wt";
		this.txtSupNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupNetWeight.Location = new System.Drawing.Point(210, 76);
		this.txtSupNetWeight.Name = "txtSupNetWeight";
		this.txtSupNetWeight.ShortcutsEnabled = false;
		this.txtSupNetWeight.Size = new System.Drawing.Size(103, 23);
		this.txtSupNetWeight.TabIndex = 9;
		this.txtSupNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupNetWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupNetWeight_KeyPress);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(3, 80);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(52, 15);
		this.label15.TabIndex = 117;
		this.label15.Text = "AdLs Wt";
		this.txtSupAdLsWt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupAdLsWt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupAdLsWt.Location = new System.Drawing.Point(58, 76);
		this.txtSupAdLsWt.Name = "txtSupAdLsWt";
		this.txtSupAdLsWt.ShortcutsEnabled = false;
		this.txtSupAdLsWt.Size = new System.Drawing.Size(103, 23);
		this.txtSupAdLsWt.TabIndex = 8;
		this.txtSupAdLsWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupAdLsWt.TextChanged += new System.EventHandler(txtSupAdLsWt_TextChanged);
		this.txtSupAdLsWt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupAdLsWt_KeyPress);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(165, 52);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(45, 15);
		this.label14.TabIndex = 115;
		this.label14.Text = "P.M Wt";
		this.txtSupPckMtrlWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupPckMtrlWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupPckMtrlWeight.Location = new System.Drawing.Point(210, 48);
		this.txtSupPckMtrlWeight.Name = "txtSupPckMtrlWeight";
		this.txtSupPckMtrlWeight.ShortcutsEnabled = false;
		this.txtSupPckMtrlWeight.Size = new System.Drawing.Size(103, 23);
		this.txtSupPckMtrlWeight.TabIndex = 7;
		this.txtSupPckMtrlWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupPckMtrlWeight.TextChanged += new System.EventHandler(txtSupPckMtrlWeight_TextChanged);
		this.txtSupPckMtrlWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupPckMtrlWeight_KeyPress);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(3, 52);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(55, 15);
		this.label13.TabIndex = 113;
		this.label13.Text = "Gross Wt";
		this.txtsupgrosswt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupgrosswt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsupgrosswt.Location = new System.Drawing.Point(58, 48);
		this.txtsupgrosswt.Name = "txtsupgrosswt";
		this.txtsupgrosswt.ShortcutsEnabled = false;
		this.txtsupgrosswt.Size = new System.Drawing.Size(103, 23);
		this.txtsupgrosswt.TabIndex = 6;
		this.txtsupgrosswt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtsupgrosswt.TextChanged += new System.EventHandler(txtsupgrosswt_TextChanged);
		this.txtsupgrosswt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtsupgrosswt_KeyPress);
		this.cmbsupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance83).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Location = new System.Drawing.Point(58, 17);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Name = "cmbsupplier";
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Size = new System.Drawing.Size(255, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).TabIndex = 5;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(3, 23);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(51, 15);
		this.label12.TabIndex = 111;
		this.label12.Text = "Supplier";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(169, 103);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(51, 15);
		this.label10.TabIndex = 109;
		this.label10.Text = "ItemQty";
		this.txtItemQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQty.Location = new System.Drawing.Point(246, 99);
		this.txtItemQty.Name = "txtItemQty";
		this.txtItemQty.ShortcutsEnabled = false;
		this.txtItemQty.Size = new System.Drawing.Size(91, 23);
		this.txtItemQty.TabIndex = 4;
		this.txtItemQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemQty.TextChanged += new System.EventHandler(txtItemQty_TextChanged);
		this.txtItemQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemQty_KeyPress);
		this.cmbuom.AllowNull = (DefaultableBoolean)1;
		this.cmbuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance46;
		((SpecialBoxBase)((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance65).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbuom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbuom).Location = new System.Drawing.Point(62, 97);
		((System.Windows.Forms.Control)(object)this.cmbuom).Name = "cmbuom";
		((System.Windows.Forms.Control)(object)this.cmbuom).Size = new System.Drawing.Size(107, 26);
		((System.Windows.Forms.Control)(object)this.cmbuom).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbuom).TextChanged += new System.EventHandler(cmbuom_TextChanged);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(5, 103);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(35, 15);
		this.label11.TabIndex = 107;
		this.label11.Text = "UOM";
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance22;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance28).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(62, 40);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(275, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbitem).Leave += new System.EventHandler(cmbitem_Leave);
		this.labItemName.AutoSize = true;
		this.labItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labItemName.Location = new System.Drawing.Point(5, 46);
		this.labItemName.Name = "labItemName";
		this.labItemName.Size = new System.Drawing.Size(32, 15);
		this.labItemName.TabIndex = 105;
		this.labItemName.Text = "Item";
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1009, 661);
		this.tabControl1.TabIndex = 30;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1001, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 410);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(995, 219);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 32;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label46);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 385);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(995, 25);
		this.panel5.TabIndex = 31;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(951, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 25);
		this.ctrlGrdBar1.TabIndex = 45;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label46.AutoSize = true;
		this.label46.BackColor = System.Drawing.Color.Teal;
		this.label46.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.White;
		this.label46.Location = new System.Drawing.Point(8, 3);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(44, 16);
		this.label46.TabIndex = 0;
		this.label46.Text = "Detail";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.groupBox2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 193);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(995, 192);
		this.panel2.TabIndex = 30;
		this.groupBox2.Controls.Add(this.btnAddDetail);
		this.groupBox2.Controls.Add(this.btnCancelDetail);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.groupBox4);
		this.groupBox2.Controls.Add(this.groupBox5);
		this.groupBox2.Controls.Add(this.groupBox3);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(988, 186);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail Infomation";
		this.btnAddDetail.BackColor = System.Drawing.Color.Teal;
		this.btnAddDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddDetail.ForeColor = System.Drawing.Color.White;
		this.btnAddDetail.Location = new System.Drawing.Point(805, 154);
		this.btnAddDetail.Name = "btnAddDetail";
		this.btnAddDetail.Size = new System.Drawing.Size(33, 27);
		this.btnAddDetail.TabIndex = 17;
		this.btnAddDetail.Text = "+";
		this.btnAddDetail.UseVisualStyleBackColor = false;
		this.btnAddDetail.Click += new System.EventHandler(btnAddDetail_Click);
		this.btnAddDetail.Leave += new System.EventHandler(btnAddDetail_Leave);
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(914, 154);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(67, 27);
		this.btnCancelDetail.TabIndex = 19;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnCancelDetail.Leave += new System.EventHandler(btnAddDetail_Leave);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(841, 154);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(71, 27);
		this.btnUpdateDetail.TabIndex = 18;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.groupBox5.Controls.Add(this.rdSearchByCode);
		this.groupBox5.Controls.Add(this.rdSearchByName);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.groupBox5.Controls.Add(this.label10);
		this.groupBox5.Controls.Add(this.txtItemQty);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbuom);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcropYear);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox5.Controls.Add(this.label11);
		this.groupBox5.Controls.Add(this.labItemName);
		this.groupBox5.Controls.Add(this.label7);
		this.groupBox5.Controls.Add(this.label8);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox5.Location = new System.Drawing.Point(5, 21);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(341, 132);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Item Information";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(129, 15);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 111;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(66, 15);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 110;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.cmbcropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbcropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance12;
		((SpecialBoxBase)((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Location = new System.Drawing.Point(62, 69);
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Name = "cmbcropYear";
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Size = new System.Drawing.Size(107, 26);
		((System.Windows.Forms.Control)(object)this.cmbcropYear).TabIndex = 1;
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance9;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(246, 69);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(91, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 2;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 75);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(57, 15);
		this.label7.TabIndex = 107;
		this.label7.Text = "Crop Year";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(169, 75);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(77, 15);
		this.label8.TabIndex = 107;
		this.label8.Text = "Packing Type";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 55);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(995, 138);
		this.panel4.TabIndex = 30;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1001, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistoryDetail).BackColor = System.Drawing.Color.White;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(3, 459);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(995, 170);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 15;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label19);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 435);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(995, 24);
		this.panel3.TabIndex = 14;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(5, 5);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(232, 17);
		this.label19.TabIndex = 40;
		this.label19.Text = "Detail of the Selected Above row";
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistory).BackColor = System.Drawing.Color.White;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 57);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(995, 378);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 13;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Controls.Add(this.label20);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(995, 54);
		this.panel6.TabIndex = 12;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(952, 28);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(7, 4, 7, 4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 25);
		this.ctrlGrdBar2.TabIndex = 44;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnLoaddAllHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(995, 27);
		this.toolStrip2.TabIndex = 7;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoaddAllHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoaddAllHistory.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoaddAllHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoaddAllHistory.Name = "btnLoaddAllHistory";
		this.btnLoaddAllHistory.Size = new System.Drawing.Size(82, 24);
		this.btnLoaddAllHistory.Text = "&Load All";
		this.btnLoaddAllHistory.Click += new System.EventHandler(btnLoaddAllHistory_Click);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label20.ForeColor = System.Drawing.Color.White;
		this.label20.Location = new System.Drawing.Point(2, 31);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(137, 19);
		this.label20.TabIndex = 40;
		this.label20.Text = "Commission Bill";
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.Location = new System.Drawing.Point(611, 3);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1273, 25);
		this.miniToolStrip.TabIndex = 27;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1009, 661);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "CommTradeLoadingDelivery";
		this.Text = "CommTradeLoadingDelivery";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(CommTradeLoadingDelivery_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(CommTradeLoadingDelivery_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTransporter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBroker).EndInit();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcustomer).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		this.panel4.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
