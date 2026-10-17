package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.Inv4cDefineItemRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.Inv4cDefineItemRepository.map;
import static com.mst.repositories.Inv4cDefineItemRepository.now;
import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Inventory hub — screens 777 "Define Item" (module 65 Mfg Definitions) and 776 "Item Definition_Engr" (module 8), both
 * Architecture.WinApp.Inventory_Definition.frmAddItem_Engr (base.Name "frmAddItem_Engr"), and screen 93 "Define Item Concrete",
 * frmAddItemConcrete (base.Name "frmAddItemConcrete"). The two forms are the same code apart from a handful of fields
 * ({@link Variant}); line refs ":NNN" are frmAddItem_Engr.cs unless marked "C:" (frmAddItemConcrete.cs).
 *
 * <pre>
 *   InvDefrmAddItem_Load :337        rights, "All Account Allow on Item and Item Category" config, every combo fill,
 *                                    CompaniesBindInGrid :1029, AmountCaluculation :851, rdWithVariant checked (Engr)
 *   cmbItemCategory_Leave :974       Item.GenerateCode → txtItemCode/txtItemCodeNew; GetGLAccountbyItemCategoryId → the 3 GL combos
 *                                    (only while btnsave is visible and enabled); any error → "Record Not Found"
 *   Insert :1260                     FormValidationForAddItem :1079 → "Are you sure to Save/Update?" → Item model → allocation grid
 *                                    → Item.Save(items, true) → "Receord Save/Update Successfully " + items.Id → refresh :1150
 *   ReadById :1456                   Item.GetByID → the form; cmbItemCategory read-only; btnupdate shown
 *   grdfrmfill :1586                 Item.FormHistory(NoOfRecords, ScreenName, CanViewAllRecord/EntryUser) — 50 on the History tab, all on Load All / Search
 *   btnUomUpdate_Click :1752         Item.UpdateItemName per checked row (Ctrl+Shift+F10 shows the group)
 * </pre>
 *
 * DESKTOP BEHAVIOUR REPRODUCED:
 * <ol>
 *  <li>grdfrmfill sets reports.Id from the history category combo (:1598) but BLL 0583 FormHistory never sends @Id, so the
 *      "Filter By Category" combo filters nothing. The page sends the id; the server ignores it, as the BLL does.</li>
 *  <li>Item.Save does not copy the new id back into items.Id (DAL 0436 :143), so a new item reports
 *      "Receord Save Successfully 0"; an update reports the record id.</li>
 *  <li>Cost / Purchase rates, Max Stock Level, Reorder Level go through Conversion.ToInt — a decimal text becomes 0 (:1341-1346);
 *      WholeSalePrice is Conversion.ToDouble of the Purchase Rate text (:1338). The Cost/Purchase/Sale UOM combos are never filled
 *      (CostUOMFill :931 has no caller) — their TEXT is converted with ToInt (:1345-1348).</li>
 *  <li>Sp_Item_Update returns 0, so on an update the DAL writes none of the child schedules (DAL 0436 :144).</li>
 *  <li>Allocation grid: with one company the row is taken whatever its tick (:1383); otherwise only ticked rows, and
 *      "Please select Location first" when none is ticked (:1406). "Grid Record Not Found" when the grid is empty.</li>
 *  <li>Engr: WeightKgs and MaxMeaurement are forced to 0, CommOnSale/CommOnPurchase false, ItemGroupId 0, BaseCropYear "".
 *      Concrete: WeightKgs / MaxMeaurement / MaxMeaurementUnitId / ItemQcGradeId from the form, CommOnSale/Purchase from the two checks.</li>
 *  <li>refresh :1150 after a save leaves Item Category / Type / Class / Mother Item / Modal as they were (only btnnew clears them)
 *      and re-runs cmbItemCategory_Leave, so the next code is generated at once — done in the page.</li>
 * </ol>
 * DEVIATIONS: attachments and the two item pictures are not ported (Pic1/Pic2 keep the stored values on update, "" on insert);
 * ids posted for combos must be rows of the signed-in company's lists; an item can only be opened/updated by its own company.
 */
@Service
public class Inv4cDefineItemService {

    public enum Variant {
        ENGR("frmAddItem_Engr"),
        CONCRETE("frmAddItemConcrete");
        public final String screen;
        Variant(String screen) { this.screen = screen; }
    }

    private final Inv4cDefineItemRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public Inv4cDefineItemService(Inv4cDefineItemRepository repo, StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ================================================================================= lookups

    /** InvDefrmAddItem_Load :337 / toolStripButton1_Click (Refresh) :1227 — every combo of the form. */
    public Map<String, Object> lookups(Variant v) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(v.screen));
        boolean allAccounts = Inv4cDefineItemRepository.truthy(repo.config(u, "All Account Allow on Item and Item Category"));   // :356
        out.put("allAccountAllow", allAccounts);
        out.put("itemTypes", pick(repo.itemTypes(u), "Id", "TypeDescription"));                       // ItemTypeFill :491
        List<Map<String, Object>> classes = new ArrayList<>();                                        // ItemClassFill :521 — 1, 6, 7, 8
        for (Map<String, Object> r : repo.itemClasses()) {
            int id = toInt(ci(r, "ClassId"));
            if (id == 1 || id == 6 || id == 7 || id == 8) classes.add(map("Id", id, "Name", str(ci(r, "ClassDescription"))));
        }
        out.put("itemClasses", classes);
        /* PurchaseGLAcFill :572 — type 4 / 10 / 12 split, or one list of 4+10+12 when the config is on */
        List<Map<String, Object>> purchase = new ArrayList<>(), sale = new ArrayList<>(), cgs = new ArrayList<>(), all = new ArrayList<>();
        for (Map<String, Object> r : repo.accounts(u)) {
            int type = toInt(ci(r, "AccountTypeId"));
            Map<String, Object> o = map("Id", toInt(ci(r, "Id")), "Name", str(ci(r, "AccountTitle")));
            if (type == 4) purchase.add(o);
            if (type == 10) sale.add(o);
            if (type == 12) cgs.add(o);
            if (type == 4 || type == 10 || type == 12) all.add(o);
        }
        out.put("purchaseAccounts", allAccounts ? all : purchase);
        out.put("saleAccounts", allAccounts ? all : sale);
        out.put("cgsAccounts", allAccounts ? all : cgs);
        out.put("suppliers", pick(repo.suppliers(u), "Id", "CompanyName"));                           // SupplierBuyerfill :899
        out.put("countries", pick(repo.countries(u), "Id", "Description"));                           // cmbcountryfill :822
        out.put("categories", pick(repo.categories(u), "Id", "CategoryDescription"));                 // ItemCatagoryHistoryFill :382
        if (v == Variant.ENGR) {
            out.put("itemModels", pick(repo.itemModels(u), "itemModelId", "modalName"));             // ItemModalFill :719
            out.put("motherItems", pick(repo.motherItems(u), "Id", "MotherItem"));                   // getMotherItems :869
        } else {
            out.put("qcGrades", pick(repo.qcGrades(u), "Id", "QcGradeDesc"));                        // C: ItemQcGradeFill
        }
        List<Map<String, Object>> taxes = new ArrayList<>();                                          // CmbTaxTypeFill :770 — Type != 1
        for (Map<String, Object> r : repo.taxTypes(u)) {
            if (toInt(ci(r, "Type")) != 1) taxes.add(map("Id", toInt(ci(r, "Id")), "Name", str(ci(r, "TaxName"))));
        }
        out.put("taxTypes", taxes);
        List<Map<String, Object>> uoms = new ArrayList<>();                                           // BaseUnitFill :440 — Id, UOMCode, Equivalent
        for (Map<String, Object> r : repo.uoms(u)) {
            uoms.add(map("Id", toInt(ci(r, "Id")), "Name", str(ci(r, "UOMCode")), "Equivalent", toDouble(ci(r, "Equivalent"))));
        }
        out.put("uoms", uoms);
        out.put("companies", pick(repo.companies(u), "Id", "CompName"));                             // CompaniesBindInGrid :1029
        return out;
    }

    /** cmbItemCategory_Leave :974 — GenerateCode + GetGLAccountbyItemCategoryId; any failure is "Record Not Found". */
    public Map<String, Object> categoryLeave(Variant v, int itemCategoryId) {
        UserAccount u = ctx.requireAccountingUser();
        try {
            Map<String, Object> out = new LinkedHashMap<>();
            List<Map<String, Object>> code = repo.generateCode(u, itemCategoryId);
            if (!code.isEmpty()) {
                out.put("ItemCode", str(ci(code.get(0), "ItemCode")));
                out.put("ItemCodeNew", str(ci(code.get(0), "ItemCodeNew")));
            }
            List<Map<String, Object>> gl = repo.glByCategory(u, itemCategoryId);
            if (!gl.isEmpty()) {
                out.put("InventoryAccountId", toInt(ci(gl.get(0), "InventoryAccountId")));
                out.put("RevenueAccountId", toInt(ci(gl.get(0), "RevenueAccountId")));
                out.put("CGSAccountId", toInt(ci(gl.get(0), "CGSAccountId")));
            }
            return out;
        } catch (RuntimeException e) {
            throw new IllegalArgumentException("Record Not Found");                                   // :1011
        }
    }

    // ================================================================================= history

    /** grdfrmfill :1586 — the dt columns of each form, in its order. */
    public List<Map<String, Object>> history(Variant v, int noOfRecords) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(v.screen, "viewAll");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, noOfRecords, v.screen, viewAll)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            if (v == Variant.ENGR) {                                                                  // :1610-1636
                o.put("ItemParentCategory", str(ci(r, "InvParentCateDescription")));
                o.put("ItemCategory", str(ci(r, "CategoryDescription")));
                o.put("ItemType", str(ci(r, "TypeDescription")));
                o.put("MotherItem", str(ci(r, "MotherItem")));
                o.put("ItemClass", str(ci(r, "ClassGroupName")));
                o.put("ItemCode", str(ci(r, "ItemCode")));
                o.put("ItemCodeNew", str(ci(r, "ItemCodeNew")));
                o.put("ItemName", str(ci(r, "ItemName")));
                o.put("BaseUnit", str(ci(r, "BaseUnit")));
                o.put("WeightSemiFinishDye", toDouble(ci(r, "WeightSemiFinish")));
                o.put("WeightSemiFinishSand", toDouble(ci(r, "WeightSemiFinishSand")));
                o.put("WeightFinishGoods", toDouble(ci(r, "WeightFinishGoods")));
                o.put("ItemModal", str(ci(r, "ModalName")));
                o.put("Active", str(ci(r, "ItemStatus")));
                o.put("StockAc", str(ci(r, "GlStockAccountTitle")));
                o.put("SaleAc", str(ci(r, "GlSaleAccountTitle")));
                o.put("CgsAc", str(ci(r, "GlCgsAccountTitle")));
                o.put("HasVariant", str(ci(r, "ItemWithVarient")));
                o.put("EntryDate", day(ci(r, "EntryDate")));
                o.put("EntryUser", str(ci(r, "EntryUserName")));
                o.put("ModifyDate", day(ci(r, "ModifyDate")));
                o.put("ModifyUser", str(ci(r, "ModifyUserName")));
            } else {                                                                                  // C: grdfrmfill
                o.put("ItemCode", str(ci(r, "ItemCode")));
                o.put("ItemCodeNew", str(ci(r, "ItemCodeNew")));
                o.put("ItemName", str(ci(r, "ItemName")));
                o.put("ItemQcGrade", str(ci(r, "ItemQcGrade")));
                o.put("ItemType", str(ci(r, "TypeDescription")));
                o.put("ItemCategory", str(ci(r, "CategoryDescription")));
                o.put("ItemParentCategory", str(ci(r, "InvParentCateDescription")));
                o.put("ItemClass", str(ci(r, "ClassGroupName")));
                o.put("BaseUnit", str(ci(r, "BaseUnit")));
                o.put("WeightKgs", toDouble(ci(r, "WeightKgs")));
                o.put("MaxLength", toDouble(ci(r, "MaxMeaurement")));
                o.put("MaxLengthUnit", str(ci(r, "MaxUOMCode")));
                o.put("ItemStatus", str(ci(r, "ItemStatus")));
                o.put("StockAc", str(ci(r, "GlStockAccountTitle")));
                o.put("SaleAc", str(ci(r, "GlSaleAccountTitle")));
                o.put("CgsAc", str(ci(r, "GlCgsAccountTitle")));
                o.put("ItemWithVarient", str(ci(r, "ItemWithVarient")));
                o.put("EntryDate", day(ci(r, "EntryDate")));
                o.put("EntryUser", str(ci(r, "EntryUserName")));
            }
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    /** btnUomUpdate_Click :1752 — Item.UpdateItemName(ItemName, ItemCode, Id) per checked row. */
    @Transactional
    public Map<String, Object> updateItemNames(Variant v, List<Map<String, Object>> rows) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(v.screen, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        if (rows == null || rows.isEmpty()) throw new IllegalArgumentException("Please check row first");          // :1763
        for (Map<String, Object> r : rows) {
            int id = toInt(r.get("Id"));
            if (owned(u, id) == null) throw new IllegalArgumentException("Record Not Found");
            repo.updateItemName(id, str(r.get("ItemName")), str(r.get("ItemCode")));
        }
        return map("success", true, "message", "Record Update Successfully");                                       // :1775
    }

    // ==================================================================================== read

    /** ReadById :1456 — Item.GetByID. */
    public Map<String, Object> load(Variant v, int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> it = owned(u, id);
        if (it == null) return null;
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        for (String k : new String[] { "ItemCategoryId", "ItemCode", "ItemCodeNew", "ItemName", "MotherItemId", "WeightSemiFinish",
                "WeightSemiFinishSand", "WeightFinishGoods", "ItemTypeId", "ItemClassId", "BaseUnitId", "itemModalId", "PurchaseGLAC",
                "SaleGLAC", "COGSGLAC", "ApplyGST", "TaxTypeId", "ItemStatus", "ItemWithVarient", "ManufactureId", "ManufacturePartNo",
                "BuyerSupplierId", "BuyerPartNo", "ProductNo", "MaxStockLevel", "ReorderLevel", "ItemOriginId", "CostPrice",
                "UOMScheduleIdCostRate", "PurchasePrice", "UOMScheduleIdPurRate", "WholeSalePrice", "UOMScheduleIdWhsRate",
                "AmountCalcType", "ApplyVAT", "ApplyExciese", "OtherTax", "Pic1", "Pic2", "ItemQcGradeId", "WeightKgs", "MaxMeaurement",
                "MaxMeaurementUnitId", "CommOnPurchase", "CommOnSale" }) {
            Object val = ci(it, k);
            if (val instanceof BigDecimal) val = ((BigDecimal) val).doubleValue();
            o.put(k, val);
        }
        return o;
    }

    /** The item must exist and belong to the signed-in company (web-only tenancy check). */
    private Map<String, Object> owned(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> it = repo.getById(id);
        if (it == null) return null;
        if (ci(it, "CompanyId") != null && toInt(ci(it, "CompanyId")) != u.getCompanyId()) return null;
        if (ci(it, "OrganizationId") != null && toInt(ci(it, "OrganizationId")) != u.getOrganizationId()) return null;
        return it;
    }

    // ==================================================================================== save

    /** Insert :1260 up to the confirmation — FormValidationForAddItem :1079 (C: the Concrete order). */
    public Map<String, Object> precheck(Variant v, Map<String, Object> b) {
        ctx.requireAccountingUser();
        int recId = toInt(b.get("Id"));
        requireRight(v, recId);
        formValidation(v, b);
        return map("success", true, "update", recId > 0);
    }

    private void requireRight(Variant v, int recId) {
        if (recId == 0 && !rights.has(v.screen, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(v.screen, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
    }

    private static void formValidation(Variant v, Map<String, Object> b) {
        if (v == Variant.ENGR) {
            if (toInt(b.get("ItemCategoryId")) == 0) throw new IllegalArgumentException("Item Category Field is Required");
            if (toInt(b.get("ItemTypeId")) == 0) throw new IllegalArgumentException("Item Type Field is Required");
            if (toInt(b.get("MotherItemId")) == 0) throw new IllegalArgumentException("Mother Item Field is Required");
            if (toInt(b.get("ItemClassId")) == 0) throw new IllegalArgumentException("Item Class Field is Required");
            if (str(b.get("ItemCodeNew")).trim().isEmpty()) throw new IllegalArgumentException("Item Code Field is Required");
            if (str(b.get("ItemName")).trim().isEmpty()) throw new IllegalArgumentException("Item Name Field is Required");
            if (toInt(b.get("BaseUnitId")) == 0) throw new IllegalArgumentException("Base Unit Field is Required");
        } else {
            if (toInt(b.get("ItemCategoryId")) == 0) throw new IllegalArgumentException("Item Category Field is Required");
            if (str(b.get("ItemName")).trim().isEmpty()) throw new IllegalArgumentException("Item Name Field is Required");
            if (toInt(b.get("ItemQcGradeId")) == 0) throw new IllegalArgumentException("Quality Grade Field is Required");
            if (toInt(b.get("ItemTypeId")) == 0) throw new IllegalArgumentException("Item Type Field is Required");
            if (toInt(b.get("ItemClassId")) == 0) throw new IllegalArgumentException("Item Class Field is Required");
            if (toInt(b.get("BaseUnitId")) == 0) throw new IllegalArgumentException("Base Unit Field is Required");
            if (cDec(b.get("WeightKgs")).signum() == 0 || str(b.get("WeightKgs")).trim().isEmpty()) throw new IllegalArgumentException("Weight (Kgs) Field is Required");
            if (cDec(b.get("MaxMeaurement")).signum() == 0 || str(b.get("MaxMeaurement")).trim().isEmpty()) throw new IllegalArgumentException("Max Meaurement Field is Required");
            if (toInt(b.get("MaxMeaurementUnitId")) == 0) throw new IllegalArgumentException("Max Meaurement Unit Field is Required");
        }
        if (toInt(b.get("PurchaseGLAC")) == 0) throw new IllegalArgumentException("Purchase GL A/C Field is Required");
        if (toInt(b.get("SaleGLAC")) == 0) throw new IllegalArgumentException("Sale GL A/C Field is Required");
        if (toInt(b.get("COGSGLAC")) == 0) throw new IllegalArgumentException("COGS GL A/c Filed is Required");
        String variant = str(b.get("Variant"));                                       // "with" | "without" | ""
        if (!variant.equals("with") && !variant.equals("without")) {
            throw new IllegalArgumentException(v == Variant.ENGR ? "Please Select One Option 'Has Variant' Or 'No Variant'"
                    : "Please Select One Option With Or Without Variant");
        }
    }

    /** Insert :1291-1423 → BLL 0583 Save(items, true) → DAL 0436 SetData, one transaction. */
    @Transactional
    @SuppressWarnings("unchecked")
    public Map<String, Object> save(Variant v, Map<String, Object> b) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = toInt(b.get("Id"));
        requireRight(v, recId);
        formValidation(v, b);
        Map<String, Object> existing = null;
        if (recId > 0) {
            existing = owned(u, recId);
            if (existing == null) throw new IllegalArgumentException("Record Not Found");
        }
        Map<String, Object> look = lookups(v);
        int baseUnitId = toInt(b.get("BaseUnitId"));
        Map<String, Object> unit = find((List<Map<String, Object>>) look.get("uoms"), baseUnitId);
        if (unit == null) throw new IllegalArgumentException("Base Unit is not one of this company's units.");
        requireIn(look, "categories", toInt(b.get("ItemCategoryId")), "Item Category");
        requireIn(look, "itemTypes", toInt(b.get("ItemTypeId")), "Item Type");
        requireIn(look, "itemClasses", toInt(b.get("ItemClassId")), "Item Class");
        requireIn(look, "purchaseAccounts", toInt(b.get("PurchaseGLAC")), "Purchase GL A/C");
        requireIn(look, "saleAccounts", toInt(b.get("SaleGLAC")), "Sale GL A/C");
        requireIn(look, "cgsAccounts", toInt(b.get("COGSGLAC")), "COGS GL A/c");
        if (v == Variant.ENGR) {
            requireIn(look, "motherItems", toInt(b.get("MotherItemId")), "Mother Item");
            if (toInt(b.get("itemModalId")) != 0) requireIn(look, "itemModels", toInt(b.get("itemModalId")), "Item Modal");
        } else {
            requireIn(look, "qcGrades", toInt(b.get("ItemQcGradeId")), "Quality Grade");
            requireIn(look, "uoms", toInt(b.get("MaxMeaurementUnitId")), "Max Meaurement Unit");
        }
        if (toInt(b.get("ManufactureId")) != 0) requireIn(look, "suppliers", toInt(b.get("ManufactureId")), "Manufacturer");
        if (toInt(b.get("BuyerSupplierId")) != 0) requireIn(look, "suppliers", toInt(b.get("BuyerSupplierId")), "Buyer/Supplier");
        if (toInt(b.get("ItemOriginId")) != 0) requireIn(look, "countries", toInt(b.get("ItemOriginId")), "Item Region");
        boolean gst = bool(b.get("ApplyGST"));
        if (gst && toInt(b.get("TaxTypeId")) != 0) requireIn(look, "taxTypes", toInt(b.get("TaxTypeId")), "Tax Type");

        /* Model 1044 Item (:1291-1353), on top of the CLR defaults. */
        Map<String, Object> p = Inv4cDefineItemRepository.itemModel();
        p.put("Id", recId);
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("BranchesId", u.getBranchesId());
        p.put("EntryUser", u.getId());
        p.put("ModifyUser", u.getId());
        p.put("ItemCategoryId", toInt(b.get("ItemCategoryId")));
        p.put("ItemName", str(b.get("ItemName")).trim());
        p.put("ItemCode", str(b.get("ItemCode")).trim());
        p.put("ItemCodeNew", str(b.get("ItemCodeNew")).trim());
        p.put("ItemTypeId", toInt(b.get("ItemTypeId")));
        p.put("ItemClassId", toInt(b.get("ItemClassId")));
        p.put("BaseUnitId", baseUnitId);
        double equivalent = toDouble(unit.get("Equivalent"));                                              // :1304 SelectedRow.Cells[2]
        if (v == Variant.ENGR) {
            p.put("MotherItemId", toInt(b.get("MotherItemId")));
            p.put("WeightSemiFinish", cDouble(b.get("WeightSemiFinish")));
            p.put("WeightSemiFinishSand", cDouble(b.get("WeightSemiFinishSand")));
            p.put("WeightFinishGoods", cDouble(b.get("WeightFinishGoods")));
            p.put("WeightKgs", BigDecimal.ZERO);
            p.put("MaxMeaurement", BigDecimal.ZERO);
            p.put("itemModalId", toInt(b.get("itemModalId")));
            p.put("CommOnSale", false);
            p.put("CommOnPurchase", false);
        } else {
            p.put("ItemQcGradeId", toInt(b.get("ItemQcGradeId")));
            p.put("WeightKgs", cDec(b.get("WeightKgs")));
            p.put("MaxMeaurement", cDec(b.get("MaxMeaurement")));
            p.put("MaxMeaurementUnitId", toInt(b.get("MaxMeaurementUnitId")));
            p.put("CommOnSale", bool(b.get("CommOnSale")));
            p.put("CommOnPurchase", bool(b.get("CommOnPurchase")));
        }
        p.put("PurchaseGLAC", toInt(b.get("PurchaseGLAC")));
        p.put("SaleGLAC", toInt(b.get("SaleGLAC")));
        p.put("COGSGLAC", toInt(b.get("COGSGLAC")));
        p.put("ApplyGST", gst);                                                                               // :1350 (also :1314)
        p.put("TaxTypeId", gst ? toInt(b.get("TaxTypeId")) : 0);
        p.put("ItemWithVarient", "with".equals(str(b.get("Variant"))));
        p.put("ItemStatus", bool(b.get("ItemStatus")));
        p.put("Pic1", existing == null ? "" : str(ci(existing, "Pic1")));                                    // pictures not ported
        p.put("Pic2", existing == null ? "" : str(ci(existing, "Pic2")));
        p.put("ManufactureId", toInt(b.get("ManufactureId")));
        p.put("ManufacturePartNo", str(b.get("ManufacturePartNo")).trim());
        p.put("BuyerSupplierId", toInt(b.get("BuyerSupplierId")));
        p.put("BuyerPartNo", str(b.get("BuyerPartNo")).trim());
        p.put("ProductNo", str(b.get("ProductNo")).trim());
        p.put("ItemGroupId", 0);
        p.put("WholeSalePrice", cDouble(b.get("PurchaseRate")));                                              // :1338 txtPurchaseRate
        p.put("BaseCropYear", "");
        p.put("MaxStockLevel", (double) cInt(b.get("MaxStockLevel")));
        p.put("ReorderLevel", (double) cInt(b.get("ReorderLevel")));
        p.put("ItemOriginId", toInt(b.get("ItemOriginId")));
        p.put("CostPrice", (double) cInt(b.get("CostPrice")));
        p.put("UOMScheduleIdCostRate", cInt(b.get("CostUOMText")));
        p.put("PurchasePrice", (double) cInt(b.get("PurchaseRate")));
        p.put("UOMScheduleIdPurRate", cInt(b.get("PurchaseUOMText")));
        p.put("UOMScheduleIdWhsRate", cInt(b.get("SaleUOMText")));
        p.put("AmountCalcType", str(b.get("AmountCalcType")));
        p.put("ApplyVAT", bool(b.get("ApplyVAT")));
        p.put("ApplyExciese", bool(b.get("ApplyExciese")));
        p.put("OtherTax", bool(b.get("OtherTax")));
        /* BLL 0583 Save :19-29 */
        Timestamp now = now();
        p.put("EntryDate", now);
        p.put("ModifyDate", now);
        p.put("PostDate", now);
        p.put("PostState", false);
        if (recId != 0) { p.put("CommOnPurchase", false); p.put("CommOnSale", false); }

        /* grdAllocation :1375-1409 → ItemAllocation list */
        List<Map<String, Object>> grid = b.get("companies") instanceof List ? (List<Map<String, Object>>) b.get("companies") : new ArrayList<>();
        if (grid.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        List<Map<String, Object>> companies = (List<Map<String, Object>>) look.get("companies");
        List<Map<String, Object>> allocations = new ArrayList<>();
        int countFalse = 0;
        for (Map<String, Object> r : grid) {
            int companyId = toInt(r.get("Id"));
            if (find(companies, companyId) == null) throw new IllegalArgumentException("Company is not one of this organization's companies.");
            boolean ticked = bool(r.get("Value"));
            if (grid.size() == 1) {
                allocations.add(map("Id", 0, "ItemId", 0, "OrganizationId", u.getOrganizationId(), "CompanyId", companyId, "BranchId", u.getBranchesId(), "IsActive", true));
            } else if (ticked) {
                allocations.add(map("Id", 0, "ItemId", 0, "OrganizationId", u.getOrganizationId(), "CompanyId", companyId, "BranchId", u.getBranchesId(), "IsActive", true));
            } else {
                countFalse++;
            }
        }
        if (grid.size() > 1 && grid.size() == countFalse) throw new IllegalArgumentException("Please select Location first");

        int id = persist(u, p, equivalent, allocations, recId == 0);
        int reported = recId > 0 ? recId : 0;                                                                   // note 2
        return map("success", true, "message", (recId > 0 ? "Receord Update Successfully " : "Receord Save Successfully ") + reported, "id", id);
    }

    /**
     * DAL 0436 SetData(obj, Sp_Item_Insert|Update, AutoCoaInsertIsOn = true): the AutoCoa branch, the item procedure, then
     * (only when the procedure returned an id — Sp_Item_Update returns 0) tax schedule, UOM schedule, allocations with their
     * price / reorder / commission / feed-price rows.
     */
    private int persist(UserAccount u, Map<String, Object> p, double equivalent, List<Map<String, Object>> allocations, boolean insert) {
        boolean autoCoa = Inv4cDefineItemRepository.truthy(repo.config(u, "AutoCoaDefineByItemNameOnInsert"));
        boolean sameAccount = Inv4cDefineItemRepository.truthy(repo.config(u, "SameAccountForStockAndRevenue"));
        int categoryId = toInt(p.get("ItemCategoryId"));
        if (repo.parentCategoryId(u, categoryId) != 9 && insert && autoCoa) {                                   // :36
            int stockId, saleId;
            Map<String, Object> stock = repo.accountsInfoByCategory(categoryId, "Stock");
            if (sameAccount) {
                stockId = saveCoa(u, stock, p, allocations, "Stock A/c");
                saleId = stockId;
            } else {
                Map<String, Object> sale = repo.accountsInfoByCategory(categoryId, "Sale");
                int num3 = stock == null ? 0 : toInt(ci(stock, "ParentCodeId"));
                int num4 = sale == null ? 0 : toInt(ci(sale, "ParentCodeId"));
                if (num3 == num4) {
                    stockId = saveCoa(u, stock, p, allocations, "Stock A/c");
                    saleId = stockId;
                } else {
                    stockId = saveCoa(u, stock, p, allocations, "Stock A/c");
                    saleId = saveCoa(u, sale, p, allocations, "Sale A/c");
                }
            }
            p.put("PurchaseGLAC", stockId);
            p.put("SaleGLAC", saleId);
            if (stockId == 0) throw new IllegalArgumentException("PurchaseGLAC  Account Not Found");
            if (saleId == 0) throw new IllegalArgumentException("SaleGLAC Account Not Found");
            if (sameAccount && stockId != saleId) throw new IllegalArgumentException("PurchaseGLAC And SaleGLAC Account should be same when Configuration 'SameAccountForStockAndRevenue' is on");
        }
        int num5 = repo.exec(insert ? "dbo.Sp_Item_Insert" : "dbo.Sp_Item_Update", p, 0);                       // :143
        if (num5 > 0) {
            Timestamp now = now();
            int entryUser = toInt(p.get("EntryUser"));
            if (bool(p.get("ApplyGST")) && toInt(p.get("TaxTypeId")) > 0) {                                      // :146
                repo.exec("dbo.Sp_ItemTaxSchedule_Insert", map("Id", 0, "ItemId", num5, "IsActive", true, "EffectedDate", now, "EntryDate", now,
                        "ModifyDate", now, "EntryUser", entryUser, "ModifyUser", 0, "OrganizationId", u.getOrganizationId(),
                        "CompanyId", u.getCompanyId(), "TaxTypeId", toInt(p.get("TaxTypeId"))), 0);
            }
            int itemClassId = toInt(p.get("ItemClassId"));
            if (toInt(p.get("ItemGroupId")) > 0) {                                                                // :160 — never (ItemGroupId = 0)
                for (Map<String, Object> row : repo.uomSchedulesOfGroup(toInt(p.get("ItemGroupId")))) {
                    uomSchedule(u, num5, itemClassId, toInt(ci(row, "UOMId")), toDouble(ci(row, "Equivalent")), toDouble(ci(row, "QtyEquivalent")), entryUser);
                }
            } else {
                uomSchedule(u, num5, itemClassId, toInt(p.get("BaseUnitId")), equivalent, 1d, entryUser);           // :203
            }
            if (allocations.isEmpty()) throw new IllegalArgumentException("Item Allocation List Empty");           // :228
            for (Map<String, Object> a : allocations) {
                a.put("ItemId", num5);
                int company = toInt(a.get("CompanyId"));
                repo.exec("dbo.Sp_ItemAllocation_Insert", a, 0);
                double[] prices = { toDouble(p.get("CostPrice")), toDouble(p.get("PurchasePrice")), toDouble(p.get("WholeSalePrice")), toDouble(p.get("RetailPrice")) };
                for (int n = 0; n < prices.length; n++) {                                                         // :234-292 PriceTypeId 4..7
                    if (prices[n] <= 0d) continue;
                    Map<String, Object> ps = Inv4cDefineItemRepository.priceModel();
                    ps.put("ItemId", num5); ps.put("EffectedDate", now); ps.put("EntryDate", now); ps.put("ModifyDate", now);
                    ps.put("EntryUser", entryUser); ps.put("OrganizationId", u.getOrganizationId()); ps.put("CompanyId", company);
                    ps.put("BranchesId", u.getBranchesId()); ps.put("ItemPrice", prices[n]); ps.put("PriceTypeId", n + 4);
                    repo.exec("dbo.Sp_ItemPricingSchedule_Insert", ps, 0);
                }
                if (toDouble(p.get("ReorderLevel")) > 0d) {                                                       // :294
                    Map<String, Object> rs = Inv4cDefineItemRepository.reorderModel();
                    rs.put("ItemId", num5); rs.put("EffectiveDate", now); rs.put("EntryDate", now); rs.put("ModifyDate", now);
                    rs.put("EntryUser", entryUser); rs.put("OrganizationId", u.getOrganizationId()); rs.put("CompanyId", company);
                    rs.put("BranchesId", u.getBranchesId()); rs.put("MinQty", p.get("MinStockLevel")); rs.put("MaxQty", p.get("MaxStockLevel"));
                    rs.put("OptimalQty", p.get("OptimalStockLevel")); rs.put("ReOrderPoint", p.get("ReorderLevel"));
                    repo.exec("dbo.Sp_ItemsReorderSchedule_Insert", rs, 0);
                }
                if (bool(p.get("CommOnSale")) || bool(p.get("CommOnPurchase"))) {                                 // :311
                    repo.exec("[dbo].[USP_ItemWiseCommission_InsertOrUpdate]", map("CompanyId", company, "OrgnizationId", u.getOrganizationId(),
                            "BranchesId", u.getBranchesId(), "EntryUserId", entryUser, "ModifyUserId", toInt(p.get("ModifyUser")),
                            "ModifyDate", now, "EntryDate", now, "Id", 0, "ItemId", num5, "OnSale", bool(p.get("CommOnSale")),
                            "OnPurchase", bool(p.get("CommOnPurchase")), "EffectedFromDate", financialYearStart(u)), 0);
                }
                Map<String, Object> fp = Inv4cDefineItemRepository.feedPriceModel();                             // :328-345
                fp.put("ItemId", num5); fp.put("EffectiveDate", new Timestamp(System.currentTimeMillis() - 7L * 24 * 60 * 60 * 1000));
                fp.put("ItemRate", 1d); fp.put("PricingCustomGroupId", 1); fp.put("IsActive", true); fp.put("DocumentTypeId", 1);
                fp.put("EntryDate", now); fp.put("ModifyDate", now); fp.put("EntryUserId", entryUser); fp.put("ModifyUserId", toInt(p.get("ModifyUser")));
                fp.put("IsApproved", false); fp.put("ApprovedUserId", 0); fp.put("ApprovedDate", now); fp.put("FinancialYearId", 0);
                fp.put("OrganizationId", u.getOrganizationId()); fp.put("CompanyId", company);
                repo.exec("[fed].[usp_ItemPricingSchedule_Insert]", fp, 0);
            }
            return num5;
        }
        return toInt(p.get("Id"));                                                                                 // :349 num5 = obj.Id
    }

    /** DAL 0436 :203-225 / :178-200 — one Sp_UOMSchedule_Insert. */
    private void uomSchedule(UserAccount u, int itemId, int itemClassId, int unitId, double equivalent, double qtyEquivalent, int entryUser) {
        Timestamp now = now();
        boolean baseRate = (itemClassId == 8 && equivalent == 1000d) || equivalent == 40d;
        repo.exec("dbo.Sp_UOMSchedule_Insert", map("Id", 0, "ItemId", itemId, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "EntryUser", entryUser, "ModifyUser", 0, "EntryDate", now, "ModifyDate", now, "ScheduleUnitId", unitId, "Equivalent", equivalent,
                "QtyEquivalent", qtyEquivalent, "BaseRateUom", baseRate, "BasePackUom", false, "BaseSecondaryUom", false, "Active", true), 0);
    }

    /** DAL 0436 SaveCoa :606-724 — ChartofAccount "Detail" level 4 + a COAAllocation and opening-balance row per allocation. */
    private int saveCoa(UserAccount u, Map<String, Object> parent, Map<String, Object> item, List<Map<String, Object>> allocations, String description) {
        boolean feature17 = repo.erpFeature(u, 17);
        Map<String, Object> coa = Inv4cDefineItemRepository.accountModel();
        String parentCode = parent == null ? "" : str(ci(parent, "ParentAccountCode"));
        coa.put("ParentAccountCode", parentCode);
        coa.put("AccountClass", parent == null ? 0 : toInt(ci(parent, "AccountClass")));
        coa.put("ParentCodeId", parent == null ? 0 : toInt(ci(parent, "ParentCodeId")));
        coa.put("Account_Level", 4);
        String title = str(item.get("ItemName")) + " " + description;
        coa.put("AccountTitle", title);
        coa.put("AccountGroup", "Detail");
        coa.put("AccountTypeId", parent == null ? 0 : toInt(ci(parent, "AccountTypeId")));
        coa.put("PLNoteId", parent == null ? 0 : toInt(ci(parent, "PLNoteId")));
        coa.put("BSNoteId", parent == null ? 0 : toInt(ci(parent, "BSNoteId")));
        coa.put("OtherErpCode", str(item.get("ItemCode")));
        coa.put("OrganizationId", u.getOrganizationId());
        coa.put("CompanyId", u.getCompanyId());
        coa.put("BranchId", u.getBranchesId());
        coa.put("EntryUser", toInt(item.get("EntryUser")));
        coa.put("IsActive", true);
        int fy = ctx.currentFinancialYearId();
        coa.put("FinancialYearId", fy);
        if (allocations.isEmpty()) throw new IllegalArgumentException("Item Allocation Grid Record Not Found");    // :638
        if (!parentCode.isEmpty()) coa.put("AccountCode", parentCode);
        if (parentCode.isEmpty()) coa.put("ParentAccountCode", "0");
        coa.put("ActionId", 1);
        Timestamp now = now();
        coa.put("EntryDate", now); coa.put("ModifyDate", now); coa.put("PostDate", now);
        int num = repo.exec("dbo.Proc_ChartofAccount_Insert", coa, 0);
        if (num > 0) {
            Set<Integer> seen = new LinkedHashSet<>();
            for (Map<String, Object> a : allocations) {
                int company = toInt(a.get("CompanyId"));
                if (!seen.add(company)) continue;
                Map<String, Object> al = Inv4cDefineItemRepository.accountAllocationModel();
                al.put("CompanyId", company); al.put("IsActive", true); al.put("BranchId", u.getBranchesId()); al.put("ChartofAccountId", num);
                repo.exec("dbo.Sp_COAAllocation_Insert", al, 0);
                Map<String, Object> ob = Inv4cDefineItemRepository.accountOpeningModel();
                ob.put("OrganizationId", u.getOrganizationId()); ob.put("CompanyId", company); ob.put("EntryUser", toInt(item.get("EntryUser")));
                ob.put("ModifyUser", 0); ob.put("EntryDate", now); ob.put("ModifyDate", now); ob.put("PostDate", now); ob.put("PostState", false);
                ob.put("ChartOfAccountId", num); ob.put("ChartOfAccountTitle", title); ob.put("FinancialYearId", fy);
                ob.put("YearObDebit", 0d); ob.put("YearObCredit", 0d);                                             // feature 17 or CLR 0 — both 0
                repo.exec("dbo.Sp_AccountsOpeningBalances_Insert", ob, 0);
            }
        }
        return num;
    }

    private Timestamp financialYearStart(UserAccount u) {
        try {
            List<Map<String, Object>> years = repo.query("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId());
            for (Map<String, Object> r : years) {
                if (toInt(ci(r, "Id")) == ctx.currentFinancialYearId() && ci(r, "Start_Period") instanceof Timestamp) return (Timestamp) ci(r, "Start_Period");
            }
        } catch (RuntimeException ignore) { /* fall through */ }
        return now();
    }

    // ================================================================================= helpers

    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String idKey, String nameKey) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(map("Id", toInt(ci(r, idKey)), "Name", str(ci(r, nameKey))));
        return out;
    }
    @SuppressWarnings("unchecked")
    private static void requireIn(Map<String, Object> look, String key, int id, String label) {
        if (find((List<Map<String, Object>>) look.get(key), id) == null) throw new IllegalArgumentException(label + " is not one of this company's records.");
    }
    private static Map<String, Object> find(List<Map<String, Object>> rows, int id) {
        if (rows == null) return null;
        for (Map<String, Object> r : rows) if (toInt(r.get("Id")) == id) return r;
        return null;
    }
    private static boolean bool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = str(v).trim().toLowerCase();
        return s.equals("true") || s.equals("1");
    }
    /** Conversion.ToInt(text) — Convert.ToInt32: whole numbers only, anything else (blank, "12.5") is 0. */
    static int cInt(Object v) {
        if (v instanceof Number) { double d = ((Number) v).doubleValue(); return d == Math.rint(d) ? (int) d : 0; }
        String s = str(v).trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s.replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }
    /** Conversion.ToDouble(text) — 0 when blank or not numeric. */
    static double cDouble(Object v) {
        if (v instanceof Number) return ((Number) v).doubleValue();
        String s = str(v).trim().replace(",", "");
        if (s.isEmpty()) return 0d;
        try { double d = Double.parseDouble(s); return Double.isInfinite(d) ? 0d : d; } catch (NumberFormatException e) { return 0d; }
    }
    /** Conversion.ToDecimal(text) — 0 when blank or not numeric. */
    static BigDecimal cDec(Object v) {
        if (v instanceof BigDecimal) return (BigDecimal) v;
        if (v instanceof Number) return BigDecimal.valueOf(((Number) v).doubleValue());
        String s = str(v).trim().replace(",", "");
        if (s.isEmpty()) return BigDecimal.ZERO;
        try { return new BigDecimal(s); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }
    static String day(Object v) {
        if (v == null) return null;
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        String s = String.valueOf(v);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }
}
