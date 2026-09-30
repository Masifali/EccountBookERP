package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpCModels.InvTrans;
import com.mst.models.partyprocessing.PpCModels.InvTransFifo;
import com.mst.models.partyprocessing.PpCModels.Transfer;
import com.mst.models.partyprocessing.PpCModels.TransferDetail;
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
 * 603 Stock Transfer (Party Processing) - Architecture.WinApp.PartyProcessing.StockTransfer.cs,
 * BLL 0295 / DAL 0298 InvStockTransferPartyProcessing (DocumentTypeId 220, TransferType "MoveOrder").
 *
 * Load: rights (btnsave = Save, btnupdate = Update; print gated only on Ctrl+P), WagesCompulsoryForStockTransferPartyProcessing
 * and the 220 row of WagesRefDocumentsStatusList, DefaultDaysToLessFromHistoryFromDate, branch / project combos (display
 * only - the save takes the session branch), DocumentNoFill, TicketNofill(220), StockPartyBind, WareHouseFill (active),
 * GetJobLot, GetCropYear, ItemDetailFill ("1,2,4"), GetPackingType.
 * Save: FormValidation, empty grid, the row loop's messages, GrossWeight = Factory Weight check, BLL Save (the
 * "Id Greater Than 0" guard) and DAL SetData: header, details (LineId 1..n), then either FIFO out + mirrored in rows
 * (USP_InventoryTransactionsPartyProcessing_Insert, after QtyReverseAndDelete on an update) or, with loader rows,
 * Sp_InventoryTransactionsPartyProcessing_Insert; USP_InventoryValidationPartyProcessing for every row. One transaction.
 * Attachments (DMS) are not ported.
 */
@Service
public class PpCStockTransferService {

    public static final int SCREEN = 603;
    public static final int DOC_TYPE = 220;

    @Autowired private PpCRepository repo;
    @Autowired private PpCSupport pp;

    public Map<String, Object> setup() {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> out = lookups(u);
        out.put("rights", pp.rights(u, SCREEN));
        out.put("wagesStatus", pp.configBool(u, "WagesCompulsoryForStockTransferPartyProcessing"));
        out.put("wagesActive", pp.wagesActive(DOC_TYPE));
        out.put("defaultDays", pp.defaultDays(u));
        out.put("docNo", repo.transferCode(u, pp.year()));
        out.put("tickets", tickets(u, 0));
        return out;
    }

    /** btnRefresh_Click: branch, project, stock party, warehouse, job lot, crop year, item, packing type. */
    public Map<String, Object> lookups(UserAccount u) {
        return m("branches", project(repo.branches(u), "Id", "BranchName"),
                "projects", project(repo.projects(u), "Id", "ProjectName"),
                "stockParties", project(repo.stockParties(u), "Id", "CompanyName"),
                "warehouses", project(repo.activeWarehouses(u), "Id", "WareHouseName"),
                "jobLots", project(repo.jobLots(u), "Id", "JobLotDescription"),
                "cropYears", project(repo.cropYears(u), "Id", "CropYear"),
                "items", project(repo.itemsForPartyProcessing(u, "1,2,4"), "Id", "ItemName", "ItemCode"),
                "packingTypes", project(repo.packingTypes(), "Id", "PackTypeDesc"),
                "wagesActive", pp.wagesActive(DOC_TYPE));
    }

    public Map<String, Object> refresh() { return lookups(pp.user(SCREEN)); }

    /** DocumentNoFill + TicketNofill(220) (Reset). */
    public Map<String, Object> code(int recId) {
        UserAccount u = pp.user(SCREEN);
        return m("docNo", repo.transferCode(u, pp.year()), "tickets", tickets(u, recId));
    }

    private List<Map<String, Object>> tickets(UserAccount u, int recId) {
        return project(repo.ticketNos(u, DOC_TYPE, recId), "Id", "TicketNo", "NetWbWeight");
    }

    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = pp.user(SCREEN);
        return project(repo.uomSchedule(u, itemId), "Id", "UOMCode", "Equivalent");
    }

    /** AvailableStockGetByItem: GetWeightCurrStockByItem -> AvailableStock of row 0, else "0". */
    public Map<String, Object> stock(int warehouseId, int itemId, int jobLotId, String cropYear, String docDate) {
        UserAccount u = pp.user(SCREEN);
        List<Map<String, Object>> r = repo.weightCurrStockByItem(u, itemId, clrDate(picker(docDate)), warehouseId, jobLotId, cropYear == null ? "" : cropYear.trim());
        return m("available", r.isEmpty() ? "0" : str(r.get(0).get("AvailableStock")));
    }

    /** ReadById: GetByID header + ReadDetailByHeaderId rows (the form's table). */
    public Map<String, Object> read(int id) {
        UserAccount u = pp.user(SCREEN);
        List<Map<String, Object>> h = repo.transfer(id);
        if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) throw invalid("Record not found.");
        Map<String, Object> x = h.get(0);
        return m("Id", toInt(x.get("Id")), "BranchesId", toInt(x.get("BranchesId")), "ProjectsId", toInt(x.get("ProjectsId")),
                "DocNo", str(x.get("DocNo")), "DocDate", x.get("DocDate"), "WbTicketId", toInt(x.get("WbTicketId")),
                "WbNetWeight", str(x.get("WbNetWeight")), "RemarksHeader", str(x.get("RemarksHeader")), "IsApproved", toBool(x.get("IsApproved")),
                "rows", detailRows(id), "tickets", tickets(u, id));
    }

    private List<Map<String, Object>> detailRows(int id) {
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.transferDetails(id)) {
            rows.add(m("Id", toInt(d.get("Id")), "RefDocumentTypeId", toInt(d.get("RefDocumentTypeId")), "RefDocNoId", toInt(d.get("RefDocNoId")),
                    "RefDocSubIdNo", toInt(d.get("RefDocSubIdNo")), "StockPartyFromId", toInt(d.get("FromStockPartyId")), "StockPartyFrom", str(d.get("FromStockParty")),
                    "StockPartyToId", toInt(d.get("ToStockPartyId")), "StockPartyTo", str(d.get("ToStockParty")),
                    "WareHouseFromId", toInt(d.get("FromWarehouseId")), "WareHouseFrom", str(d.get("FromWareHouseName")),
                    "WareHouseToId", toInt(d.get("ToWarehouseId")), "WareHouseTo", str(d.get("ToWareHouseName")),
                    "JobLotId", toInt(d.get("JobLotId")), "JobLot", str(d.get("JobLotDescription")), "JobLotIdTo", toInt(d.get("JobLotIdTo")),
                    "JobLotTo", str(d.get("JobLotTo")), "CropYearId", toInt(d.get("CropYearId")), "CropYear", str(d.get("CropYear")),
                    "ItemId", toInt(d.get("ItemId")), "ItemName", str(d.get("ItemName")), "PackTypeId", toInt(d.get("InvPackingTypeId")),
                    "PackType", str(d.get("PackTypeDesc")), "PackUomId", toInt(d.get("PackUomId")), "PackUom", str(d.get("PackUom")),
                    "QTY", dbl(d.get("Qty")), "GrossWeight", dbl(d.get("GrossWeight")), "EbUnit", dbl(d.get("EbUnit")), "EbTotal", dbl(d.get("EbTotal")),
                    "Ad/LsWeight", dbl(d.get("AdLsWeight")), "NetWeight", dbl(d.get("NetWeight")), "BalQty", dbl(d.get("Qty")),
                    "BalWeight", dbl(d.get("NetWeight")), "Remarks", str(d.get("RemarksDetail"))));
        }
        return rows;
    }

    /** grdhistory_SelectionChanged: the detail list as the history detail grid shows it. */
    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = pp.user(SCREEN);
        List<Map<String, Object>> h = repo.transfer(id);
        if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) return new ArrayList<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.transferDetails(id)) {
            out.add(m("FromStockParty", d.get("FromStockParty"), "ToStockParty", d.get("ToStockParty"), "FromWarehouse", d.get("FromWareHouseName"),
                    "ToWarehouse", d.get("ToWareHouseName"), "FromJobLot", d.get("JobLotDescription"), "ToJobLot", d.get("JobLotTo"),
                    "CropYear", d.get("CropYear"), "ItemName", d.get("ItemName"), "PackingType", d.get("PackTypeDesc"), "Qty", dbl(d.get("Qty")),
                    "PackUom", d.get("PackUom"), "GrossWeight", dbl(d.get("GrossWeight")), "EbUnit", dbl(d.get("EbUnit")), "EbTotal", dbl(d.get("EbTotal")),
                    "AddLessWt", dbl(d.get("AdLsWeight")), "StockWeight", dbl(d.get("NetWeight")), "RemarksDetail", d.get("RemarksDetail")));
        }
        return out;
    }

    /** Insert(). */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        int recId = Math.max(0, toInt(b.get("id")));
        pp.require(u, SCREEN, recId > 0 ? "Update" : "Save");
        String docNo = trim(b.get("docNo"));
        if (docNo.isEmpty() || docNo.equals("0")) throw invalid("DocNo Field is Required");
        int ticket = toInt(b.get("ticketId"));
        if (toBool(b.get("ticketSelected")) && dbl(b.get("headNetWeight")) == 0d) throw invalid("Factory Weight Field is Required");
        List<Map<String, Object>> rows = list(b.get("rows"));
        if (rows.isEmpty()) throw invalid("Grid Record Not Found");
        if (recId > 0) {
            List<Map<String, Object>> h = repo.transfer(recId);
            if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) throw invalid("Record not found.");
            if (toBool(h.get(0).get("IsApproved"))) throw invalid("Approved Record Not Update");
        }
        LocalDateTime now = LocalDateTime.now();
        Transfer o = new Transfer();
        if (recId > 0) o.Id = recId;
        o.DocumentTypeId = DOC_TYPE;
        o.OrganizationId = u.getOrganizationId();
        o.CompanyId = u.getCompanyId();
        o.BranchesId = branch(u);
        o.EntryUser = uid(u);
        o.ModifyUser = uid(u);
        o.FinancialYearId = pp.year();
        o.EntryDate = now;
        o.ModifyDate = now;
        o.DocNo = toInt(docNo);
        o.DocDate = clrDate(picker(b.get("docDate")));
        o.RemarksHeader = str(b.get("remarks"));
        o.WbTicketId = ticket;
        o.WbNetWeight = dbl(b.get("headNetWeight"));
        o.TransferType = "MoveOrder";
        double gross = 0d;
        for (Map<String, Object> r : rows) {
            TransferDetail d = new TransferDetail();
            d.Id = toInt(r.get("Id"));
            d.RefDocumentTypeId = toInt(r.get("RefDocumentTypeId"));
            d.RefDocNoId = toInt(r.get("RefDocNoId"));
            d.RefDocSubIdNo = toInt(r.get("RefDocSubIdNo"));
            if (d.RefDocumentTypeId > 0 && dbl(r.get("NetWeight")) > dbl(r.get("BalWeight"))) throw invalid("NetWeight cannot be greater than BalWeight Please Check!");
            if (dbl(r.get("StockPartyFromId")) == 0d) throw invalid("Stock Party From Field Required In Detail Grid...");
            d.FromStockPartyId = toInt(r.get("StockPartyFromId"));
            if (dbl(r.get("StockPartyToId")) == 0d) throw invalid("Stock Party To Field Required In Detail Grid...");
            d.ToStockPartyId = toInt(r.get("StockPartyToId"));
            if (dbl(r.get("WareHouseFromId")) == 0d) throw invalid("WareHouseFrom  Field Required In Detail Grid...");
            d.FromWarehouseId = toInt(r.get("WareHouseFromId"));
            if (dbl(r.get("WareHouseToId")) == 0d) throw invalid("WareHouseTo Field Required In Detail Grid...");
            d.ToWarehouseId = toInt(r.get("WareHouseToId"));
            if (dbl(r.get("JobLotId")) == 0d) throw invalid("JobLot From Field Required In Detail Grid...");
            d.JobLotId = toInt(r.get("JobLotId"));
            if (dbl(r.get("JobLotIdTo")) == 0d) throw invalid("JobLot To Field Required In Detail Grid...");
            d.JobLotIdTo = toInt(r.get("JobLotIdTo"));
            d.CropYearId = toInt(r.get("CropYearId"));
            if (dbl(r.get("ItemId")) == 0d) throw invalid("Item Name Field Required In Detail Grid...");
            d.ItemId = toInt(r.get("ItemId"));
            d.InvPackingTypeId = toInt(r.get("PackTypeId"));
            d.PackUomId = toInt(r.get("PackUomId"));
            d.Qty = dbl(r.get("QTY"));
            d.GrossWeight = dbl(r.get("GrossWeight"));
            gross += d.GrossWeight;
            d.AdLsWeight = dbl(r.get("Ad/LsWeight"));
            d.EbUnit = dbl(r.get("EbUnit"));
            d.EbTotal = dbl(r.get("EbTotal"));
            d.NetWeight = dbl(r.get("NetWeight"));
            d.RemarksDetail = str(r.get("Remarks"));
            o.details.add(d);
        }
        if (dbl(trim(b.get("headNetWeight"))) != gross) throw invalid("GrossWeight and Factory Weight Not Match");
        /* BLL Save */
        if (o.Id == 0) {
            for (TransferDetail d : o.details) if (d.Id > 0) throw invalid("Record cannot be inserted because Id Greater Than 0");
            o.ModifyUser = 0;
        } else {
            o.EntryUser = 0;
        }
        String proc = o.Id == 0 ? "Sp_InvStockTransferHeaderPartyProcessing_Insert" : "Sp_InvStockTransferHeaderPartyProcessing_Update";
        int id = repo.tx(() -> setData(u, o, proc));
        return m("success", true, "id", id, "message", (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + o.DocNo,
                "openWages", pp.configBool(u, "WagesCompulsoryForStockTransferPartyProcessing") && pp.wagesActive(DOC_TYPE),
                "grossWeightTotal", dbl(b.get("headNetWeight")));
    }

    /** DAL InvStockTransferPartyProcessing.SetData. */
    private int setData(UserAccount u, Transfer o, String proc) {
        if (o.details.isEmpty()) throw invalid("InvStockTransferDetailPartyProcessingList not found");
        int num = repo.set(proc, o);
        if (num > 0) o.Id = num; else num = o.Id;
        int line = 1;
        boolean loader = false;
        for (TransferDetail d : o.details) {
            if (d.RefDocumentTypeId > 0 && d.RefDocNoId > 0 && d.RefDocSubIdNo > 0) loader = true;
            d.InvStockTransferHeaderId = o.Id;
            d.LineId = line;
            d.Id = repo.set("Sp_InvStockTransferDetailPartyProcessing_Insert", d);
            line++;
        }
        if (!loader) {
            List<Map<String, Object>> gl = repo.itemGl(u);
            List<InvTransFifo> fifo = new ArrayList<>();
            for (TransferDetail d : o.details) {
                Map<String, Object> item = find(gl, "Id", d.ItemId);
                if (item == null) continue;
                FifoArgs a = new FifoArgs();
                a.itemId = d.ItemId; a.stockPartyId = d.FromStockPartyId; a.warehouseId = d.FromWarehouseId; a.docDate = o.DocDate;
                a.stockUom = d.PackUomId; a.jobLotId = d.JobLotId; a.cropYearId = d.CropYearId; a.packingTypeId = d.InvPackingTypeId;
                if (o.ModifyUser > 0) { a.documentTypeId = o.DocumentTypeId; a.id = o.Id; }
                a.itemQty = d.Qty; a.netWeight = d.NetWeight; a.lineId = d.LineId; a.itemName = str(item.get("ItemName"));
                for (InvTransFifo t : pp.fifo(u, a, fifo)) {
                    t.TranRemarks = "Stock Transfer Party Processing";
                    t.DetailRemarks = d.RemarksDetail;
                    t.GrossWeight = d.GrossWeight; t.EbUnit = d.EbUnit; t.EbTotal = d.EbTotal; t.AddLess = d.AdLsWeight;
                    t.StockPartyId = d.FromStockPartyId;
                    t.RefWarehouseId = d.ToWarehouseId;
                    InvTransFifo in = new InvTransFifo();
                    in.LineId = t.LineId; in.ItemId = t.ItemId; in.WarehouseId = d.ToWarehouseId; in.JobLotId = d.JobLotIdTo;
                    in.CropYearId = t.CropYearId; in.InvPackingTypeId = t.InvPackingTypeId; in.ItemUom = t.ItemUom; in.RefWarehouseId = d.FromWarehouseId;
                    in.StockPartyId = d.ToStockPartyId; in.GrossWeight = d.GrossWeight; in.EbUnit = d.EbUnit; in.EbTotal = d.EbTotal; in.AddLess = d.AdLsWeight;
                    in.QtyIn = t.QtyOut; in.BillWeightIn = t.BillWeightOut; in.StockWeightIn = t.StockWeightOut; in.TranRemarks = t.TranRemarks;
                    in.RefRefDocumentTypeId = 0; in.RefRefDocIdNo = 0; in.RefRefDocSubIdNo = 0;
                    fifo.add(in);
                    fifo.add(t);
                }
            }
            if (!fifo.isEmpty()) {
                if (o.ModifyUser > 0) repo.reverseAndDelete(u, o.DocumentTypeId, o.Id);
                LocalDateTime now = LocalDateTime.now();
                for (InvTransFifo t : fifo) {
                    t.OrganizationId = o.OrganizationId; t.CompanyId = o.CompanyId; t.DocDate = o.DocDate; t.DocCodeNo = o.DocNo;
                    t.BranchesId = o.BranchesId; t.RefDocumentTypeId = o.DocumentTypeId; t.CalcType = "Weight";
                    t.EntryUserId = o.EntryUser; t.ModifyUserId = o.ModifyUser; t.EntryDate = now; t.ModifyDate = now;
                    for (TransferDetail d : o.details) {
                        if (d.LineId == t.LineId && d.LineId > 0 && t.LineId > 0) { t.RefDocIdNo = d.InvStockTransferHeaderId; t.RefDocSubIdNo = d.Id; break; }
                    }
                    repo.set("USP_InventoryTransactionsPartyProcessing_Insert", t);
                }
            }
        } else {
            InvTrans t = new InvTrans();
            t.OrganizationId = o.OrganizationId; t.CompanyId = o.CompanyId; t.RefDocumentTypeId = o.DocumentTypeId; t.RefDocIdNo = num;
            repo.set("Sp_InventoryTransactionsPartyProcessing_Insert", t);
        }
        for (TransferDetail d : o.details) {
            repo.inventoryValidation(PpCRepository.p("OrganizationId", o.OrganizationId, "CompanyId", o.CompanyId, "DocumentTypeId", o.DocumentTypeId,
                    "DocDate", ts(o.DocDate), "StockPartyId", d.FromStockPartyId, "ItemId", d.ItemId, "WarehouseId", d.FromWarehouseId, "JobLotId", d.JobLotId,
                    "CropYearId", d.CropYearId, "InvPackingTypeId", d.InvPackingTypeId, "PackUomId", d.PackUomId, "NetWeight", d.NetWeight,
                    "RefDocumentTypeId", d.RefDocumentTypeId, "RefDocNoId", d.RefDocNoId, "RefDocSubIdNo", d.RefDocSubIdNo));
        }
        return num;
    }

    /** gridhistoryfill: FormHistory (@DocDateFrom / @DocDateTo, @FromDocNo / @ToDocNo). */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> p = pp.history(u, SCREEN, b, new Object[]{"OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branch(u), "DocumentTypeId", DOC_TYPE, "FinancialYearId", pp.year()}, "DocDateFrom", "DocDateTo", "FromDocNo", "ToDocNo");
        return project(repo.transferHistory(p), "Id", "DocDate", "DocNo", "TicketId=WBTicketId", "TicketNo", "FactoryWeight=WbNetWeight", "EntryDate",
                "EntryUser=EntryUserName", "ModifyDate", "ModifyUser=ModifyUserName", "NoOfAttachments", "Remarks=RemarksHeader");
    }
}
