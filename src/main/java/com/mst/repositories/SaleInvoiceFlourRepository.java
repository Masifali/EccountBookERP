package com.mst.repositories;

import com.mst.models.UserAccount;
import org.springframework.stereotype.Repository;

import java.time.LocalDateTime;
import java.util.*;

import static com.mst.repositories.SaleInvoiceRepository.*;

/**
 * Reads of the two flour direct sale invoice forms:
 *   screen 137 frmSaleInvoiceDirect          (DocumentTypeId 186)
 *   screen 138 frmSaleInvoiceDirectAutoRated (DocumentTypeId 184)
 * The lists both forms share with frmSaleInvoiceDirect139 (customers, due terms, packing types, warehouses, job lots, other
 * items, pack uoms) come from {@link SaleInvoiceDirect139Repository}, which calls the same CommonServices methods.
 */
@Repository
public class SaleInvoiceFlourRepository {
    public static final int DOC_FLOUR = 186;
    public static final int DOC_AUTO_RATED = 184;
    public static final String SCREEN_FLOUR = "frmSaleInvoiceDirect";
    public static final String SCREEN_AUTO_RATED = "frmSaleInvoiceDirectAutoRated";

    private final SaleInvoiceRepository si;

    public SaleInvoiceFlourRepository(SaleInvoiceRepository si) { this.si = si; }

    /** BinCity() - CommonServices.CityGetAllService -> City.GetAll (SP_City_GetAllMethod 'GetAll'). */
    public List<Map<String, Object>> cities(UserAccount u) {
        return si.proc("SP_City_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll"));
    }

    /** cropyear() - CommonServices.CropYearGetAllService -> InvCropYear.Getall (Sp_InvCropYear_GetAllMethod 'ReadAll'). */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        return si.proc("Sp_InvCropYear_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** Project() - CommonServices.ProjectServiceBind -> Projects.GetAlldt (Sp_Projects_GetAllMethod 'GetAll'). */
    public List<Map<String, Object>> projects(UserAccount u) {
        return si.proc("Sp_Projects_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll"));
    }

    /** braches() - CommonServices.BrancheServiceBind -> Branches.GetAll (Sp_Branches_GetAllMethod 'GetAll'). */
    public List<Map<String, Object>> branches(UserAccount u) {
        return si.proc("Sp_Branches_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll"));
    }

    /** ItemNameFill() of 137 - CommonServices.ItemGetAllServiceBind -> Item.GetAll (Sp_Item_GetAllMethod 'ReadByOrganizationCompanyId'); Id, ItemName, ItemCode. */
    public List<Map<String, Object>> itemsFlour(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : si.proc("Sp_Item_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "ReadByOrganizationCompanyId"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i(col(r, "Id"))); m.put("ItemName", s(col(r, "ItemName"))); m.put("ItemCode", s(col(r, "ItemCode")));
            out.add(m);
        }
        return out;
    }

    /** ItemNameFill() of 138 - ItemPricingScheduleForRice.AllItemsFillFromPricingSchedule(DocumentTypeId 183); Id, ItemName, ItemCodeNew as ItemCode. */
    public List<Map<String, Object>> itemsAutoRated(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : si.proc("Sp_ItemPricingScheduleForRice_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", 183, "Activity", "AllItemsFillFromPricingSchedule"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i(col(r, "Id"))); m.put("ItemName", s(col(r, "ItemName"))); m.put("ItemCode", s(col(r, "ItemCodeNew")));
            out.add(m);
        }
        return out;
    }

    /** RateUOMFromSchedule() (138) - ItemPricingScheduleForRice.GetRateUOMFromItemPriceScheduleByItemId: RateUomId, UOMCode, Equivalent. */
    public List<Map<String, Object>> rateUomsFromSchedule(UserAccount u, int itemId, LocalDateTime docDate) {
        return si.proc("[dbo].[Sp_ItemPricingSchedule_GetAllMethod]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "EffectiveDate", docDate, "Activity", "GetRateUOMFromItemPriceScheduleByItemId"));
    }

    /** GetRateFromItemPriceScheduleByItemId (138) - first row's ItemRate, else 0. */
    public double rateFromSchedule(UserAccount u, int itemId, LocalDateTime docDate, int rateUomId) {
        var rows = si.proc("[dbo].[Sp_ItemPricingSchedule_GetAllMethod]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "RateUomId", rateUomId, "ItemId", itemId, "EffectiveDate", docDate, "Activity", "GetRateFromItemPriceScheduleByItemId"));
        return rows.isEmpty() ? 0d : d(col(rows.get(0), "ItemRate"));
    }

    /** AvailableStockGetByItem() - InventoryStockEvalautionDetail.GetCurrentStockByItemId (BLL 0574:870); every parameter is sent. */
    public double currentStock(UserAccount u, int itemId, LocalDateTime docDate, int warehouseId, int jobLotId, String cropYear, int packingTypeId, int stockUom) {
        var rows = si.proc("Sp_SaleOrder_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "DocDateTo", docDate, "WareHouseId", warehouseId, "JobLotId", jobLotId, "CropYear", cropYear == null ? "" : cropYear,
                "InvPackingTypeId", packingTypeId, "ItemUomId", stockUom, "Activity", "GetCurrentStockByItemId"));
        return rows.isEmpty() ? 0d : d(col(rows.get(0), "AvailableStock"));
    }

    /** HistoryComboBind() - InvSaleInvoice.AllComboBindAgainstSaleInvoice(DocumentTypeIds), the 'Supplier' rows. */
    public List<Map<String, Object>> historyCustomers(UserAccount u, int doc) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : si.proc("[dbo].[Usp_AllComboAgainstSaleInvoice]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "AppId", n(u.getAppId()), "UserId", u.getId(), "DocumentTypeIds", String.valueOf(doc)))) {
            if (!"Supplier".equals(s(col(r, "Activity")))) continue;
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", i(col(r, "Id"))); x.put("CustomerName", s(col(r, "ReferenceName")));
            out.add(x);
        }
        return out;
    }

    /** GetAll() - InvSaleInvoice.FormHistory (BLL 0580): dates by the picked date type, doc-no range, customer; EntryUser only without CanViewAllRecord. */
    public List<Map<String, Object>> history(UserAccount u, int doc, int financialYearId, boolean canViewAll, String dateMode,
                                             LocalDateTime from, LocalDateTime to, int fromDocNo, int toDocNo, int customerId) {
        LinkedHashMap<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", doc, "CanViewAllRecord", canViewAll);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        String fromKey, toKey;
        switch (dateMode == null ? "" : dateMode) {
            case "entry": fromKey = "EntryFromDate"; toKey = "EntryToDate"; break;
            case "modify": fromKey = "ModifyFromDate"; toKey = "ModifyToDate"; break;
            case "approved": fromKey = "ApprovedFromDate"; toKey = "ApprovedToDate"; break;
            default: fromKey = "FromDate"; toKey = "ToDate"; break;
        }
        if (from != null) p.put(fromKey, from);
        if (to != null) p.put(toKey, to);
        if (fromDocNo != 0) p.put("DocNoFrom", fromDocNo);
        if (toDocNo != 0) p.put("DocNoTo", toDocNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (!canViewAll) p.put("EntryUser", u.getId());
        p.put("Activity", "FormHistory");
        return si.proc("[Sp_InvSaleInvoice_GetAllMethod]", p);
    }
}
