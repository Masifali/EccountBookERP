using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Infragistics.Shared;
using Infragistics.Win.UltraWinGrid;

namespace Architecture.WinApp.CommissionAgent.Helper_Methods;

public class CommonBindings
{
	public static void ParentCategoryBindFromGlobal(UltraCombo cmbParentItem)
	{
		try
		{
			List<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems;
			if (!allItems.Any())
			{
				return;
			}
			DataTable dtParentCategory = new DataTable();
			dtParentCategory.Columns.Add("Id", typeof(int));
			dtParentCategory.Columns.Add("Description", typeof(string));
			HashSet<string> parentSet = new HashSet<string>();
			foreach (getGlobalAllItems item in allItems)
			{
				if (!string.IsNullOrEmpty(item.InvParentCateDescription) && parentSet.Add(item.InvParentCateDescription))
				{
					dtParentCategory.Rows.Add(item.InventoryParentCategoriesId, item.InvParentCateDescription);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbParentItem, dtParentCategory, "Id", "Description", "Parent Item", AllColumns: true, Conversion.ToInt(cmbParentItem.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading categories: " + ex.Message);
		}
	}

	public static void ItemdtFillFromGlobal(DataTable dtItem, int ParentCategoryId = 0)
	{
		try
		{
			if (dtItem != null && dtItem.Columns.Count == 0)
			{
				dtItem.Columns.Add("Id", typeof(int));
				dtItem.Columns.Add("ItemName");
				dtItem.Columns.Add("ItemCode");
				dtItem.Columns.Add("InventoryParentCategoriesId");
				dtItem.Columns.Add("ItemCategoryId");
				dtItem.Columns.Add("ItemCategory");
			}
			dtItem.Rows.Clear();
			List<getGlobalAllItems> dtItemAll = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems row) => ParentCategoryId == 0 || row.InventoryParentCategoriesId == ParentCategoryId).ToList();
			if (dtItemAll == null || dtItemAll.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllItems row2 in dtItemAll)
			{
				dtItem.Rows.Add(row2.Id, row2.ItemName, row2.ItemCode, row2.InventoryParentCategoriesId, row2.ItemCategoryId, row2.ItemCategory);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void CropDtFillFromGlobalAndBind(DataTable dtCrop, UltraCombo CmbCropYear)
	{
		try
		{
			if (dtCrop != null && dtCrop.Columns.Count == 0)
			{
				dtCrop.Columns.Add("Id", typeof(int));
				dtCrop.Columns.Add("CropYear");
			}
			dtCrop.Clear();
			List<GlobalCropYear> lst = clsGlobalVariables.globalCropYear;
			DatatableHelper.PopulateDataTable(dtCrop, lst, (GlobalCropYear x) => x.Id, (GlobalCropYear x) => x.CropYear);
			int Id = Conversion.ToInt(CmbCropYear.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbCropYear, dtCrop, "Id", "CropYear", "Crop Year", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PackingTypeDtFillFromGlobalAndBind(DataTable dtPackType, UltraCombo CmbPackingType)
	{
		try
		{
			if (dtPackType != null && dtPackType.Columns.Count == 0)
			{
				dtPackType.Columns.Add("Id", typeof(int));
				dtPackType.Columns.Add("PackTypeDesc");
			}
			dtPackType.Clear();
			List<InvPackingType> Finallist = clsGlobalVariables.globalInvPackingType;
			DatatableHelper.PopulateDataTable(dtPackType, Finallist, (InvPackingType x) => x.Id, (InvPackingType x) => x.PackTypeDesc);
			int Id = Conversion.ToInt(CmbPackingType.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbPackingType, dtPackType, "Id", "PackTypeDesc", "Packing Type", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SupplierDtFillFromGlobal(DataTable dtSupplier, bool useBusinessName, RadioButton radPartyCode = null)
	{
		try
		{
			if (dtSupplier != null && dtSupplier.Columns.Count == 0)
			{
				dtSupplier.Columns.Add("Id", typeof(int));
				dtSupplier.Columns.Add("CompanyName", typeof(string));
				dtSupplier.Columns.Add("PartyCode", typeof(string));
				dtSupplier.Columns.Add("GlAccountId", typeof(int));
				dtSupplier.Columns.Add("CityName", typeof(string));
				dtSupplier.Columns.Add("MobileNo", typeof(string));
			}
			dtSupplier.Clear();
			List<getGlobalAllSupplierCustomer> customers = clsGlobalVariables.globalAllSupplierCustomer;
			if (customers == null || customers.Count == 0)
			{
				return;
			}
			Dictionary<int, getGlobalAllSupplierCustomer> customerLookup = customers.ToDictionary((getGlobalAllSupplierCustomer x) => x.Id);
			dtSupplier.BeginLoadData();
			foreach (getGlobalAllSupplierCustomer item in customers)
			{
				if (radPartyCode != null && radPartyCode.Checked)
				{
					useBusinessName = true;
				}
				string name = (useBusinessName ? (item.CompanyName ?? string.Empty) : (item.NickName ?? string.Empty));
				if (item.IsSubSupCust && item.ParentsSupCustId > 0 && customerLookup.TryGetValue(item.ParentsSupCustId, out var parent))
				{
					name = (useBusinessName ? (parent.CompanyName ?? string.Empty) : (parent.NickName ?? string.Empty)) + " / " + name;
				}
				dtSupplier.Rows.Add(item.Id, name, item.PartyCode, item.GlAccountId, item.CityName, item.MobilePersonal);
			}
			dtSupplier.EndLoadData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SupplierBind(UltraCombo ultraCombo, DataTable dataTable, string caption, RadioButton radPartyCode = null)
	{
		try
		{
			int num;
			object obj;
			if (radPartyCode != null)
			{
				num = (radPartyCode.Checked ? 1 : 0);
				if (num != 0)
				{
					obj = "PartyCode";
					goto IL_001b;
				}
			}
			else
			{
				num = 0;
			}
			obj = "CompanyName";
			goto IL_001b;
			IL_001b:
			string valueMember = (string)obj;
			string displayMember = ((num != 0) ? "Party Code" : caption);
			int previousValue = 0;
			if (ultraCombo.Value != null)
			{
				int.TryParse(ultraCombo.Value.ToString(), out previousValue);
			}
			InfragisticsHelper.BindAndRetainSelection(ultraCombo, dataTable, "Id", valueMember, displayMember, AllColumns: true, previousValue, insertDefaultRow: false, null, null, ActivateRow: false);
			if (((DisposableObjectCollectionBase)((UltraGridBase)ultraCombo).DisplayLayout.Bands).Count > 0 && ((DisposableObjectCollectionBase)((UltraGridBase)ultraCombo).DisplayLayout.Bands[0].Columns).Count > 3)
			{
				((UltraGridBase)ultraCombo).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ItemUomFromGlobalBind(int itemId, UltraCombo cmbPackUom = null, UltraCombo cmbRateUom = null, UltraCombo CmbSecondaryUom = null, bool setBaseValues = true)
	{
		string packUomText = ((Control)(object)cmbPackUom)?.Text;
		string rateUomText = ((Control)(object)cmbRateUom)?.Text;
		string SecondaryUomText = ((Control)(object)CmbSecondaryUom)?.Text;
		DataTable dtUOM = CommonServices.dtUomFromGloablUomScheduleByItemId(itemId, addBaseSecondaryUom: true);
		if (cmbPackUom != null)
		{
			InfragisticsHelper.BindAndRetainSelection(cmbPackUom, dtUOM, "Id", "UOMCode", "Pack Uom", AllColumns: true, packUomText, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
		}
		if (cmbRateUom != null)
		{
			InfragisticsHelper.BindAndRetainSelection(cmbRateUom, dtUOM, "Id", "UOMCode", "Rate Uom", AllColumns: true, rateUomText, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
		}
		if (CmbSecondaryUom != null)
		{
			InfragisticsHelper.BindAndRetainSelection(CmbSecondaryUom, dtUOM, "Id", "UOMCode", "Secondary Uom", AllColumns: true, SecondaryUomText, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
		}
		if (dtUOM != null && dtUOM.Rows.Count != 0 && setBaseValues)
		{
			EnumerableRowCollection<DataRow> source = dtUOM.AsEnumerable();
			DataRow basePackRow = source.FirstOrDefault((DataRow r) => r.Field<bool?>("BasePackUom") == true);
			DataRow baseRateRow = source.FirstOrDefault((DataRow r) => r.Field<bool?>("BaseRateUom") == true);
			DataRow baseSecondaryRow = source.FirstOrDefault((DataRow r) => r.Field<bool?>("BaseSecondaryUom") == true);
			if (cmbPackUom != null && basePackRow != null)
			{
				cmbPackUom.Value = basePackRow.Field<int>("Id");
			}
			if (cmbRateUom != null && baseRateRow != null)
			{
				cmbRateUom.Value = baseRateRow.Field<int>("Id");
			}
			if (CmbSecondaryUom != null && baseSecondaryRow != null)
			{
				CmbSecondaryUom.Value = baseSecondaryRow.Field<int>("Id");
			}
		}
	}

	public static void ItemConditionBindFromGlobal(DataTable dtCondition, UltraCombo CmbItemCondition)
	{
		try
		{
			if (dtCondition != null && dtCondition.Columns.Count == 0)
			{
				dtCondition.Columns.Add("Id", typeof(int));
				dtCondition.Columns.Add("ItemCondition");
			}
			dtCondition.Clear();
			DatatableHelper.PopulateDataTable(dtCondition, clsGlobalVariables.globalItemConditions, (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
			InfragisticsHelper.BindAndRetainSelection(CmbItemCondition, dtCondition, "Id", "ItemCondition", "Item Condition", AllColumns: true, Conversion.ToInt(CmbItemCondition.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static DataTable JoblotBindFromGlobal(DataTable dtJobLot = null, UltraCombo cmbJobLot = null, string valueMember = "Id", string displayMember = "JobLot", string caption = "Job Lot")
	{
		try
		{
			if (dtJobLot == null)
			{
				dtJobLot = new DataTable();
			}
			if (dtJobLot.Columns.Count == 0)
			{
				dtJobLot.Columns.Add(valueMember, typeof(int));
				dtJobLot.Columns.Add(displayMember, typeof(string));
			}
			dtJobLot.Clear();
			List<GetJobLotGlIdsandName> lst = clsGlobalVariables.globalJobLot;
			if (lst != null && lst.Count > 0)
			{
				DatatableHelper.PopulateDataTable(dtJobLot, lst, (GetJobLotGlIdsandName x) => x.Id, (GetJobLotGlIdsandName x) => x.JobLotDescription);
			}
			if (cmbJobLot != null)
			{
				int selectedValue = 0;
				if (cmbJobLot.Value != null && cmbJobLot.Value != DBNull.Value)
				{
					selectedValue = Conversion.ToInt(cmbJobLot.Value);
				}
				InfragisticsHelper.BindAndRetainSelection(cmbJobLot, dtJobLot, valueMember, displayMember, caption, AllColumns: true, selectedValue);
			}
			return dtJobLot;
		}
		catch (Exception ex)
		{
			MessageBox.Show("Job Lot not found: " + ex.Message);
			return dtJobLot;
		}
	}

	public static void BindShipToAddressAgainstBuyer(UltraCombo CmbShipToAddress, DataTable dtShipToAddress, int BuyerId)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Address", typeof(string));
			dt.Columns.Add("PartyId", typeof(int));
			dt.Columns.Add("PartyName", typeof(string));
			EnumerableRowCollection<DataRow> rows = from r in dtShipToAddress.AsEnumerable()
				where BuyerId == 0 || Conversion.ToInt(r["SupplierCustomerId"]) == BuyerId
				select r;
			DataTable dtResult = (rows.Any() ? rows.CopyToDataTable() : dtShipToAddress.Clone());
			if (dtResult.Rows.Count > 0)
			{
				for (int i = 0; i < dtResult.Rows.Count; i++)
				{
					dt.Rows.Add(dtResult.Rows[i]["Id"], dtResult.Rows[i]["AddressLine1"], dtResult.Rows[i]["SupplierCustomerId"], dtResult.Rows[i]["CompanyName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbShipToAddress, dt, "Id", "Address", "Ship To Address", AllColumns: true, Conversion.ToInt(CmbShipToAddress.Value), insertDefaultRow: false, null, null, dt.Rows.Count == 1);
			if (dt != null && dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbShipToAddress).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
