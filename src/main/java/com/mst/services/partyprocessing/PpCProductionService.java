package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpCModels.FoodProduction;
import com.mst.models.partyprocessing.PpCModels.FoodProductionDetail;
import com.mst.models.partyprocessing.PpCModels.FoodProductionPm;
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
 * 677 Production (Party Processing) - Architecture.WinApp.PartyProcessing.frmProductionPartyProcessing.cs.
 * One form, four tabs, all ported:
 *   Input   (DocumentTypeId 117, EntryType "Input", detail EntryType "Issue")  BLL 0299 / DAL 0302 InvFoodProductionPartyProcessing
 *   Output  (DocumentTypeId 118, EntryType "Output", detail ByProduct / FinishGoods)  same BLL / DAL
 *   PM      (DocumentTypeId 119)  BLL 0300 / DAL 0303 InvFoodProductionPartyProcessingPackingMaterial
 *   Transaction History (FormHistory without a document type) with the 608 summary and 609 issuance prints.
 * Rights: Input / Output Save, Update, Delete (DoHaveCanDelete), Print as the Load sets them; the PM buttons are not
 * rights-gated on the desktop and are not here either. The loader is the shared issuance loader (PpCLoaderService).
 */
@Service
public class PpCProductionService {

    public static final int SCREEN = 677;
    public static final int DOC_INPUT = 117, DOC_OUTPUT = 118, DOC_PM = 119;
    private static final String SCREEN_NAME = "frmProductionPartyProcessing";

    @Autowired private PpCRepository repo;
    @Autowired private PpCSupport pp;

    /** frmFoodProduction_Load. */
    public Map<String, Object> setup() {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> out = lookups(u);
        int y = pp.year();
        out.put("rights", pp.rights(u, SCREEN));
        out.put("financialYearId", y);
        out.put("languages", project(repo.languages(u), "Id", "LanguageDescription"));
        out.put("docNoInput", repo.productionCode(u, y, DOC_INPUT));
        out.put("docNoOutput", repo.productionCode(u, y, DOC_OUTPUT));
        out.put("docNoPm", repo.pmCode(u, y));
        out.put("defaultDays", pp.defaultDays(u));
        out.put("inputHistoryJobOrders", jobOrderRows(repo.productionDropDown(u, "117")));
        out.put("outputHistoryJobOrders", jobOrderRows(repo.productionDropDown(u, "118")));
        out.put("pmHistoryJobOrders", jobOrderRows(repo.pmDropDown(u)));
        out.put("summaryJobOrders", jobOrderRows(repo.productionDropDown(u, null)));
        return out;
    }

    /** Refresh buttons: JobOrderNoFill, StockPartyFill, Warehouse, ItemFill, CropYear, combojoblotfill, PackingTypeFill, ReferencePartyFill, WareHousePM, ItemPMFill. */
    public Map<String, Object> lookups(UserAccount u) {
        return m("jobOrders", project(repo.jobOrdersForProduction(u, pp.year()), "Id", "PlanCode"),
                "stockParties", project(repo.stockParties(u), "Id", "CompanyName"),
                "warehouses", project(repo.warehouses(u), "Id", "WareHouseName"),
                "items", project(repo.itemsForPartyProcessing(u, ""), "Id", "ItemName", "ItemCode"),
                "cropYears", project(repo.cropYears(u), "Id", "CropYear"),
                "jobLots", project(repo.jobLots(u), "Id", "JobLotDescription"),
                "packingTypes", project(repo.packingTypes(), "Id", "PackTypeDesc"),
                "referenceParties", project(repo.referenceParties(u), "Id", "ReferencePartyName"),
                "pmWarehouses", project(repo.warehousesByType(u, 2), "Id", "WareHouseName"),
                "pmItems", project(repo.itemsForPartyProcessingPm(u), "Id", "ItemName", "ItemCode"));
    }

    public Map<String, Object> refresh() { return lookups(pp.user(SCREEN)); }

    private static List<Map<String, Object>> jobOrderRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) if ("InvJobOrder".equals(str(r.get("Activity")))) out.add(m("Id", r.get("Id"), "ReferenceName", r.get("ReferenceName")));
        return out;
    }

    public Map<String, Object> historyCombos() {
        UserAccount u = pp.user(SCREEN);
        return m("inputHistoryJobOrders", jobOrderRows(repo.productionDropDown(u, "117")), "outputHistoryJobOrders", jobOrderRows(repo.productionDropDown(u, "118")),
                "pmHistoryJobOrders", jobOrderRows(repo.pmDropDown(u)), "summaryJobOrders", jobOrderRows(repo.productionDropDown(u, null)));
    }

    public Map<String, Object> codes() {
        UserAccount u = pp.user(SCREEN);
        int y = pp.year();
        return m("docNoInput", repo.productionCode(u, y, DOC_INPUT), "docNoOutput", repo.productionCode(u, y, DOC_OUTPUT), "docNoPm", repo.pmCode(u, y));
    }

    /** CmbJobOrderNo_Leave: GetGlAccountsByJobOrderId -> WIP item and stock party of the job order. */
    public Map<String, Object> jobOrderGl(int jobOrderId) {
        UserAccount u = pp.user(SCREEN);
        List<Map<String, Object>> r = repo.jobOrderGl(u, pp.year(), jobOrderId);
        return m("wipItems", project(r, "WipItemId", "ItemName"), "stockParties", project(r, "StockPartyId", "StockParty"));
    }

    /** GetBalQtyAndWeightForPartyProcessing (input detail): Conversion.ToDouble(...).ToString(). */
    public Map<String, Object> balance(int itemId, int stockPartyId, int warehouseId) {
        UserAccount u = pp.user(SCREEN);
        List<Map<String, Object>> r = repo.balQtyAndWeight(u, itemId, stockPartyId, warehouseId);
        if (r.isEmpty()) return m("qty", "", "weight", "");
        return m("qty", clr(dbl(r.get(0).get("BalQty"))), "weight", clr(dbl(r.get(0).get("BalStock"))));
    }

    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = pp.user(SCREEN);
        return project(repo.uomSchedule(u, itemId), "Id", "UOMCode", "Equivalent");
    }

    /** CmbJobOrderNoOutput_Leave: GetInPutTotalQtyandWeightByJobOrderId (Input / Output rows). */
    public Map<String, Object> totals(int jobOrderId) {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> out = m("inQty", "0", "inWeight", "0", "outQty", "0", "outWeight", "0");
        List<Map<String, Object>> r = repo.inputTotalsByJobOrder(u, jobOrderId);
        if (r.isEmpty()) return out;
        out = m("inQty", "", "inWeight", "", "outQty", "", "outWeight", "");
        for (Map<String, Object> x : r) {
            if ("Input".equals(str(x.get("EntryType")))) { out.put("inQty", str(x.get("ItemQty"))); out.put("inWeight", str(x.get("StockWeight"))); }
            if ("Output".equals(str(x.get("EntryType")))) { out.put("outQty", str(x.get("ItemQty"))); out.put("outWeight", str(x.get("StockWeight"))); }
        }
        return out;
    }

    // ============================================================================== read

    /** GetByID: header + ReadDetailByHeaderId rows (all columns the form's grids read). */
    public Map<String, Object> read(int id, String expect) {
        UserAccount u = pp.user(SCREEN);
        List<Map<String, Object>> h = repo.production(id);
        if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) throw invalid("Record Not Found...");
        Map<String, Object> x = h.get(0);
        String type = str(x.get("EntryType"));
        if ("Input".equals(expect) && !"Input".equals(type)) throw invalid("Record Not Found...");
        if ("Output".equals(expect) && "Input".equals(type)) throw invalid("Record Not Found...");
        return m("Id", toInt(x.get("Id")), "DocNo", str(x.get("DocNo")), "DocDate", x.get("DocDate"), "EntryType", type, "MainRemarks", str(x.get("MainRemarks")),
                "InvJobOrderId", toInt(x.get("InvJobOrderId")), "InvJobOrderNo", str(x.get("InvJobOrderNo")), "WIPItemId", toInt(x.get("WIPItemId")),
                "details", details(id));
    }

    private List<Map<String, Object>> details(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.productionDetails(id)) {
            out.add(m("Id", toInt(d.get("Id")), "RefDocumentTypeId", toInt(d.get("RefDocumentTypeId")), "RefDocIdNo", toInt(d.get("RefDocNoId")),
                    "RefDocSubIdNo", toInt(d.get("RefDocSubIdNo")), "DocDate", d.get("DocDate"), "DocNo", toInt(d.get("DocNo")),
                    "EntryType", str(d.get("EntryType")), "StockPartyId", toInt(d.get("StockPartyId")), "StockParty", str(d.get("StockParty")),
                    "WareHouseId", toInt(d.get("WarehouseId")), "WareHouse", str(d.get("WareHouseName")), "ItemId", toInt(d.get("ItemId")),
                    "ItemCode", str(d.get("ItemCode")), "Item", str(d.get("ItemName")), "CropYearId", toInt(d.get("CropYearId")), "CropYear", str(d.get("CropBatch")),
                    "JobLotId", toInt(d.get("JobLotId")), "JobLot", str(d.get("JobLotDescription")), "PackingTypeId", toInt(d.get("PackingtypeId")),
                    "PackingType", str(d.get("PackTypeDesc")), "ItemUOMId", toInt(d.get("ItemUomId")), "ItemUOM", str(d.get("PackUom")),
                    "ItemUOMEquivalent", dbl(d.get("Equivalent")), "Quantity", dbl(d.get("Qty")), "GrossWeight", dbl(d.get("GrossWeight")),
                    "EbUnit", dbl(d.get("EbUnit")), "EbTotal", dbl(d.get("EbTotal")), "Weight", dbl(d.get("Weight")), "BalanceQty", dbl(d.get("Qty")),
                    "BalanceWeight", dbl(d.get("Weight")), "ReferencePartyId", toInt(d.get("ReferencePartyId")), "ReferenceParty", str(d.get("ReferencePartyName")),
                    "Remarks", str(d.get("Remarks")), "ActionId", toInt(d.get("ActionId"))));
        }
        return out;
    }

    /** History selection (input / output / transaction history): the detail rows, empty when not the user's record. */
    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = pp.user(SCREEN);
        List<Map<String, Object>> h = repo.production(id);
        if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) return new ArrayList<>();
        return details(id);
    }

    // ============================================================================== input (InsertInput)

    public Map<String, Object> saveInput(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        int recId = Math.max(0, toInt(b.get("id")));
        pp.require(u, SCREEN, recId > 0 ? "Update" : "Save");
        List<Map<String, Object>> rows = list(b.get("rows"));
        if (rows.isEmpty()) throw invalid("Grd Record Not Found.\nEnter Detail First ...");
        String docNo = trim(b.get("docNo"));
        if (docNo.isEmpty() || docNo.equals("0")) throw invalid("document Number Field Required");
        int jo = toInt(b.get("jobOrderId"));
        if (jo == 0) throw invalid("Job Order Number Field Required");
        if (toInt(b.get("wipItemId")) == 0) throw invalid("WIP Item Field Required");
        if (toInt(b.get("wipStockPartyId")) == 0) throw invalid("StockParty Field Required");
        guard(u, recId, "Input");
        LocalDateTime now = LocalDateTime.now();
        FoodProduction h = header(u, recId, DOC_INPUT, now);
        h.ProjectId = 0;
        h.DocNo = toInt(docNo);
        h.DocDate = clrDate(picker(b.get("docDate")));
        h.EntryType = "Input";
        h.InvJobOrderId = jo;
        h.InvJobOrderNo = trim(b.get("jobOrderText"));
        h.WIPItemId = toInt(b.get("wipItemId"));
        h.MainRemarks = str(b.get("remarks"));
        double gross = 0d;
        for (Map<String, Object> r : rows) {
            if (!(dbl(r.get("Quantity")) > 0d) || !(dbl(r.get("Weight")) > 0d)) continue;
            FoodProductionDetail d = inputDetail(r);
            d.Id = recId > 0 ? toInt(r.get("Id")) : 0;
            d.ActionTypeId = d.Id == 0 ? 1 : 2;
            if (d.ActionId == 1) {
                if (d.RefDocumentTypeId == 0) throw invalid("RefDocumentTypeId not Found");
                if (d.RefDocNoId == 0) throw invalid("RefDocNoId not Found");
                if (d.RefDocSubIdNo == 0) throw invalid("RefDocSubIdNo not Found");
            }
            if (toInt(r.get("StockPartyId")) == 0) throw invalid("Stock Party Field Require in Input Detail");
            if (toInt(r.get("WareHouseId")) == 0) throw invalid("WareHouse Field Require in Input Detail");
            if (toInt(r.get("ItemId")) == 0) throw invalid("Item Field Require in Input Detail");
            if (str(r.get("CropYear")).isEmpty() || toInt(r.get("CropYearId")) == 0) throw invalid("CropYear Field Require in Input Detail");
            if (toInt(r.get("JobLotId")) == 0) throw invalid("JobLot Field Require in Input Detail");
            if (toInt(r.get("PackingTypeId")) == 0) throw invalid("Packing Type Field Require in Input Detail");
            if (toInt(r.get("ItemUOMId")) == 0) throw invalid("ItemUOM Field Require in Input Detail");
            d.InvJobOrderId = jo;
            d.InvJobOrderNo = str(b.get("jobOrderText"));
            gross += d.Weight;
            h.details.add(d);
        }
        for (Map<String, Object> r : list(b.get("removed"))) {
            FoodProductionDetail d = inputDetail(r);
            d.Id = toInt(r.get("Id"));
            d.ActionTypeId = 3;
            h.details.add(d);
        }
        if (h.details.isEmpty()) throw invalid("Grid Record Not Found");
        String proc = h.Id == 0 ? "Sp_InvFoodProductionPartyProcessing_Insert" : "Sp_InvFoodProductionPartyProcessing_Update";
        int id = repo.tx(() -> setData(h, proc));
        return m("success", true, "id", id, "message", (recId > 0 ? "Record Update Successfully" : "Record Save Successfully") + h.DocNo,
                "openWages", pp.configBool(u, "WagesCompulsoryOnProductionPartyProcessing") && pp.wagesActive(DOC_INPUT), "grossWeightTotal", gross);
    }

    private static FoodProductionDetail inputDetail(Map<String, Object> r) {
        FoodProductionDetail d = new FoodProductionDetail();
        d.EntryType = "Issue";
        d.RefDocNoId = toInt(r.get("RefDocIdNo"));
        d.RefDocumentTypeId = toInt(r.get("RefDocumentTypeId"));
        d.RefDocSubIdNo = toInt(r.get("RefDocSubIdNo"));
        d.ActionId = toInt(r.get("ActionId"));
        d.StockPartyId = toInt(r.get("StockPartyId"));
        d.WarehouseId = toInt(r.get("WareHouseId"));
        d.ItemId = toInt(r.get("ItemId"));
        d.CropYearId = toInt(r.get("CropYearId"));
        d.JobLotId = toInt(r.get("JobLotId"));
        d.PackingtypeId = toInt(r.get("PackingTypeId"));
        d.ItemUomId = toInt(r.get("ItemUOMId"));
        d.Qty = dbl(r.get("Quantity"));
        d.Weight = dbl(r.get("Weight"));
        d.ReferencePartyId = toInt(r.get("ReferencePartyId"));
        d.Remarks = str(r.get("Remarks"));
        return d;
    }

    private FoodProduction header(UserAccount u, int recId, int docType, LocalDateTime now) {
        FoodProduction h = new FoodProduction();
        h.Id = recId;
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.BranchId = branch(u);
        h.FinancialYearId = pp.year();
        h.DocumentTypeId = docType;
        h.EntryDate = now;
        h.ModifyDate = now;
        h.EntryUser = uid(u);
        h.ModifyUser = uid(u);
        h.ApprovedUserId = uid(u);
        h.ApprovedDate = now;
        h.ScreenName = SCREEN_NAME;
        return h;
    }

    private void guard(UserAccount u, int recId, String expect) {
        if (recId == 0) return;
        List<Map<String, Object>> h = repo.production(recId);
        if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) throw invalid("Rec Id Not Found....");
        String type = str(h.get(0).get("EntryType"));
        if ("Input".equals(expect) != "Input".equals(type)) throw invalid("Rec Id Not Found....");
    }

    /** DAL InvFoodProductionPartyProcessing.SetData. */
    private int setData(FoodProduction h, String proc) {
        int num = repo.set(proc, h);
        if (num > 0) h.Id = num; else num = h.Id;
        for (FoodProductionDetail d : h.details) {
            d.InvFoodProductionPartyProcessingId = h.Id;
            repo.set("Sp_InvFoodProductionPartyProcessingDetail_Insert", d);
        }
        InvTrans t = new InvTrans();
        t.OrganizationId = h.OrganizationId; t.CompanyId = h.CompanyId; t.RefDocumentTypeId = h.DocumentTypeId; t.RefDocIdNo = num;
        repo.set("Sp_InventoryTransactionsPartyProcessing_Insert", t);
        if (h.DocumentTypeId == DOC_INPUT) {
            for (FoodProductionDetail d : h.details) {
                repo.inventoryValidation(PpCRepository.p("OrganizationId", h.OrganizationId, "CompanyId", h.CompanyId, "DocumentTypeId", h.DocumentTypeId,
                        "DocDate", ts(h.DocDate), "StockPartyId", d.StockPartyId, "ItemId", d.ItemId, "WarehouseId", d.WarehouseId, "JobLotId", d.JobLotId,
                        "CropYearId", d.CropYearId, "InvPackingTypeId", d.PackingtypeId, "PackUomId", d.ItemUomId, "NetWeight", d.Weight,
                        "RefDocumentTypeId", d.RefDocumentTypeId, "RefDocNoId", d.RefDocNoId, "RefDocSubIdNo", d.RefDocSubIdNo));
            }
        }
        return num;
    }

    /** BtnDeleteInput_Click / BtnDeleteOutput_Click: DeleteById(Id, user) - enabled by DoHaveCanDelete. */
    public Map<String, Object> delete(int id, String expect) {
        UserAccount u = pp.user(SCREEN);
        pp.require(u, SCREEN, "Delete");
        if (id <= 0) throw invalid("No record found to Delete");
        List<Map<String, Object>> h = repo.production(id);
        if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) throw invalid("No record found to Delete");
        repo.productionDelete(id, uid(u));
        return saved(id, "Delete Record Successfully");
    }

    // ============================================================================== output (OutPutInsert)

    public Map<String, Object> saveOutput(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        int recId = Math.max(0, toInt(b.get("id")));
        pp.require(u, SCREEN, recId > 0 ? "Update" : "Save");
        List<Map<String, Object>> rows = list(b.get("rows"));
        if (rows.isEmpty()) throw invalid("Grd Record Not Found.\nEnter Detail First ...");
        String docNo = trim(b.get("docNo"));
        if (docNo.isEmpty() || docNo.equals("0")) throw invalid("DocNo Field Required");
        int jo = toInt(b.get("jobOrderId"));
        if (jo == 0) throw invalid("Job Order Number Field Required");
        guard(u, recId, "Output");
        LocalDateTime now = LocalDateTime.now();
        FoodProduction h = header(u, recId, DOC_OUTPUT, now);
        h.ProjectId = branch(u);
        /* OutPutInsert: a new record regenerates the doc no (GenerateDocNumberOutPut) after its confirm. */
        int code = recId == 0 ? repo.productionCode(u, pp.year(), DOC_OUTPUT) : 0;
        h.DocNo = recId == 0 && code > 0 ? code : toInt(docNo);
        h.DocDate = clrDate(picker(b.get("docDate")));
        h.InvJobOrderId = jo;
        h.InvJobOrderNo = trim(b.get("jobOrderText"));
        h.EntryType = "Output";
        h.MainRemarks = str(b.get("remarks"));
        if (recId > 0) {
            for (Map<String, Object> r : list(b.get("removed"))) {
                FoodProductionDetail d = new FoodProductionDetail();
                d.ActionTypeId = 3;
                d.Id = toInt(r.get("Id"));
                d.EntryType = trim(r.get("EntryType"));
                d.WarehouseId = toInt(r.get("WareHouseId"));
                d.ItemId = toInt(r.get("ItemId"));
                d.CropYearId = toInt(r.get("CropYearId"));
                d.JobLotId = toInt(r.get("JobLotId"));
                d.PackingtypeId = toInt(r.get("PackingTypeId"));
                d.ItemUomId = toInt(r.get("ItemUOMId"));
                d.Qty = dbl(r.get("Quantity"));
                d.Weight = dbl(r.get("Weight"));
                d.Remarks = trim(r.get("Remarks"));
                h.details.add(d);
            }
        }
        double gross = 0d;
        for (Map<String, Object> r : rows) {
            FoodProductionDetail d = new FoodProductionDetail();
            d.Id = toInt(r.get("Id"));
            d.ActionTypeId = d.Id > 0 ? 2 : 1;
            if (trim(r.get("EntryType")).isEmpty()) throw invalid("EntryType Field Require in Output Detail");
            if (toInt(r.get("WareHouseId")) == 0) throw invalid("WareHouse Field Require in Output Detail");
            if (toInt(r.get("ItemId")) == 0) throw invalid("Item Field Require in Output Detail");
            if (toInt(r.get("CropYearId")) == 0) throw invalid("CropYear Field Required in Output Detail");
            if (toInt(r.get("JobLotId")) == 0) throw invalid("JobLot Field Require in Output Detail");
            if (toInt(r.get("PackingTypeId")) == 0) throw invalid("PackingType Field Required in Output Detail");
            if (toInt(r.get("ItemUOMId")) == 0) throw invalid("ItemUOM Field Required in Output Detail");
            if (dbl(r.get("Quantity")) == 0d) throw invalid("Quantity Field Required in Output Detail");
            if (dbl(r.get("Weight")) == 0d) throw invalid("Weight Field Required in Output Detail");
            d.InvJobOrderId = jo;
            d.EntryType = str(r.get("EntryType"));
            d.WarehouseId = toInt(r.get("WareHouseId"));
            d.ItemId = toInt(r.get("ItemId"));
            d.CropYearId = toInt(r.get("CropYearId"));
            d.JobLotId = toInt(r.get("JobLotId"));
            d.PackingtypeId = toInt(r.get("PackingTypeId"));
            d.ItemUomId = toInt(r.get("ItemUOMId"));
            d.Qty = dbl(r.get("Quantity"));
            d.GrossWeight = dbl(r.get("GrossWeight"));
            d.EbUnit = dbl(r.get("EbUnit"));
            d.EbTotal = dbl(r.get("EbTotal"));
            d.Weight = dbl(r.get("Weight"));
            gross += d.Weight;
            d.Remarks = str(r.get("Remarks"));
            d.StockPartyId = 0;
            d.ActionId = 1;
            h.details.add(d);
        }
        if (h.details.isEmpty()) throw invalid("Grid Record Not Found");
        String proc = h.Id == 0 ? "Sp_InvFoodProductionPartyProcessing_Insert" : "Sp_InvFoodProductionPartyProcessing_Update";
        int id = repo.tx(() -> setData(h, proc));
        return m("success", true, "id", id, "message", (recId > 0 ? "Record Update Successfully" : "Record Save Successfully") + h.DocNo,
                "openWages", pp.configBool(u, "WagesCompulsoryOnProductionPartyProcessing") && pp.wagesActive(DOC_OUTPUT), "grossWeightTotal", gross);
    }

    // ============================================================================== history

    /** InputHistory / OutputGridHistory / GridHistory: USP_InvFoodProductionPartyProcessing_FormHistory. */
    public List<Map<String, Object>> history(Map<String, Object> b, int docType) {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> filter = new java.util.LinkedHashMap<>(b);
        /* OutputGridHistory reads its To Doc No from ToDateHistoryOutPut.Text (a date) -> Conversion.ToInt = 0: never sent. */
        if (docType == DOC_OUTPUT) filter.put("toDocNo", 0);
        Map<String, Object> p = pp.history(u, SCREEN, filter, new Object[]{"OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", pp.year(), "DocumentTypeId", docType == 0 ? null : docType, "BranchesId", branch(u) == 0 ? null : branch(u)},
                "FromDate", "ToDate", "DocNoFrom", "DocNoTo");
        int jo = toInt(b.get("jobOrderId"));
        if (jo != 0) p.put("InvJobOrderId", jo);
        List<Map<String, Object>> rows = repo.productionHistory(p);
        if (docType == DOC_INPUT) {
            return project(rows, "Id", "DocDate", "DocNo", "DocumentTypeId", "WIPItemId", "WIPItemCode", "WIPItemName", "InvJobOrderId", "InvJobOrderNo",
                    "EntryType", "Remarks=MainRemarks", "EntryDate", "EntryUserName", "ModifyDate", "ModifyUserName");
        }
        return project(rows, "Id", "DocDate", "DocNo", "DocumentTypeId", "InvJobOrderId", "InvJobOrderNo", "EntryType", "Remarks=MainRemarks",
                "EntryDate", "EntryUserName", "ModifyDate", "ModifyUserName");
    }

    // ============================================================================== PM (PMInsert)

    public Map<String, Object> savePm(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        int recId = Math.max(0, toInt(b.get("id")));
        if (toInt(b.get("jobOrderId")) == 0) throw invalid("Product No. Field Required");
        if (toInt(b.get("itemId")) == 0) throw invalid(" Item Field Required");
        if (toInt(b.get("warehouseId")) == 0) throw invalid("Warehouse  Field Required");
        if (trim(b.get("qty")).isEmpty() || dbl(b.get("qty")) == 0d) throw invalid(" Qty Field Required");
        if (recId > 0) {
            List<Map<String, Object>> h = repo.pm(recId);
            if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) throw invalid("Rec Id not found");
        }
        LocalDateTime now = LocalDateTime.now();
        FoodProductionPm h = new FoodProductionPm();
        h.Id = recId;
        int code = recId == 0 ? repo.pmCode(u, pp.year()) : 0;
        h.CompanyId = u.getCompanyId();
        h.OrganizationId = u.getOrganizationId();
        h.FinancialYearId = pp.year();
        h.DocumentTypeId = DOC_PM;
        h.EntryDate = now;
        h.EntryUser = uid(u);
        h.ModifyDate = now;
        h.ModifyUser = uid(u);
        h.ApprovedDate = now;
        h.ApprovedUserId = uid(u);
        h.IsApproved = false;
        h.DocDate = orNow(picker(b.get("docDate")));
        /* GenerateCodeOfPM on a new record: the fresh code, or "" (0) when none. */
        h.DocNo = recId == 0 ? code : toInt(b.get("docNo"));
        h.InvJobOrderId = toInt(b.get("jobOrderId"));
        h.WarehouseId = toInt(b.get("warehouseId"));
        h.ItemId = toInt(b.get("itemId"));
        h.Qty = dbl(b.get("qty"));
        h.Remarks = str(b.get("remarks"));
        h.ScreenName = SCREEN_NAME;
        String proc = h.Id == 0 ? "Sp_InvFoodProductionPartyProcessingPackingMaterial_Insert" : "Sp_InvFoodProductionPartyProcessingPackingMaterial_Update";
        int id = repo.tx(() -> {
            int num = repo.set(proc, h);
            if (num > 0) h.Id = num; else num = h.Id;
            InvTrans t = new InvTrans();
            t.OrganizationId = h.OrganizationId; t.CompanyId = h.CompanyId; t.RefDocumentTypeId = h.DocumentTypeId; t.RefDocIdNo = num;
            repo.set("Sp_InventoryTransactionsPartyProcessing_Insert", t);
            repo.inventoryValidation(PpCRepository.p("OrganizationId", h.OrganizationId, "CompanyId", h.CompanyId, "DocumentTypeId", h.DocumentTypeId,
                    "DocDate", ts(h.DocDate), "ItemId", h.ItemId, "WarehouseId", h.WarehouseId, "NetWeight", h.Qty));
            return num;
        });
        return saved(id, recId > 0 ? "Packing Material  Update Successfully" : "Packing Material Save Successfully");
    }

    public Map<String, Object> readPm(int id) {
        UserAccount u = pp.user(SCREEN);
        List<Map<String, Object>> h = repo.pm(id);
        if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) throw invalid("Record Not Found...");
        Map<String, Object> x = h.get(0);
        return m("Id", toInt(x.get("Id")), "DocDate", x.get("DocDate"), "DocNo", str(x.get("DocNo")), "InvJobOrderId", toInt(x.get("InvJobOrderId")),
                "WarehouseId", toInt(x.get("WarehouseId")), "ItemId", toInt(x.get("ItemId")), "Qty", clr(dbl(x.get("Qty"))), "Remarks", str(x.get("Remarks")));
    }

    public Map<String, Object> deletePm(int id) {
        UserAccount u = pp.user(SCREEN);
        if (id <= 0) throw invalid("No record found to Delete");
        List<Map<String, Object>> h = repo.pm(id);
        if (h.isEmpty() || !PpCScreens.owns(u, h.get(0))) throw invalid("No record found to Delete");
        repo.pmDelete(id, uid(u));
        return saved(id, "Delete Record Seccessfully");
    }

    /** BindGrid: PM FormHistory (@DocumentTypeId 119 always; its To Doc No is read from ToDatePMHistory.Text -> 0, never sent). */
    public List<Map<String, Object>> pmHistory(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> filter = new java.util.LinkedHashMap<>(b);
        filter.put("toDocNo", 0);
        Map<String, Object> p = pp.history(u, SCREEN, filter, new Object[]{"OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", pp.year(), "DocumentTypeId", DOC_PM}, "FromDate", "ToDate", "DocNoFrom", "DocNoTo");
        int jo = toInt(b.get("jobOrderId"));
        if (jo != 0) p.put("InvJobOrderId", jo);
        return project(repo.pmHistory(p), "Id", "DocumentTypeId", "DocDate", "DocNo", "InvJobOrderId", "JobOrderNo", "WarehouseId", "WareHouseName",
                "ItemId", "ItemName", "ItemCode", "Qty", "Remarks", "EntryDate", "EntryUserName", "ModifyDate", "ModifyUserName");
    }

    // ============================================================================== prints

    /** GenerateReportPM: FoodProductionPartyProcessingPackingMaterial_SlipAndRegister rows, else "Record Not Found For Display". */
    public Map<String, Object> pmPrintCheck(int id) {
        UserAccount u = pp.user(SCREEN);
        if (repo.pmSlip(u, pp.year(), id).isEmpty()) throw invalid("Record Not Found For Display");
        return m("id", id, "financialYearId", pp.year());
    }

    /** btnSummeryReport_Click (608) / btn609ProductionIssuance_Click: rows, else "Record Not Found For DisPlay". */
    public Map<String, Object> reportCheck(String which, int jobOrderId, int languageId) {
        UserAccount u = pp.user(SCREEN);
        List<Map<String, Object>> r = "608".equals(which) ? repo.summary608(u, jobOrderId, languageId) : repo.issuance609(u, jobOrderId);
        if (r.isEmpty()) throw invalid("Record Not Found For DisPlay");
        return m("jobOrderId", jobOrderId, "languageId", languageId);
    }
}
