package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpCModels.Conversion;
import com.mst.models.partyprocessing.PpCModels.ConversionDetail;
import com.mst.models.partyprocessing.PpCModels.ConversionPm;
import com.mst.models.partyprocessing.PpCModels.InvTrans;
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
 * 602 Stock Conversion (Party Processing) - Architecture.WinApp.PartyProcessing.invfrmStockConversionPartyProcessing.cs,
 * BLL 0297 / DAL 0300 StockConversionPartyProcessing (DocumentTypeId 176).
 *
 * Load: rights (Save / Update / Delete / Print), WagesCompulsoryOnStockConversionPartyProcessing + the 176 row of
 * WagesRefDocumentsStatusList, GenerateDocNumber, StockPartyFill, CmbEntryTypeFill (2 Recovery By Product / 3 Recovery
 * Head Rice), Warehouse, CropYear, combojoblotfill, bagType, ItemFill (all party-processing items), the PM grid's value
 * lists (CommonServices.GetItemPartyProcessing("7") and GetActiveWareHouseByWareHouseType(2)).
 * Stock party change: BindReferenceParty (GellAllReferencePartiesStockPartyWise) + GetBalQtyAndWeightForPartyProcessing;
 * leave: GetJobOrderNoForConversion. Inputs ("Issue") come only from the issuance loader.
 * Save: FormValidation, the Issue / empty-grid / Head Rice checks, the row loops, BLL Save and DAL SetData (header,
 * every detail incl. the removed ActionTypeId 3 rows, PM rows, Sp_InventoryTransactionsPartyProcessing_Insert,
 * USP_InventoryValidationPartyProcessing for kept Issue rows and for every PM row with @Activity 'PM'), one transaction.
 * Attachments (DMS) are not ported.
 */
@Service
public class PpCStockConversionService {

    public static final int SCREEN = 602;
    public static final int DOC_TYPE = 176;

    @Autowired private PpCRepository repo;
    @Autowired private PpCSupport pp;

    public Map<String, Object> setup() {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> out = lookups(u);
        out.put("rights", pp.rights(u, SCREEN));
        out.put("docNo", repo.conversionCode(u, pp.year()));
        out.put("defaultDays", pp.defaultDays(u));
        out.put("pmItems", project(repo.itemsForPartyProcessing(u, "7"), "Id", "ItemName"));
        out.put("pmWarehouses", project(repo.warehousesByType(u, 2), "Id", "WareHouseName"));
        return out;
    }

    /** btnRefresh_Click: StockPartyFill, Warehouse, ItemFill, CropYear, combojoblotfill, bagType. */
    public Map<String, Object> lookups(UserAccount u) {
        return m("stockParties", project(repo.stockParties(u), "Id", "CompanyName"),
                "warehouses", project(repo.warehouses(u), "Id", "WareHouseName"),
                "items", project(repo.itemsForPartyProcessing(u, ""), "Id", "ItemName", "ItemCode"),
                "cropYears", project(repo.cropYears(u), "Id", "CropYear"),
                "jobLots", project(repo.jobLots(u), "Id", "JobLotDescription"),
                "packingTypes", project(repo.packingTypes(), "Id", "PackTypeDesc"));
    }

    public Map<String, Object> refresh() { return lookups(pp.user(SCREEN)); }

    public Map<String, Object> code() { UserAccount u = pp.user(SCREEN); return m("docNo", repo.conversionCode(u, pp.year())); }

    /** BindReferenceParty + CmbStockPartyHeader_Leave (GetJobOrderNoForConversion). */
    public Map<String, Object> party(int stockPartyId) {
        UserAccount u = pp.user(SCREEN);
        return m("referenceParties", project(repo.referencePartiesByStockParty(u, stockPartyId), "SupplierCustomerId", "ReferencePartyName"),
                "jobOrders", stockPartyId > 0 ? project(repo.jobOrdersForConversion(u, pp.year(), stockPartyId), "Id", "PlanCode") : new ArrayList<>());
    }

    /** bindUom: UOMSchedule.SearchByObject(ItemId) -> Id, UOMCode, Equivalent. */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = pp.user(SCREEN);
        return project(repo.uomSchedule(u, itemId), "Id", "UOMCode", "Equivalent");
    }

    /** GetBalQtyAndWeightForPartyProcessing: BalQty / BalStock of row 0 (Conversion.ToInt), else "". */
    public Map<String, Object> balance(int itemId, int stockPartyId, int warehouseId) {
        UserAccount u = pp.user(SCREEN);
        List<Map<String, Object>> r = repo.balQtyAndWeight(u, itemId, stockPartyId, warehouseId);
        if (r.isEmpty()) return m("qty", "", "weight", "");
        return m("qty", String.valueOf(toInt(r.get(0).get("BalQty"))), "weight", String.valueOf(toInt(r.get(0).get("BalStock"))));
    }

    /** ReadById: header + details + packing material. */
    public Map<String, Object> read(int id) {
        UserAccount u = pp.user(SCREEN);
        List<Map<String, Object>> h = repo.conversion(id);
        if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) throw invalid("Record not found...");
        Map<String, Object> x = h.get(0);
        List<Map<String, Object>> details = details(id);
        if (details.isEmpty()) throw invalid("Record not found...");
        List<Map<String, Object>> pm = new ArrayList<>();
        for (Map<String, Object> r : repo.conversionPm(id)) {
            pm.add(m("Id", toInt(r.get("Id")), "ItemId", toInt(r.get("ItemId")), "ItemQTY", dbl(r.get("ItemQty")), "WarehouseId", toInt(r.get("WarehouseId")),
                    "Item", str(r.get("ItemName")), "WareHouse", str(r.get("WareHouseName"))));
        }
        return m("Id", toInt(x.get("Id")), "DocNo", str(x.get("DocNo")), "DocDate", x.get("DocDate"), "ProductionNo", str(x.get("ProductionNo")),
                "StockPartyId", toInt(x.get("StockPartyId")), "Remarks", str(x.get("Remarks")), "details", details, "pm", pm);
    }

    private List<Map<String, Object>> details(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.conversionDetails(id)) {
            out.add(m("Id", toInt(d.get("Id")), "RefDocNoId", toInt(d.get("RefDocNoId")), "RefDocSubIdNo", toInt(d.get("RefDocSubIdNo")),
                    "RefDocumentTypeId", toInt(d.get("RefDocumentTypeId")), "JobOrderId", toInt(d.get("JobOrderId")), "JobOrder", str(d.get("JobOrder")),
                    "ReferencePartyId", toInt(d.get("ReferencePartyId")), "ReferenceParty", str(d.get("ReferenceParty")), "EntryType", str(d.get("EntryType")),
                    "WareHouseId", toInt(d.get("WarehouseId")), "WareHouse", str(d.get("WareHouseName")), "ItemId", toInt(d.get("ItemId")),
                    "ItemCode", str(d.get("ItemCode")), "Item", str(d.get("ItemName")), "CropYearId", toInt(d.get("CropYearId")), "CropYear", str(d.get("CropBatch")),
                    "JobLotId", toInt(d.get("JobLotId")), "JobLot", str(d.get("JobLotDescription")), "PackingTypeId", toInt(d.get("PackingtypeId")),
                    "PackingType", str(d.get("PackTypeDesc")), "ItemUOMId", toInt(d.get("ItemUomId")), "PackUom", str(d.get("ItemUom")),
                    "PackUomEquivalent", dbl(d.get("ItemUomEquivalent")), "Quantity", dbl(d.get("Qty")), "Weight", dbl(d.get("Weight")),
                    "Remarks", str(d.get("Remarks")), "BalQty", dbl(d.get("Qty")), "BalWeight", dbl(d.get("Weight"))));
        }
        return out;
    }

    /** DatagridHistory_SelectionChanged -> HistoryDetailsBind. */
    public Map<String, Object> historyDetail(int id) {
        UserAccount u = pp.user(SCREEN);
        List<Map<String, Object>> h = repo.conversion(id);
        if (id <= 0 || h.isEmpty() || !PpCScreens.owns(u, h.get(0))) return m("details", new ArrayList<>(), "pm", new ArrayList<>());
        List<Map<String, Object>> pm = new ArrayList<>();
        for (Map<String, Object> r : repo.conversionPm(id)) pm.add(m("Item", r.get("ItemName"), "ItemQTY", dbl(r.get("ItemQty")), "WareHouse", r.get("WareHouseName")));
        return m("details", details(id), "pm", pm);
    }

    /** Insert(). */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        int recId = Math.max(0, toInt(b.get("id")));
        pp.require(u, SCREEN, recId > 0 ? "Update" : "Save");
        String docNo = trim(b.get("docNo"));
        if (docNo.isEmpty() || docNo.equals("0")) throw invalid("document Number Field Required");
        String prodNo = trim(b.get("productionNo"));
        if (prodNo.isEmpty() || prodNo.equals("0")) throw invalid("Production NO. Field Required");
        int party = toInt(b.get("stockPartyId"));
        if (party == 0) throw invalid("Stock Party Field Required");
        List<Map<String, Object>> inputs = list(b.get("inputs")), outputs = list(b.get("outputs"));
        boolean issue = false, headRice = false;
        for (Map<String, Object> r : inputs) if ("Issue".equals(str(r.get("EntryType")))) issue = true;
        if (!issue) throw invalid("Input is Required in Detail Grid");
        if (inputs.isEmpty()) throw invalid("InPut Grid Not Found");
        if (outputs.isEmpty()) throw invalid("OutPut Grid Not Found");
        for (Map<String, Object> r : outputs) if ("Recovery Head Rice".equals(str(r.get("EntryType")))) headRice = true;
        if (!headRice) throw invalid("Recovery Head Rice is Required in Detail Grid");
        if (recId > 0) {
            List<Map<String, Object>> h = repo.conversion(recId);
            if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) throw invalid("RecId not found");
        }
        LocalDateTime now = LocalDateTime.now();
        Conversion o = new Conversion();
        o.Id = recId;
        o.OrganizationId = u.getOrganizationId();
        o.CompanyId = u.getCompanyId();
        o.BranchedId = branch(u);
        o.FinancialYearId = pp.year();
        o.DocumentTypeId = DOC_TYPE;
        o.DocNo = toInt(docNo);
        o.DocDate = clrDate(picker(b.get("docDate")));
        o.ProductionNo = str(b.get("productionNo"));
        o.DocManualRef = "";
        o.StockPartyId = party;
        o.Remarks = str(b.get("remarks"));
        o.EntryUser = uid(u);
        o.ModifyUser = uid(u);
        o.PostUser = uid(u);
        o.EntryDate = now;
        o.ModifyDate = now;
        o.PostDate = now;
        for (Map<String, Object> r : inputs) {
            if (toInt(r.get("ItemId")) == 0) continue;
            ConversionDetail d = detail(r, party, true);
            d.ActionTypeId = d.Id <= 0 ? 1 : 2;
            if (d.Qty == 0d) throw invalid("Qty field Required");
            if (d.Weight == 0d) throw invalid("Weight field Required");
            o.details.add(d);
        }
        for (Map<String, Object> r : outputs) {
            if (toInt(r.get("ItemId")) == 0) continue;
            ConversionDetail d = detail(r, party, false);
            d.ActionTypeId = d.Id <= 0 ? 1 : 2;
            o.details.add(d);
        }
        if (recId > 0) {
            for (Map<String, Object> r : list(b.get("removed"))) {
                ConversionDetail d = detail(r, party, true);
                d.ActionTypeId = 3;
                o.details.add(d);
            }
        }
        int n = 0;
        for (Map<String, Object> r : list(b.get("pm"))) {
            if (toInt(r.get("ItemId")) != 0) {
                ConversionPm pmr = new ConversionPm();
                pmr.Id = toInt(r.get("Id"));
                if (str(r.get("ItemId")).isEmpty()) throw invalid("PackingItem Name Field Require");
                pmr.ItemId = toInt(r.get("ItemId"));
                if (str(r.get("ItemQTY")).isEmpty()) throw invalid("PackingItem Qty Field Requirein row " + n + "1");
                pmr.ItemQty = dbl(r.get("ItemQTY"));
                if (str(r.get("WarehouseId")).isEmpty() || toInt(r.get("WarehouseId")) == 0) throw invalid("Warehouse Field Require in row " + n + "1");
                pmr.WarehouseId = toInt(r.get("WarehouseId"));
                o.packing.add(pmr);
            }
            n++;
        }
        o.ActionId = o.Id == 0 ? 1 : 2;
        String proc = o.Id == 0 ? "[USP_InvStockConversionPartyProcessing_Insert]" : "[USP_InvStockConversionPartyProcessing_Update]";
        int id = repo.tx(() -> setData(o, proc));
        return m("success", true, "id", id, "message", (recId == 0 ? "Record Save Successfully" : "Record Update Successfully") + o.DocNo,
                "openWages", pp.configBool(u, "WagesCompulsoryOnStockConversionPartyProcessing") && pp.wagesActive(DOC_TYPE));
    }

    private static ConversionDetail detail(Map<String, Object> r, int party, boolean input) {
        ConversionDetail d = new ConversionDetail();
        d.Id = toInt(r.get("Id"));
        if (input) {
            d.RefDocNoId = toInt(r.get("RefDocNoId"));
            d.RefDocSubIdNo = toInt(r.get("RefDocSubIdNo"));
            d.RefDocumentTypeId = toInt(r.get("RefDocumentTypeId"));
        }
        d.StockPartyId = party;
        d.JobOrderId = toInt(r.get("JobOrderId"));
        d.ReferencePartyId = toInt(r.get("ReferencePartyId"));
        d.EntryType = str(r.get("EntryType"));
        d.WarehouseId = toInt(r.get("WareHouseId"));
        d.ItemId = toInt(r.get("ItemId"));
        d.ItemUomId = toInt(r.get("ItemUOMId"));
        d.CropYearId = toInt(r.get("CropYearId"));
        d.JobLotId = toInt(r.get("JobLotId"));
        d.PackingtypeId = toInt(r.get("PackingTypeId"));
        d.Qty = dbl(r.get("Quantity"));
        d.Weight = dbl(r.get("Weight"));
        d.ProjectId = 0;
        d.PackUnit = 0;
        d.Remarks = str(r.get("Remarks"));
        return d;
    }

    /** DAL StockConversionPartyProcessing.SetData. */
    private int setData(Conversion o, String proc) {
        int num = repo.set(proc, o);
        if (num > 0) o.Id = num; else num = o.Id;
        for (ConversionDetail d : o.details) {
            d.InvStockConversionPartyProcessingId = o.Id;
            repo.set("USP_InvStockConversionPartyProcessingDetail_Insert", d);
        }
        for (ConversionPm pmr : o.packing) {
            pmr.InvStockConversionPartyProcessingId = o.Id;
            repo.set("[USP_InvStockConversionPartyProcessingPackingMaterial_Insert]", pmr);
        }
        InvTrans t = new InvTrans();
        t.OrganizationId = o.OrganizationId; t.CompanyId = o.CompanyId; t.RefDocumentTypeId = o.DocumentTypeId; t.RefDocIdNo = num;
        repo.set("Sp_InventoryTransactionsPartyProcessing_Insert", t);
        for (ConversionDetail d : o.details) {
            if (d.ActionTypeId == 3 || !"Issue".equals(d.EntryType)) continue;
            repo.inventoryValidation(PpCRepository.p("OrganizationId", o.OrganizationId, "CompanyId", o.CompanyId, "DocumentTypeId", o.DocumentTypeId,
                    "DocDate", ts(o.DocDate), "StockPartyId", d.StockPartyId, "ItemId", d.ItemId, "WarehouseId", d.WarehouseId, "JobLotId", d.JobLotId,
                    "CropYearId", d.CropYearId, "InvPackingTypeId", d.PackingtypeId, "PackUomId", d.ItemUomId, "NetWeight", d.Weight,
                    "RefDocumentTypeId", d.RefDocumentTypeId, "RefDocNoId", d.RefDocNoId, "RefDocSubIdNo", d.RefDocSubIdNo));
        }
        for (ConversionPm pmr : o.packing) {
            repo.inventoryValidation(PpCRepository.p("OrganizationId", o.OrganizationId, "CompanyId", o.CompanyId, "DocDate", ts(o.DocDate),
                    "ItemId", pmr.ItemId, "WarehouseId", pmr.WarehouseId, "NetWeight", pmr.ItemQty, "Activity", "PM"));
        }
        return num;
    }

    /** btnDelete_Click: DeleteByID(UserAccount.ID, RecId) - btnDelete is enabled by DoHaveCanDelete. */
    public Map<String, Object> delete(int id) {
        UserAccount u = pp.user(SCREEN);
        pp.require(u, SCREEN, "Delete");
        if (id == 0) throw invalid("Record Id Not Found");
        List<Map<String, Object>> h = repo.conversion(id);
        if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) throw invalid("Record Id Not Found");
        repo.conversionDelete(uid(u), id);
        return saved(id, "Delete Record Successfully");
    }

    /** GridHistoryMainFill: FormHistory (@BranchedId, doc / entry / modify date radios, @DocNoFrom / @DocNoTo). */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> p = pp.history(u, SCREEN, b, new Object[]{"OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchedId", branch(u), "DocumentTypeId", DOC_TYPE, "FinancialYearId", pp.year()}, "FromDate", "ToDate", "DocNoFrom", "DocNoTo");
        return project(repo.conversionHistory(p), "Id", "DocumentTypeId", "DocNo", "DocDate", "ProductionNo", "ManualNo=DocManualRef", "StockPartyId",
                "StockPartyName", "Remarks", "EntryDate", "EntryUser", "ModifyDate", "ModifyUser", "NoOfAttachments");
    }
}
