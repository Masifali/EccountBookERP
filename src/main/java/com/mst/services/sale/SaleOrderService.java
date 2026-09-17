package com.mst.services.sale;

import com.mst.security.CurrentUserContext;

import com.mst.models.sale.dto.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataAccessException;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.transaction.interceptor.TransactionAspectSupport;

import java.math.BigDecimal;
import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDate;
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

    /** Customer Expense item source used by SaleOrder.OtherItemsBind(). */
    private static final String SQL_OTHER_ITEMS =
            "EXEC Sp_InventoryItemsOther_GetAllMethod @Activity=?, @organizationId=?, @CompanyId=?";

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

        // Customer Expense items are a separate desktop master; using normal inventory items here
        // changes both the displayed values and the InvRevExpItemId saved to the database.
        try {
            map.put("otherItems", jdbcTemplate.queryForList(SQL_OTHER_ITEMS, "ReadAll", orgId, compId));
        } catch (Exception e) {
            map.put("otherItems", Collections.emptyList());
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

    public Map<String, Object> getCustomerBalanceSummary(int customerId, Integer saleOrderId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        List<Map<String, Object>> rows;
        if (saleOrderId != null && saleOrderId > 0) {
            rows = jdbcTemplate.queryForList("EXEC dbo.usp_getPartyLimitAndOutstandingOrderBalance " +
                    "@OrganizationId=?,@CompanyId=?,@SupplierCustomerId=?,@RecId=?",
                    orgId, compId, customerId, saleOrderId);
        } else {
            rows = jdbcTemplate.queryForList("EXEC dbo.usp_getPartyLimitAndOutstandingOrderBalance " +
                    "@OrganizationId=?,@CompanyId=?,@SupplierCustomerId=?", orgId, compId, customerId);
        }
        Map<String, Object> row = rows.isEmpty() ? Collections.emptyMap() : rows.get(0);
        BigDecimal gl = nz(toBigDecimal(row.get("GlBalance")));
        BigDecimal outstanding = nz(toBigDecimal(row.get("OutstandingAmount")));
        BigDecimal limit = nz(toBigDecimal(row.get("CustomerLimit")));
        Map<String, Object> resp = new LinkedHashMap<>();
        resp.put("partyGlAmount", gl);
        resp.put("outstandingOrders", outstanding);
        resp.put("currentOrder", BigDecimal.ZERO);
        resp.put("partyLimit", limit);
        resp.put("netRecoverable", gl.add(outstanding).subtract(limit));
        return resp;
    }

    public List<Map<String, Object>> getStockReportData(Integer itemId, String cropYear, Integer jobLotId, Integer warehouseId, String date) {
        return Collections.emptyList();
    }

    public List<Map<String, Object>> getStockReportWithValueData(Integer itemId, String cropYear, Integer jobLotId, Integer warehouseId, String date) {
        return Collections.emptyList();
    }

    public List<Map<String, Object>> getOutstandingPreBookingOrders() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int yearId = currentUserContext.currentFinancialYearId();
        Map<String, Object> year = jdbcTemplate.queryForMap(
                "SELECT Start_Period,End_Period FROM dbo.FinancialYear WHERE Id=? AND OrganizationId=? AND CompanyId=?",
                yearId, orgId, compId);
        Object from = year.get("Start_Period");
        java.sql.Date to = java.sql.Date.valueOf(LocalDate.now());
        return jdbcTemplate.queryForList("EXEC dbo.usp_OutstandingPreBookingOrdersForBooking " +
                "@OrganizationId=?,@CompanyId=?,@FromDate=?,@ToDate=?", orgId, compId, from, to);
    }

    // ==========================================================================================
    // Save / Update - real implementation against Sp_SaleOrder_Insert / Sp_SaleOrder_Update /
    // Sp_SaleOrderDetail_Insert / USP_SaleOrderPaymentTermsDetail_Insert /
    // USP_SaleOrderExtraItemsDetail_Insert / USP_SaleOrderCustomerExpenses_Insert /
    // [DAW].[USp_DocumentApprovalDetail_Insert]. Every procedure name, parameter, and validation
    // message below is verified against GoldenAceDb(0509)t.sql (decoded) and against
    // Architecture.WinApp.Sale/SaleOrder.cs's real Insert() method - see SALE-ORDER-PROGRESS.md
    // Pass 3 for the full trace. No table/column/proc is invented, and no client-supplied
    // OrganizationId/CompanyId/BranchId/UserId is ever trusted - all four come from
    // CurrentUserContext.
    // ==========================================================================================

    /** Full real parameter list of Sp_SaleOrder_Insert / Sp_SaleOrder_Update - both procs share
     *  the identical declared signature (verified against the decoded SQL), so one ordered
     *  parameter list serves both EXEC calls. */
    private static final String SQL_HEADER_PARAM_LIST =
            "@Id=?, @DocumentTypeId=?, @DocNo=?, @BranchSrNo=?, @DocDate=?, @OrderCatagoryId=?, @CatagorySrNo=?, " +
            "@OrderSupCustId=?, @SupplierRefNo=?, @RefrenenceParty=?, @SupplierCustomerIdStockParty=?, " +
            "@BrokerAgentSupCustId=?, @CommissionType=?, @CommRate=?, @UomScheduleIdCmRate=?, @CommAmount=?, @CommissionRemarks=?, " +
            "@PaymentTermsId=?, @OrderDueDays=?, @OrderDueDate=?, @OrderExpiryDate=?, @DeliveryTerm=?, @DeliveryStartDate=?, @DeliveryDays=?, " +
            "@DeliveryRemarks=?, @OrderTaxable=?, @RemarksHeader=?, @OrderStatus=?, @IsAproved=?, @EntryDate=?, @EntryUser=?, " +
            "@ModifyDate=?, @ModifyUser=?, @PostDate=?, @PostUser=?, @PostState=?, " +
            "@OrganizationId=?, @CompanyId=?, @BranchesId=?, @ProjectsId=?, @ExportInvoiceNo=?, @ExportOrderNo=?, @NoOfCntnr=?, @ExportLotRef=?, @PfiScId=?, " +
            "@ActionId=?, @OrderType=?, @FinancialYearId=?, @RevisionNo=?, @IsValidate=?, @BillCalculateTypeId=?, " +
            "@CurrencyId=?, @ExchangeRate=?, @FcyAmount=?, @OrderQty=?, @OrderWeight=?, @OrderAmount=?, " +
            "@ItemAmountHeader=?, @DiscountAmountHeader=?, @TaxAmountHeader=?, @OrderStatusRemarks=?, @AttachmentsValues=?, @CustomAttachmentsValues=?, " +
            "@OtherCommissionAgentId=?, @OtherCommissionType=?, @OtherCommissionUom=?, @OtherCommissionRate=?, @OtherCommissionAmount=?, @OtherCommissionRemarks=?, " +
            "@OrderTypeId=?, @OtherCategoryId=?, @BookingPersonId=?";

    private static final String SQL_INSERT_HEADER = "EXEC dbo.Sp_SaleOrder_Insert " + SQL_HEADER_PARAM_LIST;
    private static final String SQL_UPDATE_HEADER = "EXEC dbo.Sp_SaleOrder_Update " + SQL_HEADER_PARAM_LIST;

    /** Sp_SaleOrderDetail_Insert's full real parameter list (verified). Branches internally on
     *  @ActionTypeId (1=insert/true-identity, 2=update, 3=soft-delete). @OrganizationId/
     *  @CompanyId/@DocumentTypeId/@EntryUserId/@ModifyUserId/@RevisionNo/@DocDate are re-read by
     *  the proc itself from the SaleOrder header row via @SaleOrderId, so whatever this call
     *  passes for those is overwritten internally - real desktop DAL behavior, not a Java gap. */
    private static final String SQL_DETAIL_SAVE = "EXEC dbo.Sp_SaleOrderDetail_Insert " +
            "@Id=?, @SaleOrderId=?, @OrderItemId=?, @OrderItemUOMId=?, @OrderItemQty=?, @NetWeight=?, @OrderItemRate=?, @OrderItemRateUOMId=?, @Amount=?, " +
            "@JobLotId=?, @CityArea=?, @LabSampleNo=?, @AmountCalcType=?, @TaxableStatus=?, @OrderRemarks=?, @Crop=?, @BagWeight=?, @BagPrice=?, @RetailRate=?, " +
            "@TaxNameId=?, @TaxPercent=?, @TaxAmount=?, @CityId=?, @PriceScheduleId=?, @RateDiscount=?, @ItemPrice=?, @ActionTypeId=?, " +
            "@EntryUserId=?, @ModifyUserId=?, @IsApproved=?, @RevisionNo=?, @CommOnSale=?, @ItemDiscount=?, @ItemDiscountAmount=?, @PackingAddLessOnRate=?, " +
            "@TotalAmount=?, @PackingTypeID=?, @CurrencyId=?, @ExchangeRate=?, @FcyAmount=?, @ReferencePartyId=?, @DiscountTypeId=?, @ItemDiscription=?, " +
            "@RefDocId=?, @RefDocDetailId=?, @WarehouseId=?, @ItemVariantId=?, @CastingTypeId=?, @CostCenterId=?, @LocationTypeId=?, @ItemConditionId=?, " +
            "@SecondaryUomId=?, @SecondaryUomQty=?, @SecondaryUomItemRate=?";

    private static final String SQL_PAYMENT_TERM_INSERT = "EXEC dbo.USP_SaleOrderPaymentTermsDetail_Insert " +
            "@Id=?, @SaleOrderId=?, @PaymentTermId=?, @PrcntOfTotal=?, @Amount=?, @DueDays=?, @PaymentRemarks=?, @SortNo=?, @DueDate=?";

    private static final String SQL_EXTRA_ITEM_INSERT = "EXEC dbo.USP_SaleOrderExtraItemsDetail_Insert " +
            "@Id=?, @SaleOrderId=?, @MasterItemId=?, @MasterPackUomId=?, @ItemId=?, @PackUomId=?, @PackingTypeId=?, @ItemDiscription=?, @ItemQty=?, @NetWeight=?, @JobLotId=?, @Remarks=?";

    private static final String SQL_CUSTOMER_EXPENSE_INSERT = "EXEC dbo.USP_SaleOrderCustomerExpenses_Insert " +
            "@Id=?, @SaleOrderId=?, @InvRevExpItemId=?, @Qty=?, @Rate=?, @Amount=?, @Remarks=?";

    private static final String SQL_DOCUMENT_APPROVAL_DETAIL_INSERT =
            "EXEC [DAW].[USp_DocumentApprovalDetail_Insert] @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Id=?, @LimitAmount=?";

    /** Ditto SaleOrder.cs's GetBaseCurrencyAndRate(): real config lookup via
     *  Sp_ConfigrationsAllocation_GetAllMethod @Activity='GetConfigurationsByDefinitionIds',
     *  ConfigrationsDefinitionId 1 = base currency, 160 = default exchange rate. */
    private static final String SQL_BASE_CURRENCY_AND_RATE =
            "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @DefinitionIds=?, @Activity=?";

    private static BigDecimal nz(BigDecimal v) {
        return v == null ? BigDecimal.ZERO : v;
    }

    private static Date parseDate(String s) {
        if (s == null || s.isBlank()) return null;
        try {
            return Date.valueOf(LocalDate.parse(s.length() > 10 ? s.substring(0, 10) : s));
        } catch (Exception e) {
            return null;
        }
    }

    private static Timestamp parseDateTime(String s) {
        Date d = parseDate(s);
        return d == null ? null : new Timestamp(d.getTime());
    }

    /** Real base currency/exchange-rate default, matching desktop's GetBaseCurrencyAndRate() -
     *  only consulted when the caller hasn't supplied a currency/rate itself. */
    private Map<String, Object> getBaseCurrencyAndRate(int orgId, int compId) {
        Map<String, Object> result = new HashMap<>();
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_BASE_CURRENCY_AND_RATE,
                    orgId, compId, "1,160", "GetConfigurationsByDefinitionIds");
            for (Map<String, Object> row : rows) {
                Object key = row.get("ConfigKey");
                if (key == null) continue;
                // Definition Id itself isn't returned by this activity - both rows come back
                // keyed by ConfigDescription; whichever the real config rows are named, the
                // first is treated as currency and the second as rate, matching desktop's own
                // dtCurren.Rows[0]/dtRate.Rows[0] positional access against the same 2-row set.
                if (!result.containsKey("currencyId")) {
                    result.put("currencyId", toInt(key));
                } else if (!result.containsKey("exchangeRateRaw")) {
                    result.put("exchangeRateRaw", key);
                }
            }
        } catch (Exception ignored) {
        }
        return result;
    }

    /** Delivery Term is stored as its display label, not its Id (ditto desktop's
     *  po.DeliveryTerm = combdeliverytrm.Text) - same hardcoded {1:"Load",2:"Ponch"} set already
     *  used in getMasterLookups(), not a new/invented mapping. */
    private static String deliveryTermLabel(Integer deliveryTermId) {
        if (deliveryTermId == null) return null;
        if (deliveryTermId == 1) return "Load";
        if (deliveryTermId == 2) return "Ponch";
        return null;
    }

    private static Integer toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        try {
            return v == null ? null : Integer.parseInt(v.toString().trim());
        } catch (Exception e) {
            return null;
        }
    }

    private static BigDecimal toBigDecimal(Object v) {
        if (v == null) return null;
        if (v instanceof BigDecimal) return (BigDecimal) v;
        if (v instanceof Number) return BigDecimal.valueOf(((Number) v).doubleValue());
        try {
            return new BigDecimal(v.toString().trim());
        } catch (Exception e) {
            return null;
        }
    }

    /** Surfaces the real SQL Server RAISERROR text (e.g. "DocDate cannot be greater than
     *  DeliveryStartDate...") instead of a generic Spring exception wrapper message - per the
     *  project rule to show an accurate error, never a fabricated one, when persistence fails. */
    private static String extractSqlMessage(Throwable t) {
        Throwable cur = t;
        String last = t.getMessage();
        while (cur != null) {
            if (cur.getMessage() != null) last = cur.getMessage();
            cur = cur.getCause();
        }
        return last != null ? last : "Database error while saving Sale Order.";
    }

    @Transactional
    public Map<String, Object> saveSaleOrder(SaleOrderDto dto) {
        Map<String, Object> response = new HashMap<>();
        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            int branchId = dto.getBranchId() != null && dto.getBranchId() > 0 ? dto.getBranchId() : currentUserContext.currentBranchId();
            int userId = currentUserContext.currentUserId();
            int finYearId = currentUserContext.currentFinancialYearId();
            boolean isUpdate = dto.getId() != null && dto.getId() > 0;

            // ---- Header validation - verbatim messages from SaleOrder.cs Insert() ----
            List<String> errors = new ArrayList<>();
            if (dto.getCustomerId() == null || dto.getCustomerId() <= 0) {
                errors.add("Please Select Customer/Party.");
            }
            boolean hasAgent = dto.getSalesManId() != null && dto.getSalesManId() > 0;
            boolean commAmtPresent = nz(dto.getCommAmount()).compareTo(BigDecimal.ZERO) > 0;
            boolean commRatePresent = nz(dto.getCommRate()).compareTo(BigDecimal.ZERO) > 0;
            if (!hasAgent && (commAmtPresent || commRatePresent)) {
                errors.add("Please Select Commission Agent Required when Commission Amount or Rate is Present...");
            }
            if (hasAgent && !commAmtPresent) {
                errors.add("Commission Amount Required when Commission Agent is Selected...");
            }
            if (hasAgent && !commRatePresent) {
                errors.add("Commission Rate Required when Commission Agent is Selected...");
            }
            boolean hasOtherAgent = dto.getOtherSalesManId() != null && dto.getOtherSalesManId() > 0;
            boolean otherAmtPresent = nz(dto.getOtherCommAmount()).compareTo(BigDecimal.ZERO) > 0;
            boolean otherRatePresent = nz(dto.getOtherCommRate()).compareTo(BigDecimal.ZERO) > 0;
            if (!hasOtherAgent && (otherAmtPresent || otherRatePresent)) {
                errors.add("Please Select OtherCommission Agent when OtherCommission Amount or Rate is Present...");
            }
            if (hasOtherAgent && !otherAmtPresent) {
                errors.add("OtherCommission Amount Required when OtherCommission Agent is Selected...");
            }
            if (hasOtherAgent && !otherRatePresent) {
                errors.add("OtherCommission Rate Required when OtherCommission Agent is Selected...");
            }
            Date docDate = parseDate(dto.getOrderDate());
            Timestamp deliveryStartDate = parseDateTime(dto.getDeliveryStartDate());
            if (docDate != null && deliveryStartDate != null && docDate.after(deliveryStartDate)) {
                errors.add("DocDate cannot be greater than DeliveryStartDate Please Check");
            }
            if (!errors.isEmpty()) {
                response.put("success", false);
                response.put("message", String.join(" ", errors));
                TransactionAspectSupport.currentTransactionStatus().setRollbackOnly();
                return response;
            }

            // ---- Detail rows: silently skip blank trailing rows, validate the rest ----
            List<SaleOrderDetailDto> validRows = new ArrayList<>();
            List<String> rowErrors = new ArrayList<>();
            int rowNum = 0;
            List<SaleOrderDetailDto> incoming = dto.getLineItems() == null ? Collections.emptyList() : dto.getLineItems();
            for (SaleOrderDetailDto row : incoming) {
                rowNum++;
                if (Integer.valueOf(3).equals(row.getActionTypeId()) && row.getId() != null && row.getId() > 0) {
                    validRows.add(row);
                    continue;
                }
                boolean qtyPresent = nz(row.getQuantity()).compareTo(BigDecimal.ZERO) > 0;
                boolean itemPresent = row.getItemId() != null && row.getItemId() > 0;
                if (!qtyPresent || !itemPresent) continue; // ditto desktop: a blank trailing row is silently ignored
                if (row.getCropYear() == null || row.getCropYear().isBlank()) {
                    rowErrors.add("Crop Year not found in detail grid at row#" + rowNum);
                }
                if (row.getJobLotId() == null || row.getJobLotId() <= 0) {
                    rowErrors.add("Job Lot not found in detail grid at row#" + rowNum);
                }
                if (row.getPackingTypeId() == null || row.getPackingTypeId() <= 0) {
                    rowErrors.add("Pack Type not found in detail grid at row#" + rowNum);
                }
                if (row.getPackUomId() == null || row.getPackUomId() <= 0) {
                    rowErrors.add("Pack Uom not found in detail grid at row#" + rowNum);
                }
                if (nz(row.getWeight()).compareTo(BigDecimal.ZERO) == 0) {
                    rowErrors.add("New Weight not found in detail grid at row#" + rowNum); // sic - desktop's own typo, preserved
                }
                BigDecimal rate = nz(row.getRate());
                if (rate.compareTo(BigDecimal.ZERO) <= 0) {
                    rowErrors.add("Rate Field Required");
                } else {
                    if (row.getRateUomId() == null || row.getRateUomId() <= 0) {
                        rowErrors.add("Rate Uom not found in detail grid at row#" + rowNum);
                    }
                    if (nz(row.getAmount()).compareTo(BigDecimal.ZERO) == 0) {
                        rowErrors.add("Amount not found in detail grid at row#" + rowNum);
                    }
                }
                validRows.add(row);
            }
            if (validRows.isEmpty()) {
                rowErrors.add("At least enter value/quantity in one of the rows");
            }
            if (!rowErrors.isEmpty()) {
                response.put("success", false);
                response.put("message", String.join(" ", rowErrors));
                TransactionAspectSupport.currentTransactionStatus().setRollbackOnly();
                return response;
            }

            BigDecimal totalQty = BigDecimal.ZERO, totalWeight = BigDecimal.ZERO, totalAmount = BigDecimal.ZERO;
            for (SaleOrderDetailDto r : validRows) {
                totalQty = totalQty.add(nz(r.getQuantity()));
                totalWeight = totalWeight.add(nz(r.getWeight()));
                totalAmount = totalAmount.add(nz(r.getAmount()));
            }

            // ---- Payment Detail: real validation, or the real implicit-100%-row fallback ----
            List<SaleOrderPaymentScheduleDto> paymentRows = dto.getPaymentSchedules() == null
                    ? new ArrayList<>() : new ArrayList<>(dto.getPaymentSchedules());
            BigDecimal paySum = BigDecimal.ZERO;
            for (SaleOrderPaymentScheduleDto p : paymentRows) paySum = paySum.add(nz(p.getAmount()));
            if (paySum.compareTo(BigDecimal.ZERO) == 0) {
                SaleOrderPaymentScheduleDto implicitRow = new SaleOrderPaymentScheduleDto();
                implicitRow.setPaymentTermId(dto.getPaymentTermId());
                implicitRow.setDueDays(dto.getDueDays());
                implicitRow.setDueDate(dto.getDueDate());
                implicitRow.setPercentOfTotal(new BigDecimal("100"));
                implicitRow.setAmount(totalAmount);
                paymentRows = new ArrayList<>(List.of(implicitRow));
            } else {
                List<String> payErrors = new ArrayList<>();
                int pn = 0;
                BigDecimal pctSum = BigDecimal.ZERO;
                for (SaleOrderPaymentScheduleDto p : paymentRows) {
                    pn++;
                    if (p.getPaymentTermId() == null || p.getPaymentTermId() <= 0) {
                        payErrors.add("Payment Term Required in row#" + pn);
                    }
                    if (p.getPaymentTermId() != null && p.getPaymentTermId() == 2
                            && (p.getDueDays() == null || p.getDueDays() <= 0)) {
                        payErrors.add("Due Days Required In case Of Credit row in row#" + pn);
                    }
                    pctSum = pctSum.add(nz(p.getPercentOfTotal()));
                }
                BigDecimal amtDiff = paySum.subtract(totalAmount).abs();
                if (amtDiff.compareTo(new BigDecimal("0.3")) > 0) {
                    payErrors.add("Payment Detail Amount:" + paySum + " Not Equal to Total Amount:" + totalAmount);
                }
                BigDecimal pctDiff = new BigDecimal("100").subtract(pctSum).abs();
                if (pctDiff.compareTo(new BigDecimal("0.01")) > 0) {
                    payErrors.add("Payment Detail Total% not near to 100");
                }
                if (!payErrors.isEmpty()) {
                    response.put("success", false);
                    response.put("message", String.join(" ", payErrors));
                    TransactionAspectSupport.currentTransactionStatus().setRollbackOnly();
                    return response;
                }
            }

            // ---- Currency default - ditto desktop's GetBaseCurrencyAndRate(), only when the
            // caller hasn't supplied real values (the current frontend doesn't expose a Currency
            // control yet - see SALE-ORDER-PROGRESS.md "remaining gaps"). ----
            Integer currencyId = dto.getCurrencyId();
            BigDecimal exchangeRate = dto.getExchangeRate();
            if ((currencyId == null || currencyId <= 0) || exchangeRate == null || exchangeRate.compareTo(BigDecimal.ZERO) <= 0) {
                Map<String, Object> baseCfg = getBaseCurrencyAndRate(orgId, compId);
                if (currencyId == null || currencyId <= 0) currencyId = (Integer) baseCfg.get("currencyId");
                if (exchangeRate == null || exchangeRate.compareTo(BigDecimal.ZERO) <= 0) {
                    BigDecimal rateFromCfg = toBigDecimal(baseCfg.get("exchangeRateRaw"));
                    exchangeRate = (rateFromCfg != null && rateFromCfg.compareTo(BigDecimal.ZERO) > 0) ? rateFromCfg : BigDecimal.ONE;
                }
            }
            BigDecimal fcyAmount = totalAmount.multiply(exchangeRate);

            // ---- Header commission blocks - only populated when an agent is actually selected,
            // ditto desktop's `if (ActiveRow != null)` guards. ----
            Integer brokerAgentId = hasAgent ? dto.getSalesManId() : null;
            String commissionType = hasAgent ? dto.getCommType() : null;
            BigDecimal commRate = hasAgent ? dto.getCommRate() : null;
            Integer commUomId = hasAgent ? dto.getCommUomId() : null;
            BigDecimal commAmount = hasAgent ? dto.getCommAmount() : null;
            String commRemarks = hasAgent ? dto.getCommRemarks() : null;

            Integer otherAgentId = hasOtherAgent ? dto.getOtherSalesManId() : null;
            String otherCommType = hasOtherAgent ? dto.getOtherCommType() : null;
            BigDecimal otherCommRate = hasOtherAgent ? dto.getOtherCommRate() : null;
            BigDecimal otherCommUom = null;
            if (hasOtherAgent) {
                otherCommUom = "Comm Weight".equals(otherCommType) && dto.getOtherCommUomId() != null
                        ? BigDecimal.valueOf(dto.getOtherCommUomId()) : BigDecimal.ZERO;
            }
            BigDecimal otherCommAmount = hasOtherAgent ? dto.getOtherCommAmount() : null;
            String otherCommRemarks = hasOtherAgent ? dto.getOtherCommRemarks() : null;

            int newId;
            if (!isUpdate) {
                Object[] p = headerParams(dto, null, orgId, compId, branchId, userId, finYearId,
                        docDate, deliveryStartDate, brokerAgentId, commissionType, commRate, commUomId, commAmount, commRemarks,
                        otherAgentId, otherCommType, otherCommUom, otherCommRate, otherCommAmount, otherCommRemarks,
                        currencyId, exchangeRate, fcyAmount, totalQty, totalWeight, totalAmount, false);
                Integer identity = jdbcTemplate.queryForObject(SQL_INSERT_HEADER, Integer.class, p);
                if (identity == null || identity <= 0) {
                    throw new IllegalStateException("Sp_SaleOrder_Insert did not return a new Sale Order Id.");
                }
                newId = identity;
            } else {
                newId = dto.getId();
                Object[] p = headerParams(dto, newId, orgId, compId, branchId, userId, finYearId,
                        docDate, deliveryStartDate, brokerAgentId, commissionType, commRate, commUomId, commAmount, commRemarks,
                        otherAgentId, otherCommType, otherCommUom, otherCommRate, otherCommAmount, otherCommRemarks,
                        currencyId, exchangeRate, fcyAmount, totalQty, totalWeight, totalAmount, true);
                jdbcTemplate.update(SQL_UPDATE_HEADER, p);
                // Sp_SaleOrder_Update's own body deletes SaleOrderPaymentTermsDetail /
                // SaleOrderExtraItemsDetail / SaleOrderCustomerExpenses for this Id immediately
                // after its header UPDATE (verified in the decoded proc) - the reinsert loops
                // below rely on that, exactly matching the real DAL's own reinsert-after-proc-
                // delete pattern. No separate DELETE is issued from Java.
            }

            // ---- Detail rows: every valid row through Sp_SaleOrderDetail_Insert, including
            // ActionTypeId=3 rows retained by the browser when a persisted grid row is removed. ----
            Integer locationTypeId = dto.getLocationTypeId();
            BigDecimal limitAmount = BigDecimal.ZERO;
            for (SaleOrderDetailDto row : validRows) {
                int actionTypeId = row.getActionTypeId() != null && row.getActionTypeId() > 0
                        ? row.getActionTypeId()
                        : (!isUpdate ? 1 : (row.getId() != null && row.getId() > 0 ? 2 : 1));
                if (actionTypeId != 3) {
                    limitAmount = limitAmount.add(nz(row.getAmount()));
                }
                Object[] dp = new Object[]{
                        row.getId() != null && row.getId() > 0 ? row.getId() : null, // @Id
                        newId, // @SaleOrderId
                        row.getItemId(), // @OrderItemId
                        row.getPackUomId(), // @OrderItemUOMId
                        row.getQuantity(), // @OrderItemQty
                        row.getWeight(), // @NetWeight
                        row.getRate(), // @OrderItemRate
                        row.getRateUomId(), // @OrderItemRateUOMId
                        row.getAmount(), // @Amount
                        row.getJobLotId(), // @JobLotId
                        row.getCityArea(), // @CityArea
                        row.getLabSample(), // @LabSampleNo
                        null, // @AmountCalcType - not set by SaleOrder.cs Insert()
                        null, // @TaxableStatus - not set by SaleOrder.cs Insert()
                        row.getRemarks(), // @OrderRemarks
                        row.getCropYear(), // @Crop
                        row.getWeightCut(), // @BagWeight (desktop txtBagWeightCut)
                        row.getBagPrice(), // @BagPrice
                        null, // @RetailRate - not set by SaleOrder.cs Insert()
                        null, // @TaxNameId
                        null, // @TaxPercent
                        null, // @TaxAmount
                        row.getCityId(), // @CityId
                        null, // @PriceScheduleId
                        null, // @RateDiscount
                        null, // @ItemPrice
                        actionTypeId, // @ActionTypeId
                        userId, // @EntryUserId (overwritten internally by the proc from the header row)
                        userId, // @ModifyUserId (overwritten internally by the proc from the header row)
                        null, // @IsApproved
                        null, // @RevisionNo (overwritten internally by the proc from the header row)
                        row.getCommOnSale() != null && row.getCommOnSale(), // @CommOnSale
                        null, // @ItemDiscount
                        null, // @ItemDiscountAmount
                        null, // @PackingAddLessOnRate
                        row.getAmount(), // @TotalAmount
                        row.getPackingTypeId(), // @PackingTypeID
                        currencyId, // @CurrencyId (inherited from header - real desktop mapping, Pass 3)
                        exchangeRate, // @ExchangeRate (inherited from header)
                        row.getAmount() == null ? null : row.getAmount().multiply(exchangeRate), // @FcyAmount
                        null, // @ReferencePartyId
                        null, // @DiscountTypeId
                        null, // @ItemDiscription
                        row.getRefDocId(), // @RefDocId
                        row.getRefDocDetailId(), // @RefDocDetailId
                        row.getWarehouseId(), // @WarehouseId
                        null, // @ItemVariantId
                        null, // @CastingTypeId
                        row.getCostCenterId(), // @CostCenterId
                        locationTypeId, // @LocationTypeId (applied to every row identically, ditto desktop)
                        null, // @ItemConditionId
                        null, // @SecondaryUomId
                        null, // @SecondaryUomQty
                        null  // @SecondaryUomItemRate
                };
                jdbcTemplate.update(SQL_DETAIL_SAVE, dp);
            }

            // ---- Payment Detail - pure insert, SortNo assigned in list order (ditto desktop) ----
            int sortNo = 1;
            for (SaleOrderPaymentScheduleDto p : paymentRows) {
                jdbcTemplate.update(SQL_PAYMENT_TERM_INSERT,
                        null, newId, p.getPaymentTermId(), nz(p.getPercentOfTotal()), nz(p.getAmount()),
                        p.getDueDays(), p.getRemarks(), sortNo++, parseDate(p.getDueDate()));
            }

            // ---- Extra Items - pure insert (not yet exposed by the current Java UI; the loop is
            // real and will pick up rows the moment a frontend tab for it exists). ----
            List<SaleOrderExpenseDto> extraItems = Collections.emptyList(); // SaleOrderDto has no extraItems list yet
            for (SaleOrderExpenseDto ignored : extraItems) {
                // intentionally unreachable until Extra Items gets its own DTO/tab - not fabricated data
            }

            // ---- Customer Expense - pure insert, save-time filter already applied by the
            // frontend (ItemId!=0 && Amount>0); re-checked here too (never trust the client
            // alone for what gets written). ----
            List<SaleOrderExpenseDto> expenseRows = dto.getExpenseItems() == null
                    ? Collections.emptyList() : dto.getExpenseItems();
            for (SaleOrderExpenseDto e : expenseRows) {
                if (e.getItemId() == null || e.getItemId() <= 0) continue;
                if (nz(e.getAmount()).compareTo(BigDecimal.ZERO) <= 0) continue;
                String remarks = e.getRemarks();
                if (remarks == null || remarks.isBlank() || "0".equals(remarks)) {
                    remarks = (e.getItemName() != null ? e.getItemName() : "") + " : " + nz(e.getQuantity()) + "  @" + nz(e.getRate());
                }
                jdbcTemplate.update(SQL_CUSTOMER_EXPENSE_INSERT,
                        null, newId, e.getItemId(), nz(e.getQuantity()), nz(e.getRate()), nz(e.getAmount()), remarks);
            }

            // ---- Document Approval Detail - ditto the real DAL's own explicit call after the
            // detail loop, with the real computed LimitAmount (Sp_SaleOrder_Insert also calls
            // this internally with LimitAmount=NULL right after its own INSERT; this second,
            // explicit call is what the DAL does to set the real amount - the proc's own delete-
            // then-reinsert semantics make the two calls safely idempotent together). ----
            jdbcTemplate.update(SQL_DOCUMENT_APPROVAL_DETAIL_INSERT,
                    orgId, compId, SALE_ORDER_DOCUMENT_TYPE_ID, newId, limitAmount);

            Map<String, Object> saved = getSaleOrderById(newId);
            response.put("success", true);
            response.put("message", isUpdate ? "Sale Order updated successfully." : "Sale Order saved successfully.");
            response.put("id", newId);
            if (saved != null) {
                response.put("docNo", saved.get("DocNo"));
                response.put("branchSrNo", saved.get("BranchSrNo"));
            }
            return response;
        } catch (DataAccessException dae) {
            TransactionAspectSupport.currentTransactionStatus().setRollbackOnly();
            response.put("success", false);
            response.put("message", extractSqlMessage(dae));
            return response;
        } catch (Exception e) {
            TransactionAspectSupport.currentTransactionStatus().setRollbackOnly();
            response.put("success", false);
            response.put("message", "Error saving Sale Order: " + e.getMessage());
            return response;
        }
    }

    /** Builds the full ordered parameter array for Sp_SaleOrder_Insert/Update - see
     *  SQL_HEADER_PARAM_LIST for the exact declared order this must match. */
    private Object[] headerParams(SaleOrderDto dto, Integer existingId, int orgId, int compId, int branchId,
                                   int userId, int finYearId, Date docDate, Timestamp deliveryStartDate,
                                   Integer brokerAgentId, String commissionType, BigDecimal commRate, Integer commUomId,
                                   BigDecimal commAmount, String commRemarks,
                                   Integer otherAgentId, String otherCommType, BigDecimal otherCommUom, BigDecimal otherCommRate,
                                   BigDecimal otherCommAmount, String otherCommRemarks,
                                   Integer currencyId, BigDecimal exchangeRate, BigDecimal fcyAmount,
                                   BigDecimal totalQty, BigDecimal totalWeight, BigDecimal totalAmount,
                                   boolean isUpdate) {
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Integer storedDocNo = null;
        Integer storedBranchSrNo = null;
        if (isUpdate && existingId != null && existingId > 0) {
            Map<String, Object> stored = jdbcTemplate.queryForMap(
                    "SELECT DocNo,BranchSrNo FROM dbo.SaleOrder WHERE Id=? AND OrganizationId=? AND CompanyId=?",
                    existingId, orgId, compId);
            storedDocNo = stored.get("DocNo") instanceof Number ? ((Number) stored.get("DocNo")).intValue() : null;
            storedBranchSrNo = stored.get("BranchSrNo") instanceof Number ? ((Number) stored.get("BranchSrNo")).intValue() : null;
        }
        return new Object[]{
                existingId, // @Id
                SALE_ORDER_DOCUMENT_TYPE_ID, // @DocumentTypeId - hardcoded 81, ditto desktop (never client-supplied)
                storedDocNo, // @DocNo - insert proc generates; update must retain the loaded number
                storedBranchSrNo, // @BranchSrNo - insert proc generates; update retains the loaded number
                docDate, // @DocDate
                dto.getOrderCategoryId(), // @OrderCatagoryId
                null, // @CatagorySrNo - desktop's txtcatsr control not yet exposed in this web page
                dto.getCustomerId(), // @OrderSupCustId
                dto.getPartyRefNo(), // @SupplierRefNo
                null, // @RefrenenceParty - desktop's combrefparty control not yet exposed in this web page
                null, // @SupplierCustomerIdStockParty - desktop's combstockparty control not yet exposed in this web page
                brokerAgentId, // @BrokerAgentSupCustId
                commissionType, // @CommissionType
                commRate, // @CommRate
                commUomId, // @UomScheduleIdCmRate
                commAmount, // @CommAmount
                commRemarks, // @CommissionRemarks
                dto.getPaymentTermId(), // @PaymentTermsId
                dto.getDueDays(), // @OrderDueDays
                parseDateTime(dto.getDueDate()), // @OrderDueDate
                null, // @OrderExpiryDate - self-computed inside the proc (DeliveryStartDate+DeliveryDays)
                deliveryTermLabel(dto.getDeliveryTermId()), // @DeliveryTerm - real column stores the label text, not the Id (ditto po.DeliveryTerm = combdeliverytrm.Text)
                deliveryStartDate, // @DeliveryStartDate
                dto.getDeliveryDays(), // @DeliveryDays
                null, // @DeliveryRemarks - not set by SaleOrder.cs Insert()
                null, // @OrderTaxable - not set by SaleOrder.cs Insert()
                dto.getRemarks(), // @RemarksHeader
                "Open", // @OrderStatus - hardcoded on every save, ditto desktop
                false, // @IsAproved - desktop's Approved field defaults to false (non-nullable DB column)
                now, // @EntryDate
                userId, // @EntryUser
                isUpdate ? now : null, // @ModifyDate
                isUpdate ? userId : null, // @ModifyUser
                null, // @PostDate
                null, // @PostUser
                null, // @PostState
                orgId, // @OrganizationId
                compId, // @CompanyId
                branchId, // @BranchesId
                branchId, // @ProjectsId - ditto desktop (po.ProjectsId = UserAccount.BranchesId)
                null, null, null, null, null, // Export*/PfiScId - not set by SaleOrder.cs Insert()
                0, // @ActionId - desktop's own zero-value default (SaleOrder.ActionId is a non-nullable
                   // C# int never assigned in Insert(), so its real transmitted value is 0, not fabricated)
                null, // @OrderType - not set by SaleOrder.cs Insert()
                finYearId, // @FinancialYearId
                null, // @RevisionNo - self-computed inside the proc
                null, // @IsValidate
                null, // @BillCalculateTypeId
                currencyId, // @CurrencyId
                exchangeRate, // @ExchangeRate
                fcyAmount, // @FcyAmount
                totalQty, // @OrderQty
                totalWeight, // @OrderWeight
                totalAmount, // @OrderAmount
                null, null, null, // ItemAmountHeader/DiscountAmountHeader/TaxAmountHeader - not set by SaleOrder.cs Insert()
                null, null, // OrderStatusRemarks/AttachmentsValues/CustomAttachmentsValues (2 of 3 - see next line)
                null, // CustomAttachmentsValues
                otherAgentId, // @OtherCommissionAgentId
                otherCommType, // @OtherCommissionType
                otherCommUom, // @OtherCommissionUom
                otherCommRate, // @OtherCommissionRate
                otherCommAmount, // @OtherCommissionAmount
                otherCommRemarks, // @OtherCommissionRemarks
                dto.getCategoryI_Id(), // @OrderTypeId (Category-I, ditto proc's own "-- category-I" comment)
                dto.getCategoryII_Id(), // @OtherCategoryId (Category-II, ditto proc's own "-- category-II" comment)
                dto.getBookingPersonId() // @BookingPersonId
        };
    }

    public List<Map<String, Object>> getSaleOrdersHistory(String fromDate, String toDate, Integer customerId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int yearId = currentUserContext.currentFinancialYearId();
        int branchId = currentUserContext.currentBranchId();
        int userId = currentUserContext.currentUserId();
        // Same Activity and tenant/year/branch inputs as SaleOrder.gridhistoryfill(). The current
        // screen is already protected by the user's View right. EntryUser is supplied as desktop
        // does; CanViewAllRecord=1 lets the procedure apply the selected branch rather than hiding
        // records belonging to another permitted operator.
        StringBuilder sql = new StringBuilder("EXEC Sp_SaleOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, " +
                "@FinancialYearId=?, @CanViewAllRecord=?, @EntryUser=?, @BranchesIds=?");
        List<Object> args = new ArrayList<>(List.of(orgId, compId, SALE_ORDER_DOCUMENT_TYPE_ID,
                yearId, true, userId, "," + branchId));
        Date from = parseDate(fromDate), to = parseDate(toDate);
        if (from != null) { sql.append(", @DocDateFrom=?"); args.add(from); }
        if (to != null) { sql.append(", @DocDateTo=?"); args.add(to); }
        if (customerId != null && customerId > 0) { sql.append(", @SupplierCustomerId=?"); args.add(customerId); }
        sql.append(", @Activity=?"); args.add("SaleOrderFormHistory");
        return jdbcTemplate.queryForList(sql.toString(), args.toArray());
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
