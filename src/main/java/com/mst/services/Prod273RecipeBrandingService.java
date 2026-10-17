package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.Prod273RecipeBrandingRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.Prod273Support.blank;
import static com.mst.services.Prod273Support.ci;
import static com.mst.services.Prod273Support.day;
import static com.mst.services.Prod273Support.list;
import static com.mst.services.Prod273Support.ok;
import static com.mst.services.Prod273Support.project;
import static com.mst.services.Prod273Support.stamp;
import static com.mst.services.Prod273Support.str;
import static com.mst.services.Prod273Support.toDouble;
import static com.mst.services.Prod273Support.toInt;
import static com.mst.services.Prod273Support.truthy;

/**
 * Production hub (module 18) — screen 285 "Receipe (Branding)".
 *
 * <pre>
 *   Desktop form   Architecture.WinApp.Mfg.frmBillOfMaterial_Manufacturing (.cs, 5257 lines)
 *                  base.Name = "frmBillOfMaterial_Manufacturing" (the rights key)
 *   DocumentTypeId 1120 (:405, :1400, :2010, :2201)
 *   Route          /production/recipe-branding      API /production/recipe-branding/api
 *   Print          CommonServices.Recipe_Slip1120(id) → 1120_Recipe_Slip.rpt (:1878)
 *   BLL / DAL      BomHeader (fed schema) — Save / ReadById / FormHistory / DeleteByID / GenerateCode / GetDataForDropDownFromBOM
 * </pre>
 *
 * DESKTOP BEHAVIOUR REPRODUCED:
 * <ol>
 *  <li>Insert():1353 order — FormValidation ("doc No Field Required", "DocDate cannot be greater than Effected Date",
 *      "Recipe No Field Required", "Item Field Required", "IteUom Field Required"), "Grid Record Not Found...",
 *      confirm, then Input rows → Consumption rows → OverHead rows with the desktop's texts and row numbers.</li>
 *  <li>An Input / Consumption row with ItemId 0 is skipped (the "+" row the grid always keeps, :1420 / :1512); an
 *      OverHead row is sent only when its account is set (:1600).</li>
 *  <li>A UOM id of 0 is resolved from Sp_UOMSchedule_GetAllMethod 'ReadByItemID' by Equivalent (:1440, :1473, :1537,
 *      :1571) — "UOM in Input Grid Row No: n Is not Define Against Item X. Please Check..." when none matches.</li>
 *  <li>Input Rate / RateUom / Amount are sent only when Rate &gt; 0 (:1468); Consumption always.</li>
 *  <li>Removed Input / Consumption rows are sent with ActionTypeId 3 (:1502, :1590); OverHead rows are not tracked.</li>
 *  <li>BomType is "Finish Goods", ProjectsId = BranchesId, ApprovedUserId / ApprovedDate are set while IsApproved
 *      stays false (:1391-1412).</li>
 *  <li>Save / Update messages "Data Save Successfully....  n" / "Data Update Successfully....  n" (:1644, :1675);
 *      Delete "Delete Record Successfully" (:1823).</li>
 *  <li>getAvgRateByItemId(item, DocDate) on an item pick and on "Generate AvgRate" (:734, :3255).</li>
 * </ol>
 *
 * DEVIATIONS (web-only, with reason):
 * <ol>
 *  <li>Tenancy: open / update / delete only a 1120 header of the signed-in company.</li>
 *  <li>Attachments (Attachment dialog, DMS copy) are not ported; AttachmentsList is left empty.</li>
 *  <li>CommonServices.getAvgRateByItemId is not in the recovered source; it is read as
 *      Sp_GetAvgRatesAndStockInHand_GetAllMethod 'GetOnlyAvgRateForCGS' @ItemId @DocDate (the same BLL method
 *      every other rate lookup of this form family uses).</li>
 *  <li>The grid's F1 lookups (GrdPopUp for Item / UOM / Rate UOM) are in-cell pickers on the page.</li>
 * </ol>
 */
@Service
public class Prod273RecipeBrandingService {

    public static final String SCREEN = "frmBillOfMaterial_Manufacturing";
    public static final int DOC_TYPE = 1120;
    private static final Set<Integer> OVERHEAD_ACCOUNT_TYPES = new HashSet<>(Arrays.asList(3, 6, 8));   // AccountTitleFill:499

    private final Prod273Support db;
    private final Prod273RecipeBrandingRepository repo;

    public Prod273RecipeBrandingService(Prod273Support db, Prod273RecipeBrandingRepository repo) {
        this.db = db;
        this.repo = repo;
    }

    // ====================================================================================== load

    /** frmBillOfMaterial_Manufacturing_Load:307. */
    public Map<String, Object> lookups() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", db.rights(SCREEN));
        out.put("branchId", db.branchId(u));
        out.put("rateDecimals", db.rateDecimals(u));
        out.put("amountDecimals", amountDecimals(u));
        out.put("financialYearStart", db.financialYearStart(u, db.financialYearId()));
        out.put("items", project(db.itemsGetAll(u), "Id", "ItemName"));                     // ItemFill:422
        out.put("consumptionItems", db.itemsByType(u, "14,17"));                            // ConsumptionItems:462
        out.put("allUoms", db.allUoms(u));                                                   // uomgetAll:479
        out.put("accounts", accounts(u));                                                    // AccountTitleFill:491
        out.put("dateTypes", Prod273Support.literal(Prod273Support.DATE_TYPES, "Parameters"));   // Datetypefill:1966
        out.putAll(historyCombos());                                                         // AllCombobind:1886
        out.put("docNo", repo.generateCode(u, db.branchId(u), DOC_TYPE, db.financialYearId()));   // DocumentNoFill:400
        return out;
    }

    /** btnRefresh_Click:1320 — ItemFill, ConsumptionItems, uomgetAll, AccountTitleFill. */
    public Map<String, Object> refresh() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", project(db.itemsGetAll(u), "Id", "ItemName"));
        out.put("consumptionItems", db.itemsByType(u, "14,17"));
        out.put("allUoms", db.allUoms(u));
        out.put("accounts", accounts(u));
        return out;
    }

    public int docNo() {
        UserAccount u = db.user();
        return repo.generateCode(u, db.branchId(u), DOC_TYPE, db.financialYearId());
    }

    /** clsGlobalVariables.DefaultNoofDecimalPointsForAmount (grdOverHead_CellUpdated:1215). */
    private int amountDecimals(UserAccount u) {
        int n = toInt(db.config(u, "Default NoofDecimal Points For Amount"));
        return n >= 0 && n <= 6 ? n : 2;
    }

    private List<Map<String, Object>> accounts(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.coaAllocation(u)) {
            if (!OVERHEAD_ACCOUNT_TYPES.contains(toInt(ci(r, "AccountTypeId")))) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "AccountTitle")));
            out.add(o);
        }
        return out;
    }

    /** AllCombobind:1886 — Activity "BOMName" → CmbBomNoHistory, "ItemHeader" → cmbItemhistory. */
    public Map<String, Object> historyCombos() {
        UserAccount u = db.user();
        List<Map<String, Object>> bom = new ArrayList<>(), items = new ArrayList<>();
        for (Map<String, Object> r : repo.dropDowns(u)) {
            String a = str(ci(r, "Activity")).trim();
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "ReferenceName")));
            if ("BOMName".equals(a)) bom.add(o);
            else if ("ItemHeader".equals(a)) items.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("bomNames", bom);
        out.put("headerItems", items);
        return out;
    }

    /** CommonServices.getAvgRateByItemId(itemId, DocDate) — DEVIATION 3. */
    public double avgRate(int itemId, String docDate) {
        UserAccount u = db.user();
        if (itemId <= 0) return 0d;
        return db.avgRateOnlyForCgs(u, itemId, Prod273Support.pickerValue(docDate), 0, 0, 0, 0, null, 0, 0);
    }

    // ====================================================================================== read

    /** ReadById:1714 — header + the three grids in dt column names. */
    public Map<String, Object> load(int id) {
        UserAccount u = db.user();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) return null;
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> head = new LinkedHashMap<>();
        head.put("Id", toInt(ci(h, "Id")));
        head.put("DocDate", day(ci(h, "DocDate")));
        head.put("DocNo", toInt(ci(h, "DocNo")));
        head.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        head.put("ItemId", toInt(ci(h, "ItemId")));
        head.put("ItemUomId", toInt(ci(h, "ItemUomId")));
        head.put("BomName", str(ci(h, "BomName")));
        head.put("EffectedDate", day(ci(h, "EffectedDate")));
        head.put("BomBatchQty", toDouble(ci(h, "BomBatchQty")));
        head.put("IsActive", truthy(ci(h, "IsActive")));
        head.put("IsDefault", truthy(ci(h, "IsDefault")));
        out.put("header", head);
        out.put("inputs", detailRows(repo.inputRows(id)));
        out.put("consumption", detailRows(repo.consumptionRows(id)));
        List<Map<String, Object>> foh = new ArrayList<>();
        for (Map<String, Object> r : repo.fohRows(id)) {                               // dtOverHead.Rows.Add :1777
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("OverHeadTypeId", toInt(ci(r, "OverHeadTypeId")));
            o.put("OverHeadAccountId", toInt(ci(r, "GLAccountId")));
            o.put("OverHeadAccount", str(ci(r, "OverHeadAccountTitle")));
            o.put("PercentOfTotalInput", toDouble(ci(r, "PercentOfTotalInput")));
            o.put("Qty", toDouble(ci(r, "Qty")));
            o.put("Rate", toDouble(ci(r, "Rate")));
            o.put("Amount", toDouble(ci(r, "Amount")));
            o.put("OhRemarks", str(ci(r, "OhRemarks")));
            foh.add(o);
        }
        out.put("overheads", foh);
        return out;
    }

    /** dtInput / dtConsumption.Rows.Add :1755 / :1766 — a RateUomId of 0 shows as "1KG" × 1 (:1750). */
    private static List<Map<String, Object>> detailRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            int rateUomId = toInt(ci(r, "RateUomId"));
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemUomId", toInt(ci(r, "ItemUomId")));
            o.put("Uom", toDouble(ci(r, "PackUomEquivalent")));
            o.put("UomCode", str(ci(r, "PackUom")));
            o.put("ItemQty", toDouble(ci(r, "ItemQty")));
            o.put("NetWeight", toDouble(ci(r, "WeightKg")));
            o.put("Rate", toDouble(ci(r, "Rate")));
            o.put("RateUomId", rateUomId);
            o.put("RateUom", rateUomId == 0 ? 1d : toDouble(ci(r, "RateUomEquivalent")));
            o.put("RateUomCode", rateUomId == 0 ? "1KG" : str(ci(r, "RateUom")));
            o.put("Amount", toDouble(ci(r, "Amount")));
            out.add(o);
        }
        return out;
    }

    // ====================================================================================== history

    /** HistoryGridFill:1998 — the 20 dt columns. */
    public List<Map<String, Object>> history(String from, String to, int fromDocNo, int toDocNo, int bomId, int itemId) {
        UserAccount u = db.user();
        boolean viewAll = db.has(SCREEN, "viewAll");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, db.branchId(u), db.financialYearId(), DOC_TYPE, fromDocNo, toDocNo,
                Prod273Support.pickerValue(from), Prod273Support.pickerValue(to), bomId, itemId, viewAll, u.getId())) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("RecordNo", toInt(ci(r, "RecordNo")));
            o.put("Id", toInt(ci(r, "Id")));
            o.put("DocumentTypeId", toInt(ci(r, "DocumentTypeId")));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("DocDate", day(ci(r, "DocDate")));
            o.put("BomName", str(ci(r, "BomName")));
            o.put("BomType", str(ci(r, "BomType")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("Uom", str(ci(r, "UOM")));
            o.put("BomBatchQty", toDouble(ci(r, "BomBatchQty")));
            o.put("EffectedDate", day(ci(r, "EffectedDate")));
            o.put("IsActive", truthy(ci(r, "IsActive")));
            o.put("IsDefault", truthy(ci(r, "IsDefault")));
            o.put("RemarksHeader", str(ci(r, "RemarksHeader")));
            o.put("EntryUser", str(ci(r, "EntryUserName")));
            o.put("EntryDate", stamp(ci(r, "EntryDate")));
            o.put("ModifyUser", str(ci(r, "ModifyUserName")));
            o.put("ModifyDate", stamp(ci(r, "ModifyDate")));
            o.put("IsApproved", truthy(ci(r, "IsApproved")));
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    public List<Map<String, Object>> attachments(int id) {
        db.user();
        return db.attachments(SCREEN, id);
    }

    // ====================================================================================== save

    private void requireRight(int recId) {
        if (recId == 0 && !db.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !db.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
    }

    /** FormValidation:593 + the grid test :1371. */
    private void preConfirm(Map<String, Object> b) {
        if (toInt(b.get("DocNo")) == 0) throw new IllegalArgumentException("doc No Field Required");
        String doc = str(b.get("DocDate")), eff = str(b.get("EffectedDate"));
        if (doc.length() >= 10 && eff.length() >= 10 && doc.substring(0, 10).compareTo(eff.substring(0, 10)) > 0)
            throw new IllegalArgumentException("DocDate cannot be greater than Effected Date");
        String bom = str(b.get("BomName")).trim();
        if (bom.isEmpty() || "0".equals(bom)) throw new IllegalArgumentException("Recipe No Field Required");
        if (toInt(b.get("ItemId")) == 0) throw new IllegalArgumentException("Item Field Required");
        if (toInt(b.get("ItemUomId")) == 0) throw new IllegalArgumentException("IteUom Field Required");
        if (list(b.get("inputs")).isEmpty()) throw new IllegalArgumentException("Grid Record Not Found...\nGrid (Input Material) Should have atleast one record");
    }

    public Map<String, Object> precheck(Map<String, Object> b) {
        UserAccount u = db.user();
        int recId = toInt(b.get("Id"));
        requireRight(recId);
        if (recId != 0 && ownedHeader(u, recId) == null) throw new IllegalArgumentException("Record Not Found");
        preConfirm(b);
        return ok("", 0, 0);
    }

    @Transactional
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = db.user();
        int recId = toInt(b.get("Id"));
        requireRight(recId);
        if (recId != 0 && ownedHeader(u, recId) == null) throw new IllegalArgumentException("Record Not Found");
        preConfirm(b);

        Timestamp now = Prod273Support.now();
        Map<String, Object> head = new LinkedHashMap<>();                                   // Insert():1389
        head.put("Id", recId);
        head.put("DocumentTypeId", DOC_TYPE);
        head.put("DocNo", toInt(b.get("DocNo")));
        head.put("DocDate", Prod273Support.pickerValue(b.get("DocDate")));
        head.put("BomName", str(b.get("BomName")));
        head.put("BomType", "Finish Goods");
        head.put("ItemId", toInt(b.get("ItemId")));
        head.put("ItemUomId", toInt(b.get("ItemUomId")));
        head.put("BomBatchQty", toDouble(b.get("BomBatchQty")));
        head.put("EffectedDate", Prod273Support.pickerValue(b.get("EffectedDate")));
        head.put("IsActive", truthy(b.get("IsActive")));
        head.put("IsDefault", truthy(b.get("IsDefault")));
        head.put("RemarksHeader", str(b.get("RemarksHeader")));
        head.put("OrganizationId", u.getOrganizationId());
        head.put("CompanyId", u.getCompanyId());
        head.put("BranchesId", db.branchId(u));
        head.put("ProjectsId", db.branchId(u));                                            // :1391 — the desktop's own value
        head.put("FinancialYearId", db.financialYearId());
        head.put("EntryUserId", u.getId());
        head.put("EntryDate", now);
        head.put("ModifyUserId", u.getId());
        head.put("ModifyDate", now);
        head.put("IsApproved", Boolean.FALSE);
        head.put("ApprovedUserId", u.getId());
        head.put("ApprovedDate", now);
        head.put("ActionId", recId == 0 ? 1 : 2);

        /* ---- Input rows :1417 ---- */
        List<Map<String, Object>> inputs = new ArrayList<>();
        int idx = 0;
        for (Map<String, Object> r : list(b.get("inputs"))) {
            idx++;
            int itemId = toInt(r.get("ItemId"));
            if (itemId == 0) throw new IllegalArgumentException("Item Field Required in Grid Input Row No: " + idx);   // :1499
            int id = recId > 0 ? toInt(r.get("Id")) : 0;
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", id);
            d.put("BomHeaderId", 0);
            d.put("ItemId", itemId);
            d.put("ItemUomId", resolveUom(u, itemId, toInt(r.get("ItemUomId")), toDouble(r.get("Uom")), "UOM in Input Grid Row No: " + idx, str(r.get("ItemName"))));
            if (toDouble(r.get("ItemQty")) == 0d) throw new IllegalArgumentException("ItemQty Field Required in Grid Input Row No: " + idx);
            d.put("ItemQty", toDouble(r.get("ItemQty")));
            if (blank(r.get("NetWeight"))) throw new IllegalArgumentException("NetWeight Field Required in Grid Input Row No: " + idx);
            d.put("WeightKg", toDouble(r.get("NetWeight")));
            if (toDouble(r.get("Rate")) > 0d) {                                                   // :1468
                d.put("Rate", toDouble(r.get("Rate")));
                d.put("RateUomId", resolveUom(u, itemId, toInt(r.get("RateUomId")), toDouble(r.get("RateUom")), "RateUom in Input Grid Row No: " + idx, str(r.get("ItemName"))));
                if (toDouble(r.get("Amount")) == 0d) throw new IllegalArgumentException("Amount Field Required in Grid Input Row No: " + idx);
                d.put("Amount", toDouble(r.get("Amount")));
            } else {
                d.put("Rate", 0d); d.put("RateUomId", 0); d.put("Amount", 0d);
            }
            d.put("ActionTypeId", id > 0 ? 2 : 1);
            inputs.add(d);
        }
        for (Map<String, Object> r : list(b.get("removedInputs"))) inputs.add(removedDetail(r));          // :1502

        /* ---- Consumption rows :1509 ---- */
        List<Map<String, Object>> consumption = new ArrayList<>();
        idx = 0;
        for (Map<String, Object> r : list(b.get("consumption"))) {
            idx++;
            int itemId = toInt(r.get("ItemId"));
            if (itemId == 0) continue;
            int id = recId > 0 ? toInt(r.get("Id")) : 0;
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", id);
            d.put("BomHeaderId", 0);
            d.put("ItemId", itemId);
            if (blank(r.get("ItemUomId"))) throw new IllegalArgumentException("UOM  Field Required in Grid Consumption Row No: " + idx);
            d.put("ItemUomId", resolveUom(u, itemId, 0, toDouble(r.get("Uom")), "UOM in Consumption Grid Row No: " + idx, str(r.get("ItemName"))));
            if (blank(r.get("ItemQty"))) throw new IllegalArgumentException("ItemQty Field Required in Grid Consumption Row No: " + idx);
            d.put("ItemQty", toDouble(r.get("ItemQty")));
            if (blank(r.get("NetWeight"))) throw new IllegalArgumentException("NetWeight Field Required in Grid Consumption Row No: " + idx);
            d.put("WeightKg", toDouble(r.get("NetWeight")));
            if (blank(r.get("Rate"))) throw new IllegalArgumentException("Rate Field Required in Grid Consumption Row No: " + idx);
            d.put("Rate", toDouble(r.get("Rate")));
            if (blank(r.get("RateUomId"))) throw new IllegalArgumentException("Rate UOM Field Required in Grid Consumption Row No: " + idx);
            d.put("RateUomId", resolveUom(u, itemId, 0, toDouble(r.get("RateUom")), "RateUom in Consumption Grid Row No: " + idx, str(r.get("ItemName"))));
            if (blank(r.get("Amount"))) throw new IllegalArgumentException("AmountField Required in Grid Consumption Row No: " + idx);
            d.put("Amount", toDouble(r.get("Amount")));
            d.put("ActionTypeId", id > 0 ? 2 : 1);
            consumption.add(d);
        }
        for (Map<String, Object> r : list(b.get("removedConsumption"))) consumption.add(removedDetail(r));   // :1590

        /* ---- OverHead rows :1597 — only rows with an account ---- */
        List<Map<String, Object>> foh = new ArrayList<>();
        idx = 0;
        for (Map<String, Object> r : list(b.get("overheads"))) {
            idx++;
            int acc = toInt(r.get("OverHeadAccountId"));
            if (acc <= 0) continue;
            if (blank(r.get("Amount"))) throw new IllegalArgumentException("Amount Field Required in Grid Over Head Row No: " + idx);
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", recId > 0 ? toInt(r.get("Id")) : 0);
            d.put("BomHeaderId", 0);
            d.put("OverHeadTypeId", acc);                                                        // :1609 — the account id again
            d.put("GLAccountId", acc);
            d.put("Qty", toDouble(r.get("Qty")));
            d.put("Rate", toDouble(r.get("Rate")));
            d.put("Amount", toDouble(r.get("Amount")));
            d.put("OhRemarks", str(r.get("OhRemarks")));
            d.put("PercentOfTotalInput", toDouble(r.get("PercentOfTotalInput")));
            foh.add(d);
        }

        int num = repo.saveHeader(recId == 0, head);
        if (num <= 0) num = recId;
        if (num <= 0) throw new RuntimeException("Recipe save returned no Id");
        for (Map<String, Object> d : inputs)      { d.put("BomHeaderId", num); repo.insertInput(d); }
        for (Map<String, Object> d : consumption) { d.put("BomHeaderId", num); repo.insertConsumption(d); }
        for (Map<String, Object> d : foh)         { d.put("BomHeaderId", num); repo.insertFoh(d); }

        int docNo = toInt(head.get("DocNo"));
        return ok((recId == 0 ? "Data Save Successfully....  " : "Data Update Successfully....  ") + docNo, num, docNo);
    }

    /** grdInputMaterial_ColumnButtonClick:827 / grdOutPutByProduct_ColumnButtonClick:1050 — the removed row, ActionTypeId 3. */
    private static Map<String, Object> removedDetail(Map<String, Object> r) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", toInt(r.get("Id")));
        d.put("BomHeaderId", 0);
        d.put("ItemId", toInt(r.get("ItemId")));
        d.put("ItemUomId", toInt(r.get("ItemUomId")));
        d.put("ItemQty", toDouble(r.get("ItemQty")));
        d.put("WeightKg", toDouble(r.get("NetWeight")));
        d.put("Rate", toDouble(r.get("Rate")));
        d.put("RateUomId", toInt(r.get("RateUomId")));
        d.put("Amount", toDouble(r.get("Amount")));
        d.put("ActionTypeId", 3);
        return d;
    }

    /** :1438-1451 — a UOM id of 0 is looked up by Equivalent in the item's schedule. */
    private int resolveUom(UserAccount u, int itemId, int uomId, double equivalent, String where, String itemText) {
        if (uomId != 0) return uomId;
        List<Map<String, Object>> dt = db.uomsByItem(u, itemId);
        if (dt.isEmpty()) return 0;
        for (Map<String, Object> r : dt) if (toDouble(ci(r, "Equivalent")) == equivalent) return toInt(ci(r, "Id"));   // dr[0].ItemArray[0] = Id
        throw new IllegalArgumentException(where + " Is not Define Against Item " + itemText + ". Please Check...");
    }

    /** btnDelete_Click:1814. */
    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = db.user();
        if (!db.has(SCREEN, "delete")) throw new IllegalStateException("You do not have the Delete right for this screen.");
        if (id <= 0) throw new IllegalArgumentException("No record found to Delete");
        if (ownedHeader(u, id) == null) throw new IllegalArgumentException("Record Not Found");
        repo.deleteById(u.getId(), id);
        return ok("Delete Record Successfully", id, 0);
    }

    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOC_TYPE) return null;
        if (toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }
}
