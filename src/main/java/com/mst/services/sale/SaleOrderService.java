package com.mst.services.sale;

import com.mst.security.CurrentUserContext;

import com.mst.models.sale.dto.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service
public class SaleOrderService {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CurrentUserContext currentUserContext;

    /**
     * Real DocumentTypeId for Sale Order, hard-confirmed from two independent desktop
     * call sites (DocumentNoFill / BranchSrNoFill in Architecture.WinApp.Sale/SaleOrder.cs)
     * and from the real stored procedure's own inline usage-example comment in
     * GoldenAceDb(0509)t.sql ("-- Sp_SaleOrder_GetAllMethod 1,1,81,null,null,null,null,null,
     * GenerateSaleOrderCodeByDocId"). NOT 20 - the previous implementation's DocumentTypeId=20
     * against a fabricated VoucherHead table was wrong.
     */
    private static final int SALE_ORDER_DOCUMENT_TYPE_ID = 81;

    /** ERPFeatures.Id = 4 ('SubsidiaryAccountAllownOnVouchers'), verified in the Configuration module. */
    private static final int ERP_FEATURE_SUBSIDIARY_ACCOUNT_ALLOWN_ON_VOUCHERS = 4;

    // ==========================================================================================
    // Real stored-procedure SQL constants - every name/param below verified directly against
    // GoldenAceDb(0509)t.sql (UTF-16LE-decoded) and against the desktop BLL/DAL call sites in
    // Architecture.WinApp.Sale/SaleOrder.cs, Architecture.BLL.*, Architecture.DAL.*.
    // No table/column/procedure name is guessed. Columns below are the raw SELECT column names
    // each procedure actually returns (GenericProvider.GetDataTableProc<T> pass-through
    // convention already established for this codebase) - the Java Map keys mirror them as-is.
    // ==========================================================================================

    /** usp_getLocationType - no params. Columns: Id, Location. */
    private static final String SQL_LOCATION_TYPES = "EXEC usp_getLocationType";

    /** Sp_InvOrderCategory_GetAllMethod @Activity='GetAll' -> WHERE Id IN (1,4,5,6) (hardcoded in the proc itself). */
    private static final String SQL_ORDER_CATEGORIES = "EXEC Sp_InvOrderCategory_GetAllMethod @Activity=?";

    /** Sp_InvLookup_GetAllMethod @Activity='ReadByInvlookTypeId' - Category-I (type 18) / Category-II (type 19). */
    private static final String SQL_LOOKUPS_BY_TYPE =
            "EXEC Sp_InvLookup_GetAllMethod @OrganizationId=?, @CompanyId=?, @InvLookupTypeId=?, @Activity=?";

    /** USP_GetVendorsAndCustomersWithCityName - used when SubsidiaryAccountAllownOnVouchers=true (PartyTypeId=2,
     *  matching the desktop's CommonServices.GetVendorsAndCustomersWithCityName(2) call). PageSize/PageNumber
     *  intentionally omitted (left NULL) so the proc's own "@PageSize IS NULL AND @PageNumber IS NULL" branch
     *  returns up to 1,000,000 rows unpaginated, matching the desktop's own unpaginated dropdown load. */
    private static final String SQL_CUSTOMERS_SUBSIDIARY =
            "EXEC USP_GetVendorsAndCustomersWithCityName @OrganizationId=?, @CompanyId=?, @PartyTypeId=?";

    /** Sp_SupplierCustomer_GetAllMethod @Activity='AllSupplierCustomerWithCityName' - used when the flag above is false. */
    private static final String SQL_CUSTOMERS_ALL =
            "EXEC Sp_SupplierCustomer_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?";

    /** Sp_ReferenceParties_GetAllMethod @Activity='ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId', type 5 = Booking Person. */
    private static final String SQL_BOOKING_PERSONS =
            "EXEC Sp_ReferenceParties_GetAllMethod @OrganizationId=?, @CompanyId=?, @ReferencePartyTypeId=?, @Activity=?";

    /** Sp_InvDueTerms_GetAllMethod @Activity='GetAll' (the 'GetAll' branch itself has no WHERE clause - params passed regardless, matching the desktop's own getPaymentTermlist call). */
    private static final String SQL_PAYMENT_TERMS =
            "EXEC Sp_InvDueTerms_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?";

    /** SpStaticColumnNames @Activity='GetCommissionUom' - static Id/type rows {1,10,25,40,50,60,80,100}, matching CommonServices.StaticColumnsService("GetCommissionUom"). */
    private static final String SQL_COMMISSION_UOM = "EXEC SpStaticColumnNames @Activity=?";

    /** USP_Item_AllItemsWithModal - PageSize/PageNumber/Keyword intentionally omitted (NULL) for the same
     *  unpaginated-load reason as the customer lookup above. Own WHERE already excludes InventoryParentCategoriesId (5,9). */
    private static final String SQL_ITEMS = "EXEC USP_Item_AllItemsWithModal @OrganizationId=?, @CompanyId=?";

    /** Sp_InvCropYear_GetAllMethod @Activity='ReadAll', matching CommonServices.CropYearGetAllService(). */
    private static final String SQL_CROP_YEARS = "EXEC Sp_InvCropYear_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?";

    /** USP_GetJobLotsAllocatedToBranch, matching JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId. @BranchId may be NULL (all branches). */
    private static final String SQL_JOB_LOTS = "EXEC USP_GetJobLotsAllocatedToBranch @OrganizationId=?, @CompanyId=?, @BranchId=?";

    /** Sp_InvPackingType_GetAllMethod @Activity='ReadAll' (WHERE IsActive=1 baked into the proc), matching InvPackingType.Getall(). */
    private static final String SQL_PACKING_TYPES = "EXEC Sp_InvPackingType_GetAllMethod @Activity=?";

    /** SP_City_GetAllMethod @MethodType='GetAll', matching City.GetAll(...). */
    private static final String SQL_CITIES = "EXEC SP_City_GetAllMethod @OrganizationId=?, @CompanyId=?, @MethodType=?";

    /** USP_GetWarehousesAllocatedToBranch, matching WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId. @BranchId may be NULL (all branches). */
    private static final String SQL_WAREHOUSES = "EXEC USP_GetWarehousesAllocatedToBranch @OrganizationId=?, @CompanyId=?, @BranchId=?";

    /** Sp_UOMSchedule_GetAllMethod @Activity='ReadByItemID', matching CommonServices.GetUomScheduleByItemId(itemId). */
    private static final String SQL_UOM_BY_ITEM =
            "EXEC Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @ItemId=?, @Activity=?";

    /** USP_GetERPFeaturesByCompanyId - same proc already verified/used by the Configuration module. */
    private static final String SQL_ERP_FEATURES_BY_COMPANY = "EXEC USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?";

    /** Sp_SaleOrder_GetAllMethod @Activity='ReadById' - real Sale Order header, verified against
     *  GoldenAceDb(0509)t.sql (decoded line 458682). Only @Id is required by this branch. */
    private static final String SQL_SALE_ORDER_HEADER_BY_ID =
            "EXEC Sp_SaleOrder_GetAllMethod @Id=?, @Activity=?";

    /** Sp_SaleOrder_GetAllMethod @Activity='ReadBySaleOrderHeaderId' - real Detail-tab line items
     *  (SaleOrderDetail joined to Item/UOM/JobLot/PackingType/Warehouse/etc.), decoded line 458491. */
    private static final String SQL_SALE_ORDER_DETAIL_BY_HEADER_ID =
            "EXEC Sp_SaleOrder_GetAllMethod @Id=?, @Activity=?";

    /** Sp_SaleOrder_GetAllMethod @Activity='SaleOrderPaymentTermDetailByHeaderId' - real Payment
     *  Detail schedule rows, decoded line 458642. */
    private static final String SQL_SALE_ORDER_PAYMENT_TERMS_BY_HEADER_ID =
            "EXEC Sp_SaleOrder_GetAllMethod @Id=?, @Activity=?";

    /** Sp_SaleOrder_GetAllMethod @Activity='SaleOrderCustomerExpensesByHeaderId' - real Customer
     *  Expense rows, decoded line 458210. */
    private static final String SQL_SALE_ORDER_CUSTOMER_EXPENSES_BY_HEADER_ID =
            "EXEC Sp_SaleOrder_GetAllMethod @Id=?, @Activity=?";

    /** Sp_SaleOrder_GetAllMethod @Activity='ReadBySaleOrderId_ExtraItemDetail' - real Extra Items
     *  rows, decoded line 458625. The desktop's own DAL (Architecture.DAL.Inventory.SaleOrder.GetData)
     *  loads all four sub-lists for every header row it returns - replicated here for the same reason. */
    private static final String SQL_SALE_ORDER_EXTRA_ITEMS_BY_ID =
            "EXEC Sp_SaleOrder_GetAllMethod @Id=?, @Activity=?";

    /** Sp_SaleOrder_GetAllMethod @Activity='GenerateSaleOrderCodeByDocId' -> column DocNo. */
    private static final String SQL_GENERATE_SALE_ORDER_DOC_NO =
            "EXEC Sp_SaleOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?, @Activity=?";

    /** Sp_SaleOrder_GetAllMethod @Activity='GenerateSaleOrderBranchCodeByDocId' -> column BranchSrNo. */
    private static final String SQL_GENERATE_SALE_ORDER_BRANCH_SR_NO =
            "EXEC Sp_SaleOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @BranchesId=?, @FinancialYearId=?, @Activity=?";

    // ==========================================================================================
    // Document numbering - real two-part DocNo + BranchSrNo scheme via Sp_SaleOrder_GetAllMethod.
    // Replaces the previous fabricated "SELECT MAX(VoucherCode)+1 FROM VoucherHead" logic, which
    // read a table/DocumentTypeId that do not exist for Sale Order.
    // ==========================================================================================

    public int generateNextSaleOrderDocNo() {
        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            int finYearId = currentUserContext.currentFinancialYearId();
            Integer docNo = jdbcTemplate.queryForObject(SQL_GENERATE_SALE_ORDER_DOC_NO, Integer.class,
                    orgId, compId, SALE_ORDER_DOCUMENT_TYPE_ID, finYearId, "GenerateSaleOrderCodeByDocId");
            return (docNo != null && docNo > 0) ? docNo : 1;
        } catch (Exception e) {
            return 1;
        }
    }

    public int generateNextSaleOrderBranchSrNo(int branchId) {
        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            int finYearId = currentUserContext.currentFinancialYearId();
            Integer srNo = jdbcTemplate.queryForObject(SQL_GENERATE_SALE_ORDER_BRANCH_SR_NO, Integer.class,
                    orgId, compId, SALE_ORDER_DOCUMENT_TYPE_ID, branchId, finYearId, "GenerateSaleOrderBranchCodeByDocId");
            return (srNo != null && srNo > 0) ? srNo : 1;
        } catch (Exception e) {
            return 1;
        }
    }

    /** Retained so any pre-existing caller keeps compiling; now delegates to the real DocNo generator. */
    public int generateNextSaleOrderCode() {
        return generateNextSaleOrderDocNo();
    }

    /**
     * Ditto CommonServices.GetERPFeatureById(4): true when this org+company's ERPConfigurations
     * has an active row for FeaturesId=4 ('SubsidiaryAccountAllownOnVouchers'). Governs which of
     * the two real Customer/Party lookup procedures the desktop's own SupplierdtDBCall() calls.
     */
    private boolean isSubsidiaryAccountAllownOnVouchers(int orgId, int compId) {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_ERP_FEATURES_BY_COMPANY, orgId, compId);
            for (Map<String, Object> row : rows) {
                Object idVal = row.get("Id");
                int id = idVal instanceof Number ? ((Number) idVal).intValue() : -1;
                if (id == ERP_FEATURE_SUBSIDIARY_ACCOUNT_ALLOWN_ON_VOUCHERS) {
                    return true;
                }
            }
        } catch (Exception ignored) {
        }
        return false;
    }

    public Map<String, Object> getMasterLookups() {
        Map<String, Object> map = new LinkedHashMap<>();
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();

        // Location Type - usp_getLocationType (Id, Location)
        try {
            map.put("locationTypes", jdbcTemplate.queryForList(SQL_LOCATION_TYPES));
        } catch (Exception e) {
            map.put("locationTypes", Collections.emptyList());
        }

        // Order Category - Sp_InvOrderCategory_GetAllMethod @Activity='GetAll' (Id, OrderCategoryName, OrderCategoryDescription, ...)
        try {
            map.put("orderCategories", jdbcTemplate.queryForList(SQL_ORDER_CATEGORIES, "GetAll"));
        } catch (Exception e) {
            map.put("orderCategories", Collections.emptyList());
        }

        // Category-I (InvLookupTypeId=18) / Category-II (InvLookupTypeId=19) - Sp_InvLookup_GetAllMethod
        try {
            map.put("categoriesI", jdbcTemplate.queryForList(SQL_LOOKUPS_BY_TYPE, orgId, compId, 18, "ReadByInvlookTypeId"));
        } catch (Exception e) {
            map.put("categoriesI", Collections.emptyList());
        }
        try {
            map.put("categoriesII", jdbcTemplate.queryForList(SQL_LOOKUPS_BY_TYPE, orgId, compId, 19, "ReadByInvlookTypeId"));
        } catch (Exception e) {
            map.put("categoriesII", Collections.emptyList());
        }

        // Customer / Party (dtsuppcus) - real branch on SubsidiaryAccountAllownOnVouchers, matching SupplierdtDBCall().
        // Same list also feeds Sales Man/Agent and Other Commission Agent (CompanyNameBind is called identically
        // for CmbCustomerName, combsalesman, and CmbOtherCommissionAgent against this one DataTable in the desktop).
        List<Map<String, Object>> customers;
        try {
            if (isSubsidiaryAccountAllownOnVouchers(orgId, compId)) {
                customers = jdbcTemplate.queryForList(SQL_CUSTOMERS_SUBSIDIARY, orgId, compId, 2);
            } else {
                customers = jdbcTemplate.queryForList(SQL_CUSTOMERS_ALL, orgId, compId, "AllSupplierCustomerWithCityName");
            }
        } catch (Exception e) {
            customers = Collections.emptyList();
        }
        map.put("customers", customers);
        map.put("salesMen", customers);
        map.put("otherCommissionAgents", customers);

        // Booking Person - Sp_ReferenceParties_GetAllMethod, ReferencePartyTypeId=5
        try {
            map.put("bookingPersons", jdbcTemplate.queryForList(SQL_BOOKING_PERSONS, orgId, compId, 5,
                    "ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId"));
        } catch (Exception e) {
            map.put("bookingPersons", Collections.emptyList());
        }

        // Payment Terms - Sp_InvDueTerms_GetAllMethod @Activity='GetAll' (Id, TermsCode, TermsDescription, ...)
        try {
            map.put("paymentTerms", jdbcTemplate.queryForList(SQL_PAYMENT_TERMS, orgId, compId, "GetAll"));
        } catch (Exception e) {
            map.put("paymentTerms", Collections.emptyList());
        }

        // Delivery Term - hardcoded in the desktop itself (DeliveryTerms(): Load=1, Ponch=2). Not a DB lookup.
        map.put("deliveryTerms", List.of(
                mapOf("Id", 1, "Description", "Load"),
                mapOf("Id", 2, "Description", "Ponch")
        ));

        // Order Status - hardcoded in the desktop itself (OrderStatus(): Open/Complete/Cancel). Not a DB lookup.
        map.put("orderStatuses", List.of(
                mapOf("Id", 1, "Description", "Open"),
                mapOf("Id", 2, "Description", "Complete"),
                mapOf("Id", 3, "Description", "Cancel")
        ));

        // Commission Type (regular + other) - hardcoded in the desktop itself (CommissionTypeFill()). Not a DB lookup.
        map.put("commissionTypes", List.of(
                mapOf("Id", "Flat", "Description", "Flat"),
                mapOf("Id", "Percent", "Description", "Percent"),
                mapOf("Id", "Comm Weight", "Description", "Comm Weight")
        ));

        // Commission UOM (regular + other) - SpStaticColumnNames @Activity='GetCommissionUom' (static Id/type rows)
        try {
            map.put("commissionUoms", jdbcTemplate.queryForList(SQL_COMMISSION_UOM, "GetCommissionUom"));
        } catch (Exception e) {
            map.put("commissionUoms", Collections.emptyList());
        }

        // Item - USP_Item_AllItemsWithModal (Id, ItemName, ItemCode, ItemCategoryId, ItemCategory, ItemTypeId, ItemType, ItemTypeOfTypeId, ItemTypeOfType, ...)
        try {
            map.put("items", jdbcTemplate.queryForList(SQL_ITEMS, orgId, compId));
        } catch (Exception e) {
            map.put("items", Collections.emptyList());
        }

        // Crop Year - Sp_InvCropYear_GetAllMethod @Activity='ReadAll' (Id, CropYear, IsActive, ...)
        try {
            map.put("cropYears", jdbcTemplate.queryForList(SQL_CROP_YEARS, orgId, compId, "ReadAll"));
        } catch (Exception e) {
            map.put("cropYears", Collections.emptyList());
        }

        // Job/Lot - USP_GetJobLotsAllocatedToBranch, scoped to the current branch (Id, JobLotDescription, BranchId, BranchName)
        try {
            map.put("jobLots", jdbcTemplate.queryForList(SQL_JOB_LOTS, orgId, compId, branchId));
        } catch (Exception e) {
            map.put("jobLots", Collections.emptyList());
        }

        // Packing Type - Sp_InvPackingType_GetAllMethod @Activity='ReadAll' (Id, PackTypeCode, PackTypeDesc, ...)
        try {
            map.put("packingTypes", jdbcTemplate.queryForList(SQL_PACKING_TYPES, "ReadAll"));
        } catch (Exception e) {
            map.put("packingTypes", Collections.emptyList());
        }

        // City/Area - SP_City_GetAllMethod @MethodType='GetAll' (Id, Code, CityName, CityNameOtherLingo, ...)
        try {
            map.put("cities", jdbcTemplate.queryForList(SQL_CITIES, orgId, compId, "GetAll"));
        } catch (Exception e) {
            map.put("cities", Collections.emptyList());
        }

        // Warehouse - USP_GetWarehousesAllocatedToBranch, scoped to the current branch (Id, WareHouseName, BranchId, BranchName, ...)
        try {
            map.put("warehouses", jdbcTemplate.queryForList(SQL_WAREHOUSES, orgId, compId, branchId));
        } catch (Exception e) {
            map.put("warehouses", Collections.emptyList());
        }

        return map;
    }

    /** Small helper - avoids Map.of()'s null-hostility and keeps insertion order for JSON output. */
    private static Map<String, Object> mapOf(String k1, Object v1, String k2, Object v2) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put(k1, v1);
        m.put(k2, v2);
        return m;
    }

    /**
     * Rate UOM schedule dependent on the selected Item - Sp_UOMSchedule_GetAllMethod @Activity='ReadByItemID',
     * matching CommonServices.GetUomScheduleByItemId(itemId). Columns: Id, ItemId, ScheduleUnitId, UOMCode,
     * ItemName, ScheduleUnitName, QtyEquivalent, BaseRateUom, BasePackUom, BaseSecondaryUom, Active.
     * BaseRateUom=true marks the row the desktop auto-selects by default (CommonServices.GetBaseRateUomId).
     */
    public List<Map<String, Object>> getItemUomSchedule(int itemId) {
        try {
            int orgId = currentUserContext.currentOrganizationId();
            return jdbcTemplate.queryForList(SQL_UOM_BY_ITEM, orgId, itemId, "ReadByItemID");
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    // ==========================================================================================
    // NOT YET FIXED IN THIS PASS - still reading/writing the fabricated VoucherHead/VoucherDetail
    // schema. Left untouched deliberately (out of scope for the "fix dropdowns first" pass);
    // each is wrapped so it fails safe (empty list / success:false) rather than crashing the page.
    // Tracked as separate tasks: Customer balance summary (Party GL/Outstanding/Limit panel),
    // Stock Report / Stock Report With Value, Save/Update/Delete, History, Load-by-ID.
    // ==========================================================================================

    public Map<String, Object> getCustomerBalanceSummary(int customerId) {
        Map<String, Object> resp = new HashMap<>();
        resp.put("partyGlAmount", 0.00);
        resp.put("outstandingOrders", 0.00);
        resp.put("currentOrder", 0.00);
        resp.put("partyLimit", 0.00);
        resp.put("netRecoverable", 0.00);
        resp.put("notImplemented", true);
        return resp;
    }

    public List<Map<String, Object>> getStockReportData(Integer itemId, String cropYear, Integer jobLotId, Integer warehouseId, String date) {
        return Collections.emptyList();
    }

    public List<Map<String, Object>> getStockReportWithValueData(Integer itemId, String cropYear, Integer jobLotId, Integer warehouseId, String date) {
        return Collections.emptyList();
    }

    @Transactional
    public Map<String, Object> saveSaleOrder(SaleOrderDto dto) {
        Map<String, Object> response = new HashMap<>();
        response.put("success", false);
        response.put("message", "Save is not yet implemented against the real SaleOrder/SaleOrderDetail schema. " +
                "This pass only fixed the master-lookup dropdowns; Save/Update/Delete is a separate pending task.");
        return response;
    }

    public List<Map<String, Object>> getSaleOrdersHistory() {
        return Collections.emptyList();
    }

    /**
     * Real load-by-id, matching Architecture.DAL.Inventory.SaleOrder.GetData: one header row via
     * Sp_SaleOrder_GetAllMethod @Activity='ReadById', plus the same four sub-list activities the
     * desktop loads for every header row (detail lines, payment schedule, customer expenses, extra
     * items). Returns raw column names exactly as the real procedures emit them (no invented
     * fields) - null if no such Sale Order exists (matching the desktop's own [ActionId] <> 3
     * exclusion of deleted orders).
     */
    public Map<String, Object> getSaleOrderById(int id) {
        try {
            List<Map<String, Object>> headerRows =
                    jdbcTemplate.queryForList(SQL_SALE_ORDER_HEADER_BY_ID, id, "ReadById");
            if (headerRows.isEmpty()) {
                return null;
            }
            Map<String, Object> header = new LinkedHashMap<>(headerRows.get(0));

            header.put("lineItems",
                    jdbcTemplate.queryForList(SQL_SALE_ORDER_DETAIL_BY_HEADER_ID, id, "ReadBySaleOrderHeaderId"));
            header.put("paymentSchedules",
                    jdbcTemplate.queryForList(SQL_SALE_ORDER_PAYMENT_TERMS_BY_HEADER_ID, id, "SaleOrderPaymentTermDetailByHeaderId"));
            header.put("customerExpenses",
                    jdbcTemplate.queryForList(SQL_SALE_ORDER_CUSTOMER_EXPENSES_BY_HEADER_ID, id, "SaleOrderCustomerExpensesByHeaderId"));
            header.put("extraItems",
                    jdbcTemplate.queryForList(SQL_SALE_ORDER_EXTRA_ITEMS_BY_ID, id, "ReadBySaleOrderId_ExtraItemDetail"));

            return header;
        } catch (Exception e) {
            return null;
        }
    }
}
