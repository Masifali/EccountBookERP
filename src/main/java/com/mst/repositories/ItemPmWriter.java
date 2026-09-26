package com.mst.repositories;

import com.mst.models.UserAccount;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.Timestamp;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;

/**
 * The write half of screen 496 "Item PM": BLL {@code Item.Save(items, AutoCoaDefineByItemNameOnInsertPm)}
 * (BLL 0583 :15) and DAL {@code Item.SetData(obj, proc, AutoCoaInsertIsOn)} (DAL 0436 :23-606),
 * ported for this caller. The caller owns the transaction (the DAL opens one; the service opens
 * the same one with @Transactional).
 *
 * <h3>Why a writer of its own</h3>
 * {@link DesktopInventoryItemWriter} ports the same DAL method for screen 111, but it builds its
 * parameter set from InvDefrmAddItem's request and reads only ONE of the two switches below. The
 * PM form assigns a different set of Item properties (MasterItemId, RackId, EmptyBagWeight,
 * MinStockLevel, ReOrderQty, LeadTimeDay, PackSize, PackSizeId, AllowMultiUom, BarcodeNo, IsImport,
 * Pic1/Pic2) and passes {@code AutoCoaInsertIsOn} from its own configuration row, so reusing the
 * screen-111 writer would have silently dropped those columns - the "CLR default" defect class.
 *
 * <h3>Automatic chart-of-account creation (DAL :36)</h3>
 * All four must hold:
 * <pre>
 *   parent category of ItemCategoryId != 9
 *   AND Id == 0                                         (insert only)
 *   AND config "AutoCoaDefineByItemNameOnInsert"        (read by the DAL itself, :29)
 *   AND AutoCoaInsertIsOn                               (the form passes config "AutoCoaDefineByItemNameOnInsertPm", :343)
 * </pre>
 * Then with "SameAccountForStockAndRevenue" on, ONE "Stock A/c" account is made and used for both
 * Stock and Sale (:52-82); otherwise Stock and Sale parents are read and one account is made when
 * their ParentCodeId match, two when they differ (:84-127).
 *
 * <h3>Children - only when the Item procedure returns an id > 0 (:144)</h3>
 * tax schedule (ApplyGST and TaxTypeId > 0) · one UOM schedule from the base unit (ItemGroupId is
 * always 0 on this form) · per allocated company: ItemAllocation, the four price schedules (all
 * zero on this form, so none written), the reorder schedule when ReorderLevel > 0, commission (off
 * on this form) and the FeedMill default price row. When the procedure returns 0 - as the update
 * procedure does - {@code num5 = obj.Id} and no child is written (:348-351). That is the desktop's
 * control flow, reproduced rather than assumed.
 */
@Repository
public class ItemPmWriter {

    private final JdbcTemplate jdbc;
    private final Map<String, List<String>> parameters = new ConcurrentHashMap<>();

    public ItemPmWriter(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    /** Everything SetData needs beyond the Item property bag. */
    public static final class Save {
        public boolean insert;
        public int id;
        /** The Item property bag, keyed by model property name. */
        public Map<String, Object> item;
        public int itemCategoryId;
        public int itemTypeId;
        public String itemName;
        public String itemCode;
        public double equivalent;
        public boolean applyGst;
        public int taxTypeId;
        public double reorderLevel, minStockLevel, maxStockLevel;
        /** ItemAllocationlist - one row per company the form sends (:941-976). */
        public List<Allocation> allocations = new ArrayList<>();
        /** ItemImageslist - already resolved to (FileName, ImagePath, SortNo). */
        public List<Map<String, Object>> images = new ArrayList<>();
        public boolean autoCoaInsertIsOn;
        public int financialYearId;
    }

    public static final class Allocation {
        public int companyId;
        public boolean isActive;
        public int branchId;
        public int organizationId;
        public Allocation(int companyId, boolean isActive, int branchId, int organizationId) {
            this.companyId = companyId; this.isActive = isActive; this.branchId = branchId; this.organizationId = organizationId;
        }
    }

    public int save(UserAccount u, Save s) {
        Map<String, Object> p = InventoryPosDefaults.item();
        p.putAll(s.item);
        Timestamp now = now();
        p.put("EntryDate", now);                      // BLL :19-22
        p.put("ModifyDate", now);
        p.put("PostDate", now);
        p.put("PostState", false);
        if (!s.insert) {                              // BLL :27-28
            p.put("CommOnPurchase", false);
            p.put("CommOnSale", false);
        }

        // ---------------------------------------------------------- DAL :36-142
        if (parentCategory(u, s.itemCategoryId) != 9 && s.insert
                && configuration(u, "AutoCoaDefineByItemNameOnInsert") && s.autoCoaInsertIsOn) {
            boolean same = configuration(u, "SameAccountForStockAndRevenue");
            int purchase, sale;
            if (same) {
                Map<String, Object> stock = accountParent(s.itemCategoryId, "Stock");
                purchase = sale = saveCoa(u, s, stock, "Stock A/c");
            } else {
                Map<String, Object> stock = accountParent(s.itemCategoryId, "Stock");
                Map<String, Object> saleParent = accountParent(s.itemCategoryId, "Sale");
                if (intOf(stock.get("ParentCodeId")) == intOf(saleParent.get("ParentCodeId"))) {
                    purchase = sale = saveCoa(u, s, stock, "Stock A/c");
                } else {
                    purchase = saveCoa(u, s, stock, "Stock A/c");
                    sale = saveCoa(u, s, saleParent, "Sale A/c");
                }
            }
            if (purchase == 0) throw new IllegalStateException("PurchaseGLAC  Account Not Found");
            if (sale == 0) throw new IllegalStateException("SaleGLAC Account Not Found");
            if (same && purchase != sale) {
                throw new IllegalStateException("PurchaseGLAC And SaleGLAC Account should be same when Configuration 'SameAccountForStockAndRevenue' is on");
            }
            p.put("PurchaseGLAC", purchase);
            p.put("SaleGLAC", sale);
        }

        // ---------------------------------------------------------- DAL :143
        int num5 = execute(s.insert ? "dbo.Sp_Item_Insert" : "dbo.Sp_Item_Update", p);
        if (num5 > 0) {
            int entryUser = intOf(p.get("EntryUser"));
            int modifyUser = intOf(p.get("ModifyUser"));
            if (s.applyGst && s.taxTypeId > 0) {                                   // :146-159
                Map<String, Object> tax = new LinkedHashMap<>();
                tax.put("Id", 0); tax.put("ItemId", num5); tax.put("IsActive", true);
                tax.put("EffectedDate", now()); tax.put("EntryDate", now()); tax.put("ModifyDate", now());
                tax.put("EntryUser", entryUser); tax.put("ModifyUser", 0);
                tax.put("OrganizationId", u.getOrganizationId()); tax.put("CompanyId", u.getCompanyId());
                tax.put("TaxTypeId", s.taxTypeId); tax.put("TaxName", null);
                execute("dbo.Sp_ItemTaxSchedule_Insert", tax);
            }
            // ItemGroupId is always 0 on this form (:903) -> base-unit schedule (:203-225)
            Map<String, Object> uom = new LinkedHashMap<>();
            uom.put("Id", 0); uom.put("ItemId", num5);
            uom.put("ScheduleUnitId", intOf(p.get("BaseUnitId")));
            uom.put("Equivalent", s.equivalent); uom.put("QtyEquivalent", 1.0d);
            uom.put("EntryDate", now()); uom.put("ModifyDate", now());
            uom.put("EntryUser", entryUser); uom.put("ModifyUser", 0);
            uom.put("OrganizationId", u.getOrganizationId()); uom.put("CompanyId", u.getCompanyId());
            uom.put("Active", true);
            int itemClass = intOf(p.get("ItemClassId"));
            uom.put("BaseRateUom", (itemClass == 8 && s.equivalent == 1000.0) || s.equivalent == 40.0);
            uom.put("BasePackUom", false); uom.put("BaseSecondaryUom", false);
            execute("dbo.Sp_UOMSchedule_Insert", uom);

            if (s.allocations == null || s.allocations.isEmpty()) {              // :226-229
                throw new IllegalStateException("Item Allocation List Empty");
            }
            int branchesId = intOf(p.get("BranchesId"));
            for (Allocation a : s.allocations) {
                Map<String, Object> al = new LinkedHashMap<>();
                al.put("Id", 0); al.put("ItemId", num5); al.put("IsActive", a.isActive);
                al.put("BranchId", a.branchId); al.put("CompanyId", a.companyId); al.put("OrganizationId", a.organizationId);
                execute("dbo.Sp_ItemAllocation_Insert", al);
                // CostPrice / PurchasePrice / WholeSalePrice / RetailPrice are never set on this
                // form, so the four price schedules at :234-293 are never written.
                if (s.reorderLevel > 0.0) {                                         // :294-310
                    Map<String, Object> r = InventoryPosDefaults.reorder();
                    r.put("ItemId", num5); r.put("EffectiveDate", now()); r.put("EntryDate", now()); r.put("ModifyDate", now());
                    r.put("EntryUser", entryUser);
                    r.put("OrganizationId", a.organizationId); r.put("CompanyId", a.companyId);
                    r.put("BranchesId", branchesId);
                    r.put("MinQty", s.minStockLevel); r.put("MaxQty", s.maxStockLevel);
                    r.put("OptimalQty", 0d); r.put("ReOrderPoint", s.reorderLevel);
                    execute("dbo.Sp_ItemsReorderSchedule_Insert", r);
                }
                // CommOnSale / CommOnPurchase: never set on this form (:311-327 not reached).
                Map<String, Object> f = InventoryPosDefaults.feedPrice();              // :328-345
                f.put("ItemId", num5);
                f.put("EffectiveDate", new Timestamp(System.currentTimeMillis() - 7L * 24 * 60 * 60 * 1000));
                f.put("ItemRate", 1.0d); f.put("PricingCustomGroupId", 1); f.put("IsActive", true); f.put("DocumentTypeId", 1);
                f.put("EntryDate", now()); f.put("ModifyDate", now());
                f.put("EntryUserId", entryUser); f.put("ModifyUserId", modifyUser);
                f.put("IsApproved", false); f.put("ApprovedUserId", 0); f.put("ApprovedDate", now()); f.put("FinancialYearId", 0);
                f.put("OrganizationId", a.organizationId); f.put("CompanyId", a.companyId);
                execute("fed.usp_ItemPricingSchedule_Insert", f);
            }
        } else {
            num5 = s.id;                                                             // :348-351
        }

        // ---------------------------------------------------------- images :499-506
        for (Map<String, Object> img : s.images) {
            Map<String, Object> row = new LinkedHashMap<>(img);
            row.put("Id", 0);
            row.put("ItemId", num5);
            execute("dbo.USP_ItemImage_Insert", row);
        }
        return num5;
    }

    /**
     * Item.SaveMasterItemAllocations (BLL :2718) -> DAL SetData(MasterItemAllocations) :999, one
     * USP_MasterItemAllocations_Insert per row, EntryDate/ModifyDate stamped per row.
     */
    public int saveMasterItemAllocations(UserAccount u, List<Map<String, Object>> rows) {
        int result = 0;
        for (Map<String, Object> r : rows) {
            Map<String, Object> p = new LinkedHashMap<>();
            p.put("EntryDate", now()); p.put("ModifyDate", now());
            p.put("EntryUserId", u.getId()); p.put("ModifyUserId", u.getId());
            p.put("Id", 0); p.put("ManualCode", null);
            p.put("ItemId", r.get("ItemId"));
            p.put("MasterItemId", r.get("MasterItemId"));
            p.put("EmptyBagWeight", r.get("EmptyBagWeight"));
            result = execute("dbo.USP_MasterItemAllocations_Insert", p);
        }
        return result;
    }

    // ------------------------------------------------------------------ DAL SaveCoa :608-723

    private int saveCoa(UserAccount u, Save s, Map<String, Object> parent, String description) {
        Map<String, Object> a = InventoryPosDefaults.account();
        String code = Objects.toString(parent.get("ParentAccountCode"), "");
        a.put("ParentAccountCode", code.isEmpty() ? "0" : code);
        a.put("AccountCode", code.isEmpty() ? null : code);
        a.put("AccountClass", intOf(parent.get("AccountClass")));
        a.put("ParentCodeId", intOf(parent.get("ParentCodeId")));
        a.put("AccountTypeId", intOf(parent.get("AccountTypeId")));
        a.put("PLNoteId", intOf(parent.get("PLNoteId")));
        a.put("BSNoteId", intOf(parent.get("BSNoteId")));
        a.put("Account_Level", 4);
        a.put("AccountTitle", Objects.toString(s.itemName, "") + " " + description);
        a.put("AccountGroup", "Detail");
        a.put("OtherErpCode", Objects.toString(s.itemCode, ""));
        a.put("OrganizationId", u.getOrganizationId());
        a.put("CompanyId", u.getCompanyId());
        int branch = intOf(s.item.get("BranchesId"));     // objitem.BranchesId - not set by this form
        a.put("BranchId", branch);
        a.put("EntryUser", intOf(s.item.get("EntryUser")));
        a.put("IsActive", true);
        a.put("FinancialYearId", s.financialYearId);
        a.put("ActionId", 1);
        a.put("EntryDate", now()); a.put("ModifyDate", now()); a.put("PostDate", now());

        // COAAllocationList (:631-667): one company -> it; several -> the active ones.
        List<Integer> companies = new ArrayList<>();
        int inactive = 0;
        int count = s.allocations.size();
        if (count == 0) throw new IllegalStateException("Item Allocation Grid Record Not Found");
        for (Allocation al : s.allocations) {
            if (count == 1 || al.isActive) companies.add(al.companyId); else inactive++;
        }
        if (count > 1 && count == inactive) throw new IllegalStateException("Please select Branch first");

        int id = execute("dbo.Proc_ChartofAccount_Insert", a);
        if (id > 0) {
            for (int company : companies) {
                Map<String, Object> alloc = InventoryPosDefaults.accountAllocation();
                alloc.put("CompanyId", company); alloc.put("IsActive", true);
                alloc.put("BranchId", branch); alloc.put("ChartofAccountId", id);
                execute("dbo.Sp_COAAllocation_Insert", alloc);
                Map<String, Object> ob = InventoryPosDefaults.accountOpening();
                ob.put("OrganizationId", u.getOrganizationId()); ob.put("CompanyId", company);
                ob.put("EntryUser", intOf(s.item.get("EntryUser"))); ob.put("ModifyUser", 0);
                ob.put("PostState", false);
                ob.put("ChartOfAccountId", id); ob.put("ChartOfAccountTitle", a.get("AccountTitle"));
                ob.put("FinancialYearId", s.financialYearId);
                ob.put("YearObDebit", 0d); ob.put("YearObCredit", 0d);    // chartofAccount.YearOb* are never set
                execute("dbo.Sp_AccountsOpeningBalances_Insert", ob);
            }
        }
        return id;
    }

    // ------------------------------------------------------------------ reads used by SetData

    private int parentCategory(UserAccount u, int category) {
        var rows = jdbc.queryForList(
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemCategoryId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), category, "GetParentCategoryIdByItemCategoryId");
        return rows.isEmpty() ? 0 : intOf(ItemPmRepository.col(rows.get(0), "InventoryParentCategoriesId"));
    }

    private boolean configuration(UserAccount u, String name) {
        var rows = jdbc.queryForList(
                "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), name, "GetConfigurationByOrgCompandConfigDescription");
        return !rows.isEmpty() && Set.of("true", "1").contains(
                Objects.toString(ItemPmRepository.col(rows.get(0), "ConfigKey"), "").trim().toLowerCase(Locale.ROOT));
    }

    /** usp_getAccountsInfoByItemCategoryId. A missing row leaves every field 0/"" as the DAL does. */
    private Map<String, Object> accountParent(int category, String activity) {
        var rows = jdbc.queryForList("EXEC dbo.usp_getAccountsInfoByItemCategoryId @ItemCategoryId=?, @Activity=?", category, activity);
        if (rows.isEmpty()) return new HashMap<>();
        Map<String, Object> r = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        r.putAll(rows.get(0));
        return r;
    }

    // ------------------------------------------------------------------ GenericProvider.SetProc

    /**
     * Sends exactly the model properties the procedure declares (read from sys.parameters), the
     * way GenericProvider.SetProc does, and returns the first column of the first row, or 0.
     */
    private int execute(String procedure, Map<String, Object> values) {
        List<String> names = parameters.computeIfAbsent(procedure, p -> jdbc.queryForList(
                "SELECT SUBSTRING(name,2,128) FROM sys.parameters WHERE object_id=OBJECT_ID(?) AND parameter_id>0 ORDER BY parameter_id",
                String.class, p));
        if (names.isEmpty()) throw new IllegalStateException("Procedure contract unavailable: " + procedure);
        TreeMap<String, Object> ci = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        ci.putAll(values);
        List<String> selected = names.stream().filter(ci::containsKey).toList();
        String sql = "EXEC " + procedure + " " + String.join(", ", selected.stream().map(k -> "@" + k + "=?").toList());
        return jdbc.execute(sql, (PreparedStatementCallback<Integer>) st -> {
            int n = 1;
            for (String name : selected) st.setObject(n++, ci.get(name));
            return firstNumber(st);
        });
    }

    private static int firstNumber(PreparedStatement st) throws java.sql.SQLException {
        boolean result = st.execute();
        Integer first = null;
        while (true) {
            if (result) {
                try (ResultSet rs = st.getResultSet()) {
                    if (first == null && rs.next()) {
                        Object v = rs.getObject(1);
                        if (v instanceof Number num) first = num.intValue();
                        else if (v != null) { try { first = (int) Double.parseDouble(v.toString()); } catch (NumberFormatException ignored) { first = 0; } }
                    }
                }
            } else if (st.getUpdateCount() == -1) {
                break;
            }
            result = st.getMoreResults();
        }
        return first == null ? 0 : first;
    }

    private static Timestamp now() { return new Timestamp(System.currentTimeMillis()); }

    private static int intOf(Object v) { return ItemPmRepository.intOf(v); }
}
