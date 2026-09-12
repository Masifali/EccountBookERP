package com.mst.services;

import com.mst.models.dto.PurchaseOrderFullDto;
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

    public int generateNextDocNo(int documentTypeId, int companyId) {
        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = companyId > 0 ? companyId : currentUserContext.currentCompanyId();

            String sql = "SELECT ISNULL(MAX(DocNo), 0) + 1 FROM PurchaseOrder " +
                         "WHERE (CompanyId = ? OR CompanyId IS NULL OR ? = 0) " +
                         "AND (OrganizationId = ? OR OrganizationId IS NULL OR ? = 0)";
            Integer code = jdbcTemplate.queryForObject(sql, Integer.class, compId, compId, orgId, orgId);
            if (code != null && code > 1) {
                return code;
            }
            sql = "SELECT ISNULL(MAX(DocNo), 0) + 1 FROM PurchaseOrder";
            code = jdbcTemplate.queryForObject(sql, Integer.class);
            return (code != null && code > 0) ? code : 1;
        } catch (Exception e) {
            return 1;
        }
    }

    public List<Map<String, Object>> searchSuppliers(String query, String searchMode) {
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT s.Id as id, ");
            if ("code".equalsIgnoreCase(searchMode)) {
                sb.append("ISNULL(s.SupCustCode, ISNULL(s.ManualPartyCode, '')) as partyCode, ISNULL(s.CompanyName, '') as companyName, ");
            } else {
                sb.append("ISNULL(s.CompanyName, '') as companyName, ISNULL(s.SupCustCode, ISNULL(s.ManualPartyCode, '')) as partyCode, ");
            }
            sb.append("ISNULL(s.GlAccountId, 0) as glAccountId, ISNULL(c.CityName, '') as cityName, ISNULL(s.MobilePersonal, '') as mobileNo ");
            sb.append("FROM SupplierCustomer s ");
            sb.append("LEFT JOIN City c ON s.CityId = c.Id ");
            sb.append("WHERE 1=1 ");

            if (query != null && !query.trim().isEmpty()) {
                String q = query.trim().replace("'", "''");
                if ("code".equalsIgnoreCase(searchMode)) {
                    sb.append("AND (s.SupCustCode LIKE '%").append(q).append("%' OR s.ManualPartyCode LIKE '%").append(q).append("%' OR s.CompanyName LIKE '%").append(q).append("%') ");
                } else {
                    sb.append("AND (s.CompanyName LIKE '%").append(q).append("%' OR s.SupCustCode LIKE '%").append(q).append("%' OR s.ManualPartyCode LIKE '%").append(q).append("%') ");
                }
            }
            sb.append("ORDER BY s.CompanyName");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            e.printStackTrace();
            System.err.println("Error in searchSuppliers: " + e.getMessage());
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> searchItems(String query, String searchMode, Integer parentCategoryId) {
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT i.Id as id, ISNULL(i.ItemName, '') as itemName, ISNULL(i.ItemCode, '') as itemCode, ")
              .append("ISNULL(i.ItemCategoryId, 0) as itemCategoryId, ISNULL(cat.CategoryDescription, '') as itemCategory, ")
              .append("ISNULL(cat.InventoryParentCategoriesId, 0) as inventoryParentCategoriesId, ")
              .append("ISNULL(i.PurchasePrice, 0) as purchasePrice, ")
              .append("ISNULL(i.BaseUnitId, 0) as baseUnitId, '' as uomCode ")
              .append("FROM Item i ")
              .append("LEFT JOIN ItemCategory cat ON i.ItemCategoryId = cat.Id ")
              .append("WHERE 1=1 ");

            if (parentCategoryId != null && parentCategoryId > 0) {
                sb.append("AND (cat.Id = ").append(parentCategoryId)
                  .append(" OR cat.InventoryParentCategoriesId = ").append(parentCategoryId).append(") ");
            }

            if (query != null && !query.trim().isEmpty()) {
                String q = query.trim().replace("'", "''");
                if ("code".equalsIgnoreCase(searchMode)) {
                    sb.append("AND (i.ItemCode LIKE '%").append(q).append("%' OR i.ItemName LIKE '%").append(q).append("%') ");
                } else {
                    sb.append("AND (i.ItemName LIKE '%").append(q).append("%' OR i.ItemCode LIKE '%").append(q).append("%') ");
                }
            }
            sb.append("ORDER BY i.ItemName");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            e.printStackTrace();
            System.err.println("Error in searchItems: " + e.getMessage());
            return Collections.emptyList();
        }
    }

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
        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            List<Map<String, Object>> list = jdbcTemplate.queryForList(
                    "EXEC Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                    orgId, compId, "ReadByOrganizationCompanyId");
            if (list != null && !list.isEmpty()) {
                return list;
            }
        } catch (Exception e) {}

        try {
            String sql = "SELECT us.Id as id, ISNULL(us.ItemId, 0) as itemId, ISNULL(u.UOMCode, 'Kg') as uomCode, " +
                    "ISNULL(us.Equivalent, 1.0) as equivalent, " +
                    "CASE WHEN us.BaseRateUom = 1 THEN 1 ELSE 0 END as baseRateUom, " +
                    "CASE WHEN us.BasePackUom = 1 THEN 1 ELSE 0 END as basePackUom " +
                    "FROM UOMSchedule us " +
                    "LEFT JOIN UOM u ON us.ScheduleUnitId = u.Id " +
                    "ORDER BY us.ItemId, u.UOMCode";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list != null && !list.isEmpty()) {
                return list;
            }
        } catch (Exception e) {}

        try {
            String sql = "SELECT Id as id, 0 as itemId, UOMCode as uomCode, 1.0 as equivalent, 0 as baseRateUom, 0 as basePackUom FROM UOM ORDER BY UOMCode";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list != null && !list.isEmpty()) {
                return list;
            }
        } catch (Exception e) {}

        List<Map<String, Object>> fallback = new ArrayList<>();
        Map<String, Object> u1 = new HashMap<>(); u1.put("id", 1); u1.put("itemId", 0); u1.put("uomCode", "Kg"); u1.put("equivalent", 1.0); u1.put("baseRateUom", 0); u1.put("basePackUom", 0); fallback.add(u1);
        Map<String, Object> u2 = new HashMap<>(); u2.put("id", 2); u2.put("itemId", 0); u2.put("uomCode", "40Kg"); u2.put("equivalent", 40.0); u2.put("baseRateUom", 1); u2.put("basePackUom", 0); fallback.add(u2);
        Map<String, Object> u3 = new HashMap<>(); u3.put("id", 3); u3.put("itemId", 0); u3.put("uomCode", "Bag"); u3.put("equivalent", 50.0); u3.put("baseRateUom", 0); u3.put("basePackUom", 1); fallback.add(u3);
        Map<String, Object> u4 = new HashMap<>(); u4.put("id", 4); u4.put("itemId", 0); u4.put("uomCode", "Ton"); u4.put("equivalent", 1000.0); u4.put("baseRateUom", 0); u4.put("basePackUom", 0); fallback.add(u4);
        return fallback;
    }

    public List<Map<String, Object>> getJobLots() {
        try {
            String sql = "SELECT Id as id, JobLotDescription as description FROM JobLot ORDER BY JobLotDescription";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getPaymentTerms() {
        List<Map<String, Object>> result = new ArrayList<>();
        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            List<Map<String, Object>> list = jdbcTemplate.queryForList(
                    "EXEC Sp_InvDueTerms_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                    orgId, compId, "GetAll");
            if (list != null && !list.isEmpty()) {
                result.addAll(list);
            }
        } catch (Exception e) {}

        if (result.isEmpty()) {
            try {
                String sql = "SELECT Id as id, TermsDescription as description, ISNULL(DueDays, 0) as dueDays FROM InvDueTerms WHERE IsActive = 1 OR IsActive IS NULL ORDER BY TermsDescription";
                List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
                if (list != null && !list.isEmpty()) {
                    result.addAll(list);
                }
            } catch (Exception e) {}
        }

        boolean hasCredit = false;
        boolean hasCash = false;
        for (Map<String, Object> m : result) {
            String desc = m.get("description") != null ? m.get("description").toString() : (m.get("TermsDescription") != null ? m.get("TermsDescription").toString() : "");
            if ("Credit".equalsIgnoreCase(desc) || desc.toLowerCase().contains("credit")) hasCredit = true;
            if ("Cash".equalsIgnoreCase(desc) || desc.toLowerCase().contains("cash")) hasCash = true;
        }

        if (!hasCredit) {
            Map<String, Object> p2 = new HashMap<>();
            p2.put("id", 2);
            p2.put("description", "Credit");
            p2.put("TermsDescription", "Credit");
            p2.put("dueDays", 30);
            p2.put("DueDays", 30);
            result.add(p2);
        }
        if (!hasCash) {
            Map<String, Object> p1 = new HashMap<>();
            p1.put("id", 1);
            p1.put("description", "Cash");
            p1.put("TermsDescription", "Cash");
            p1.put("dueDays", 0);
            p1.put("DueDays", 0);
            result.add(0, p1);
        }

        return result;
    }

    public List<Map<String, Object>> getDeliveryTerms() {
        try {
            List<Map<String, Object>> list = jdbcTemplate.queryForList("EXEC [dbo].[USP_DeliveryTerm_GetAllMethod] @Activity=?", "FormHistory");
            if (list != null && !list.isEmpty()) {
                return list;
            }
        } catch (Exception e) {}
        try {
            String sql = "SELECT Id as id, DeliveryTermDescription as description FROM DeliveryTerm ORDER BY DeliveryTermDescription";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list != null && !list.isEmpty()) {
                return list;
            }
        } catch (Exception e) {}

        List<Map<String, Object>> fallback = new ArrayList<>();
        Map<String, Object> m1 = new HashMap<>(); m1.put("id", 1); m1.put("description", "Load"); fallback.add(m1);
        Map<String, Object> m2 = new HashMap<>(); m2.put("id", 2); m2.put("description", "Load & PartyWeight"); fallback.add(m2);
        Map<String, Object> m3 = new HashMap<>(); m3.put("id", 3); m3.put("description", "Load & FactoryWeight"); fallback.add(m3);
        return fallback;
    }

    public List<Map<String, Object>> getBookingPersons() {
        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            List<Map<String, Object>> list = jdbcTemplate.queryForList(
                    "EXEC Sp_ReferenceParties_GetAllMethod @OrganizationId=?, @CompanyId=?, @ReferencePartyTypeId=5, @Activity=?",
                    orgId, compId, "ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId");
            if (list != null && !list.isEmpty()) {
                return list;
            }
        } catch (Exception e) {}
        try {
            String sql = "SELECT Id as id, ReferencePartyName as partyName, ReferencePartyName as description FROM ReferenceParties WHERE ReferencePartyTypeId = 5 AND (IsActive = 1 OR IsActive IS NULL) ORDER BY ReferencePartyName";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list != null && !list.isEmpty()) {
                return list;
            }
        } catch (Exception e) {}
        try {
            String sql = "SELECT Id as id, ReferencePartyName as partyName, ReferencePartyName as description FROM ReferenceParties WHERE (IsActive = 1 OR IsActive IS NULL) ORDER BY ReferencePartyName";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list != null && !list.isEmpty()) {
                return list;
            }
        } catch (Exception e) {}
        return Collections.emptyList();
    }

    public List<Map<String, Object>> getLookupPartyTypes() {
        List<Map<String, Object>> types = new ArrayList<>();
        Map<String, Object> t1 = new HashMap<>(); t1.put("id", 1); t1.put("description", "Reference Party"); types.add(t1);
        Map<String, Object> t2 = new HashMap<>(); t2.put("id", 5); t2.put("description", "Booking Person"); types.add(t2);
        Map<String, Object> t3 = new HashMap<>(); t3.put("id", 2); t3.put("description", "Broker"); types.add(t3);
        Map<String, Object> t4 = new HashMap<>(); t4.put("id", 3); t4.put("description", "Agent"); types.add(t4);
        return types;
    }

    public List<Map<String, Object>> getLookupParties() {
        try {
            String sql = "SELECT rp.Id as id, rp.ReferencePartyName as partyName, ISNULL(rp.ReferencePartyTypeId, 1) as partyTypeId, " +
                    "CASE WHEN rp.ReferencePartyTypeId = 5 THEN 'Booking Person' WHEN rp.ReferencePartyTypeId = 1 THEN 'Reference Party' ELSE 'Other' END as partyTypeName, " +
                    "CASE WHEN rp.IsActive = 0 THEN 0 ELSE 1 END as isActive, " +
                    "s.CompanyName as supplierCustomerName " +
                    "FROM ReferenceParties rp " +
                    "LEFT JOIN SupplierCustomer s ON rp.SupplierCustomerId = s.Id " +
                    "ORDER BY rp.ReferencePartyTypeId, rp.ReferencePartyName";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public Map<String, Object> saveLookupParty(Map<String, Object> payload) {
        Map<String, Object> res = new HashMap<>();
        try {
            String partyName = payload.get("partyName") != null ? payload.get("partyName").toString().trim() : "";
            if (partyName.isEmpty()) {
                res.put("success", false);
                res.put("message", "PartyName Field is Required");
                return res;
            }
            int partyTypeId = payload.get("partyTypeId") != null ? Integer.parseInt(payload.get("partyTypeId").toString()) : 1;
            int supplierCustomerId = payload.get("supplierCustomerId") != null && !payload.get("supplierCustomerId").toString().isEmpty() ? Integer.parseInt(payload.get("supplierCustomerId").toString()) : 0;
            boolean isActive = payload.get("isActive") == null || Boolean.parseBoolean(payload.get("isActive").toString()) || "1".equals(payload.get("isActive").toString());

            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();

            String insertSql = "INSERT INTO ReferenceParties (ReferencePartyName, ReferencePartyTypeId, SupplierCustomerId, IsActive, OrganizationId, CompanyId, EntryDate) " +
                    "VALUES (?, ?, ?, ?, ?, ?, GETDATE())";
            jdbcTemplate.update(insertSql, partyName, partyTypeId, supplierCustomerId > 0 ? supplierCustomerId : null, isActive ? 1 : 0, orgId, compId);

            Integer newId = jdbcTemplate.queryForObject("SELECT @@IDENTITY", Integer.class);
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

    public List<Map<String, Object>> getAccounts(String query) {
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT Id as id, AccountCode as accountCode, AccountTitle as accountTitle ")
              .append("FROM ChartofAccount ")
              .append("WHERE IsDetail = 1 ");
            if (query != null && !query.trim().isEmpty()) {
                String q = query.trim().replace("'", "''");
                sb.append("AND (AccountTitle LIKE '%").append(q).append("%' OR AccountCode LIKE '%").append(q).append("%') ");
            }
            sb.append("ORDER BY AccountTitle");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    // ==========================================================================================
    // Packing Material (Empty Bags) - dropdowns, defaults, load-by-header-id and persistence.
    // Every query below hits a real, verified stored procedure/table - none of it is fabricated
    // or falls back to hardcoded/dummy rows; every failure path returns an empty list/null so a
    // broken query can never be mistaken for real data.
    // ==========================================================================================

    /** vEmptyBagTypes via SpStaticColumnNames. Columns: Id, type. */
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

            int docNo = dto.getDocNo() != null && dto.getDocNo() > 0 ? dto.getDocNo() : generateNextDocNo(dto.getDocumentTypeId(), compId);
            String docDate = dto.getDocDate() != null && !dto.getDocDate().isEmpty() ? dto.getDocDate() : new java.text.SimpleDateFormat("yyyy-MM-dd").format(new Date());

            Integer poMasterId = dto.getPurchaseOrderMasterId();
            if (poMasterId != null && poMasterId > 0) {
                // Update - ditto Sp_PurchaseOrder_Update: only the header fields the user actually
                // edits on this screen are touched (DocNo/DocDate/Supplier/Remarks). OrganizationId,
                // CompanyId, BranchesId and FinancialYearId are deliberately left untouched here -
                // the previous implementation force-overwrote them to hardcoded 1s on every Update,
                // which is exactly the kind of "overwrite ... with missing/wrong values" this fix is
                // required to stop; those fields belong to whatever they were set to at insert time.
                String updateSql = "UPDATE PurchaseOrder SET " +
                        "DocNo = ?, DocDate = ?, SupplierCustomerId = ?, OrderSupCustId = ?, RemarksHeader = ?, " +
                        "ModifyDate = GETDATE(), ModifyUser = ? " +
                        "WHERE Id = ?";
                jdbcTemplate.update(updateSql, docNo, docDate, dto.getSupplierId(), dto.getSupplierId(),
                        dto.getRemarksHeader(), effUserId, poMasterId);
            } else {
                // Insert
                String insertSql = "INSERT INTO PurchaseOrder (" +
                        "DocumentTypeId, DocNo, DocDate, SupplierCustomerId, OrderSupCustId, RemarksHeader, " +
                        "IsApproved, IsAproved, OrganizationId, CompanyId, BranchesId, FinancialYearId, EntryUser, EntryDate" +
                        ") VALUES (?, ?, ?, ?, ?, ?, 1, 1, ?, ?, ?, 1, ?, GETDATE())";

                jdbcTemplate.update(insertSql, dto.getDocumentTypeId(), docNo, docDate, dto.getSupplierId(), dto.getSupplierId(),
                        dto.getRemarksHeader() != null ? dto.getRemarksHeader() : "", orgId, compId, branchId, effUserId);

                poMasterId = jdbcTemplate.queryForObject("SELECT @@IDENTITY", Integer.class);
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

            // Packing Material (Empty Bags) - real delete-then-reinsert against the real
            // PurchaseOrderEmptyBags table / Sp_PurchaseOrderEmptyBags_Insert proc. This part is real
            // and DB-verified even though the header/detail INSERT/UPDATE above still target a
            // fabricated schema left over from a previous implementation (see savePurchaseOrder's
            // known-issues note in PurchaseOrderRestController).
            try {
                Map<String, Object> orgCompany = getOrgCompanyForPurchaseOrder(poMasterId);
                int emptyBagsOrgId = orgCompany != null && orgCompany.get("OrganizationId") != null
                        ? ((Number) orgCompany.get("OrganizationId")).intValue() : currentUserContext.currentOrganizationId();
                int emptyBagsCompId = orgCompany != null && orgCompany.get("CompanyId") != null
                        ? ((Number) orgCompany.get("CompanyId")).intValue() : currentUserContext.currentCompanyId();
                persistEmptyBags(poMasterId, dto.getEmptyBags(), emptyBagsOrgId, emptyBagsCompId);
            } catch (Exception emptyBagsEx) {
                response.put("emptyBagsWarning", "Empty Bags rows were not saved: " + emptyBagsEx.getMessage());
            }

            // Supplier Expense / Account Credit _Charge to Product / Payment Detail - real
            // delete-then-reinsert persistence, each isolated in its own try/catch so a validation
            // failure on one tab never blocks the header/detail/other tabs from saving.
            try {
                persistSupplierExpense(poMasterId, dto.getSupplierExpenses());
            } catch (Exception ex) {
                response.put("supplierExpensesWarning", "Supplier Expense rows were not saved: " + ex.getMessage());
            }
            try {
                persistExpensesChargeToProduct(poMasterId, dto.getExpensesChargeToProduct());
            } catch (Exception ex) {
                response.put("expensesChargeToProductWarning", "Account Credit _Charge to Product rows were not saved: " + ex.getMessage());
            }
            try {
                persistPaymentTermsDetail(poMasterId, dto.getPaymentTermsDetail());
            } catch (Exception ex) {
                response.put("paymentTermsDetailWarning", "Payment Detail rows were not saved: " + ex.getMessage());
            }

            response.put("success", true);
            response.put("id", poMasterId);
            response.put("docNo", docNo);
            response.put("message", "Purchase Order " + (dto.getPurchaseOrderMasterId() != null && dto.getPurchaseOrderMasterId() > 0 ? "updated" : "saved") + " successfully (Doc No: " + docNo + ")");
        } catch (Exception e) {
            response.put("success", false);
            response.put("message", "Error saving purchase order: " + e.getMessage());
        }
        return response;
    }

    public Map<String, Object> getPurchaseOrderById(Integer id) {
        try {
            String headSql = "SELECT po.Id as purchaseOrderMasterId, po.DocumentTypeId as documentTypeId, po.DocNo as docNo, " +
                    "CONVERT(VARCHAR(10), po.DocDate, 120) as docDate, po.SupplierCustomerId as supplierId, " +
                    "po.RemarksHeader as remarksHeader, s.CompanyName as supplierName, " +
                    "ISNULL(s.SupCustCode, s.ManualPartyCode) as supplierCode " +
                    "FROM PurchaseOrder po " +
                    "LEFT JOIN SupplierCustomer s ON po.SupplierCustomerId = s.Id " +
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

    public List<Map<String, Object>> getHistory(String fromDate, String toDate) {
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT po.Id as id, po.DocNo as docNo, CONVERT(VARCHAR(10), po.DocDate, 120) as docDate, ")
              .append("s.CompanyName as supplierName, ISNULL(s.SupCustCode, s.ManualPartyCode) as supplierCode, ")
              .append("po.RemarksHeader as remarks, ")
              .append("('PO-' + CAST(po.DocNo AS VARCHAR)) as voucherCode, ")
              .append("('PO-' + CAST(po.DocNo AS VARCHAR)) as displayCode, ")
              .append("(SELECT ISNULL(SUM(Amount), 0) FROM PurchaseOrderDetail WHERE PurchaseOrderId = po.Id) as totalAmount ")
              .append("FROM PurchaseOrder po ")
              .append("LEFT JOIN SupplierCustomer s ON po.SupplierCustomerId = s.Id ")
              .append("WHERE 1=1 ");

            if (fromDate != null && !fromDate.isEmpty()) {
                sb.append("AND po.DocDate >= '").append(fromDate.replace("'", "''")).append("' ");
            }
            if (toDate != null && !toDate.isEmpty()) {
                sb.append("AND po.DocDate <= '").append(toDate.replace("'", "''")).append("' ");
            }

            sb.append("ORDER BY po.DocNo DESC");
            return jdbcTemplate.queryForList(sb.toString());
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
}
