package com.mst.repositories;

import com.mst.models.UserAccount;
import org.springframework.stereotype.Repository;

import java.time.LocalDateTime;
import java.util.*;

import static com.mst.repositories.SaleInvoiceRepository.*;

/**
 * Screen 145 "Sale Invoice Direct 139" (frmSaleInvoiceDirect139, DocumentTypeId 139) - the reads the form makes through
 * CommonServices / BLL, each with the procedure, parameters and Activity the desktop sends. Writes go through
 * SaleInvoiceDirectPersist.persist (BLL 0580 Save / DAL 0433 SetData).
 */
@Repository
public class SaleInvoiceDirect139Repository {
    public static final int DOC = 139;
    public static final String SCREEN = "frmSaleInvoiceDirect139";

    private final SaleInvoiceRepository si;

    public SaleInvoiceDirect139Repository(SaleInvoiceRepository si) { this.si = si; }

    /** suppliercustomer() :436 - CommonServices.SupplierCustomerGetAllServiceBind -> SupplierCustomer.Getall (BLL 0600:110). */
    public List<Map<String, Object>> customers(UserAccount u) {
        return si.proc("Sp_SupplierCustomer_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "ReadByOrganizationCompanyId"));
    }

    /** item() :455 - CommonServices.ReadAllItems -> Item.ReadAllItems (BLL 0583:246), ItemCategoryId 0 so no category filter. */
    public List<Map<String, Object>> items(UserAccount u) {
        return si.proc("Sp_Item_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAllItems"));
    }

    /** PaymentTerms() :472 - CommonServices.GetDueTermServiceBind -> InvDueTerms.GetAll. */
    public List<Map<String, Object>> dueTerms(UserAccount u) {
        return si.proc("Sp_InvDueTerms_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll"));
    }

    /** PackingType() :507 - InvPackingType.Getall (Activity 'ReadAll', no organization parameters). */
    public List<Map<String, Object>> packingTypes() {
        return si.proc("Sp_InvPackingType_GetAllMethod", params("Activity", "ReadAll"));
    }

    /** JobLotFill() :523 - CommonServices.JobLotGetAllService -> jobLot.GetAll (SP_JobLot_ReadMethod 'GetAll'). */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        return si.proc("SP_JobLot_ReadMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll"));
    }

    /** WareHouseFill() :539 - CommonServices.getActiveWareHouse -> InvWareHouse.GetActiveWareHouse. */
    public List<Map<String, Object>> warehouses(UserAccount u) {
        return si.proc("Sp_InvWareHouse_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetActiveWareHouse"));
    }

    /** OtherItemsBind() :555 - InventoryItemsOther.GetAll. */
    public List<Map<String, Object>> otherItems(UserAccount u) { return si.otherItems(u); }

    /** PackUOM() :586 - UOMSchedule.SearchByObject (Sp_UOMSchedule_GetAllMethod 'ReadByItemID'). */
    public List<Map<String, Object>> uomsOfItem(UserAccount u, int itemId) {
        return si.proc("Sp_UOMSchedule_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "Activity", "ReadByItemID"));
    }

    /** AvailableStockGetByItem() :663 - InventoryStockEvalautionDetail.GetCurrentStockByItemId (BLL 0574:870); WarehouseId, JobLotId, CropYear stay unset. */
    public double currentStock(UserAccount u, int itemId, LocalDateTime docDate, int packingTypeId, int stockUom) {
        var rows = si.proc("Sp_SaleOrder_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "DocDateTo", docDate, "WareHouseId", 0, "JobLotId", 0, "InvPackingTypeId", packingTypeId, "ItemUomId", stockUom,
                "Activity", "GetCurrentStockByItemId"));
        return rows.isEmpty() ? 0d : d(col(rows.get(0), "AvailableStock"));
    }

    /** GetAll() :1390 - InvSaleInvoice.FormHistory (BLL 0580): no dates, NoOfRecords only when not 0, EntryUser only without CanViewAllRecord. */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId, boolean canViewAll, int noOfRecords) {
        LinkedHashMap<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC, "CanViewAllRecord", canViewAll);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (noOfRecords != 0) p.put("NoOfRecords", noOfRecords);
        if (!canViewAll) p.put("EntryUser", u.getId());
        p.put("Activity", "FormHistory");
        return si.proc("[Sp_InvSaleInvoice_GetAllMethod]", p);
    }
}
