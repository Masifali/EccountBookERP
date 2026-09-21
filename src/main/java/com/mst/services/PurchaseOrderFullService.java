package com.mst.services;

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

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CurrentUserContext currentUserContext;

    /** The desktop's own header write path - Sp_PurchaseOrder_Insert / _Update. */
    @Autowired
    private PurchaseOrderHeaderRepository purchaseOrderHeaderRepository;

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
            for (Map<String, Object> r : raw) {
                int typeOfType = intOf(r.get("ItemTypeOfTypeId"));
                if (typeOfType == 14 || typeOfType == 17) continue;          /* desktop :1238, :1293 */

                if (parent > 0 && intOf(r.get("InventoryParentCategoriesId")) != parent) continue;

                String name = strOf(r.get("ItemName"));
                String code = strOf(r.get("ItemCode"));
                if (!q.isEmpty()
                        && !name.toLowerCase().contains(q)
                        && !code.toLowerCase().contains(q)) continue;

                Map<String, Object> m = new LinkedHashMap<>();
                m.put("id", intOf(r.get("Id")));
                m.put("itemName", name);
                m.put("itemCode", code);
                m.put("itemCategoryId", intOf(r.get("ItemCategoryId")));
                m.put("itemCategory", strOf(r.get("ItemCategory")));
                m.put("itemTypeId", intOf(r.get("ItemTypeId")));
                m.put("itemType", strOf(r.get("ItemType")));
                m.put("inventoryParentCategoriesId", intOf(r.get("InventoryParentCategoriesId")));
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
            LOG_ITEMS(e);
            return Collections.emptyList();
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

    public List<Map<String, Object>> getParentCategories() {
        try {
            String sql = "SELECT Id as id, CategoryDescription as description " +
                    "FROM ItemCategory " +
                    "WHERE (CategoryStatus IS NULL OR CategoryStatus = 1 OR CategoryStatus = 'true') " +
                    "AND CategoryDescription IS NOT NULL AND CategoryDescription <> '' " +
                    "ORDER BY CategoryDescription";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            e.printStackTrace();
            System.err.println("Error in getParentCategories: " + e.getMessage());
            return Collections.emptyList();
        }
    }

    /** Real City dropdown, ditto desktop's Architecture.BLL.City.GetAll() -> SP_City_GetAllMethod
     *  @MethodType='GetAll', org/company scoped. Rows whose OrganizationId/CompanyId are NULL are
     *  also included (legacy/global city rows created before org/company scoping existed on this
     *  table) so a real, populated City table never appears empty to the dropdown. */
    public List<Map<String, Object>> searchCities(String query) {
        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT Id as id, Description as cityName, TehsilId as tehsilId FROM City ")
              .append("WHERE Description IS NOT NULL ")
              .append("AND (OrganizationId = ").append(orgId).append(" OR OrganizationId IS NULL) ")
              .append("AND (CompanyId = ").append(compId).append(" OR CompanyId IS NULL) ");
            if (query != null && !query.trim().isEmpty()) {
                String q = query.trim().replace("'", "''");
                sb.append("AND Description LIKE '%").append(q).append("%' ");
            }
            sb.append("ORDER BY Description");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            return Collections.emptyList();
        }
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

    /** Real Chart of Account (4th-level/Detail) dropdown for "Account Credit _Charge to Product",
     *  based on CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,13,14,15,20,21,22"),
     *  with AccountTypeId=4 ('Inventory') added to the exclusion list to keep per-product Stock A/c
     *  accounts out of this dropdown - see the comment above SQL_COA_ACCOUNTS_FOR_CHARGE_TO_PRODUCT. */
    public List<Map<String, Object>> getAccountsForChargeToProduct() {
        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            int userId = currentUserContext.currentUserId();
            return jdbcTemplate.queryForList(SQL_COA_ACCOUNTS_FOR_CHARGE_TO_PRODUCT,
                    orgId, compId, userId, 1, "2,4,11,12,13,14,15,20,21,22", "GetAccountTitleByAccountTypeIds");
        } catch (Exception e) {
            return Collections.emptyList();
        }
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
        try {
            return jdbcTemplate.queryForList(SQL_PAYMENT_TERMS_DETAIL_BY_HEADER_ID, purchaseOrderId,
                    "PurchaseOrderPaymentTermDetailByHeaderId");
        } catch (Exception e) {
            return Collections.emptyList();
        }
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
            jdbcTemplate.update(SQL_SUPPLIER_EXPENSE_INSERT,
                    null, purchaseOrderId, itemId,
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
        jdbcTemplate.update("DELETE FROM PurchaseOrderExpensesChargeToProduct WHERE PurchaseOrderId=?", purchaseOrderId);
        if (rows == null) {
            return;
        }
        for (PurchaseOrderFullDto.PurchaseOrderExpensesChargeToProductDto row : rows) {
            double amount = row.getAmount() != null ? row.getAmount() : 0.0;
            if (amount <= 0.0) {
                continue;
            }
            int accountId = row.getAccountId() != null ? row.getAccountId() : 0;
            if (accountId == 0) {
                throw new IllegalArgumentException("AccountTitle Field Required");
            }
            jdbcTemplate.update(SQL_CHARGE_TO_PRODUCT_INSERT,
                    null, purchaseOrderId, accountId,
                    row.getPercentage() != null ? row.getPercentage() : 0.0,
                    row.getQty() != null ? row.getQty() : 0.0,
                    row.getRate() != null ? row.getRate() : 0.0,
                    amount,
                    row.getRemarks(),
                    row.getSupplierCustomerId() != null ? row.getSupplierCustomerId() : 0);
        }
    }

    /**
     * Real delete-then-reinsert persistence for Payment Detail rows, ditto Sp_PurchaseOrder_Update's
     * "DELETE PurchaseOrderPaymentTermsDetail WHERE PurchaseOrderId=@Id" followed by the DAL's per-row
     * USP_PurchaseOrderPaymentTermsDetail_Insert loop. PaymentTermId is required for every row, and
     * DueDays must be > 0 when PaymentTermId=2 (Credit) - ditto PurchsaeOrder.cs's validation
     * ("Payment Term Required in row#.." / "Due Days Required In case Of Credit row in row#..").
     */
    private void persistPaymentTermsDetail(int purchaseOrderId, List<PurchaseOrderFullDto.PurchaseOrderPaymentTermsDetailDto> rows) {
        jdbcTemplate.update("DELETE FROM PurchaseOrderPaymentTermsDetail WHERE PurchaseOrderId=?", purchaseOrderId);
        if (rows == null) {
            return;
        }
        int sortNo = 1;
        for (PurchaseOrderFullDto.PurchaseOrderPaymentTermsDetailDto row : rows) {
            int paymentTermId = row.getPaymentTermId() != null ? row.getPaymentTermId() : 0;
            if (paymentTermId <= 0) {
                throw new IllegalArgumentException("Payment Term Required in row#" + sortNo);
            }
            int dueDays = row.getDueDays() != null ? row.getDueDays() : 0;
            if (paymentTermId == 2 && dueDays <= 0) {
                throw new IllegalArgumentException("Due Days Required In case Of Credit row in row#" + sortNo);
            }
            jdbcTemplate.update(SQL_PAYMENT_TERMS_DETAIL_INSERT,
                    null, purchaseOrderId, paymentTermId,
                    row.getPrcntOfTotal() != null ? row.getPrcntOfTotal() : 0.0,
                    row.getAmount() != null ? row.getAmount() : 0.0,
                    dueDays,
                    row.getPaymentRemarks(),
                    sortNo,
                    row.getDueDate());
            sortNo++;
        }
    }

    /** Standalone persist for Supplier Expense against an EXISTING, already-saved Purchase Order Id. */
    @Transactional
    public Map<String, Object> saveSupplierExpense(Integer purchaseOrderId, List<PurchaseOrderFullDto.PurchaseOrderSupplierExpenseDto> rows) {
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
            response.put("message", "Error saving Supplier Expense: " + e.getMessage());
        }
        return response;
    }

    /** Standalone persist for Account Credit _Charge to Product against an EXISTING, already-saved Purchase Order Id. */
    @Transactional
    public Map<String, Object> saveChargeToProduct(Integer purchaseOrderId, List<PurchaseOrderFullDto.PurchaseOrderExpensesChargeToProductDto> rows) {
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
            response.put("message", "Error saving Account Credit _Charge to Product: " + e.getMessage());
        }
        return response;
    }

    /** Standalone persist for Payment Detail against an EXISTING, already-saved Purchase Order Id. */
    @Transactional
    public Map<String, Object> savePaymentTermsDetail(Integer purchaseOrderId, List<PurchaseOrderFullDto.PurchaseOrderPaymentTermsDetailDto> rows) {
        Map<String, Object> response = new HashMap<>();
        try {
            if (purchaseOrderId == null || purchaseOrderId <= 0) {
                throw new IllegalArgumentException("A saved Purchase Order Id is required before Payment Detail rows can be persisted.");
            }
            persistPaymentTermsDetail(purchaseOrderId, rows);
            response.put("success", true);
            response.put("message", "Payment Detail rows saved successfully.");
        } catch (Exception e) {
            response.put("success", false);
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
            jdbcTemplate.update(SQL_EMPTY_BAGS_INSERT,
                    null,
                    purchaseOrderId,
                    row.getType(),
                    row.getItemId() != null ? row.getItemId() : 0,
                    row.getPackingTypeId(),
                    row.getRate() != null ? row.getRate() : 0.0,
                    row.getWeightCut() != null ? row.getWeightCut() : 0.0);
        }
    }

    /** Real [dbo].[PurchaseOrderDetail] column list (verified against the decoded CREATE TABLE and
     *  against Sp_PurchaseOrderDetail_Insert's own INSERT/UPDATE branches). Id is NOT an IDENTITY
     *  column on this table (unlike PurchaseOrder.Id) so a new row's Id must be self-generated the
     *  same way the real proc does it: MAX(Id)+1. */
    private static final String SQL_DETAIL_NEXT_ID = "SELECT ISNULL(MAX(CONVERT(INT,Id)),0)+1 FROM PurchaseOrderDetail";

    private static final String SQL_DETAIL_INSERT = "INSERT INTO PurchaseOrderDetail (" +
            "Id, PurchaseOrderId, OrderItemId, OrderItemUOMId, OrderItemQty, NetWeight, OrderItemRate, " +
            "OrderItemRateUOMId, Amount, JobLotId, CityArea, CityId, LabSampleNo, OrderRemarks, Crop, CropYearId, " +
            "Moisture, EntryDate, EntryUserId" +
            ") VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, GETDATE(), ?)";

    private static final String SQL_DETAIL_UPDATE = "UPDATE PurchaseOrderDetail SET " +
            "OrderItemId = ?, OrderItemUOMId = ?, OrderItemQty = ?, NetWeight = ?, OrderItemRate = ?, " +
            "OrderItemRateUOMId = ?, Amount = ?, JobLotId = ?, CityArea = ?, CityId = ?, LabSampleNo = ?, " +
            "OrderRemarks = ?, Crop = ?, CropYearId = ?, Moisture = ?, ModifyDate = GETDATE(), ModifyUserId = ? " +
            "WHERE Id = ? AND PurchaseOrderId = ?";

    private static final String SQL_DETAIL_EXISTING_IDS = "SELECT Id FROM PurchaseOrderDetail WHERE PurchaseOrderId = ?";

    /** Ditto USP_DeletePurchaseOrderDetailIfNotExistInGrn's own GRN/Purchase-Invoice reference guard -
     *  a Detail row already pulled into a real Goods Receipt Note or Purchase Invoice must never be
     *  deleted, even if the user removed it from the on-screen grid. */
    private static final String SQL_DETAIL_REFERENCED_IN_GRN =
            "SELECT COUNT(*) FROM InvGrnDetail WHERE PurchaseOrderDetailId = ?";
    private static final String SQL_DETAIL_REFERENCED_IN_INVOICE =
            "SELECT COUNT(*) FROM InvPurchaseInvoiceDetail WHERE PurchaseOrderDetailId = ?";

    private static final String SQL_DETAIL_DELETE = "DELETE FROM PurchaseOrderDetail WHERE Id = ? AND PurchaseOrderId = ?";

    /**
     * Real per-row Insert/Update/Delete for the Purchase Order Detail grid, ditto desktop's
     * btnsave_Click() (Id==0 => insert, Id>0 => update, matching purchaseOrderDetailId round-tripped
     * from getPurchaseOrderById()'s own read) plus its OrderDetailRemoveIds /
     * USP_DeletePurchaseOrderDetailIfNotExistInGrn removed-row handling. Required-field validation
     * mirrors Sp_PurchaseOrderDetail_Insert's own RAISERROR checks (ItemId/ItemQty/NetWeight/
     * ItemRate/OrderItemRateUOMId/Amount) so a bad row is rejected with the same message the desktop
     * proc would raise instead of an opaque SQL error. Returns a list of soft warnings (e.g. a row
     * that could not be deleted because it is already referenced by a real GRN/Invoice) - it never
     * throws for those cases since the rest of the save must still be allowed to complete.
     */
    private List<String> persistPurchaseOrderDetail(int purchaseOrderId, List<PurchaseOrderFullDto.PurchaseOrderDetailItemDto> items, int userId) {
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

            String cityArea = item.getLoadingLocationCityName() != null ? item.getLoadingLocationCityName() : "";
            Integer cityId = item.getLoadingLocationCityId() != null && item.getLoadingLocationCityId() > 0 ? item.getLoadingLocationCityId() : null;
            // Desktop keeps LabSampleNo as a distinct field (Factory Sample vs Standard selector); the
            // current web UI does not yet expose that control, so it is left null here (never a
            // fabricated/guessed value) rather than reusing OrderRemarks for it.
            String crop = item.getCropYear() != null ? item.getCropYear() : "";
            Integer cropYearId = item.getCropYearId() != null && item.getCropYearId() > 0 ? item.getCropYearId() : null;
            String moisture = item.getMoisturePercent() != null ? String.valueOf(item.getMoisturePercent()) : null;
            Integer jobLotId = item.getJobLotId() != null && item.getJobLotId() > 0 ? item.getJobLotId() : null;

            Integer detailId = item.getPurchaseOrderDetailId() != null && item.getPurchaseOrderDetailId() > 0
                    ? item.getPurchaseOrderDetailId() : null;

            if (detailId != null && existingIds.contains(detailId)) {
                jdbcTemplate.update(SQL_DETAIL_UPDATE,
                        itemId, packUomId, qty, netWeight, rate, rateUomId, amount,
                        jobLotId, cityArea, cityId, null, item.getRemarks(), crop, cropYearId, moisture,
                        userId, detailId, purchaseOrderId);
                seenIds.add(detailId);
            } else {
                Integer newId = jdbcTemplate.queryForObject(SQL_DETAIL_NEXT_ID, Integer.class);
                jdbcTemplate.update(SQL_DETAIL_INSERT,
                        newId, purchaseOrderId, itemId, packUomId, qty, netWeight, rate, rateUomId, amount,
                        jobLotId, cityArea, cityId, null, item.getRemarks(), crop, cropYearId, moisture, userId);
                item.setPurchaseOrderDetailId(newId);
                seenIds.add(newId);
            }
        }

        // Rows that existed for this Purchase Order before this save but were not present in the
        // incoming grid - the user removed them. Delete individually, guarded by the same GRN/Invoice
        // reference check the real USP_DeletePurchaseOrderDetailIfNotExistInGrn proc performs.
        for (Integer oldId : existingIds) {
            if (seenIds.contains(oldId)) {
                continue;
            }
            Integer grnCount = jdbcTemplate.queryForObject(SQL_DETAIL_REFERENCED_IN_GRN, Integer.class, oldId);
            Integer invCount = jdbcTemplate.queryForObject(SQL_DETAIL_REFERENCED_IN_INVOICE, Integer.class, oldId);
            if ((grnCount != null && grnCount > 0) || (invCount != null && invCount > 0)) {
                warnings.add("Detail row Id " + oldId + " could not be removed because it already exists in a real GRN or Purchase Invoice.");
                continue;
            }
            jdbcTemplate.update(SQL_DETAIL_DELETE, oldId, purchaseOrderId);
        }

        return warnings;
    }

    @Transactional
    public Map<String, Object> savePurchaseOrder(PurchaseOrderFullDto dto, Integer userId) {
        Map<String, Object> response = new HashMap<>();
        try {
            if (dto.getSupplierId() == null || dto.getSupplierId() <= 0) {
                throw new IllegalArgumentException("Supplier / Party is required.");
            }
            if (dto.getLineItems() == null || dto.getLineItems().isEmpty()) {
                throw new IllegalArgumentException("At least one detail item is required.");
            }

            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            int branchId = currentUserContext.currentBranchId();
            int effUserId = userId != null ? userId : currentUserContext.currentUserId();

            int docNo = dto.getDocNo() != null && dto.getDocNo() > 0 ? dto.getDocNo() : generateNextDocNo(dto.getDocumentTypeId());
            String docDate = dto.getDocDate() != null && !dto.getDocDate().isEmpty() ? dto.getDocDate() : new java.text.SimpleDateFormat("yyyy-MM-dd").format(new Date());

            Integer poMasterId = dto.getPurchaseOrderMasterId();
            int recId = (poMasterId != null && poMasterId > 0) ? poMasterId : 0;

            /* ------------------------------------------------------------------------------
               BLL 0595 Architecture.BLL.Inventory.PurchaseOrder.Save(obj)

               Step 1 - the date lock, BEFORE anything is written. The desktop refuses the whole
               save when the document date is on or before the configured lock date.
               ------------------------------------------------------------------------------ */
            java.sql.Date docDateSql = java.sql.Date.valueOf(docDate);
            purchaseOrderHeaderRepository.assertNotDateLocked(orgId, compId, docDateSql);

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
            head.put("DocumentTypeId", dto.getDocumentTypeId());
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
            head.put("OrderDueDate",   dateOrNull(dto.getPaymentDueDate()));
            head.put("OrderExpiryDate", nowTs);                          // :3311 DateTime.Now
            head.put("DeliveryTermId", zero(dto.getDeliveryTermId()));
            head.put("DeliveryTerm",   dto.getDeliveryTermName());
            head.put("DeliveryStartDate", dateOrNull(dto.getDeliveryStartDate()));
            head.put("DeliveryDays",   zero(dto.getDeliveryDays()));
            head.put("OrderCatagoryId", zero(dto.getOrderCategoryId()));
            head.put("CatagorySrNo",   zero(dto.getCategorySrNo()));
            head.put("OrderStatus",    dto.getOrderStatus());
            head.put("LocationTypeId", zero(dto.getLocationTypeId()));
            head.put("OrderQty",       dec(dto.getOrderQty()));
            head.put("OrderWeight",    dec(dto.getOrderWeight()));
            head.put("OrderAmount",    dec(dto.getOrderAmount()));
                                                head.put("EntryUser",      effUserId);
            head.put("EntryDate",      nowTs);
            head.put("ModifyUser",     effUserId);
            head.put("ModifyDate",     nowTs);
            head.put("PostState",      Boolean.FALSE);
            head.put("OrderTaxable",   Boolean.FALSE);
            /* CurrencyId / ExchangeRate / FcyAmount are written by the desktop (:3318-3320) from
               cmbCurrency, txtExchangeRate and txtFcyAmount. This web page has no such controls,
               so they are left unsent and the procedure's own defaults apply. Recorded as an open
               item rather than filled with an invented rate - a fabricated exchange rate of 1 is
               exactly the kind of default this port has been removing elsewhere. */

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
                Boolean stored = jdbcTemplate.queryForObject(
                        "SELECT ISNULL(IsAproved,0) FROM PurchaseOrder WHERE Id = ?",
                        Boolean.class, recId);
                head.put("IsAproved", Boolean.TRUE.equals(stored));
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
            List<String> detailWarnings = persistPurchaseOrderDetail(poMasterId, dto.getLineItems(), effUserId);
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
            persistExpensesChargeToProduct(poMasterId, dto.getExpensesChargeToProduct());
            persistPaymentTermsDetail(poMasterId, dto.getPaymentTermsDetail());

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
            } catch (IllegalStateException noTx) {
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

    public Map<String, Object> getPurchaseOrderById(Integer id) {
        try {
            String headSql = "SELECT po.Id as purchaseOrderMasterId, po.DocumentTypeId as documentTypeId, po.DocNo as docNo, " +
                    "CONVERT(VARCHAR(10), po.DocDate, 120) as docDate, po.SupplierCustomerId as supplierId, " +
                    "po.RemarksHeader as remarksHeader, s.CompanyName as supplierName, " +
                    "ISNULL(s.SupCustCode, s.ManualPartyCode) as supplierCode, " +
                    "po.BrokerAgentId as brokerAccountId, brokerAcc.CompanyName as brokerAccountName, " +
                    "po.CommissionAgentId as commissionAgentId, commAgent.CompanyName as commissionAgentName, " +
                    "po.BookingPersonId as bookingPersonId, bookingP.ReferencePartyName as bookingPersonName " +
                    "FROM PurchaseOrder po " +
                    "LEFT JOIN SupplierCustomer s ON po.SupplierCustomerId = s.Id " +
                    "LEFT JOIN SupplierCustomer brokerAcc ON po.BrokerAgentId = brokerAcc.Id " +
                    "LEFT JOIN SupplierCustomer commAgent ON po.CommissionAgentId = commAgent.Id " +
                    "LEFT JOIN ReferenceParties bookingP ON po.BookingPersonId = bookingP.Id " +
                    "WHERE po.Id = ?";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(headSql, id);
            if (list == null || list.isEmpty()) {
                return null;
            }
            Map<String, Object> head = new HashMap<>(list.get(0));
            if (head.get("docNo") != null) {
                int docNo = ((Number) head.get("docNo")).intValue();
                head.put("displayCode", String.format("PO-%d", docNo));
                head.put("branchNo", docNo);
            }

            try {
                // Real read, ditto Sp_PurchaseOrderDetail_GetAllMethod's own column list and joins
                // (Item / JobLot / City / UOMSchedule+UOM for the Pack UOM and Rate UOM display text).
                // Field names below match the exact camelCase shape the Detail-tab JS already builds
                // for a freshly-added row (see countx_purchase_order_full.js btnAddDetailRow_Click's
                // `line` object) so an existing Purchase Order's saved rows render identically to a
                // row the user just added, and so purchaseOrderDetailId round-trips back on Save/Update.
                String detailSql = "SELECT d.Id as purchaseOrderDetailId, d.OrderItemId as itemId, " +
                        "i.ItemCode as itemCode, i.ItemName as itemName, " +
                        "d.CropYearId as cropYearId, ISNULL(d.Crop, '') as cropYear, " +
                        "d.OrderItemUOMId as packUomId, packUom.UOMCode as packUomCode, " +
                        "d.OrderItemQty as itemQty, d.NetWeight as itemWeight, d.OrderItemRate as itemRate, " +
                        "d.OrderItemRateUOMId as rateUomId, rateUom.UOMCode as rateUomCode, " +
                        "d.Amount as itemAmount, " +
                        "d.JobLotId as jobLotId, jl.JobLotDescription as jobLotName, " +
                        "d.CityId as loadingLocationCityId, ISNULL(c.Description, d.CityArea) as loadingLocationCityName, " +
                        "d.Moisture as moisturePercent, ISNULL(d.OrderRemarks, '') as remarks " +
                        "FROM PurchaseOrderDetail d " +
                        "LEFT JOIN Item i ON d.OrderItemId = i.Id " +
                        "LEFT JOIN JobLot jl ON d.JobLotId = jl.Id " +
                        "LEFT JOIN City c ON d.CityId = c.Id " +
                        "LEFT JOIN UOMSchedule packSched ON d.OrderItemUOMId = packSched.Id " +
                        "LEFT JOIN UOM packUom ON packSched.ScheduleUnitId = packUom.Id " +
                        "LEFT JOIN UOMSchedule rateSched ON d.OrderItemRateUOMId = rateSched.Id " +
                        "LEFT JOIN UOM rateUom ON rateSched.ScheduleUnitId = rateUom.Id " +
                        "WHERE d.PurchaseOrderId = ? " +
                        "ORDER BY d.Id";
                head.put("lineItems", jdbcTemplate.queryForList(detailSql, id));
            } catch (Exception detailEx) {
                head.put("lineItems", Collections.emptyList());
            }

            // Packing Material (Empty Bags) - real, DB-verified read, independent of the (still
            // legacy/fabricated) header+detail queries above. This is what fixes the reported bug:
            // an existing Purchase Order's saved Empty Bags rows (e.g. Jute Bags / PP Bags) are now
            // actually returned instead of always coming back empty.
            head.put("emptyBags", getEmptyBagsByHeaderId(id));

            // Supplier Expense / Account Credit _Charge to Product / Payment Detail - real,
            // DB-verified reads, ditto the Empty Bags read above (independent of the legacy
            // header+detail queries).
            head.put("supplierExpenses", getSupplierExpenseByHeaderId(id));
            head.put("expensesChargeToProduct", getExpensesChargeToProductByHeaderId(id));
            head.put("paymentTermsDetail", getPaymentTermsDetailByHeaderId(id));

            return head;
        } catch (Exception e) {
            return null;
        }
    }

    public List<Map<String, Object>> getBranches() {
        try {
            return jdbcTemplate.queryForList("SELECT Id as id, BranchName as branchName FROM Branch ORDER BY BranchName");
        } catch (Exception e) {
            try {
                return jdbcTemplate.queryForList("SELECT Id as id, BranchName as branchName FROM Branches ORDER BY BranchName");
            } catch (Exception ex) {
                return Collections.emptyList();
            }
        }
    }

    public List<Map<String, Object>> getHistory(
            String fromDate, String toDate,
            Integer fromDocNo, Integer toDocNo,
            Integer supplierId, Integer bookingPersonId,
            Integer branchId, String dateType) {
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT po.Id as id, po.DocNo as docNo, ")
              .append("CONVERT(VARCHAR(10), po.DocDate, 120) as docDate, ")
              .append("CONVERT(VARCHAR(10), po.EntryDate, 120) as entryDate, ")
              .append("CONVERT(VARCHAR(10), po.ModifyDate, 120) as modifyDate, ")
              .append("CONVERT(VARCHAR(10), po.ApprovedDate, 120) as approvedDate, ")
              .append("s.CompanyName as supplierName, ISNULL(s.SupCustCode, s.ManualPartyCode) as supplierCode, ")
              .append("bp.ReferencePartyName as bookingPersonName, ")
              .append("ISNULL(b.BranchName, '') as branchName, ISNULL(po.BranchSrNo, 0) as branchSrNo, ")
              .append("po.RemarksHeader as remarks, ")
              .append("('PO-' + CAST(po.DocNo AS VARCHAR)) as voucherCode, ")
              .append("('PO-' + CAST(po.DocNo AS VARCHAR)) as displayCode, ")
              .append("(SELECT ISNULL(SUM(OrderItemQty), 0) FROM PurchaseOrderDetail WHERE PurchaseOrderId = po.Id) as orderQty, ")
              .append("(SELECT ISNULL(SUM(Amount), 0) FROM PurchaseOrderDetail WHERE PurchaseOrderId = po.Id) as totalAmount, ")
              .append("po.OrderDueDays as dueDays, CONVERT(VARCHAR(10), po.OrderDueDate, 120) as dueDate, ")
              .append("po.DeliveryTerm as deliveryTerm, po.DeliveryDays as deliveryDays, ")
              .append("CONVERT(VARCHAR(10), po.DeliveryStartDate, 120) as deliveryStartDate, ")
              .append("ISNULL(po.OrderStatus, 'Open') as orderStatus ")
              .append("FROM PurchaseOrder po ")
              .append("LEFT JOIN SupplierCustomer s ON po.SupplierCustomerId = s.Id ")
              .append("LEFT JOIN ReferenceParties bp ON po.BookingPersonId = bp.Id ")
              .append("LEFT JOIN Branch b ON po.BranchId = b.Id ")
              .append("WHERE 1=1 ");

            List<Object> params = new ArrayList<>();

            String dateCol = "po.DocDate";
            if ("EntryDate".equalsIgnoreCase(dateType)) {
                dateCol = "po.EntryDate";
            } else if ("ModifyDate".equalsIgnoreCase(dateType)) {
                dateCol = "po.ModifyDate";
            } else if ("ApprovedDate".equalsIgnoreCase(dateType)) {
                dateCol = "po.ApprovedDate";
            }

            if (fromDate != null && !fromDate.trim().isEmpty()) {
                sb.append("AND ").append(dateCol).append(" >= ? ");
                params.add(fromDate.trim());
            }
            if (toDate != null && !toDate.trim().isEmpty()) {
                sb.append("AND ").append(dateCol).append(" <= ? ");
                params.add(toDate.trim() + " 23:59:59");
            }
            if (fromDocNo != null && fromDocNo > 0) {
                sb.append("AND po.DocNo >= ? ");
                params.add(fromDocNo);
            }
            if (toDocNo != null && toDocNo > 0) {
                sb.append("AND po.DocNo <= ? ");
                params.add(toDocNo);
            }
            if (supplierId != null && supplierId > 0) {
                sb.append("AND po.SupplierCustomerId = ? ");
                params.add(supplierId);
            }
            if (bookingPersonId != null && bookingPersonId > 0) {
                sb.append("AND po.BookingPersonId = ? ");
                params.add(bookingPersonId);
            }
            if (branchId != null && branchId > 0) {
                sb.append("AND (po.BranchId = ? OR po.BranchesId = ?) ");
                params.add(branchId);
                params.add(branchId);
            }

            sb.append("ORDER BY po.DocNo DESC");
            return jdbcTemplate.queryForList(sb.toString(), params.toArray());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public boolean deletePurchaseOrder(Integer id) {
        try {
            jdbcTemplate.update("DELETE FROM PurchaseOrderDetail WHERE PurchaseOrderId = ?", id);
            jdbcTemplate.update("DELETE FROM PurchaseOrderEmptyBags WHERE PurchaseOrderId = ?", id);
            jdbcTemplate.update("DELETE FROM PurchaseOrder WHERE Id = ?", id);
            return true;
        } catch (Exception e) {
            return false;
        }
    }

    // ==========================================================================================
    // Small conversions used by the Purchase Order header mapping.
    // ==========================================================================================

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
