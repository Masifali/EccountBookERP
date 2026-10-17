package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.W3BMfgRepository;
import com.mst.repositories.W5BBomRepository;
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
 * W5B - Bill Of Material family (see {@link W5BBomProfile}): 774 frmBillOfMaterial (1617), 845 frmBillOfMaterialCasting
 * (1664) and 598 frmBillOfMaterialConcrete (1851). Modelled on {@link Prod273RecipeBrandingService} (screen 285).
 *
 * DESKTOP BEHAVIOUR REPRODUCED (line numbers: 774 frmBillOfMaterial.cs / 845 frmBillOfMaterialCasting.cs / 598 frmBillOfMaterialConcrete.cs):
 * <ol>
 *  <li>FormValidation (774:806, 845:626, 598:~930) in the desktop order, then "Grid Record Not Found...", then the confirm.</li>
 *  <li>Insert() (774:2002, 598:2150): input rows (an empty ItemId blocks the save - "Item Field Required in Grid Input Row No: n"),
 *      output rows (empty ItemId skipped), over-head rows (only with an account), contractor wages rows (only with a
 *      ContractorWagesRateScheduleId), and for 774 the operations rows (removed first, "Missing sequence numbers: ...").</li>
 *  <li>The UOM / Rate UOM of every input / output row is validated against UOMSchedule 'ReadByItemID' ("UOM in Input Grid Row No: n
 *      Is not Define Against Item X. Please Check...") and the schedule row's Id is what is saved.</li>
 *  <li>ActionTypeId 2 for a row that has an Id, otherwise 1; removed rows are sent with ActionTypeId 3 (774/845 only when
 *      updating, 598 always).</li>
 *  <li>598: MinQty / MaxQty are validated and then forced to ItemQty; MainItem rules.</li>
 *  <li>BomType = BomLevelType = the type combo text, ProjectsId = BranchesId, ApprovedUserId / ApprovedDate set while IsApproved
 *      stays false. 774: ItemId may be 0 when a CastingType is chosen; 845: ItemId 0, IsActive true.</li>
 * </ol>
 *
 * DEVIATIONS (web-only, with reason):
 * <ol>
 *  <li>Tenancy: open / update / delete only a header of this screen's DocumentTypeId and the signed-in company.</li>
 *  <li>Attachments (Attachment dialog / DMS copy, AttachmentsValues) are not ported; the NoOfAttachments history column is shown.</li>
 *  <li>History BOM No filter: the desktop sends @Id, which neither FormHistory procedure reads; the web sends @BOMNo, which the
 *      procedures do filter on (the same choice screen 285 makes).</li>
 *  <li>The grid's F1 lookups (GrdPopUp) are in-cell pickers on the page.</li>
 * </ol>
 */
@Service
public class W5BBomService {

    private static final Set<Integer> OVERHEAD_ACCOUNT_TYPES = new HashSet<>(Arrays.asList(3, 6, 8, 11, 12, 13, 14, 20, 21));   // AccountTitleFill:625
    private static final Set<Integer> CONCRETE_PRODUCTION_TYPES = new HashSet<>(Arrays.asList(45, 46, 47));                       // 598 ProductionTypeFill:762

    private final Prod273Support db;
    private final W5BBomRepository repo;
    private final W3BMfgRepository mfg;

    public W5BBomService(Prod273Support db, W5BBomRepository repo, W3BMfgRepository mfg) {
        this.db = db;
        this.repo = repo;
        this.mfg = mfg;
    }

    // ====================================================================================== load

    private static List<Map<String, Object>> idName(List<Map<String, Object>> rows, String idKey, String nameKey) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, idKey)));
            o.put("Name", str(ci(r, nameKey)));
            out.add(o);
        }
        return out;
    }

    /** Form_Load (774:387, 845:312, 598:405). */
    public Map<String, Object> lookups(W5BBomProfile p) {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("config", p.config());
        out.put("rights", db.rights(p.screen));
        out.put("branchId", db.branchId(u));
        out.put("rateDecimals", db.rateDecimals(u));
        out.put("amountDecimals", amountDecimals(u));
        out.put("financialYearStart", db.financialYearStart(u, db.financialYearId()));
        out.putAll(itemLists(p, u));
        out.put("allUoms", db.allUoms(u));                                                       // uomgetAll
        out.put("accounts", accounts(u));                                                        // AccountTitleFill
        out.put("bomTypes", bomTypes());                                                         // TypeBind
        out.put("dateTypes", Prod273Support.literal(Prod273Support.DATE_TYPES, "Parameters"));   // Datetypefill
        if (p.mfgSchema) {
            out.put("uoms", idName(repo.uomMaster(u), "Id", "UOMCode"));                         // FillUOM
            out.put("castingTypes", idName(repo.castingTypes(), "Id", "LookupName"));            // BindCastingType
        }
        if (p.hasOperations) {
            List<Map<String, Object>> ops = new ArrayList<>();                                   // grdProcessSetting:1676
            for (Map<String, Object> r : mfg.wsoByWsoId(u, 0)) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", toInt(ci(r, "Id")));
                o.put("Name", str(ci(r, "Description")));
                o.put("WorkStationName", str(ci(r, "WorkStationName")));
                o.put("OperationName", str(ci(r, "OperationName")));
                ops.add(o);
            }
            out.put("operationCombo", ops);
        }
        if (p.hasRouting) out.put("routingNames", idName(mfg.routingNamesInUse(u), "Id", "ReferenceName"));   // RoutingNameBind
        if (p.concrete) {
            List<Map<String, Object>> pt = new ArrayList<>();                                    // ProductionTypeFill:750
            for (Map<String, Object> r : repo.invLookupType6(u)) {
                if (!CONCRETE_PRODUCTION_TYPES.contains(toInt(ci(r, "Id")))) continue;
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", toInt(ci(r, "Id")));
                o.put("Name", str(ci(r, "LookupName")));
                pt.add(o);
            }
            out.put("productionTypes", pt);
            out.put("plants", idName(repo.plants(u), "Id", "Description"));                      // :815
        }
        out.putAll(historyCombos(p));
        out.put("docNo", repo.generateCode(p, u, db.branchId(u), db.financialYearId()));
        return out;
    }

    /** ItemFill: 774/845 AllItemsBindForEngr for the header item and every grid; 598 ItemGetAllServiceBind for the input grid and 45/46/47 items for the header + output grid. */
    private Map<String, Object> itemLists(W5BBomProfile p, UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        if (p.concrete) {
            out.put("items", project(db.itemsGetAll(u), "Id", "ItemName"));                      // dtitemInput (ItemFill:561)
            out.put("headerItems", db.itemsByType(u, "45,46,47"));                               // dtitemHeader (ItemFill:563)
        } else {
            List<Map<String, Object>> items = project(repo.itemsForEngr(u), "Id", "ItemName");
            out.put("items", items);
            out.put("headerItems", items);
        }
        return out;
    }

    /** btnRefresh_Click (774:1900, 845, 598:2095). */
    public Map<String, Object> refresh(W5BBomProfile p) {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>(itemLists(p, u));
        out.put("allUoms", db.allUoms(u));
        out.put("accounts", accounts(u));
        out.put("bomTypes", bomTypes());
        if (p.mfgSchema) {
            out.put("uoms", idName(repo.uomMaster(u), "Id", "UOMCode"));
            out.put("castingTypes", idName(repo.castingTypes(), "Id", "LookupName"));
        }
        return out;
    }

    /** CmbProductionType_Leave (598:846) - the header / output item list for one production type. */
    public List<Map<String, Object>> itemsByProductionType(int typeId) {
        UserAccount u = db.user();
        return db.itemsByType(u, String.valueOf(typeId));
    }

    public int docNo(W5BBomProfile p) {
        UserAccount u = db.user();
        return repo.generateCode(p, u, db.branchId(u), db.financialYearId());
    }

    private static List<Map<String, Object>> bomTypes() {                                          // TypeBind:743
        List<Map<String, Object>> t = new ArrayList<>();
        String[] names = {"Semi-Finish Goods", "Finish Goods"};
        for (int i = 0; i < names.length; i++) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", i + 1);
            o.put("Name", names[i]);
            t.add(o);
        }
        return t;
    }

    /** clsGlobalVariables.DefaultNoofDecimalPointsForAmount. */
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

    /** AllCombobind (774:2675, 598:2783) - Activity "BOMType" -> type, "BOMName" -> BOM, "ItemHeader" -> item (845 has no item filter). */
    public Map<String, Object> historyCombos(W5BBomProfile p) {
        UserAccount u = db.user();
        List<Map<String, Object>> types = new ArrayList<>(), bom = new ArrayList<>(), items = new ArrayList<>();
        for (Map<String, Object> r : repo.dropDowns(p, u)) {
            String a = str(ci(r, "Activity")).trim();
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "ReferenceName")));
            if ("BOMType".equals(a)) types.add(o);
            else if ("BOMName".equals(a)) bom.add(o);
            else if ("ItemHeader".equals(a)) items.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("historyTypes", types);
        out.put("bomNames", bom);
        out.put("historyItems", items);
        return out;
    }

    /** CmbRoutingName_Leave (774:4273) - ReadByRoutingNameId rows for the Operations grid. */
    public List<Map<String, Object>> routing(W5BBomProfile p, int routingNameId) {
        db.user();
        if (!p.hasRouting) throw new IllegalArgumentException("Routing is not available on this screen.");
        if (!db.has(p.screen, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");   // :4291
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : mfg.routingByNameId(routingNameId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", 0);
            o.put("WorkStationOperationId", toInt(ci(r, "WorkStationOperationId")));
            o.put("OperationName", str(ci(r, "OperationName")));
            o.put("WorkStationName", str(ci(r, "WorkStationName")));
            o.put("CapacityQty", toDouble(ci(r, "CapacityQty")));
            int[] hm = hoursMinutes(ci(r, "operationTime"));
            o.put("Hours", hm[0]);
            o.put("Minutes", hm[1]);
            o.put("SeqNo", toInt(ci(r, "sequenceNo")));
            o.put("HourlyCost", 0d);
            o.put("OperationCost", 0d);
            o.put("OperationRemarks", "");
            out.add(o);
        }
        return out;
    }

    /** GetServiceActivityByItemId (774/598 grdContractorWages F1) - rows for the picker. */
    public List<Map<String, Object>> wagesActivities(W5BBomProfile p, int itemId, String docDate) {
        db.user();
        if (!p.hasWages) throw new IllegalArgumentException("Contractor wages are not available on this screen.");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.serviceActivities(itemId, Prod273Support.pickerValue(docDate))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("ContractorWagesRateScheduleId", toInt(ci(r, "ContractorWagesRateScheduleId")));
            o.put("WagesActivity", str(ci(r, "WagesActivity")));
            o.put("Rate", toDouble(ci(r, "Rate")));
            o.put("Qty", toDouble(ci(r, "Qty")));
            out.add(o);
        }
        return out;
    }

    // ====================================================================================== read

    private static int[] hoursMinutes(Object v) {
        String s = str(v).trim();
        int h = 0, m = 0;
        String[] parts = s.split(":");
        try {
            if (parts.length >= 2) {
                String hp = parts[0];
                int dot = hp.lastIndexOf('.');
                if (dot >= 0) hp = hp.substring(dot + 1);
                h = Integer.parseInt(hp.trim());
                m = Integer.parseInt(parts[1].trim());
            }
        } catch (NumberFormatException e) {
            h = 0; m = 0;
        }
        return new int[] { h, m };
    }

    /** ReadById (774:2467, 845, 598:2518) - header + the grids in the dt column names. */
    public Map<String, Object> load(W5BBomProfile p, int id) {
        UserAccount u = db.user();
        Map<String, Object> h = ownedHeader(p, u, id);
        if (h == null) return null;
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> head = new LinkedHashMap<>();
        head.put("Id", headerId(h));
        head.put("DocDate", day(ci(h, "DocDate")));
        head.put("DocNo", toInt(ci(h, "DocNo")));
        head.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        head.put("ItemId", toInt(ci(h, "ItemId")));
        head.put("ItemUomId", toInt(ci(h, "ItemUomId")));
        head.put("BomName", str(ci(h, "BomName")));
        head.put("BomType", str(ci(h, "BomType")));
        head.put("EffectedDate", day(ci(h, "EffectedDate")));
        head.put("BomBatchQty", toDouble(ci(h, "BomBatchQty")));
        head.put("IsActive", truthy(ci(h, "IsActive")));
        head.put("IsDefault", truthy(ci(h, "IsDefault")));
        if (p.mfgSchema) head.put("CastingTypeId", toInt(ci(h, "CastingTypeId")));
        if (p.concrete) {
            head.put("ProductionTypeId", toInt(ci(h, "ProductionTypeId")));
            head.put("PlantId", toInt(ci(h, "PlantId")));
            head.put("TopForce", toDouble(ci(h, "TopForce")));
            head.put("BottomForce", toDouble(ci(h, "BottomForce")));
        }
        out.put("header", head);
        out.put("inputs", detailRows(repo.inputRows(p, id), "BomInputDetailId", p.concrete));
        out.put("byproducts", detailRows(repo.byProductRows(p, id), "BomByProductDetailId", false));
        List<Map<String, Object>> foh = new ArrayList<>();
        for (Map<String, Object> r : repo.fohRows(p, id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", firstInt(r, "BomFohDetailId", "Id"));
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
        if (p.hasWages) {
            List<Map<String, Object>> w = new ArrayList<>();
            for (Map<String, Object> r : repo.wagesRows(p, id)) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", toInt(ci(r, "Id")));
                o.put("ContractorWagesRateScheduleId", toInt(ci(r, "ContractorWagesRateScheduleId")));
                Object sa = ci(r, "ServiceActivity");
                if (sa == null) sa = ci(r, "WagesActivity");
                o.put("ServiceActivity", str(sa));
                o.put("ItemName", str(ci(r, "ItemName")));
                o.put("Qty", toDouble(ci(r, "ItemQty")));
                o.put("Rate", toDouble(ci(r, "Rate")));
                o.put("Amount", toDouble(ci(r, "Amount")));
                o.put("Remarks", str(ci(r, "Remarks")));
                w.add(o);
            }
            out.put("wages", w);
        }
        if (p.hasOperations) {
            List<Map<String, Object>> ops = new ArrayList<>();                                // 774:2560
            for (Map<String, Object> r : repo.operationRows(p, id)) {
                Map<String, Object> o = new LinkedHashMap<>();
                int[] hm = hoursMinutes(ci(r, "operationTime"));
                o.put("Id", firstInt(r, "BomOperationDetailId", "Id"));
                o.put("WorkStationOperationId", toInt(ci(r, "WorkStationOperationId")));
                o.put("OperationName", str(ci(r, "OperationName")));
                o.put("WorkStationName", str(ci(r, "WorkStationName")));
                o.put("CapacityQty", toDouble(ci(r, "capacityQty")));
                o.put("Hours", hm[0]);
                o.put("Minutes", hm[1]);
                o.put("SeqNo", toInt(ci(r, "sequenceNo")));
                o.put("HourlyCost", toDouble(ci(r, "hourlyCost")));
                o.put("OperationCost", toDouble(ci(r, "operatingCost")));
                o.put("OperationRemarks", str(ci(r, "OperationRemarks")));
                ops.add(o);
            }
            out.put("operations", ops);
        }
        return out;
    }

    private static int firstInt(Map<String, Object> r, String... keys) {
        for (String k : keys) {
            Object v = ci(r, k);
            if (v != null) return toInt(v);
        }
        return 0;
    }

    private static int headerId(Map<String, Object> h) { return firstInt(h, "BomHeaderId", "Id"); }

    private static List<Map<String, Object>> detailRows(List<Map<String, Object>> rows, String mfgIdKey, boolean concreteExtras) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", firstInt(r, mfgIdKey, "Id"));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemUomId", toInt(ci(r, "ItemUomId")));
            o.put("Uom", toDouble(ci(r, "PackUomEquivalent")));
            o.put("UomCode", str(ci(r, "PackUom")));
            o.put("ItemQty", toDouble(ci(r, "ItemQty")));
            Object w = ci(r, "WeightKg");
            if (w == null) w = ci(r, "Weight");
            o.put("NetWeight", toDouble(w));
            Object rate = ci(r, "Rate");
            if (rate == null) rate = ci(r, "ItemRate");
            o.put("Rate", toDouble(rate));
            o.put("RateUomId", toInt(ci(r, "RateUomId")));
            o.put("RateUom", toDouble(ci(r, "RateUomEquivalent")));
            o.put("RateUomCode", str(ci(r, "RateUom")));
            o.put("Amount", toDouble(ci(r, "Amount")));
            if (concreteExtras) {
                o.put("MinQty", toDouble(ci(r, "MinQty")));
                o.put("MaxQty", toDouble(ci(r, "MaxQty")));
                o.put("MainItemId", toInt(ci(r, "MainItemId")));
                o.put("MainItemName", str(ci(r, "MainItemName")));
                o.put("MainItemUomId", toInt(ci(r, "MainItemUomId")));
                o.put("MainUomCode", str(ci(r, "MainPackUom")));
            }
            out.add(o);
        }
        return out;
    }

    // ====================================================================================== history

    /** HistoryGridFill (774:2813, 598:2919). */
    public List<Map<String, Object>> history(W5BBomProfile p, String from, String to, int fromDocNo, int toDocNo, int bomId, String bomType, int itemId) {
        UserAccount u = db.user();
        boolean viewAll = db.has(p.screen, "viewAll");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(p, u, db.branchId(u), db.financialYearId(), fromDocNo, toDocNo,
                Prod273Support.pickerValue(from), Prod273Support.pickerValue(to), bomId, bomType, itemId, viewAll, u.getId())) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("RecordNo", toInt(ci(r, "RecordNo")));
            o.put("Id", headerId(r));
            o.put("DocumentTypeId", toInt(ci(r, "DocumentTypeId")));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("DocDate", day(ci(r, "DocDate")));
            o.put("BomName", str(ci(r, "BomName")));
            if (p.mfgSchema) o.put("CastingType", str(ci(r, "CastingType")));
            if (p.concrete) {
                o.put("ProductionType", str(ci(r, "ProductionType")));
                o.put("PlantName", str(ci(r, "PlantName")));
            }
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

    // ====================================================================================== save

    private void requireRight(W5BBomProfile p, int recId) {
        if (recId == 0 && !db.has(p.screen, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !db.has(p.screen, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
    }

    /** FormValidation (774:806, 845:626, 598:930) + the grid test. */
    private void preConfirm(W5BBomProfile p, Map<String, Object> b) {
        if (toInt(b.get("DocNo")) == 0) throw new IllegalArgumentException("doc No Field Required");
        String bom = str(b.get("BomName")).trim();
        if (bom.isEmpty() || "0".equals(bom)) throw new IllegalArgumentException("BOM Field Required");
        if (p.concrete) {
            if (toInt(b.get("ProductionTypeId")) == 0) throw new IllegalArgumentException("Bom Type Field Required");
            if (str(b.get("BomType")).trim().isEmpty()) throw new IllegalArgumentException("Finish Type Field Required");
            if (toInt(b.get("ItemId")) == 0) throw new IllegalArgumentException("Item Feild Required");
            if (toInt(b.get("ItemUomId")) == 0) throw new IllegalArgumentException("IteUom Field Required");
        } else if (p.hasItem) {                                                                 // 774
            if (str(b.get("BomType")).trim().isEmpty()) throw new IllegalArgumentException("Type Field Required");
            if (toInt(b.get("CastingTypeId")) == 0 && toInt(b.get("ItemId")) == 0) throw new IllegalArgumentException("CastingType or Item Field Required");
            if (toInt(b.get("ItemUomId")) == 0) throw new IllegalArgumentException("IteUom Field Required");
        } else {                                                                                  // 845
            if (str(b.get("BomType")).trim().isEmpty()) throw new IllegalArgumentException("Type Field Required");
            if (toInt(b.get("CastingTypeId")) == 0) throw new IllegalArgumentException("CastingType Field Required");
            if (toInt(b.get("ItemUomId")) == 0) throw new IllegalArgumentException("IteUom Field Required");
            if (blank(b.get("BomBatchQty")) || toDouble(b.get("BomBatchQty")) == 0d) throw new IllegalArgumentException("Qty Field Required");
        }
        if (list(b.get("inputs")).isEmpty()) throw new IllegalArgumentException("Grid Record Not Found...\nGrid Input Should have atleast one record");
    }

    public Map<String, Object> precheck(W5BBomProfile p, Map<String, Object> b) {
        UserAccount u = db.user();
        int recId = toInt(b.get("Id"));
        requireRight(p, recId);
        if (recId != 0 && ownedHeader(p, u, recId) == null) throw new IllegalArgumentException("Record Not Found");
        preConfirm(p, b);
        return ok("", 0, 0);
    }

    @Transactional
    public Map<String, Object> save(W5BBomProfile p, Map<String, Object> b) {
        UserAccount u = db.user();
        int recId = toInt(b.get("Id"));
        requireRight(p, recId);
        if (recId != 0 && ownedHeader(p, u, recId) == null) throw new IllegalArgumentException("Record Not Found");
        preConfirm(p, b);

        Timestamp now = Prod273Support.now();
        String type = str(b.get("BomType"));
        Map<String, Object> head = new LinkedHashMap<>();
        head.put("Id", recId);
        head.put("DocumentTypeId", p.docType);
        head.put("DocNo", toInt(b.get("DocNo")));
        head.put("DocDate", Prod273Support.pickerValue(b.get("DocDate")));
        head.put("BomName", str(b.get("BomName")));
        head.put("BomType", type);
        head.put("BomLevelType", type);
        head.put("ItemId", p.hasItem ? toInt(b.get("ItemId")) : 0);                              // 845 never sends an item
        head.put("ItemUomId", toInt(b.get("ItemUomId")));
        head.put("BomBatchQty", toDouble(b.get("BomBatchQty")));
        head.put("EffectedDate", Prod273Support.pickerValue(b.get("EffectedDate")));
        head.put("IsActive", p.hasActive ? truthy(b.get("IsActive")) : Boolean.TRUE);
        head.put("IsDefault", truthy(b.get("IsDefault")));
        head.put("RemarksHeader", str(b.get("RemarksHeader")));
        head.put("OrganizationId", u.getOrganizationId());
        head.put("CompanyId", u.getCompanyId());
        head.put("BranchesId", db.branchId(u));
        head.put("ProjectsId", db.branchId(u));
        head.put("FinancialYearId", db.financialYearId());
        head.put("EntryUserId", u.getId());
        head.put("EntryDate", now);
        head.put("ModifyUserId", u.getId());
        head.put("ModifyDate", now);
        head.put("IsApproved", Boolean.FALSE);
        head.put("ApprovedUserId", u.getId());
        head.put("ApprovedDate", now);
        head.put("ActionId", recId == 0 ? 1 : 2);
        if (p.mfgSchema) head.put("CastingTypeId", toInt(b.get("CastingTypeId")));
        if (p.concrete) {
            head.put("ProductionTypeId", toInt(b.get("ProductionTypeId")));
            head.put("PlantId", toInt(b.get("PlantId")));
            head.put("TopForce", toDouble(b.get("TopForce")));
            head.put("BottomForce", toDouble(b.get("BottomForce")));
        }

        /* ---- Input rows ---- */
        List<Map<String, Object>> inputs = new ArrayList<>();
        int idx = 0;
        for (Map<String, Object> r : list(b.get("inputs"))) {
            idx++;
            int itemId = toInt(r.get("ItemId"));
            if (itemId == 0) throw new IllegalArgumentException("Item Field Required in Grid Input Row No: " + idx);
            int id = toInt(r.get("Id"));
            String itemText = str(r.get("ItemName"));
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", id);
            d.put("BomHeaderId", 0);
            d.put("ItemId", itemId);
            d.put("ActionTypeId", id > 0 ? 2 : 1);
            if (blank(r.get("ItemUomId"))) throw new IllegalArgumentException("UOM  Field Required in Grid Input Row No: " + idx);
            d.put("ItemUomId", scheduleId(u, itemId, toInt(r.get("ItemUomId")), "UOM in Input Grid Row No: " + idx, itemText));
            if (blank(r.get("ItemQty"))) throw new IllegalArgumentException("ItemQty Field Required in Grid Input Row No: " + idx);
            d.put("ItemQty", toDouble(r.get("ItemQty")));
            if (blank(r.get("NetWeight"))) throw new IllegalArgumentException("NetWeight Field Required in Grid Input Row No: " + idx);
            d.put("WeightKg", toDouble(r.get("NetWeight")));
            if (blank(r.get("Rate"))) throw new IllegalArgumentException("Rate Field Required in Grid Input Row No: " + idx);
            d.put("Rate", toDouble(r.get("Rate")));
            if (blank(r.get("RateUomId"))) throw new IllegalArgumentException("Rate UOM Field Required in Grid Input Row No: " + idx);
            d.put("RateUomId", scheduleId(u, itemId, toInt(r.get("RateUomId")), "RateUom in Input Grid Row No: " + idx, itemText));
            if (blank(r.get("Amount"))) throw new IllegalArgumentException("AmountField Required in Grid Input Row No: " + idx);
            d.put("Amount", toDouble(r.get("Amount")));
            d.put("QtyUomId", 0);
            d.put("SortNo", 0);
            if (p.concrete) {                                                                    // 598:2305-2330
                double qty = toDouble(r.get("ItemQty"));
                double min = toDouble(r.get("MinQty")), max = toDouble(r.get("MaxQty"));
                int mainItem = toInt(r.get("MainItemId")), mainUom = toInt(r.get("MainItemUomId"));
                if (min > 0d && min > qty) throw new IllegalArgumentException("MinQty Should be less than or equal to ItemQty... in Input Grid Row No : " + idx);
                if (max > 0d && max < qty) throw new IllegalArgumentException("MaxQty Should be greater than or equal to ItemQty... in Input Grid Row No : " + idx);
                if (mainItem > 0 && mainItem == itemId) throw new IllegalArgumentException("MainItem Can't be Same as Item in Input Grid Row No : " + idx);
                if (mainItem > 0 && mainUom == 0) throw new IllegalArgumentException("MainItemUom required when MainItem is Selected in Input Grid Row No : " + idx);
                d.put("MinQty", qty);                                                            // the desktop forces both to ItemQty
                d.put("MaxQty", qty);
                d.put("MainItemId", mainItem);
                d.put("MainItemUomId", mainUom);
            }
            inputs.add(d);
        }
        if (recId > 0 || p.concrete) for (Map<String, Object> r : list(b.get("removedInputs"))) inputs.add(removedInput(r, p.concrete));

        /* ---- Output (by-product) rows ---- */
        List<Map<String, Object>> byproducts = new ArrayList<>();
        idx = 0;
        for (Map<String, Object> r : list(b.get("byproducts"))) {
            idx++;
            int itemId = toInt(r.get("ItemId"));
            if (itemId == 0) continue;
            int id = toInt(r.get("Id"));
            String itemText = str(r.get("ItemName"));
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", id);
            d.put("BomHeaderId", 0);
            d.put("ItemId", itemId);
            d.put("ActionTypeId", id > 0 ? 2 : 1);
            if (blank(r.get("ItemUomId"))) throw new IllegalArgumentException("UOM  Field Required in Grid Input Row No: " + idx);
            d.put("ItemUomId", scheduleId(u, itemId, toInt(r.get("ItemUomId")), "UOM in Output Grid Row No: " + idx, itemText));
            if (blank(r.get("ItemQty"))) throw new IllegalArgumentException("ItemQty Field Required in Grid Input Row No: " + idx);
            d.put("ItemQty", toDouble(r.get("ItemQty")));
            if (blank(r.get("NetWeight"))) throw new IllegalArgumentException("NetWeight Field Required in Grid Input Row No: " + idx);
            d.put("Weight", toDouble(r.get("NetWeight")));
            if (blank(r.get("Rate"))) throw new IllegalArgumentException("Rate Field Required in Grid Output Row No: " + idx);
            d.put("ItemRate", toDouble(r.get("Rate")));
            if (blank(r.get("RateUomId"))) throw new IllegalArgumentException("Rate UOM Field Required in Grid Output Row No: " + idx);
            d.put("RateUomId", scheduleId(u, itemId, toInt(r.get("RateUomId")), "RateUom in output Grid Row No: " + idx, itemText));
            if (blank(r.get("Amount"))) throw new IllegalArgumentException("AmountField Required in Grid Output Row No: " + idx);
            d.put("Amount", toDouble(r.get("Amount")));
            byproducts.add(d);
        }
        if (recId > 0 || p.concrete) for (Map<String, Object> r : list(b.get("removedByproducts"))) byproducts.add(removedByProduct(r));

        /* ---- OverHead rows - only rows with an account ---- */
        List<Map<String, Object>> foh = new ArrayList<>();
        idx = 0;
        for (Map<String, Object> r : list(b.get("overheads"))) {
            idx++;
            int acc = toInt(r.get("OverHeadAccountId"));
            if (acc <= 0) continue;
            if (blank(r.get("Amount"))) throw new IllegalArgumentException("Amount Field Required in Grid Over Head Row No: " + idx);
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", toInt(r.get("Id")));
            d.put("BomHeaderId", 0);
            d.put("OverHeadTypeId", acc);                                                        // the account id again, as the desktop does
            d.put("GLAccountId", acc);
            d.put("Qty", toDouble(r.get("Qty")));
            d.put("Rate", toDouble(r.get("Rate")));
            d.put("Amount", toDouble(r.get("Amount")));
            d.put("OhRemarks", str(r.get("OhRemarks")));
            d.put("PercentOfTotalInput", toDouble(r.get("PercentOfTotalInput")));
            foh.add(d);
        }

        /* ---- Contractor wages rows - only rows with a rate schedule ---- */
        List<Map<String, Object>> wages = new ArrayList<>();
        if (p.hasWages) {
            idx = 0;
            for (Map<String, Object> r : list(b.get("wages"))) {
                idx++;
                if (toInt(r.get("ContractorWagesRateScheduleId")) <= 0) continue;
                if (blank(r.get("ServiceActivity"))) throw new IllegalArgumentException("Service Activity Field Required in Grid Contractor Wages Row No: " + idx);
                if (blank(r.get("Amount"))) throw new IllegalArgumentException("Amount Field Required in Grid Contractor Wages Row No: " + idx);
                Map<String, Object> d = new LinkedHashMap<>();
                d.put("Id", toInt(r.get("Id")));
                d.put("BomHeaderId", 0);
                d.put("ContractorWagesRateScheduleId", toInt(r.get("ContractorWagesRateScheduleId")));
                d.put("ItemId", toInt(b.get("ItemId")));                                          // the header item
                d.put("ItemQty", toDouble(r.get("Qty")));
                d.put("Rate", toDouble(r.get("Rate")));
                d.put("Amount", toDouble(r.get("Amount")));
                d.put("Remarks", str(r.get("Remarks")));
                wages.add(d);
            }
        }

        /* ---- Operations rows (774) - removed rows first ---- */
        List<Map<String, Object>> ops = new ArrayList<>();
        if (p.hasOperations) {
            if (recId > 0) for (Map<String, Object> r : list(b.get("removedOperations"))) ops.add(operationRow(r, 3));
            idx = 0;
            List<Map<String, Object>> opRows = list(b.get("operations"));
            boolean anyOperation = false;
            for (Map<String, Object> r : opRows) {
                idx++;
                if (toInt(r.get("WorkStationOperationId")) <= 0) throw new IllegalArgumentException("WorkStationOperation Field Required in Grid Operation Row No: " + idx);
                anyOperation = true;
                if (toInt(r.get("Hours")) == 0 && toInt(r.get("Minutes")) == 0) throw new IllegalArgumentException("Hours or Minutes Field Required in Grid Operations Row No: " + idx);
                if (toDouble(r.get("HourlyCost")) == 0d) throw new IllegalArgumentException("Hourly Cost Field Required in Grid Operations Row No: " + idx);
                if (toDouble(r.get("OperationCost")) == 0d) throw new IllegalArgumentException("OperationCost Field Required in Grid Operations Row No: " + idx);
                ops.add(operationRow(r, toInt(r.get("Id")) <= 0 ? 1 : 2));
            }
            if (anyOperation) {                                                                   // GetMissingSequence
                Set<Integer> seq = new HashSet<>();
                for (Map<String, Object> r : opRows) seq.add(toInt(r.get("SeqNo")));
                StringBuilder missing = new StringBuilder();
                for (int i = 1; i <= opRows.size(); i++) if (!seq.contains(i)) missing.append(i).append(',');
                if (missing.length() > 0) throw new IllegalArgumentException("Missing sequence numbers: " + missing + " in Operations Grid");
            }
        }

        int num = repo.saveHeader(p, recId == 0, head);
        if (num <= 0) num = recId;
        if (num <= 0) throw new RuntimeException("Bill of material save returned no Id");
        for (Map<String, Object> d : inputs)     { d.put("BomHeaderId", num); repo.insertInput(p, d); }
        for (Map<String, Object> d : foh)        { d.put("BomHeaderId", num); repo.insertFoh(p, d); }
        for (Map<String, Object> d : byproducts) { d.put("BomHeaderId", num); repo.insertByProduct(p, d); }
        for (Map<String, Object> d : ops)        { d.put("bomHeaderId", num); repo.insertOperation(d); }
        for (Map<String, Object> d : wages)      { d.put("BomHeaderId", num); repo.insertWages(p, d); }

        int docNo = toInt(head.get("DocNo"));
        return ok((recId == 0 ? "Data Save Successfully....  " : "Data Update Successfully....  ") + docNo, num, docNo);
    }

    /**
     * 774:2037 / 598:2237 - GetUomScheduleByItemId(item); when the item has schedule rows the chosen id must be one of
     * them (else "... Is not Define Against Item X. Please Check...") and that row's Id is saved; with no schedule rows the
     * desktop leaves the id at 0.
     */
    private int scheduleId(UserAccount u, int itemId, int uomId, String where, String itemText) {
        List<Map<String, Object>> dt = db.uomsByItem(u, itemId);
        if (dt.isEmpty()) return 0;
        for (Map<String, Object> r : dt) if (toInt(ci(r, "Id")) == uomId) return toInt(ci(r, "Id"));
        throw new IllegalArgumentException(where + " Is not Define Against Item " + itemText + ". Please Check...");
    }

    /** grdInputMaterial_ColumnButtonClick (598:1197) - the removed row, ActionTypeId 3. */
    private static Map<String, Object> removedInput(Map<String, Object> r, boolean concrete) {
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
        if (concrete) { d.put("MinQty", 0d); d.put("MaxQty", 0d); d.put("MainItemId", 0); d.put("MainItemUomId", 0); }
        return d;
    }

    private static Map<String, Object> removedByProduct(Map<String, Object> r) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", toInt(r.get("Id")));
        d.put("BomHeaderId", 0);
        d.put("ItemId", toInt(r.get("ItemId")));
        d.put("ItemUomId", toInt(r.get("ItemUomId")));
        d.put("ItemQty", toDouble(r.get("ItemQty")));
        d.put("Weight", toDouble(r.get("NetWeight")));
        d.put("ItemRate", toDouble(r.get("Rate")));
        d.put("RateUomId", toInt(r.get("RateUomId")));
        d.put("Amount", toDouble(r.get("Amount")));
        d.put("ActionTypeId", 3);
        return d;
    }

    /** 774:2317 / DeleteOperationDetailRow:1799 - operationTime is TimeSpan(h, m, 0).ToString("hh\:mm\:ss"). */
    private static Map<String, Object> operationRow(Map<String, Object> r, int actionTypeId) {
        int total = toInt(r.get("Hours")) * 60 + toInt(r.get("Minutes"));
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("bomHeaderId", 0);
        d.put("Id", toInt(r.get("Id")));
        d.put("sequenceNo", toInt(r.get("SeqNo")));
        d.put("WorkStationOperationId", toInt(r.get("WorkStationOperationId")));
        d.put("capacityQty", 0d);
        d.put("operationTime", String.format("%02d:%02d:00", (total / 60) % 24, total % 60));
        d.put("operatingCost", toDouble(r.get("OperationCost")));
        d.put("hourlyCost", toDouble(r.get("HourlyCost")));
        d.put("operationRemarks", str(r.get("OperationRemarks")));
        d.put("ActionTypeId", actionTypeId);
        return d;
    }

    /** btnDelete_Click (774:2600, 598:2688). */
    @Transactional
    public Map<String, Object> delete(W5BBomProfile p, int id) {
        UserAccount u = db.user();
        if (!db.has(p.screen, "delete")) throw new IllegalStateException("You do not have the Delete right for this screen.");
        if (id <= 0) throw new IllegalArgumentException("No record found to Delete");
        if (ownedHeader(p, u, id) == null) throw new IllegalArgumentException("Record Not Found");
        repo.deleteById(p, u.getId(), id);
        return ok(p.concrete ? "Delete Record Seccessfully" : "Delete Record Successfully", id, 0);
    }

    private Map<String, Object> ownedHeader(W5BBomProfile p, UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(p, id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != p.docType) return null;
        if (toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }
}
