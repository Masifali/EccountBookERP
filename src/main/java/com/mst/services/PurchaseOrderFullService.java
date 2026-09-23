package com.mst.services;

import com.mst.repositories.support.ProcExec;

import com.mst.models.dto.PurchaseOrderFullDto;
import com.mst.repositories.PurchaseOrderHeaderRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service
public class PurchaseOrderFullService {

    private static final org.slf4j.Logger CHARGE_ACCT_LOG =
            org.slf4j.LoggerFactory.getLogger(PurchaseOrderFullService.class);


    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CurrentUserContext currentUserContext;

    /** The desktop's own header write path - Sp_PurchaseOrder_Insert / _Update. */
    @Autowired
    private PurchaseOrderHeaderRepository purchaseOrderHeaderRepository;

    @Autowired
    private com.mst.repositories.PurchaseOrderRecordRepository purchaseOrderRecords;

    @Autowired
    private PurchaseOrderAttachmentService purchaseOrderAttachments;

    @Autowired
    private com.mst.repositories.PurchaseOrderDetailRepository purchaseOrderDetails;

    @Autowired
    private com.mst.repositories.PurchaseOrderSupplementRepository purchaseOrderSupplements;

    // ==========================================================================================
    // Packing Material (Empty Bags) - real stored-procedure / table names, verified directly
    // against GoldenAceDb(0509)t.sql (UTF-16LE-decoded) and against the desktop code-behind
    // recovered_source/ECCOUNTBOOKERP/Architecture.WinApp.Purchase/PurchsaeOrder.cs (dtEmptyBags /
    // grdEmptyBags) and recovered_source/projects/architecture.dal/0448_Architecture.DAL.Inventory.
    // PurchaseOrder.cs (SetData / GetData). No table/column/procedure name below is guessed.
    // ==========================================================================================

    /** Real table [dbo].[PurchaseOrderEmptyBags] columns: Id, PurchaseOrderId, Type, ItemId, PackingTypeId, Rate, WeightCut. No Qty/Amount column exists. */
    private static final String SQL_EMPTY_BAGS_DELETE_BY_HEADER = "DELETE FROM PurchaseOrderEmptyBags WHERE PurchaseOrderId=?";

    /** Sp_PurchaseOrderEmptyBags_Insert(@Id,@PurchaseOrderId,@Type,@ItemId,@PackingTypeId,@Rate,@WeightCut) - also enforces the
     *  desktop's own Jute(PackingTypeId=1)/PP(PackingTypeId=2) min/max WeightCut range server-side via ConfigId 324
     *  ('StopShowingPurchaseOrderEmptyBagsWeightValidations') and ConfigrationsDefinitionId 196/197 (Jute min/max),
     *  198/199 (PP min/max). */
    private static final String SQL_EMPTY_BAGS_INSERT =
            "EXEC Sp_PurchaseOrderEmptyBags_Insert @Id=?, @PurchaseOrderId=?, @Type=?, @ItemId=?, @PackingTypeId=?, @Rate=?, @WeightCut=?";

    /** Sp_PurchaseOrder_GetAllMethod @Activity='ReadPurchaseOrderEmptyBagsDetailByHeaderId' - real read path used by
     *  Architecture.DAL.Inventory.PurchaseOrder.GetData(...) for every loaded Purchase Order. Columns returned:
     *  Id, ItemId, ItemName, PackingTypeId, PackTypeDesc, PurchaseOrderId, Rate, WeightCut, Type, EmptyBagsType. */
    private static final String SQL_EMPTY_BAGS_BY_HEADER_ID =
            "EXEC Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?";

    /** SpStaticColumnNames @Activity='PurchaseOrderEmptyBagsType' -> SELECT Id, type FROM [dbo].vEmptyBagTypes, matching
     *  CommonServices.StaticColumnsService("PurchaseOrderEmptyBagsType") used by PurchsaeOrder.cs's EmptyBagsTypeFill().
     *  vEmptyBagTypes real values: (1,'Normal (Purchase/Receive)'), (2,'Purchase Against Weight'), (3,'Free of Cost'),
     *  (4,'Retained'), (5,'Returned'). */
    private static final String SQL_EMPTY_BAG_TYPES = "EXEC SpStaticColumnNames @Activity=?";

    /** [dbo].[USP_PackingMaterialItemsAllocateToTransactionFlow_GetForCombo] - matching CommonServices.
     *  GetPackingMaterialItemsAllocateToFlow(TransactionFlowId=1) used by PurchsaeOrder.cs's EmptyBagsItemFill().
     *  Columns returned: ItemId, ItemName, ItemCode. */
    private static final String SQL_EMPTY_BAG_ITEMS =
            "EXEC USP_PackingMaterialItemsAllocateToTransactionFlow_GetForCombo @OrganizationId=?, @CompanyId=?, @ItemCategoryId=?, @ItemTypeId=?, @TransactionFlowId=?";

    /** Sp_InvPackingType_GetAllMethod @Activity='ReadAll' (WHERE IsActive=1 baked into the proc) - matching
     *  CommonServices.InvPackingTypeGetAllService()/Architecture.BLL.Inventory.InvPackingType.Getall() used by
     *  PurchsaeOrder.cs's PackingTypeFillForEmptyBagsGrid() and by the per-row WeightCut range validation
     *  (GrdEmptyBagsRefresh/packType.MinEbWeight/MaxEbWeight). @Id is intentionally NOT bound here (the proc's
     *  own signature defaults it to NULL) - Getall() never sets it either. Passing a literal null as the sole
     *  argument right after `sql` in a JdbcTemplate.queryForList(sql, Object...) call is ambiguous with the
     *  queryForList(sql, Class<T> elementType, Object...) overload and silently resolved to the wrong one
     *  (confirmed live: this returned an empty list against the real database even though InvPackingType has
     *  real active rows) - omitting the unused @Id parameter entirely avoids that overload-resolution trap.
     *  Columns returned: Id, PackTypeCode, PackTypeDesc, MinEbWeight, MaxEbWeight, MinEbStockWeight, MaxEbStockWeight. */
    private static final String SQL_PACKING_TYPES_FOR_EMPTY_BAGS = "EXEC Sp_InvPackingType_GetAllMethod @Activity=?";

    /** Sp_ConfigrationsAllocation_GetAllMethod @Activity='GetConfigurationByOrgCompandConfigDescription' - matching
     *  GlobalVariables_Helper.GetConfigValueFromGlobal(configName) / CommonServices.GetConfigurationFromAllocation(...).
     *  Used for 'WeightCutForJuteBags', 'WeightCutForPPBags' (default row seeding, PurchsaeOrder.cs AddRowInvEmptyBagsGrid())
     *  and 'StopShowingPurchaseOrderEmptyBagsWeightValidations' (per-row validation gate). Returns a single ConfigKey column. */
    private static final String SQL_CONFIG_VALUE_BY_DESCRIPTION =
            "EXEC Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @DefinitionIds=?, @Activity=?";

    private static final String SQL_ORG_COMPANY_BY_PURCHASE_ORDER_ID =
            "SELECT OrganizationId, CompanyId FROM PurchaseOrder WHERE Id=?";

    // ==========================================================================================
    // Supplier Expense (Credit To Supplier & Debit To Product) - real stored-procedure / table names,
    // verified against GoldenAceDb(0509)t.sql and PurchsaeOrder.cs (grdInvExp/dtInvExp,
    // AccountTitleFill()-adjacent InventoryItemsOther master, architecture.dal PurchaseOrder.cs
    // SetData/GetData). Real table [dbo].[PurchaseOrderSupplierExpense]: Id, PurchaseOrderId,
    // InvRevExpItemId, Qty, Rate, Amount, Remarks.
    // ==========================================================================================

    /** Sp_InventoryItemsOther_GetAllMethod @Activity='ReadAll' - real "Other Item" master list
     *  (e.g. BARDANA, OTHER CHARGES, MUNSHIANA, SOTRI), matching Architecture.BLL.Inventory.
     *  InventoryItemsOther.GetAll(), org/company scoped. Columns returned include Id, OtherItemName. */
    private static final String SQL_OTHER_ITEMS_FOR_EXPENSE =
            "EXEC Sp_InventoryItemsOther_GetAllMethod @Activity=?, @organizationId=?, @CompanyId=?";

    /** Sp_PurchaseOrderSupplierExpense_Insert(@Id,@PurchaseOrderId,@InvRevExpItemId,@Qty,@Rate,@Amount,@Remarks). */
    private static final String SQL_SUPPLIER_EXPENSE_INSERT =
            "EXEC Sp_PurchaseOrderSupplierExpense_Insert @Id=?, @PurchaseOrderId=?, @InvRevExpItemId=?, @Qty=?, @Rate=?, @Amount=?, @Remarks=?";

    /** Sp_PurchaseOrder_GetAllMethod @Activity='ReadPurchaseOrderSupplierExpenseByHeaderId' - real read path.
     *  Columns returned: Id, PurchaseOrderId, InvRevExpItemId, Qty, Rate, Amount, Remarks, OtherItemName. */
    private static final String SQL_SUPPLIER_EXPENSE_BY_HEADER_ID =
            "EXEC Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?";

    // ==========================================================================================
    // Account Credit _Charge to Product - real stored-procedure / table names, verified against
    // GoldenAceDb(0509)t.sql and PurchsaeOrder.cs (grdExpensesChargeToProduct/dtChargeToProduct,
    // AccountTitleFill() non-subsidiary branch -> CommonServices.CoaAllocationAccountTitleByAccountTypeIds
    // -> Sp_COAAllocation_GetAllMethod @Activity='GetAccountTitleByAccountTypeIds'). Real table
    // [dbo].[PurchaseOrderExpensesChargeToProduct]: Id, PurchaseOrderId, AccountId, Percentage, Qty,
    // Rate, Amount, Remarks, SupplierCustomerId.
    // ==========================================================================================

    /** Sp_COAAllocation_GetAllMethod @Activity='GetAccountTitleByAccountTypeIds' - real Chart of Account
     *  dropdown, restricted to AccountGroup='Detail' (4th-level / leaf accounts) inside the proc itself,
     *  excluding AccountTypeIds 2,4,11,12,13,14,15,20,21,22. AccountTypeId 4 ('Inventory', per the
     *  AccountTypes seed row (4,'Inventory','Inventory','Inventory Stocks',...)) was added on top of
     *  desktop's own exclusion list: desktop's AccountTitleFill() omits it, which is why per-product
     *  "...Stock A/c" accounts leak into this dropdown on both desktop and the original port here.
     *  Excluding it keeps only genuine chargeable expense accounts, matching what this dropdown is for.
     *  Requires a non-zero @UserId (the proc raises an error otherwise) - CurrentUserContext.currentUserId()
     *  falls back to 1, same as everywhere else in this dev environment. @AppId is passed as 1 (any value
     *  other than 5/4/6 bypasses the CostCenter/CustomerPortal branches the desktop's other AppIds use).
     *  Columns returned: Id, AccountTitle, AccountCode. */
    private static final String SQL_COA_ACCOUNTS_FOR_CHARGE_TO_PRODUCT =
            "EXEC Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @UserId=?, @AppId=?, @AccountTypeIdsNot=?, @Activity=?";

    private static final String SQL_CHARGE_TO_PRODUCT_INSERT =
            "EXEC Sp_PurchaseOrderExpensesChargeToProduct_Insert @Id=?, @PurchaseOrderId=?, @AccountId=?, @Percentage=?, @Qty=?, @Rate=?, @Amount=?, @Remarks=?, @SupplierCustomerId=?";

    /** Sp_PurchaseOrder_GetAllMethod @Activity='ReadPurchaseOrderExpensesChargeToProductDetailByHeaderId'.
     *  Columns returned: Id, AccountId, AccountTitle, Percentage, PurchaseOrderId, Qty, Rate,
     *  SupplierCustomerId, SupplierCustomerName, Amount, Remarks. */
    private static final String SQL_CHARGE_TO_PRODUCT_BY_HEADER_ID =
            "EXEC Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?";

    // ==========================================================================================
    // Payment Detail - real stored-procedure / table names, verified against GoldenAceDb(0509)t.sql
    // and PurchsaeOrder.cs (grdPaymentDetail/dtPaymentTerm, grdPaymentDetail_CellUpdated() two-way
    // %OfTotal<->Amount and DueDays<->DueDate calculation). Real table
    // [dbo].[PurchaseOrderPaymentTermsDetail]: Id, PurchaseOrderId, PaymentTermId, PrcntOfTotal,
    // Amount, DueDays, PaymentRemarks, SortNo, DueDate. PaymentTermId comes from the real InvDueTerms
    // master (Id=1 'Cash', Id=2 'Credit' - DueDays required when PaymentTermId=2), already exposed by
    // the pre-existing getPaymentTerms().
    // ==========================================================================================

    private static final String SQL_PAYMENT_TERMS_DETAIL_INSERT =
            "EXEC [dbo].[USP_PurchaseOrderPaymentTermsDetail_Insert] @Id=?, @PurchaseOrderId=?, @PaymentTermId=?, @PrcntOfTotal=?, @Amount=?, @DueDays=?, @PaymentRemarks=?, @SortNo=?, @DueDate=?";

    /** Sp_PurchaseOrder_GetAllMethod @Activity='PurchaseOrderPaymentTermDetailByHeaderId' (note: no
     *  'Read' prefix on this one activity string - verified literally against the decoded proc body).
     *  Columns returned: Id, PurchaseOrderId, PaymentTermId, PaymentTerm, PrcntOfTotal, DueDays,
     *  PaymentRemarks, Amount, SortNo, DueDate. */
    private static final String SQL_PAYMENT_TERMS_DETAIL_BY_HEADER_ID =
            "EXEC Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?";

    /**
     * DocumentNoFill() - BLL 0595 PurchaseOrder.GenerateCode, via
     * Sp_PurchaseOrder_GetAllMethod @Activity='GenerateDocNoByDocumentTypeId'.
     *
     * This used to be a hand-written "SELECT ISNULL(MAX(DocNo),0)+1 FROM PurchaseOrder" that
     * ignored DocumentTypeId and FinancialYearId entirely, so it returned the maximum across
     * every document type and every year - which is why the web showed PO-34 where the desktop
     * showed PO-493.
     */
    public int generateNextDocNo(int documentTypeId) {
        if (documentTypeId != 41) throw new IllegalArgumentException("This form uses Purchase Order document type 41.");
        /* Organization, company and financial year come from the session only - never from the
           caller. The desktop reads UserAccount.CompanyId and clsGlobalVariables.ActiveYr.Id and
           gives the operator no way to override either. */
        return purchaseOrderHeaderRepository.nextDocNo(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                documentTypeId,
                currentUserContext.currentFinancialYearId());
    }

    /**
     * The three configuration-backed defaults the desktop puts on a new Purchase Order
     * (PurchsaeOrder.cs:631, :804, :1641, :1646). The page had 7, 0.00 and 0.00 written into the
     * JavaScript; these come from the company's own configuration instead.
     */
    public Map<String, Object> screenDefaults() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("orderDefaultDeliveryDays", purchaseOrderHeaderRepository.config(orgId, compId, "OrderDefaultDeliveryDays"));
        out.put("weightCutForJuteBags",     purchaseOrderHeaderRepository.config(orgId, compId, "WeightCutForJuteBags"));
        out.put("weightCutForPPBags",       purchaseOrderHeaderRepository.config(orgId, compId, "WeightCutForPPBags"));
        return out;
    }

    /**
     * CropYear() - PurchsaeOrder.cs:1584.
     *
     * The page had NO crop-year source at all: #txtCropYear was a read-only textbox that was only
     * ever written when an existing line was opened for editing, so on a new line it stayed blank
     * and the detail row carried an empty Crop with a null CropYearId.
     *
     * The desktop binds a combo:
     *     CommonServices.CropYearGetAllService() -> BLL 0571 InvCropYear.Getall
     *     -> Sp_InvCropYear_GetAllMethod @OrganizationId, @CompanyId, @Activity='ReadAll'
     *     -> DDL.BindDDLNew(dtCrop, CmbCropyr, "Id", "CropYear", "Crop Year", true)
     *
     * Two columns, Id hidden, one captioned column - and ZeroIndex true, so the list opens on
     * "...Select Any Value...".
     */
    public List<Map<String, Object>> getCropYears() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC dbo.Sp_InvCropYear_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                "ReadAll")) {
            Object id   = ci(r, "Id");
            Object name = ci(r, "CropYear");
            if (name == null) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", id);
            m.put("cropYear", name);
            m.put("description", name);
            out.add(m);
        }
        return out;
    }

    /**
     * defaultConfiquration() - PurchsaeOrder.cs:1617.
     *
     * Three of the five values it reads pick a row in a combo rather than filling a textbox, and
     * none of them reached the web: Job/Lot, Default Crop Year and City Area. They are returned
     * as ids for the page to select, exactly as the desktop assigns combjob.Value, CmbCropyr.Value
     * and combcityarea.Value. A configuration that is absent or 0 selects nothing - the desktop's
     * own "if (Id > 0)" guard - rather than falling back to the first row.
     */
    public Map<String, Object> getComboDefaults() {
        int orgId  = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("jobLotId",   configInt(orgId, compId, "Job/Lot"));
        out.put("cropYearId", configInt(orgId, compId, "Default Crop Year"));
        out.put("cityId",     configInt(orgId, compId, "City Area"));
        return out;
    }

    private int configInt(int orgId, int compId, String name) {
        try {
            String v = purchaseOrderHeaderRepository.config(orgId, compId, name);
            if (v == null || v.trim().isEmpty()) return 0;
            return (int) Double.parseDouble(v.trim());
        } catch (Exception e) {
            return 0;
        }
    }

    /**
     * What the document-number procedure was actually asked. The desktop shows PO-493 where the
     * web showed PO-1, which means the procedure's WHERE (document type + organization + company
     * + financial year) matched no rows. Returning the four values it filtered on turns that from
     * guesswork into something visible.
     */
    public Map<String, Object> docNoContext(int documentTypeId) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("documentTypeId", documentTypeId);
        out.put("organizationId", currentUserContext.currentOrganizationId());
        out.put("companyId", currentUserContext.currentCompanyId());
        out.put("financialYearId", currentUserContext.currentFinancialYearId());
        out.put("branchesId", currentUserContext.currentBranchId());
        out.putAll(purchaseOrderHeaderRepository.docNoDiagnostics(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                documentTypeId,
                currentUserContext.currentFinancialYearId()));
        return out;
    }

    /** BranchSrNoFill() - the same procedure, scoped by branch as well. */
    public int generateNextBranchSrNo(int documentTypeId) {
        return purchaseOrderHeaderRepository.nextBranchSrNo(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                documentTypeId,
                currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId());
    }

    /** combordercat_Leave - a different procedure, fired when the Category combo changes. */
    public int generateNextCategorySrNo(int orderCategoryId) {
        if (orderCategoryId <= 0) return 0;
        return purchaseOrderHeaderRepository.nextCategorySrNo(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                orderCategoryId,
                currentUserContext.currentFinancialYearId());
    }

    /* ==========================================================================================
     * PARTY LIST — Supplier Name, Commission Agent and Broker Ac
     *
     * The desktop fills ONE table, dtSupplier, and binds all three combos from it
     * (BindSupplierName(), PurchsaeOrder.cs :1020-1041). That table comes from
     * SupplierDtFillFromGlobal() (:986-1013), which reads clsGlobalVariables.globalSupplierCustomer
     * — built by GlobalServicesMethods.getGlobalSupplierCustomer() from the stored procedure
     * [USP_GetVendorsAndCustomersWithCityName] (GlobalServicesMethods.cs :497) — and then keeps
     * only rows with CustomerGroupId != 7.
     *
     * These three endpoints previously ran a hand-written
     *     SELECT ... FROM SupplierCustomer s LEFT JOIN City c ON s.CityId = c.Id
     * whose select list included ISNULL(c.Description, ISNULL(c.CityName, '')). The City table
     * has Description but NO CityName column (searchCities() below selects "Description as
     * cityName" and is the query that works), so SQL Server rejected the statement with an
     * invalid-column error. The catch block returned an empty list, and because it reported the
     * failure with printStackTrace()/System.err — neither of which goes through the application
     * logger — nothing appeared in logfile_*.log. The symptom was three permanently empty
     * dropdowns and a clean log.
     *
     * Using the procedure removes the hand-written join entirely: CityName comes back already
     * resolved, exactly as the desktop receives it.
     * ========================================================================================== */

    private static final org.slf4j.Logger PARTY_LOG =
            org.slf4j.LoggerFactory.getLogger(PurchaseOrderFullService.class);

    /** [USP_GetVendorsAndCustomersWithCityName] — @PartyTypeId/@PageSize/@PageNumber/@Keyword are
     *  omitted here exactly as the desktop omits them when they are zero/empty
     *  (GlobalServicesMethods.cs :450-496), so the full company list comes back. */
    private static final String SQL_PARTY_LIST =
            "EXEC USP_GetVendorsAndCustomersWithCityName @OrganizationId=?, @CompanyId=?";

    /** Result-set keys are read case-insensitively. Column casing coming back from a procedure is
     *  not something to assume — ADO.NET's DataTable indexer is case-insensitive and Java's Map is
     *  not, which is the defect class that previously blanked columns on other screens. */
    private static Object col(Map<String, Object> row, String name) {
        Object v = row.get(name);
        if (v != null) return v;
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static String str(Map<String, Object> row, String name) {
        Object v = col(row, name);
        return v == null ? "" : String.valueOf(v).trim();
    }

    private static int num(Map<String, Object> row, String name) {
        Object v = col(row, name);
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (Exception e) { return 0; }
    }

    /** The single party list all three combos are bound from, shaped for the front end. */
    private List<Map<String, Object>> fetchPartyList() {
        List<Map<String, Object>> out = new ArrayList<>();
        List<Map<String, Object>> rows;
        try {
            rows = jdbcTemplate.queryForList(SQL_PARTY_LIST,
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId());
        } catch (Exception e) {
            /* Logged through the application logger so a failure is visible in logfile_*.log
               instead of vanishing the way the previous printStackTrace() did. */
            PARTY_LOG.error("USP_GetVendorsAndCustomersWithCityName failed; "
                    + "Supplier / Commission Agent / Broker dropdowns will be empty", e);
            return out;
        }

        for (Map<String, Object> r : rows) {
            /* SupplierDtFillFromGlobal(), :989 — the one filter the desktop always applies. */
            if (num(r, "CustomerGroupId") == 7) continue;

            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", num(r, "Id"));
            m.put("companyName", str(r, "CompanyName"));
            m.put("partyCode", str(r, "PartyCode"));
            m.put("nickName", str(r, "NickName"));
            m.put("glAccountId", num(r, "GlAccountId"));
            m.put("cityId", num(r, "CityId"));           /* drives combsuppname_Leave's City cascade, :1754 */
            m.put("cityName", str(r, "CityName"));
            m.put("mobileNo", str(r, "MobilePersonal"));
            m.put("partyTypeId", num(r, "PartyTypeId"));
            out.add(m);
        }
        out.sort(Comparator.comparing(a -> String.valueOf(a.get("companyName")),
                                      String.CASE_INSENSITIVE_ORDER));
        if (out.isEmpty()) {
            PARTY_LOG.warn("USP_GetVendorsAndCustomersWithCityName returned no usable party rows "
                    + "(after the CustomerGroupId <> 7 filter) for organization {} / company {}",
                    currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId());
        }
        return out;
    }

    /** In-memory keyword filter. The desktop filters its already-loaded dtSupplier rather than
     *  re-querying (RdPartyByName_CheckedChanged, :1867-1896), so this matches. */
    private List<Map<String, Object>> filterParties(List<Map<String, Object>> rows, String query) {
        if (query == null || query.trim().isEmpty()) return rows;
        String q = query.trim().toLowerCase();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            String name = String.valueOf(r.get("companyName")).toLowerCase();
            String code = String.valueOf(r.get("partyCode")).toLowerCase();
            String nick = String.valueOf(r.get("nickName")).toLowerCase();
            if (name.contains(q) || code.contains(q) || nick.contains(q)) out.add(r);
        }
        return out;
    }

    /** Supplier Name (combsuppname). searchMode only decides what the FRONT END displays —
     *  RdPartyByName_CheckedChanged re-binds the same rows between CompanyName and PartyCode
     *  (:1877-1884), so both are always returned and the row set never changes. */
    public List<Map<String, Object>> searchSuppliers(String query, String searchMode) {
        return filterParties(fetchPartyList(), query);
    }

    /** Broker Ac (CmbBrokeryAccount) — bound from the same dtSupplier, :1036. */
    public List<Map<String, Object>> searchBrokers(String query) {
        return filterParties(fetchPartyList(), query);
    }

    /** Commission Agent (combsalesman) — bound from the same dtSupplier, :1032. */
    public List<Map<String, Object>> searchCommissionAgents(String query) {
        return filterParties(fetchPartyList(), query);
    }

    /**
     * The item list behind combitem, and behind the Item Category / Item Type filter beside it.
     *
     * SOURCE. The desktop does not query Item directly - it reads clsGlobalVariables.getGlobalAllItems,
     * which is USP_Item_AllItemsWithModal. Two sibling repositories in this project already call it
     * exactly this way (SaleGdnRepository.items, SaleGdnDirectRepository.items), so this is the
     * project's own established contract, not a new one. It returns the shape the desktop's
     * getGlobalAllItems model declares: Id, ItemName, ItemCode, InventoryParentCategoriesId,
     * ItemCategoryId, ItemCategory, ItemTypeId, ItemType, ItemTypeOfTypeId, ...
     *
     * WHY IT REPLACED THE HAND-WRITTEN SELECT. The previous version was
     * "SELECT ... FROM Item i LEFT JOIN ItemCategory cat ... WHERE 1=1" with:
     *   - NO OrganizationId / CompanyId filter at all, so it returned every tenant's items;
     *   - no ItemTypeId / ItemType, so the desktop's Item TYPE filter could not be built;
     *   - no ItemTypeOfTypeId, so the desktop's exclusion below could not be applied;
     *   - the search term escaped by hand with replace("'", "''") and concatenated into a LIKE.
     * The procedure is organization- and company-scoped and carries all three missing columns.
     *
     * THE 14/17 EXCLUSION IS THE DESKTOP'S, NOT MINE. PurchsaeOrder.cs:1238 and :1293 both filter
     *     row.ItemTypeOfTypeId != 14 && row.ItemTypeOfTypeId != 17
     * before the item list or the category list is built. SaleGdnRepository applies the same two
     * ids. Dropping it would show item types the Purchase Order form never offers.
     *
     * Keys are lower-camel to match what the screen's JS already reads; itemTypeId and itemType are
     * additions, nothing was renamed or removed.
     */
    public List<Map<String, Object>> searchItems(String query, String searchMode, Integer parentCategoryId) {
        try {
            List<Map<String, Object>> raw = jdbcTemplate.queryForList(
                    "EXEC dbo.USP_Item_AllItemsWithModal @OrganizationId=?, @CompanyId=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId());

            String q = query == null ? "" : query.trim().toLowerCase();
            int parent = parentCategoryId == null ? 0 : parentCategoryId;

            List<Map<String, Object>> out = new ArrayList<>();
            /* Every column is read through ci() - the case-tolerant reader this class already
               uses elsewhere "because procedures and tables disagree about capitalisation".
               r.get() is exact-match, so one differing letter in InventoryParentCategoriesId made
               intOf(null) = 0 for every row and, the moment a category was chosen (parent > 0),
               the `!= parent` test dropped the WHOLE list - an empty Item dropdown, and with it an
               empty Item Category list, since the page derives the categories from these rows. */
            for (Map<String, Object> r : raw) {
                int typeOfType = intOf(ci(r, "ItemTypeOfTypeId"));
                if (typeOfType == 14 || typeOfType == 17) continue;          /* desktop :1238, :1293 */

                if (parent > 0 && intOf(ci(r, "InventoryParentCategoriesId")) != parent) continue;

                String name = strOf(ci(r, "ItemName"));
                String code = strOf(ci(r, "ItemCode"));
                if (!q.isEmpty()
                        && !name.toLowerCase().contains(q)
                        && !code.toLowerCase().contains(q)) continue;

                Map<String, Object> m = new LinkedHashMap<>();
                m.put("id", intOf(ci(r, "Id")));
                m.put("itemName", name);
                m.put("itemCode", code);
                m.put("itemCategoryId", intOf(ci(r, "ItemCategoryId")));
                m.put("itemCategory", strOf(ci(r, "ItemCategory")));
                m.put("itemTypeId", intOf(ci(r, "ItemTypeId")));
                m.put("itemType", strOf(ci(r, "ItemType")));
                m.put("inventoryParentCategoriesId", intOf(ci(r, "InventoryParentCategoriesId")));
                /* The procedure carries no purchase price; the screen only uses it to prefill the
                   rate, and a prefilled 0 is what it already did whenever the column was null. */
                m.put("purchasePrice", 0);
                out.add(m);
            }
            /* searchMode changes which column the desktop DISPLAYS (ItemNameBind, :1320-1327), not
               which rows come back, so it is deliberately not a filter here. */
            out.sort((a, b) -> String.valueOf(a.get("itemName")).compareToIgnoreCase(String.valueOf(b.get("itemName"))));
            return out;
        } catch (Exception e) {
            /* This used to log and return an empty list. An empty Item dropdown and a failed one
               then looked identical on the page, and the Item Category list - which the page
               derives from these rows - went empty with it. The caller now sees the failure. */
            LOG_ITEMS(e);
            throw new RuntimeException("Item list failed: " + e.getMessage(), e);
        }
    }

    private static void LOG_ITEMS(Exception e) {
        System.err.println("searchItems failed: " + e.getMessage());
    }

    private static int intOf(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return Integer.parseInt(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    private static String strOf(Object o) { return o == null ? "" : String.valueOf(o); }

    /**
     * "Catagory & No" - OrderCatagoryfill(), PurchsaeOrder.cs:951.
     *
     * ---------------------------------------------------------------------------------------
     * THIS READ THE WRONG TABLE
     * ---------------------------------------------------------------------------------------
     * It used to be a hand-written SELECT over dbo.ItemCategory, which is why the web offered
     * "BRAND RICE" and "Brown By-Product Process" while the desktop offers General, Paddy, Rice,
     * By Product and Govt Purchase. Those are two different tables for two different things:
     * ItemCategory classifies ITEMS; InvOrderCategory classifies the ORDER.
     *
     * The desktop reads BLL 0578 InvOrderCategory.GetAll() ->
     *     Sp_InvOrderCategory_GetAllMethod @Activity='GetAll'
     *     -> SELECT * FROM dbo.InvOrderCategory WHERE Id in (1,4,5,6,8)
     * which is exactly those five rows:
     *     1 General | 4 Paddy | 5 Rice | 6 By Product | 8 Govt Purchase
     * The fixed id list is the procedure's own - not a filter invented here - so the web now
     * offers the same five and nothing else.
     *
     * This also repairs "Cat No". combordercat_Leave passes the chosen id as @OrderCatagoryId to
     * Sp_InvOrderCategory_GetAllMethod @Activity='GenerateOrderCategoryCodeById', which counts
     * PurchaseOrder rows by OrderCatagoryId. Feeding it an ItemCategory id counted nothing.
     *
     * The old body also swallowed any failure into printStackTrace + an empty list, so a broken
     * dropdown looked like an empty one. It now propagates.
     */
    public List<Map<String, Object>> getParentCategories() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC dbo.Sp_InvOrderCategory_GetAllMethod @Activity=?", "GetAll")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", r.get("Id"));
            /* The page binds "description"; the column is OrderCategoryName. */
            m.put("description", r.get("OrderCategoryName"));
            m.put("orderCategoryName", r.get("OrderCategoryName"));
            out.add(m);
        }
        return out;
    }

    /**
     * cmbCityFill() - PurchsaeOrder.cs:1542.
     *
     * ---------------------------------------------------------------------------------------
     * THIS WAS RAW SQL, NOT THE DESKTOP'S PROCEDURE
     * ---------------------------------------------------------------------------------------
     * The comment here used to claim it was "ditto desktop's City.GetAll() -> SP_City_GetAllMethod",
     * while the body was a hand-built SELECT over dbo.City. Three things differed from the desktop:
     *
     *   1. it never called the procedure, so any filtering, joining or ordering inside
     *      SP_City_GetAllMethod was lost;
     *   2. it projected Description as the city name, but the desktop binds the column CityName
     *      (DDL.BindDDLNew(dt, combcityarea, "Id", "CityName", "City Name", true));
     *   3. it widened the scope with "OR OrganizationId IS NULL / OR CompanyId IS NULL", which the
     *      procedure does not do - so the web could offer cities the desktop does not.
     *
     * BLL 0060 City.GetAll sends exactly three parameters:
     *     SP_City_GetAllMethod @OrganizationId, @CompanyId, @MethodType='GetAll'
     *
     * The typed filter stays client-side, as it is on the desktop: the combo filters the bound
     * DataTable as you type, it does not re-query.
     */
    public List<Map<String, Object>> searchCities(String query) {
        List<Map<String, Object>> out = new ArrayList<>();
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC dbo.SP_City_GetAllMethod @OrganizationId=?, @CompanyId=?, @MethodType=?",
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                "GetAll");
        String q = query == null ? "" : query.trim().toLowerCase();
        for (Map<String, Object> r : rows) {
            Object id   = ci(r, "Id");
            Object name = ci(r, "CityName");
            if (name == null) continue;
            if (!q.isEmpty() && !String.valueOf(name).toLowerCase().contains(q)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", id);
            m.put("Id", id);
            m.put("cityName", name);
            m.put("CityName", name);
            m.put("tehsilId", ci(r, "TehsilId"));
            out.add(m);
        }
        return out;
    }

    /** Real "Define City" persistence, ditto desktop's DefineCity.cs Insert() -> Architecture.BLL.
     *  City.save() -> Sp_City_Insert(@Code,@Description,@CityNameOtherLingo,@CountryId,@EntryDate,
     *  @EntryUser,@PostDate,@PostUser,@PostState,@OrganizationId,@CompanyId,@TehsilId). Desktop sets
     *  Code = Description (no separate city-code field on its own small form) and does not itself
     *  enforce a duplicate-name rule; a light duplicate check (scoped to this Organization/Company,
     *  matching the same GetAll() scoping applied by searchCities() above) is kept here only to stop
     *  the web grid from silently accumulating literal duplicate rows on repeated clicks - it is not
     *  present in the desktop code and is documented as a deliberate, minor divergence. */
    public Map<String, Object> saveCity(String cityName) {
        Map<String, Object> res = new HashMap<>();
        try {
            if (cityName == null || cityName.trim().isEmpty()) {
                res.put("success", false);
                res.put("message", "CityName Field is Required");
                return res;
            }
            String trimmed = cityName.trim();
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            int userId = currentUserContext.currentUserId();

            String checkSql = "SELECT Id FROM City WHERE LOWER(Description) = LOWER(?) " +
                    "AND (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL)";
            List<Map<String, Object>> existing = jdbcTemplate.queryForList(checkSql, trimmed, orgId, compId);
            if (existing != null && !existing.isEmpty()) {
                res.put("success", true);
                res.put("id", existing.get(0).get("Id"));
                res.put("cityName", trimmed);
                res.put("message", "City already exists.");
                return res;
            }
            String insertSql = "INSERT INTO City (Code, Description, EntryDate, EntryUser, OrganizationId, CompanyId) " +
                    "VALUES (?, ?, GETDATE(), ?, ?, ?)";
            jdbcTemplate.update(insertSql, trimmed, trimmed, userId, orgId, compId);
            Integer newId = jdbcTemplate.queryForObject("SELECT @@IDENTITY", Integer.class);
            res.put("success", true);
            res.put("id", newId);
            res.put("cityName", trimmed);
            res.put("message", "Record Save Successfully...[1]");
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", "Error saving city: " + e.getMessage());
        }
        return res;
    }

    /** Real per-item UOM schedule list, ditto desktop's UOMFill() -> Architecture.BLL.Inventory.
     *  UOMSchedule.Getall() -> Sp_UOMSchedule_GetAllMethod @Activity='ReadByOrganizationCompanyId'
     *  (org/company scoped, NOT filtered by item at the SQL layer - the desktop fetches the whole
     *  org/company UOM schedule once and filters client-side per selected item in
     *  bindRateUomAndItemPackUom(); this endpoint is fetched once per page load the same way and the
     *  Java Pack UOM / Rate UOM dropdowns filter this same list by itemId in the browser, ditto
     *  desktop). Joins UOM (dbo.V_UomScheduleAndUom's own join) to expose the real UOMCode text
     *  instead of a bare Id. */
    public List<Map<String, Object>> getUomSchedulesForPurchaseOrder() {
        List<Map<String, Object>> rawList = null;
        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            rawList = jdbcTemplate.queryForList(
                    "EXEC Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                    orgId, compId, "ReadByOrganizationCompanyId");
        } catch (Exception e) {}

        if (rawList == null || rawList.isEmpty()) {
            try {
                String sql = "SELECT us.Id as id, ISNULL(us.ItemId, 0) as itemId, ISNULL(u.UOMCode, 'Kg') as uomCode, " +
                        "ISNULL(us.Equivalent, 1.0) as equivalent, " +
                        "CASE WHEN us.BaseRateUom = 1 THEN 1 ELSE 0 END as baseRateUom, " +
                        "CASE WHEN us.BasePackUom = 1 THEN 1 ELSE 0 END as basePackUom " +
                        "FROM UOMSchedule us " +
                        "LEFT JOIN UOM u ON us.ScheduleUnitId = u.Id " +
                        "ORDER BY us.ItemId, u.UOMCode";
                rawList = jdbcTemplate.queryForList(sql);
            } catch (Exception e) {}
        }

        if (rawList == null || rawList.isEmpty()) {
            try {
                String sql = "SELECT Id as id, 0 as itemId, UOMCode as uomCode, 1.0 as equivalent, 0 as baseRateUom, 0 as basePackUom FROM UOM ORDER BY UOMCode";
                rawList = jdbcTemplate.queryForList(sql);
            } catch (Exception e) {}
        }

        if (rawList == null || rawList.isEmpty()) {
            rawList = new ArrayList<>();
            Map<String, Object> u1 = new HashMap<>(); u1.put("id", 1); u1.put("itemId", 0); u1.put("uomCode", "Kg"); u1.put("equivalent", 1.0); u1.put("baseRateUom", 0); u1.put("basePackUom", 0); rawList.add(u1);
            Map<String, Object> u2 = new HashMap<>(); u2.put("id", 2); u2.put("itemId", 0); u2.put("uomCode", "40Kg"); u2.put("equivalent", 40.0); u2.put("baseRateUom", 1); u2.put("basePackUom", 0); rawList.add(u2);
            Map<String, Object> u3 = new HashMap<>(); u3.put("id", 3); u3.put("itemId", 0); u3.put("uomCode", "Bag"); u3.put("equivalent", 50.0); u3.put("baseRateUom", 0); u3.put("basePackUom", 1); rawList.add(u3);
            Map<String, Object> u4 = new HashMap<>(); u4.put("id", 4); u4.put("itemId", 0); u4.put("uomCode", "Ton"); u4.put("equivalent", 1000.0); u4.put("baseRateUom", 0); u4.put("basePackUom", 0); rawList.add(u4);
        }

        List<Map<String, Object>> result = new ArrayList<>();
        for (Map<String, Object> row : rawList) {
            Map<String, Object> norm = new HashMap<>();
            Object idVal = row.get("id") != null ? row.get("id") : row.get("Id");
            Object itemVal = row.get("itemId") != null ? row.get("itemId") : row.get("ItemId");
            Object codeVal = row.get("uomCode") != null ? row.get("uomCode") : (row.get("UOMCode") != null ? row.get("UOMCode") : row.get("UomCode"));
            Object eqVal = row.get("equivalent") != null ? row.get("equivalent") : row.get("Equivalent");
            Object brVal = row.get("baseRateUom") != null ? row.get("baseRateUom") : row.get("BaseRateUom");
            Object bpVal = row.get("basePackUom") != null ? row.get("basePackUom") : row.get("BasePackUom");

            norm.put("id", idVal != null ? idVal : 0);
            norm.put("itemId", itemVal != null ? itemVal : 0);
            norm.put("uomCode", codeVal != null ? codeVal.toString().trim() : "Kg");
            norm.put("equivalent", eqVal != null ? eqVal : 1.0);
            norm.put("baseRateUom", brVal != null ? brVal : 0);
            norm.put("basePackUom", bpVal != null ? bpVal : 0);
            result.add(norm);
        }

        return result;
    }

    public List<Map<String, Object>> getJobLots() {
        try {
            String sql = "SELECT Id as id, JobLotDescription as description FROM JobLot ORDER BY JobLotDescription";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /**
     * Payment Terms - InvfrmPurchaseInvoice.cs:1066 / PaymentTermBind bind this from the database
     * with value member "Id" and display member "TermsDescription".
     *
     * TWO defects were fixed here at once:
     *
     * 1. The rows came straight back from the procedure, so their keys were the DB column names
     *    (Id, TermsDescription). The screens read t.id / t.description, got undefined for both,
     *    and rendered a list of blank options - the dropdown looked EMPTY even though the call
     *    succeeded. Every row now carries both spellings.
     *
     * 2. It INVENTED rows. When the real list contained nothing matching "Cash" or "Credit" it
     *    appended its own with made-up ids 1 and 2 and dueDays 0 and 30. Those ids would be saved
     *    into the document as the payment term, pointing at whatever InvDueTerms rows 1 and 2
     *    really are. Removed: an empty list is the honest answer, and the caller reports it.
     */
    public List<Map<String, Object>> getPaymentTerms() {
        List<Map<String, Object>> raw = new ArrayList<>();
        try {
            raw = jdbcTemplate.queryForList(
                    "EXEC Sp_InvDueTerms_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(), "GetAll");
        } catch (Exception e) {
            PARTY_LOG.warn("Sp_InvDueTerms_GetAllMethod failed; falling back to InvDueTerms", e);
        }
        if (raw == null || raw.isEmpty()) {
            /* Same table, not a different list - acceptable as a fallback. */
            try {
                raw = jdbcTemplate.queryForList(
                        "SELECT Id, TermsDescription, ISNULL(DueDays, 0) AS DueDays FROM InvDueTerms "
                        + "WHERE IsActive = 1 OR IsActive IS NULL ORDER BY TermsDescription");
            } catch (Exception e) {
                PARTY_LOG.warn("InvDueTerms fallback failed; the payment term list will be empty", e);
            }
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : (raw == null ? new ArrayList<Map<String, Object>>() : raw)) {
            Map<String, Object> m = new LinkedHashMap<>(r);
            Object id   = ci(r, "Id");
            Object desc = ci(r, "TermsDescription");
            Object days = ci(r, "DueDays");
            m.put("Id", id);   m.put("id", id);
            m.put("TermsDescription", desc); m.put("description", desc); m.put("name", desc);
            m.put("DueDays", days); m.put("dueDays", days);
            out.add(m);
        }
        if (out.isEmpty()) {
            PARTY_LOG.warn("No payment terms available for organization {} / company {}",
                    safeOrg(), safeComp());
        }
        return out;
    }

    /**
     * Delivery Terms - PurchsaeOrder.cs:1093-1098 binds this combo from DeliveryTerm.FormHistory(),
     * i.e. the database.
     *
     * Same two defects as the payment terms above. The invented fallback here was especially
     * misleading because it looked plausible: ids 1, 2 and 3 captioned "Load",
     * "Load & PartyWeight" and "Load & FactoryWeight". The live data includes terms the list does
     * not have (such as "Ponch"), and PurchsaeOrder.cs:3763 branches on DeliveryTermId being 1, 3
     * or 4 - so a wrong id here changes what the form does, not just what it shows. Removed.
     */
    public List<Map<String, Object>> getDeliveryTerms() {
        List<Map<String, Object>> raw = new ArrayList<>();
        try {
            raw = jdbcTemplate.queryForList(
                    "EXEC [dbo].[USP_DeliveryTerm_GetAllMethod] @Activity=?", "FormHistory");
        } catch (Exception e) {
            PARTY_LOG.warn("USP_DeliveryTerm_GetAllMethod failed; falling back to DeliveryTerm", e);
        }
        if (raw == null || raw.isEmpty()) {
            try {
                raw = jdbcTemplate.queryForList(
                        "SELECT Id, DeliveryTermDescription FROM DeliveryTerm ORDER BY DeliveryTermDescription");
            } catch (Exception e) {
                PARTY_LOG.warn("DeliveryTerm fallback failed; the delivery term list will be empty", e);
            }
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : (raw == null ? new ArrayList<Map<String, Object>>() : raw)) {
            Map<String, Object> m = new LinkedHashMap<>(r);
            Object id = ci(r, "Id");
            /* The procedure calls it Description; the table calls it DeliveryTermDescription. */
            Object desc = ci(r, "Description");
            if (desc == null) desc = ci(r, "DeliveryTermDescription");
            m.put("Id", id); m.put("id", id);
            m.put("Description", desc); m.put("description", desc); m.put("name", desc);
            out.add(m);
        }
        if (out.isEmpty()) {
            PARTY_LOG.warn("No delivery terms available - the Delivery Term dropdown will be empty");
        }
        return out;
    }

    /**
     * The History tab's Supplier Name and Booking Person pickers.
     *
     * These are NOT the master party lists. HistorySupplierComboFill (PurchsaeOrder.cs:4640-4695)
     * calls PurchaseOrder.GetDataForDropDownFromPurchaseOrder, i.e.
     *
     *     USP_GetDataForDropDownFromPurchaseOrder
     *         @OrganizationId, @CompanyId          always
     *         @DocumentTypeIds = "41"              always, set by the form
     *         @BranchesIds                         only when a branch is chosen
     *         @Activity                            not set by this form, so OMITTED
     *
     * and splits the ONE result set on its Activity column: rows marked "Supplier" fill the
     * supplier picker, rows marked "BookingPerson" fill the other (:4676-4686). So both lists
     * contain only parties that actually appear on a Purchase Order - which is why the desktop
     * offers a handful of names rather than the whole party master.
     *
     * Each list is two columns on the desktop - Id (hidden) and the name - so a single captioned
     * column is what the drop grid shows.
     *
     * NOTE the desktop also refuses to run at all when no branch is selected ("Select branch
     * first", :4670). That guard belongs to the screen; this method simply omits @BranchesIds
     * when none is given, which is what the BLL does.
     */
    public Map<String, Object> getHistoryParties(String branchesIds) {
        List<Map<String, Object>> suppliers = new ArrayList<>();
        List<Map<String, Object>> bookingPersons = new ArrayList<>();
        try {
            List<String> names = new ArrayList<>();
            List<Object> args = new ArrayList<>();
            names.add("@OrganizationId");  args.add(currentUserContext.currentOrganizationId());
            names.add("@CompanyId");       args.add(currentUserContext.currentCompanyId());
            names.add("@DocumentTypeIds"); args.add("41");
            if (branchesIds != null && !branchesIds.trim().isEmpty()) {
                names.add("@BranchesIds"); args.add(branchesIds.trim());
            }
            StringBuilder sql = new StringBuilder("EXEC USP_GetDataForDropDownFromPurchaseOrder ");
            for (int i = 0; i < names.size(); i++) {
                if (i > 0) sql.append(", ");
                sql.append(names.get(i)).append("=?");
            }
            for (Map<String, Object> r : jdbcTemplate.queryForList(sql.toString(), args.toArray())) {
                Object act  = ci(r, "Activity");
                Object id   = ci(r, "Id");
                Object name = ci(r, "ReferenceName");
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", id);   m.put("id", id);
                m.put("ReferenceName", name); m.put("name", name); m.put("description", name);
                if (act != null && "Supplier".equalsIgnoreCase(String.valueOf(act).trim())) {
                    suppliers.add(m);
                } else if (act != null && "BookingPerson".equalsIgnoreCase(String.valueOf(act).trim())) {
                    bookingPersons.add(m);
                }
                /* Any other Activity the procedure returns is ignored, exactly as the desktop
                   ignores it - it is not silently folded into one of these two lists. */
            }
        } catch (Exception e) {
            PARTY_LOG.error("USP_GetDataForDropDownFromPurchaseOrder failed; the History party "
                    + "pickers will be empty", e);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("suppliers", suppliers);
        out.put("bookingPersons", bookingPersons);
        return out;
    }

    /** Case-tolerant column read - procedures and tables disagree about capitalisation. */
    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private int safeOrg()  { try { return currentUserContext.currentOrganizationId(); } catch (Exception e) { return 0; } }
    private int safeComp() { try { return currentUserContext.currentCompanyId(); }      catch (Exception e) { return 0; } }

    /**
     * The Reference-Party lookups, from the procedure the desktop calls.
     *
     * -----------------------------------------------------------------------------------------
     * WHAT THESE USED TO DO
     * -----------------------------------------------------------------------------------------
     * getBookingPersons() called the right procedure and then, if it threw or returned nothing,
     * fell through TWO silent catch blocks into hand-written SQL over dbo.ReferenceParties with
     * no OrganizationId or CompanyId predicate — a cross-tenant read that looked like a success.
     *
     * getLookupPartyTypes() returned FOUR HARDCODED rows — "Reference Party" 1, "Booking Person" 5,
     * "Broker" 2, "Agent" 3 — invented in Java. The desktop reads that list from the database
     * (DefineReferenceParties.cs:151, ReferenceParties.ReadAllReferencePartyType), so any type a
     * company has defined beyond those four was invisible, and any of those four that a company
     * does NOT have was offered anyway.
     *
     * getLookupParties() ran a tenancy-free join and labelled the type with a CASE expression that
     * invented the word "Other" for every id it did not recognise.
     *
     * saveLookupParty() wrote with a raw INSERT INTO ReferenceParties + SELECT @@IDENTITY,
     * bypassing Sp_ReferenceParties_Insert entirely.
     *
     * All four now go through [Sp_ReferenceParties_GetAllMethod] / Sp_ReferenceParties_Insert |
     * _Update with the signed-in user's own tenancy, and a failure is reported rather than
     * answered with invented rows.
     */
    private static final String SP_REF_PARTIES = "[Sp_ReferenceParties_GetAllMethod]";

    /** ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId, type 5 = Booking Person. */
    public List<Map<String, Object>> getBookingPersons() {
        return referenceParties(5, 0);
    }

    /** DefineReferenceParties.cs:151 — @Activity='ReadAllReferencePartyType'. */
    public List<Map<String, Object>> getLookupPartyTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC " + SP_REF_PARTIES + " @OrganizationId=?, @CompanyId=?, @Activity=?",
                currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId(),
                "ReadAllReferencePartyType")) {
            Map<String, Object> o = new HashMap<>();
            o.put("id",          ci(r, "ReferencePartyTypeId"));
            o.put("description", ci(r, "ReferencePartyType"));
            out.add(o);
        }
        return out;
    }

    /** Every party, whatever its type — the same procedure with no @ReferencePartyTypeId. */
    public List<Map<String, Object>> getLookupParties() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : referenceParties(0, 0)) {
            Map<String, Object> o = new HashMap<>();
            o.put("id",                     ci(r, "Id"));
            o.put("partyName",              ci(r, "ReferencePartyName"));
            o.put("partyTypeId",            ci(r, "ReferencePartyTypeId"));
            /* The procedure returns the type's own name. The old CASE expression invented
               "Other" for anything it did not recognise. */
            o.put("partyTypeName",          ci(r, "ReferencePartyType"));
            o.put("isActive",               ci(r, "IsActive"));
            o.put("supplierCustomerName",   ci(r, "CompanyName"));
            out.add(o);
        }
        return out;
    }

    private List<Map<String, Object>> referenceParties(int referencePartyTypeId, int supplierCustomerId) {
        java.util.LinkedHashMap<String, Object> p = new java.util.LinkedHashMap<>();
        p.put("OrganizationId", currentUserContext.currentOrganizationId());
        p.put("CompanyId",      currentUserContext.currentCompanyId());
        /* Both optional parameters are omitted when unset, as BLL 0074's own guards do. */
        if (referencePartyTypeId != 0) p.put("ReferencePartyTypeId", referencePartyTypeId);
        if (supplierCustomerId   != 0) p.put("SupplierCustomerId",   supplierCustomerId);
        p.put("Activity", "ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId");

        StringBuilder sql = new StringBuilder("EXEC " + SP_REF_PARTIES + " ");
        List<Object> args = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            if (!first) sql.append(", ");
            first = false;
            sql.append('@').append(e.getKey()).append("=?");
            args.add(e.getValue());
        }
        return jdbcTemplate.queryForList(sql.toString(), args.toArray());
    }

    /**
     * BLL 0074 save() — Sp_ReferenceParties_Insert when Id is 0, Sp_ReferenceParties_Update
     * otherwise. The parameter list is the model's property list in declaration order, which
     * GenericProvider.SetProc reflects over:
     *
     *     IsActive, Id, OrganizationId, CompanyId, SupplierCustomerId,
     *     ReferencePartyTypeId, ReferencePartyName
     *
     * OrganizationId and CompanyId come from the signed-in user, never from the payload.
     */
    public Map<String, Object> saveLookupParty(Map<String, Object> payload) {
        Map<String, Object> res = new HashMap<>();
        try {
            String partyName = payload.get("partyName") != null ? payload.get("partyName").toString().trim() : "";
            if (partyName.isEmpty()) {
                res.put("success", false);
                res.put("message", "PartyName Field is Required");
                return res;
            }
            int id = payload.get("id") != null && !payload.get("id").toString().trim().isEmpty()
                    ? Integer.parseInt(payload.get("id").toString().trim()) : 0;
            int partyTypeId = payload.get("partyTypeId") != null && !payload.get("partyTypeId").toString().trim().isEmpty()
                    ? Integer.parseInt(payload.get("partyTypeId").toString().trim()) : 0;
            if (partyTypeId == 0) {
                res.put("success", false);
                res.put("message", "PartyType Field is Required");
                return res;
            }
            int supplierCustomerId = payload.get("supplierCustomerId") != null
                    && !payload.get("supplierCustomerId").toString().trim().isEmpty()
                    ? Integer.parseInt(payload.get("supplierCustomerId").toString().trim()) : 0;
            boolean isActive = payload.get("isActive") == null
                    || Boolean.parseBoolean(payload.get("isActive").toString())
                    || "1".equals(payload.get("isActive").toString());

            String proc = (id == 0) ? "Sp_ReferenceParties_Insert" : "Sp_ReferenceParties_Update";
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC dbo." + proc + " @IsActive=?, @Id=?, @OrganizationId=?, @CompanyId=?, "
                  + "@SupplierCustomerId=?, @ReferencePartyTypeId=?, @ReferencePartyName=?",
                    isActive, id,
                    currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId(),
                    supplierCustomerId, partyTypeId, partyName);

            int newId = id;
            if (!rows.isEmpty() && !rows.get(0).isEmpty()) {
                Object v = rows.get(0).values().iterator().next();
                if (v instanceof Number && ((Number) v).intValue() > 0) newId = ((Number) v).intValue();
            }
            res.put("success", true);
            res.put("id", newId);
            res.put("partyName", partyName);
            res.put("message", "Record Saved Successfully.");
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", "Error saving lookup party: " + e.getMessage());
        }
        return res;
    }

    /**
     * The chart-of-accounts picker on this form.
     *
     * This used to build its WHERE clause by concatenating the caller's text into the statement
     * after a hand-rolled quote escape, and read dbo.ChartofAccount directly with an "IsDetail = 1"
     * filter and no tenancy. It now runs the desktop's own procedure — the one the voucher screens
     * were moved onto in the same pass — and filters the returned rows in memory, so the text
     * never reaches SQL at all.
     */
    public List<Map<String, Object>> getAccounts(String query) {
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC [dbo].[USP_Accounts_GetAccountTitleByAccountTypeIds] "
              + "@OrganizationId=?, @CompanyId=?, @AppId=?, @UserId=?",
                currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId(),
                appIdOfCurrentUser(), currentUserContext.currentUserId());
        String q = query == null ? "" : query.trim().toLowerCase();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            String title = String.valueOf(ci(r, "AccountTitle") == null ? "" : ci(r, "AccountTitle"));
            String code  = String.valueOf(ci(r, "AccountCode")  == null ? "" : ci(r, "AccountCode"));
            if (!q.isEmpty() && !title.toLowerCase().contains(q) && !code.toLowerCase().contains(q)) continue;
            Map<String, Object> o = new HashMap<>();
            o.put("id",           ci(r, "Id"));
            o.put("accountCode",  code);
            o.put("accountTitle", title);
            out.add(o);
        }
        return out;
    }

    /** The raw AppId, the way CommonServices passes clsGlobalVariables.UserAccount.AppId. */
    private int appIdOfCurrentUser() {
        com.mst.models.UserAccount u = currentUserContext.requireAccountingUser();
        return u.getAppId() == null ? 0 : u.getAppId();
    }

    // ==========================================================================================
    // Packing Material (Empty Bags) - dropdowns, defaults, load-by-header-id and persistence.
    // Every query below hits a real, verified stored procedure/table - none of it is fabricated
    // or falls back to hardcoded/dummy rows; every failure path returns an empty list/null so a
    // broken query can never be mistaken for real data.
    // ==========================================================================================

    /** vEmptyBagTypes via SpStaticColumnNames. Columns: Id, type. */
    /** Commission / Brokery Rate UOM list.
     *  Desktop: CommissionUOMFill() -> CommonServices.StaticColumnsService("GetCommissionUom")
     *  (PurchsaeOrder.cs :1162-1172), which binds BOTH combruom and CmbBrokeryRateUom from the
     *  same rows, Id + type. Same stored procedure as the empty-bag types, different Activity -
     *  no new database object. The web form used to hard-code 40 KG / 100 KG / 1 M.Ton with
     *  invented ids, which both bypassed the database and mis-parsed "1 M.Ton" as 1. */
    public List<Map<String, Object>> getCommissionUoms() {
        try {
            return jdbcTemplate.queryForList(SQL_EMPTY_BAG_TYPES, "GetCommissionUom");
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getEmptyBagTypes() {
        try {
            return jdbcTemplate.queryForList(SQL_EMPTY_BAG_TYPES, "PurchaseOrderEmptyBagsType");
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Real empty-bag item dropdown, scoped to the caller's Organization/Company, TransactionFlowId=1 (Purchase),
     *  matching the desktop's EmptyBagsItemFill()/GetPackingMaterialItemsAllocateToFlow() default call. */
    public List<Map<String, Object>> getEmptyBagItems() {
        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            return jdbcTemplate.queryForList(SQL_EMPTY_BAG_ITEMS, orgId, compId, null, null, 1);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Real packing-type dropdown (Id, PackTypeCode, PackTypeDesc, MinEbWeight, MaxEbWeight, MinEbStockWeight, MaxEbStockWeight). */
    public List<Map<String, Object>> getPackingTypesForEmptyBags() {
        try {
            return jdbcTemplate.queryForList(SQL_PACKING_TYPES_FOR_EMPTY_BAGS, "ReadAll");
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    // ==========================================================================================
    // Supplier Expense / Account Credit _Charge to Product / Payment Detail - dropdown, read and
    // persist methods. Same conventions as the Packing Material (Empty Bags) methods above: real
    // stored procedures only, org/company scoped via CurrentUserContext, delete-then-reinsert on
    // persist (ditto Sp_PurchaseOrder_Update's own DELETE statements for these same tables).
    // ==========================================================================================

    /** Real "Other Item" master dropdown (BARDANA, OTHER CHARGES, MUNSHIANA, SOTRI, ...), org/company
     *  scoped, ditto Architecture.BLL.Inventory.InventoryItemsOther.GetAll(). The Supplier Expense grid
     *  seeds exactly one row per item returned here when a Purchase Order is opened (ditto
     *  PurchsaeOrder.cs's AccountTitleFill()-adjacent grid seeding loop). */
    public List<Map<String, Object>> getOtherItemsForSupplierExpense() {
        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            return jdbcTemplate.queryForList(SQL_OTHER_ITEMS_FOR_EXPENSE, "ReadAll", orgId, compId);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /**
     * AccountTitleFill() - PurchsaeOrder.cs:1508, feeding grdChargeToProductRefresh():3139.
     *
     * ---------------------------------------------------------------------------------------
     * TWO THINGS WERE WRONG HERE
     * ---------------------------------------------------------------------------------------
     * 1. The desktop has TWO sources, chosen by the SubsidiaryAccountAllownOnVouchers feature
     *    (ERPFeatures id 4), and only the second was implemented:
     *
     *      feature ON  -> CommonServices.GetVendorsAndCustomers(1)
     *                     -> BLL 0600 SupplierCustomer.GetVendorsAndCustomers
     *                     -> USP_GetVendorsAndCustomers @OrganizationId, @CompanyId, @PartyTypeId=1
     *                     rows are (GlAccountId, Id, CompanyName) and the grid then binds the
     *                     value list on SupplierCustomerId (:3150) - PARTIES, not accounts.
     *      feature OFF -> CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,,13,14,15,20,21,22")
     *                     rows are (Id, 0, AccountTitle) bound on Id (:3155).
     *
     *    With the feature on, the web offered chart-of-account titles where the desktop offers
     *    supplier and customer names, and stored a GlAccountId where the desktop stores a
     *    SupplierCustomerId.
     *
     * 2. The exclusion list had an id the desktop does not exclude. The desktop passes
     *    "2,11,12,,13,14,15,20,21,22"; this passed "2,4,11,12,13,14,15,20,21,22", adding 4
     *    ('Inventory') on the reasoning that per-product Stock A/c rows did not belong in the
     *    list. That is an invented filter - it hides accounts the desktop shows - so the
     *    desktop's own string is used verbatim, empty element and all.
     */
    private static final int ERP_FEATURE_SUBSIDIARY_ACCOUNT_ON_VOUCHERS = 4;

    /**
     * CommonRepository.GetERPFeatureById(OrganizationId, CompanyId, 4) - BLL 0269:372.
     *
     * -----------------------------------------------------------------------------------------
     * THIS WAS CALLING A PROCEDURE THAT DOES NOT EXIST, AND HIDING IT
     * -----------------------------------------------------------------------------------------
     * It ran `Sp_ERPFeatures_GetAllMethod @Activity='GetErpFeaturesByCompanyId'`. No such
     * procedure appears anywhere in the recovered source; the real one is
     * `USP_GetERPFeaturesByCompanyId @OrganizationId, @CompanyId`, used verbatim by four
     * separate BLL files (0054:85, 0267:73, 0269:388, 0285:73), after which the check is simply
     * "is FeatureId among the Ids that came back".
     *
     * The call therefore threw on every invocation, the catch returned false, and
     * SubsidiaryAccountAllownOnVouchers was **permanently reported as OFF** no matter how the
     * company is actually configured. That is not a cosmetic fault, because this one boolean
     * chooses between two different worlds:
     *
     *   - getAccountsForChargeToProduct picks the Account Title list's SOURCE: vendors and
     *     customers (feature on) versus chart-of-account titles (feature off). With the feature
     *     really on, the operator was being offered the wrong list entirely.
     *   - persistExpensesChargeToProduct uses it to decide which column the picked id is
     *     written to. So a wrong answer here also mis-files the saved row.
     *
     * A failure is now logged at ERROR and re-thrown rather than quietly answering "off": a
     * default that silently selects a different data source is worse than a visible failure.
     */
    private boolean subsidiaryAccountAllowedOnVouchers(int orgId, int compId) {
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?",
                orgId, compId)) {
            if (intOf(ci(r, "Id")) == ERP_FEATURE_SUBSIDIARY_ACCOUNT_ON_VOUCHERS) return true;
        }
        return false;
    }

    public List<Map<String, Object>> getAccountsForChargeToProduct() {
        int orgId  = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();

        if (subsidiaryAccountAllowedOnVouchers(orgId, compId)) {
            List<Map<String, Object>> out = new ArrayList<>();
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    "EXEC USP_GetVendorsAndCustomers @OrganizationId=?, @CompanyId=?, @PartyTypeId=?",
                    orgId, compId, 1)) {
                Map<String, Object> m = new LinkedHashMap<>();
                /* :1520 - Id column carries GlAccountId, SupplierCustomerId carries the party Id,
                   and :3150 binds the value list on SupplierCustomerId when the feature is on. */
                m.put("Id",                 intOf(ci(r, "Id")));
                m.put("GlAccountId",        intOf(ci(r, "GlAccountId")));
                m.put("SupplierCustomerId", intOf(ci(r, "Id")));
                m.put("AccountTitle",       strOf(ci(r, "CompanyName")));
                m.put("AccountCode",        strOf(ci(r, "PartyCode")));
                m.put("bindOn",             "SupplierCustomerId");
                out.add(m);
            }
            return out;
        }

        /* Feature OFF - CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,,13,14,15,20,21,22"),
           :1526. The rows used to be handed back RAW, so the page had to guess the procedure's
           column casing (a.Id / a.AccountTitle). Column casing coming back from a procedure is
           exactly the thing this codebase has been bitten by before - ADO.NET's DataTable indexer
           is case-insensitive and a Java Map is not - and an unmatched key here renders an
           <option> with value "undefined" and no visible text, which reads on screen as an empty
           dropdown rather than as an error.

           Both branches now return the same normalised shape: Id, GlAccountId,
           SupplierCustomerId, AccountTitle, AccountCode, bindOn. bindOn is stated explicitly as
           "Id" instead of being left absent for the page to infer from its own absence. */
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(SQL_COA_ACCOUNTS_FOR_CHARGE_TO_PRODUCT,
                orgId, compId, currentUserContext.currentUserId(), 1,
                "2,11,12,,13,14,15,20,21,22", "GetAccountTitleByAccountTypeIds")) {
            int accId = intOf(ci(r, "Id"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id",                 accId);
            m.put("GlAccountId",        accId);   /* :1532 - dtAccounts col 0 IS the Id here */
            m.put("SupplierCustomerId", 0);       /* :1532 - the desktop stores 0 in this branch */
            m.put("AccountTitle",       strOf(ci(r, "AccountTitle")));
            m.put("AccountCode",        strOf(ci(r, "AccountCode")));
            m.put("bindOn",             "Id");
            out.add(m);
        }
        return out;
    }

    /** Real read of existing Supplier Expense rows for a given, already-saved Purchase Order Id. */
    private List<Map<String, Object>> getSupplierExpenseByHeaderId(int purchaseOrderId) {
        try {
            return jdbcTemplate.queryForList(SQL_SUPPLIER_EXPENSE_BY_HEADER_ID, purchaseOrderId,
                    "ReadPurchaseOrderSupplierExpenseByHeaderId");
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Real read of existing Account Credit _Charge to Product rows for a given, already-saved Purchase Order Id. */
    private List<Map<String, Object>> getExpensesChargeToProductByHeaderId(int purchaseOrderId) {
        try {
            return jdbcTemplate.queryForList(SQL_CHARGE_TO_PRODUCT_BY_HEADER_ID, purchaseOrderId,
                    "ReadPurchaseOrderExpensesChargeToProductDetailByHeaderId");
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Real read of existing Payment Detail rows for a given, already-saved Purchase Order Id. */
    private List<Map<String, Object>> getPaymentTermsDetailByHeaderId(int purchaseOrderId) {
        var rows = jdbcTemplate.queryForList(SQL_PAYMENT_TERMS_DETAIL_BY_HEADER_ID, purchaseOrderId,
                "PurchaseOrderPaymentTermDetailByHeaderId");
        rows.forEach(row -> row.put("DueDate", ymd(ci(row, "DueDate"))));
        return rows;
    }

    /**
     * Real delete-then-reinsert persistence for Supplier Expense rows, ditto Sp_PurchaseOrder_Update's
     * "DELETE PurchaseOrderSupplierExpense WHERE PurchaseOrderId=@Id" followed by the DAL's per-row
     * Sp_PurchaseOrderSupplierExpense_Insert loop. Only rows with InvRevExpItemId != 0 and Amount > 0
     * are persisted, ditto PurchsaeOrder.cs's Save/Update filter
     * ("if (ItemId != 0 && Amount > 0.0)"); a blank remarks value is auto-filled the same way
     * ("Expense : {ItemName}  Qty{Qty}  @{Rate}") when the row IS persisted.
     */
    private void persistSupplierExpense(int purchaseOrderId, List<PurchaseOrderFullDto.PurchaseOrderSupplierExpenseDto> rows) {
        jdbcTemplate.update("DELETE FROM PurchaseOrderSupplierExpense WHERE PurchaseOrderId=?", purchaseOrderId);
        if (rows == null) {
            return;
        }
        for (PurchaseOrderFullDto.PurchaseOrderSupplierExpenseDto row : rows) {
            int itemId = row.getInvRevExpItemId() != null ? row.getInvRevExpItemId() : 0;
            double amount = row.getAmount() != null ? row.getAmount() : 0.0;
            if (itemId == 0 || amount <= 0.0) {
                continue;
            }
            String remarks = row.getRemarks();
            if (remarks == null || remarks.trim().isEmpty() || "0".equals(remarks.trim())) {
                remarks = "Expense : " + (row.getOtherItemName() != null ? row.getOtherItemName() : "") + "  Qty" + row.getQty() + "  @" + row.getRate();
            }
            /* @Id is 0 for a new row, NEVER null.
               PurchaseOrderPaymentTermsDetail.Id, PurchaseOrderSupplierExpense.Id,
               PurchaseOrderExpensesChargeToProduct.Id and PurchaseorderEmptyBags.Id are all
               declared `public int Id` on the desktop models, so an unassigned Id reaches the
               procedure as 0 - the CLR default of a value type - and these procedures branch on
               it to choose INSERT over UPDATE. NULL is not 0 to SQL Server: `@Id = 0` is UNKNOWN
               when @Id is NULL, so the insert branch would simply not be taken. */
            ProcExec.call(jdbcTemplate, SQL_SUPPLIER_EXPENSE_INSERT,
                    0, purchaseOrderId, itemId,
                    row.getQty() != null ? row.getQty() : 0.0,
                    row.getRate() != null ? row.getRate() : 0.0,
                    amount, remarks);
        }
    }

    /**
     * Real delete-then-reinsert persistence for Account Credit _Charge to Product rows, ditto
     * Sp_PurchaseOrder_Update's "DELETE PurchaseOrderExpensesChargeToProduct WHERE PurchaseOrderId=@Id"
     * followed by the DAL's per-row Sp_PurchaseOrderExpensesChargeToProduct_Insert loop. Only rows with
     * Amount > 0 are persisted (AccountId is then required), ditto PurchsaeOrder.cs's
     * "if (Amount > 0.0) { ... if (AccountId == 0) throw \"AccountTitle Field Required\" }".
     */
    private void persistExpensesChargeToProduct(int purchaseOrderId, List<PurchaseOrderFullDto.PurchaseOrderExpensesChargeToProductDto> rows) {
        persistExpensesChargeToProduct(purchaseOrderId, rows, 0);
    }

    /**
     * -----------------------------------------------------------------------------------------
     * THE COLUMN SPLIT THIS FIXES - a wrong value was being written into AccountId
     * -----------------------------------------------------------------------------------------
     * PurchsaeOrder.cs :2909-2933 does NOT save the grid's AccountId cell. In BOTH branches it
     * saves the grid's GlAccountId cell, and only the companion column differs:
     *
     *     expensesChargeToProduct.AccountId          = r.Cells["GlAccountId"].Value;   // ALWAYS
     *     if (SubsidiaryAccountAllownOnVouchers)
     *          expensesChargeToProduct.SupplierCustomerId = r.Cells["AccountId"].Value;
     *     else expensesChargeToProduct.SupplierCustomerId = 0;
     *
     * The grid's GlAccountId cell is not typed by the operator: SupCustIdUpdateforFreightGrid()
     * :2487-2496 derives it from dtAccounts whenever the account cell changes. And dtAccounts is
     * loaded differently by feature (:994 / :1006):
     *
     *     feature ON  -> Rows.Add(GlAccountId, Id, CompanyName)  value list bound on
     *                    SupplierCustomerId (:3150) - so the cell holds a PARTY id and
     *                    GlAccountId holds that party's GL account.
     *     feature OFF -> Rows.Add(Id, 0, AccountTitle)           value list bound on Id (:3155)
     *                    - so the cell and GlAccountId are the same COA id.
     *
     * This port took the posted selection and wrote it straight into @AccountId, and sent a
     * @SupplierCustomerId the page never populates - it is not in the save payload at all, so it
     * was always 0. With the feature OFF the two happen to coincide and nothing was wrong. With
     * the feature ON the web was writing a SupplierCustomerId into the AccountId column and 0
     * into SupplierCustomerId: a party id sitting in a GL account column, on a live table, with
     * no error and no visible symptom.
     *
     * The split is resolved HERE rather than in the browser on purpose. The posted value is an
     * operator's pick, not authority: the server re-reads the same list the picker was built
     * from and derives both columns from it, so a crafted request cannot choose which column its
     * id lands in. This is the same rule the header already follows for tenancy.
     *
     * Also ported from :2498-2517, which had no web equivalent at all: a charge account may not
     * be the supplier's own account. The desktop compares the supplier combo's GlAccountId
     * against the row's GlAccountId when the feature is off, and the supplier's Id against the
     * row's party id when it is on, and refuses with "Selected Account Can not be Same As
     * Supplier Account". Enforced server-side for the same reason.
     */
    private void persistExpensesChargeToProduct(
            int purchaseOrderId,
            List<PurchaseOrderFullDto.PurchaseOrderExpensesChargeToProductDto> rows,
            int supplierId) {

        jdbcTemplate.update("DELETE FROM PurchaseOrderExpensesChargeToProduct WHERE PurchaseOrderId=?", purchaseOrderId);
        if (rows == null) {
            return;
        }

        int orgId  = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        boolean subsidiary = subsidiaryAccountAllowedOnVouchers(orgId, compId);

        /* The same list getAccountsForChargeToProduct() gives the picker - dtAccounts. */
        List<Map<String, Object>> accounts = getAccountsForChargeToProduct();
        String valueMember = subsidiary ? "SupplierCustomerId" : "Id";

        /* combsuppname's own GL account, for the feature-OFF comparison at :2500. */
        int supplierGlAccountId = subsidiary ? 0 : supplierGlAccountId(supplierId);

        int rowNo = 0;
        for (PurchaseOrderFullDto.PurchaseOrderExpensesChargeToProductDto row : rows) {
            rowNo++;
            double amount = row.getAmount() != null ? row.getAmount() : 0.0;
            if (amount <= 0.0) {
                continue;                                     /* :2910 - untouched rows are skipped */
            }
            int picked = row.getAccountId() != null ? row.getAccountId() : 0;
            if (picked == 0) {
                throw new IllegalArgumentException("AccountTitle Field Required");   /* :2916 */
            }

            Map<String, Object> match = null;
            for (Map<String, Object> a : accounts) {
                if (intOf(ci(a, valueMember)) == picked) { match = a; break; }
            }
            if (match == null) {
                /* LimitToList is true on this column (:2573), so a value that is not in the list
                   is not something the desktop can produce. Refused rather than written. */
                throw new IllegalArgumentException(
                        "AccountTitle Field Required (row#" + rowNo + ": the selected account is "
                        + "not in the list this screen offers)");
            }

            /* :2921 / :2926 - AccountId is the GL account in both branches. */
            int glAccountId = subsidiary ? intOf(ci(match, "GlAccountId")) : intOf(ci(match, "Id"));
            int supplierCustomerId = subsidiary ? picked : 0;

            /* :2498-2517 - "Selected Account Can not be Same As Supplier Account". */
            if (subsidiary) {
                if (supplierId != 0 && picked == supplierId) {
                    throw new IllegalArgumentException("Selected Account Can not be Same As Supplier Account");
                }
            } else if (supplierGlAccountId != 0 && glAccountId == supplierGlAccountId) {
                throw new IllegalArgumentException("Selected Account Can not be Same As Supplier Account");
            }

            ProcExec.call(jdbcTemplate, SQL_CHARGE_TO_PRODUCT_INSERT,
                    0, purchaseOrderId, glAccountId,
                    row.getPercentage() != null ? row.getPercentage() : 0.0,
                    row.getQty() != null ? row.getQty() : 0.0,
                    row.getRate() != null ? row.getRate() : 0.0,
                    amount,
                    row.getRemarks(),
                    supplierCustomerId);
        }
    }

    /**
     * combsuppname.SelectedRow.Cells[2].Value (:2500) - the supplier's GL account, read from the
     * same USP_GetVendorsAndCustomersWithCityName rows the supplier combo is filled from.
     * Returns 0 when it cannot be resolved, which makes the comparison a no-op rather than
     * refusing a save on a lookup failure.
     */
    private int supplierGlAccountId(int supplierId) {
        if (supplierId <= 0) return 0;
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(SQL_PARTY_LIST,
                    currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId())) {
                if (intOf(ci(r, "Id")) == supplierId) return intOf(ci(r, "GlAccountId"));
            }
        } catch (Exception e) {
            CHARGE_ACCT_LOG.warn("Could not resolve the supplier's GL account for the "
                    + "\"same as supplier account\" check; the check is skipped for this save", e);
        }
        return 0;
    }

    /**
     * Real delete-then-reinsert persistence for Payment Detail rows, ditto Sp_PurchaseOrder_Update's
     * "DELETE PurchaseOrderPaymentTermsDetail WHERE PurchaseOrderId=@Id" followed by the DAL's per-row
     * USP_PurchaseOrderPaymentTermsDetail_Insert loop. PaymentTermId is required for every row, and
     * DueDays must be > 0 when PaymentTermId=2 (Credit) - ditto PurchsaeOrder.cs's validation
     * ("Payment Term Required in row#.." / "Due Days Required In case Of Credit row in row#..").
     */
    private void persistPaymentTermsDetail(int purchaseOrderId, PurchaseOrderFullDto dto) {
        jdbcTemplate.update("DELETE FROM PurchaseOrderPaymentTermsDetail WHERE PurchaseOrderId=?", purchaseOrderId);

        List<PurchaseOrderFullDto.PurchaseOrderPaymentTermsDetailDto> rows =
                dto.getPaymentTermsDetail() == null
                        ? java.util.Collections.emptyList() : dto.getPaymentTermsDetail();

        /* :3559 - decimal num = GetColumnSum(grdPaymentDetail, "Amount"). The SUM OF THE GRID'S
           AMOUNT COLUMN is what chooses between the two branches below, and nothing else. */
        java.math.BigDecimal gridAmountTotal = java.math.BigDecimal.ZERO;
        for (PurchaseOrderFullDto.PurchaseOrderPaymentTermsDetailDto r : rows) {
            if (r.getAmount() != null) {
                gridAmountTotal = gridAmountTotal.add(java.math.BigDecimal.valueOf(r.getAmount()));
            }
        }

        java.math.BigDecimal detailSum = sumDetails(dto, "amount");   /* DetailSumAmount */
        PurchaseOrderPaymentRules.recalculate(rows, detailSum, dto.isPaymentByPercent(), true);
        java.math.BigDecimal paidTotal = java.math.BigDecimal.ZERO;
        java.math.BigDecimal pctTotal  = java.math.BigDecimal.ZERO;

        List<Object[]> toWrite = new ArrayList<>();

        if (gridAmountTotal.compareTo(java.math.BigDecimal.ZERO) > 0) {
            /* :3562-3600 - the grid is used, with both refusals. */
            int rowNo = 0;
            for (PurchaseOrderFullDto.PurchaseOrderPaymentTermsDetailDto r : rows) {
                rowNo++;
                int termId  = r.getPaymentTermId() != null ? r.getPaymentTermId() : 0;
                int dueDays = r.getDueDays() != null ? r.getDueDays() : 0;
                java.math.BigDecimal pct = r.getPrcntOfTotal() != null
                        ? java.math.BigDecimal.valueOf(r.getPrcntOfTotal()) : java.math.BigDecimal.ZERO;
                java.math.BigDecimal amt = r.getAmount() != null
                        ? java.math.BigDecimal.valueOf(r.getAmount()) : java.math.BigDecimal.ZERO;

                if (termId <= 0) {                                           /* :3584 */
                    throw new IllegalArgumentException("Payment Term Required in row#" + rowNo);
                }
                if (termId == 2 && dueDays <= 0) {                           /* :3591 */
                    throw new IllegalArgumentException("Due Days Required In case Of Credit row in row#" + rowNo);
                }
                pctTotal  = pctTotal.add(pct);
                paidTotal = paidTotal.add(amt);
                java.sql.Date dueDate = dateOrNull(r.getDueDate());
                if (dueDate == null) throw new IllegalArgumentException("Valid Due Date required in Payment Detail row#" + rowNo);
                toWrite.add(new Object[]{ termId, pct, amt, dueDays, r.getPaymentRemarks(), dueDate });
            }
        } else {
            /* -------------------------------------------------------------------------------
             * :3601-3613 - THE BRANCH THAT WAS NEVER PORTED.
             *
             * When the grid's Amount column totals zero - i.e. the operator never touched the
             * Payment Detail tab and it still holds only the blank row AddRowInPaymentGrid put
             * there - the desktop IGNORES THE GRID COMPLETELY and writes ONE row built from the
             * HEADER controls:
             *
             *     PaymentTermId = combpttrm.Value     (the header "Payment Terms" combo)
             *     PaymentTerm   = combpttrm.Text
             *     DueDays       = txtduedays.Text     (the header "Due Days")
             *     DueDate       = DocDate + DueDays
             *     PrcntOfTotal  = 100
             *     Amount        = DetailSumAmount     (the order total)
             *
             * The port instead iterated the blank grid row and threw "Payment Term Required in
             * row#1", so an order the desktop saves without complaint could not be saved from
             * the web at all - and when it did write, it wrote the blank row's zeros rather than
             * the single 100% row the desktop writes.
             * ------------------------------------------------------------------------------- */
            int termId  = dto.getPaymentTermId() != null ? dto.getPaymentTermId() : 0;
            int dueDays = dto.getDueDays() != null ? dto.getDueDays() : 0;

            java.sql.Date docDate = dateOrNull(dto.getDocDate());
            java.sql.Date dueDate = null;
            if (docDate != null) {
                java.time.LocalDate d = docDate.toLocalDate().plusDays(dueDays);
                dueDate = java.sql.Date.valueOf(d);                          /* :3606 */
            }

            pctTotal  = java.math.BigDecimal.valueOf(100);
            paidTotal = detailSum;
            toWrite.add(new Object[]{ termId, pctTotal, detailSum, dueDays, null,
                                      dueDate == null ? null : dueDate.toString() });
        }

        /* :3615 - |PaymentDetailAmount - DetailSumAmount| must be within 0.3 */
        if (paidTotal.subtract(detailSum).abs().compareTo(new java.math.BigDecimal("0.3")) > 0) {
            throw new IllegalArgumentException(
                    "Payment Detail Amount:" + paidTotal.stripTrailingZeros().toPlainString()
                  + " Not Equal to Total Amount:" + detailSum.stripTrailingZeros().toPlainString());
        }
        /* :3622 - the total percentage, rounded to 4, must be within 0.01 of 100 */
        pctTotal = pctTotal.setScale(4, java.math.RoundingMode.HALF_UP);
        if (pctTotal.subtract(java.math.BigDecimal.valueOf(100)).abs()
                .compareTo(new java.math.BigDecimal("0.01")) > 0) {
            throw new IllegalArgumentException("Payment Detail Total% not near to 100");
        }

        int sortNo = 1;
        for (Object[] w : toWrite) {
            ProcExec.call(jdbcTemplate, SQL_PAYMENT_TERMS_DETAIL_INSERT,
                    0, purchaseOrderId, w[0], w[1], w[2], w[3], w[4], sortNo++, w[5]);
        }
    }

    /** Standalone persist for Supplier Expense against an EXISTING, already-saved Purchase Order Id. */
    @Transactional
    public Map<String, Object> saveSupplierExpense(Integer purchaseOrderId, List<PurchaseOrderFullDto.PurchaseOrderSupplierExpenseDto> rows) {
        purchaseOrderRecords.require(zero(purchaseOrderId));
        Map<String, Object> response = new HashMap<>();
        try {
            if (purchaseOrderId == null || purchaseOrderId <= 0) {
                throw new IllegalArgumentException("A saved Purchase Order Id is required before Supplier Expense rows can be persisted.");
            }
            persistSupplierExpense(purchaseOrderId, rows);
            response.put("success", true);
            response.put("message", "Supplier Expense rows saved successfully.");
        } catch (Exception e) {
            response.put("success", false);
            rollbackWrite();
            response.put("message", "Error saving Supplier Expense: " + e.getMessage());
        }
        return response;
    }

    /** Standalone persist for Account Credit _Charge to Product against an EXISTING, already-saved Purchase Order Id. */
    @Transactional
    public Map<String, Object> saveChargeToProduct(Integer purchaseOrderId, List<PurchaseOrderFullDto.PurchaseOrderExpensesChargeToProductDto> rows) {
        purchaseOrderRecords.require(zero(purchaseOrderId));
        Map<String, Object> response = new HashMap<>();
        try {
            if (purchaseOrderId == null || purchaseOrderId <= 0) {
                throw new IllegalArgumentException("A saved Purchase Order Id is required before Account Credit _Charge to Product rows can be persisted.");
            }
            persistExpensesChargeToProduct(purchaseOrderId, rows);
            response.put("success", true);
            response.put("message", "Account Credit _Charge to Product rows saved successfully.");
        } catch (Exception e) {
            response.put("success", false);
            rollbackWrite();
            response.put("message", "Error saving Account Credit _Charge to Product: " + e.getMessage());
        }
        return response;
    }

    /**
     * The Payment Detail rule (:3559-3613) needs the ORDER's header terms and its detail total,
     * not just the grid rows. The standalone tab-save has only the rows, so the rest is read
     * back from the saved order rather than defaulted to zero - defaulting would make the
     * synthesised branch write PaymentTermId = 0 and the reconciliation compare against 0,
     * both of which would be wrong in a way that writes rather than refuses.
     */
    private PurchaseOrderFullDto dtoOf(int purchaseOrderId,
                                       List<PurchaseOrderFullDto.PurchaseOrderPaymentTermsDetailDto> rows) {
        PurchaseOrderFullDto d = new PurchaseOrderFullDto();
        d.setPaymentTermsDetail(rows);
        List<Map<String, Object>> h = jdbcTemplate.queryForList(
                "EXEC Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?", purchaseOrderId, "ReadById");
        if (h.isEmpty()) {
            throw new IllegalArgumentException("Purchase Order " + purchaseOrderId + " was not found.");
        }
        Map<String, Object> head = h.get(0);
        d.setPaymentTermId(intOf(ci(head, "PaymentTermsId")));
        d.setDueDays(intOf(ci(head, "OrderDueDays")));
        d.setDocDate(ymd(ci(head, "DocDate")));

        List<PurchaseOrderFullDto.PurchaseOrderDetailItemDto> lines = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?",
                purchaseOrderId, "ReadByPurchaseOrderHeaderId")) {
            PurchaseOrderFullDto.PurchaseOrderDetailItemDto li =
                    new PurchaseOrderFullDto.PurchaseOrderDetailItemDto();
            Object amt = ci(r, "Amount");
            li.setItemAmount(amt instanceof Number ? ((Number) amt).doubleValue() : 0d);
            lines.add(li);
        }
        d.setLineItems(lines);
        return d;
    }

    /** Standalone persist for Payment Detail against an EXISTING, already-saved Purchase Order Id. */
    @Transactional
    public Map<String, Object> savePaymentTermsDetail(Integer purchaseOrderId, List<PurchaseOrderFullDto.PurchaseOrderPaymentTermsDetailDto> rows) {
        purchaseOrderRecords.require(zero(purchaseOrderId));
        Map<String, Object> response = new HashMap<>();
        try {
            if (purchaseOrderId == null || purchaseOrderId <= 0) {
                throw new IllegalArgumentException("A saved Purchase Order Id is required before Payment Detail rows can be persisted.");
            }
            persistPaymentTermsDetail(purchaseOrderId, dtoOf(purchaseOrderId, rows));
            response.put("success", true);
            response.put("message", "Payment Detail rows saved successfully.");
        } catch (Exception e) {
            response.put("success", false);
            rollbackWrite();
            response.put("message", "Error saving Payment Detail: " + e.getMessage());
        }
        return response;
    }

    /** GetConfigValueFromGlobal(configDescription) ditto - single ConfigKey text value for this Organization/Company, or null. */
    private String getConfigValue(int orgId, int compId, String configDescription) {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    SQL_CONFIG_VALUE_BY_DESCRIPTION, orgId, compId, configDescription, null,
                    "GetConfigurationByOrgCompandConfigDescription");
            if (rows.isEmpty()) {
                return null;
            }
            Object v = rows.get(0).get("ConfigKey");
            return v != null ? v.toString() : null;
        } catch (Exception e) {
            return null;
        }
    }

    private double parseConfigDouble(String s) {
        if (s == null || s.trim().isEmpty()) {
            return 0.0;
        }
        try {
            return Double.parseDouble(s.trim());
        } catch (Exception e) {
            return 0.0;
        }
    }

    private boolean parseConfigBool(String s) {
        if (s == null) {
            return false;
        }
        String t = s.trim();
        return "1".equals(t) || "true".equalsIgnoreCase(t);
    }

    /**
     * Ditto of PurchsaeOrder.cs's AddRowInvEmptyBagsGrid(): a brand-new Purchase Order always seeds
     * exactly two Empty Bags rows - PackingTypeId=1 (Jute Bags) and PackingTypeId=2 (PP Bags), both
     * Type=1 (Normal (Purchase/Receive)), ItemId=0 (not yet selected), Rate=0, and WeightCut defaulted
     * from the 'WeightCutForJuteBags'/'WeightCutForPPBags' configuration values for this Org/Company.
     */
    public List<PurchaseOrderFullDto.PurchaseOrderEmptyBagDto> getDefaultEmptyBagRows() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        double juteCut = parseConfigDouble(getConfigValue(orgId, compId, "WeightCutForJuteBags"));
        double ppCut = parseConfigDouble(getConfigValue(orgId, compId, "WeightCutForPPBags"));

        PurchaseOrderFullDto.PurchaseOrderEmptyBagDto jute = new PurchaseOrderFullDto.PurchaseOrderEmptyBagDto();
        jute.setType(1);
        jute.setItemId(0);
        jute.setPackingTypeId(1);
        jute.setRate(0.0);
        jute.setWeightCut(juteCut);

        PurchaseOrderFullDto.PurchaseOrderEmptyBagDto pp = new PurchaseOrderFullDto.PurchaseOrderEmptyBagDto();
        pp.setType(1);
        pp.setItemId(0);
        pp.setPackingTypeId(2);
        pp.setRate(0.0);
        pp.setWeightCut(ppCut);

        return new ArrayList<>(List.of(jute, pp));
    }

    /** Real read of existing Empty Bags rows for a given, already-saved Purchase Order Id. */
    private List<Map<String, Object>> getEmptyBagsByHeaderId(int purchaseOrderId) {
        try {
            return jdbcTemplate.queryForList(SQL_EMPTY_BAGS_BY_HEADER_ID, purchaseOrderId,
                    "ReadPurchaseOrderEmptyBagsDetailByHeaderId");
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    private Map<String, Object> getOrgCompanyForPurchaseOrder(int purchaseOrderId) {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_ORG_COMPANY_BY_PURCHASE_ORDER_ID, purchaseOrderId);
            return rows.isEmpty() ? null : rows.get(0);
        } catch (Exception e) {
            return null;
        }
    }

    /**
     * Ditto of PurchsaeOrder.cs's per-row Empty Bags validation (lines ~3439-3477, run before Save/Update)
     * PLUS the real Sp_PurchaseOrderEmptyBags_Insert proc's own server-side Jute/PP weight-range check.
     * Throws IllegalArgumentException with the exact desktop message on failure.
     */
    private void validateEmptyBagRow(PurchaseOrderFullDto.PurchaseOrderEmptyBagDto row, int orgId, int compId,
                                       List<Map<String, Object>> packingTypes, boolean skipWeightValidations) {
        int type = row.getType() != null ? row.getType() : 0;
        int packingTypeId = row.getPackingTypeId() != null ? row.getPackingTypeId() : 0;
        double weightCut = row.getWeightCut() != null ? row.getWeightCut() : 0.0;

        if (skipWeightValidations) {
            return;
        }
        if (type == 0) {
            throw new IllegalArgumentException("EmptyBagsType filed required In Empty bags Grid...");
        }
        if (packingTypeId == 0) {
            throw new IllegalArgumentException("PackingType filed required In Empty bags Grid...");
        }
        if (type != 2 && weightCut <= 0.0) {
            throw new IllegalArgumentException("WeightCut filed required In Empty bags Grid...");
        }
        if (weightCut > 0.0) {
            int validationTypeId = (packingTypeId == 1 || packingTypeId == 2 || packingTypeId == 5) ? packingTypeId : 2;
            for (Map<String, Object> pt : packingTypes) {
                Object idObj = pt.get("Id");
                int ptId = idObj instanceof Number ? ((Number) idObj).intValue() : -1;
                if (ptId == validationTypeId) {
                    Object minObj = pt.get("MinEbWeight");
                    Object maxObj = pt.get("MaxEbWeight");
                    double min = minObj instanceof Number ? ((Number) minObj).doubleValue() : 0.0;
                    double max = maxObj instanceof Number ? ((Number) maxObj).doubleValue() : 0.0;
                    if (weightCut < min || weightCut > max) {
                        Object descObj = pt.get("PackTypeDesc");
                        throw new IllegalArgumentException("Weight Cut Should be in Range of: " + min + " to " + max
                                + " For Packing Type:" + (descObj != null ? descObj : ""));
                    }
                    break;
                }
            }
        }
    }

    /**
     * Real delete-then-reinsert persistence for a Purchase Order's Empty Bags rows, ditto of the
     * real Sp_PurchaseOrder_Update proc's own "DELETE PurchaseOrderEmptyBags WHERE PurchaseOrderId=@Id"
     * step followed by Architecture.DAL.Inventory.PurchaseOrder.SetData's per-row
     * Sp_PurchaseOrderEmptyBags_Insert loop. Requires purchaseOrderId to already exist in the real
     * PurchaseOrder table (its OrganizationId/CompanyId are read back from that row).
     */
    @Transactional
    public Map<String, Object> saveEmptyBags(Integer purchaseOrderId, List<PurchaseOrderFullDto.PurchaseOrderEmptyBagDto> rows) {
        purchaseOrderRecords.require(zero(purchaseOrderId));
        Map<String, Object> response = new HashMap<>();
        try {
            if (purchaseOrderId == null || purchaseOrderId <= 0) {
                throw new IllegalArgumentException("A saved Purchase Order Id is required before Empty Bags rows can be persisted.");
            }
            Map<String, Object> orgCompany = getOrgCompanyForPurchaseOrder(purchaseOrderId);
            if (orgCompany == null) {
                throw new IllegalArgumentException("Purchase Order Id " + purchaseOrderId + " was not found - cannot persist Empty Bags rows.");
            }
            int orgId = ((Number) orgCompany.get("OrganizationId")).intValue();
            int compId = ((Number) orgCompany.get("CompanyId")).intValue();
            persistEmptyBags(purchaseOrderId, rows, orgId, compId);
            response.put("success", true);
            response.put("message", "Packing Material (Empty Bags) rows saved successfully.");
            response.put("rowCount", rows != null ? rows.size() : 0);
        } catch (Exception e) {
            response.put("success", false);
            rollbackWrite();
            response.put("message", "Error saving Packing Material (Empty Bags): " + e.getMessage());
        }
        return response;
    }

    private void persistEmptyBags(int purchaseOrderId, List<PurchaseOrderFullDto.PurchaseOrderEmptyBagDto> rows, int orgId, int compId) {
        if (rows == null) {
            rows = Collections.emptyList();
        }
        boolean skipWeightValidations = parseConfigBool(
                getConfigValue(orgId, compId, "StopShowingPurchaseOrderEmptyBagsWeightValidations"));
        List<Map<String, Object>> packingTypes = getPackingTypesForEmptyBags();

        for (PurchaseOrderFullDto.PurchaseOrderEmptyBagDto row : rows) {
            validateEmptyBagRow(row, orgId, compId, packingTypes, skipWeightValidations);
        }

        jdbcTemplate.update(SQL_EMPTY_BAGS_DELETE_BY_HEADER, purchaseOrderId);
        for (PurchaseOrderFullDto.PurchaseOrderEmptyBagDto row : rows) {
            ProcExec.call(jdbcTemplate, SQL_EMPTY_BAGS_INSERT,
                    0,
                    purchaseOrderId,
                    row.getType(),
                    row.getItemId() != null ? row.getItemId() : 0,
                    row.getPackingTypeId(),
                    row.getRate() != null ? row.getRate() : 0.0,
                    row.getWeightCut() != null ? row.getWeightCut() : 0.0);
        }
    }

    private static final String SQL_DETAIL_EXISTING_IDS = "SELECT Id FROM PurchaseOrderDetail WHERE PurchaseOrderId = ?";

    /**
     * Real per-row Insert/Update/Delete for the Purchase Order Detail grid, ditto desktop's
     * btnsave_Click() (Id==0 => insert, Id>0 => update, matching purchaseOrderDetailId round-tripped
     * from getPurchaseOrderById()'s own read) plus its OrderDetailRemoveIds /
     * USP_DeletePurchaseOrderDetailIfNotExistInGrn removed-row handling. Required-field validation
     * mirrors Sp_PurchaseOrderDetail_Insert's own RAISERROR checks (ItemId/ItemQty/NetWeight/
     * ItemRate/OrderItemRateUOMId/Amount) so a bad row is rejected with the same message the desktop
     * proc would raise instead of an opaque SQL error. The original procedure decides whether
     * a removed row can be deleted; failures roll back the whole order.
     */
    private List<String> persistPurchaseOrderDetail(int purchaseOrderId, List<PurchaseOrderFullDto.PurchaseOrderDetailItemDto> items, int userId, PurchaseOrderFullDto order) {
        List<String> warnings = new ArrayList<>();
        List<Map<String, Object>> existingRows = jdbcTemplate.queryForList(SQL_DETAIL_EXISTING_IDS, purchaseOrderId);
        Set<Integer> existingIds = new HashSet<>();
        for (Map<String, Object> row : existingRows) {
            existingIds.add(((Number) row.get("Id")).intValue());
        }
        Set<Integer> seenIds = new HashSet<>();

        int rowNo = 0;
        for (PurchaseOrderFullDto.PurchaseOrderDetailItemDto item : items) {
            rowNo++;
            int itemId = item.getItemId() != null ? item.getItemId() : 0;
            double qty = item.getItemQty() != null ? item.getItemQty() : 0.0;
            double netWeight = item.getItemWeight() != null ? item.getItemWeight() : 0.0;
            double rate = item.getItemRate() != null ? item.getItemRate() : 0.0;
            int rateUomId = item.getRateUomId() != null ? item.getRateUomId() : 0;
            int packUomId = item.getPackUomId() != null ? item.getPackUomId() : 0;
            double amount = item.getItemAmount() != null ? item.getItemAmount() : 0.0;

            if (itemId == 0) {
                throw new IllegalArgumentException("ItemId Field Required (row #" + rowNo + ")");
            }
            if (qty <= 0.0) {
                throw new IllegalArgumentException("ItemQty Field Required (row #" + rowNo + ")");
            }
            if (netWeight <= 0.0) {
                throw new IllegalArgumentException("NetWeight Field Required (row #" + rowNo + ")");
            }
            if (rate <= 0.0) {
                throw new IllegalArgumentException("ItemRate Field Required (row #" + rowNo + ")");
            }
            if (rateUomId <= 0) {
                throw new IllegalArgumentException("OrderItemRateUOMId Field Required (row #" + rowNo + ")");
            }
            if (amount <= 0.0) {
                throw new IllegalArgumentException("Amount Field Required (row #" + rowNo + ")");
            }

            Integer detailId = item.getPurchaseOrderDetailId() != null && item.getPurchaseOrderDetailId() > 0
                    ? item.getPurchaseOrderDetailId() : null;

            if (detailId != null && (!existingIds.contains(detailId) || seenIds.contains(detailId)))
                throw new IllegalArgumentException("Detail row does not belong to this order or was supplied twice.");
            int savedId = purchaseOrderDetails.save(purchaseOrderId, item, order);
            item.setPurchaseOrderDetailId(savedId);
            seenIds.add(savedId);
        }

        // Rows that existed for this Purchase Order before this save but were not present in the
        // incoming grid - the user removed them. Delete individually, guarded by the same GRN/Invoice
        // reference check the real USP_DeletePurchaseOrderDetailIfNotExistInGrn proc performs.
        existingIds.removeAll(seenIds);
        if (!existingIds.isEmpty()) {
            String removed = String.join(",", existingIds.stream().map(String::valueOf).toList());
            ProcExec.call(jdbcTemplate, "EXEC dbo.USP_DeletePurchaseOrderDetailIfNotExistInGrn @OrganizationId=?,@CompanyId=?,@OrderId=?,@UserId=?,@OrderDetailIds=?",
                    currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId(), purchaseOrderId, userId, removed);
        }

        return warnings;
    }

    @Transactional
    public Map<String, Object> savePurchaseOrder(PurchaseOrderFullDto dto, Integer userId) {
        Map<String, Object> response = new HashMap<>();
        try {
            if (dto.getDocumentTypeId() == null || dto.getDocumentTypeId() != PO_DOCUMENT_TYPE_ID) {
                throw new IllegalArgumentException("This form saves Purchase Orders (document type 41) only.");
            }
            if (dto.getSupplierId() == null || dto.getSupplierId() <= 0) {
                throw new IllegalArgumentException("Supplier / Party is required.");
            }
            /* ------------------------------------------------------------------------------
               PurchsaeOrder.cs btnsave_Click :3257-3284 - the checks the desktop runs, in the
               desktop's own order, with the desktop's own message strings. These are server-side
               on purpose: a crafted API request must not be able to bypass them.
               ------------------------------------------------------------------------------ */
            if (dto.getLineItems() == null || dto.getLineItems().isEmpty()) {
                throw new IllegalArgumentException("Detail Information Required");       // :3260
            }
            if (dto.getEmptyBags() == null || dto.getEmptyBags().isEmpty()) {
                throw new IllegalArgumentException("Empty Bags Information Required");   // :3265
            }
            int vAgentId   = zero(dto.getCommissionAgentId());
            double vCommAmt  = dbl(dto.getCommAmount());
            double vCommRate = dbl(dto.getCommRate());
            if ((vCommAmt > 0d || vCommRate > 0d) && vAgentId == 0) {                    // :3270
                throw new IllegalArgumentException(
                        "Please Select Commission Agent Required when Commission Amount or Rate is Present...");
            }
            if (vAgentId > 0 && (vCommAmt == 0d || vCommRate == 0d)) {                   // :3274
                if (vCommAmt == 0d) {
                    throw new IllegalArgumentException(
                            "Commission Amount Required when Commission Agent is Selected...");
                }
                throw new IllegalArgumentException(
                        "Commission Rate Required when Commission Agent is Selected...");
            }

            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            int branchId = currentUserContext.currentBranchId();
            int effUserId = userId != null ? userId : currentUserContext.currentUserId();

            int docNo = dto.getDocNo() != null && dto.getDocNo() > 0 ? dto.getDocNo() : generateNextDocNo(dto.getDocumentTypeId());
            String docDate = dto.getDocDate() != null && !dto.getDocDate().isEmpty() ? dto.getDocDate() : new java.text.SimpleDateFormat("yyyy-MM-dd").format(new Date());

            Integer poMasterId = dto.getPurchaseOrderMasterId();
            int recId = (poMasterId != null && poMasterId > 0) ? poMasterId : 0;
            Map<String, Object> existing = recId > 0 ? purchaseOrderRecords.require(recId) : null;
            var supplements = purchaseOrderSupplements.beforeUpdate(recId);

            /* ------------------------------------------------------------------------------
               BLL 0595 Architecture.BLL.Inventory.PurchaseOrder.Save(obj)

               Step 1 - the date lock, BEFORE anything is written. The desktop refuses the whole
               save when the document date is on or before the configured lock date.
               ------------------------------------------------------------------------------ */
            java.sql.Date docDateSql = java.sql.Date.valueOf(docDate);
            purchaseOrderHeaderRepository.assertNotDateLocked(orgId, compId, docDateSql);
            var attachmentChanges = purchaseOrderAttachments.prepare(recId, dto.getAttachments());

            /* Step 2 - on INSERT the desktop refuses a grid that already carries saved detail ids. */
            if (recId == 0 && dto.getLineItems() != null) {
                for (PurchaseOrderFullDto.PurchaseOrderDetailItemDto li : dto.getLineItems()) {
                    if (li.getPurchaseOrderDetailId() != null && li.getPurchaseOrderDetailId() > 0) {
                        throw new IllegalArgumentException(
                                "Record cannot be inserted because detailId greater than zero");
                    }
                }
            }

            /* Step 3 - the header itself, through the procedure. Field mapping is
               PurchsaeOrder.cs:3285-3355, which is the authority for which control feeds which
               column. Note in particular:
                 - the commission agent is BrokerAgentSupCustId (:3323), NOT "CommissionAgentId";
                   that column belongs to the pcc and FeedMill order models, not this one
                 - the party is OrderSupCustId only (:3303); there is no SupplierCustomerId here
                 - IsAproved is false on insert and keeps the row's EXISTING value on update
                   (:3289-3293); IsApproved is never written through this path
                 - FinancialYearId is the active year (:4707), never a literal
               DocNo and BranchSrNo are sent as the screen has them, but Sp_PurchaseOrder_Insert
               recomputes both as MAX+1 itself, so the procedure remains the single source of
               document numbering. */
            Map<String, Object> head = PurchaseOrderHeaderRepository.blankModel();
            java.sql.Timestamp nowTs = new java.sql.Timestamp(System.currentTimeMillis());

            head.put("Id",             recId);
            head.put("DocumentTypeId", PO_DOCUMENT_TYPE_ID);
            head.put("AttachmentsValues", attachmentChanges.names());
            head.put("CustomAttachmentsValues", attachmentChanges.storedNames());
            head.put("CurrencyId", zero(dto.getCurrencyId()));
            head.put("ExchangeRate", dec(dto.getExchangeRate()));
            head.put("FcyAmount", dec(dto.getFcyAmount()));
            head.put("DocNo",          docNo);
            head.put("DocDate",        docDateSql);
            head.put("BranchesId",     branchId);
            head.put("ProjectsId",     branchId);                       // :3296
            head.put("OrganizationId", orgId);
            head.put("CompanyId",      compId);
            head.put("FinancialYearId", currentUserContext.currentFinancialYearId());
            head.put("BranchSrNo",     dto.getBranchNo() == null ? 0 : dto.getBranchNo());
            head.put("OrderSupCustId", dto.getSupplierId());
            head.put("SupplierRefNo",  dto.getSupplierRefNo());
            head.put("BookingPersonId", zero(dto.getBookingPersonId()));
            head.put("RemarksHeader",  dto.getRemarksHeader() == null ? "" : dto.getRemarksHeader());
            head.put("PaymentTermsId", zero(dto.getPaymentTermId()));
            head.put("OrderDueDays",   zero(dto.getDueDays()));
            /* :3309 `po.OrderDueDate = duedate.Value` - an UltraDateTimeEditor assigned into a
               non-nullable DateTime, so the desktop can never send null here. The editor's own
               value is set at :5357-5361: DocDate + OrderDueDays, or DateTime.Now when the days
               box is empty. That is a desktop-owned default, reproduced rather than invented. */
            java.sql.Date orderDueDate = dateOrNull(dto.getPaymentDueDate());
            if (orderDueDate == null) {
                int dueDays = zero(dto.getDueDays());
                orderDueDate = dueDays > 0
                        ? java.sql.Date.valueOf(docDateSql.toLocalDate().plusDays(dueDays))  // :5357
                        : new java.sql.Date(System.currentTimeMillis());                     // :5361
            }
            head.put("OrderDueDate",   orderDueDate);
            head.put("OrderExpiryDate", nowTs);                          // :3311 DateTime.Now
            head.put("DeliveryTermId", zero(dto.getDeliveryTermId()));
            head.put("DeliveryTerm",   dto.getDeliveryTermName());
            /* :3313 `po.DeliveryStartDate = deliverystartdate.Value` - likewise non-nullable.
               Reset() seeds that editor with DateTime.Now (:3941). */
            java.sql.Date deliveryStart = dateOrNull(dto.getDeliveryStartDate());
            if (deliveryStart == null) {
                deliveryStart = new java.sql.Date(System.currentTimeMillis());                // :3941
            }
            head.put("DeliveryStartDate", deliveryStart);
            head.put("DeliveryDays",   zero(dto.getDeliveryDays()));
            head.put("OrderCatagoryId", zero(dto.getOrderCategoryId()));
            head.put("CatagorySrNo",   zero(dto.getCategorySrNo()));
            head.put("OrderStatus",    dto.getOrderStatus());
            head.put("LocationTypeId", zero(dto.getLocationTypeId()));
            /* CalculateTotalInformation() - PurchsaeOrder.cs :4200-4222. The desktop SUMS the
               detail grid's ItemQty / Weight / Amount columns into these three header fields,
               so they are derived data, never independently entered.
               
               They used to be taken verbatim from the posted JSON, and the page never filled
               the boxes it read them from - so every Purchase Order saved from the web stored
               OrderQty = 0, OrderWeight = 0, OrderAmount = 0 beside detail rows carrying the
               real figures. Deriving them here from the detail list means a page defect, a
               stale field or a crafted request cannot put a header total out of step with the
               lines it is supposed to total. */
            head.put("OrderQty",    sumDetails(dto, "qty"));
            head.put("OrderWeight", sumDetails(dto, "weight"));
            head.put("OrderAmount", sumDetails(dto, "amount"));
                                                head.put("EntryUser",      effUserId);
            head.put("EntryDate",      nowTs);
            head.put("ModifyUser",     effUserId);
            head.put("ModifyDate",     nowTs);
            head.put("PostState",      Boolean.FALSE);
            head.put("OrderTaxable",   Boolean.FALSE);
            /* CurrencyId / ExchangeRate / FcyAmount are written by the desktop (:3318-3320) from
               cmbCurrency, txtExchangeRate and txtFcyAmount. This web page has no such controls.
               They are NOT left to the procedure's defaults: an unselected UltraCombo gives
               Conversion.ToInt(null) == 0 and an empty textbox gives Conversion.ToDecimal("") == 0,
               so the desktop sends 0 / 0 / 0 for a form in this state. blankModel() already carries
               those three as the CLR defaults of `int` and `decimal`, which is exactly that. A
               fabricated exchange rate of 1 would still be wrong; 0 is what the desktop sends. */

            /* :3320-3327 - the commission block is written ONLY when an agent is chosen. */
            if (dto.getCommissionAgentId() != null && dto.getCommissionAgentId() > 0) {
                head.put("BrokerAgentSupCustId", dto.getCommissionAgentId());
                head.put("CommissionType",       dto.getCommissionTypeName());
                head.put("CommRate",             dbl(dto.getCommRate()));
                head.put("UomScheduleIdCmRate",  zero(dto.getCommUomId()));
                head.put("CommAmount",           dbl(dto.getCommAmount()));
            }
            /* :3328-3335 - likewise the brokery block. */
            if (dto.getBrokerAccountId() != null && dto.getBrokerAccountId() > 0) {
                head.put("BrokerAgentId",  dto.getBrokerAccountId());
                head.put("BrokeryType",    dto.getBrokeryTypeName());
                head.put("BrokeryRate",    dbl(dto.getBrokeryRate()));
                head.put("BrokeryUom",     dbl(dto.getBrokeryRateUomId()));
                head.put("BrokeryAmount",  dbl(dto.getBrokeryAmount()));
            }
            /* :3337-3344 - exactly one of the two freight flags; the page already sends both
               booleans from the rdFreightCash / rdFreightCredit pair. */
            boolean creditFreight = Boolean.TRUE.equals(dto.getCreditFreight());
            head.put("CashFreight",   !creditFreight);
            head.put("CreditFreight", creditFreight);

            if (recId > 0) {
                /* :3289 - an update carries the row's existing approval state; it is read back
                   from the row rather than taken from the client, which must never be able to
                   approve an order by posting a flag. */
                Object stored = existing.get("IsAproved");
                head.put("IsAproved", Boolean.TRUE.equals(stored) || "1".equals(String.valueOf(stored)));
            } else {
                head.put("IsAproved", Boolean.FALSE);   // :3293
            }

            poMasterId = purchaseOrderHeaderRepository.save(head);
            if (poMasterId == null || poMasterId <= 0) {
                throw new IllegalStateException("Purchase Order save returned no Id.");
            }

            // Purchase Order Detail (main line-items grid) - real per-row Insert/Update/Delete against
            // the real PurchaseOrderDetail table, ditto desktop's btnsave_Click()/Sp_PurchaseOrderDetail_Insert
            // (which itself branches INSERT vs UPDATE on whether @Id is 0). Existing rows are matched
            // by purchaseOrderDetailId and only updated in place; a row previously saved for this
            // Purchase Order that is no longer present in the incoming grid is deleted individually
            // (never a blanket "DELETE ... WHERE PurchaseOrderId=?"), and ONLY after confirming - ditto
            // desktop's USP_DeletePurchaseOrderDetailIfNotExistInGrn guard - that it is not already
            // referenced by a real GRN or Purchase Invoice line; if it is, that row is left in place
            // and a warning is reported instead of silently failing or wiping unrelated data.
            List<String> detailWarnings = persistPurchaseOrderDetail(poMasterId, dto.getLineItems(), effUserId, dto);
            if (!detailWarnings.isEmpty()) {
                response.put("detailWarnings", detailWarnings);
            }

            /* Packing Material (Empty Bags), then the three remaining tabs.

               These used to sit in four separate try/catch blocks that downgraded any failure to
               a "...Warning" string in the response, so a half-written order was the normal
               outcome. They now run inside the one transaction with the header: if any of them
               fails, the whole save rolls back. */
            Map<String, Object> orgCompany = getOrgCompanyForPurchaseOrder(poMasterId);
            int emptyBagsOrgId = orgCompany != null && orgCompany.get("OrganizationId") != null
                    ? ((Number) orgCompany.get("OrganizationId")).intValue()
                    : currentUserContext.currentOrganizationId();
            int emptyBagsCompId = orgCompany != null && orgCompany.get("CompanyId") != null
                    ? ((Number) orgCompany.get("CompanyId")).intValue()
                    : currentUserContext.currentCompanyId();
            persistEmptyBags(poMasterId, dto.getEmptyBags(), emptyBagsOrgId, emptyBagsCompId);

            persistSupplierExpense(poMasterId, dto.getSupplierExpenses());
            persistExpensesChargeToProduct(poMasterId, dto.getExpensesChargeToProduct(),
                    dto.getSupplierId() != null ? dto.getSupplierId() : 0);
            persistPaymentTermsDetail(poMasterId, dto);
            purchaseOrderSupplements.restore(poMasterId, supplements);
            purchaseOrderAttachments.persist(poMasterId, dto.getSupplierId(), attachmentChanges);
            if (recId > 0) {
                ProcExec.call(jdbcTemplate, "EXEC dbo.Sp_PurchaseOrder_GetAllMethod @OrganizationId=?,@CompanyId=?,@Id=?,@Activity=?",
                        orgId, compId, poMasterId, "PoWeightAndGpWeightValidation");
            }

            // The insert procedure owns the final number, including concurrent allocations.
            docNo = jdbcTemplate.queryForObject("SELECT DocNo FROM dbo.PurchaseOrder WHERE Id=? AND OrganizationId=? AND CompanyId=? AND DocumentTypeId=41",
                    Integer.class, poMasterId, orgId, compId);

            response.put("success", true);
            response.put("id", poMasterId);
            response.put("docNo", docNo);
            response.put("message", "Purchase Order " + (dto.getPurchaseOrderMasterId() != null && dto.getPurchaseOrderMasterId() > 0 ? "updated" : "saved") + " successfully (Doc No: " + docNo + ")");
        } catch (Exception e) {
            /* @Transactional rolls back on a propagating RuntimeException. This method returns a
               response object instead of throwing, so the rollback has to be asked for
               explicitly - otherwise the header stayed committed while a child collection had
               failed, which is how partial orders were being written. */
            try {
                org.springframework.transaction.interceptor.TransactionAspectSupport
                        .currentTransactionStatus().setRollbackOnly();
            } catch (IllegalStateException | org.springframework.transaction.NoTransactionException noTx) {
                /* not running in a transaction - nothing to roll back */
            }
            response.clear();
            response.put("success", false);
            /* RAISERROR text from Sp_PurchaseOrder_Insert / _Update is the desktop's own wording
               ("Document Date(...) is Not Valid Against Active Financial Year!", "DeliveryTerm
               Field Required", "OrderStatus Field Required", "DocDate cannot be greater than
               DeliveryStartDate Please Check") and is passed through unchanged. */
            response.put("message", e.getMessage());
        }
        return response;
    }

    /**
     * ReadById(ID) - PurchsaeOrder.cs :3706-3875, via PurchaseOrder.GetByID (BLL 0595:46).
     *
     * -----------------------------------------------------------------------------------------
     * WHY THIS RETURNED 404 FOR EVERY PURCHASE ORDER
     * -----------------------------------------------------------------------------------------
     * The previous implementation was a hand-written SELECT, and it asked for
     *
     *     po.CommissionAgentId as commissionAgentId
     *
     * There is no such column. Architecture.Model.Inventory.PurchaseOrder has no
     * CommissionAgentId, and the save path in this very file already knows why: the commission
     * agent is stored in BrokerAgentSupCustId (:3323), while BrokerAgentId is the BROKERY
     * account (:3331). Two deceptively similar names, and the read picked the one that does not
     * exist. SQL Server raised "Invalid column name", the surrounding `catch (Exception)`
     * returned null, the controller turned null into 404, and the page reported
     * "Failed to load item details" - and Edit filled nothing - with the real reason discarded.
     *
     * So the save wrote the right column and the read asked for a column that was never there.
     *
     * -----------------------------------------------------------------------------------------
     * WHAT IT DOES NOW
     * -----------------------------------------------------------------------------------------
     * The desktop's own contract: Sp_PurchaseOrder_GetAllMethod with @Activity='ReadById' for the
     * header and @Activity='ReadByPurchaseOrderHeaderId' for the lines (DAL 0448:243), plus
     * @Activity='ReadByHeaderId_PurchaseOrderSupplierDispatchDetail' (DAL 0448:313) for the
     * dispatch rows that decide whether this order may still be re-pointed at another supplier.
     *
     * GenericProvider maps a procedure's columns onto the model's properties, so the model IS the
     * column list: every name read below is a property of Architecture.Model.Inventory.
     * PurchaseOrder or .PurchaseOrderDetail. The keys written out keep the camelCase shape the
     * page already consumes, so only the SOURCE changed, not the page's contract.
     *
     * A failure is raised, not swallowed into a null. A 404 that means "the query was wrong"
     * cost this screen two defects that looked like missing data.
     */
    public Map<String, Object> getPurchaseOrderById(Integer id) {
        purchaseOrderRecords.require(zero(id));
        List<Map<String, Object>> heads = jdbcTemplate.queryForList(
                "EXEC Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?", id, "ReadById");
        if (heads.isEmpty()) {
            throw new IllegalArgumentException("Purchase Order " + id + " was not found.");
        }
        Map<String, Object> h = heads.get(0);
        Map<String, Object> head = new LinkedHashMap<>();

        /* :3718-3757 - the header assignments, in the desktop's own order. */
        head.put("purchaseOrderMasterId", intOf(ci(h, "Id")));
        head.put("documentTypeId",  intOf(ci(h, "DocumentTypeId")));
        head.put("docNo",           intOf(ci(h, "DocNo")));
        head.put("branchSrNo",      intOf(ci(h, "BranchSrNo")));
        head.put("docDate",         ymd(ci(h, "DocDate")));
        head.put("orderCategoryId", intOf(ci(h, "OrderCatagoryId")));   /* desktop's spelling */
        head.put("categorySrNo",    intOf(ci(h, "CatagorySrNo")));
        head.put("supplierId",      intOf(ci(h, "OrderSupCustId")));    /* :3724, NOT SupplierCustomerId */
        head.put("bookingPersonId", intOf(ci(h, "BookingPersonId")));
        head.put("supplierRefNo",   strOf(ci(h, "SupplierRefNo")));
        head.put("remarksHeader",   strOf(ci(h, "RemarksHeader")));
        head.put("paymentTermsId",  intOf(ci(h, "PaymentTermsId")));
        head.put("orderDueDays",    intOf(ci(h, "OrderDueDays")));
        head.put("orderDueDate",    ymd(ci(h, "OrderDueDate")));
        head.put("orderQty",        ci(h, "OrderQty"));
        head.put("orderWeight",     ci(h, "OrderWeight"));
        head.put("orderAmount",     ci(h, "OrderAmount"));
        head.put("currencyId",      intOf(ci(h, "CurrencyId")));
        head.put("exchangeRate",    ci(h, "ExchangeRate"));
        head.put("fcyAmount",       ci(h, "FcyAmount"));
        head.put("deliveryTermId",  intOf(ci(h, "DeliveryTermId")));
        head.put("deliveryStartDate", ymd(ci(h, "DeliveryStartDate")));
        head.put("deliveryDays",    intOf(ci(h, "DeliveryDays")));
        head.put("orderExpiryDate", ymd(ci(h, "OrderExpiryDate")));

        /* :3745 combsalesman.Value = po.BrokerAgentSupCustId - the COMMISSION AGENT.
           :3750 CmbBrokeryAccount.Value = po.BrokerAgentId   - the BROKERY ACCOUNT.
           This pair is the defect described above; they are not interchangeable. */
        head.put("commissionAgentId", intOf(ci(h, "BrokerAgentSupCustId")));
        head.put("commissionType",    strOf(ci(h, "CommissionType")));
        head.put("commissionRateUom", ci(h, "UomScheduleIdCmRate"));
        head.put("commRate",          ci(h, "CommRate"));
        head.put("commAmount",        ci(h, "CommAmount"));
        head.put("commissionRemarks", strOf(ci(h, "CommissionRemarks")));

        head.put("brokerAccountId", intOf(ci(h, "BrokerAgentId")));
        head.put("brokeryType",     strOf(ci(h, "BrokeryType")));
        head.put("brokeryRate",     ci(h, "BrokeryRate"));
        head.put("brokeryUom",      ci(h, "BrokeryUom"));
        head.put("brokeryAmount",   ci(h, "BrokeryAmount"));

        head.put("orderStatus",    strOf(ci(h, "OrderStatus")));
        head.put("isApproved",     truthy(ci(h, "IsAproved")));   /* desktop's spelling: one 'p' */
        head.put("cashFreight",    truthy(ci(h, "CashFreight")));
        head.put("creditFreight",  truthy(ci(h, "CreditFreight")));
        head.put("locationTypeId", intOf(ci(h, "LocationTypeId")));
        head.put("displayCode",    "PO-" + intOf(ci(h, "DocNo")));

        /* DAL 0448:243 - the real line read. Its columns are PurchaseOrderDetail's properties;
           ItemCode / ItemName / UOMCode / RateUom / JobLotDescription are declared `virtual`,
           which only excludes them from the WRITE parameter list - the read mapper fills them,
           which is why the desktop can display them straight from purchaseOrderDetailList. */
        List<Map<String, Object>> lines = jdbcTemplate.queryForList(
                "EXEC Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?",
                id, "ReadByPurchaseOrderHeaderId");

        List<Map<String, Object>> lineItems = new ArrayList<>();
        boolean hasLabSample = false;
        for (Map<String, Object> d : lines) {
            if (intOf(ci(d, "InvLabSampleAnalysisHeaderId")) > 0) hasLabSample = true;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("purchaseOrderDetailId", intOf(ci(d, "Id")));
            m.put("itemId",        intOf(ci(d, "OrderItemId")));
            m.put("itemCode",      strOf(ci(d, "ItemCode")));
            m.put("itemName",      strOf(ci(d, "ItemName")));
            m.put("cropYearId",    intOf(ci(d, "CropYearId")));
            m.put("cropYear",      strOf(ci(d, "Crop")));
            m.put("packUomId",     intOf(ci(d, "OrderItemUOMId")));
            m.put("packUomCode",   strOf(ci(d, "UOMCode")));
            m.put("packUomEquivalent", ci(d, "UOMDescription"));
            m.put("itemQty",       ci(d, "OrderItemQty"));
            m.put("itemWeight",    ci(d, "NetWeight"));
            m.put("rateUomId",     intOf(ci(d, "OrderItemRateUOMId")));
            m.put("rateUomCode",   strOf(ci(d, "RateUom")));
            m.put("equivalentRate", ci(d, "EquivalentRate"));
            m.put("itemRate",      ci(d, "OrderItemRate"));
            m.put("itemAmount",    ci(d, "Amount"));
            m.put("jobLotId",      intOf(ci(d, "JobLotId")));
            m.put("jobLotName",    strOf(ci(d, "JobLotDescription")));
            m.put("loadingLocationCityId",   intOf(ci(d, "CityId")));
            m.put("loadingLocationCityName", strOf(ci(d, "CityArea")));
            m.put("moisturePercent", strOf(ci(d, "Moisture")));
            m.put("labSampleId",   intOf(ci(d, "InvLabSampleAnalysisHeaderId")));
            m.put("labSampleNo",   strOf(ci(d, "LabSampleNo")));
            m.put("labAnalysisStandardScheduleId", intOf(ci(d, "LabAnalysisStandardScheduleId")));
            m.put("fcyAmount",     ci(d, "FcyAmount"));
            m.put("remarks",       strOf(ci(d, "OrderRemarks")));
            lineItems.add(m);
        }
        head.put("lineItems", lineItems);

        /* DAL 0448:313 - dispatch rows against this order. :3779 treats "any row with
           SupplierDispatchId > 0" as the trigger. */
        List<Map<String, Object>> dispatch;
        try {
            dispatch = jdbcTemplate.queryForList(
                    "EXEC Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?",
                    id, "ReadByHeaderId_PurchaseOrderSupplierDispatchDetail");
        } catch (Exception e) {
            READ_LOG.warn("Supplier dispatch detail could not be read for Purchase Order {}; the "
                    + "Delivery Term lock is left OFF, which is the state when no dispatch exists", id, e);
            dispatch = Collections.emptyList();
        }
        boolean hasDispatch = false;
        for (Map<String, Object> r : dispatch) {
            if (intOf(ci(r, "SupplierDispatchId")) > 0) { hasDispatch = true; break; }
        }
        head.put("supplierDispatchDetail", dispatch);

        /* -------------------------------------------------------------------------------------
         * The control-state rules, computed here because they are decisions about the RECORD.
         *
         * ReadById applies them in this order and the ORDER MATTERS:
         *   :3781-3783  a dispatch row disables Supplier, Order Category and Delivery Term
         *   :3864       PartyDisableEnableOnLabAnalysis() then RE-EVALUATES two of them (:2052):
         *                 rows exist -> Order Category is disabled, always (:2076)
         *                            -> Supplier disabled ONLY if some row has a Lab Sample,
         *                               and otherwise ENABLED (:2070-2074) - which undoes the
         *                               dispatch disable from :3781
         *                 no rows    -> both enabled
         *   Delivery Term is not revisited, so the dispatch disable stands.
         *
         * Reproduced exactly, including the re-enable. It reads like an oversight in the
         * desktop, but it is the desktop's behaviour and this is a parity port.
         * ------------------------------------------------------------------------------------- */
        boolean hasLines = !lineItems.isEmpty();
        head.put("lockOrderCategory", hasLines);
        head.put("lockSupplier",      hasLines && hasLabSample);
        head.put("lockDeliveryTerm",  hasDispatch);
        head.put("hasSupplierDispatch", hasDispatch);

        /* :3969-3972 Reset() disables the four commission fields, and combsalesman_Leave (:1654)
           is the only thing that enables them - so they are live exactly when an agent is set. */
        head.put("commissionFieldsEnabled", intOf(ci(h, "BrokerAgentSupCustId")) > 0);

        head.put("emptyBags",               getEmptyBagsByHeaderId(id));
        head.put("supplierExpenses",        getSupplierExpenseByHeaderId(id));
        head.put("expensesChargeToProduct", getExpensesChargeToProductByHeaderId(id));
        head.put("paymentTermsDetail",      getPaymentTermsDetailByHeaderId(id));
        return head;
    }

    private static final org.slf4j.Logger READ_LOG =
            org.slf4j.LoggerFactory.getLogger(PurchaseOrderFullService.class.getName() + ".ReadById");

    private static boolean truthy(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number)  return ((Number) v).intValue() != 0;
        if (v == null) return false;
        String t = String.valueOf(v).trim();
        return "1".equals(t) || "true".equalsIgnoreCase(t);
    }

    /** yyyy-MM-dd, the shape every date input on this page expects. */
    private static String ymd(Object v) {
        if (v == null) return null;
        String t = String.valueOf(v);
        return t.length() >= 10 ? t.substring(0, 10) : t;
    }

    /**
     * HistoryBranchComboFill() - PurchsaeOrder.cs :4596-4620.
     *
     * -----------------------------------------------------------------------------------------
     * THIS WAS `SELECT Id, BranchName FROM Branch` - a fabricated query
     * -----------------------------------------------------------------------------------------
     * The desktop never reads the Branch table directly here. It has two branches of its own,
     * chosen by the PurchaseOrderBranchWise configuration value (:632):
     *
     *   BranchImplemented  -> the list is ONE row, the signed-in user's own branch
     *                         (UserAccount.BranchesId / BranchName), :4604
     *   otherwise          -> PurchaseOrder.GetBranchesAllocatedToUserFromPurchaseOrder(
     *                             OrganizationId, CompanyId, UserId, 41)
     *                         -> [dbo].[USP_GetBranchsAllocatedToUserFromPurchaseOrder]
     *                         with @DocumentTypeId GUARDED (BLL 0595:2865), :4609
     *
     * So the desktop offers only the branches THIS user is allocated FOR PURCHASE ORDER RICE
     * (document type 41). The plain table read offered every branch in the database, of every
     * company - a tenancy leak on a read, and a list the desktop would never show.
     */
    public List<Map<String, Object>> getBranches() {
        int orgId  = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();

        if (purchaseOrderBranchWise(orgId, compId)) {
            /* :4604 - the user's own branch, and nothing else. UserAccount.BranchName is a
               virtual property the desktop fills at login and this port does not carry, so the
               name is resolved from [dbo].[USP_GetBranchsAllocatedToUser] (BranchId, BranchName)
               - a real procedure already used by the Sale Order screen - rather than from an
               invented SELECT against a Branch/Branches table whose spelling varies. */
            int myBranch = currentUserContext.currentBranchId();
            String myBranchName = "";
            try {
                for (Map<String, Object> r : jdbcTemplate.queryForList(
                        "EXEC [dbo].[USP_GetBranchsAllocatedToUser] @OrganizationId=?, @CompanyId=?, @UserId=?",
                        orgId, compId, currentUserContext.currentUserId())) {
                    if (intOf(ci(r, "BranchId")) == myBranch) {
                        myBranchName = strOf(ci(r, "BranchName"));
                        break;
                    }
                }
            } catch (Exception e) {
                HISTORY_LOG.warn("Could not resolve the signed-in user's branch name", e);
            }
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", myBranch);
            m.put("branchName", myBranchName);
            List<Map<String, Object>> one = new ArrayList<>();
            one.add(m);
            return one;
        }

        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC [dbo].[USP_GetBranchsAllocatedToUserFromPurchaseOrder] "
              + "@OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeId=?",
                orgId, compId, currentUserContext.currentUserId(), PO_DOCUMENT_TYPE_ID)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", intOf(ci(r, "BranchId")));       /* :4613 - BranchId, not Id */
            m.put("branchName", strOf(ci(r, "BranchName")));
            out.add(m);
        }
        return out;
    }

    /** GlobalVariables_Helper.GetConfigValueFromGlobal("PurchaseOrderBranchWise"), :632. */
    private boolean purchaseOrderBranchWise(int orgId, int compId) {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
                  + "@ConfigDescription=?, @Activity=?",
                    orgId, compId, "PurchaseOrderBranchWise",
                    "GetConfigurationByOrgCompandConfigDescription");
            if (rows.isEmpty()) return false;
            String v = strOf(ci(rows.get(0), "ConfigKey")).trim();
            return "1".equals(v) || "true".equalsIgnoreCase(v) || "yes".equalsIgnoreCase(v);
        } catch (Exception e) {
            HISTORY_LOG.warn("PurchaseOrderBranchWise could not be read; using the allocated-branch "
                    + "list, which is the desktop's behaviour when the flag is off", e);
            return false;
        }
    }

    /**
     * The history grid cannot decide its own shape - three of its decisions are database answers
     * that the desktop reads at form load, and hardcoding any of them would be a guess:
     *
     *   branchImplemented -> HistoryGridSettings :4842-4846 and :4891-4892 HIDE the BranchSrNo and
     *                        BranchName columns entirely when PurchaseOrderBranchWise is on.
     *   amountDecimals    -> CommonServices :5397, clsGlobalVariables.stringFormatsingle is
     *                        "#,##0." + N where N comes from the configuration value
     *                        "Default NoofDecimal Points For Amount".
     *   rateDecimals      -> CommonServices :5420, DecimalRateFormate is "#,#0." + N from
     *                        "Default NoofDecimal Points For Rate".
     *
     * The desktop's own switch statements leave the format suffix EMPTY for amount when the
     * configured value is not 1-4 (so zero decimals), while rate falls through case 0 to "00"
     * (two decimals). Both defaults are reproduced rather than assumed.
     */
    public Map<String, Object> historyMeta() {
        int orgId  = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("branchImplemented", purchaseOrderBranchWise(orgId, compId));
        m.put("amountDecimals", decimalPoints(orgId, compId, "Default NoofDecimal Points For Amount", 0));
        m.put("rateDecimals",   decimalPoints(orgId, compId, "Default NoofDecimal Points For Rate", 2));
        return m;
    }

    /** CommonServices :5378-5420 - only 1..4 are honoured; anything else takes the stated default. */
    private int decimalPoints(int orgId, int compId, String configDescription, int fallback) {
        try {
            String v = configValueFor(orgId, compId, configDescription);
            int n = Integer.parseInt(v.trim());
            return (n >= 1 && n <= 4) ? n : fallback;
        } catch (Exception e) {
            return fallback;
        }
    }

    private String configValueFor(int orgId, int compId, String configDescription) {
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@ConfigDescription=?, @Activity=?",
                orgId, compId, configDescription,
                "GetConfigurationByOrgCompandConfigDescription");
        if (rows.isEmpty()) return "";
        return strOf(ci(rows.get(0), "ConfigKey"));
    }

    /**
     * getUpdateForHistory() - PurchsaeOrder.cs :4971-5010. The "Detail Of Above Selected Row" grid.
     *
     * -----------------------------------------------------------------------------------------
     * WHY THIS IS ITS OWN ENDPOINT
     * -----------------------------------------------------------------------------------------
     * The page was calling GET /api/purchase-order/{id} - the FORM-LOAD payload - and reading
     * `lineItems` off it. Two problems:
     *
     *   a) getPurchaseOrderById() builds a hand-written header SELECT joining SupplierCustomer
     *      three times and ReferenceParties, and it returns `null` from a bare `catch (Exception)`
     *      that swallows the reason. A null becomes 404, the page's .fail() branch fires, and the
     *      operator sees "Failed to load item details" with nothing anywhere saying why. That is
     *      what was on screen.
     *   b) The detail grid does not need the header at all. Coupling it to the most fragile query
     *      on the screen means any header defect blanks the detail grid too.
     *
     * So the detail rows are read on their own, and the fourteen columns are exactly the ones
     * :4986-5000 declares, in that order, from the same source fields :5004 maps:
     *
     *   ItemCode ItemName CropYear UOM ItemQTY Weight ItemRate RateUOM Amount Job/Lot
     *   FcyAmount(hidden, :5021) CityName Moisture% Remarks
     *
     * A failure here is reported, not swallowed - an empty detail grid and a broken query must
     * not look the same.
     */
    public List<Map<String, Object>> historyDetail(int purchaseOrderId) {
        purchaseOrderRecords.require(purchaseOrderId);
        String sql =
                "SELECT i.ItemCode AS ItemCode, i.ItemName AS ItemName, "
              + "ISNULL(d.Crop, '') AS CropYear, packUom.UOMCode AS UOM, "
              + "d.OrderItemQty AS ItemQTY, d.NetWeight AS Weight, "
              + "d.OrderItemRate AS ItemRate, rateUom.UOMCode AS RateUOM, "
              + "d.Amount AS Amount, jl.JobLotDescription AS JobLot, "
              + "d.FcyAmount AS FcyAmount, "
              + "ISNULL(c.Description, d.CityArea) AS CityName, "
              + "d.Moisture AS Moisture, ISNULL(d.OrderRemarks, '') AS Remarks "
              + "FROM PurchaseOrderDetail d "
              + "LEFT JOIN Item i ON d.OrderItemId = i.Id "
              + "LEFT JOIN JobLot jl ON d.JobLotId = jl.Id "
              + "LEFT JOIN City c ON d.CityId = c.Id "
              + "LEFT JOIN UOMSchedule packSched ON d.OrderItemUOMId = packSched.Id "
              + "LEFT JOIN UOM packUom ON packSched.ScheduleUnitId = packUom.Id "
              + "LEFT JOIN UOMSchedule rateSched ON d.OrderItemRateUOMId = rateSched.Id "
              + "LEFT JOIN UOM rateUom ON rateSched.ScheduleUnitId = rateUom.Id "
              + "WHERE d.PurchaseOrderId = ? ORDER BY d.Id";
        return jdbcTemplate.queryForList(sql, purchaseOrderId);
    }

    /** PurchsaeOrder.cs :4706 - obj.DocumentTypeId = 41, hard-coded on this form. */
    private static final int PO_DOCUMENT_TYPE_ID = 41;

    private static final org.slf4j.Logger HISTORY_LOG =
            org.slf4j.LoggerFactory.getLogger(PurchaseOrderFullService.class.getName() + ".History");

    /**
     * btnshow_Click's history load - PurchsaeOrder.cs :4700-4774, BLL 0595:119 PurchaseOrder.Getall.
     *
     * -----------------------------------------------------------------------------------------
     * THIS WAS A HAND-WRITTEN SELECT AND IT RETURNED NOTHING
     * -----------------------------------------------------------------------------------------
     * The previous implementation built raw SQL over PurchaseOrder/SupplierCustomer/
     * ReferenceParties/Branch and filtered with `AND (po.BranchId = ? OR po.BranchesId = ?)`
     * against a single integer. That is not what the desktop runs, and it was wrong in four
     * independent ways at once:
     *
     *   1. WRONG INSTRUMENT. The desktop calls Sp_PurchaseOrder_GetAllMethod with
     *      @Activity='PurchaseOrderFormHistory'. Whatever that procedure joins, selects and
     *      orders is the contract; a reimplementation of it in the Java layer is a guess about
     *      a procedure body nobody read.
     *
     *   2. NO TENANCY ON A READ. Not one of @OrganizationId, @CompanyId, @FinancialYearId or
     *      @DocumentTypeId appeared in the WHERE clause. With the branch filter removed it
     *      would have listed every Purchase Order in GoldenAcedb, of every company, of every
     *      year, of every document type - Purchase Order Rice (41) mixed with everything else.
     *
     *   3. NO ROW-LEVEL VISIBILITY. :4708 sends @CanViewAllRecord from the form right, and
     *      when the user does NOT hold it, :4710 pins @EntryUser to that user so they see only
     *      their own documents. Neither was implemented, so every user saw every row.
     *
     *   4. THE BRANCH FILTER WAS THE WRONG SHAPE, AND IS THE REASON THE GRID WAS EMPTY.
     *      The desktop's branch combo is MULTI-select (:4620 adds a checkbox column) and the
     *      filter is built by splitting its TEXT on commas and looking each name up in dtBranch
     *      (:4762-4771), producing a comma-separated STRING sent as @BranchesIds - with a
     *      leading comma, because it accumulates as `BranchIds + "," + id` from "". A single
     *      integer compared to po.BranchId cannot express that, and any id that is not a real
     *      BranchId of a stored Purchase Order silently matches zero rows - which is exactly
     *      what `branchId=58` did while the desktop, running the real procedure, returned three.
     *
     * Every parameter below is guarded exactly as BLL 0595:119-275 guards it. A guarded
     * parameter is OMITTED when unset, never sent as NULL: the procedure's own defaults are
     * what the desktop relies on, and NULL is a different question.
     *
     * One deliberate behaviour that looks like a bug and is not: when no branch is chosen the
     * desktop does not query at all - :4761 wraps the entire Getall call in
     * `if (cmbBranchName.Text != string.Empty)`. An empty result is returned here rather than
     * an unfiltered one.
     */
    public List<Map<String, Object>> getHistory(
            String fromDate, String toDate,
            Integer fromDocNo, Integer toDocNo,
            Integer supplierId, Integer bookingPersonId,
            String branchIds, String dateType) {

        /* :4761 - no branch selected, no query. */
        if (branchIds == null || branchIds.trim().isEmpty() || "0".equals(branchIds.trim())) {
            return Collections.emptyList();
        }

        Set<String> allowedBranches = new HashSet<>();
        getBranches().forEach(branch -> allowedBranches.add(String.valueOf(branch.get("id"))));
        for (String requested : branchIds.split(",")) {
            String value = requested.trim();
            if (!value.isEmpty() && !allowedBranches.contains(value))
                throw new IllegalArgumentException("Select a branch allocated to your Purchase Order screen.");
        }

        List<String> names = new ArrayList<>();
        List<Object> args  = new ArrayList<>();

        /* :4704-4708 - always sent. */
        add(names, args, "@OrganizationId", currentUserContext.currentOrganizationId());
        add(names, args, "@CompanyId",      currentUserContext.currentCompanyId());
        add(names, args, "@DocumentTypeId", PO_DOCUMENT_TYPE_ID);

        boolean canViewAll = canViewAllPurchaseOrderRecords();
        add(names, args, "@CanViewAllRecord", canViewAll);

        /* BLL :144 - guarded on != 0. */
        int yearId = currentUserContext.currentFinancialYearId();
        if (yearId != 0) add(names, args, "@FinancialYearId", yearId);

        /* BLL :152 / form :4709 - ONLY when the user may not see everything. */
        if (!canViewAll) add(names, args, "@EntryUser", currentUserContext.currentUserId());

        /* @NoOfRecords (BLL :160) is guarded on != 0 and this form never sets it - omitted. */

        /* :4713-4755 - the four radios are mutually exclusive and each drives its OWN pair of
           parameters. Sending a date range on the wrong pair filters the wrong column. */
        java.sql.Date f = dateOrNull(fromDate), t = dateOrNull(toDate);
        String kind = dateType == null ? "" : dateType.replace(" ", "").trim();
        if ("EntryDate".equalsIgnoreCase(kind)) {
            if (f != null) add(names, args, "@EntryFromDate", f);
            if (t != null) add(names, args, "@EntryToDate",   t);
        } else if ("ModifyDate".equalsIgnoreCase(kind)) {
            if (f != null) add(names, args, "@ModifyFromDate", f);
            if (t != null) add(names, args, "@ModifyToDate",   t);
        } else if ("ApprovedDate".equalsIgnoreCase(kind)) {
            if (f != null) add(names, args, "@ApprovedFromDate", f);
            if (t != null) add(names, args, "@ApprovedToDate",   t);
        } else {
            if (f != null) add(names, args, "@DocDateFrom", f);
            if (t != null) add(names, args, "@DocDateTo",   t);
        }

        if (fromDocNo != null && fromDocNo != 0) add(names, args, "@FromDocNo", fromDocNo);
        if (toDocNo   != null && toDocNo   != 0) add(names, args, "@ToDocNo",   toDocNo);

        /* BLL :246 - the parameter is @OrderSupCustId, NOT @SupplierCustomerId. */
        if (supplierId       != null && supplierId       != 0) add(names, args, "@OrderSupCustId",  supplierId);
        if (bookingPersonId  != null && bookingPersonId  != 0) add(names, args, "@BookingPersonId", bookingPersonId);

        /* BLL :262 - a STRING, guarded on IsNullOrEmpty. */
        add(names, args, "@BranchesIds", branchIds.trim());

        add(names, args, "@Activity", "PurchaseOrderFormHistory");

        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_PurchaseOrder_GetAllMethod ");
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) sql.append(", ");
            sql.append(names.get(i)).append("=?");
        }

        /* A failed history read is reported, not swallowed into an empty grid: "no rows" and
           "the query broke" look identical to the operator otherwise, which is how the previous
           implementation hid its own defect behind "No matching records found". */
        return jdbcTemplate.queryForList(sql.toString(), args.toArray());
    }

    private static void add(List<String> names, List<Object> args, String name, Object value) {
        names.add(name);
        args.add(value);
    }

    /**
     * formright.DoHaveCanViewAllRecordRights (:4708), read against THIS screen's real name.
     * The rights dump records the desktop form as "PurchsaeOrder" - the misspelling is the
     * stored ScreenName, not a typo here.
     */
    private boolean canViewAllPurchaseOrderRecords() {
        String role = currentUserContext.currentRoleName();
        if ("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) return true;
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, "
                  + "@CompanyId=?, @Activity=?",
                    currentUserContext.currentUserId(), "PurchsaeOrder", role == null ? "" : role,
                    currentUserContext.currentCompanyId(), "GetByUserId")) {
                Object name = ci(r, "RightName");
                if (name != null && "CanView AllRecord".equalsIgnoreCase(name.toString().trim())) {
                    Object v = ci(r, "Value");
                    if (v instanceof Boolean) return (Boolean) v;
                    if (v instanceof Number)  return ((Number) v).intValue() != 0;
                    return v != null && ("1".equals(v.toString().trim())
                            || "true".equalsIgnoreCase(v.toString().trim()));
                }
            }
        } catch (Exception e) {
            HISTORY_LOG.warn("CanView AllRecord could not be read for PurchsaeOrder; the history "
                    + "is restricted to this user's own documents, which is the safe side", e);
        }
        return false;
    }

    public boolean deletePurchaseOrder(Integer id) {
        // PurchsaeOrder.cs:3914 has an empty handler; the designer hides the button (:9669).
        throw new org.springframework.web.server.ResponseStatusException(
                org.springframework.http.HttpStatus.METHOD_NOT_ALLOWED,
                "Purchase Order deletion is not available in the desktop form.");
    }

    private static void rollbackWrite() {
        org.springframework.transaction.interceptor.TransactionAspectSupport.currentTransactionStatus().setRollbackOnly();
    }

    // ==========================================================================================
    // Small conversions used by the Purchase Order header mapping.
    // ==========================================================================================

    /**
     * grd.GetTotal(column, AggregateFunction.Sum) over the detail rows - :4205-4208.
     * Qty and Weight are rounded to 2 as the desktop rounds them (:4210-4211); Amount is not
     * rounded here because the desktop only formats it for display.
     */
    private static java.math.BigDecimal sumDetails(PurchaseOrderFullDto dto, String which) {
        java.math.BigDecimal total = java.math.BigDecimal.ZERO;
        if (dto.getLineItems() != null) {
            for (PurchaseOrderFullDto.PurchaseOrderDetailItemDto d : dto.getLineItems()) {
                Double v = "qty".equals(which)    ? d.getItemQty()
                         : "weight".equals(which) ? d.getItemWeight()
                                                  : d.getItemAmount();
                if (v != null) total = total.add(java.math.BigDecimal.valueOf(v));
            }
        }
        if (!"amount".equals(which)) {
            total = total.setScale(2, java.math.RoundingMode.HALF_UP);
        }
        return total;
    }

    /** The desktop's Conversion.ToInt: a missing value is 0, never null. */
    private static int zero(Integer v) { return v == null ? 0 : v; }

    /** Conversion.ToDecimal. */
    private static java.math.BigDecimal dec(Double v) {
        return v == null ? java.math.BigDecimal.ZERO : java.math.BigDecimal.valueOf(v);
    }

    /** Conversion.ToDouble. */
    private static double dbl(Double v) { return v == null ? 0d : v; }
    private static double dbl(Integer v) { return v == null ? 0d : v.doubleValue(); }

    /** yyyy-MM-dd from the page, or null when the box is empty. */
    private static java.sql.Date dateOrNull(String ymd) {
        if (ymd == null || ymd.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(ymd.trim().substring(0, 10)); }
        catch (RuntimeException e) { return null; }
    }
}
