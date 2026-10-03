package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportSalesContractRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportGSupport.*;
import static com.mst.services.ExportSalesContractService.*;

/**
 * The BLL side of 240 frmExportSalesContractPmDetail "Packing Detail By Export Contract"
 * (Architecture.WinApp.Export), ClientSize 1234 x 711, history DocumentTypeIds "202". The form picks a
 * Sales Contract (CommonServices.GetLcOrderNo, or the Load Contract popup LoadSalesContractForPmDetail),
 * reads it with ExImLcOrder.GetByID, keeps its contract detail (CurrentOrderDetailRecord) for the Brand
 * combo and the PM quantity checks, shows the saved packing material rows, auto-fills rows from
 * ItemAndPMItemMap, and saves the PM grid alone with ExImLcOrder.SaveForPmDetail (delete + re-insert).
 *
 * Rights: View; Update button enabled with Update OR Save right (btnUpdate.Enabled =
 * DoHaveUpdateRights || DoHaveSaveRight); Print (501); CanViewAllRecord for the history filter.
 */
@Service
public class ExportContractPmDetailService {

    public static final int SCREEN_ID = 240;
    public static final int DOCUMENT_TYPE_ID = 202;

    @Autowired private ExportSalesContractRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    private int financialYearId() { return currentUserContext.currentFinancialYearId(); }

    /** frmExportSalesContractPmDetail_Load. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("defaultDaysToLessFromHistoryFromDate", asInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        c.put("itemSearchByCode", asBool(repo.config(u, "ItemSearchByCode")));
        c.put("fcyDecimals", asInt(repo.config(u, "DefaultNoOfDecimalPointsForFcyAmount")));
        out.put("config", c);
        out.putAll(refreshData(u));
        try { out.put("historyCustomers", pairs(repo.historyDropDowns(u, null), "Id", "name", "Customer")); } catch (Exception e) { out.put("historyCustomers", new ArrayList<>()); }
        return out;
    }

    /** btnRefresh_Click: ContractNoFill, PackingItemFill, ItemAndPMItemMap. */
    public Map<String, Object> refresh() { return refreshData(user("View")); }

    private Map<String, Object> refreshData(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            List<Map<String, Object>> c = new ArrayList<>();
            for (Map<String, Object> r : repo.lcOrderNos(u, 0)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("name", text(ci(r, "LcOrderNo")));
                m.put("PartyName", text(ci(r, "PartyName")));
                c.add(m);
            }
            out.put("contracts", c);
        } catch (Exception e) { out.put("contracts", new ArrayList<>()); out.put("contractsError", msg(e)); }
        try {
            List<Map<String, Object>> pm = new ArrayList<>();
            for (Map<String, Object> r : repo.itemsByType14(u)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("ItemName", text(ci(r, "ItemName")));
                m.put("ItemCode", text(ci(r, "ItemCodeNew")));
                pm.add(m);
            }
            out.put("pmItems", pm);
        } catch (Exception e) { out.put("pmItems", new ArrayList<>()); out.put("pmItemsError", msg(e)); }
        try {
            List<Map<String, Object>> map = new ArrayList<>();
            for (Map<String, Object> r : repo.itemAndPmItemMap(u)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("ItemId", asInt(ci(r, "ItemId")));
                m.put("PmItemId", asInt(ci(r, "PmItemId")));
                m.put("PmItemCode", text(ci(r, "PmItemCode")));
                m.put("PmItemName", text(ci(r, "PmItemName")));
                m.put("PmBaseUomId", asInt(ci(r, "PmBaseUomId")));
                m.put("PmBaseUomCode", text(ci(r, "PmBaseUomCode")));
                map.add(m);
            }
            out.put("itemPmMap", map);
        } catch (Exception e) { out.put("itemPmMap", new ArrayList<>()); }
        return out;
    }

    /** HistoryCombosFill: GetDataForDropDownFromExportContract (no Activity) kept to ActivityType "Customer". */
    private static List<Map<String, Object>> pairs(List<Map<String, Object>> rows, String idCol, String nameCol, String activity) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!activity.equals(text(ci(r, "ActivityType")))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, idCol)));
            m.put("name", text(ci(r, nameCol)));
            out.add(m);
        }
        return out;
    }

    public Map<String, Object> historyCombos() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("historyCustomers", pairs(repo.historyDropDowns(user("View"), null), "Id", "name", "Customer"));
        return out;
    }

    /** BindPmItemPackUom: dtUomFromGloablUomScheduleByItemId(PM item). */
    public List<Map<String, Object>> pmUoms(int itemId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.globalUoms(u, itemId)) out.add(uomRow(r));
        return out;
    }

    /**
     * ReadById(ID): the contract header (for the title), CurrentOrderDetailRecord (the contract detail
     * rows FilldtBrandForPMItem groups by ExImItemId / InvPackingMaterialTypeId / UOMScheduleIdOuter), the
     * saved PM rows and the dtBrandForPMItem groups.
     */
    public Map<String, Object> readById(int id) {
        user("View");
        List<Map<String, Object>> hdr = repo.header(id);
        if (hdr.isEmpty()) throw new IllegalArgumentException("Record not found");
        int docType = asInt(ci(hdr.get(0), "DocumentTypeId"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", header(hdr.get(0)));
        List<Map<String, Object>> details = detailRows(repo.detail(id, docType));
        out.put("details", details);
        out.put("brandItems", brandItems(details));
        out.put("pmRows", pmRows(repo.packingMaterialDetail(id)));
        return out;
    }

    /** FilldtBrandForPMItem over CurrentOrderDetailRecord - Id, ItemName, ItemCode, PackingTypeId, PackingType, PackUomId, PackUom, OuterQty, InnerQty, M_Ton. */
    static List<Map<String, Object>> brandItems(List<Map<String, Object>> details) {
        Map<String, Map<String, Object>> acc = new LinkedHashMap<>();
        for (Map<String, Object> r : details) {
            String key = asInt(r.get("ItemId")) + "|" + asInt(r.get("PackTypeId")) + "|" + asInt(r.get("PackSizeId"));
            double bags = asDouble(r.get("NoOfBags")), mton = asDouble(r.get("QtyMTon"));
            Map<String, Object> m = acc.get(key);
            if (m == null) {
                m = new LinkedHashMap<>();
                m.put("Id", asInt(r.get("ItemId")));
                m.put("ItemName", text(r.get("ItemName")));
                m.put("ItemCode", text(r.get("ItemCode")));
                m.put("PackingTypeId", asInt(r.get("PackTypeId")));
                m.put("PackingType", text(r.get("PackType")));
                m.put("PackUomId", asInt(r.get("PackSizeId")));
                m.put("PackUom", text(r.get("PackSize")));
                m.put("bags", bags);
                m.put("eq", asDouble(r.get("QtyEquivalent")));
                m.put("M_Ton", mton);
                acc.put(key, m);
            } else {
                m.put("bags", asDouble(m.get("bags")) + bags);
                m.put("M_Ton", asDouble(m.get("M_Ton")) + mton);
            }
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> m : acc.values()) {
            double bags = asDouble(m.remove("bags")), eq = asDouble(m.remove("eq"));
            m.put("OuterQty", bags);
            m.put("InnerQty", bags * eq);
            out.add(m);
        }
        return out;
    }

    /**
     * Insert(): "Sale ContractNo Is required", "RecId not found..." when nothing is loaded,
     * CheckBomHeaderIds against the contract detail, the PM row checks, FilldtBrandForPMItem +
     * PmGridOuter/InnerQtyValidation, "Packing Material Grid record not found", SaveForPmDetail;
     * "Record Save Successfully" (RecId was 0 - unreachable, RecId > 0 is required) / "Record Update Successfully".
     */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = user("View");
        /* btnUpdate.Enabled = DoHaveUpdateRights || DoHaveSaveRight */
        if (!allowed(u, "Update") && !allowed(u, "Save")) throw new AccessDeniedException("The user does not have Update rights for this report");
        int recId = asInt(b.get("recId"));
        if (asInt(b.get("contractId")) == 0) throw new IllegalArgumentException("Sale ContractNo Is required");
        if (recId <= 0) throw new IllegalArgumentException("RecId not found...");
        List<Map<String, Object>> hdr = repo.header(recId);
        if (hdr.isEmpty()) throw new IllegalArgumentException("Record not found");
        List<Map<String, Object>> details = detailRows(repo.detail(recId, asInt(ci(hdr.get(0), "DocumentTypeId"))));
        List<Map<String, Object>> pm = list(b.get("pmRows"));
        checkBomHeaderIds(details, pm, "Contract Detail Grid");
        if (pm.isEmpty()) throw new IllegalArgumentException("Packing Material Grid record not found");
        List<Map<String, Object>> models = new ArrayList<>();
        int i = 0;
        for (Map<String, Object> r : pm) models.add(pmModel(r, asInt(r.get("Id")), i++, false));
        pmGridValidation(brandForPmItem(details, "PackSizeId", "NoOfBags"), pm);
        int id = repo.saveForPmDetail(recId, models);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("message", "Record Update Successfully");
        return out;
    }

    /** HistoryFill - DocumentTypeIds "202", the same filters as the contract form (no Approved radio shown). */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = user("View");
        boolean canViewAll = allowed(u, "CanViewAllRecord");
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("DocumentTypeIds", String.valueOf(DOCUMENT_TYPE_ID));
        p.put("CanViewAllRecord", canViewAll);
        int fy = financialYearId();
        if (fy != 0) p.put("FinancialYearId", fy);
        if (!canViewAll) p.put("EntryUser", u.getId());
        java.time.LocalDate from = asBool(f.get("fromChecked")) ? asDate(f.get("fromDate")) : null;
        java.time.LocalDate to = asBool(f.get("toChecked")) ? asDate(f.get("toDate")) : null;
        String by = text(f.get("dateBy"));
        String fromKey = "FromDate", toKey = "ToDate";
        if ("entry".equals(by)) { fromKey = "EntryFromDate"; toKey = "EntryToDate"; }
        else if ("modify".equals(by)) { fromKey = "ModifyFromDate"; toKey = "ModifyToDate"; }
        else if ("approved".equals(by)) { fromKey = "ApprovedFromDate"; toKey = "ApprovedToDate"; }
        if (from != null) p.put(fromKey, sqlDate(from));
        if (to != null) p.put(toKey, sqlDate(to));
        int fromDoc = asInt(f.get("fromDocNo")), toDoc = asInt(f.get("toDocNo"));
        if (fromDoc != 0) p.put("DocNoFrom", (double) fromDoc);
        if (toDoc != 0) p.put("DocNoTo", (double) toDoc);
        int cust = asInt(f.get("customerId"));
        if (cust != 0) p.put("SupplierCustomerId", cust);
        return historyRows(repo.formHistory(p));
    }

    /** GetDetailGrdByHeadId - the PM rows of the selected history row (only when the contract has detail rows). */
    public List<Map<String, Object>> historyDetail(int id) {
        user("View");
        List<Map<String, Object>> hdr = repo.header(id);
        if (hdr.isEmpty()) return new ArrayList<>();
        if (repo.detail(id, asInt(ci(hdr.get(0), "DocumentTypeId"))).isEmpty()) return new ArrayList<>();
        return pmRows(repo.packingMaterialDetail(id));
    }

    static Map<String, Integer> count(List<Map<String, Object>> rows, String col) {
        Map<String, Integer> m = new HashMap<>();
        for (Map<String, Object> r : rows) m.merge(text(r.get(col)), 1, Integer::sum);
        return m;
    }
}
