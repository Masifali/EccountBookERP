package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ItemTypeStoreDto;
import com.mst.repositories.ItemStoreMasterSupport;
import com.mst.repositories.ItemTypeStoreRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.ItemStoreMasterSupport.convToBool;
import static com.mst.repositories.ItemStoreMasterSupport.convToString;
import static com.mst.repositories.ItemStoreMasterSupport.desktopContains;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Store Management (ModuleId 24) — screen 337 "Item Type Store", ScreenName {@code ItemTypeStore}.
 * No DocumentTypeId (a master). Page /store/item-type-store, API /api/store/item-type-store.
 *
 * <p>WHICH DESKTOP FORM. dbo.ScreenDefinition row 337 has TargetUrl
 * {@code Architecture.WinApp.Inventory_Definition.InvDeffrmItemType} — the same class as Inventory
 * screen 113 "Define Item Type". CommonServices.OpenDynamicallyScreen:348 only sets
 * {@code form.Tag = "ItemTypeStore"}; {@code FormTypeId} stays 0, so TypeIds = "15,16,18" and
 * ParentCategories = "1,2,4,6,10,11" (Load:113-117). The store variant FormTypeId 2
 * (TypeIds "17", parent "8") exists in the class but is not what this menu entry opens.
 *
 * <p>Files: InvDeffrmItemType.cs; BLL 0593 ItemType, 0577 InvLookUp.GetLookupsByTypeIdDt,
 * 0583 Item.InventoryParentCategories; DAL 0446 ItemType.SetData; Model 1056 ItemType;
 * WinApp.Helper GlobalVariables_Helper.GetConfigValueFromGlobal.
 *
 * <p>DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 * <ol>
 *   <li>Every id filter is {@code string.Contains} (:240, :298, :509): Type lookups whose Id is a
 *       substring of "15,16,18" (so 1, 5, 6, 8 also pass), parent categories of "1,2,4,6,10,11"
 *       (0 would pass), history rows whose Type is a substring of "15,16,18" — a NULL Type
 *       ("" after Conversion.ToString) is always shown.</li>
 *   <li>The Code / Description "0" checks are on the untrimmed text (:157, :164); the values are
 *       saved trimmed (:430-431).</li>
 *   <li>Reset (New / after save, :333) clears Code and Description only; Type, Parent Category and
 *       IsMother keep their values.</li>
 *   <li>Update sends a fresh model (:417): PostState false, PostUser 0, PostDate NULL (the CLR null
 *       is omitted, the procedure default NULL is written) — the stored posting fields are
 *       overwritten. Sp_ItemType_Update has no organization/company filter.</li>
 *   <li>No rights are checked in the form; there is no Delete and no Print.</li>
 *   <li>History (gridfill:481) keeps the previous rows when the procedure returns none.</li>
 *   <li>Refresh (toolStripButton1_Click:394) re-binds both combos, keeping a value still in the list
 *       and blanking one that is not.</li>
 *   <li>Messages "Record Save Successfully.." / "Record Update Successfully.." (two dots, :442-446).</li>
 * </ol>
 *
 * <p>DEVIATIONS (web only)
 * <ol>
 *   <li>Organization/company/users from the session. Open (ReadById has no company filter) and
 *       Update are allowed only for a type of the user's company that this screen's history shows
 *       (parent in 1,2,4,6,10,11 and Type passing note 1).</li>
 *   <li>Type and Parent Category are re-checked against the server's lists (LimitToList parity); a
 *       value outside them gets "Please Select Type" / "Please Select Parent Category".</li>
 *   <li>ItemCodingEnable: the code box is disabled on the desktop; the server does not trust it —
 *       insert sends GenerateCode() (Sp_ItemType_Insert regenerates it anyway when configuration 410
 *       is on), update sends the stored code.</li>
 *   <li>The Yes/No confirmation is asked on the page before the request.</li>
 *   <li>Save/Update require the Save/Update right of this ScreenName (the desktop form checks no
 *       rights); the page hides Save/Update when the right is missing.</li>
 * </ol>
 *
 * <p>NOT PORTED: Multi Lingo (ItemTypeMultiLanguage popup), saved grid layouts (ctrlGrdBar2),
 * keyboard shortcuts.
 */
@Service
public class ItemTypeStoreService {

    public static final String SCREEN_NAME = "ItemTypeStore";
    public static final int SCREEN_ID = 337;

    /** FormTypeId 0 (:113-117). */
    static final String TYPE_IDS = "15,16,18";
    static final String PARENT_CATEGORIES = "1,2,4,6,10,11";

    private final ItemTypeStoreRepository repo;
    private final ItemStoreMasterSupport support;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public ItemTypeStoreService(ItemTypeStoreRepository repo, ItemStoreMasterSupport support,
                                StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.support = support;
        this.rights = rights;
        this.ctx = ctx;
    }

    /** InvDeffrmItemType_Load:105 (and Refresh:394 — the page applies the keep-or-blank rule). */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));
        boolean coding = convToBool(support.config(u, "ItemCodingEnable"));          // GetConfigurationsFromGlobal:186
        out.put("itemCodingEnable", coding);
        out.put("multiLanguage", support.erpFeature(u, 7));                           // :133-134
        out.put("parentCategories", parentCategories());                               // ItemParentCategoryFill:219
        out.put("types", types(u));                                                    // CombTypeFill:275
        out.put("code", coding ? repo.generateCode(u) : "");                           // :137-139
        out.put("history", history(u));                                                // gridfill:481
        return out;
    }

    /** GenerateCode:199 — Reset with ItemCodingEnable on. */
    public Map<String, Object> generateCode() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("code", repo.generateCode(u));
        return out;
    }

    public List<Map<String, Object>> history() {
        return history(ctx.requireAccountingUser());
    }

    /** grdfrm_DoubleClick:538 → ItemType.GetByID. */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> t = owned(u, id);
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", toInt(t.get("Id")));
        o.put("TypeCode", convToString(t.get("TypeCode")));
        o.put("TypeDescription", convToString(t.get("TypeDescription")));
        o.put("Type", toInt(t.get("Type")));
        o.put("ParentCategoryId", toInt(t.get("ParentCategoryId")));
        o.put("IsMother", convToBool(t.get("IsMother")));
        return o;
    }

    /**
     * btnsave_Click:456 (RecId = 0) / btnUpdate_Click:469 → Insert:407 → BLL Save → DAL 0446 SetData:
     * one transaction, Sp_ItemType_Insert | Sp_ItemType_Update.
     */
    @Transactional
    public Map<String, Object> save(ItemTypeStoreDto d) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = d.id == null ? 0 : d.id;
        /* Rights (reviewer, HIGH): the desktop form checks none; the web enforces the grant rows. */
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new IllegalArgumentException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new IllegalArgumentException("You do not have the Update right for this screen.");
        Map<String, Object> stored = recId > 0 ? owned(u, recId) : null;
        boolean coding = convToBool(support.config(u, "ItemCodingEnable"));

        String code = d.typeCode == null ? "" : d.typeCode;
        String desc = d.typeDescription == null ? "" : d.typeDescription;
        if (coding) code = stored == null ? repo.generateCode(u) : convToString(stored.get("TypeCode")); // deviation 3
        int type = inList(types(u), d.type == null ? 0 : d.type);
        int parent = inList(parentCategories(), d.parentCategoryId == null ? 0 : d.parentCategoryId);

        // ---- formvalidation:155
        if (code.isEmpty() || code.equals("0")) throw new IllegalArgumentException("Please Insert Code");
        if (desc.isEmpty() || desc.equals("0")) throw new IllegalArgumentException("Please Insert Description");
        if (type == 0) throw new IllegalArgumentException("Please Select Type");
        if (parent == 0) throw new IllegalArgumentException("Please Select Parent Category");

        // ---- Architecture.Model.Inventory.ItemType, property order (GenericProvider.SetProc)
        Timestamp now = new Timestamp(System.currentTimeMillis());   // BLL Save: EntryDate = ModifyDate = DateTime.Now
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("PostState", false);
        m.put("IsMother", Boolean.TRUE.equals(d.isMother));
        m.put("EntryDate", now);
        m.put("ModifyDate", now);
        m.put("PostDate", null);                                     // DateTime? null — omitted
        m.put("CompanyId", u.getCompanyId());
        m.put("EntryUser", u.getId());
        m.put("Id", recId);
        m.put("ModifyUser", u.getId());
        m.put("OrganizationId", u.getOrganizationId());
        m.put("PostUser", 0);
        m.put("Type", type);
        m.put("ParentCategoryId", parent);
        m.put("TypeCode", code.trim());
        m.put("TypeDescription", desc.trim());
        int num = repo.setProc(recId == 0 ? "Sp_ItemType_Insert" : "Sp_ItemType_Update", m);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", recId == 0 ? num : recId);
        out.put("message", recId > 0 ? "Record Update Successfully.." : "Record Save Successfully..");
        return out;
    }

    // ============================================================================ internals

    private List<Map<String, Object>> parentCategories() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : support.inventoryParentCategories()) {
            if (desktopContains(PARENT_CATEGORIES, r.get("Id"))) {
                Map<String, Object> o = new LinkedHashMap<>();                 // dtParent: Id, ParentCategory
                o.put("Id", toInt(r.get("Id")));
                o.put("ParentCategory", str(r.get("InvParentCateDescription")));
                out.add(o);
            }
        }
        return out;
    }

    private List<Map<String, Object>> types(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.lookupsByType(u, 6)) {
            if (desktopContains(TYPE_IDS, r.get("Id"))) {
                Map<String, Object> o = new LinkedHashMap<>();                 // dttype: Id, ItemType
                o.put("Id", toInt(r.get("Id")));
                o.put("ItemType", str(r.get("LookupName")));
                out.add(o);
            }
        }
        return out;
    }

    private List<Map<String, Object>> history(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.getAll(u, PARENT_CATEGORIES)) {
            if (!desktopContains(TYPE_IDS, r.get("Type"))) continue;           // :509
            Map<String, Object> o = new LinkedHashMap<>();                     // :500-511
            o.put("Id", toInt(r.get("Id")));
            o.put("Code", str(r.get("TypeCode")));
            o.put("Description", str(r.get("TypeDescription")));
            o.put("Type", str(r.get("LookupName")));
            o.put("ParentCategory", str(r.get("InvParentCateDescription")));
            o.put("MotherStatus", str(r.get("MotherStatus")));
            out.add(o);
        }
        return out;
    }

    /** Deviation 1. */
    private Map<String, Object> owned(UserAccount u, int id) {
        Map<String, Object> t = repo.readById(id);
        if (t == null
                || toInt(t.get("OrganizationId")) != nz(u.getOrganizationId())
                || toInt(t.get("CompanyId")) != nz(u.getCompanyId())
                || !parentSet().contains(toInt(t.get("ParentCategoryId")))
                || !desktopContains(TYPE_IDS, t.get("Type"))) {
            throw new IllegalArgumentException("Record not found.");
        }
        return t;
    }

    /** fnSplitString(@ParentCategoryIds, ',') — the procedure's exact list. */
    private static Set<Integer> parentSet() {
        Set<Integer> s = new HashSet<>();
        for (String p : PARENT_CATEGORIES.split(",")) s.add(Integer.parseInt(p.trim()));
        return s;
    }

    private static int inList(List<Map<String, Object>> rows, int id) {
        if (id == 0) return 0;
        for (Map<String, Object> r : rows) if (toInt(r.get("Id")) == id) return id;
        return 0;
    }

    private static int nz(Integer v) { return v == null ? 0 : v; }
}
