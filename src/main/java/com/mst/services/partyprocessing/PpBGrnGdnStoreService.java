package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpBModels;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.partyprocessing.PpBSupport.*;

/**
 * BLL of Architecture.WinApp.PartyProcessing.frmGrnGdnStorePartyProcessing (one form, two ScreenDefinition rows):
 *
 *   679  Tag "GRNStorePartyProcessing"  DocumentTypeId 44, gate pass type 54  (?mode=grn)
 *   680  Tag "GDNStorePartyProcessing"  DocumentTypeId 45, gate pass type 55  (?mode=gdn)
 *
 * Save: InvGrnGdnStorePartyProcessing.Save (BLL 0301) -> DAL 0304 SetData, one transaction:
 * Sp_InvGrnGdnStorePartyProcessing_Insert / _Update -> Sp_InvGrnGdnStoreDetailPartyProcessing_Insert per row (removed rows first,
 * ActionTypeId 3) -> Sp_InventoryTransactionsPartyProcessing_Insert. Delete: 'DeleteById' (@Id, @EntryUserId).
 */
@Service
public class PpBGrnGdnStoreService {

    public static final int SCREEN_GRN = 679;
    public static final int SCREEN_GDN = 680;
    private static final String PROC = "Sp_InvGrnGdnStorePartyProcessing_GetAllMethod";

    @Autowired private PpBSupport pp;

    static final class Mode {
        final int screen, doc, gpDoc;
        final String screenName;
        Mode(String m) {
            boolean gdn = "gdn".equalsIgnoreCase(str(m));
            screen = gdn ? SCREEN_GDN : SCREEN_GRN;
            doc = gdn ? 45 : 44;
            gpDoc = gdn ? 55 : 54;
            screenName = gdn ? "GDNStorePartyProcessing" : "GRNStorePartyProcessing";
        }
    }

    // ------------------------------------------------------------------ Load

    /**
     * frmGrnGdnStorePartyProcessing_Load: rights (btnsave Save, btnprint Print, PrinGdn Grid Print, btnupdate Update, btnDelete Delete),
     * GenerateCode, StockPartyFill, ReferencePartyFill, WareHouseFill (InvWareHouse.Getall), ItemFill ("7,8"),
     * GetPendingGatePassForInvGrnGdnStorePartyProcessing, StockPartyFillHistory.
     */
    public Map<String, Object> setup(String mode) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        Map<String, Object> out = lists(u);
        out.put("rights", pp.rights(u, m.screen));
        out.put("documentTypeId", m.doc);
        out.put("docNo", docNo(u, m));
        out.put("pending", pending(u, m));
        out.put("historyParties", historyParties(u));
        return out;
    }

    /** toolStripButton1_Click (Refresh): StockPartyFill, ReferencePartyFill, WareHouseFill, ItemFill. */
    public Map<String, Object> refresh(String mode) {
        Mode m = new Mode(mode);
        return lists(pp.user(m.screen));
    }

    private Map<String, Object> lists(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("stockParties", pick(pp.lookups().stockParties(u), "Id", "CompanyName"));
        out.put("refParties", pick(pp.lookups().referenceParties(u), "Id", "ReferencePartyName"));
        out.put("warehouses", pick(pp.lookups().warehouses(u), "Id", "WareHouseName"));
        out.put("items", pick(pp.lookups().itemsForPartyProcessing(u, "7,8"), "Id", "ItemName"));
        return out;
    }

    /** GenerateCode(): BLL 0301 GenerateCode (@Org @Company @BranchesId @FinancialYearId @DocumentTypeId 'GenerateCode') -> DocNo; blank when 0. */
    private int docNo(UserAccount u, Mode m) {
        List<Map<String, Object>> r = pp.db().rows(PROC, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branch(u), "FinancialYearId", pp.hrm().financialYearId(), "DocumentTypeId", m.doc, "Activity", "GenerateCode");
        return r.isEmpty() ? 0 : toInt(col(r.get(0), "DocNo"));
    }

    public Map<String, Object> code(String mode) {
        Mode m = new Mode(mode);
        return map("docNo", docNo(pp.user(m.screen), m));
    }

    /** UOMFill(ItemId): CommonServices.GetUomScheduleByItemId -> UOM schedule of the item (Id, UOMCode, Equivalent). */
    public List<Map<String, Object>> uoms(String mode, int itemId) {
        Mode m = new Mode(mode);
        return pick(pp.lookups().uoms(pp.user(m.screen), itemId), "Id", "UOMCode", "Equivalent");
    }

    /**
     * GetPendingGatePassForInvGrnGdnStorePartyProcessing: GatePassPartyProcessing BLL 0298 ->
     * Sp_GatePassPartyProcessing_GetAllMethod @Org @Company @DocumentTypeId (54 / 55) 'GetPendingGatePassForInvGrnGdnStorePartyProcessing'.
     */
    private List<Map<String, Object>> pending(UserAccount u, Mode m) {
        return pick(pp.db().rows("Sp_GatePassPartyProcessing_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                        "DocumentTypeId", m.gpDoc, "Activity", "GetPendingGatePassForInvGrnGdnStorePartyProcessing"),
                "Id", "DocumentTypeId", "StockPartyId", "SupplierCustomerId", "GpSrNo", "GpDate", "VehicleType", "VehicleNo", "BiltyNo",
                "OtherRemarks", "Freight", "ItemQty", "SupplierWeight", "FactoryWeight", "DifferenceWeight", "PartyName", "ReferencePartyName",
                "DocumentTypeDescription");
    }

    public List<Map<String, Object>> pending(String mode) {
        Mode m = new Mode(mode);
        return pending(pp.user(m.screen), m);
    }

    // ------------------------------------------------------------------ history

    /** StockPartyFillHistory: SupplierCustomer.GetDataForDropDownFromGRNGDN(org, co) -> rows with Activity "StockParty" (Id / ReferenceName). */
    private List<Map<String, Object>> historyParties(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : pp.db().rows("USP_GetDataForDropDownFromGrnGdnStorePartyProcessing", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId())) {
            if ("StockParty".equals(str(col(r, "Activity")))) out.add(map("Id", col(r, "Id"), "name", col(r, "ReferenceName")));
        }
        return out;
    }

    public List<Map<String, Object>> historyParties(String mode) {
        Mode m = new Mode(mode);
        return historyParties(pp.user(m.screen));
    }

    /**
     * GridFill(): BLL 0301 FormHistory -> Sp_InvGrnGdnStorePartyProcessing_GetAllMethod 'FormHistory' (@BranchesId, @FinancialYearId,
     * @DocumentTypeId always; the date pair by the radio incl. Approved Date; doc no range; stock party; @EntryUser when the user
     * cannot view all records).
     */
    public List<Map<String, Object>> history(String mode, Map<String, Object> b) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        boolean all = pp.canViewAll(u, m.screen);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("BranchesId", branch(u));
        p.put("FinancialYearId", pp.hrm().financialYearId());
        p.put("DocumentTypeId", m.doc);
        dateFilter(p, b, "FromDate", "ToDate", "EntryFromDate", "EntryToDate", "ModifyFromDate", "ModifyToDate", "ApprovedFromDate", "ApprovedToDate");
        p.put("DocNoFrom", nz0(toInt(b.get("fromDocNo"))));
        p.put("DocNoTo", nz0(toInt(b.get("toDocNo"))));
        p.put("StockPartyId", nz0(toInt(b.get("stockPartyId"))));
        if (!all) p.put("EntryUser", u.getId());
        p.put("CanViewAllRecord", all);
        p.put("Activity", "FormHistory");
        return pick(pp.db().rows(PROC, p), "Id", "DocumentTypeId", "DocNo", "DocDate", "StockParty", "ReferenceParty=ReferencePartyName",
                "VehicleNo", "BiltyNo", "RefDocement=ReferenceDocNo", "EntryUser=EntryUserName", "EntryDate", "ModifyUser=ModifyUserName",
                "ModifyDate", "ApprovedUser=ApprovedUserName", "ApprovedDate", "NoOfAttachments", "Remarks=RemarksHeader");
    }

    // ------------------------------------------------------------------ read

    /** ReadById / grdHistory_SelectionChanged: BLL 0301 GetByID -> 'ReadById' header + 'ReadDetailByHeaderId' rows. */
    public Map<String, Object> byId(String mode, int id) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        return read(u, m, id);
    }

    private Map<String, Object> read(UserAccount u, Mode m, int id) {
        List<Map<String, Object>> h = pp.db().rows(PROC, "Id", id, "Activity", "ReadById");
        if (h.isEmpty()) throw invalid("Record not found");
        Map<String, Object> r = h.get(0);
        if (col(r, "CompanyId") != null && toInt(col(r, "CompanyId")) != u.getCompanyId()) throw invalid("Record not found");
        if (col(r, "DocumentTypeId") != null && toInt(col(r, "DocumentTypeId")) != m.doc) throw invalid("Record not found");
        Map<String, Object> out = pick(h, "Id", "DocNo", "DocDate", "StockPartyId", "ReferencePartyId", "VehicleNo", "BiltyNo",
                "RefDocumentTypeId", "DocumentTypeDescription", "RefEntryIdNo", "RefDoNo", "RemarksHeader").get(0);
        out.put("details", pick(pp.db().rows(PROC, "Id", id, "Activity", "ReadDetailByHeaderId"), "Id", "WarehouseId", "WareHouseName",
                "ItemId", "ItemName", "ItemUomId", "UOMCode", "ItemQty", "RemarksSub"));
        return out;
    }

    // ------------------------------------------------------------------ save

    /**
     * Insert(): FormValidation() in the form's order and wording, "Grid record not found", then the model exactly as the form fills it
     * (ApprovedDate / Entry / Modify now, users = the signed-in user, RefDoNo = the document no combo text) and the DAL transaction.
     * On a new document the gate pass must be one of this company's pending gate passes (grdGatePass Load); on update the
     * reference stays the saved one (the form binds only that row).
     */
    public Map<String, Object> save(String mode, Map<String, Object> b) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        int recId = Math.max(0, toInt(b.get("id")));
        pp.hrm().require(u, m.screen, recId > 0 ? "Update" : "Save");
        if (str(b.get("docNo")).isEmpty()) throw invalid("DocNo Required");
        if (toInt(b.get("stockPartyId")) == 0) throw invalid("Stock Party Required");
        if (toInt(b.get("referencePartyId")) == 0) throw invalid("Reference Party Required");
        if (str(b.get("vehicleNo")).isEmpty()) throw invalid("Vehicle No Required");
        if (toInt(b.get("refDocumentTypeId")) == 0) throw invalid("Reference Document Required");
        if (toInt(b.get("refEntryIdNo")) == 0) throw invalid("Reference Document No Required");

        int refDocType, refEntry, refDoNo;
        Map<String, Object> existing = recId > 0 ? read(u, m, recId) : null;
        if (existing != null) {
            refDocType = toInt(existing.get("RefDocumentTypeId"));
            refEntry = toInt(existing.get("RefEntryIdNo"));
            refDoNo = toInt(existing.get("RefDoNo"));
        } else {
            Map<String, Object> gp = null;
            for (Map<String, Object> r : pending(u, m)) if (toInt(r.get("Id")) == toInt(b.get("refEntryIdNo"))) gp = r;
            if (gp == null) throw invalid("Reference Document No Required");
            refDocType = toInt(gp.get("DocumentTypeId"));
            refEntry = toInt(gp.get("Id"));
            refDoNo = toInt(gp.get("GpSrNo"));
        }

        List<Map<String, Object>> rows = list(b.get("rows"));
        List<Map<String, Object>> removed = list(b.get("removed"));
        List<Integer> ownDetailIds = new ArrayList<>();
        if (existing != null) for (Map<String, Object> d : list(existing.get("details"))) ownDetailIds.add(toInt(col(d, "Id")));

        PpBModels.GrnGdnStorePartyProcessing h = new PpBModels.GrnGdnStorePartyProcessing();
        h.Id = recId;
        h.DocNo = toInt(b.get("docNo"));
        LocalDateTime docDate = toDate(b.get("docDate"));
        LocalDateTime now = LocalDateTime.now();
        h.DocDate = docDate == null ? now : docDate;
        h.VehicleNo = str(b.get("vehicleNo"));
        h.BiltyNo = str(b.get("biltyNo"));
        h.RefDoNo = refDoNo;
        h.DocumentTypeId = m.doc;
        h.RefDocumentTypeId = refDocType;
        h.RefEntryIdNo = refEntry;
        h.ReferencePartyId = toInt(b.get("referencePartyId"));
        h.StockPartyId = toInt(b.get("stockPartyId"));
        h.RemarksHeader = str(b.get("remarks"));
        h.ApprovedDate = now;
        h.ApprovedUserId = u.getId();
        h.CompanyId = u.getCompanyId();
        h.OrganizationId = u.getOrganizationId();
        h.FinancialYearId = pp.hrm().financialYearId();
        h.BranchesId = branch(u);
        h.EntryDate = now;
        h.EntryUser = u.getId();
        h.ModifyDate = now;
        h.ModifyUser = u.getId();
        h.ScreenName = m.screenName;

        List<PpBModels.GrnGdnStoreDetailPartyProcessing> details = new ArrayList<>();
        if (recId > 0) {
            for (Map<String, Object> r : removed) {
                int did = toInt(r.get("Id"));
                if (did <= 0 || !ownDetailIds.contains(did)) continue;
                PpBModels.GrnGdnStoreDetailPartyProcessing d = new PpBModels.GrnGdnStoreDetailPartyProcessing();
                d.Id = did;
                d.WarehouseId = toInt(r.get("WareHouseId"));
                d.ItemId = toInt(r.get("ItemId"));
                d.ItemUomId = toInt(r.get("UOM"));            // desktop reads the "UOM" (code text) cell -> 0
                d.ItemQty = toDec(r.get("Qty"));
                d.RemarksSub = str(r.get("Remarks"));
                d.ActionTypeId = 3;
                details.add(d);
            }
        }
        if (rows.isEmpty()) throw invalid("Grid record not found");
        for (Map<String, Object> r : rows) {
            PpBModels.GrnGdnStoreDetailPartyProcessing d = new PpBModels.GrnGdnStoreDetailPartyProcessing();
            d.Id = toInt(r.get("Id"));
            if (d.Id > 0 && !ownDetailIds.contains(d.Id)) d.Id = 0;
            d.ActionTypeId = d.Id <= 0 ? 1 : 2;
            d.WarehouseId = toInt(r.get("WareHouseId"));
            d.ItemId = toInt(r.get("ItemId"));
            d.ItemUomId = toInt(r.get("UOMId"));
            d.ItemQty = toDec(r.get("Qty"));
            d.RemarksSub = str(r.get("Remarks"));
            details.add(d);
        }
        int saved = pp.db().tx(() -> {
            int num = pp.db().set(h.Id == 0 ? "Sp_InvGrnGdnStorePartyProcessing_Insert" : "Sp_InvGrnGdnStorePartyProcessing_Update", h);
            if (num > 0) h.Id = num; else num = h.Id;
            for (PpBModels.GrnGdnStoreDetailPartyProcessing d : details) {
                d.InvGrnStorePartyProcessingId = h.Id;
                pp.db().set("Sp_InvGrnGdnStoreDetailPartyProcessing_Insert", d);
            }
            PpBModels.InventoryTransactionsPartyProcessing t = new PpBModels.InventoryTransactionsPartyProcessing();
            t.OrganizationId = h.OrganizationId; t.CompanyId = h.CompanyId; t.RefDocumentTypeId = h.DocumentTypeId; t.RefDocIdNo = num;
            pp.db().set("Sp_InventoryTransactionsPartyProcessing_Insert", t);
            return num;
        });
        return saved(saved, recId > 0 ? "Update Successfully" : "Save Successfully");
    }

    @SuppressWarnings("unchecked")
    static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    // ------------------------------------------------------------------ delete / print

    /** btnDelete_Click: "Record Id Not Found...." / BLL 0301 DeleteById (@Id, @EntryUserId, 'DeleteById') -> "Delete Record Successfully". */
    public Map<String, Object> delete(String mode, int id) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        pp.hrm().require(u, m.screen, "Delete");
        if (id == 0) throw invalid("Record Id Not Found....");
        read(u, m, id);
        pp.db().tx(() -> pp.db().rows(PROC, "Id", id, "EntryUserId", u.getId(), "Activity", "DeleteById"));
        return saved(id, "Delete Record Successfully");
    }

    /**
     * GenerateReport(PrintId) -> CommonServices.PartyProcessingGRNSlip332 (44) / PartyProcessingGDNSlip332_01 (45): the slip data is
     * PartyProcessingGatePassReports.PartyProcessingGrnGdnStoreSlipandRegister -> Sp_InvGrnGdnStorePartyProcessing_SlipandRegister
     * (@Org @Company @FinancialYearId @BranchesId @DocumentTypeId @Id). The .rpt file names are not in the available sources, so the
     * page prints these rows through the grid-to-PDF printer.
     */
    public List<Map<String, Object>> slip(String mode, int id, boolean grid) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        pp.hrm().require(u, m.screen, grid ? "Grid Print" : "Print");
        read(u, m, id);
        List<Map<String, Object>> rows = pp.db().rows("Sp_InvGrnGdnStorePartyProcessing_SlipandRegister", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "FinancialYearId", pp.hrm().financialYearId(), "BranchesId", branch(u), "DocumentTypeId", m.doc, "Id", id);
        if (rows.isEmpty()) throw invalid("No Record Found For Display");
        return rows;
    }
}
