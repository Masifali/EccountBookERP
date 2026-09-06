using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Mfg;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Mfg;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Mfg;

public class frmBillOfMaterialCasting : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtInput = new DataTable();

	private DataTable dtOutPutByProduct = new DataTable();

	private DataTable dtOverHead = new DataTable();

	private DataTable lstitem = new DataTable();

	private DataTable dtUom = new DataTable();

	private DataTable dtRateUom = new DataTable();

	private DataTable dtuomGetAll = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtAccounts = new DataTable();

	private bool UpdateMode;

	private int RecId;

	private int ProcessRecId;

	private bool ProcessUpdateMode;

	private List<BomInputDetail> lstRemoveRecordInput = new List<BomInputDetail>();

	private List<BomByProductDetail> lstRemoveRecordOutPut = new List<BomByProductDetail>();

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private IContainer components;

	private Panel panel1;

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

	private CheckBox chkIsDefault;

	private TextBox txtQty;

	private Label label18;

	internal UltraCombo cmbUOM;

	private Label label44;

	private TextBox txtNetValue;

	private Label label10;

	private TextBox txtOverHead;

	private Label label9;

	private TextBox txtLessByProduct;

	private Label label8;

	private TextBox txtTotalInputs;

	private Label label6;

	private TextBox txtBom;

	private Label label11;

	private Label label12;

	private TabPage tabPage3;

	private Label label13;

	private DateTimePicker txtEffectedDate;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnRegister;

	private Panel panel4;

	private Panel panel20;

	private Label label5;

	private Label label25;

	private Label label24;

	private Label label3;

	private GroupBox groupBox2;

	private Label label2;

	private Label label21;

	private Panel panel6;

	internal UltraCombo cmbBOMType;

	private Label label37;

	private DateTimePicker txtdocdate;

	private TextBox txtdocno;

	private Panel panel10;

	internal GridEX grdInputMaterial;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnDelete;

	private ToolStripButton btnshortcutkeys;

	private Panel panel38;

	private Label label32;

	private Panel panel45;

	private Panel panel8;

	internal GridEX grdHistoryOverHead;

	private Panel panel42;

	private Label label34;

	private Panel panel19;

	internal GridEX grdHistoryInput;

	private Panel panel44;

	private Label label16;

	internal GridEX DatagridHistory;

	private Panel panel15;

	private Label label17;

	private Panel panel13;

	private GroupBox groupBox1;

	private Label label19;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private Label label28;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label35;

	private TextBox txtFromNoHistory;

	private Label label30;

	internal UltraCombo cmbbomtypeHistory;

	internal UltraCombo CmbBomNoHistory;

	private Label label20;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar1;

	private CheckBox ChkPreview;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator2;

	private TextBox txtRemarksHeader;

	internal GridEX grdOutPutByProduct;

	private Panel panel11;

	private Label label23;

	private Panel panel7;

	private Label label1;

	private Panel panel17;

	private TextBox textBox1;

	private Label label31;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar3;

	internal GridEX grdOverHead;

	private Panel panel14;

	private Label label22;

	internal UltraCombo CmbCastingType;

	private Label label77;

	internal GridEX grdHistoryOutput;

	private Panel panel12;

	private Label label15;

	private Label label4;

	public frmBillOfMaterialCasting(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmBillOfMaterialCasting_Manufacturing_Load(object sender, EventArgs e)
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
			btnDelete.Visible = formright.DoHaveCanDelete;
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
			dtAccounts.Columns.Add("Id");
			dtAccounts.Columns.Add("AccountTitle");
			dtUom.Columns.Add("Id", typeof(int));
			dtUom.Columns.Add("ItemUOM", typeof(double));
			dtUom.Columns.Add("ItemUOMCode", typeof(string));
			dtRateUom.Columns.Add("RateId", typeof(int));
			dtRateUom.Columns.Add("RateUOM", typeof(double));
			dtRateUom.Columns.Add("RateUOMCode", typeof(string));
			BindCastingType();
			FillUOM();
			uomgetAll();
			AccountTitleFill();
			Datetypefill();
			AllCombobind();
			DocumentNoFill();
			TypeBind();
			ItemFill();
			dtInput.Columns.Add("Id", typeof(int));
			dtInput.Columns.Add("ItemId", typeof(int));
			dtInput.Columns.Add("ItemUomId", typeof(int));
			dtInput.Columns.Add("Uom", typeof(decimal));
			dtInput.Columns.Add("UomCode", typeof(string));
			dtInput.Columns.Add("ItemQty", typeof(decimal));
			dtInput.Columns.Add("NetWeight", typeof(decimal));
			dtInput.Columns.Add("Rate", typeof(decimal));
			dtInput.Columns.Add("RateUomId", typeof(int));
			dtInput.Columns.Add("RateUom", typeof(decimal));
			dtInput.Columns.Add("RateUomCode", typeof(string));
			dtInput.Columns.Add("Amount", typeof(decimal));
			AddRowInInputGrid();
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
			dtOutPutByProduct.Columns.Add("Id", typeof(int));
			dtOutPutByProduct.Columns.Add("ItemId", typeof(int));
			dtOutPutByProduct.Columns.Add("ItemUomId", typeof(int));
			dtOutPutByProduct.Columns.Add("Uom", typeof(decimal));
			dtOutPutByProduct.Columns.Add("UomCode", typeof(string));
			dtOutPutByProduct.Columns.Add("ItemQty", typeof(decimal));
			dtOutPutByProduct.Columns.Add("NetWeight", typeof(decimal));
			dtOutPutByProduct.Columns.Add("Rate", typeof(decimal));
			dtOutPutByProduct.Columns.Add("RateUomId", typeof(int));
			dtOutPutByProduct.Columns.Add("RateUom", typeof(decimal));
			dtOutPutByProduct.Columns.Add("RateUomCode", typeof(string));
			dtOutPutByProduct.Columns.Add("Amount", typeof(decimal));
			AddRowInOutPutByProductGrid();
			grdOutPutByProduct.RetrieveStructure();
			grdOutPutGridSetting();
			dtOverHead.Columns.Add("Id", typeof(int));
			dtOverHead.Columns.Add("OverHeadTypeId", typeof(int));
			dtOverHead.Columns.Add("OverHeadAccountId", typeof(int));
			dtOverHead.Columns.Add("PercentOfTotalInput", typeof(decimal));
			dtOverHead.Columns.Add("Qty", typeof(decimal));
			dtOverHead.Columns.Add("Rate", typeof(decimal));
			dtOverHead.Columns.Add("Amount", typeof(decimal));
			dtOverHead.Columns.Add("OhRemarks", typeof(string));
			AddRowInOverHeadGrid();
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSetting();
			txtBom.Focus();
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
			int code = BomHeader.GenerateCode(new BomHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1664,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			txtdocno.Text = code.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCastingType()
	{
		try
		{
			int DetailId = Conversion.ToInt(CmbCastingType.Value);
			DataTable dt = LookUps.GetDataByTypeId(1);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCastingType, "Id", "LookupName", "Casting Type", true);
				((UltraGridBase)CmbCastingType).DisplayLayout.Bands[0].Columns[1].Width = 350;
				if (DetailId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DetailId))
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

	private void AccountTitleFill()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationGetAllServiceBind();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			dtAccounts.Rows.Clear();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 3 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 6 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 8 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 11 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 12 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 13 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 14 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 20 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 21)
				{
					dtAccounts.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["AccountTitle"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindUom(int ItemId)
	{
		try
		{
			dtUom.Clear();
			for (int i = 0; i < dtuomGetAll.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtuomGetAll.Rows[i]["ItemId"]) == Conversion.ToInt((object)ItemId))
				{
					dtUom.Rows.Add(dtuomGetAll.Rows[i]["Id"], dtuomGetAll.Rows[i]["Equivalent"], dtuomGetAll.Rows[i]["UOMCode"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillUOM()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(cmbUOM.Value);
			DataTable dt = UOM.Getall(new UOM
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtUom = new DataTable();
			dtUom.Columns.Add("Id", typeof(int));
			dtUom.Columns.Add("UOMCode", typeof(string));
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dtUom.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["UOMCode"]);
			}
			if (dtUom.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtUom, cmbUOM, "Id", "UOMCode", "UOM", ZeroIndex: true);
			if (Id > 0)
			{
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbUOM.Value = Id;
				}
				else
				{
					((Control)(object)cmbUOM).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void uomgetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule obj = new UOMSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtuomGetAll = UOMSchedule.Getall(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUom(int ItemId)
	{
		try
		{
			dtRateUom.Clear();
			if (dtuomGetAll.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtuomGetAll.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtuomGetAll.Rows[i]["ItemId"]) == Conversion.ToInt((object)ItemId))
				{
					dtRateUom.Rows.Add(dtuomGetAll.Rows[i]["Id"], dtuomGetAll.Rows[i]["Equivalent"], dtuomGetAll.Rows[i]["UOMCode"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TypeBind()
	{
		DataTable DtType = new DataTable();
		DtType.Columns.Add("Id");
		DtType.Columns.Add("Type");
		DtType.Rows.Add(1, "Semi-Finish Goods");
		DtType.Rows.Add(2, "Finish Goods");
		if (DtType.Rows.Count > 0)
		{
			DDL.BindDDLNew(DtType, cmbBOMType, "Id", "Type", "Type Title", false);
			((UltraGridBase)cmbBOMType).Rows[0].Activate();
		}
	}

	private void ItemFill()
	{
		try
		{
			lstitem.Clear();
			lstitem = CommonServices.ItemGetForComboServiceBindForEngr();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text == "0")
		{
			MessageBox.Show("doc No Field Required");
			txtdocno.Focus();
			return false;
		}
		if (txtBom.Text.Trim() == string.Empty || txtBom.Text == "0")
		{
			MessageBox.Show("BOM Field Required");
			txtBom.Focus();
			return false;
		}
		if (((Control)(object)cmbBOMType).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Type Field Required");
			cmbBOMType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCastingType).ActiveRow == null || Conversion.ToInt(CmbCastingType.Value) == 0)
		{
			MessageBox.Show("CastingType Field Required");
			CmbCastingType.Focus();
			return false;
		}
		if (((Control)(object)cmbUOM).Text.Trim() == string.Empty || Conversion.ToInt(cmbUOM.Value) == 0)
		{
			MessageBox.Show("IteUom Field Required");
			cmbUOM.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtQty.Text) == 0.0)
		{
			MessageBox.Show("Qty Field Required");
			txtQty.Focus();
			return false;
		}
		return true;
	}

	private void AddRowInInputGrid()
	{
		dtInput.Rows.Add(0, 0, 0, 0, "", 0, 0, 0, 0, 0, "", 0);
		grdInputMaterial.DataSource = dtInput;
	}

	private void grdInputComboFill()
	{
		try
		{
			if (lstitem != null)
			{
				grdInputMaterial.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
				grdInputMaterial.RootTable.Columns["ItemId"].HasValueList = true;
				grdInputMaterial.RootTable.Columns["ItemId"].LimitToList = true;
				grdInputMaterial.RootTable.Columns["ItemId"].EditType = (EditType)4;
				grdInputMaterial.RootTable.Columns["ItemId"].Caption = "Item Name";
				grdInputMaterial.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)lstitem.DefaultView, "Id", "ItemName");
				grdInputMaterial.RootTable.Columns["ItemId"].Width = 200;
			}
			grdInputMaterial.RootTable.Columns.Add("Delete");
			grdInputMaterial.RootTable.Columns["Delete"].Key = "Delete";
			grdInputMaterial.RootTable.Columns["Delete"].Caption = "X";
			grdInputMaterial.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInputMaterial.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInputMaterial.RootTable.Columns["Delete"].ButtonText = "X";
			grdInputMaterial.RootTable.Columns["Delete"].Width = 25;
			grdInputMaterial.RootTable.Columns["Delete"].Position = 0;
			grdInputMaterial.RootTable.Columns.Add("Add");
			grdInputMaterial.RootTable.Columns["Add"].Key = "Add";
			grdInputMaterial.RootTable.Columns["Add"].Caption = "+";
			grdInputMaterial.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInputMaterial.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdInputMaterial.RootTable.Columns["Add"].ButtonText = "+";
			grdInputMaterial.RootTable.Columns["Add"].Width = 25;
			grdInputMaterial.RootTable.Columns["Add"].Position = 1;
			grdInputMaterial.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputSettings()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInputComboFill();
			grdInputMaterial.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdInputMaterial.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdInputMaterial.RootTable.Columns["ItemId"].Width = 200;
			grdInputMaterial.RootTable.Columns["Uom"].Width = 50;
			grdInputMaterial.RootTable.Columns["UomCode"].Width = 60;
			grdInputMaterial.RootTable.Columns["ItemQty"].Width = 70;
			grdInputMaterial.RootTable.Columns["NetWeight"].Width = 75;
			grdInputMaterial.RootTable.Columns["Rate"].Width = 65;
			grdInputMaterial.RootTable.Columns["RateUom"].Width = 50;
			grdInputMaterial.RootTable.Columns["RateUomCode"].Width = 60;
			grdInputMaterial.RootTable.Columns["Amount"].Width = 90;
			grdInputMaterial.RootTable.Columns["Id"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUomId"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUom"].Visible = false;
			grdInputMaterial.RootTable.Columns["ItemUomId"].Visible = false;
			grdInputMaterial.RootTable.Columns["NetWeight"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUomCode"].Visible = false;
			grdInputMaterial.RootTable.Columns["Uom"].Visible = false;
			grdInputMaterial.RootTable.Columns["UomCode"].Caption = "Uom";
			grdInputMaterial.RootTable.Columns["RateUomCode"].Caption = "Rate Uom";
			grdInputMaterial.RootTable.Columns["Id"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["RateUomCode"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["ItemUomId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["Uom"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["UomCode"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["ItemQty"].EditType = (EditType)1;
			grdInputMaterial.RootTable.Columns["Rate"].EditType = (EditType)1;
			grdInputMaterial.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["ItemQty"].FormatString = "##,#.##";
			grdInputMaterial.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.##";
			grdInputMaterial.RootTable.Columns["Uom"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["Uom"].FormatString = "##,#.##";
			grdInputMaterial.RootTable.Columns["Uom"].TotalFormatString = "##,#.##";
			grdInputMaterial.RootTable.Columns["RateUom"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["RateUom"].FormatString = "##,#.##";
			grdInputMaterial.RootTable.Columns["RateUom"].TotalFormatString = "##,#.##";
			grdInputMaterial.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInputMaterial.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdInputMaterial.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["NetWeight"].FormatString = "##,#.##";
			grdInputMaterial.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.##";
			grdInputMaterial.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInputMaterial.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInputMaterial.UpdateData();
			GridEXRow item = grdInputMaterial.CurrentRow;
			if (e.Column.Key == "ItemId")
			{
				item.Cells["ItemUomId"].Value = 0;
				item.Cells["Uom"].Value = 0;
				item.Cells["UomCode"].Value = "";
				item.Cells["RateUom"].Value = 0;
				item.Cells["RateUomId"].Value = 0;
				item.Cells["RateUomCode"].Value = "";
				item.Cells["NetWeight"].Value = 0;
				item.Cells["Amount"].Value = 0;
			}
			if ((e.Column.Key == "ItemQty" || e.Column.Key == "Uom" || e.Column.Key == "ItemUomId") && item != null && (int)item.RowType == 0)
			{
				if (item.Cells["ItemQty"].Value.ToString() != string.Empty && item.Cells["ItemUomId"].Value.ToString() != string.Empty)
				{
					decimal num = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
					decimal quom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
					decimal Weight2 = num * quom;
					item.Cells["NetWeight"].Value = Weight2;
				}
				else
				{
					item.Cells["NetWeight"].Value = 0;
				}
			}
			if ((e.Column.Key == "ItemQty" || e.Column.Key == "Rate" || e.Column.Key == "RateUomId" || e.Column.Key == "RateUom") && item != null && (int)item.RowType == 0)
			{
				decimal Weight = default(decimal);
				decimal Iuom = default(decimal);
				if (!(item.Cells["ItemQty"].Value.ToString() != string.Empty) || !(item.Cells["ItemUomId"].Value.ToString() != string.Empty))
				{
					throw new Exception("ItemQty And ItemUom Should be greater than zero...");
				}
				decimal num2 = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
				Weight = num2 * Iuom;
				_ = Weight != 0m;
				if (Conversion.ToDecimal(item.Cells["Rate"].Value) != 0m && Conversion.ToInt(item.Cells["RateUomId"].Value) != 0)
				{
					decimal num3 = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
					Iuom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
					decimal rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
					decimal Ruom = Conversion.ToDecimal((object)item.Cells["RateUom"].Value.ToString());
					decimal Amount = num3 * Iuom / Ruom * rate;
					item.Cells["Amount"].Value = Amount;
				}
				else
				{
					item.Cells["Amount"].Value = 0;
				}
			}
			grdInputMaterial.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InputGridUombind(int ItemId, GridEX grd, int Index)
	{
	}

	private void grdInputMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grdInputMaterial.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					BomInputDetail d1 = new BomInputDetail();
					d1.BomHeaderId = Conversion.ToInt(r.Cells["Id"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.WeightKg = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					d1.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					d1.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					d1.ActionTypeId = 3;
					lstRemoveRecordInput.Add(d1);
					dtInput.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdInputMaterial.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtInput.Rows.Count == 0)
				{
					AddRowInInputGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInInputGrid();
			}
			grdInputMaterial.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInOutPutByProductGrid()
	{
		dtOutPutByProduct.Rows.Add(0, 0, 0, 0, "", 0, 0, 0, 0, 0, "", 0);
		grdOutPutByProduct.DataSource = dtOutPutByProduct;
	}

	private void grdOutPutByProductComboFill()
	{
		try
		{
			if (lstitem != null)
			{
				grdOutPutByProduct.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
				grdOutPutByProduct.RootTable.Columns["ItemId"].LimitToList = true;
				grdOutPutByProduct.RootTable.Columns["ItemId"].HasValueList = true;
				grdOutPutByProduct.RootTable.Columns["ItemId"].EditType = (EditType)4;
				grdOutPutByProduct.RootTable.Columns["ItemId"].Caption = "Item Name";
				grdOutPutByProduct.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)lstitem.DefaultView, "Id", "ItemName");
				grdOutPutByProduct.RootTable.Columns["ItemId"].Width = 200;
			}
			grdOutPutByProduct.RootTable.Columns.Add("Delete");
			grdOutPutByProduct.RootTable.Columns["Delete"].Key = "Delete";
			grdOutPutByProduct.RootTable.Columns["Delete"].Caption = "X";
			grdOutPutByProduct.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOutPutByProduct.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOutPutByProduct.RootTable.Columns["Delete"].ButtonText = "X";
			grdOutPutByProduct.RootTable.Columns["Delete"].Width = 25;
			grdOutPutByProduct.RootTable.Columns["Delete"].Position = 0;
			grdOutPutByProduct.RootTable.Columns.Add("Add");
			grdOutPutByProduct.RootTable.Columns["Add"].Key = "Add";
			grdOutPutByProduct.RootTable.Columns["Add"].Caption = "+";
			grdOutPutByProduct.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOutPutByProduct.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOutPutByProduct.RootTable.Columns["Add"].ButtonText = "+";
			grdOutPutByProduct.RootTable.Columns["Add"].Width = 25;
			grdOutPutByProduct.RootTable.Columns["Add"].Position = 1;
			grdOutPutByProduct.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutGridSetting()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOutPutByProductComboFill();
			grdOutPutByProduct.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdOutPutByProduct.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdOutPutByProduct.RootTable.Columns["ItemId"].Width = 200;
			grdOutPutByProduct.RootTable.Columns["Uom"].Width = 50;
			grdOutPutByProduct.RootTable.Columns["UomCode"].Width = 60;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].Width = 70;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].Width = 75;
			grdOutPutByProduct.RootTable.Columns["Rate"].Width = 65;
			grdOutPutByProduct.RootTable.Columns["RateUom"].Width = 50;
			grdOutPutByProduct.RootTable.Columns["RateUomCode"].Width = 60;
			grdOutPutByProduct.RootTable.Columns["Amount"].Width = 90;
			grdOutPutByProduct.RootTable.Columns["Id"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["RateUomId"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["ItemUomId"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["RateUom"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["RateUomCode"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["Uom"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["UomCode"].Caption = "Uom";
			grdOutPutByProduct.RootTable.Columns["RateUomCode"].Caption = "Rate Uom";
			grdOutPutByProduct.RootTable.Columns["Id"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["ItemUomId"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["RateUomCode"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["Uom"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["UomCode"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].EditType = (EditType)1;
			grdOutPutByProduct.RootTable.Columns["Rate"].EditType = (EditType)1;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].FormatString = "##,#.##";
			grdOutPutByProduct.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.##";
			grdOutPutByProduct.RootTable.Columns["Uom"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["Uom"].FormatString = "##,#.##";
			grdOutPutByProduct.RootTable.Columns["Uom"].TotalFormatString = "##,#.##";
			grdOutPutByProduct.RootTable.Columns["RateUom"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["RateUom"].FormatString = "##,#.##";
			grdOutPutByProduct.RootTable.Columns["RateUom"].TotalFormatString = "##,#.##";
			grdOutPutByProduct.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
			grdOutPutByProduct.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdOutPutByProduct.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].FormatString = "##,#.##";
			grdOutPutByProduct.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.##";
			grdOutPutByProduct.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOutPutByProduct.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutByProduct.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutByProduct_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOutPutByProduct.UpdateData();
			GridEXRow item = grdOutPutByProduct.CurrentRow;
			if (e.Column.Key == "ItemId")
			{
				item.Cells["ItemUomId"].Value = 0;
				item.Cells["Uom"].Value = 0;
				item.Cells["UomCode"].Value = "";
				item.Cells["RateUom"].Value = 0;
				item.Cells["RateUomId"].Value = 0;
				item.Cells["RateUomCode"].Value = "";
				item.Cells["NetWeight"].Value = 0;
				item.Cells["Amount"].Value = 0;
			}
			if ((e.Column.Key == "ItemQty" || e.Column.Key == "Uom" || e.Column.Key == "ItemUomId") && item != null && (int)item.RowType == 0)
			{
				if (item.Cells["ItemQty"].Value.ToString() != string.Empty && item.Cells["ItemUomId"].Value.ToString() != string.Empty)
				{
					decimal num = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
					decimal quom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
					decimal Weight2 = num * quom;
					item.Cells["NetWeight"].Value = Weight2;
				}
				else
				{
					item.Cells["NetWeight"].Value = 0;
				}
			}
			if ((e.Column.Key == "ItemQty" || e.Column.Key == "Rate" || e.Column.Key == "RateUomId" || e.Column.Key == "RateUom") && item != null && (int)item.RowType == 0)
			{
				decimal Weight = default(decimal);
				decimal Iuom = default(decimal);
				if (!(item.Cells["ItemQty"].Value.ToString() != string.Empty) || !(item.Cells["ItemUomId"].Value.ToString() != string.Empty))
				{
					throw new Exception("ItemQty And ItemUom Should be greater than zero...");
				}
				decimal num2 = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
				Weight = num2 * Iuom;
				_ = Weight != 0m;
				if (Conversion.ToDecimal(item.Cells["Rate"].Value) != 0m && Conversion.ToInt(item.Cells["RateUomId"].Value) != 0)
				{
					decimal num3 = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
					Iuom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
					decimal rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
					decimal Ruom = Conversion.ToDecimal((object)item.Cells["RateUom"].Value.ToString());
					decimal Amount = num3 * Iuom / Ruom * rate;
					item.Cells["Amount"].Value = Amount;
				}
				else
				{
					item.Cells["Amount"].Value = 0;
				}
			}
			grdOutPutByProduct.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutByProduct_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grdOutPutByProduct.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					BomByProductDetail d1 = new BomByProductDetail();
					d1.BomByProductDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.Weight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					d1.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					d1.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					d1.ActionTypeId = 3;
					lstRemoveRecordOutPut.Add(d1);
					dtOutPutByProduct.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdOutPutByProduct.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtOutPutByProduct.Rows.Count == 0)
				{
					AddRowInOutPutByProductGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInOutPutByProductGrid();
			}
			grdOutPutByProduct.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInOverHeadGrid()
	{
		dtOverHead.Rows.Add(0, 0, 0, 0, 0, 0, 0, "");
		grdOverHead.DataSource = dtOverHead;
	}

	private void grdOverHeadComboFill()
	{
		try
		{
			if (dtAccounts.Rows.Count > 0)
			{
				grdOverHead.RootTable.Columns["OverHeadAccountId"].ColumnType = (ColumnType)0;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].LimitToList = true;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].HasValueList = true;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].EditType = (EditType)4;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].Caption = "AccountTitle";
				grdOverHead.RootTable.Columns["OverHeadAccountId"].ValueList.PopulateValueList((IEnumerable)dtAccounts.DefaultView, "Id", "AccountTitle");
				grdOverHead.RootTable.Columns["OverHeadAccountId"].Width = 200;
			}
			grdOverHead.RootTable.Columns.Add("Delete");
			grdOverHead.RootTable.Columns["Delete"].Key = "Delete";
			grdOverHead.RootTable.Columns["Delete"].Caption = "X";
			grdOverHead.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Delete"].ButtonText = "X";
			grdOverHead.RootTable.Columns["Delete"].Width = 25;
			grdOverHead.RootTable.Columns["Delete"].Position = 0;
			grdOverHead.RootTable.Columns.Add("Add");
			grdOverHead.RootTable.Columns["Add"].Key = "Add";
			grdOverHead.RootTable.Columns["Add"].Caption = "+";
			grdOverHead.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Add"].ButtonText = "+";
			grdOverHead.RootTable.Columns["Add"].Width = 25;
			grdOverHead.RootTable.Columns["Add"].Position = 1;
			grdOverHead.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHeadGridSetting()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOverHeadComboFill();
			grdOverHead.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdOverHead.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdOverHead.RootTable.Columns["OverHeadAccountId"].Width = 200;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].Width = 90;
			grdOverHead.RootTable.Columns["Qty"].Width = 60;
			grdOverHead.RootTable.Columns["Rate"].Width = 65;
			grdOverHead.RootTable.Columns["Amount"].Width = 90;
			grdOverHead.RootTable.Columns["OhRemarks"].Width = 250;
			grdOverHead.RootTable.Columns["Id"].Visible = false;
			grdOverHead.RootTable.Columns["OverHeadTypeId"].Visible = false;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].Caption = "Percentage";
			grdOverHead.RootTable.Columns["Id"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["OverHeadTypeId"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["Qty"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["Rate"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["Amount"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].FormatString = "##,#.##";
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].TotalFormatString = "##,#.##";
			grdOverHead.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["Qty"].FormatString = "##,#.##";
			grdOverHead.RootTable.Columns["Qty"].TotalFormatString = "##,#.##";
			grdOverHead.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdOverHead.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdOverHead.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOverHead.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOverHead.UpdateData();
			GridEXRow item = grdOverHead.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0)
			{
				decimal Amount = default(decimal);
				decimal Iuom = default(decimal);
				if (!(item.Cells["Qty"].Value.ToString() != string.Empty) || !(item.Cells["Rate"].Value.ToString() != string.Empty))
				{
					throw new Exception("Qty And Rate Should be greater than zero...");
				}
				decimal num = Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)item.Cells["Rate"].Text.ToString());
				Amount = num * Iuom;
				item.Cells["Amount"].Value = Amount;
			}
			if (e.Column.Key == "PercentOfTotalInput" && item != null && (int)item.RowType == 0)
			{
				double TotalPercentage = 0.0;
				GridEXRow[] rows = grdOverHead.GetRows();
				foreach (GridEXRow r in rows)
				{
					TotalPercentage += Conversion.ToDouble(r.Cells["PercentOfTotalInput"].Value);
				}
				if (TotalPercentage > 100.0)
				{
					double Percent = Conversion.ToDouble(grdOverHead.CurrentRow.Cells["PercentOfTotalInput"].Value);
					double i = TotalPercentage - 100.0;
					grdOverHead.CurrentRow.Cells["PercentOfTotalInput"].Value = Percent - i;
					MessageBox.Show("TotalPercentage Can not be Greater than 100...");
					return;
				}
				if (item.Cells["PercentOfTotalInput"].Value.ToString() != string.Empty)
				{
					double num2 = Conversion.ToDouble((object)grdInputMaterial.GetTotal(grdInputMaterial.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
					double Percentage = Conversion.ToDouble((object)item.Cells["PercentOfTotalInput"].Value.ToString());
					double TotlaPercentage = num2 / 100.0 * Percentage;
					if (TotlaPercentage > 0.0)
					{
						item.Cells["Amount"].Value = Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					}
					else
					{
						item.Cells["Amount"].Value = 0;
					}
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
				}
			}
			if (e.Column.Key == "Amount")
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			grdOutPutByProduct.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdOverHead.CurrentRow;
				dtOverHead.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtOverHead.Rows.Count == 0)
				{
					AddRowInOverHeadGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInOverHeadGrid();
			}
			grdOverHead.UpdateData();
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

	public void FromReset()
	{
		try
		{
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			DocumentNoFill();
			txtBom.Text = string.Empty;
			txtEffectedDate.Value = DateTime.Now;
			((Control)(object)cmbBOMType).Text = string.Empty;
			((Control)(object)cmbUOM).Text = string.Empty;
			txtQty.Text = string.Empty;
			dtInput.Rows.Clear();
			grdInputMaterial.ClearStructure();
			dtOverHead.Rows.Clear();
			grdOverHead.ClearStructure();
			dtOutPutByProduct.Rows.Clear();
			grdOutPutByProduct.ClearStructure();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			AddRowInInputGrid();
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
			AddRowInOutPutByProductGrid();
			grdOutPutByProduct.RetrieveStructure();
			grdOutPutGridSetting();
			AddRowInOverHeadGrid();
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSetting();
			txtTotalInputs.Text = string.Empty;
			txtOverHead.Text = string.Empty;
			txtLessByProduct.Text = string.Empty;
			txtNetValue.Text = string.Empty;
			textBox1.Text = string.Empty;
			txtdocdate.Focus();
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
			TypeBind();
			BindCastingType();
			FillUOM();
			ItemFill();
			uomgetAll();
			AccountTitleFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Calculation()
	{
		try
		{
			string sumoftotalInputs = "";
			string sumoftotalOutputs = "";
			string sumofOverhead = "";
			if (grdInputMaterial.GetRows().Count() > 0)
			{
				sumoftotalInputs = grdInputMaterial.GetTotal(grdInputMaterial.RootTable.Columns["Amount"], (AggregateFunction)2).ToString();
			}
			if (grdOutPutByProduct.GetRows().Count() > 0)
			{
				sumoftotalOutputs = grdOutPutByProduct.GetTotal(grdOutPutByProduct.RootTable.Columns["Amount"], (AggregateFunction)2).ToString();
			}
			if (grdOverHead.GetRows().Count() > 0)
			{
				sumofOverhead = grdOverHead.GetTotal(grdOverHead.RootTable.Columns["Amount"], (AggregateFunction)2).ToString();
			}
			txtTotalInputs.Text = Math.Round(Conversion.ToDouble((object)sumoftotalInputs), 2).ToString();
			txtOverHead.Text = Math.Round(Conversion.ToDouble((object)sumofOverhead), 2).ToString();
			txtLessByProduct.Text = Math.Round(Conversion.ToDouble((object)sumoftotalOutputs), 2).ToString();
			double NetValue = Conversion.ToDouble((object)sumoftotalInputs) + Conversion.ToDouble((object)sumofOverhead) - Conversion.ToDouble((object)sumoftotalOutputs);
			txtNetValue.Text = Math.Round(NetValue, 2).ToString();
			textBox1.Text = Math.Round(NetValue / Conversion.ToDouble((object)txtQty.Text), 2).ToString();
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
			Calculation();
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
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Expected O, but got Unknown
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0841: Expected O, but got Unknown
		//IL_0da2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da9: Expected O, but got Unknown
		//IL_0f6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f71: Expected O, but got Unknown
		//IL_102c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1033: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grdInputMaterial.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found...\nGrid Input Should have atleast one record");
				return;
			}
			BomHeader bOMHeader = new BomHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				bOMHeader.BomHeaderId = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			bOMHeader.CompanyId = UserAccount.CompanyId;
			bOMHeader.BranchesId = UserAccount.BranchesId;
			bOMHeader.ProjectsId = UserAccount.BranchesId;
			bOMHeader.OrganizationId = UserAccount.OrganizationId;
			bOMHeader.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			bOMHeader.ItemId = 0;
			bOMHeader.ItemUomId = Conversion.ToInt(cmbUOM.Value);
			bOMHeader.CastingTypeId = Conversion.ToInt(CmbCastingType.Value);
			bOMHeader.BomName = txtBom.Text;
			bOMHeader.BomType = ((Control)(object)cmbBOMType).Text;
			bOMHeader.BomLevelType = ((Control)(object)cmbBOMType).Text;
			bOMHeader.EffectedDate = txtEffectedDate.Value;
			bOMHeader.BomBatchQty = Conversion.ToDecimal((object)txtQty.Text);
			bOMHeader.DocumentTypeId = 1664;
			bOMHeader.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
			bOMHeader.DocNo = Conversion.ToInt((object)txtdocno.Text);
			bOMHeader.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			bOMHeader.IsActive = true;
			bOMHeader.IsDefault = chkIsDefault.Checked;
			bOMHeader.EntryUserId = UserAccount.ID;
			bOMHeader.EntryDate = DateTime.Now;
			bOMHeader.ModifyDate = DateTime.Now;
			bOMHeader.ModifyUserId = UserAccount.ID;
			bOMHeader.ApprovedUserId = UserAccount.ID;
			bOMHeader.ApprovedDate = DateTime.Now;
			bOMHeader.IsApproved = false;
			bOMHeader.BomInputDetailList = new List<BomInputDetail>();
			bOMHeader.BomFohDetailList = new List<BomFohDetail>();
			bOMHeader.BomByProductDetailList = new List<BomByProductDetail>();
			bOMHeader.BomOperationDetailList = new List<BomOperationDetail>();
			bOMHeader.BomWagesDetailList = new List<BomWagesDetail>();
			bOMHeader.AttachmentsList = new List<DMSAttachments>();
			GridEXRow[] rows = grdInputMaterial.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) != 0)
				{
					BomInputDetail d2 = new BomInputDetail();
					d2.BomInputDetailId = Conversion.ToInt(r2.Cells["Id"].Value);
					if (d2.BomInputDetailId > 0)
					{
						d2.ActionTypeId = 2;
					}
					else
					{
						d2.ActionTypeId = 1;
					}
					if (r2.Cells["ItemId"].Text == string.Empty)
					{
						MessageBox.Show("Item Field Required in Grid Input Row No: " + (r2.RowIndex + 1));
						return;
					}
					d2.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					if (r2.Cells["ItemUomId"].Text == string.Empty)
					{
						MessageBox.Show("UOM  Field Required in Grid Input Row No: " + (r2.RowIndex + 1));
						return;
					}
					DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r2.Cells["ItemId"].Value));
					if (dt.Rows.Count > 0)
					{
						DataRow[] dr = dt.Select("Id='" + Conversion.ToInt(r2.Cells["ItemUomId"].Value) + "'");
						if (dr.Length == 0)
						{
							MessageBox.Show("UOM in Input Grid Row No: " + (r2.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r2.Cells["ItemId"].Text) + ". Please Check...");
							return;
						}
						d2.ItemUomId = Conversion.ToInt(dr[0].ItemArray[0]);
					}
					if (r2.Cells["ItemQty"].Text == string.Empty)
					{
						MessageBox.Show("ItemQty Field Required in Grid Input Row No: " + (r2.RowIndex + 1));
						return;
					}
					d2.ItemQty = Conversion.ToDecimal((object)r2.Cells["ItemQty"].Text);
					if (r2.Cells["NetWeight"].Text == string.Empty)
					{
						MessageBox.Show("NetWeight Field Required in Grid Input Row No: " + (r2.RowIndex + 1));
						return;
					}
					d2.WeightKg = Conversion.ToDecimal(r2.Cells["NetWeight"].Value);
					if (r2.Cells["Rate"].Text == string.Empty)
					{
						MessageBox.Show("Rate Field Required in Grid Input Row No: " + (r2.RowIndex + 1));
						return;
					}
					d2.Rate = Conversion.ToDecimal(r2.Cells["Rate"].Value);
					if (r2.Cells["RateUomId"].Text == string.Empty)
					{
						MessageBox.Show("Rate UOM Field Required in Grid Input Row No: " + (r2.RowIndex + 1));
						return;
					}
					DataTable dt2 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r2.Cells["ItemId"].Value));
					if (dt2.Rows.Count > 0)
					{
						DataRow[] dr3 = dt2.Select("Id='" + Conversion.ToInt(r2.Cells["RateUomId"].Value) + "'");
						if (dr3.Length == 0)
						{
							MessageBox.Show("RateUom in Input Grid Row No: " + (r2.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r2.Cells["ItemId"].Text) + ". Please Check...");
							return;
						}
						d2.RateUomId = Conversion.ToInt(dr3[0].ItemArray[0]);
					}
					if (r2.Cells["Amount"].Text == string.Empty)
					{
						MessageBox.Show("AmountField Required in Grid Input Row No: " + (r2.RowIndex + 1));
						return;
					}
					d2.Amount = Conversion.ToDecimal(r2.Cells["Amount"].Value);
					bOMHeader.BomInputDetailList.Add(d2);
					continue;
				}
				MessageBox.Show("Item Field Required in Grid Input Row No: " + (r2.RowIndex + 1));
				return;
			}
			if (lstRemoveRecordInput.Count > 0 && RecId > 0)
			{
				for (int k = 0; k < lstRemoveRecordInput.Count; k++)
				{
					bOMHeader.BomInputDetailList.Add(lstRemoveRecordInput[k]);
				}
			}
			rows = grdOutPutByProduct.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt((object)r3.Cells["ItemId"].Value.ToString()) == 0)
				{
					continue;
				}
				BomByProductDetail d3 = new BomByProductDetail();
				d3.BomByProductDetailId = Conversion.ToInt(r3.Cells["Id"].Value);
				if (d3.BomByProductDetailId > 0)
				{
					d3.ActionTypeId = 2;
				}
				else
				{
					d3.ActionTypeId = 1;
				}
				if (r3.Cells["ItemId"].Text == string.Empty)
				{
					MessageBox.Show("Item Field Required in Grid Input Row No: " + (r3.RowIndex + 1));
					return;
				}
				d3.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
				if (r3.Cells["ItemUomId"].Text == string.Empty)
				{
					MessageBox.Show("UOM  Field Required in Grid Input Row No: " + (r3.RowIndex + 1));
					return;
				}
				DataTable dt4 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r3.Cells["ItemId"].Value));
				if (dt4.Rows.Count > 0)
				{
					DataRow[] dr4 = dt4.Select("Id='" + Conversion.ToInt(r3.Cells["ItemUomId"].Value) + "'");
					if (dr4.Length == 0)
					{
						MessageBox.Show("UOM in Output Grid Row No: " + (r3.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r3.Cells["ItemId"].Text) + ". Please Check...");
						return;
					}
					d3.ItemUomId = Conversion.ToInt(dr4[0].ItemArray[0]);
				}
				if (r3.Cells["ItemQty"].Text == string.Empty)
				{
					MessageBox.Show("ItemQty Field Required in Grid Input Row No: " + (r3.RowIndex + 1));
					return;
				}
				d3.ItemQty = Conversion.ToDecimal((object)r3.Cells["ItemQty"].Text);
				if (r3.Cells["NetWeight"].Text == string.Empty)
				{
					MessageBox.Show("NetWeight Field Required in Grid Input Row No: " + (r3.RowIndex + 1));
					return;
				}
				d3.Weight = Conversion.ToDecimal(r3.Cells["NetWeight"].Value);
				if (r3.Cells["Rate"].Text == string.Empty)
				{
					MessageBox.Show("Rate Field Required in Grid Output Row No: " + (r3.RowIndex + 1));
					return;
				}
				d3.ItemRate = Conversion.ToDecimal(r3.Cells["Rate"].Value);
				if (r3.Cells["RateUomId"].Text == string.Empty)
				{
					MessageBox.Show("Rate UOM Field Required in Grid Output Row No: " + (r3.RowIndex + 1));
					return;
				}
				DataTable dt3 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r3.Cells["ItemId"].Value));
				if (dt3.Rows.Count > 0)
				{
					DataRow[] dr2 = dt3.Select("Id='" + Conversion.ToInt(r3.Cells["RateUomId"].Value) + "'");
					if (dr2.Length == 0)
					{
						MessageBox.Show("RateUom in output Grid Row No: " + (r3.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r3.Cells["ItemId"].Text) + ". Please Check...");
						return;
					}
					d3.RateUomId = Conversion.ToInt(dr2[0].ItemArray[0]);
				}
				if (r3.Cells["Amount"].Text == string.Empty)
				{
					MessageBox.Show("AmountField Required in Grid Output Row No: " + (r3.RowIndex + 1));
					return;
				}
				d3.Amount = Conversion.ToDecimal(r3.Cells["Amount"].Value);
				bOMHeader.BomByProductDetailList.Add(d3);
			}
			if (lstRemoveRecordOutPut.Count > 0 && RecId > 0)
			{
				for (int j = 0; j < lstRemoveRecordOutPut.Count; j++)
				{
					bOMHeader.BomByProductDetailList.Add(lstRemoveRecordOutPut[j]);
				}
			}
			rows = grdOverHead.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["OverHeadAccountId"].Value) > 0)
				{
					BomFohDetail d = new BomFohDetail();
					if (r.Cells["OverHeadAccountId"].Text == string.Empty)
					{
						MessageBox.Show("OverHeadAccount Field Required in Grid Over Head Row No: " + (r.RowIndex + 1));
						return;
					}
					d.BomFohDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					d.OverHeadTypeId = Conversion.ToInt(r.Cells["OverHeadAccountId"].Value);
					d.GLAccountId = Conversion.ToInt(r.Cells["OverHeadAccountId"].Value);
					if (r.Cells["Amount"].Text == string.Empty)
					{
						MessageBox.Show("Amount Field Required in Grid Over Head Row No: " + (r.RowIndex + 1));
						return;
					}
					d.PercentOfTotalInput = Conversion.ToDecimal((object)r.Cells["PercentOfTotalInput"].Text);
					d.Qty = Conversion.ToDecimal((object)r.Cells["Qty"].Text);
					d.Rate = Conversion.ToDecimal((object)r.Cells["Rate"].Text);
					d.Amount = Conversion.ToDecimal((object)r.Cells["Amount"].Text);
					d.OhRemarks = Conversion.ToString((object)r.Cells["OhRemarks"].Text);
					bOMHeader.BomFohDetailList.Add(d);
				}
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.UploadedFileCustomName = Attachment.dtAttachments.Rows[i]["CustomFileName"].ToString();
				attach.ScreenName = base.Name;
				bOMHeader.AttachmentsList.Add(attach);
			}
			bOMHeader.AttachmentsValues = AttachmentsValues;
			bOMHeader.CustomAttachmentsValues = CustomAttachmentsValues;
			if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
			{
				bOMHeader.DeleteAttachmentsList = new List<DMSAttachments>();
				for (int l = 0; l < AT.RemovedAttachmentListInUpdateCase.Count; l++)
				{
					DMSAttachments delete = new DMSAttachments();
					delete.Attachment = Path.GetFileName(AT.RemovedAttachmentListInUpdateCase[l].Path);
					delete.ScreenName = base.Name;
					bOMHeader.DeleteAttachmentsList.Add(delete);
				}
			}
			int success = BomHeader.Save(bOMHeader);
			bOMHeader.BomHeaderId = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + bOMHeader.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && Attachment.dtAttachments.Rows.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 1664, 0);
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + bOMHeader.DocNo);
			}
			FromReset();
			if (ChkPreview.Checked && formright.DoHavePrintRights)
			{
				BOMSlip(bOMHeader.BomHeaderId);
			}
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			dtInput.Clear();
			grdInputMaterial.ClearStructure();
			dtOutPutByProduct.Clear();
			grdOutPutByProduct.ClearStructure();
			dtOverHead.Clear();
			grdOverHead.ClearStructure();
			BomHeader bOMHeader = new BomHeader();
			RecId = Id;
			bOMHeader = BomHeader.ReadById(RecId);
			if (bOMHeader == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			tabControl1.SelectedIndex = 0;
			txtdocdate.Value = bOMHeader.DocDate;
			txtdocno.Text = Conversion.ToString((object)bOMHeader.DocNo);
			txtRemarksHeader.Text = Conversion.ToString((object)bOMHeader.RemarksHeader);
			CmbCastingType.Value = bOMHeader.CastingTypeId;
			cmbUOM.Value = bOMHeader.ItemUomId;
			txtBom.Text = bOMHeader.BomName;
			((Control)(object)cmbBOMType).Text = bOMHeader.BomType;
			txtEffectedDate.Value = bOMHeader.EffectedDate;
			txtQty.Text = bOMHeader.BomBatchQty.ToString();
			chkIsDefault.Checked = bOMHeader.IsDefault;
			AttachmentsValues = bOMHeader.AttachmentsValues;
			CustomAttachmentsValues = bOMHeader.CustomAttachmentsValues;
			if (bOMHeader.BomInputDetailList.Count > 0)
			{
				for (int k = 0; k < bOMHeader.BomInputDetailList.Count; k++)
				{
					dtInput.Rows.Add(bOMHeader.BomInputDetailList[k].BomInputDetailId, bOMHeader.BomInputDetailList[k].ItemId, bOMHeader.BomInputDetailList[k].ItemUomId, bOMHeader.BomInputDetailList[k].PackUomEquivalent, bOMHeader.BomInputDetailList[k].PackUom, bOMHeader.BomInputDetailList[k].ItemQty, bOMHeader.BomInputDetailList[k].WeightKg, bOMHeader.BomInputDetailList[k].Rate, bOMHeader.BomInputDetailList[k].RateUomId, bOMHeader.BomInputDetailList[k].RateUomEquivalent, bOMHeader.BomInputDetailList[k].RateUom, bOMHeader.BomInputDetailList[k].Amount);
				}
			}
			else
			{
				AddRowInInputGrid();
			}
			if (bOMHeader.BomByProductDetailList.Count > 0)
			{
				for (int j = 0; j < bOMHeader.BomByProductDetailList.Count; j++)
				{
					dtOutPutByProduct.Rows.Add(bOMHeader.BomByProductDetailList[j].BomByProductDetailId, bOMHeader.BomByProductDetailList[j].ItemId, bOMHeader.BomByProductDetailList[j].ItemUomId, bOMHeader.BomByProductDetailList[j].PackUomEquivalent, bOMHeader.BomByProductDetailList[j].PackUom, bOMHeader.BomByProductDetailList[j].ItemQty, bOMHeader.BomByProductDetailList[j].Weight, bOMHeader.BomByProductDetailList[j].ItemRate, bOMHeader.BomByProductDetailList[j].RateUomId, bOMHeader.BomByProductDetailList[j].RateUomEquivalent, bOMHeader.BomByProductDetailList[j].RateUom, bOMHeader.BomByProductDetailList[j].Amount);
				}
			}
			else
			{
				AddRowInOutPutByProductGrid();
			}
			if (bOMHeader.BomFohDetailList.Count > 0)
			{
				for (int i = 0; i < bOMHeader.BomFohDetailList.Count; i++)
				{
					dtOverHead.Rows.Add(bOMHeader.BomFohDetailList[i].BomFohDetailId, bOMHeader.BomFohDetailList[i].OverHeadTypeId, bOMHeader.BomFohDetailList[i].GLAccountId, bOMHeader.BomFohDetailList[i].PercentOfTotalInput, bOMHeader.BomFohDetailList[i].Qty, bOMHeader.BomFohDetailList[i].Rate, bOMHeader.BomFohDetailList[i].Amount, bOMHeader.BomFohDetailList[i].OhRemarks);
				}
			}
			else
			{
				AddRowInOverHeadGrid();
			}
			grdInputMaterial.DataSource = dtInput;
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
			grdOutPutByProduct.DataSource = dtOutPutByProduct;
			grdOutPutByProduct.RetrieveStructure();
			grdOutPutGridSetting();
			grdOverHead.DataSource = dtOverHead;
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSetting();
			UpdateMode = true;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			List<DMSAttachments> byID = DMSAttachments.GetByID(RecId, base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				TA.ActionId = 2;
				TA.CustomFileName = Attachment.UploadedFileCustomName;
				AT.lst.Add(TA);
			}
			Calculation();
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
					BomHeader.DeleteByID(UserAccount.ID, RecId);
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			BOMSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BOMSlip(int PrintId)
	{
		try
		{
			CommonServices.BOM_Casting_Slip1664(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllCombobind()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		try
		{
			string Idtype = Conversion.ToString(cmbbomtypeHistory.Value);
			int Idname = Conversion.ToInt(CmbBomNoHistory.Value);
			DataTable dtcombo = BomHeader.GetDataForDropDownFromBOM(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = 1664.ToString()
			});
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtBOMType = new DataTable();
			dtBOMType.Columns.Add("Id");
			dtBOMType.Columns.Add("BOMType");
			DataTable dtBOMName = new DataTable();
			dtBOMName.Columns.Add("Id", typeof(int));
			dtBOMName.Columns.Add("BOMName");
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "BOMType")
				{
					dtBOMType.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "BOMName")
				{
					dtBOMName.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtBOMType.Rows.Count > 0)
			{
				DDL.BindDDL(dtBOMType, cmbbomtypeHistory, "Id", "BOMType", "BOM Type", ZeroIndex: true);
				((UltraGridBase)cmbbomtypeHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Idtype != null && Idtype != "" && Idtype != "0")
				{
					if (dtBOMType.AsEnumerable().Any((DataRow row) => row.Field<string>("Id") == Idtype))
					{
						((Control)(object)cmbbomtypeHistory).Text = Idtype;
					}
					else
					{
						((Control)(object)cmbbomtypeHistory).Text = string.Empty;
					}
				}
			}
			if (dtBOMName.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtBOMName, CmbBomNoHistory, "Id", "BOMName", "BOM Name", ZeroIndex: true);
			((UltraGridBase)CmbBomNoHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Idname > 0)
			{
				if (dtBOMName.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idname))
				{
					CmbBomNoHistory.Value = Idname;
				}
				else
				{
					((Control)(object)CmbBomNoHistory).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Datetypefill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateTypeHistory, "Id", "Parameters", "Parameters", ZeroIndex: true);
			((UltraGridBase)cmbDateTypeHistory).Rows[2].Activate();
			((UltraGridBase)cmbDateTypeHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateTypeHistory_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateTypeHistory.Value) == 1)
			{
				txtFromdateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				txtFromdateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.Now;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromdateHistory.Value = first;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromdateHistory.Value = first2;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 5)
			{
				txtFromdateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
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
			Datetypefill();
			AllCombobind();
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
			header.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			header.DocumentTypeId = 1664;
			header.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text.Trim());
			header.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text.Trim());
			if (txtFromdateHistory.Checked)
			{
				header.FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value);
			}
			if (txtToDateHistory.Checked)
			{
				header.ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value);
			}
			if (Conversion.ToInt(cmbbomtypeHistory.Value) > 0)
			{
				header.ProductionNo = Conversion.ToString((object)((Control)(object)cmbbomtypeHistory).Text);
			}
			header.Id = Conversion.ToInt(CmbBomNoHistory.Value);
			header.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			dtHistory = BomHeader.FormHistory(header);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("BomName", typeof(string));
				dtcol.Columns.Add("CastingType", typeof(string));
				dtcol.Columns.Add("BomType", typeof(string));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("Uom", typeof(string));
				dtcol.Columns.Add("BomBatchQty", typeof(decimal));
				dtcol.Columns.Add("EffectedDate", typeof(DateTime));
				dtcol.Columns.Add("IsActive", typeof(bool));
				dtcol.Columns.Add("IsDefault", typeof(bool));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("IsApproved", typeof(bool));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				foreach (DataRow row in dtHistory.Rows)
				{
					dtcol.Rows.Add(row["RecordNo"], row["BomHeaderId"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["BomName"], row["CastingType"], row["BomType"], row["ItemName"], row["Uom"], row["BomBatchQty"], row["EffectedDate"], row["IsActive"], row["IsDefault"], row["RemarksHeader"], row["EntryUserName"], row["EntryDate"], row["ModifyUserName"], row["ModifyDate"], row["IsApproved"], row["NoOfAttachments"]);
				}
				DatagridHistory.DataSource = dtcol;
				DatagridHistory.RetrieveStructure();
				HistoryGridSetting();
			}
			else
			{
				DatagridHistory.DataSource = null;
				grdHistoryInput.DataSource = null;
				grdHistoryOutput.DataSource = null;
				grdHistoryOverHead.DataSource = null;
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
			DatagridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DatagridHistory.RootTable.Columns["IsActive"].Visible = false;
			DatagridHistory.RootTable.Columns["IsDefault"].Visible = false;
			DatagridHistory.RootTable.Columns["RecordNo"].Visible = false;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridWrappingAndColumnSettings(DatagridHistory, 2);
			DatagridHistory.RootTable.Columns["RecordNo"].Width = Constants.InventoryConstants.RecordNo;
			DatagridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DatagridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			DatagridHistory.RootTable.Columns["BomName"].Width = Constants.InventoryConstants.ProductionNo;
			DatagridHistory.RootTable.Columns["CastingType"].Width = Constants.InventoryConstants.CastingType;
			DatagridHistory.RootTable.Columns["BomType"].Width = Constants.InventoryConstants.CastingType;
			DatagridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			DatagridHistory.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
			DatagridHistory.RootTable.Columns["BomBatchQty"].Width = Constants.InventoryConstants.QtyConstant;
			DatagridHistory.RootTable.Columns["EffectedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DatagridHistory.RootTable.Columns["IsActive"].Width = 60;
			DatagridHistory.RootTable.Columns["IsDefault"].Width = 60;
			DatagridHistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			DatagridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			DatagridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DatagridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			DatagridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DatagridHistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			DatagridHistory.RootTable.Columns.Add("Print");
			DatagridHistory.RootTable.Columns["Print"].Key = "Print";
			DatagridHistory.RootTable.Columns["Print"].Caption = "Print";
			DatagridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DatagridHistory.RootTable.Columns["Print"].Width = 50;
			DatagridHistory.RootTable.Columns["Print"].Position = 0;
			DatagridHistory.RootTable.Columns.Add("Edit");
			DatagridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Width = 50;
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
		try
		{
			if (formright.DoHaveUpdateRights)
			{
				ReadById(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
				return;
			}
			throw new Exception("ypu don't have updae rights...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				BOMSlip(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value), 1664);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				DatagridHistoryDetailbind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistoryDetailbind(int Id)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		try
		{
			grdHistoryInput.ClearStructure();
			grdHistoryOutput.ClearStructure();
			grdHistoryOverHead.ClearStructure();
			BomHeader bOMHeader = new BomHeader();
			bOMHeader = BomHeader.ReadById(Id);
			if (bOMHeader == null)
			{
				return;
			}
			DataTable dtInputHistory = new DataTable();
			DataTable dtOutPutByProductHistory = new DataTable();
			DataTable dtOverHeadHistory = new DataTable();
			dtInputHistory.Columns.Add("Id", typeof(int));
			dtInputHistory.Columns.Add("Item", typeof(string));
			dtInputHistory.Columns.Add("Uom", typeof(string));
			dtInputHistory.Columns.Add("ItemQty", typeof(decimal));
			dtInputHistory.Columns.Add("NetWeight", typeof(decimal));
			dtInputHistory.Columns.Add("Rate", typeof(decimal));
			dtInputHistory.Columns.Add("RateUom", typeof(string));
			dtInputHistory.Columns.Add("Amount", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("Id", typeof(int));
			dtOutPutByProductHistory.Columns.Add("Item", typeof(string));
			dtOutPutByProductHistory.Columns.Add("Uom", typeof(string));
			dtOutPutByProductHistory.Columns.Add("ItemQty", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("NetWeight", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("Rate", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("RateUom", typeof(string));
			dtOutPutByProductHistory.Columns.Add("Amount", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Id", typeof(int));
			dtOverHeadHistory.Columns.Add("OverHeadType", typeof(string));
			dtOverHeadHistory.Columns.Add("OverHeadAccount", typeof(string));
			dtOverHeadHistory.Columns.Add("Percentage", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Qty", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Rate", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Amount", typeof(decimal));
			dtOverHeadHistory.Columns.Add("OhRemarks", typeof(string));
			if (bOMHeader.BomInputDetailList.Count > 0)
			{
				for (int k = 0; k < bOMHeader.BomInputDetailList.Count; k++)
				{
					dtInputHistory.Rows.Add(bOMHeader.BomInputDetailList[k].BomInputDetailId, bOMHeader.BomInputDetailList[k].ItemName, bOMHeader.BomInputDetailList[k].PackUom, bOMHeader.BomInputDetailList[k].ItemQty, bOMHeader.BomInputDetailList[k].WeightKg, bOMHeader.BomInputDetailList[k].Rate, bOMHeader.BomInputDetailList[k].RateUom, bOMHeader.BomInputDetailList[k].Amount);
				}
			}
			else
			{
				grdHistoryInput.ClearStructure();
			}
			if (bOMHeader.BomByProductDetailList.Count > 0)
			{
				for (int j = 0; j < bOMHeader.BomByProductDetailList.Count; j++)
				{
					dtOutPutByProductHistory.Rows.Add(bOMHeader.BomByProductDetailList[j].BomByProductDetailId, bOMHeader.BomByProductDetailList[j].ItemName, bOMHeader.BomByProductDetailList[j].PackUom, bOMHeader.BomByProductDetailList[j].ItemQty, bOMHeader.BomByProductDetailList[j].Weight, bOMHeader.BomByProductDetailList[j].ItemRate, bOMHeader.BomByProductDetailList[j].RateUom, bOMHeader.BomByProductDetailList[j].Amount);
				}
			}
			else
			{
				grdHistoryOutput.ClearStructure();
			}
			if (bOMHeader.BomFohDetailList.Count > 0)
			{
				for (int i = 0; i < bOMHeader.BomFohDetailList.Count; i++)
				{
					dtOverHeadHistory.Rows.Add(bOMHeader.BomFohDetailList[i].BomFohDetailId, bOMHeader.BomFohDetailList[i].OverHeadTypeId, bOMHeader.BomFohDetailList[i].OverHeadAccountTitle, bOMHeader.BomFohDetailList[i].PercentOfTotalInput, bOMHeader.BomFohDetailList[i].Qty, bOMHeader.BomFohDetailList[i].Rate, bOMHeader.BomFohDetailList[i].Amount, bOMHeader.BomFohDetailList[i].OhRemarks);
				}
			}
			else
			{
				grdHistoryOverHead.ClearStructure();
			}
			grdHistoryInput.DataSource = dtInputHistory;
			grdHistoryInput.RetrieveStructure();
			grdHistoryInputSetting();
			grdHistoryOutput.DataSource = dtOutPutByProductHistory;
			grdHistoryOutput.RetrieveStructure();
			grdHistoryOutputSetting();
			grdHistoryOverHead.DataSource = dtOverHeadHistory;
			grdHistoryOverHead.RetrieveStructure();
			grdHistoryOverHeadSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryInputSetting()
	{
		try
		{
			grdHistoryInput.RootTable.Columns["Id"].Visible = false;
			grdHistoryInput.RootTable.Columns["NetWeight"].Visible = false;
			grdHistoryInput.RootTable.Columns["RateUom"].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(grdHistoryInput, 1);
			grdHistoryInput.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			grdHistoryInput.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
			grdHistoryInput.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdHistoryInput.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdHistoryInput.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdHistoryInput.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdHistoryInput.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryOutputSetting()
	{
		try
		{
			grdHistoryOutput.RootTable.Columns["Id"].Visible = false;
			grdHistoryOutput.RootTable.Columns["NetWeight"].Visible = false;
			grdHistoryOutput.RootTable.Columns["RateUom"].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(grdHistoryOutput, 1);
			grdHistoryOutput.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			grdHistoryOutput.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
			grdHistoryOutput.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdHistoryOutput.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdHistoryOutput.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdHistoryOutput.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdHistoryOutput.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryOverHeadSetting()
	{
		try
		{
			grdHistoryOverHead.RootTable.Columns["Id"].Visible = false;
			grdHistoryOverHead.RootTable.Columns["OverHeadType"].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(grdHistoryOverHead, 1);
			grdHistoryOverHead.RootTable.Columns["OverHeadType"].Width = Constants.InventoryConstants.CastingType;
			grdHistoryOverHead.RootTable.Columns["OverHeadAccount"].Width = Constants.InventoryConstants.AccountTitle;
			grdHistoryOverHead.RootTable.Columns["Percentage"].Width = Constants.InventoryConstants.PercentOfTotal + 20;
			grdHistoryOverHead.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdHistoryOverHead.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdHistoryOverHead.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistoryOverHead.RootTable.Columns["OhRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbDateTypeHistory).Text = string.Empty;
			((Control)(object)cmbbomtypeHistory).Text = string.Empty;
			((Control)(object)CmbBomNoHistory).Text = string.Empty;
			grdHistoryOverHead.ClearStructure();
			grdHistoryOutput.ClearStructure();
			grdHistoryInput.ClearStructure();
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmBillOfMaterialCasting_Manufacturing_KeyDown(object sender, KeyEventArgs e)
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
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && formright.DoHaveCanDelete && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled && UpdateMode)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && btnPrint.Visible && btnPrint.Enabled)
				{
					btnPrint_Click(null, null);
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
					txtdocdate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.F12)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdInputMaterial).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					txtdocdate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdInputMaterial).Focused)
					{
						((Control)(object)grdOutPutByProduct).Focus();
					}
					else if (((Control)(object)grdOutPutByProduct).Focused)
					{
						((Control)(object)grdOverHead).Focus();
					}
					else
					{
						((Control)(object)grdInputMaterial).Focus();
					}
				}
			}
			else
			{
				if (tabControl1.SelectedIndex != 1)
				{
					return;
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DatagridHistory).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdHistoryInput).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdHistoryInput).Focused)
					{
						((Control)(object)grdHistoryOutput).Focus();
					}
					else if (((Control)(object)grdHistoryOutput).Focused)
					{
						((Control)(object)grdHistoryOverHead).Focus();
					}
					else
					{
						((Control)(object)grdHistoryInput).Focus();
					}
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

	private void grdInputMaterial_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInputMaterial.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdInputMaterial.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdInputMaterial.CurrentColumn.Key == "Add")
				{
					AddRowInInputGrid();
				}
				if (grdInputMaterial.CurrentColumn.Key == "Delete")
				{
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						BomInputDetail d2 = new BomInputDetail();
						d2.BomInputDetailId = Conversion.ToInt(r.Cells["Id"].Value);
						d2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						d2.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
						d2.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
						d2.WeightKg = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						d2.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						d2.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
						d2.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
						d2.ActionTypeId = 3;
						lstRemoveRecordInput.Add(d2);
						dtInput.Rows[r.RowIndex].Delete();
						if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
						grdInputMaterial.UpdateData();
					}
					else if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtInput.Rows.Count == 0)
					{
						AddRowInInputGrid();
					}
				}
				grdInputMaterial.UpdateData();
				Calculation();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInInputGrid();
				grdInputMaterial.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					BomInputDetail d1 = new BomInputDetail();
					d1.BomInputDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.WeightKg = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					d1.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					d1.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					d1.ActionTypeId = 3;
					lstRemoveRecordInput.Add(d1);
					dtInput.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdInputMaterial.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtInput.Rows.Count == 0)
				{
					AddRowInInputGrid();
				}
				grdInputMaterial.UpdateData();
				Calculation();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			decimal Name = default(decimal);
			string NameCode = "";
			if (grdInputMaterial.CurrentColumn.Key == "ItemId")
			{
				int PreviousItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				GrdPopUp obj3 = new GrdPopUp(lstitem, "Id", "ItemName");
				obj3.StartPosition = FormStartPosition.CenterScreen;
				obj3.ShowDialog();
				obj3.BringToFront();
				Id = Conversion.ToInt((object)obj3.ReturnId);
				r.Cells["ItemId"].Value = Id;
				if (Id != PreviousItemId)
				{
					r.Cells["ItemUomId"].Value = Id;
					r.Cells["Uom"].Value = Name;
					r.Cells["UomCode"].Value = NameCode;
					r.Cells["RateUom"].Value = Name;
					r.Cells["RateUomId"].Value = Id;
					r.Cells["RateUomCode"].Value = NameCode;
					r.Cells["NetWeight"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
			}
			if (grdInputMaterial.CurrentColumn.Key == "UomCode")
			{
				bindUom(Conversion.ToInt(r.Cells["ItemId"].Value));
				GrdPopUp obj2 = new GrdPopUp(dtUom, "Id", "ItemUOMCode");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				Id = Conversion.ToInt((object)obj2.ReturnId);
				NameCode = Conversion.ToString((object)obj2.ReturnName);
				DataRow foundRow2 = dtUom.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Id);
				Name = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["ItemUOM"]) : 0m);
				r.Cells["ItemUomId"].Value = Id;
				r.Cells["Uom"].Value = Name;
				r.Cells["UomCode"].Value = NameCode;
				r.Cells["RateUom"].Value = Name;
				r.Cells["RateUomId"].Value = Id;
				r.Cells["RateUomCode"].Value = NameCode;
			}
			if (grdInputMaterial.CurrentColumn.Key == "RateUomCode")
			{
				bindRateUom(Conversion.ToInt(r.Cells["ItemId"].Value));
				GrdPopUp obj = new GrdPopUp(dtRateUom, "RateId", "RateUOMCode");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id = Conversion.ToInt((object)obj.ReturnId);
				NameCode = Conversion.ToString((object)obj.ReturnName);
				DataRow foundRow = dtRateUom.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("RateId") == Id);
				Name = ((foundRow != null) ? Conversion.ToDecimal(foundRow["RateUOM"]) : 0m);
				r.Cells["RateUom"].Value = Name;
				r.Cells["RateUomId"].Value = Id;
				r.Cells["RateUomCode"].Value = NameCode;
			}
			decimal Weight = default(decimal);
			decimal Iuom = default(decimal);
			if (r.Cells["ItemQty"].Value.ToString() != string.Empty && r.Cells["ItemUomId"].Value.ToString() != string.Empty)
			{
				decimal num = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
				Weight = num * Iuom;
				r.Cells["NetWeight"].Value = Weight;
			}
			else
			{
				Weight = default(decimal);
				r.Cells["NetWeight"].Value = Weight;
			}
			if (Weight != 0m && Conversion.ToDecimal(r.Cells["Rate"].Value) != 0m && Conversion.ToInt(r.Cells["RateUomId"].Value) != 0)
			{
				decimal num2 = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
				decimal rate = Conversion.ToDecimal((object)r.Cells["Rate"].Value.ToString());
				decimal Ruom = Conversion.ToDecimal((object)r.Cells["RateUom"].Value.ToString());
				decimal Amount = num2 * Iuom / Ruom * rate;
				r.Cells["Amount"].Value = Amount;
			}
			else
			{
				r.Cells["Amount"].Value = 0;
			}
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutByProduct_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutPutByProduct.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOutPutByProduct.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdOutPutByProduct.CurrentColumn.Key == "Add")
				{
					AddRowInOutPutByProductGrid();
				}
				if (grdOutPutByProduct.CurrentColumn.Key == "Delete")
				{
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						BomByProductDetail d2 = new BomByProductDetail();
						d2.BomByProductDetailId = Conversion.ToInt(r.Cells["Id"].Value);
						d2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						d2.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
						d2.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
						d2.Weight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						d2.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						d2.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
						d2.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
						d2.ActionTypeId = 3;
						lstRemoveRecordOutPut.Add(d2);
						dtOutPutByProduct.Rows[r.RowIndex].Delete();
						if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
						grdOutPutByProduct.UpdateData();
					}
					else if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtOutPutByProduct.Rows.Count == 0)
					{
						AddRowInOutPutByProductGrid();
					}
				}
				grdOutPutByProduct.UpdateData();
				Calculation();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInOutPutByProductGrid();
				grdOutPutByProduct.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					BomByProductDetail d1 = new BomByProductDetail();
					d1.BomByProductDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.Weight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					d1.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					d1.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					d1.ActionTypeId = 3;
					lstRemoveRecordOutPut.Add(d1);
					dtOutPutByProduct.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdOutPutByProduct.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtOutPutByProduct.Rows.Count == 0)
				{
					AddRowInOutPutByProductGrid();
				}
				grdOutPutByProduct.UpdateData();
				Calculation();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			decimal Name = default(decimal);
			string NameCode = "";
			if (grdOutPutByProduct.CurrentColumn.Key == "ItemId")
			{
				int PreviousItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				GrdPopUp obj3 = new GrdPopUp(lstitem, "Id", "ItemName");
				obj3.StartPosition = FormStartPosition.CenterScreen;
				obj3.ShowDialog();
				obj3.BringToFront();
				Id = Conversion.ToInt((object)obj3.ReturnId);
				r.Cells["ItemId"].Value = Id;
				if (Id != PreviousItemId)
				{
					r.Cells["ItemUomId"].Value = Id;
					r.Cells["Uom"].Value = Name;
					r.Cells["UomCode"].Value = NameCode;
					r.Cells["RateUom"].Value = Name;
					r.Cells["RateUomId"].Value = Id;
					r.Cells["RateUomCode"].Value = NameCode;
					r.Cells["NetWeight"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
			}
			if (grdOutPutByProduct.CurrentColumn.Key == "UomCode")
			{
				bindUom(Conversion.ToInt(r.Cells["ItemId"].Value));
				GrdPopUp obj2 = new GrdPopUp(dtUom, "Id", "ItemUOMCode");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				Id = Conversion.ToInt((object)obj2.ReturnId);
				NameCode = Conversion.ToString((object)obj2.ReturnName);
				DataRow foundRow2 = dtUom.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Id);
				Name = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["ItemUOM"]) : 0m);
				r.Cells["ItemUomId"].Value = Id;
				r.Cells["Uom"].Value = Name;
				r.Cells["UomCode"].Value = NameCode;
				r.Cells["RateUom"].Value = Name;
				r.Cells["RateUomId"].Value = Id;
				r.Cells["RateUomCode"].Value = NameCode;
			}
			if (grdOutPutByProduct.CurrentColumn.Key == "RateUomCode")
			{
				bindRateUom(Conversion.ToInt(r.Cells["ItemId"].Value));
				GrdPopUp obj = new GrdPopUp(dtRateUom, "RateId", "RateUOMCode");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id = Conversion.ToInt((object)obj.ReturnId);
				NameCode = Conversion.ToString((object)obj.ReturnName);
				DataRow foundRow = dtRateUom.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("RateId") == Id);
				Name = ((foundRow != null) ? Conversion.ToDecimal(foundRow["RateUOM"]) : 0m);
				r.Cells["RateUom"].Value = Name;
				r.Cells["RateUomId"].Value = Id;
				r.Cells["RateUomCode"].Value = NameCode;
			}
			decimal Weight = default(decimal);
			decimal Iuom = default(decimal);
			if (r.Cells["ItemQty"].Value.ToString() != string.Empty && r.Cells["ItemUomId"].Value.ToString() != string.Empty)
			{
				decimal num = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
				Weight = num * Iuom;
				r.Cells["NetWeight"].Value = Weight;
			}
			else
			{
				Weight = default(decimal);
				r.Cells["NetWeight"].Value = Weight;
			}
			if (Weight != 0m && Conversion.ToDecimal(r.Cells["Rate"].Value) != 0m && Conversion.ToInt(r.Cells["RateUomId"].Value) != 0)
			{
				decimal num2 = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
				decimal rate = Conversion.ToDecimal((object)r.Cells["Rate"].Value.ToString());
				decimal Ruom = Conversion.ToDecimal((object)r.Cells["RateUom"].Value.ToString());
				decimal Amount = num2 * Iuom / Ruom * rate;
				r.Cells["Amount"].Value = Amount;
			}
			else
			{
				r.Cells["Amount"].Value = 0;
			}
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOverHead.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOverHead.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdOverHead.CurrentColumn.Key == "Add")
				{
					AddRowInOverHeadGrid();
				}
				if (grdOverHead.CurrentColumn.Key == "Delete")
				{
					dtOverHead.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtOverHead.Rows.Count == 0)
					{
						AddRowInOverHeadGrid();
					}
				}
				grdOverHead.UpdateData();
				Calculation();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInOverHeadGrid();
				grdOverHead.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtOverHead.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtOverHead.Rows.Count == 0)
				{
					AddRowInOverHeadGrid();
				}
				grdOverHead.UpdateData();
				Calculation();
			}
			if (e.KeyCode == Keys.F1 && grdOverHead.CurrentColumn != null && grdOverHead.CurrentColumn.Key == "OverHeadAccountId")
			{
				GrdPopUp obj = new GrdPopUp(dtAccounts, "Id", "AccountTitle");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				r.Cells["OverHeadAccountId"].Value = obj.ReturnId;
			}
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
			int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DatagridHistory.CurrentColumn.Key == "Edit")
				{
					if (!formright.DoHaveUpdateRights)
					{
						throw new Exception("ypu don't have updae rights...");
					}
					ReadById(Conversion.ToInt((object)Id));
				}
				if (DatagridHistory.CurrentColumn.Key == "Print")
				{
					BOMSlip(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value));
				}
				if (DatagridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 1664);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("ypu don't have updae rights...");
				}
				ReadById(Conversion.ToInt((object)Id));
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DatagridHistory).Name);
			if (fs != null)
			{
				DatagridHistory.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdInputMaterial).Name);
			if (fs != null)
			{
				grdInputMaterial.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdOutPutByProduct).Name);
			if (fs != null)
			{
				grdOutPutByProduct.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Expected O, but got Unknown
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Expected O, but got Unknown
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Expected O, but got Unknown
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Expected O, but got Unknown
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ab: Expected O, but got Unknown
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Expected O, but got Unknown
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		//IL_0c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c86: Expected O, but got Unknown
		//IL_0c93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9d: Expected O, but got Unknown
		//IL_0f5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f67: Expected O, but got Unknown
		//IL_0f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7e: Expected O, but got Unknown
		//IL_1221: Unknown result type (might be due to invalid IL or missing references)
		//IL_122b: Expected O, but got Unknown
		//IL_1238: Unknown result type (might be due to invalid IL or missing references)
		//IL_1242: Expected O, but got Unknown
		//IL_4471: Unknown result type (might be due to invalid IL or missing references)
		//IL_447b: Expected O, but got Unknown
		//IL_449f: Unknown result type (might be due to invalid IL or missing references)
		//IL_44a9: Expected O, but got Unknown
		GridEXLayout grdOutPutByProduct_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOverHead_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInputMaterial_DesignTimeLayout = new GridEXLayout();
		Appearance appearance118 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance127 = new Appearance();
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
		GridEXLayout grdHistoryOutput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryOverHead_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryInput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdOutPutByProduct = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.grdOverHead = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdInputMaterial = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbCastingType = new UltraCombo();
		this.label77 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.txtNetValue = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.txtTotalInputs = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtOverHead = new System.Windows.Forms.TextBox();
		this.txtLessByProduct = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.txtEffectedDate = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.txtBom = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbBOMType = new UltraCombo();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.cmbUOM = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.chkIsDefault = new System.Windows.Forms.CheckBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.label21 = new System.Windows.Forms.Label();
		this.ChkPreview = new System.Windows.Forms.CheckBox();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel45 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdHistoryOutput = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.grdHistoryOverHead = new GridEX();
		this.panel42 = new System.Windows.Forms.Panel();
		this.label34 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdHistoryInput = new GridEX();
		this.panel44 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.DatagridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.label30 = new System.Windows.Forms.Label();
		this.cmbbomtypeHistory = new UltraCombo();
		this.CmbBomNoHistory = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label28 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.panel38 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.label32 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutByProduct).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).BeginInit();
		this.panel14.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputMaterial).BeginInit();
		this.panel7.SuspendLayout();
		this.panel17.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCastingType).BeginInit();
		this.panel4.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBOMType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel45.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOverHead).BeginInit();
		this.panel42.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).BeginInit();
		this.panel44.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbomtypeHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBomNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel38.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1084, 661);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1084, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1076, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel10);
		this.panel2.Controls.Add(this.chkIsDefault);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 56);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1070, 573);
		this.panel2.TabIndex = 0;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutPutByProduct);
		this.panel6.Controls.Add(this.panel11);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHead);
		this.panel6.Controls.Add(this.panel14);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(532, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(538, 573);
		this.panel6.TabIndex = 0;
		((ScrollableBase)this.grdOutPutByProduct).BackColor = System.Drawing.Color.White;
		grdOutPutByProduct_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutPutByProduct.DesignTimeLayout = grdOutPutByProduct_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutPutByProduct.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Location = new System.Drawing.Point(0, 224);
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Name = "grdOutPutByProduct";
		((ScrollableBase)this.grdOutPutByProduct).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Size = new System.Drawing.Size(538, 349);
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).TabIndex = 1;
		this.grdOutPutByProduct.TotalRow = (InheritableBoolean)1;
		this.grdOutPutByProduct.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutPutByProduct).VisualStyle = (VisualStyle)4;
		this.grdOutPutByProduct.CellUpdated += new ColumnActionEventHandler(grdOutPutByProduct_CellUpdated);
		this.grdOutPutByProduct.ColumnButtonClick += new ColumnActionEventHandler(grdOutPutByProduct_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOutPutByProduct_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.White;
		this.panel11.Controls.Add(this.ctrlGrdBar3);
		this.panel11.Controls.Add(this.label23);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 197);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(538, 27);
		this.panel11.TabIndex = 9;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(496, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdOutPutByProduct;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 127;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.White;
		this.label23.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Teal;
		this.label23.Location = new System.Drawing.Point(7, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(172, 16);
		this.label23.TabIndex = 126;
		this.label23.Text = "OutPut by Product/Scrap";
		((ScrollableBase)this.grdOverHead).BackColor = System.Drawing.Color.White;
		grdOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverHead.DesignTimeLayout = grdOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdOverHead).Name = "grdOverHead";
		((ScrollableBase)this.grdOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Size = new System.Drawing.Size(538, 171);
		((System.Windows.Forms.Control)(object)this.grdOverHead).TabIndex = 0;
		this.grdOverHead.TotalRow = (InheritableBoolean)1;
		this.grdOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverHead).VisualStyle = (VisualStyle)4;
		this.grdOverHead.CellUpdated += new ColumnActionEventHandler(grdOverHead_CellUpdated);
		this.grdOverHead.ColumnButtonClick += new ColumnActionEventHandler(grdOverHead_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOverHead).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOverHead_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.White;
		this.panel14.Controls.Add(this.label22);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(538, 26);
		this.panel14.TabIndex = 12;
		this.label22.AutoSize = true;
		this.label22.BackColor = System.Drawing.Color.White;
		this.label22.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Teal;
		this.label22.Location = new System.Drawing.Point(6, 5);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(71, 16);
		this.label22.TabIndex = 126;
		this.label22.Text = "OverHead";
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputMaterial);
		this.panel10.Controls.Add(this.panel7);
		this.panel10.Controls.Add(this.panel17);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(532, 573);
		this.panel10.TabIndex = 3;
		((ScrollableBase)this.grdInputMaterial).BackColor = System.Drawing.Color.White;
		grdInputMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputMaterial.DesignTimeLayout = grdInputMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputMaterial.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Location = new System.Drawing.Point(0, 238);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Name = "grdInputMaterial";
		((ScrollableBase)this.grdInputMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Size = new System.Drawing.Size(532, 335);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).TabIndex = 0;
		this.grdInputMaterial.TotalRow = (InheritableBoolean)1;
		this.grdInputMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputMaterial).VisualStyle = (VisualStyle)4;
		this.grdInputMaterial.CellUpdated += new ColumnActionEventHandler(grdInputMaterial_CellUpdated);
		this.grdInputMaterial.ColumnButtonClick += new ColumnActionEventHandler(grdInputMaterial_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInputMaterial_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 212);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(532, 26);
		this.panel7.TabIndex = 3;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(492, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdInputMaterial;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 127;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Teal;
		this.label1.Location = new System.Drawing.Point(7, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(99, 16);
		this.label1.TabIndex = 126;
		this.label1.Text = "Input Material";
		this.panel17.Controls.Add(this.groupBox2);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(532, 212);
		this.panel17.TabIndex = 2;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCastingType);
		this.groupBox2.Controls.Add(this.label77);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.panel4);
		this.groupBox2.Controls.Add(this.txtdocdate);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.txtEffectedDate);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.txtBom);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBOMType);
		this.groupBox2.Controls.Add(this.txtRemarksHeader);
		this.groupBox2.Controls.Add(this.txtQty);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(519, 204);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.CmbCastingType.AllowNull = (DefaultableBoolean)1;
		this.CmbCastingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCastingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCastingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCastingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance129;
		((SpecialBoxBase)((UltraGridBase)this.CmbCastingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCastingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCastingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCastingType).Location = new System.Drawing.Point(85, 120);
		((System.Windows.Forms.Control)(object)this.CmbCastingType).Name = "CmbCastingType";
		((System.Windows.Forms.Control)(object)this.CmbCastingType).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbCastingType).TabIndex = 5;
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label77.Location = new System.Drawing.Point(3, 124);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(74, 15);
		this.label77.TabIndex = 606;
		this.label77.Text = "Casting Type";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(3, 19);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(63, 15);
		this.label37.TabIndex = 138;
		this.label37.Text = "No && Date";
		this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.textBox1);
		this.panel4.Controls.Add(this.label31);
		this.panel4.Controls.Add(this.txtNetValue);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.panel20);
		this.panel4.Controls.Add(this.txtTotalInputs);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtOverHead);
		this.panel4.Controls.Add(this.txtLessByProduct);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Location = new System.Drawing.Point(289, 10);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(227, 164);
		this.panel4.TabIndex = 9;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 30);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(80, 17);
		this.label6.TabIndex = 91;
		this.label6.Text = "Total Inputs";
		this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox1.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.textBox1.Location = new System.Drawing.Point(87, 135);
		this.textBox1.Name = "textBox1";
		this.textBox1.ReadOnly = true;
		this.textBox1.Size = new System.Drawing.Size(137, 25);
		this.textBox1.TabIndex = 6;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(5, 138);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(75, 17);
		this.label31.TabIndex = 97;
		this.label31.Text = "Value / Qty";
		this.txtNetValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetValue.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtNetValue.Location = new System.Drawing.Point(87, 108);
		this.txtNetValue.Name = "txtNetValue";
		this.txtNetValue.ReadOnly = true;
		this.txtNetValue.Size = new System.Drawing.Size(137, 25);
		this.txtNetValue.TabIndex = 5;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(5, 111);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(66, 17);
		this.label10.TabIndex = 97;
		this.label10.Text = "Net Value";
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label5);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(225, 23);
		this.panel20.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.Transparent;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(141, 16);
		this.label5.TabIndex = 33;
		this.label5.Text = "Calculation Summery";
		this.txtTotalInputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalInputs.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtTotalInputs.Location = new System.Drawing.Point(87, 25);
		this.txtTotalInputs.Name = "txtTotalInputs";
		this.txtTotalInputs.ReadOnly = true;
		this.txtTotalInputs.Size = new System.Drawing.Size(137, 25);
		this.txtTotalInputs.TabIndex = 1;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 56);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(63, 17);
		this.label9.TabIndex = 95;
		this.label9.Text = "Total O.H";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(26, 100);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(18, 16);
		this.label25.TabIndex = 93;
		this.label25.Text = "=";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(29, 71);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(14, 16);
		this.label24.TabIndex = 93;
		this.label24.Text = "-";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(26, 42);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(18, 16);
		this.label3.TabIndex = 93;
		this.label3.Text = "+";
		this.txtOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOverHead.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtOverHead.Location = new System.Drawing.Point(87, 52);
		this.txtOverHead.Name = "txtOverHead";
		this.txtOverHead.ReadOnly = true;
		this.txtOverHead.Size = new System.Drawing.Size(137, 25);
		this.txtOverHead.TabIndex = 2;
		this.txtLessByProduct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLessByProduct.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtLessByProduct.Location = new System.Drawing.Point(87, 79);
		this.txtLessByProduct.Name = "txtLessByProduct";
		this.txtLessByProduct.ReadOnly = true;
		this.txtLessByProduct.Size = new System.Drawing.Size(137, 25);
		this.txtLessByProduct.TabIndex = 4;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(5, 84);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(75, 17);
		this.label8.TabIndex = 93;
		this.label8.Text = "By Product";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(164, 15);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(122, 23);
		this.txtdocdate.TabIndex = 1;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(85, 15);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(76, 23);
		this.txtdocno.TabIndex = 0;
		this.txtEffectedDate.CustomFormat = "dd-MMM-yy";
		this.txtEffectedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEffectedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEffectedDate.Location = new System.Drawing.Point(85, 41);
		this.txtEffectedDate.Name = "txtEffectedDate";
		this.txtEffectedDate.Size = new System.Drawing.Size(201, 23);
		this.txtEffectedDate.TabIndex = 2;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(3, 45);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(78, 15);
		this.label13.TabIndex = 133;
		this.label13.Text = "Effected Date";
		this.txtBom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBom.Location = new System.Drawing.Point(85, 67);
		this.txtBom.Name = "txtBom";
		this.txtBom.Size = new System.Drawing.Size(201, 23);
		this.txtBom.TabIndex = 3;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(3, 71);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(85, 15);
		this.label11.TabIndex = 130;
		this.label11.Text = "Bom Manual #";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(3, 98);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(62, 15);
		this.label12.TabIndex = 131;
		this.label12.Text = "BOM Type";
		this.cmbBOMType.AllowNull = (DefaultableBoolean)1;
		this.cmbBOMType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBOMType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBOMType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBOMType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance124;
		((SpecialBoxBase)((UltraGridBase)this.cmbBOMType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance141).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance163).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBOMType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBOMType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBOMType).Location = new System.Drawing.Point(85, 92);
		((System.Windows.Forms.Control)(object)this.cmbBOMType).Name = "cmbBOMType";
		((System.Windows.Forms.Control)(object)this.cmbBOMType).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbBOMType).TabIndex = 4;
		this.txtRemarksHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(85, 177);
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(430, 23);
		this.txtRemarksHeader.TabIndex = 8;
		this.txtRemarksHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(192, 150);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(94, 23);
		this.txtQty.TabIndex = 7;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(164, 154);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(26, 15);
		this.label18.TabIndex = 89;
		this.label18.Text = "Qty";
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance149).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(85, 148);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(76, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 6;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(3, 181);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(52, 15);
		this.label2.TabIndex = 87;
		this.label2.Text = "Remarks";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(3, 154);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(35, 15);
		this.label44.TabIndex = 87;
		this.label44.Text = "UOM";
		this.chkIsDefault.AutoSize = true;
		this.chkIsDefault.Checked = true;
		this.chkIsDefault.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkIsDefault.Enabled = false;
		this.chkIsDefault.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIsDefault.Location = new System.Drawing.Point(1436, 112);
		this.chkIsDefault.Name = "chkIsDefault";
		this.chkIsDefault.Size = new System.Drawing.Size(74, 19);
		this.chkIsDefault.TabIndex = 9;
		this.chkIsDefault.Text = "IsDefault";
		this.chkIsDefault.UseVisualStyleBackColor = true;
		this.chkIsDefault.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.ChkPreview);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1070, 53);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator3, this.btnsave, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnPrint, this.toolStripSeparator5, this.btnshortcutkeys,
			this.toolStripSeparator2
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1070, 27);
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
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
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
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "1664-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(7, 30);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(211, 19);
		this.label21.TabIndex = 126;
		this.label21.Text = "Bill Of Material (Casting)";
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Checked = true;
		this.ChkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(980, 29);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(84, 20);
		this.ChkPreview.TabIndex = 9;
		this.ChkPreview.Text = "Preview";
		this.ChkPreview.UseVisualStyleBackColor = true;
		this.tabPage3.Controls.Add(this.panel45);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.tabPage3.Controls.Add(this.panel15);
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel38);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1076, 632);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel45.Controls.Add(this.panel8);
		this.panel45.Controls.Add(this.panel19);
		this.panel45.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel45.Location = new System.Drawing.Point(0, 382);
		this.panel45.Name = "panel45";
		this.panel45.Size = new System.Drawing.Size(1076, 250);
		this.panel45.TabIndex = 55;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryOutput);
		this.panel8.Controls.Add(this.panel12);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryOverHead);
		this.panel8.Controls.Add(this.panel42);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(572, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(504, 250);
		this.panel8.TabIndex = 50;
		this.grdHistoryOutput.AllowEdit = (InheritableBoolean)2;
		grdHistoryOutput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryOutput.DesignTimeLayout = grdHistoryOutput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryOutput.DynamicFiltering = true;
		this.grdHistoryOutput.FilterMode = (FilterMode)2;
		this.grdHistoryOutput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryOutput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Location = new System.Drawing.Point(0, 159);
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Name = "grdHistoryOutput";
		((ScrollableBase)this.grdHistoryOutput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Size = new System.Drawing.Size(504, 91);
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).TabIndex = 53;
		this.grdHistoryOutput.TotalRow = (InheritableBoolean)1;
		this.grdHistoryOutput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryOutput).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label15);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 140);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(504, 19);
		this.panel12.TabIndex = 54;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(3, 0);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(172, 16);
		this.label15.TabIndex = 127;
		this.label15.Text = "OutPut by Product/Scrap";
		this.grdHistoryOverHead.AllowEdit = (InheritableBoolean)2;
		grdHistoryOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryOverHead.DesignTimeLayout = grdHistoryOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdHistoryOverHead.DynamicFiltering = true;
		this.grdHistoryOverHead.FilterMode = (FilterMode)2;
		this.grdHistoryOverHead.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Name = "grdHistoryOverHead";
		((ScrollableBase)this.grdHistoryOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Size = new System.Drawing.Size(504, 121);
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).TabIndex = 0;
		this.grdHistoryOverHead.TotalRow = (InheritableBoolean)1;
		this.grdHistoryOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryOverHead).VisualStyle = (VisualStyle)4;
		this.panel42.BackColor = System.Drawing.Color.Teal;
		this.panel42.Controls.Add(this.label34);
		this.panel42.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel42.Location = new System.Drawing.Point(0, 0);
		this.panel42.Name = "panel42";
		this.panel42.Size = new System.Drawing.Size(504, 19);
		this.panel42.TabIndex = 49;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(3, 0);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(78, 16);
		this.label34.TabIndex = 127;
		this.label34.Text = "OverHeads";
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryInput);
		this.panel19.Controls.Add(this.panel44);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(572, 250);
		this.panel19.TabIndex = 4;
		this.grdHistoryInput.AllowEdit = (InheritableBoolean)2;
		grdHistoryInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryInput.DesignTimeLayout = grdHistoryInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryInput.DynamicFiltering = true;
		this.grdHistoryInput.FilterMode = (FilterMode)2;
		this.grdHistoryInput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryInput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Name = "grdHistoryInput";
		((ScrollableBase)this.grdHistoryInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Size = new System.Drawing.Size(572, 231);
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).TabIndex = 0;
		this.grdHistoryInput.TotalRow = (InheritableBoolean)1;
		this.grdHistoryInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryInput).VisualStyle = (VisualStyle)4;
		this.panel44.BackColor = System.Drawing.Color.Teal;
		this.panel44.Controls.Add(this.label16);
		this.panel44.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel44.Location = new System.Drawing.Point(0, 0);
		this.panel44.Name = "panel44";
		this.panel44.Size = new System.Drawing.Size(572, 19);
		this.panel44.TabIndex = 49;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(-1, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(99, 16);
		this.label16.TabIndex = 127;
		this.label16.Text = "Input Material";
		this.DatagridHistory.AllowEdit = (InheritableBoolean)2;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(0, 151);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(1076, 231);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 1;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.DatagridHistory.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		this.DatagridHistory.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DatagridHistory_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label17);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 130);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1076, 21);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1032, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 20);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(4, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(112, 16);
		this.label17.TabIndex = 127;
		this.label17.Text = "Filtered Records";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 53);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1076, 77);
		this.panel13.TabIndex = 0;
		this.groupBox1.Controls.Add(this.txtToDocNoHistory);
		this.groupBox1.Controls.Add(this.txtFromNoHistory);
		this.groupBox1.Controls.Add(this.txtFromdateHistory);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbomtypeHistory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBomNoHistory);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox1.Controls.Add(this.txtToDateHistory);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(886, 72);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(411, 43);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(411, 15);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(94, 43);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.ShowCheckBox = true;
		this.txtFromdateHistory.Size = new System.Drawing.Size(126, 22);
		this.txtFromdateHistory.TabIndex = 1;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.2f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(813, 42);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(58, 25);
		this.btnshow.TabIndex = 8;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(511, 19);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(32, 15);
		this.label30.TabIndex = 178;
		this.label30.Text = "Type";
		this.cmbbomtypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbbomtypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbomtypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbomtypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance156;
		((SpecialBoxBase)((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance161).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbomtypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbomtypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbomtypeHistory).Location = new System.Drawing.Point(567, 13);
		((System.Windows.Forms.Control)(object)this.cmbbomtypeHistory).Name = "cmbbomtypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbbomtypeHistory).Size = new System.Drawing.Size(244, 26);
		((System.Windows.Forms.Control)(object)this.cmbbomtypeHistory).TabIndex = 6;
		this.CmbBomNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBomNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBomNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBomNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance119).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBomNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Location = new System.Drawing.Point(567, 41);
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Name = "CmbBomNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Size = new System.Drawing.Size(244, 26);
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).TabIndex = 5;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(511, 47);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(56, 15);
		this.label20.TabIndex = 176;
		this.label20.Text = "BOM No.";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(5, 19);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 174;
		this.label19.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(94, 13);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(253, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(221, 43);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(126, 22);
		this.txtToDateHistory.TabIndex = 2;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(4, 47);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(91, 15);
		this.label28.TabIndex = 171;
		this.label28.Text = "Date From && To";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(351, 47);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(45, 15);
		this.label4.TabIndex = 173;
		this.label4.Text = "Doc To";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(351, 19);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(60, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From";
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.toolStrip2);
		this.panel38.Controls.Add(this.label32);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(1076, 53);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator1, this.btnRegister });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1076, 27);
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
		this.btnRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(61, 24);
		this.btnRegister.Text = "&Print";
		this.btnRegister.Visible = false;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(4, 30);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(301, 18);
		this.label32.TabIndex = 127;
		this.label32.Text = "Bill Of Material Manufacturing History  ";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1084, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmBillOfMaterialCasting";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmBillOfMaterialCasting_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterialCasting_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOutPutByProduct).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInputMaterial).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel17.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCastingType).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBOMType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.panel45.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOverHead).EndInit();
		this.panel42.ResumeLayout(false);
		this.panel42.PerformLayout();
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).EndInit();
		this.panel44.ResumeLayout(false);
		this.panel44.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbomtypeHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBomNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
