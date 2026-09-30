package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpAInvTransModel;
import com.mst.models.partyprocessing.PpAStockOpeningModel;
import com.mst.repositories.partyprocessing.PpARepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.partyprocessing.PpASupport.*;

/**
 * BLL of 678 Stock Opening Balance (Party Processing) - PartyProcessing/StockOpeningBalancePartyProcessing.cs,
 * DocumentTypeId 121, ScreenName "StockOpeningBalancePartyProcessing". BLL 0305 / DAL 0308.
 */
@Service
public class PpAStockOpeningService {

    public static final int SCREEN = 678;
    public static final int DOC_TYPE = 121;
    public static final String SCREEN_NAME = "StockOpeningBalancePartyProcessing";

    @Autowired private PpARepository repo;
    @Autowired private PpASupport pp;

    /**
     * frmOpeningStockBlancing_Load: rights (Save / Update / Print / Delete / CanView AllRecord), GenerateDocNo,
     * StockPartyBind, ReferencePartyBind, Warehouse, Item, CropYear, PackingTypeFill, combojoblotfill,
     * StockPartyFillHistory.
     */
    public Map<String, Object> setup() {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> out = combos(u);
        out.put("rights", pp.rights(u, SCREEN));
        out.put("docNo", docNo(u));
        List<Map<String, Object>> hist = new ArrayList<>();
        for (Map<String, Object> r : repo.dropDownFromGrnGdnStore(u)) {
            if ("StockParty".equals(str(r.get("Activity")))) hist.add(map("Id", r.get("Id"), "name", r.get("ReferenceName")));
        }
        out.put("historyParties", hist);
        return out;
    }

    /** btnRefresh_Click: the seven entry combos again. */
    public Map<String, Object> refresh() { return combos(pp.user(SCREEN)); }

    private Map<String, Object> combos(UserAccount u) {
        return map("stockParties", pick(repo.stockParties(u), "Id", "Id", "CompanyName", "CompanyName"),
                "refParties", pick(repo.referencePartiesAll(u), "Id", "Id", "ReferencePartyName", "ReferencePartyName"),
                "warehouses", pick(repo.warehouses(u), "Id", "Id", "WareHouseName", "WareHouseName"),
                "items", pick(repo.itemsForPartyProcessing(u), "Id", "Id", "ItemName", "ItemName"),
                "cropYears", pick(repo.cropYears(u), "Id", "Id", "CropYear", "CropYear"),
                "packingTypes", pick(repo.packingTypes(), "Id", "Id", "PackTypeDesc", "PackTypeDesc"),
                "jobLots", pick(repo.jobLotsForPartyProcessing(u), "Id", "Id", "JobLotDescription", "JobLotDescription"));
    }

    /** GenerateDocNo(): GenerateCode(121, active year) - the code when > 0, else empty. */
    public Map<String, Object> newDocNo() { return map("docNo", docNo(pp.user(SCREEN))); }

    private String docNo(UserAccount u) {
        List<Map<String, Object>> r = repo.stockOpeningCode(u, DOC_TYPE, pp.financialYearId());
        int code = r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
        return code > 0 ? String.valueOf(code) : "";
    }

    /** cmbItem_Leave -> UOMFill(ItemId): GetUomScheduleByItemId (Id, UOMCode, Equivalent). */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = pp.user(SCREEN);
        return pick(repo.uomScheduleByItem(u, itemId), "Id", "Id", "UOMCode", "UOMCode", "Equivalent", "Equivalent");
    }

    /** ReadById(Id): GetByID (ActionId <> 3). */
    public Map<String, Object> record(int id) {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> r = owned(u, id);
        return map("Id", r.get("Id"), "DocNo", str(r.get("DocNo")), "DocDate", toDate(r.get("DocDate")) == null ? "" : toDate(r.get("DocDate")).toLocalDate().toString(), "WarehouseId", toInt(r.get("WarehouseId")),
                "ItemId", toInt(r.get("ItemId")), "ItemUomSch", toInt(r.get("ItemUomSch")), "JobLotId", toInt(r.get("JobLotId")),
                "PackingTypeId", toInt(r.get("PackingTypeId")), "Qty", toDouble(r.get("Qty")), "WeightKgs", toDouble(r.get("WeightKgs")),
                "Remarks", str(r.get("Remarks")), "StockPartyId", toInt(r.get("StockPartyId")), "SupplierCustomerId", toInt(r.get("SupplierCustomerId")),
                "CropYear", str(r.get("CropYear")), "uoms", uomsOf(u, toInt(r.get("ItemId"))));
    }

    private List<Map<String, Object>> uomsOf(UserAccount u, int itemId) {
        return pick(repo.uomScheduleByItem(u, itemId), "Id", "Id", "UOMCode", "UOMCode", "Equivalent", "Equivalent");
    }

    private Map<String, Object> owned(UserAccount u, int id) {
        List<Map<String, Object>> rows = repo.stockOpening(id);
        if (rows.isEmpty() || !mine(u, rows.get(0))) throw invalid("Record not found");
        return rows.get(0);
    }

    /**
     * Insert(): FormValidation() in the form's order, then the header exactly as the form fills it and
     * InvStockOpeningBalancePartyProcessing.Save -> DAL SetData in one transaction: Insert / Update, then
     * Sp_InventoryTransactionsPartyProcessing_Insert (RefDocumentTypeId 121, RefDocIdNo = id).
     */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        int id = Math.max(0, i(b, "id"));
        pp.require(u, SCREEN, id > 0 ? "Update" : "Save");
        Map<String, Object> c = combos(u);
        String docNo = s(b, "docNo");
        if (docNo.trim().isEmpty() || docNo.trim().equals("0")) throw invalid("Doc No Field Required");
        if (!in(c, "stockParties", "Id", i(b, "stockPartyId"))) throw invalid("StockParty Field Required");
        if (!in(c, "refParties", "Id", i(b, "refPartyId"))) throw invalid("Ref Party Field Required");
        if (!in(c, "warehouses", "Id", i(b, "warehouseId"))) throw invalid("WareHouse Field Required");
        if (!in(c, "items", "Id", i(b, "itemId"))) throw invalid("Item Name Field Required");
        if (!in(c, "cropYears", "Id", i(b, "cropYearId"))) throw invalid("CropYear Field Required");
        if (!in(c, "jobLots", "Id", i(b, "jobLotId"))) throw invalid("JobLot Field Required");
        int uom = i(b, "uomId");
        if (uom == 0 || !owns(uomsOf(u, i(b, "itemId")), "Id", uom)) throw invalid("Pack Uom Field Required");
        if (!in(c, "packingTypes", "Id", i(b, "packingTypeId"))) throw invalid("Packing Type Field Required");
        String qty = s(b, "qty"), weight = s(b, "weight");
        if (qty.trim().isEmpty() || cdbl(qty.trim()) == 0.0) throw invalid("Qty Field Required");
        if (weight.trim().isEmpty() || cdbl(weight.trim()) == 0.0) throw invalid("Weight Field Required");
        if (id > 0) owned(u, id);

        LocalDateTime now = LocalDateTime.now();
        LocalDateTime docDate = toDate(b.get("docDate"));
        PpAStockOpeningModel m = new PpAStockOpeningModel();
        m.Id = id;
        m.DocumentTypeId = DOC_TYPE;
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.BranchesId = toInt(u.getBranchesId());
        m.FinancialYearId = pp.financialYearId();
        m.EntryDate = now;
        m.EntryUserId = u.getId();
        m.ModifyDate = now;
        m.ModifyUserId = u.getId();
        m.IsApproved = false;
        m.ApprovedDate = now;
        m.ApprovedUserId = u.getId();
        m.DocNo = cint(docNo);
        m.DocDate = docDate == null ? now : docDate;
        m.StockPartyId = i(b, "stockPartyId");
        m.SupplierCustomerId = i(b, "refPartyId");
        m.Remarks = s(b, "remarks");
        m.WarehouseId = i(b, "warehouseId");
        m.ItemId = i(b, "itemId");
        m.CropYearId = i(b, "cropYearId");
        m.CropYear = cropText(c, m.CropYearId);
        m.JobLotId = i(b, "jobLotId");
        m.PackingTypeId = i(b, "packingTypeId");
        m.ItemUomSch = uom;
        m.Qty = cint(qty.replace(",", ""));
        m.WeightKgs = cdbl(weight);
        m.ScreenName = SCREEN_NAME;
        int n = repo.tx(() -> {
            int num = repo.set(id == 0 ? "Sp_InvStockOpeningBalancePartyProcessing_Insert" : "Sp_InvStockOpeningBalancePartyProcessing_Update", m);
            if (num > 0) m.Id = num; else num = m.Id;
            PpAInvTransModel t = new PpAInvTransModel();
            t.OrganizationId = m.OrganizationId;
            t.CompanyId = m.CompanyId;
            t.RefDocumentTypeId = m.DocumentTypeId;
            t.RefDocIdNo = num;
            repo.set("Sp_InventoryTransactionsPartyProcessing_Insert", t);
            return num;
        });
        return saved(n, id > 0 ? "Record Update Successfully " : "Record Save Successfully ");
    }

    @SuppressWarnings("unchecked")
    private static boolean in(Map<String, Object> combos, String list, String key, int id) {
        return id != 0 && owns((List<Map<String, Object>>) combos.get(list), key, id);
    }

    @SuppressWarnings("unchecked")
    private static String cropText(Map<String, Object> combos, int id) {
        for (Map<String, Object> r : (List<Map<String, Object>>) combos.get("cropYears")) if (toInt(r.get("Id")) == id) return str(r.get("CropYear"));
        return "";
    }

    /** btnDelete_Click: DeleteById(RECID, user) - the procedure raises its own refusals (approved / referred). */
    public Map<String, Object> delete(int id) {
        UserAccount u = pp.user(SCREEN);
        pp.require(u, SCREEN, "Delete");
        if (id == 0) throw invalid("Record Id Not Found....");
        owned(u, id);
        repo.deleteStockOpening(id, u.getId());
        return saved(id, "Delete Record Successfully");
    }

    /**
     * BindGridHistory() -> InvStockOpeningBalancePartyProcessing.GetAll: @OrganizationId, @CompanyId, @BranchesId,
     * @FinancialYearId, @DocumentTypeId always; the checked pair of the chosen date radio; @DocNoFrom / @DocNoTo
     * when != 0; @IsApproved (ApprovedFilter unset -> false is sent); @StockPartyId when != 0; @EntryUser when the
     * user cannot view all records; @CanViewAllRecord.
     */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = pp.user(SCREEN);
        boolean canViewAll = pp.can(u, SCREEN, "CanView AllRecord");
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("BranchesId", toInt(u.getBranchesId()));
        p.put("FinancialYearId", pp.financialYearId());
        p.put("DocumentTypeId", DOC_TYPE);
        String kind = s(f, "dateKind");
        String from = null, to = null;
        switch (kind) {
            case "entry": from = "EntryFromDate"; to = "EntryToDate"; break;
            case "modify": from = "ModifyFromDate"; to = "ModifyToDate"; break;
            case "approved": from = "ApprovedFromDate"; to = "ApprovedToDate"; break;
            default: from = "FromDate"; to = "ToDate"; break;
        }
        if (flag(f, "fromChecked") && when(f.get("fromDate")) != null) p.put(from, when(f.get("fromDate")));
        if (flag(f, "toChecked") && when(f.get("toDate")) != null) p.put(to, when(f.get("toDate")));
        int dnf = cint(f.get("docNoFrom")), dnt = cint(f.get("docNoTo"));
        if (dnf != 0) p.put("DocNoFrom", dnf);
        if (dnt != 0) p.put("DocNoTo", dnt);
        p.put("IsApproved", false);
        int sp = i(f, "stockPartyId");
        if (sp != 0) p.put("StockPartyId", sp);
        if (!canViewAll) p.put("EntryUser", u.getId());
        p.put("CanViewAllRecord", canViewAll);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.stockOpeningHistory(p)) {
            out.add(map("Id", r.get("Id"), "DocNo", r.get("DocNo"), "DocDate", r.get("DocDate"), "StockParty", r.get("StockPartyName"),
                    "RefParty", r.get("ReferenceParty"), "Warehouse", r.get("WareHouseName"), "ItemName", r.get("ItemName"),
                    "CropYear", r.get("CropYear"), "JobLot", r.get("JobLotDescription"), "PackUom", r.get("ItemUOM"),
                    "PackingType", r.get("PackTypeDesc"), "Qty", r.get("Qty"), "Weight", r.get("WeightKgs"),
                    "EntryUser", r.get("EntryUserName"), "EntryDate", r.get("EntryDate"), "ModifyUser", r.get("ModifyUserName"),
                    "ModifyDate", r.get("ModifyDate"), "ApprovedUser", r.get("ApprovedUserName"), "ApprovedDate", r.get("ApprovedDate"),
                    "Remarks", r.get("Remarks")));
        }
        return out;
    }

    /**
     * StockOpeningBalancePartyProcSlipandRegister416_01(Id) row check (only an id of this company). The CommonServices
     * body is not in the decompiled source; the check runs the same contract the print uses (see PartyprocessingPpAReports).
     */
    public Map<String, Object> printCheck(int id) {
        UserAccount u = pp.user(SCREEN);
        if (id == 0) throw invalid("Record not found");
        owned(u, id);
        Map<String, Object> p = map("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", toInt(u.getBranchesId()),
                "FinancialYearId", pp.financialYearId(), "DocumentTypeId", DOC_TYPE, "Id", id);
        if (repo.stockOpeningSlip(p).isEmpty()) throw invalid("Record Not Found For Display");
        return map("id", id);
    }
}
