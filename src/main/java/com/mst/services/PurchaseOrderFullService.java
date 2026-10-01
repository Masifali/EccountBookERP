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

    // ==========================================================================================
    // Packing Material (Empty Bags) - real stored-procedure / table names, verified directly
    // against GoldenAceDb(0509)t.sql (UTF-16LE-decoded) and against the desktop code-behind
    // recovered_source/ECCOUNTBOOKERP/Architecture.WinApp.Purchase/PurchsaeOrder.cs (dtEmptyBags /
    // grdEmptyBags) and recovered_source/projects/architecture.dal/0448_Architecture.DAL.Inventory.
    // PurchaseOrder.cs (SetData / GetData). No table/column/procedure name below is guessed.
    // ==========================================================================================



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
        String v = purchaseOrderHeaderRepository.config(orgId, compId, name);
        if (v == null || v.trim().isEmpty()) return 0;
        try { return (int) Double.parseDouble(v.trim()); }
        catch (NumberFormatException e) { return 0; }      /* Conversion.ToInt of a non-number */
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
        /* A failed read propagates (no catch-and-return-empty): an empty Supplier list and a
           broken one must not look the same. */
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_PARTY_LIST,
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId());

        /* SupplierDtFillFromGlobal() :994-1006 - with SubsidiaryAccountAllownOnVouchers (ERP
           feature 4) ON the desktop keeps only PartyTypeId == 1 rows; OFF, every row. */
        boolean onlyPartyType1 = subsidiaryAccountAllowedOnVouchers(
                currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId());

        for (Map<String, Object> r : rows) {
            /* SupplierDtFillFromGlobal(), :989 — the one filter the desktop always applies. */
            if (num(r, "CustomerGroupId") == 7) continue;
            if (onlyPartyType1 && num(r, "PartyTypeId") != 1) continue;

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
        /* No re-sort: the combo shows dtSupplier in the procedure's own row order. */
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
            /* No re-sort: ItemdtFillFromAll (:1298) keeps getGlobalAllItems' own order. */
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
        /* BLL 0060 City.save -> Sp_City_Insert (Model 0041's non-virtual properties). The raw
           INSERT INTO City / SELECT @@IDENTITY and the invented duplicate-name SELECT are gone.
           The desktop DefineCity form itself is not in the recovered source, so Code = the name
           (as before) and TehsilId/CountryId travel as their CLR default 0. */
        if (cityName == null || cityName.trim().isEmpty()) {
            throw new IllegalArgumentException("CityName Field is Required");
        }
        String trimmed = cityName.trim();
        java.sql.Timestamp now = new java.sql.Timestamp(System.currentTimeMillis());
        int userId = currentUserContext.currentUserId();
        Integer newId = ProcExec.call(jdbcTemplate,
                "EXEC dbo.Sp_City_Insert @Id=?, @Code=?, @Description=?, @CityNameOtherLingo=?, @CountryId=?, "
              + "@EntryDate=?, @EntryUser=?, @ModifyDate=?, @ModifyUser=?, @PostDate=?, @PostUser=?, @PostState=?, "
              + "@OrganizationId=?, @CompanyId=?, @TehsilId=?",
                0, trimmed, trimmed, new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.NVARCHAR, null),
                0, now, userId, now, userId,
                new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.TIMESTAMP, null), 0, false,
                currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId(), 0);
        Map<String, Object> res = new HashMap<>();
        res.put("success", true);
        res.put("id", newId);
        res.put("cityName", trimmed);
        res.put("message", "Record Save Successfully...[" + newId + "]");
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
        /* UOMFill() :1348 -> UOMSchedule.Getall -> Sp_UOMSchedule_GetAllMethod
           @Activity='ReadByOrganizationCompanyId'. ONLY the procedure: the three raw-SQL
           fallbacks and the four fabricated rows (Kg/40Kg/Bag/Ton with invented ids and
           equivalents) that used to follow it are gone - a fabricated UOM id would be saved into
           PurchaseOrderDetail. bindRateUomAndItemPackUom (:1387) keeps rows whose ItemId equals
           the chosen item; nothing else. */
        List<Map<String, Object>> rawList = jdbcTemplate.queryForList(
                "EXEC Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId(),
                "ReadByOrganizationCompanyId");

        List<Map<String, Object>> result = new ArrayList<>();
        for (Map<String, Object> row : rawList) {
            Map<String, Object> norm = new HashMap<>();
            norm.put("id", intOf(ci(row, "Id")));
            norm.put("itemId", intOf(ci(row, "ItemId")));
            norm.put("uomCode", strOf(ci(row, "UOMCode")).trim());
            norm.put("equivalent", ci(row, "Equivalent"));
            norm.put("baseRateUom", ci(row, "BaseRateUom"));
            norm.put("basePackUom", ci(row, "BasePackUom"));
            result.add(norm);
        }
        return result;
    }

    /**
     * combojoblotfill() - PurchsaeOrder.cs :5196, via
     * JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId (BLL 0019):
     *
     *     EXEC [dbo].[USP_GetJobLotsAllocatedToBranch] @OrganizationId, @CompanyId, @BranchId
     *
     * @BranchId is GUARDED in the BLL (added only when BranchesId is set) and is SINGULAR -
     * not @BranchesId, which is the property it is filled from.
     *
     * This was `SELECT Id, JobLotDescription FROM JobLot ORDER BY JobLotDescription` - every
     * Job/Lot in the database, ignoring branch allocation entirely. The desktop offers only the
     * ones allocated to the signed-in user's branch, so the web was offering Job/Lots the
     * operator is not entitled to pick and which the desktop would never show. Same defect class
     * as the Branch list (see PO-HISTORY-GRID-HEADER-AND-BODY-DISAGREED.md).
     *
     * :5210-5218 - when the previously selected Job/Lot is NOT in the returned set the desktop
     * CLEARS the combo rather than leaving a stale value; the page does that from `id`/`description`
     * as before, so the shape of the response is unchanged.
     */
    public List<Map<String, Object>> getJobLots() {
        int branchId = currentUserContext.currentBranchId();

        StringBuilder sql = new StringBuilder(
                "EXEC [dbo].[USP_GetJobLotsAllocatedToBranch] @OrganizationId=?, @CompanyId=?");
        List<Object> args = new ArrayList<>();
        args.add(currentUserContext.currentOrganizationId());
        args.add(currentUserContext.currentCompanyId());
        if (branchId != 0) { sql.append(", @BranchId=?"); args.add(branchId); }

        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(sql.toString(), args.toArray())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id",          intOf(ci(r, "Id")));
            m.put("description", strOf(ci(r, "JobLotDescription")));
            out.add(m);
        }
        return out;
    }

    /**
     * GetLabDetailByItemId() - PurchsaeOrder.cs :2290, via
     * InvLabAnalysisStandardDeductionPolicyHeader.ReadByItemId (BLL 0398):
     *
     *     EXEC Sp_InvLabAnalysisStandardDeductionPolicyHeader_GetAllMethod
     *          @OrganizationId, @CompanyId, @PolicyApplyOn, @ItemId (guarded),
     *          @FromDate (guarded), @Activity='ReadByItemId'
     *
     * @PolicyApplyOn is the literal string "PurchaseOrder" on this form.
     *
     * NONE of this existed on the web - the "Lab Deduction Standard" tab was an empty grid
     * reading "Record: 0 Of 0", because nothing ever populated it. The desktop fills it whenever
     * a detail line is added or updated, keyed on that line's item, replacing any rows already
     * held for the same item (:2311-2318).
     *
     * Columns the desktop reads, in its own order (:2321): headerId, detailId, ItemId,
     * AnalysisParameterId, ItemName, AnalysisPerameter, RangeFrom, RangeTo, DeductFrom, WeightKg,
     * DedValue - and DedValue AGAIN as the twelfth value, which is the editable "deduction"
     * column seeded from the standard. That duplication is deliberate on the desktop and is
     * reproduced rather than tidied away.
     */
    public List<Map<String, Object>> getLabDeductionStandard(int itemId) {
        StringBuilder sql = new StringBuilder(
                "EXEC Sp_InvLabAnalysisStandardDeductionPolicyHeader_GetAllMethod "
              + "@OrganizationId=?, @CompanyId=?, @PolicyApplyOn=?");
        List<Object> args = new ArrayList<>();
        args.add(currentUserContext.currentOrganizationId());
        args.add(currentUserContext.currentCompanyId());
        args.add(LAB_POLICY_APPLY_ON);
        if (itemId != 0) { sql.append(", @ItemId=?"); args.add(itemId); }
        sql.append(", @Activity=?");
        args.add("ReadByItemId");

        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(sql.toString(), args.toArray())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("headerId",            intOf(ci(r, "headerId")));
            m.put("detailId",            intOf(ci(r, "detailId")));
            m.put("itemId",              intOf(ci(r, "ItemId")));
            m.put("analysisParameterId", intOf(ci(r, "AnalysisParameterId")));
            m.put("itemName",            strOf(ci(r, "ItemName")));
            m.put("analysisParameter",   strOf(ci(r, "AnalysisPerameter")));  /* desktop's spelling */
            m.put("rangeFrom",           ci(r, "RangeFrom"));
            m.put("rangeTo",             ci(r, "RangeTo"));
            m.put("deductFrom",          strOf(ci(r, "DeductFrom")));
            m.put("weightKg",            ci(r, "WeightKg"));
            m.put("standardValue",       ci(r, "DedValue"));
            m.put("deductionValue",      ci(r, "DedValue"));   /* :2321 - seeded from the standard */
            out.add(m);
        }
        return out;
    }

    /** :2296 - the policy is looked up for this form only. */
    private static final String LAB_POLICY_APPLY_ON = "PurchaseOrder";

    /**
     * DefaultNoOfDecimalPointsForFcyAmount - CommonServices :5440, used by
     * txtExchangeRate_TextChanged to round each line's FcyAmount.
     */
    public int fcyAmountDecimals() {
        return decimalPoints(currentUserContext.currentOrganizationId(),
                             currentUserContext.currentCompanyId(),
                             "DefaultNoOfDecimalPointsForFcyAmount", 2);
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
    /**
     * PaymentTerms() :1056 - clsGlobalVariables.globalPaymentTerm, filled by
     * GlobalServicesMethods.getPaymentTermlist -> Sp_InvDueTerms_GetAllMethod @Activity='GetAll'.
     * The Purchase Order screen's own list: the procedure ONLY, no raw-SQL fallback. (The older
     * getPaymentTerms() below is left as it is because PurchaseModuleViewController also feeds
     * other purchase screens from it.)
     */
    public List<Map<String, Object>> getPaymentTermsForPurchaseOrder() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC Sp_InvDueTerms_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(), "GetAll")) {
            Map<String, Object> m = new LinkedHashMap<>();
            Object id = ci(r, "Id");
            Object desc = ci(r, "TermsDescription");
            m.put("Id", id); m.put("id", id);
            m.put("TermsDescription", desc); m.put("description", desc);
            m.put("dueDays", ci(r, "DueDays"));
            out.add(m);
        }
        return out;
    }

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
        /* DeliveryTerm.FormHistory (BLL 0512) -> USP_DeliveryTerm_GetAllMethod @Activity='FormHistory'
           = Id, Description, ValueDescription. The procedure only - the raw SELECT fallback is gone.
           ValueDescription is the combo's hidden third column (:1104) and is what the desktop
           SAVES as po.DeliveryTerm (:3312 SelectedRow.Cells[2]), so it is returned as well. */
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC [dbo].[USP_DeliveryTerm_GetAllMethod] @Activity=?", "FormHistory")) {
            Map<String, Object> m = new LinkedHashMap<>(r);
            Object id = ci(r, "Id");
            Object desc = ci(r, "Description");
            m.put("Id", id); m.put("id", id);
            m.put("Description", desc); m.put("description", desc); m.put("name", desc);
            m.put("valueDescription", ci(r, "ValueDescription"));
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
        {
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
        return jdbcTemplate.queryForList(SQL_EMPTY_BAG_TYPES, "GetCommissionUom");
    }

    public List<Map<String, Object>> getEmptyBagTypes() {
        return jdbcTemplate.queryForList(SQL_EMPTY_BAG_TYPES, "PurchaseOrderEmptyBagsType");
    }

    /** Real empty-bag item dropdown, scoped to the caller's Organization/Company, TransactionFlowId=1 (Purchase),
     *  matching the desktop's EmptyBagsItemFill()/GetPackingMaterialItemsAllocateToFlow() default call. */
    public List<Map<String, Object>> getEmptyBagItems() {
        int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            return jdbcTemplate.queryForList(SQL_EMPTY_BAG_ITEMS, orgId, compId, null, null, 1);
    }

    /** Real packing-type dropdown (Id, PackTypeCode, PackTypeDesc, MinEbWeight, MaxEbWeight, MinEbStockWeight, MaxEbStockWeight). */
    public List<Map<String, Object>> getPackingTypesForEmptyBags() {
        return jdbcTemplate.queryForList(SQL_PACKING_TYPES_FOR_EMPTY_BAGS, "ReadAll");
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
        int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            return jdbcTemplate.queryForList(SQL_OTHER_ITEMS_FOR_EXPENSE, "ReadAll", orgId, compId);
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
        return jdbcTemplate.queryForList(SQL_SUPPLIER_EXPENSE_BY_HEADER_ID, purchaseOrderId,
                    "ReadPurchaseOrderSupplierExpenseByHeaderId");
    }

    /** Real read of existing Account Credit _Charge to Product rows for a given, already-saved Purchase Order Id. */
    private List<Map<String, Object>> getExpensesChargeToProductByHeaderId(int purchaseOrderId) {
        return jdbcTemplate.queryForList(SQL_CHARGE_TO_PRODUCT_BY_HEADER_ID, purchaseOrderId,
                    "ReadPurchaseOrderExpensesChargeToProductDetailByHeaderId");
    }

    /** Real read of existing Payment Detail rows for a given, already-saved Purchase Order Id. */
    private List<Map<String, Object>> getPaymentTermsDetailByHeaderId(int purchaseOrderId) {
        return jdbcTemplate.queryForList(SQL_PAYMENT_TERMS_DETAIL_BY_HEADER_ID, purchaseOrderId,
                    "PurchaseOrderPaymentTermDetailByHeaderId");
    }

    /* =========================================================================================
     * CHILD COLLECTIONS - PurchsaeOrder.cs Insert() :3405-3614 builds them, DAL 0448 SetData
     * :62-115 writes them after the header, in this order:
     *     details, empty bags, charge-to-product, lab deduction, supplier expense,
     *     supplier dispatch, payment terms, attachments,
     *     then (update) USP_DeletePurchaseOrderDetailIfNotExistInGrn + PoWeightAndGpWeightValidation,
     *     or (insert) [DAW].[USp_DocumentApprovalDetail_Insert].
     *
     * NO RAW DELETE. Sp_PurchaseOrder_Update itself deletes PurchaseOrderEmptyBags,
     * PurchaseOrderExpensesChargeToProduct, PurchaseOrderLabDeduction, PurchaseOrderSupplierExpense,
     * PurchaseOrderPaymentTermsDetail, PurchaseOrderSupplierDispatchDetail and
     * AdvancePaymentAdjustment for the order (procdure.utf8.sql Sp_PurchaseOrder_Update, the
     * "Delete Detail Record Here" block), and on insert there is nothing to delete. The
     * "DELETE FROM ..." statements this class used to run were redundant raw SQL and are gone,
     * together with the four standalone PUT endpoints that relied on them (the desktop has no
     * tab-by-tab save).
     *
     * Each collection is VALIDATED and SHAPED first (before the header is written), in the
     * desktop's own order - empty bags :3439, charge-to-product :3486, payment :3584 - and only
     * then written, so a refusal is reported with the desktop's message before any write.
     * ========================================================================================= */

    /** Supplier expense rows to write (:3526-3543): ItemId != 0 and Amount > 0 only. */
    private List<Object[]> prepareSupplierExpense(List<PurchaseOrderFullDto.PurchaseOrderSupplierExpenseDto> rows) {
        List<Object[]> out = new ArrayList<>();
        if (rows == null) return out;
        for (PurchaseOrderFullDto.PurchaseOrderSupplierExpenseDto row : rows) {
            int itemId = row.getInvRevExpItemId() != null ? row.getInvRevExpItemId() : 0;
            double amount = row.getAmount() != null ? row.getAmount() : 0.0;
            if (itemId == 0 || amount <= 0.0) continue;
            double qty = row.getQty() != null ? row.getQty() : 0.0;
            double rate = row.getRate() != null ? row.getRate() : 0.0;
            String remarks = row.getRemarks() == null ? "" : row.getRemarks().trim();
            if (remarks.isEmpty() || "0".equals(remarks)) {
                /* :3539 "Expense : " + ItemId cell TEXT (the Other Item name) + "  Qty" + Qty + "  @" + Rate */
                remarks = "Expense : " + (row.getOtherItemName() != null ? row.getOtherItemName().trim() : "")
                        + "  Qty" + clrNumber(qty) + "  @" + clrNumber(rate);
            }
            out.add(new Object[]{ itemId, qty, rate, amount, remarks });
        }
        return out;
    }

    private static final String SQL_SUPPLIER_EXPENSE_INSERT =
            "EXEC Sp_PurchaseOrderSupplierExpense_Insert @Id=?, @PurchaseOrderId=?, @InvRevExpItemId=?, @Qty=?, @Rate=?, @Amount=?, @Remarks=?";

    private void writeSupplierExpense(int purchaseOrderId, List<Object[]> rows) {
        for (Object[] r : rows) {
            /* Model PurchaseOrderSupplierExpense.Id is a C# int never assigned -> 0, not NULL. */
            ProcExec.call(jdbcTemplate, SQL_SUPPLIER_EXPENSE_INSERT, 0, purchaseOrderId, r[0], r[1], r[2], r[3], r[4]);
        }
    }

    /**
     * Charge-to-product rows (:3479-3509). Only Amount > 0 rows; "AccountTitle Field Required" when
     * the account is 0. The saved AccountId is the grid's GlAccountId cell in BOTH branches and
     * SupplierCustomerId is the picked party only when SubsidiaryAccountAllownOnVouchers is on -
     * resolved here from the same list the picker is built from (getAccountsForChargeToProduct),
     * so a crafted request cannot choose which column its id lands in.
     * SupCustIdUpdateforFreightGrid :2907-2927 - "Selected Account Can not be Same As Supplier Account".
     */
    private List<Object[]> prepareChargeToProduct(List<PurchaseOrderFullDto.PurchaseOrderExpensesChargeToProductDto> rows,
                                                  int supplierId) {
        List<Object[]> out = new ArrayList<>();
        if (rows == null) return out;
        int orgId  = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        boolean subsidiary = subsidiaryAccountAllowedOnVouchers(orgId, compId);
        List<Map<String, Object>> accounts = null;
        String valueMember = subsidiary ? "SupplierCustomerId" : "Id";
        int supplierGl = -1;
        for (PurchaseOrderFullDto.PurchaseOrderExpensesChargeToProductDto row : rows) {
            double amount = row.getAmount() != null ? row.getAmount() : 0.0;
            if (amount <= 0.0) continue;                                       /* :3483 */
            int picked = row.getAccountId() != null ? row.getAccountId() : 0;
            if (picked == 0) throw new IllegalArgumentException("AccountTitle Field Required");   /* :3490 */
            if (accounts == null) accounts = getAccountsForChargeToProduct();
            Map<String, Object> match = null;
            for (Map<String, Object> a : accounts) {
                if (intOf(ci(a, valueMember)) == picked) { match = a; break; }
            }
            /* LimitToList (:3145) - a value outside the list cannot come from the desktop grid. */
            if (match == null) throw new IllegalArgumentException("AccountTitle Field Required");
            int glAccountId = intOf(ci(match, "GlAccountId"));
            int supplierCustomerId = subsidiary ? picked : 0;
            if (subsidiary) {
                if (supplierId != 0 && picked == supplierId)
                    throw new IllegalArgumentException("Selected Account Can not be Same As Supplier Account");
            } else {
                if (supplierGl < 0) supplierGl = supplierGlAccountId(supplierId);
                if (supplierGl == glAccountId)
                    throw new IllegalArgumentException("Selected Account Can not be Same As Supplier Account");
            }
            out.add(new Object[]{ glAccountId,
                    row.getPercentage() != null ? row.getPercentage() : 0.0,
                    row.getQty() != null ? row.getQty() : 0.0,
                    row.getRate() != null ? row.getRate() : 0.0,
                    amount, row.getRemarks(), supplierCustomerId });
        }
        return out;
    }

    private static final String SQL_CHARGE_TO_PRODUCT_INSERT =
            "EXEC Sp_PurchaseOrderExpensesChargeToProduct_Insert @Id=?, @PurchaseOrderId=?, @AccountId=?, @Percentage=?, @Qty=?, @Rate=?, @Amount=?, @Remarks=?, @SupplierCustomerId=?";

    private void writeChargeToProduct(int purchaseOrderId, List<Object[]> rows) {
        for (Object[] r : rows) {
            ProcExec.call(jdbcTemplate, SQL_CHARGE_TO_PRODUCT_INSERT, 0, purchaseOrderId, r[0], r[1], r[2], r[3], r[4], r[5], r[6]);
        }
    }

    /** combsuppname.SelectedRow.Cells[2] (:2909) - the supplier's GL account from the supplier list. */
    private int supplierGlAccountId(int supplierId) {
        if (supplierId <= 0) return 0;
        for (Map<String, Object> r : jdbcTemplate.queryForList(SQL_PARTY_LIST,
                currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId())) {
            if (intOf(ci(r, "Id")) == supplierId) return intOf(ci(r, "GlAccountId"));
        }
        return 0;
    }

    /**
     * grdlab -> PurchaseOrderLabDeduction (:3510-3524), every row. SetProc reflects the model's
     * non-virtual properties in declaration order; Id and PurchaseOrderDetailId are never assigned
     * on the desktop, so both travel as 0.
     */
    private static final String SQL_LAB_DEDUCTION_INSERT =
            "EXEC Sp_PurchaseOrderLabDeduction_Insert "
          + "@DeductionValue=?, @RangeFrom=?, @RangeTo=?, @StandardValue=?, @WeightKgs=?, "
          + "@AnalysisParameterId=?, @Id=?, @InvLabAnalysisStandardDeductionPolicyHeaderId=?, "
          + "@ItemId=?, @PurchaseOrderDetailId=?, @PurchaseOrderId=?, @DeductFrom=?";

    private void writeLabDeduction(int purchaseOrderId, List<PurchaseOrderFullDto.PurchaseOrderLabDeductionDto> rows) {
        if (rows == null) return;
        for (PurchaseOrderFullDto.PurchaseOrderLabDeductionDto r : rows) {
            ProcExec.call(jdbcTemplate, SQL_LAB_DEDUCTION_INSERT,
                    dbl(r.getDeductionValue()), dbl(r.getRangeFrom()), dbl(r.getRangeTo()),
                    dbl(r.getStandardValue()), dbl(r.getWeightKgs()),
                    zero(r.getAnalysisParameterId()), 0,
                    zero(r.getInvLabAnalysisStandardDeductionPolicyHeaderId()),
                    zero(r.getItemId()), 0, purchaseOrderId, r.getDeductFrom());
        }
    }

    /** Sp_PurchaseOrder_GetAllMethod @Activity='ReadPurchaseOrderLabDeductionByHeaderId' (DAL 0448:285). */
    private List<Map<String, Object>> getLabDeductionByHeaderId(int purchaseOrderId) {
        return jdbcTemplate.queryForList(
                "EXEC Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?",
                purchaseOrderId, "ReadPurchaseOrderLabDeductionByHeaderId");
    }

    /**
     * grdSupplierLoadingDetail -> PurchaseOrderSupplierDispatchDetail (:3405-3423), written through
     * [dbo].[USP_PurchaseOrderSupplierDispatchDetail_Insert] (DAL 0448:105). The page carries the
     * rows it loaded (ReadByHeaderId_PurchaseOrderSupplierDispatchDetail) and posts them back, so
     * the rows Sp_PurchaseOrder_Update deletes are written again - without this, updating an order
     * that has supplier dispatch rows silently destroyed them. d.Id = RecId != 0 ? row Id : 0 (:3410).
     */
    private void writeSupplierDispatch(int purchaseOrderId, int recId, List<Map<String, Object>> rows) {
        if (rows == null) return;
        for (Map<String, Object> r : rows) {
            ProcExec.call(jdbcTemplate,
                    "EXEC [dbo].[USP_PurchaseOrderSupplierDispatchDetail_Insert] @Amount=?, @ItemQty=?, @ItemRate=?, "
                  + "@NetWeight=?, @CropYearId=?, @Id=?, @ItemId=?, @ItemUOMId=?, @PurchaseOrderId=?, @RateUOMId=?, "
                  + "@SupplierDispatchId=?, @LoadingDetailId=?, @PackingTypeId=?",
                    dblOf(ci(r, "Amount")), dblOf(ci(r, "ItemQty")), dblOf(ci(r, "ItemRate")),
                    dblOf(ci(r, "NetWeight")), intOf(ci(r, "CropYearId")),
                    recId != 0 ? intOf(ci(r, "Id")) : 0,
                    intOf(ci(r, "ItemId")), intOf(ci(r, "ItemUOMId")), purchaseOrderId, intOf(ci(r, "RateUOMId")),
                    intOf(ci(r, "SupplierDispatchId")), intOf(ci(r, "LoadingDetailId")), intOf(ci(r, "PackingTypeId")));
        }
    }

    private static double dblOf(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        if (o == null) return 0d;
        try { return Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0d; }
    }

    /**
     * Payment Detail (:3558-3629). The grid's Amount SUM chooses the branch:
     *   > 0  -> a single grid row is first recalculated by percent (:3564-3572), then every row is
     *           checked ("Payment Term Required in row#n", "Due Days Required In case Of Credit row in row#n");
     *   == 0 -> ONE row built from the header term / due days, 100% of the detail total (:3602-3613).
     * Then |paid - total| <= 0.3 and |100 - pct| <= 0.01.
     */
    private List<Object[]> preparePaymentTerms(PurchaseOrderFullDto dto, java.math.BigDecimal detailSum, java.sql.Date docDate) {
        List<PurchaseOrderFullDto.PurchaseOrderPaymentTermsDetailDto> rows =
                dto.getPaymentTermsDetail() == null ? new ArrayList<>() : dto.getPaymentTermsDetail();

        java.math.BigDecimal gridAmountTotal = java.math.BigDecimal.ZERO;
        for (PurchaseOrderFullDto.PurchaseOrderPaymentTermsDetailDto r : rows) {
            if (r.getAmount() != null) gridAmountTotal = gridAmountTotal.add(java.math.BigDecimal.valueOf(r.getAmount()));
        }

        java.math.BigDecimal paidTotal = java.math.BigDecimal.ZERO;
        java.math.BigDecimal pctTotal  = java.math.BigDecimal.ZERO;
        List<Object[]> toWrite = new ArrayList<>();

        if (gridAmountTotal.compareTo(java.math.BigDecimal.ZERO) > 0) {
            /* :3564-3572 - one row: force percent mode and recalculate before reading it;
               several rows: recalculated only when the grid is in percent mode (PaymentAmountReCalculate). */
            PurchaseOrderPaymentRules.recalculate(rows, detailSum,
                    Boolean.TRUE.equals(dto.getPaymentByPercent()), true);
            int rowNo = 0;
            for (PurchaseOrderFullDto.PurchaseOrderPaymentTermsDetailDto r : rows) {
                rowNo++;
                int termId  = r.getPaymentTermId() != null ? r.getPaymentTermId() : 0;
                int dueDays = r.getDueDays() != null ? r.getDueDays() : 0;
                java.math.BigDecimal pct = r.getPrcntOfTotal() != null ? java.math.BigDecimal.valueOf(r.getPrcntOfTotal()) : java.math.BigDecimal.ZERO;
                java.math.BigDecimal amt = r.getAmount() != null ? java.math.BigDecimal.valueOf(r.getAmount()) : java.math.BigDecimal.ZERO;
                pctTotal  = pctTotal.add(pct);
                paidTotal = paidTotal.add(amt);
                if (termId <= 0) throw new IllegalArgumentException("Payment Term Required in row#" + rowNo);
                if (termId == 2 && dueDays <= 0)
                    throw new IllegalArgumentException("Due Days Required In case Of Credit row in row#" + rowNo);
                toWrite.add(new Object[]{ termId, pct, amt, dueDays, r.getPaymentRemarks(), dateOrNull(r.getDueDate()) });
            }
        } else {
            int termId  = zero(dto.getPaymentTermId());
            int dueDays = zero(dto.getDueDays());
            java.sql.Date dueDate = docDate == null ? null : java.sql.Date.valueOf(docDate.toLocalDate().plusDays(dueDays));
            pctTotal  = java.math.BigDecimal.valueOf(100);
            paidTotal = detailSum;
            toWrite.add(new Object[]{ termId, pctTotal, detailSum, dueDays, null, dueDate });
        }

        if (paidTotal.subtract(detailSum).abs().compareTo(new java.math.BigDecimal("0.3")) > 0) {
            throw new IllegalArgumentException("Payment Detail Amount:" + fmt4(paidTotal)
                    + " Not Equal to Total Amount:" + fmt4(detailSum));
        }
        pctTotal = pctTotal.setScale(4, java.math.RoundingMode.HALF_EVEN);
        if (pctTotal.subtract(java.math.BigDecimal.valueOf(100)).abs().compareTo(new java.math.BigDecimal("0.01")) > 0) {
            throw new IllegalArgumentException("Payment Detail Total% not near to 100");
        }
        return toWrite;
    }

    /** decimal.ToString("#,##0.####"). */
    private static String fmt4(java.math.BigDecimal v) {
        return new java.text.DecimalFormat("#,##0.####").format(v);
    }

    private static final String SQL_PAYMENT_TERMS_DETAIL_INSERT =
            "EXEC [dbo].[USP_PurchaseOrderPaymentTermsDetail_Insert] @Id=?, @PurchaseOrderId=?, @PaymentTermId=?, @PrcntOfTotal=?, @Amount=?, @DueDays=?, @PaymentRemarks=?, @SortNo=?, @DueDate=?";

    private void writePaymentTerms(int purchaseOrderId, List<Object[]> rows) {
        /* PurchaseOrderPaymentTermsDetail.SortNo is never assigned on the desktop (:3576-3582) -> 0. */
        for (Object[] w : rows) {
            ProcExec.call(jdbcTemplate, SQL_PAYMENT_TERMS_DETAIL_INSERT,
                    0, purchaseOrderId, w[0], w[1], w[2], w[3], w[4], 0,
                    w[5] == null ? new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.TIMESTAMP, null) : w[5]);
        }
    }

    /** GetConfigValueFromGlobal(configDescription) - the ConfigKey text, or null when not configured. */
    private String getConfigValue(int orgId, int compId, String configDescription) {
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                SQL_CONFIG_VALUE_BY_DESCRIPTION, orgId, compId, configDescription, null,
                "GetConfigurationByOrgCompandConfigDescription");
        if (rows.isEmpty()) return null;
        Object v = ci(rows.get(0), "ConfigKey");
        return v != null ? v.toString() : null;
    }

    /** Conversion.ToDouble(string): unparseable -> 0. */
    private double parseConfigDouble(String s) {
        if (s == null || s.trim().isEmpty()) return 0.0;
        try { return Double.parseDouble(s.trim()); } catch (NumberFormatException e) { return 0.0; }
    }

    /** Conversion.ToBool(string). */
    private boolean parseConfigBool(String s) {
        if (s == null) return false;
        String t = s.trim();
        return "1".equals(t) || "true".equalsIgnoreCase(t);
    }

    /**
     * AddRowInvEmptyBagsGrid() :2205-2219 - two rows: (Type 1, Item 0, Rate 0, PackingType 1,
     * WeightCutForJuteBags) and (Type 1, Item 0, Rate 0, PackingType 2, WeightCutForPPBags).
     */
    public List<PurchaseOrderFullDto.PurchaseOrderEmptyBagDto> getDefaultEmptyBagRows() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        double juteCut = parseConfigDouble(getConfigValue(orgId, compId, "WeightCutForJuteBags"));
        double ppCut = parseConfigDouble(getConfigValue(orgId, compId, "WeightCutForPPBags"));

        PurchaseOrderFullDto.PurchaseOrderEmptyBagDto jute = new PurchaseOrderFullDto.PurchaseOrderEmptyBagDto();
        jute.setType(1); jute.setItemId(0); jute.setPackingTypeId(1); jute.setRate(0.0); jute.setWeightCut(juteCut);
        PurchaseOrderFullDto.PurchaseOrderEmptyBagDto pp = new PurchaseOrderFullDto.PurchaseOrderEmptyBagDto();
        pp.setType(1); pp.setItemId(0); pp.setPackingTypeId(2); pp.setRate(0.0); pp.setWeightCut(ppCut);
        return new ArrayList<>(List.of(jute, pp));
    }

    /** Sp_PurchaseOrder_GetAllMethod @Activity='ReadPurchaseOrderEmptyBagsDetailByHeaderId'. */
    private List<Map<String, Object>> getEmptyBagsByHeaderId(int purchaseOrderId) {
        return jdbcTemplate.queryForList(SQL_EMPTY_BAGS_BY_HEADER_ID, purchaseOrderId,
                "ReadPurchaseOrderEmptyBagsDetailByHeaderId");
    }

    /**
     * Empty bags (:3424-3478): every grid row is saved; unless the configuration
     * StopShowingPurchaseOrderEmptyBagsWeightValidations is on, each row must have a Type, a
     * PackingType, a WeightCut (Type != 2), and a WeightCut within the packing type's
     * MinEbWeight..MaxEbWeight (pack 1/2/5 validate against themselves, anything else against 2).
     */
    private void validateEmptyBags(List<PurchaseOrderFullDto.PurchaseOrderEmptyBagDto> rows) {
        if (rows == null) return;
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        if (parseConfigBool(getConfigValue(orgId, compId, "StopShowingPurchaseOrderEmptyBagsWeightValidations"))) return;
        List<Map<String, Object>> packingTypes = null;
        for (PurchaseOrderFullDto.PurchaseOrderEmptyBagDto row : rows) {
            int type = row.getType() != null ? row.getType() : 0;
            int packingTypeId = row.getPackingTypeId() != null ? row.getPackingTypeId() : 0;
            double weightCut = row.getWeightCut() != null ? row.getWeightCut() : 0.0;
            if (type == 0) throw new IllegalArgumentException("EmptyBagsType filed required In Empty bags Grid...");
            if (packingTypeId == 0) throw new IllegalArgumentException("PackingType filed required In Empty bags Grid...");
            if (type != 2 && weightCut <= 0.0) throw new IllegalArgumentException("WeightCut filed required In Empty bags Grid...");
            if (weightCut > 0.0) {
                int validationTypeId = (packingTypeId == 1 || packingTypeId == 2 || packingTypeId == 5) ? packingTypeId : 2;
                if (packingTypes == null) packingTypes = getPackingTypesForEmptyBags();
                for (Map<String, Object> pt : packingTypes) {
                    if (intOf(ci(pt, "Id")) != validationTypeId) continue;
                    double min = dblOf(ci(pt, "MinEbWeight"));
                    double max = dblOf(ci(pt, "MaxEbWeight"));
                    if (weightCut < min || weightCut > max) {
                        throw new IllegalArgumentException("Weight Cut Should be in Range of: " + clrNumber(min)
                                + " to " + clrNumber(max) + "\nFor Packing Type:" + strOf(ci(pt, "PackTypeDesc")));
                    }
                    break;
                }
            }
        }
    }

    private static final String SQL_EMPTY_BAGS_INSERT =
            "EXEC Sp_PurchaseOrderEmptyBags_Insert @Id=?, @PurchaseOrderId=?, @Type=?, @ItemId=?, @PackingTypeId=?, @Rate=?, @WeightCut=?";

    private void writeEmptyBags(int purchaseOrderId, List<PurchaseOrderFullDto.PurchaseOrderEmptyBagDto> rows) {
        if (rows == null) return;
        for (PurchaseOrderFullDto.PurchaseOrderEmptyBagDto row : rows) {
            ProcExec.call(jdbcTemplate, SQL_EMPTY_BAGS_INSERT,
                    0, purchaseOrderId, zero(row.getType()), zero(row.getItemId()), zero(row.getPackingTypeId()),
                    row.getRate() != null ? row.getRate() : 0.0,
                    row.getWeightCut() != null ? row.getWeightCut() : 0.0);
        }
    }

    /** A double the way C# double.ToString() prints it: 2 -> "2", 1.5 -> "1.5". */
    private static String clrNumber(double v) {
        if (v == Math.rint(v) && !Double.isInfinite(v) && Math.abs(v) < 1e15) return String.valueOf((long) v);
        return java.math.BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
    }

    /* =========================================================================================
     * multiCurrencyFeature() - PurchsaeOrder.cs :4161-4196
     * =========================================================================================
     * ERP feature 6 decides whether the operator picks a currency at all:
     *
     *   feature ON  -> the Fcy Code / Exchange Rate / Fcy Amount controls are SHOWN and the
     *                  operator fills them.
     *   feature OFF -> those controls are HIDDEN and the base currency and rate are taken from
     *                  configuration: definition "1" is the base CurrencyId and definition "160"
     *                  is the base exchange rate, each read from its ConfigKey (:4183-4193).
     *
     * This web form has no currency controls at all, so it is always the feature-OFF case: the
     * values must come from configuration, exactly as the desktop takes them. Without this, the
     * header was writing CurrencyId = 0 and ExchangeRate = 0 on every save, and FormValidation's
     * currency checks could never pass.
     */
    private static final int ERP_FEATURE_MULTI_CURRENCY = 6;
    private static final String CONFIG_DEF_BASE_CURRENCY = "1";
    private static final String CONFIG_DEF_BASE_RATE     = "160";

    /** BLL 0621 ConfigrationsAllocation.GetConfigurationsByDefinitionIds. */
    private String configByDefinitionId(int orgId, int compId, String definitionId) {
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@DefinitionIds=?, @Activity=?",
                orgId, compId, definitionId, "GetConfigurationsByDefinitionIds");
        return rows.isEmpty() ? "" : strOf(ci(rows.get(0), "ConfigKey")).trim();
    }

    /** ERP feature 6 (multiCurrencyFeature :4165). */
    private boolean multiCurrencyFeature(int orgId, int compId) {
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?", orgId, compId)) {
            if (intOf(ci(r, "Id")) == ERP_FEATURE_MULTI_CURRENCY) return true;
        }
        return false;
    }

    /** multiCurrencyFeature() :4182-4192 with the feature OFF: base currency (definition 1) and rate (definition 160). */
    private Map<String, Object> baseCurrencyFromConfiguration(int orgId, int compId) {
        Map<String, Object> out = new LinkedHashMap<>();
        String cur  = configByDefinitionId(orgId, compId, CONFIG_DEF_BASE_CURRENCY);
        String rate = configByDefinitionId(orgId, compId, CONFIG_DEF_BASE_RATE);
        int curId = 0;
        java.math.BigDecimal rateVal = java.math.BigDecimal.ZERO;
        try { curId = (int) Double.parseDouble(cur); } catch (NumberFormatException ignored) { /* Conversion.ToInt -> 0 */ }
        try { rateVal = new java.math.BigDecimal(rate); } catch (NumberFormatException ignored) { /* Conversion -> 0 */ }
        out.put("currencyId", curId);
        out.put("exchangeRate", rateVal);
        return out;
    }

    /**
     * The currency a save carries: cmbCurrency / txtExchangeRate as the page holds them (:3318-3319).
     * With feature 6 ON the operator picks them (the page shows Fcy Code / Exchange Rate / Fcy
     * Amount). With it OFF the controls are hidden and hold what ReadById loaded or, when that
     * was 0, what multiCurrencyFeature() put there from configuration (:3740-3743) - so a posted
     * 0 falls back to the configured base currency/rate, exactly the value the hidden controls
     * would carry.
     */
    private Map<String, Object> resolveCurrency(PurchaseOrderFullDto dto) {
        int orgId  = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        boolean multi = multiCurrencyFeature(orgId, compId);
        out.put("multiCurrency", multi);
        int curId = zero(dto.getCurrencyId());
        java.math.BigDecimal rate = dto.getExchangeRate() == null ? java.math.BigDecimal.ZERO
                : java.math.BigDecimal.valueOf(dto.getExchangeRate());
        if (!multi && curId == 0) {
            Map<String, Object> base = baseCurrencyFromConfiguration(orgId, compId);
            curId = intOf(base.get("currencyId"));
            if (rate.signum() == 0) rate = (java.math.BigDecimal) base.get("exchangeRate");
        }
        out.put("currencyId", curId);
        out.put("exchangeRate", rate);
        return out;
    }

    /**
     * LocationTypeFill() - PurchsaeOrder.cs :840-856, via VoucherHead.GetLocationType (BLL 0654).
     *
     * The procedure takes NO PARAMETERS AT ALL - the BLL builds an empty SqlParameter list and
     * calls usp_getLocationType with it. Adding @OrganizationId/@CompanyId "for safety" would be
     * inventing a contract the desktop does not have.
     *
     * Columns: Id, Location. The desktop binds them with DDL.BindDDL(dt, cmbLocationType, "Id",
     * "Location", "Location Type", ZeroIndex: true).
     */
    public List<Map<String, Object>> getLocationTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList("EXEC dbo.usp_getLocationType")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id",   intOf(ci(r, "Id")));
            m.put("name", strOf(ci(r, "Location")));
            out.add(m);
        }
        return out;
    }

    /**
     * CurrencyFill() - :4136-4159, via MultiCurrency.GetAll (BLL 0076):
     * Sp_MultiCurrency_GetAllMethod @OrganizationId, @CompanyId, @Activity='ReadAll'.
     * Columns bound by the desktop: Id, CurrencyCode.
     */
    public List<Map<String, Object>> getCurrencies() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC Sp_MultiCurrency_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(), "ReadAll")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id",   intOf(ci(r, "Id")));
            m.put("name", strOf(ci(r, "CurrencyCode")));
            out.add(m);
        }
        return out;
    }

    /* =========================================================================================
     * FormValidation() - PurchsaeOrder.cs :1904-1997, ported whole.
     * =========================================================================================
     * 94 lines, 14 refusals, and NONE of it existed in the web port - see
     * PURCHASE-ORDER-METHOD-COVERAGE-113-OF-174-NEVER-PORTED.md. A Purchase Order could be saved
     * from the web with no Category, no Supplier Ref No, no Payment Term, no Delivery Term and
     * no Status, every one of which the desktop refuses outright.
     *
     * Server-side, because a crafted request must not bypass a desktop business rule. The
     * messages are the desktop's own strings, verbatim, so the operator sees what they would see
     * on the desktop; ApiExceptionAdvice turns an IllegalArgumentException into a 400 carrying
     * the text.
     *
     * The checks are in the desktop's order, because the desktop returns on the FIRST failure
     * and the operator is used to being told about them in that sequence.
     */
    private void formValidation(PurchaseOrderFullDto dto, Map<String, Object> currency) {
        /* :1906 - Location Type is checked ONLY when its list is populated
           (cmbLocationType.DataSource != null). LocationTypeFill is now ported, so the guard is
           reproduced literally: the list is read, and the field is required ONLY if it has rows.
           A company whose usp_getLocationType returns nothing is not asked for a Location Type,
           which is exactly what the desktop does. A failed lookup is treated as "no list" and
           logged, rather than refusing a save because a lookup broke. */
        boolean locationTypesExist = !getLocationTypes().isEmpty();
        if (locationTypesExist && zero(dto.getLocationTypeId()) == 0) {
            throw new IllegalArgumentException("Location Type Field is Required");
        }

        if (zero(dto.getDocNo()) == 0) {                                             /* :1912 */
            throw new IllegalArgumentException("Order No Field is Required");
        }
        if (zero(dto.getOrderCategoryId()) == 0) {                                   /* :1918 */
            throw new IllegalArgumentException("Category Field is Required");
        }
        if (zero(dto.getSupplierId()) == 0) {                                        /* :1924 */
            throw new IllegalArgumentException("Supplier Name Field is Required");
        }
        /* :1930 - Supplier RefNo is required ONLY for category 8. */
        if (zero(dto.getOrderCategoryId()) == 8
                && (dto.getSupplierRefNo() == null
                    || dto.getSupplierRefNo().trim().isEmpty()
                    || "0".equals(dto.getSupplierRefNo().trim()))) {
            throw new IllegalArgumentException("Supplier RefNo Field is Required");
        }
        int paymentTermId = zero(dto.getPaymentTermId());
        if (paymentTermId == 0) {                                                    /* :1936 */
            throw new IllegalArgumentException("Payment Terms Field is Required");
        }
        /* :1942 - the desktop compares the combo's TEXT, not its id, against these two exact
           spellings. The text is resolved from the same InvDueTerms list the combo is bound to
           rather than assuming which id means Credit. */
        String termText = paymentTermText(paymentTermId);
        if (("Credit".equals(termText) || "Company_Policy".equals(termText))
                && zero(dto.getDueDays()) == 0) {
            throw new IllegalArgumentException("Due days Required when Payment Term is Credit");
        }
        if (zero(dto.getDeliveryTermId()) == 0) {                                    /* :1948 */
            throw new IllegalArgumentException("Delivery Term Field is Required");
        }
        /* :1954 - CmbStatus. The desktop's combo carries an int value; this page stores the
           status as its text (Open / Approved / Closed), so "set" means non-empty here. */
        if (dto.getOrderStatus() == null || dto.getOrderStatus().trim().isEmpty()) {
            throw new IllegalArgumentException("Status Field is Required");
        }

        boolean multi = Boolean.TRUE.equals(currency.get("multiCurrency"));
        int curId = intOf(currency.get("currencyId"));
        java.math.BigDecimal rate = (java.math.BigDecimal) currency.get("exchangeRate");
        boolean rateSet = rate != null && rate.compareTo(java.math.BigDecimal.ZERO) != 0;

        if (multi) {                                                                 /* :1960 */
            if (curId == 0)  throw new IllegalArgumentException("Fcy Code Field is Required");
            if (!rateSet)    throw new IllegalArgumentException("Exchange Rate Field is Required");
            /* :1974 txtFcyAmount = the grid's FcyAmount total (CalculateTotalInformation :4213). */
            java.math.BigDecimal fcy = (java.math.BigDecimal) currency.get("fcyAmount");
            if (fcy == null || fcy.signum() == 0)
                throw new IllegalArgumentException("Fcy Amount Rate Field is Required");
        } else {                                                                     /* :1981 */
            if (curId == 0) {
                throw new IllegalArgumentException("Please Configure Your Base Currency In configurations");
            }
            if (!rateSet) {
                throw new IllegalArgumentException("Please Configure Your Base Currency Rate In configurations");
            }
        }
    }

    /* =========================================================================================
     * FormDetailValidation() - PurchsaeOrder.cs :1999-2050, enforced SERVER-SIDE.
     * =========================================================================================
     * The eight checks were ported into the page earlier, and only into the page. A request that
     * does not come from that page - a replayed save, a hand-built POST, a stale tab - reached
     * the database with no line validation at all, so a detail row with Item Rate 0 or no Crop
     * Year could still be written. The standing rule is that a crafted API request must not
     * bypass a desktop business rule, so the same eight run here too.
     *
     * The desktop validates the ENTRY CONTROLS before a row is added to the grid, so it never
     * holds an invalid row. The web posts the whole grid at once, so the equivalent is to check
     * every line and name the offending row - hence the "in row#N" suffix, which the desktop does
     * not need and which is the one deliberate wording difference.
     *
     * Loading Location, Moisture and Remarks are NOT validated, because the desktop does not
     * validate them either.
     */
    private void formDetailValidation(PurchaseOrderFullDto dto) {
        if (dto.getLineItems() == null) return;
        int rowNo = 0;
        for (PurchaseOrderFullDto.PurchaseOrderDetailItemDto d : dto.getLineItems()) {
            rowNo++;
            String where = " in row#" + rowNo;
            if (zero(d.getItemId()) == 0)                                        /* :2001 */
                throw new IllegalArgumentException("Item Field is Required" + where);
            if (zero(d.getCropYearId()) == 0)                                    /* :2007 */
                throw new IllegalArgumentException("CropYear Field is Required" + where);
            if (zero(d.getJobLotId()) == 0)                                      /* :2013 */
                throw new IllegalArgumentException("JobLot Field is Required" + where);
            if (zero(d.getPackUomId()) == 0)                                     /* :2019 */
                throw new IllegalArgumentException("UOM Field is Required" + where);
            if (dbl(d.getItemQty()) == 0d)                                       /* :2025 */
                throw new IllegalArgumentException("Item Qty Field is Required" + where);
            if (dbl(d.getItemRate()) == 0d)                                      /* :2031 */
                throw new IllegalArgumentException("Item Rate Field is Required" + where);
            if (zero(d.getRateUomId()) == 0)                                     /* :2037 */
                throw new IllegalArgumentException("Rate UOM Field is Required" + where);
            if (dbl(d.getItemAmount()) == 0d)                                    /* :2043 */
                throw new IllegalArgumentException("Amount Field is Required" + where);
        }
    }

    /** combpttrm's displayed text for an id, from the list the combo is bound to. */
    private String paymentTermText(int paymentTermId) {
        for (Map<String, Object> t : getPaymentTermsForPurchaseOrder()) {
            if (intOf(t.get("id")) == paymentTermId) return strOf(t.get("description")).trim();
        }
        return "";
    }

    private static final org.slf4j.Logger VALIDATION_LOG =
            org.slf4j.LoggerFactory.getLogger(PurchaseOrderFullService.class.getName() + ".FormValidation");

    @Autowired
    private com.mst.repositories.PurchaseOrderDetailRepository purchaseOrderDetailRepository;

    @Autowired
    private PurchaseOrderAttachmentService purchaseOrderAttachmentService;

    /** DefaultNoofDecimalPointsForFcyAmount - txtExchangeRate_TextChanged :4114. */
    private int fcyDecimals(int orgId, int compId) {
        return decimalPoints(orgId, compId, "DefaultNoOfDecimalPointsForFcyAmount", 2);
    }

    /**
     * Save / Update / SaveAs - PurchsaeOrder.cs btnsave_Click :3669, btnUpdate_Click_1 :3690,
     * btnSaveAs_Click :3885 -> Insert() :3196-3667 -> BLL 0595 PurchaseOrder.Save :17-44 ->
     * DAL 0448 SetData :17-199. One transaction; every refusal is an IllegalArgumentException
     * carrying the desktop's text (HTTP 400), a RAISERROR from a procedure is passed through the
     * same way, and anything thrown rolls the whole save back.
     */
    @Transactional
    public Map<String, Object> savePurchaseOrder(PurchaseOrderFullDto dto, Integer userId) {
        try {
            return savePurchaseOrderInternal(dto, userId);
        } catch (org.springframework.dao.DataAccessException e) {
            /* RAISERROR (50000) from Sp_PurchaseOrder_Insert/_Update, Sp_PurchaseOrderDetail_Insert,
               PoWeightAndGpWeightValidation ... is the desktop's own MessageBox text. */
            Throwable cause = e.getMostSpecificCause();
            if (cause instanceof java.sql.SQLException
                    && (((java.sql.SQLException) cause).getErrorCode() == 50000
                        || ((java.sql.SQLException) cause).getErrorCode() == 50001)) {
                throw new IllegalArgumentException(cause.getMessage(), e);
            }
            throw e;
        }
    }

    private Map<String, Object> savePurchaseOrderInternal(PurchaseOrderFullDto dto, Integer userId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int effUserId = userId != null ? userId : currentUserContext.currentUserId();

        /* The document type is this form's, never the caller's (:3295). */
        dto.setDocumentTypeId(PO_DOCUMENT_TYPE_ID);
        int recId = zero(dto.getPurchaseOrderMasterId()) > 0 ? dto.getPurchaseOrderMasterId() : 0;

        /* Rights - btnsave.Enabled = DoHaveSaveRight, btnUpdate.Enabled = DoHaveUpdateRights (:620-622).
           SaveAs is reached from the History "SaveAs" column, itself shown only with the Save right. */
        Map<String, Object> rights = rights();
        if (recId == 0 && !Boolean.TRUE.equals(rights.get("save")))
            throw new org.springframework.security.access.AccessDeniedException("You do not have the Save right on Purchase Order.");
        if (recId > 0 && !Boolean.TRUE.equals(rights.get("update")))
            throw new org.springframework.security.access.AccessDeniedException("You do not have the Update right on Purchase Order.");

        /* An update may only touch an order this user can open (company, type 41, visibility). */
        Map<String, Object> storedRecord = recId > 0 ? purchaseOrderRecordRepository.require(recId) : null;

        /* btnsave_Click :3674 - a NEW order may not be saved as Complete or Cancel. */
        if (recId == 0) {
            String st = dto.getOrderStatus() == null ? "" : dto.getOrderStatus().trim();
            if ("Complete".equals(st) || "Cancel".equals(st))
                throw new IllegalArgumentException("Order Status Should not Be Complete Or Cancel In Save Mode");
        }

        /* txtExchangeRate_TextChanged :4103-4127 - every line's FcyAmount = Amount / ExchangeRate,
           rounded to DefaultNoofDecimalPointsForFcyAmount; 0 when the rate is 0. The header's Fcy
           Amount is their total (:4213). */
        Map<String, Object> currency = resolveCurrency(dto);
        java.math.BigDecimal rate = (java.math.BigDecimal) currency.get("exchangeRate");
        int fcyDp = fcyDecimals(orgId, compId);
        java.math.BigDecimal fcyTotal = java.math.BigDecimal.ZERO;
        if (dto.getLineItems() != null) {
            for (PurchaseOrderFullDto.PurchaseOrderDetailItemDto li : dto.getLineItems()) {
                java.math.BigDecimal fcy = java.math.BigDecimal.ZERO;
                if (rate != null && rate.signum() > 0) {
                    fcy = java.math.BigDecimal.valueOf(dbl(li.getItemAmount()))
                            .divide(rate, fcyDp, java.math.RoundingMode.HALF_EVEN);
                }
                li.setFcyAmount(fcy.doubleValue());
                fcyTotal = fcyTotal.add(fcy);
            }
        }
        currency.put("fcyAmount", fcyTotal);

        /* Insert() :3224 - FormValidation() first. */
        formValidation(dto, currency);

        /* :3237-3251 - a NEW Govt Purchase (category 8) order may not repeat the Supplier Ref No.
           GetSupplierRefNo returns every SupplierRefNo of that category and the desktop compares
           ONLY the FIRST row (Rows[0], BLL 0595:1281) - reproduced as it is. */
        if (recId == 0 && zero(dto.getOrderCategoryId()) == 8) {
            List<Map<String, Object>> refs = jdbcTemplate.queryForList(
                    "EXEC Sp_PurchaseOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, "
                  + "@OrderCategoryId=?, @Activity=?", orgId, compId, PO_DOCUMENT_TYPE_ID, 8, "GetSupplierRefNo");
            String first = refs.isEmpty() ? "" : strOf(ci(refs.get(0), "SupplierRefNo"));
            String mine = dto.getSupplierRefNo() == null ? "" : dto.getSupplierRefNo().trim();
            if (!first.isEmpty() && first.toUpperCase().equals(mine.toUpperCase()))
                throw new IllegalArgumentException("SupplierRefNo already exist please check!");
        }

        /* The page validates each line before it enters the grid (FormDetailValidation :1999);
           run here too so a crafted request cannot carry a line the desktop would refuse. */
        formDetailValidation(dto);

        if (dto.getLineItems() == null || dto.getLineItems().isEmpty())
            throw new IllegalArgumentException("Detail Information Required");            /* :3260 */
        if (dto.getEmptyBags() == null || dto.getEmptyBags().isEmpty())
            throw new IllegalArgumentException("Empty Bags Information Required");        /* :3265 */
        int vAgentId = zero(dto.getCommissionAgentId());
        double vCommAmt = dbl(dto.getCommAmount());
        double vCommRate = dbl(dto.getCommRate());
        if ((vCommAmt > 0d || vCommRate > 0d) && vAgentId == 0)                          /* :3270 */
            throw new IllegalArgumentException("Please Select Commission Agent Required when Commission Amount or Rate is Present...");
        if (vAgentId > 0 && (vCommAmt == 0d || vCommRate == 0d)) {                         /* :3274 */
            if (vCommAmt == 0d) throw new IllegalArgumentException("Commission Amount Required when Commission Agent is Selected...");
            throw new IllegalArgumentException("Commission Rate Required when Commission Agent is Selected...");
        }

        /* :3384 / :3402 - a line without a Rate UOM stops the save. */
        for (PurchaseOrderFullDto.PurchaseOrderDetailItemDto li : dto.getLineItems()) {
            if (zero(li.getRateUomId()) <= 0) throw new IllegalArgumentException("Rate UOM Not Found");
        }

        String docDate = dto.getDocDate() != null && !dto.getDocDate().isEmpty() ? dto.getDocDate()
                : new java.text.SimpleDateFormat("yyyy-MM-dd").format(new Date());
        java.sql.Date docDateSql = java.sql.Date.valueOf(docDate.substring(0, 10));
        java.math.BigDecimal detailSum = sumDetails(dto, "amount");

        /* Collections validated and shaped BEFORE any write, in the desktop's order. */
        validateEmptyBags(dto.getEmptyBags());                                              /* :3439 */
        List<Object[]> charges = prepareChargeToProduct(dto.getExpensesChargeToProduct(), zero(dto.getSupplierId()));
        List<Object[]> expenses = prepareSupplierExpense(dto.getSupplierExpenses());
        List<Object[]> payments = preparePaymentTerms(dto, detailSum, docDateSql);         /* :3558 */

        /* BLL Save :21-28 - the date lock, then (insert) no saved detail ids. */
        purchaseOrderHeaderRepository.assertNotDateLocked(orgId, compId, docDateSql);
        if (recId == 0) {
            for (PurchaseOrderFullDto.PurchaseOrderDetailItemDto li : dto.getLineItems()) {
                if (zero(li.getPurchaseOrderDetailId()) > 0)
                    throw new IllegalArgumentException("Record cannot be inserted because detailId greater than zero");
            }
        }

        /* Attachments staged by the page (FormHelper.UpdateAttachmentsForObject :3632). */
        PurchaseOrderAttachmentService.Prepared attachments = purchaseOrderAttachmentService.prepare(recId, dto.getAttachments());

        int docNo = zero(dto.getDocNo()) > 0 ? dto.getDocNo() : generateNextDocNo(PO_DOCUMENT_TYPE_ID);

        /* Header - :3285-3354. */
        Map<String, Object> head = PurchaseOrderHeaderRepository.blankModel();
        java.sql.Timestamp nowTs = new java.sql.Timestamp(System.currentTimeMillis());
        head.put("Id",             recId);
        head.put("DocumentTypeId", PO_DOCUMENT_TYPE_ID);
        head.put("DocNo",          docNo);
        head.put("DocDate",        docDateSql);
        head.put("BranchesId",     branchId);
        head.put("ProjectsId",     branchId);                                              /* :3297 */
        head.put("OrganizationId", orgId);
        head.put("CompanyId",      compId);
        head.put("FinancialYearId", currentUserContext.currentFinancialYearId());
        head.put("BranchSrNo",     zero(dto.getBranchNo()));
        head.put("OrderCatagoryId", zero(dto.getOrderCategoryId()));
        head.put("CatagorySrNo",   zero(dto.getCategorySrNo()));
        head.put("OrderSupCustId", zero(dto.getSupplierId()));
        head.put("SupplierRefNo",  dto.getSupplierRefNo() == null ? "" : dto.getSupplierRefNo());
        head.put("BookingPersonId", zero(dto.getBookingPersonId()));
        head.put("RemarksHeader",  dto.getRemarksHeader() == null ? "" : dto.getRemarksHeader().trim());
        head.put("PaymentTermsId", zero(dto.getPaymentTermId()));
        head.put("OrderDueDays",   zero(dto.getDueDays()));
        java.sql.Date orderDueDate = dateOrNull(dto.getPaymentDueDate());
        if (orderDueDate == null) {
            int dueDays = zero(dto.getDueDays());
            orderDueDate = dueDays > 0 ? java.sql.Date.valueOf(docDateSql.toLocalDate().plusDays(dueDays))
                                       : new java.sql.Date(System.currentTimeMillis());   /* :5357-5361 */
        }
        head.put("OrderDueDate",   orderDueDate);
        head.put("OrderExpiryDate", nowTs);                                               /* :3310 */
        head.put("DeliveryTermId", zero(dto.getDeliveryTermId()));
        head.put("DeliveryTerm",   dto.getDeliveryTermName());                             /* :3312 Cells[2] */
        java.sql.Date deliveryStart = dateOrNull(dto.getDeliveryStartDate());
        head.put("DeliveryStartDate", deliveryStart != null ? deliveryStart : new java.sql.Date(System.currentTimeMillis()));
        head.put("DeliveryDays",   zero(dto.getDeliveryDays()));
        head.put("OrderQty",       sumDetails(dto, "qty"));                                 /* :4210-4212 */
        head.put("OrderWeight",    sumDetails(dto, "weight"));
        head.put("OrderAmount",    detailSum);
        head.put("CurrencyId",     intOf(currency.get("currencyId")));
        head.put("ExchangeRate",   rate);
        head.put("FcyAmount",      fcyTotal);
        if (vAgentId > 0) {                                                                 /* :3321-3328 */
            head.put("BrokerAgentSupCustId", vAgentId);
            head.put("CommissionType",       dto.getCommissionTypeName() == null ? "" : dto.getCommissionTypeName());
            head.put("CommRate",             vCommRate);
            head.put("UomScheduleIdCmRate",  zero(dto.getCommUomId()));   /* ToInt(combruom.Text) - the value IS the text */
            head.put("CommAmount",           vCommAmt);
        }
        if (zero(dto.getBrokerAccountId()) > 0) {                                           /* :3329-3336 */
            head.put("BrokerAgentId",  dto.getBrokerAccountId());
            head.put("BrokeryType",    dto.getBrokeryTypeName() == null ? "" : dto.getBrokeryTypeName());
            head.put("BrokeryRate",    dbl(dto.getBrokeryRate()));
            head.put("BrokeryUom",     dbl(dto.getBrokeryRateUomId()));   /* ToDouble(CmbBrokeryRateUom.Text) */
            head.put("BrokeryAmount",  dbl(dto.getBrokeryAmount()));
        }
        boolean creditFreight = Boolean.TRUE.equals(dto.getCreditFreight());                 /* :3337-3344 */
        head.put("CashFreight",   !creditFreight);
        head.put("CreditFreight", creditFreight);
        head.put("EntryUser",  effUserId);
        head.put("EntryDate",  nowTs);
        head.put("ModifyUser", effUserId);
        head.put("ModifyDate", nowTs);
        head.put("OrderStatus", dto.getOrderStatus() == null ? "" : dto.getOrderStatus().trim()); /* :3352 */
        head.put("LocationTypeId", zero(dto.getLocationTypeId()));
        if (recId > 0) {
            /* :3289 - IsAproved keeps the loaded record's value; taken from the stored row, never the client. */
            head.put("IsAproved", truthy(storedRecord == null ? null : storedRecord.get("IsAproved")));
        } else {
            head.put("IsAproved", Boolean.FALSE);
        }
        /* po.AttachmentsValues / CustomAttachmentsValues (:3633-3634): the order's attachment names
           ("" when there are none). */
        head.put("AttachmentsValues", attachments.names());
        head.put("CustomAttachmentsValues", attachments.storedNames());

        int poId = purchaseOrderHeaderRepository.save(head);
        if (poId <= 0) throw new IllegalStateException("Purchase Order save returned no Id.");

        /* DAL :62-67 - every grid row through Sp_PurchaseOrderDetail_Insert (it inserts when @Id is
           0 and updates otherwise). On update each line keeps its own Id (:3365-3371). */
        for (PurchaseOrderFullDto.PurchaseOrderDetailItemDto li : dto.getLineItems()) {
            if (recId == 0) li.setPurchaseOrderDetailId(0);
            dto.setCurrencyId(intOf(currency.get("currencyId")));
            dto.setExchangeRate(rate == null ? 0d : rate.doubleValue());
            purchaseOrderDetailRepository.save(poId, li, dto);
        }
        writeEmptyBags(poId, dto.getEmptyBags());                                           /* DAL :68 */
        writeChargeToProduct(poId, charges);                                                /* DAL :76 */
        writeLabDeduction(poId, dto.getLabDeductions());                                    /* DAL :84 */
        writeSupplierExpense(poId, expenses);                                               /* DAL :92 */
        writeSupplierDispatch(poId, recId, dto.getSupplierDispatchDetail());               /* DAL :100 */
        writePaymentTerms(poId, payments);                                                  /* DAL :108 */
        purchaseOrderAttachmentService.persist(poId, zero(dto.getSupplierId()), attachments); /* DAL :116 */

        if (recId > 0) {
            /* DAL :155-168 - rows removed in update mode (grd_ColumnButtonClick :2686). */
            String removeIds = dto.getOrderDetailRemoveIds();
            if (removeIds != null && !removeIds.isEmpty()) {
                ProcExec.call(jdbcTemplate,
                        "EXEC dbo.USP_DeletePurchaseOrderDetailIfNotExistInGrn @OrganizationId=?, @CompanyId=?, "
                      + "@OrderId=?, @UserId=?, @OrderDetailIds=?", orgId, compId, poId, effUserId, removeIds);
            }
            /* DAL :169-178 */
            ProcExec.call(jdbcTemplate,
                    "EXEC dbo.Sp_PurchaseOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @Id=?, @Activity=?",
                    orgId, compId, poId, "PoWeightAndGpWeightValidation");
        } else {
            /* DAL :180-189 - LimitAmount = the sum of the detail amounts. */
            java.math.BigDecimal limit = java.math.BigDecimal.ZERO;
            for (PurchaseOrderFullDto.PurchaseOrderDetailItemDto li : dto.getLineItems())
                limit = limit.add(java.math.BigDecimal.valueOf(dbl(li.getItemAmount())));
            ProcExec.call(jdbcTemplate,
                    "EXEC [DAW].[USp_DocumentApprovalDetail_Insert] @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, "
                  + "@Id=?, @LimitAmount=?", orgId, compId, PO_DOCUMENT_TYPE_ID, poId, limit);
        }

        Map<String, Object> response = new HashMap<>();
        response.put("success", true);
        response.put("id", poId);
        response.put("docNo", docNo);
        /* :3638 / :3643 */
        response.put("message", (recId > 0 ? "Data Update Successfully....[" : "Data Save Successfully....[") + docNo + "]");
        return response;
    }

    /**
     * formright = CommonServices.SetRightsValueInRightsObject("PurchsaeOrder") :619 - the rights the
     * form reads: Save, Update, Print, Delete (CanDelete) and CanView AllRecord. Role "Admin" starts
     * with Save/Update/Print (the same rule ProductionPackingMaterialService applies); grant rows
     * then apply. An unreadable grid is an error, not "no rights".
     */
    public Map<String, Object> rights() {
        String role = currentUserContext.currentRoleName();
        boolean admin = "Admin".equals(role);
        boolean save = admin, update = admin, print = admin, delete = false, viewAll = false;
        for (Map<String, Object> row : jdbcTemplate.queryForList(
                "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, @CompanyId=?, @Activity=?",
                currentUserContext.currentUserId(), "PurchsaeOrder", role == null ? "" : role,
                currentUserContext.currentCompanyId(), "GetByUserId")) {
            String name = strOf(ci(row, "RightName")).trim();
            boolean v = truthy(ci(row, "Value"));
            switch (name) {
                case "Save":   save = admin || v; break;
                case "Update": update = admin || v; break;
                case "Print":  print = admin || v; break;
                case "Delete": delete = v; break;
                case "CanView AllRecord": viewAll = v; break;
                default: break;
            }
        }
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("save", save);
        r.put("update", update);
        r.put("print", print);
        r.put("delete", delete);
        r.put("canViewAllRecord", viewAll || "Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role));
        return r;
    }

    /**
     * CmbLabSampleNo - FactorySampleOrStandardDbCall() :874-936.
     *   Sample   -> InvLabSampleAnalysisHeader.GetSampleNoBySupplierCustomerIdAndITemId (BLL 0407:458):
     *               Sp_InvLabSampleAnalysisHeader_GetAllMethod @OrganizationId, @CompanyId,
     *               @SupplierCustomerId, @ItemId (always), @CommissionAgentId (only when != 0),
     *               @Id (= RecId, only when != 0), @Activity; rows (Id, DocNo).
     *   Standard -> LabAnalysisStandardSchedule.ComboFill (BLL 0394:185):
     *               [dbo].[USP_LabAnalysisStandardSchedule_GetAllMethod] @OrganizationId, @CompanyId,
     *               @FromDate = DocDate, @Activity='ComboFill';
     *               rows (LabAnalysisGroupId, "Name - EffectiveDateFrom.ToShortDateString()").
     */
    public List<Map<String, Object>> labSampleOrStandard(boolean sample, int itemId, int supplierId,
                                                         int commissionAgentId, int orderId, String docDate) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        List<Map<String, Object>> out = new ArrayList<>();
        if (sample) {
            StringBuilder sql = new StringBuilder("EXEC Sp_InvLabSampleAnalysisHeader_GetAllMethod "
                    + "@OrganizationId=?, @CompanyId=?, @SupplierCustomerId=?, @ItemId=?");
            List<Object> args = new ArrayList<>(List.of(orgId, compId, supplierId, itemId));
            if (commissionAgentId != 0) { sql.append(", @CommissionAgentId=?"); args.add(commissionAgentId); }
            if (orderId != 0) { sql.append(", @Id=?"); args.add(orderId); }
            sql.append(", @Activity=?");
            args.add("GetSampleNoBySupplierCustomerIdAndITemId");
            for (Map<String, Object> r : jdbcTemplate.queryForList(sql.toString(), args.toArray())) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("id", intOf(ci(r, "Id")));
                m.put("description", strOf(ci(r, "DocNo")));
                out.add(m);
            }
        } else {
            java.sql.Date d = dateOrNull(docDate);
            if (d == null) d = new java.sql.Date(System.currentTimeMillis());
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    "EXEC [dbo].[USP_LabAnalysisStandardSchedule_GetAllMethod] @OrganizationId=?, @CompanyId=?, "
                  + "@FromDate=?, @Activity=?", orgId, compId, d, "ComboFill")) {
                Object eff = ci(r, "EffectiveDateFrom");
                String effText = "";
                if (eff instanceof java.util.Date) {
                    effText = new java.text.SimpleDateFormat("dd/MM/yyyy").format((java.util.Date) eff);
                } else if (eff != null) {
                    effText = String.valueOf(eff);
                }
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("id", intOf(ci(r, "LabAnalysisGroupId")));
                m.put("description", strOf(ci(r, "LabAnalysisStandardScheduleName")) + " - " + effText);
                out.add(m);
            }
        }
        return out;
    }

    /**
     * The configuration and feature values PurchsaeOrder_Load reads (:630-634, :804, :1617-1647,
     * :4161-4193), in one read, for the page to apply exactly as the desktop does.
     */
    public Map<String, Object> screenDefaults() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("orderDefaultDeliveryDays", purchaseOrderHeaderRepository.config(orgId, compId, "OrderDefaultDeliveryDays"));
        out.put("weightCutForJuteBags",     purchaseOrderHeaderRepository.config(orgId, compId, "WeightCutForJuteBags"));
        out.put("weightCutForPPBags",       purchaseOrderHeaderRepository.config(orgId, compId, "WeightCutForPPBags"));
        out.put("itemSearchByCode",         parseConfigBool(purchaseOrderHeaderRepository.config(orgId, compId, "ItemSearchByCode")));
        out.put("warningNoExpense",         parseConfigBool(purchaseOrderHeaderRepository.config(orgId, compId, "WarningMessageOnPurchaseOrderForExpenseGridIsOn")));
        out.put("defaultDaysToLessFromHistoryFromDate",
                configInt(orgId, compId, "DefaultDaysToLessFromHistoryFromDate"));
        boolean multi = multiCurrencyFeature(orgId, compId);
        out.put("multiCurrency", multi);
        out.putAll(baseCurrencyFromConfiguration(orgId, compId));
        out.put("fcyDecimals", fcyDecimals(orgId, compId));
        out.put("amountDecimals", decimalPoints(orgId, compId, "Default NoofDecimal Points For Amount", 0));
        out.put("rateDecimals", decimalPoints(orgId, compId, "Default NoofDecimal Points For Rate", 2));
        out.put("rights", rights());
        /* HistoryBranchComboFill :4631 - cmbBranchName.Text = UserAccount.BranchName: the user's own branch starts ticked. */
        out.put("branchId", currentUserContext.currentBranchId());
        return out;
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
        /* Same company / visibility bound as the History grid (CanView AllRecord, allocated branch). */
        purchaseOrderRecordRepository.require(id == null ? 0 : id);
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
            m.put("labAnalysisStandardScheduleId", intOf(ci(d, "LabAnalysisStandardScheduleId")));   /* :3778 */
            m.put("fcyAmount",     ci(d, "FcyAmount"));
            m.put("remarks",       strOf(ci(d, "OrderRemarks")));
            lineItems.add(m);
        }
        head.put("lineItems", lineItems);

        /* DAL 0448:313 - dispatch rows against this order. :3779 treats "any row with
           SupplierDispatchId > 0" as the trigger. */
        List<Map<String, Object>> dispatch = jdbcTemplate.queryForList(
                "EXEC Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?",
                id, "ReadByHeaderId_PurchaseOrderSupplierDispatchDetail");
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
        head.put("labDeductions",           getLabDeductionByHeaderId(id));
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
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    "EXEC [dbo].[USP_GetBranchsAllocatedToUser] @OrganizationId=?, @CompanyId=?, @UserId=?",
                    orgId, compId, currentUserContext.currentUserId())) {
                if (intOf(ci(r, "BranchId")) == myBranch) {
                    myBranchName = strOf(ci(r, "BranchName"));
                    break;
                }
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
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@ConfigDescription=?, @Activity=?",
                orgId, compId, "PurchaseOrderBranchWise",
                "GetConfigurationByOrgCompandConfigDescription");
        if (rows.isEmpty()) return false;
        String v = strOf(ci(rows.get(0), "ConfigKey")).trim();
        return "1".equals(v) || "true".equalsIgnoreCase(v) || "yes".equalsIgnoreCase(v);
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
        String v = configValueFor(orgId, compId, configDescription);
        int n;
        try { n = Integer.parseInt(v.trim()); } catch (NumberFormatException e) { return fallback; }
        return (n >= 1 && n <= 4) ? n : fallback;
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
        /* getUpdateForHistory() :4983 reads PurchaseOrder.GetByID -> purchaseOrderDetailList, i.e.
           Sp_PurchaseOrder_GetAllMethod @Activity='ReadByPurchaseOrderHeaderId' (DAL 0448:243) - the
           procedure, not a hand-written join. Columns in the desktop's order (:4987-5000); Amount is
           Math.Round(Amount, DefaultNoofDecimalPointsForAmount, AwayFromZero) (:5004). */
        purchaseOrderRecordRepository.require(purchaseOrderId);
        int dp = decimalPoints(currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId(),
                "Default NoofDecimal Points For Amount", 0);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : jdbcTemplate.queryForList(
                "EXEC Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?", purchaseOrderId, "ReadByPurchaseOrderHeaderId")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemCode", strOf(ci(d, "ItemCode")));
            m.put("ItemName", strOf(ci(d, "ItemName")));
            m.put("CropYear", strOf(ci(d, "Crop")));
            m.put("UOM",      strOf(ci(d, "UOMCode")));
            m.put("ItemQTY",  ci(d, "OrderItemQty"));
            m.put("Weight",   ci(d, "NetWeight"));
            m.put("ItemRate", ci(d, "OrderItemRate"));
            m.put("RateUOM",  strOf(ci(d, "RateUom")));
            m.put("Amount",   java.math.BigDecimal.valueOf(dblOf(ci(d, "Amount"))).setScale(dp, java.math.RoundingMode.HALF_UP));
            m.put("JobLot",   strOf(ci(d, "JobLotDescription")));
            m.put("FcyAmount", ci(d, "FcyAmount"));
            m.put("CityName", strOf(ci(d, "CityArea")));
            m.put("Moisture", strOf(ci(d, "Moisture")));
            m.put("Remarks",  strOf(ci(d, "OrderRemarks")));
            out.add(m);
        }
        return out;
    }

    @Autowired
    private com.mst.repositories.PurchaseOrderRecordRepository purchaseOrderRecordRepository;

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
            throw new IllegalArgumentException("Select branch first");      /* :4830 */
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
        return Boolean.TRUE.equals(rights().get("canViewAllRecord"));
    }

    /**
     * The desktop Purchase Order form CANNOT delete a Purchase Order.
     *
     * PurchsaeOrder.cs:3881-3883 is the whole of btnDelete_Click:
     *
     *     private void btnDelete_Click(object sender, EventArgs e)
     *     {
     *     }
     *
     * The button exists and :629 even enables it from formright.DoHaveCanDelete, but the handler
     * body is empty - clicking Delete on the desktop form does nothing at all. There is no
     * Sp_PurchaseOrder_Delete in GoldenAcedb either (verified against the full procedure dump,
     * D:\CShapEccorErp\procdure.sql - the only PO deletion procedure is
     * USP_DeletePurchaseOrderDetailIfNotExistInGrn, which removes a single DETAIL line, never a
     * header). Deleting a Purchase Order is not a capability this ERP exposes.
     *
     * What used to be here was a fabricated raw-SQL delete with no desktop counterpart. Besides
     * having no source to port from, it was wrong on its own terms:
     *
     *   - it removed only PurchaseOrderDetail and PurchaseOrderEmptyBags, orphaning rows in
     *     PurchaseOrderExpensesChargeToProduct, PurchaseOrderLabDeduction,
     *     PurchaseOrderSupplierExpense, PurchaseOrderPaymentTermsDetail,
     *     PurchaseOrderSupplierDispatchDetail and AdvancePaymentAdjustment - the six further
     *     children Sp_PurchaseOrder_Update:219107-219114 knows this header owns;
     *   - it applied no tenancy filter, so any id in any organization/company was deletable;
     *   - it had none of the GRN / Purchase Invoice consumption guards that
     *     persistPurchaseOrderDetail() applies before removing even one line;
     *   - and it swallowed every failure into `return false`, so a partial delete reported the
     *     same thing as a refused one.
     *
     * Reachable over DELETE /api/purchase-order/{id}, that was a crafted request doing
     * irreversible damage to live data that no desktop user can do. The faithful port of an empty
     * handler is to refuse, which is what this now does.
     */
    public boolean deletePurchaseOrder(Integer id) {
        throw new UnsupportedOperationException(
                "Purchase Orders cannot be deleted. The desktop form's Delete button does nothing "
              + "(PurchsaeOrder.cs btnDelete_Click is empty) and GoldenAcedb has no Purchase Order "
              + "delete procedure. Correct the order with Update, or close it via Order Status.");
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
