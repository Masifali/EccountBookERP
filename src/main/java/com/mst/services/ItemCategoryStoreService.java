package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ItemCategoryStoreDto;
import com.mst.repositories.ItemCategoryStoreRepository;
import com.mst.repositories.ItemStoreMasterSupport;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.ItemStoreMasterSupport.convToBool;
import static com.mst.repositories.ItemStoreMasterSupport.convToInt;
import static com.mst.repositories.ItemStoreMasterSupport.convToString;
import static com.mst.repositories.ItemStoreMasterSupport.desktopContains;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Store Management (ModuleId 24) — screen 336 "Item Category Store", ScreenName
 * {@code ItemCategoryStore}. No DocumentTypeId (a master, not a document).
 * Page /store/item-category-store, API /api/store/item-category-store.
 *
 * <p>WHICH DESKTOP FORM. dbo.ScreenDefinition row 336 (GoldenAceDb dump) has TargetUrl
 * {@code Architecture.WinApp.Inventory_Definition.InvDeffrmItemCatagory} — the SAME class as the
 * Inventory screen 112 "Define Item Category". CommonServices.OpenDynamicallyScreen:348 creates it
 * with {@code Activator.CreateInstance(type, userAccount)} and sets {@code form.Tag = "ItemCategoryStore"};
 * no public field is set, so {@code ParentCategoryId} stays 0. The form never reads its Tag, so
 * screen 336 behaves exactly like screen 112. {@code Architecture.WinApp.StoreManagement.frmItemCatagoryStore}
 * (whose FormTypeId 2 would show "Item Category Store" with parent 8 / class group 5) is NOT what
 * this menu entry opens — it is only opened from AddItemStore.cs:1481 — and is not ported here.
 *
 * <p>Files: InvDeffrmItemCatagory.cs; BLL 0584 ItemCategory, 0583 Item.InventoryParentCategories,
 * 0648 COAAllocation.GetLst, 0639 ChartofAccount.ReadAllAccountgroup; DAL 0437 ItemCategory.SetData,
 * 0205 CommonServices.GetERPFeaturesByCompanyId; Model 1046 ItemCategory;
 * WinApp.Helper GlobalVariables_Helper.GetConfigValueFromGlobal; WinApp.Common DropDownBind.BindDDL.
 *
 * <p>DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 * <ol>
 *   <li>Id lists are filtered with {@code string.Contains} (:363, :389): parent categories whose
 *       Id is a substring of "1,2,3,4,6,10,11" (so 0 would also pass), class groups of "1,2,3".</li>
 *   <li>The Production Stage / Variety Nature checks use {@code (ParentCategoryId != 5 || ParentCategoryId != 9)}
 *       (:714, :720) — always true — so both are always mandatory.</li>
 *   <li>A missing CGS account reports "Please Select Revenue Account" (:704).</li>
 *   <li>Leaving the Parent Category combo ALWAYS regenerates the code (CmbItemParentCategory_Leave:1212),
 *       whether or not ItemCodingEnable is on and also while editing an existing category —
 *       a typed or stored code is overwritten.</li>
 *   <li>Update sends a fresh model (Insert:741): accumulatedDepreciationAcId, DepreciationExpenseAcId,
 *       CapitalWipAcId, ExpenseMaintenanceAccountId, usefullLifeInMonths, depriciationrate,
 *       AssetCategoryId, depreciationMethodScheduleId are all 0 and overwrite the stored values
 *       (Sp_ItemCategory_Update then re-finds the Asset.Category row by ItemCategoryId).</li>
 *   <li>CategoryCode and CategoryDescription are saved untrimmed (:766); the "0" check on Code is
 *       on the untrimmed text (:666).</li>
 *   <li>Serial From/To are Conversion.ToInt of the text: a non-numeric text passes validation
 *       (only "" / "0" are rejected) and is saved as 0.</li>
 *   <li>Reset (New / after save, :1050) clears code, description, serials, the three accounts, class
 *       group and parent category, but keeps Production Stage, Variety Nature and Is Active; with
 *       ItemCodingEnable on it then generates the code for parent 0.</li>
 *   <li>AttributesForItemFeature is never assigned (always false): the Attributes panel never shows
 *       and no CategoryAttribute rows are sent; DAL SetData still runs
 *       [item].[USP_ItemAttribute_InsertAndUpdateByCategory] when ERP feature 13 is on.</li>
 *   <li>No rights are checked anywhere in the form (no SetRightsValueInRightsObject call); there is
 *       no Delete and no Print.</li>
 *   <li>History (gridfill:898) keeps the previous rows when the procedure returns none.</li>
 *   <li>Refresh (:1122) re-reads configurations and every list; Parent Category and Class Group are
 *       re-activated on their first row, the other combos keep their value.</li>
 *   <li>The auto-COA account lists (RevenueAcFill:473) cache the 3rd-level accounts for the life of
 *       the form (LoadThirdLevelAccounts:572); on the web every lookup reads them afresh.</li>
 * </ol>
 *
 * <p>DEVIATIONS (web only)
 * <ol>
 *   <li>Organization/company/users come from the session. ReadById (which has no company filter in
 *       the procedure) and Update are allowed only for a category of the user's company whose parent
 *       category is in this screen's history set (1,2,3,4,6,10,11) — what the desktop can open.</li>
 *   <li>Every combo value is re-checked against the list the server builds (LimitToList parity); a
 *       value outside the list is treated as "not selected" and gets the desktop's message. The
 *       account-type checks (:759) read AccountTypeId from those server lists; a Revenue/Inventory
 *       value missing from them fails like the desktop's null SelectedRow.</li>
 *   <li>With ItemCodingEnable on the code box is disabled on the desktop; the server does not trust
 *       it: insert sends GenerateCode(parent) (Sp_ItemCategory_Insert regenerates it anyway when
 *       configuration 410 is on); update keeps the stored code unless the page sends exactly the
 *       code GenerateCode(parent) returns now (the Leave regeneration of note 4).</li>
 *   <li>The Yes/No confirmation is asked on the page before the request.</li>
 *   <li>Save/Update require the Save/Update right of this ScreenName (the desktop form checks no
 *       rights); the page hides Save/Update when the right is missing.</li>
 *   <li>Every read error stops the whole lookup call (the desktop shows one MessageBox per failed
 *       list and continues).</li>
 * </ol>
 *
 * <p>NOT PORTED: Multi Lingo (ItemCategoryMultiLanguage popup), saved grid layouts (ctrlGrdBar),
 * keyboard shortcuts, the fixed-asset (ParentCategoryId 5) and ParentCategoryId 9 variants (never
 * reached from this menu entry), the Attributes grid (never visible, note 9).
 */
@Service
public class ItemCategoryStoreService {

    public static final String SCREEN_NAME = "ItemCategoryStore";
    public static final int SCREEN_ID = 336;

    /** Constructor defaults (:149-150); ParentCategoryId stays 0 so :166 / :177 never apply. */
    static final String PARENT_CATEGORY_IDS = "1,2,3,4,6,10,11";
    static final String CLASS_GROUP_IDS = "1,2,3";

    private final ItemCategoryStoreRepository repo;
    private final ItemStoreMasterSupport support;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public ItemCategoryStoreService(ItemCategoryStoreRepository repo, ItemStoreMasterSupport support,
                                    StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.support = support;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ================================================================================ load

    /** InvDeffrmItemCatagory_Load:159 (and btnRefresh_Click:1122 — the page re-applies what Refresh changes). */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));

        List<Map<String, Object>> parents = parentCategories();                         // ItemParentCategoryFill:346
        out.put("parentCategories", parents);
        out.put("classGroups", classGroups());                                          // GetItemClassGroup:380
        out.put("multiLanguage", support.erpFeature(u, 7));                             // :174-175

        Flags f = flags(u);                                                             // GetConfigurationsFromGlobal:291
        out.put("itemCodingEnable", f.itemCodingEnable);

        out.put("productionStages", repo.productionStages());                          // ItemProducitonStageBind:406
        out.put("varietyNatures", repo.varietyNatures());                              // ItemVarietyNatureBind:423
        out.put("history", history(u));                                                // gridfill:898

        Accounts a = accounts(u, f);                                                    // AccountTitleFill:329 + RevenueAcFill:473
        out.put("inventoryAccounts", a.inventory);
        out.put("revenueAccounts", a.revenue);
        out.put("cgsAccounts", a.cgs);

        /* :210-212 — the first parent row is active after ItemParentCategoryFill. */
        int firstParent = parents.isEmpty() ? 0 : toInt(parents.get(0).get("Id"));
        out.put("code", f.itemCodingEnable ? repo.generateCode(u, firstParent) : "");
        return out;
    }

    /** GenerateCode(Conversion.ToInt(CmbItemParentCategory.Value)) — Load, Reset and CmbItemParentCategory_Leave. */
    public Map<String, Object> generateCode(int parentCategoryId) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("code", repo.generateCode(u, parentCategoryId));
        return out;
    }

    /** gridfill:898 — the rows the page shows (same columns, same order). */
    public List<Map<String, Object>> history() {
        return history(ctx.requireAccountingUser());
    }

    /** grdfrm_DoubleClick:1032 → ReadById:852 (plus gridfill, which the page reloads). */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> c = owned(u, id);
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", toInt(c.get("Id")));
        o.put("CategoryCode", convToString(c.get("CategoryCode")));
        o.put("CategoryDescription", convToString(c.get("CategoryDescription")));
        o.put("SerialFrom", String.valueOf(toInt(c.get("SerialFrom"))));             // Conversion.ToString(int)
        o.put("SerialTo", String.valueOf(toInt(c.get("SerialTo"))));
        o.put("RevenueAccountId", toInt(c.get("RevenueAccountId")));
        o.put("CGSAccountId", toInt(c.get("CGSAccountId")));
        o.put("InventoryAccountId", toInt(c.get("InventoryAccountId")));
        o.put("InventoryParentCategoriesId", toInt(c.get("InventoryParentCategoriesId")));
        o.put("ItemClassGroupId", toInt(c.get("ItemClassGroupId")));
        o.put("ItemProductionStageId", toInt(c.get("ItemProductionStageId")));
        o.put("ItemVarietyNatureId", toInt(c.get("ItemVarietyNatureId")));
        o.put("CategoryStatus", convToBool(c.get("CategoryStatus")));
        o.put("history", history(u));
        return o;
    }

    // ================================================================================ save

    /**
     * btnsave_Click:827 (RecId = 0) / btnUpdate_Click:840 → Insert:729 → BLL Save → DAL SetData
     * (one transaction: Sp_ItemCategory_Insert|Update, then — ERP feature 13 —
     * [item].[USP_ItemAttribute_InsertAndUpdateByCategory]).
     */
    @Transactional
    public Map<String, Object> save(ItemCategoryStoreDto d) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = d.id == null ? 0 : d.id;
        /* Rights (reviewer, HIGH): the desktop form checks none; the web enforces the grant rows. */
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new IllegalArgumentException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new IllegalArgumentException("You do not have the Update right for this screen.");
        Map<String, Object> stored = recId > 0 ? owned(u, recId) : null;

        Flags f = flags(u);
        Accounts a = accounts(u, f);
        String code = d.categoryCode == null ? "" : d.categoryCode;
        String desc = d.categoryDescription == null ? "" : d.categoryDescription;
        String sFrom = d.serialFrom == null ? "" : d.serialFrom;
        String sTo = d.serialTo == null ? "" : d.serialTo;
        int revenueId = inList(a.revenue, nz(d.revenueAccountId));
        int inventoryId = inList(a.inventory, nz(d.inventoryAccountId));
        int cgsId = inList(a.cgs, nz(d.cgsAccountId));
        int parentId = inList(parentCategories(), nz(d.parentCategoryId));
        int classGroupId = inList(classGroups(), nz(d.classGroupId));
        int stageId = inList(repo.productionStages(), nz(d.productionStageId));
        int varietyId = inList(repo.varietyNatures(), nz(d.varietyNatureId));

        /* ItemCodingEnable: txtItemCategoryCode is disabled (:299) — deviation 3. */
        if (f.itemCodingEnable) {
            if (stored == null) {
                code = repo.generateCode(u, parentId);
            } else {
                String storedCode = convToString(stored.get("CategoryCode"));
                if (!code.equals(storedCode) && !code.equals(repo.generateCode(u, parentId))) code = storedCode;
            }
        }

        // ---- formvalidation:664, same order and texts
        if (code.trim().isEmpty() || code.equals("0")) throw new IllegalArgumentException("Please Insert Code");
        if (desc.trim().isEmpty() || desc.trim().equals("0")) throw new IllegalArgumentException("Please Insert Description");
        if (sFrom.trim().isEmpty() || sFrom.trim().equals("0")) throw new IllegalArgumentException("Please Insert Serial From");
        if (sTo.trim().isEmpty() || sTo.trim().equals("0")) throw new IllegalArgumentException("Please Insert Serial To");
        if (revenueId == 0) throw new IllegalArgumentException("Please Select Revenue Account");
        if (inventoryId == 0) throw new IllegalArgumentException("Please Select Inventory Account");
        if (cgsId == 0) throw new IllegalArgumentException("Please Select Revenue Account");      // sic, :704
        if (parentId == 0) throw new IllegalArgumentException("Please Select Parent Category");
        if (stageId == 0) throw new IllegalArgumentException("Please Select Production Stage");  // :714, always applies
        if (varietyId == 0) throw new IllegalArgumentException("Please Select Variety Nature");  // :720, always applies

        // ---- Insert:754-765
        if (f.autoCoaDefineByItemNameOnInsert && f.sameAccountForStockAndRevenue && inventoryId != revenueId) {
            throw new IllegalArgumentException("Revenue Account Should be Same as Inventory Account");
        }
        Integer revenueType = accountType(a.revenue, revenueId);
        Integer inventoryType = accountType(a.inventory, inventoryId);
        if (revenueType == null || inventoryType == null) {
            throw new IllegalArgumentException("Object reference not set to an instance of an object.");
        }
        if (revenueType == 4 && inventoryType == 4 && inventoryId != revenueId) {
            throw new IllegalArgumentException("Revenue Account Should be Same as Inventory Account or Select an Account Of Type Sale In Revenue Account");
        }

        // ---- the model, in Architecture.Model.Inventory.ItemCategory property order (SetProc)
        Timestamp now = new Timestamp(System.currentTimeMillis());           // BLL Save: EntryDate = ModifyDate = DateTime.Now
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("CategoryStatus", Boolean.TRUE.equals(d.categoryStatus));      // Conversion.ToBool(chkstatus.Checked)
        m.put("EntryDate", now);
        m.put("ModifyDate", now);
        m.put("CGSAccountId", cgsId);
        m.put("CompanyId", u.getCompanyId());
        m.put("EntryUser", u.getId());
        m.put("Id", recId);
        m.put("InventoryAccountId", inventoryId);
        m.put("ModifyUser", u.getId());
        m.put("OrganizationId", u.getOrganizationId());
        m.put("RevenueAccountId", revenueId);
        m.put("SerialFrom", convToInt(sFrom));
        m.put("SerialTo", convToInt(sTo));
        m.put("InventoryParentCategoriesId", parentId);
        m.put("accumulatedDepreciationAcId", 0);                            // set only when ParentCategoryId == 5 (:776)
        m.put("ExpenseMaintenanceAccountId", 0);
        m.put("DepreciationExpenseAcId", 0);
        m.put("CapitalWipAcId", 0);
        m.put("ItemClassGroupId", classGroupId);
        m.put("CategoryCode", code);
        m.put("CategoryDescription", desc);
        m.put("ItemProductionStageId", stageId);
        m.put("ItemVarietyNatureId", varietyId);
        m.put("usefullLifeInMonths", 0);
        m.put("depriciationrate", BigDecimal.ZERO);
        m.put("AssetCategoryId", 0);
        m.put("depreciationMethodScheduleId", 0);
        /* CategoryAttributeList / CategoryDepreciationScheduleList are virtual — not sent. */

        int num = repo.setProc(recId == 0 ? "Sp_ItemCategory_Insert" : "Sp_ItemCategory_Update", m);
        int categoryId = recId == 0 ? num : recId;                           // DAL: if (obj.Id == 0) obj.Id = num
        if (support.erpFeature(u, 13)) {
            /* obj.CategoryAttributeList is empty (AttributesForItemFeature is never true). */
            repo.itemAttributesByCategory(u, categoryId);
        }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", categoryId);
        out.put("message", recId > 0 ? "Record Update Successfully." : "Record Save Successfully.");
        return out;
    }

    // ============================================================================ internals

    private List<Map<String, Object>> parentCategories() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : support.inventoryParentCategories()) {
            if (desktopContains(PARENT_CATEGORY_IDS, r.get("Id"))) out.add(r);
        }
        return out;
    }

    private List<Map<String, Object>> classGroups() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.classGroups()) {
            if (desktopContains(CLASS_GROUP_IDS, r.get("Id"))) out.add(r);
        }
        return out;
    }

    private List<Map<String, Object>> history(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, PARENT_CATEGORY_IDS)) {
            Map<String, Object> o = new LinkedHashMap<>();                   // gridfill:915-936
            o.put("Id", toInt(r.get("Id")));
            o.put("CategoryCode", str(r.get("categoryCode")));
            o.put("CategoryDescription", str(r.get("CategoryDescription")));
            o.put("SerialFrom", toInt(r.get("SerialFrom")));
            o.put("SerialTo", toInt(r.get("SerialTo")));
            o.put("InventoryParentCategoriesId", toInt(r.get("InventoryParentCategoriesId")));
            o.put("CategoryStatus", str(r.get("CategoryStatus")));
            o.put("ParentCategory", str(r.get("InvParentCateDescription")));
            o.put("CGSAccountId", toInt(r.get("CGSAccountId")));
            o.put("CGSAccountTitle", str(r.get("CGSAccountTitle")));
            o.put("RevenueAccountId", toInt(r.get("RevenueAccountId")));
            o.put("RevenueAccountTitle", str(r.get("RevenueAccountTitle")));
            o.put("InventoryAccountId", toInt(r.get("InventoryAccountId")));
            o.put("InventoryAccountTitle", str(r.get("InventoryAccountTitle")));
            o.put("ClassGroupName", str(r.get("ClassGroupName")));
            o.put("ProductionStage", str(r.get("productionStageName")));
            o.put("VarietyNature", str(r.get("VarietyNatureName")));
            out.add(o);
        }
        return out;
    }

    /** Deviation 1: what this screen can open — own company, parent in the history set. */
    private Map<String, Object> owned(UserAccount u, int id) {
        Map<String, Object> c = repo.readById(id);
        if (c == null
                || toInt(c.get("OrganizationId")) != nz(u.getOrganizationId())
                || toInt(c.get("CompanyId")) != nz(u.getCompanyId())
                || !historySet().contains(toInt(c.get("InventoryParentCategoriesId")))) {
            throw new IllegalArgumentException("Record not found.");
        }
        return c;
    }

    /** fnSplitString(@ParentCategoryIds, ',') as FormHistory applies it (an exact list). */
    private static Set<Integer> historySet() {
        Set<Integer> s = new HashSet<>();
        for (String p : PARENT_CATEGORY_IDS.split(",")) s.add(Integer.parseInt(p.trim()));
        return s;
    }

    static final class Flags {
        boolean itemCodingEnable, allAccountAllowOn, sameAccountForStockAndRevenue, autoCoaDefineByItemNameOnInsert;
    }

    /** GetConfigurationsFromGlobal:291 — Conversion.ToBool of each ConfigKey. */
    private Flags flags(UserAccount u) {
        Flags f = new Flags();
        f.itemCodingEnable = convToBool(support.config(u, "ItemCodingEnable"));
        f.allAccountAllowOn = convToBool(support.config(u, "All Account Allow on Item and Item Category"));
        f.sameAccountForStockAndRevenue = convToBool(support.config(u, "SameAccountForStockAndRevenue"));
        f.autoCoaDefineByItemNameOnInsert = convToBool(support.config(u, "AutoCoaDefineByItemNameOnInsert"));
        return f;
    }

    static final class Accounts {
        List<Map<String, Object>> inventory = new ArrayList<>(), revenue = new ArrayList<>(), cgs = new ArrayList<>();
    }

    /**
     * AccountTitleFill:329 + RevenueAcFill:473 (with LoadThirdLevelAccounts:564, BindCGSAccounts:585,
     * HandleDefaultCase:605, BindDefaultCaseAccounts:631). A list the desktop never binds is empty.
     */
    private Accounts accounts(UserAccount u, Flags f) {
        Accounts a = new Accounts();
        List<Map<String, Object>> lst = repo.coaAllocationList(u);            // Accountslst
        if (lst.isEmpty()) return a;                                          // :477
        if (f.autoCoaDefineByItemNameOnInsert) {
            List<Map<String, Object>> third = repo.thirdLevelAccounts(u, ctx.currentFinancialYearId());
            if (third.isEmpty()) return a;                                    // :484 — CGS not bound either
            if (f.allAccountAllowOn) {                                        // :488
                for (Map<String, Object> r : third) {
                    int t = toInt(r.get("AccountTypeId"));
                    if (t == 4) a.inventory.add(r);
                    if (f.sameAccountForStockAndRevenue && t == 4) a.revenue.add(r);
                    else if (!f.sameAccountForStockAndRevenue && (t == 4 || t == 10)) a.revenue.add(r);
                }
            } else {                                                          // :513
                List<Map<String, Object>> inv = new ArrayList<>(), rev = new ArrayList<>();
                for (Map<String, Object> r : third) {
                    int t = toInt(r.get("AccountTypeId"));
                    if (t == 4) inv.add(r);
                    else if (t == 10) rev.add(r);
                }
                if (f.sameAccountForStockAndRevenue) {
                    a.inventory = inv;
                    a.revenue = new ArrayList<>(inv);
                } else {
                    if (!inv.isEmpty()) a.inventory = inv;
                    if (!rev.isEmpty()) a.revenue = rev;
                }
            }
            a.cgs = cgs(lst);                                                 // :551
        } else {                                                              // HandleDefaultCase:605
            List<Map<String, Object>> dt = new ArrayList<>(), sale = new ArrayList<>();
            for (Map<String, Object> r : lst) {
                int t = toInt(r.get("AccountTypeId"));
                if (f.allAccountAllowOn) {
                    if (t == 4 || t == 10) dt.add(r);
                } else if (t == 4) {
                    dt.add(r);
                } else if (t == 10) {
                    sale.add(r);
                }
            }
            if (f.allAccountAllowOn) {                                        // BindDefaultCaseAccounts:633
                if (!dt.isEmpty()) {
                    a.inventory = dt;
                    a.revenue = new ArrayList<>(dt);
                }
            } else {
                if (!dt.isEmpty()) a.inventory = dt;
                if (!sale.isEmpty()) a.revenue = sale;
            }
            a.cgs = cgs(lst);                                                 // :628
        }
        return a;
    }

    /** BindCGSAccounts:585 — Accountslst rows of AccountTypeId 12. */
    private static List<Map<String, Object>> cgs(List<Map<String, Object>> lst) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : lst) if (toInt(r.get("AccountTypeId")) == 12) out.add(r);
        return out;
    }

    private static Integer accountType(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(r.get("Id")) == id) return toInt(r.get("AccountTypeId"));
        return null;
    }

    /** LimitToList parity (deviation 2): the value when it is in the list, else 0 ("not selected"). */
    private static int inList(List<Map<String, Object>> rows, int id) {
        if (id == 0) return 0;
        for (Map<String, Object> r : rows) if (toInt(r.get("Id")) == id) return id;
        return 0;
    }

    private static int nz(Integer v) { return v == null ? 0 : v; }
}
