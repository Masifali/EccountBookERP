package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpCModels.Adjustment;
import com.mst.models.partyprocessing.PpCModels.AdjustmentDetail;
import com.mst.models.partyprocessing.PpCModels.InvTrans;
import com.mst.models.partyprocessing.PpCModels.InvTransFifo;
import com.mst.repositories.partyprocessing.PpCRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.partyprocessing.PpCSupport.*;

/**
 * 604 Stock Adjustment (Party Processing) - Architecture.WinApp.PartyProcessing.StockAdjustment.cs,
 * BLL 0296 / DAL 0299 InvStockAdjustmentPartyProcessing (DocumentTypeId 158).
 *
 * The grid (the form's DataTable "table") lives in the page; Save / Update post it with the rows removed from a
 * loaded record (lstRemoveRecords, ActionTypeId 3). The service runs Insert() from its first check: empty grid,
 * FormValidation, the row loop's "... in Grid row No : n" messages, the Gain / Loss clearing of the reference ids,
 * then BLL Save and DAL SetData: header, details (LineId 1..n), and
 *   Loss (2) without loader rows -> FIFO (CommonServices.FIFOImplementionForPartyProcessing) and
 *                                   USP_InventoryTransactionsPartyProcessing_Insert per FIFO row
 *                                   (after USP_InventoryPartyProcessingQtyReverseAndDeleteByReferenceId on an update)
 *   Loss with loader rows / Gain (1) -> Sp_InventoryTransactionsPartyProcessing_Insert
 *   Loss -> USP_InventoryValidationPartyProcessing per kept row,
 * all in one transaction. Attachments (DMS) are not ported. Web deviation: the FIFO reads run on the save's
 * connection (the desktop reads them on a second connection while its transaction is open).
 */
@Service
public class PpCStockAdjustmentService {

    public static final int SCREEN = 604;
    public static final int DOC_TYPE = 158;

    @Autowired private PpCRepository repo;
    @Autowired private PpCSupport pp;

    /** frmStockAdjustment_Load. */
    public Map<String, Object> setup() {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> out = lookups(u);
        out.put("rights", pp.rights(u, SCREEN));
        out.put("docNo", repo.adjustmentCode(u, pp.year()));
        out.put("entryTypes", project(repo.staticColumns("StockAdjustmentType"), "Id", "Type"));
        out.putAll(historyCombos(u));
        return out;
    }

    /** btnRefresh_Click: StockPartyBind, WareHouseFill, ItemBind, CmbPackingTypeFill, CropYearFill, JobLotFill. */
    public Map<String, Object> lookups(UserAccount u) {
        return m("stockParties", project(repo.stockParties(u), "Id", "CompanyName"),
                "warehouses", project(repo.activeWarehouses(u), "Id", "WareHouseName"),
                "items", project(repo.itemsForPartyProcessing(u, "1,2,4"), "Id", "ItemName", "ItemCode"),
                "cropYears", project(repo.cropYears(u), "Id", "CropYear"),
                "jobLots", project(repo.jobLots(u), "Id", "JobLotDescription"),
                "packingTypes", project(repo.packingTypes(), "Id", "PackTypeDesc"));
    }

    public Map<String, Object> refresh() { return lookups(pp.user(SCREEN)); }

    public Map<String, Object> code() { UserAccount u = pp.user(SCREEN); return m("docNo", repo.adjustmentCode(u, pp.year())); }

    /** bindRateUomAndItemPackUom: GetUomScheduleByItemId (Id, UOMCode, Equivalent - SelectedRow.Cells[2]). */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = pp.user(SCREEN);
        return project(repo.uomSchedule(u, itemId), "Id", "UOMCode", "Equivalent");
    }

    /** AvailableStock: GetStockInHandFromInventoryTrasactions(item, DocDate, job lot, warehouse, crop year text). */
    public Map<String, Object> stock(int itemId, String docDate, int jobLotId, int warehouseId, String cropYear) {
        UserAccount u = pp.user(SCREEN);
        return m("balance", repo.stockInHand(u, itemId, picker(docDate), jobLotId, warehouseId, cropYear == null ? "" : cropYear));
    }

    /** HistoryCombosFill: Activity "EntryType" / "StockParty" rows (ReferenceName). */
    public Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> types = new ArrayList<>(), parties = new ArrayList<>();
        for (Map<String, Object> r : repo.adjustmentDropDown(u)) {
            String a = str(r.get("Activity"));
            if ("EntryType".equals(a)) types.add(m("Id", r.get("Id"), "Name", r.get("ReferenceName")));
            else if ("StockParty".equals(a)) parties.add(m("Id", r.get("Id"), "Name", r.get("ReferenceName")));
        }
        return m("historyEntryTypes", types, "historyParties", parties);
    }

    public Map<String, Object> historyCombosApi() { return historyCombos(pp.user(SCREEN)); }

    /** ReadById / GridDetailBind: GetByID header + ReadDetailByHeaderId rows, as the form's grid table. */
    public Map<String, Object> read(int id) {
        UserAccount u = pp.user(SCREEN);
        List<Map<String, Object>> h = repo.adjustment(id);
        if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) throw invalid("Record not found.");
        Map<String, Object> x = h.get(0);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.adjustmentDetails(id)) {
            rows.add(m("Id", toInt(d.get("Id")), "RefDocumentTypeId", toInt(d.get("RefDocumentTypeId")), "RefDocIdNo", toInt(d.get("RefDocNoId")),
                    "RefDocSubIdNo", toInt(d.get("RefDocSubIdNo")), "WareHouseId", toInt(d.get("WarehouseId")), "WareHouse", str(d.get("WareHouseName")),
                    "ItemId", toInt(d.get("ItemId")), "ItemCode", str(d.get("ItemCode")), "Item", str(d.get("ItemName")),
                    "CropYearId", toInt(d.get("CropYearId")), "CropYear", str(d.get("CropYear")), "JobLotId", toInt(d.get("JobLotId")),
                    "JobLot", str(d.get("JobLot")), "PackingTypeId", toInt(d.get("PackingTypeId")), "PackingType", str(d.get("PackingType")),
                    "ItemUOMId", toInt(d.get("PackUomId")), "ItemUOM", str(d.get("PackUom")), "Equivalent", dbl(d.get("PackEquivalent")),
                    "BalQty", dbl(d.get("Qty")), "BalWeight", dbl(d.get("NetWeight")), "ItemQty", dbl(d.get("Qty")), "Weight", dbl(d.get("NetWeight")),
                    "Comments", str(d.get("RemarksDetail"))));
        }
        return m("Id", toInt(x.get("Id")), "DocDate", x.get("DocDate"), "DocNo", str(x.get("DocNo")), "AdjustmentTypeId", toInt(x.get("AdjustmentTypeId")),
                "StockPartyId", toInt(x.get("StockPartyId")), "RemarksHeader", str(x.get("RemarksHeader")), "rows", rows);
    }

    /** Insert(). */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        int recId = Math.max(0, toInt(b.get("id")));
        pp.require(u, SCREEN, recId > 0 ? "Update" : "Save");
        List<Map<String, Object>> rows = list(b.get("rows"));
        if (rows.isEmpty()) throw invalid("Grid Record Not Found");
        String docNo = trim(b.get("docNo"));
        if (docNo.isEmpty() || docNo.equals("0")) throw invalid("DocNo Field is Required");
        int entryType = toInt(b.get("entryTypeId"));
        if (entryType == 0) throw invalid("EntryType Field is Required");
        int stockParty = toInt(b.get("stockPartyId"));
        if (stockParty == 0) throw invalid("Stock Party Field is Required");
        if (recId > 0) {
            List<Map<String, Object>> h = repo.adjustment(recId);
            if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) throw invalid("Record Not Update because RecId Not Found");
        }
        Adjustment o = new Adjustment();
        if (recId > 0) { o.Id = recId; o.ModifyUserId = uid(u); }
        o.OrganizationId = u.getOrganizationId();
        o.CompanyId = u.getCompanyId();
        o.BranchesId = branch(u);
        o.FinancialYearId = pp.year();
        o.DocumentTypeId = DOC_TYPE;
        o.DocDate = orNow(picker(b.get("docDate")));
        o.DocNo = toInt(docNo);
        o.RemarksHeader = str(b.get("remarks"));
        o.AdjustmentTypeId = entryType;
        o.StockPartyId = stockParty;
        o.EntryUserId = uid(u);
        o.ModifyUserId = uid(u);
        o.ApprovalUserId = uid(u);
        if (recId > 0) for (Map<String, Object> r : list(b.get("removed"))) o.details.add(detail(r, 3));
        int n = 1;
        for (Map<String, Object> r : rows) {
            if (toInt(r.get("WareHouseId")) == 0) throw invalid("WareHouseName field is required in Grid row No : " + n);
            if (toInt(r.get("ItemId")) == 0) throw invalid("ItemName field is required in Grid row No : " + n);
            if (toInt(r.get("CropYearId")) == 0) throw invalid("CropYear field is required in Grid row No : " + n);
            if (toInt(r.get("JobLotId")) == 0) throw invalid("JobLot field is required in Grid row No : " + n);
            if (toInt(r.get("PackingTypeId")) == 0) throw invalid("Packingtype field is required in Grid row No : " + n);
            if (toInt(r.get("ItemUOMId")) == 0) throw invalid("PackUOM field is required in Grid row No : " + n);
            if (dbl(r.get("ItemQty")) == 0d) throw invalid("ItemQty field is required in Grid row No : " + n);
            if (dbl(r.get("Weight")) == 0d) throw invalid("Weight field is required in Grid row No : " + n);
            AdjustmentDetail d = detail(r, 0);
            d.ActionTypeId = d.Id <= 0 ? 1 : 2;
            if (entryType == 1) { d.RefDocNoId = 0; d.RefDocumentTypeId = 0; d.RefDocSubIdNo = 0; }
            o.details.add(d);
            n++;
        }
        /* BLL Save */
        if (o.Id == 0) {
            for (AdjustmentDetail d : o.details) if (d.ActionTypeId != 1) throw invalid("Record cannot be inserted because ActionTypeId not equal to 1");
            o.ActionId = 1;
            o.ModifyUserId = 0;
        } else {
            o.ActionId = 2;
            o.EntryUserId = 0;
        }
        int id = repo.tx(() -> setData(u, o));
        return saved(id, (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + o.DocNo);
    }

    private static AdjustmentDetail detail(Map<String, Object> r, int actionType) {
        AdjustmentDetail d = new AdjustmentDetail();
        d.Id = toInt(r.get("Id"));
        d.RefDocNoId = toInt(r.get("RefDocIdNo"));
        d.RefDocumentTypeId = toInt(r.get("RefDocumentTypeId"));
        d.RefDocSubIdNo = toInt(r.get("RefDocSubIdNo"));
        d.WarehouseId = toInt(r.get("WareHouseId"));
        d.ItemId = toInt(r.get("ItemId"));
        d.CropYearId = toInt(r.get("CropYearId"));
        d.JobLotId = toInt(r.get("JobLotId"));
        d.PackingTypeId = toInt(r.get("PackingTypeId"));
        d.PackUomId = toInt(r.get("ItemUOMId"));
        d.Qty = dbl(r.get("ItemQty"));
        d.NetWeight = dbl(r.get("Weight"));
        d.RemarksDetail = str(r.get("Comments"));
        d.ActionTypeId = actionType;
        return d;
    }

    /** DAL InvStockAdjustmentPartyProcessing.SetData (usp_InvStockAdjustmentPartyProcessing_Insert for insert and update). */
    private int setData(UserAccount u, Adjustment o) {
        if (o.details.isEmpty()) throw invalid("InvStockAdjustmentDetailPartyProcessingList not found");
        int num = repo.set("usp_InvStockAdjustmentPartyProcessing_Insert", o);
        if (num > 0) o.Id = num; else num = o.Id;
        int line = 1;
        boolean loader = false;
        for (AdjustmentDetail d : o.details) {
            if (o.AdjustmentTypeId == 2 && d.RefDocumentTypeId > 0 && d.RefDocNoId > 0 && d.RefDocSubIdNo > 0) loader = true;
            d.InvStockAdjustmentId = o.Id;
            d.LineId = line;
            d.Id = repo.set("usp_InvStockAdjustmentDetailPartyProcessing_Insert", d);
            line++;
        }
        if (o.AdjustmentTypeId == 2 && !loader) {
            List<Map<String, Object>> gl = repo.itemGl(u);
            List<InvTransFifo> fifo = new ArrayList<>();
            for (AdjustmentDetail d : o.details) {
                if (d.ActionTypeId == 3) continue;
                Map<String, Object> item = find(gl, "Id", d.ItemId);
                if (item == null) continue;
                FifoArgs a = new FifoArgs();
                a.itemId = d.ItemId; a.stockPartyId = o.StockPartyId; a.warehouseId = d.WarehouseId; a.docDate = o.DocDate;
                a.stockUom = d.PackUomId; a.jobLotId = d.JobLotId; a.cropYearId = d.CropYearId; a.packingTypeId = d.PackingTypeId;
                if (o.ModifyUserId > 0) { a.documentTypeId = o.DocumentTypeId; a.id = o.Id; }
                a.itemQty = d.Qty; a.netWeight = d.NetWeight; a.lineId = d.LineId; a.itemName = str(item.get("ItemName"));
                for (InvTransFifo t : pp.fifo(u, a, fifo)) {
                    t.TranRemarks = o.AdjustmentTypeId == 1 ? "Gain Stock Adjustment Party Processing" : "Loss Stock Adjustment Party Processing";
                    t.DetailRemarks = d.RemarksDetail;
                    fifo.add(t);
                }
            }
            if (!fifo.isEmpty()) {
                if (o.ModifyUserId > 0) repo.reverseAndDelete(u, o.DocumentTypeId, o.Id);
                LocalDateTime now = LocalDateTime.now();
                for (InvTransFifo t : fifo) {
                    t.OrganizationId = o.OrganizationId; t.CompanyId = o.CompanyId; t.DocDate = o.DocDate; t.DocCodeNo = o.DocNo;
                    t.StockPartyId = o.StockPartyId; t.BranchesId = o.BranchesId; t.RefDocumentTypeId = o.DocumentTypeId; t.CalcType = "Weight";
                    t.EntryUserId = o.EntryUserId; t.ModifyUserId = o.ModifyUserId; t.EntryDate = now; t.ModifyDate = now;
                    for (AdjustmentDetail d : o.details) {
                        if (d.LineId == t.LineId && d.LineId > 0 && t.LineId > 0) { t.RefDocIdNo = d.InvStockAdjustmentId; t.RefDocSubIdNo = d.Id; break; }
                    }
                    repo.set("USP_InventoryTransactionsPartyProcessing_Insert", t);
                }
            }
        }
        if ((o.AdjustmentTypeId == 2 && loader) || o.AdjustmentTypeId == 1) {
            InvTrans t = new InvTrans();
            t.OrganizationId = o.OrganizationId; t.CompanyId = o.CompanyId; t.RefDocumentTypeId = o.DocumentTypeId; t.RefDocIdNo = num;
            repo.set("Sp_InventoryTransactionsPartyProcessing_Insert", t);
        }
        if (o.AdjustmentTypeId == 2) {
            for (AdjustmentDetail d : o.details) {
                if (d.ActionTypeId == 3) continue;
                repo.inventoryValidation(PpCRepository.p("OrganizationId", o.OrganizationId, "CompanyId", o.CompanyId, "DocumentTypeId", o.DocumentTypeId,
                        "DocDate", ts(o.DocDate), "StockPartyId", o.StockPartyId, "ItemId", d.ItemId, "WarehouseId", d.WarehouseId, "JobLotId", d.JobLotId,
                        "CropYearId", d.CropYearId, "InvPackingTypeId", d.PackingTypeId, "PackUomId", d.PackUomId, "NetWeight", d.NetWeight,
                        "RefDocumentTypeId", d.RefDocumentTypeId, "RefDocNoId", d.RefDocNoId, "RefDocSubIdNo", d.RefDocSubIdNo));
            }
        }
        return num;
    }

    /** btnDelete_Click: DeleteById(org, comp, 158, RecId, user) - btnDelete is enabled by DoHaveCanDelete. */
    public Map<String, Object> delete(int id) {
        UserAccount u = pp.user(SCREEN);
        pp.require(u, SCREEN, "Delete");
        if (id <= 0) throw invalid("No record found to Delete");
        List<Map<String, Object>> h = repo.adjustment(id);
        if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) throw invalid("No record found to Delete");
        repo.adjustmentDelete(u, id, uid(u));
        return saved(id, "Delete Record Successfully");
    }

    /** gridhistoryfill: FormHistory (doc / entry / modify date radios; no approved radio on this form). */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> p = pp.history(u, SCREEN, b, new Object[]{"OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branch(u), "DocumentTypeId", DOC_TYPE, "FinancialYearId", pp.year()}, "FromDate", "ToDate", "DocNoFrom", "DocNoTo");
        int type = toInt(b.get("entryTypeId")), party = toInt(b.get("stockPartyId"));
        if (type != 0) p.put("AdjustmentTypeId", type);
        if (party != 0) p.put("StockPartyId", party);
        return project(repo.adjustmentHistory(p), "Id", "DocDate", "DocNo", "EntryType", "StockParty", "EntryDate", "EntryUser", "ModifyDate", "ModifyUser",
                "NoOfAttachments", "RemarksHeader");
    }
}
