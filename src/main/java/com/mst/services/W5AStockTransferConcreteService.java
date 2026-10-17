package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.services.sale.engr.SaleEngrSupport;
import com.mst.services.sale.pcc.SalePccLookups;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Date;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

import static com.mst.repositories.support.DesktopProc.rows;
import static com.mst.services.sale.engr.SaleEngrSupport.ci;
import static com.mst.services.sale.engr.SaleEngrSupport.row;
import static com.mst.services.sale.engr.SaleEngrSupport.str;
import static com.mst.services.sale.engr.SaleEngrSupport.text;
import static com.mst.services.sale.engr.SaleEngrSupport.toBool;
import static com.mst.services.sale.engr.SaleEngrSupport.toDecimal;
import static com.mst.services.sale.engr.SaleEngrSupport.toDouble;
import static com.mst.services.sale.engr.SaleEngrSupport.toInt;

/**
 * W5A - Production (Pcc) hub, screen 596 "Stock Transfer Manual" (concrete variant).
 *
 * <pre>
 *   Desktop form   Architecture.WinApp.pcc.Production.frmStockTransferManualConcrete (.cs, 4477 lines), base.Name "frmStockTransferManualConcrete" (:4421, rights key)
 *   DocumentTypeId 1860 (DocumentNoFill:467, Insert:1200, gridhistoryfill:1714, slip btnprint_Click:2011)
 *   Route          /production/pcc/stock-transfer-manual-concrete      API /production/pcc/stock-transfer-manual-concrete/api
 *   BLL / DAL      recovered_source/projects/architecture.bll/0345 pcc.InvStockTransferHeader (Save, ReadById, FormHistory, GenerateCode, DeleteByID),
 *                  architecture.dal/0284 pcc.InvStockTransferHeader (SetData), architecture.dal/0205 CommonServices.FIFOImplementionForConcrete (:1148),
 *                  architecture.bll/0056 GetAvgRatesAndStockInHand (GetStockInHandAndAvgRateFromEvaluationConcrete :304, GetStockByFifoMethodConcrete :777),
 *                  Architecture.WinApp.Common/CommonServices.GetAvgRateFromFIFOMethodConcrete (:2851)
 *   Procedures     [pcc].[USP_InvStockTransferHeader_GetAllMethod] (GenerateCode / ReadById / ReadByHeaderId_InvStockTransferDetail / FormHistory / DeleteById),
 *                  [pcc].[USP_InvStockTransferHeader_Insert] / _Update, [pcc].[USP_InvStockTransferDetail_Insert], Sp_InventoryTransactions_GetALLMethod,
 *                  [pcc].[USP_GetStockByFifoMethod], [dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId], USP_InventoryStockEvalautionDetail_Insert,
 *                  Sp_InventoryStockEvalautionDetail_Update (no FIFO), USP_StockValidationConcrete, Sp_GetAvgRatesAndStockInHand_GetAllMethod
 *   Print          existing GET /reports/print/1860-stock-transfer-manual-slip?success=ID (ConcretePrintController, 1860-StockTransferManual-Slip.rpt)
 * </pre>
 *
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 * <ol>
 *  <li>FormValidation:714 messages in order (DocNo / GatePassNo / Vehicle No); Insert:1200 "Grid Record Not Found", per-row "... Feild Required In Detail Grid..."
 *      messages (:1285-:1330) and, only when config StockTransferFinancialEffectIsActive, "AvgRate field required" / "Amount field required".</li>
 *  <li>Header: ProjectsId = BranchesId, GpDate = DocDate (the GP date box is never read: Insert:1262), IsApproved false, ApprovedUserId = the user, ApprovedDate = now;
 *      BLL Save: insert -> ActionId 1 / ModifyUserId 0, update -> ActionId 2 / EntryUserId 0.</li>
 *  <li>DAL SetData (0284): header proc, LineId numbering over the whole list (removed rows first), Sp_InventoryTransactions_GetALLMethod, then with ERP feature 5 the
 *      FIFO costing: per non-removed row FIFOImplementionForConcrete over [pcc].[USP_GetStockByFifoMethod] (@VarientId = AttributeVareintId, @WarehouseId = warehouse
 *      FROM, @DocumentTypeId/@Id only when updating, @FIFOXML of every evaluation row built so far), one out row per FIFO layer plus an in row for warehouse TO,
 *      detail ItemRate = sum(CgsAmount)/sum(QtyOut) and ItemAmount = ItemRate x ItemQty, detail procs, USP_InventoryQtyReverseAndDeleteByReferenceId on update,
 *      USP_InventoryStockEvalautionDetail_Insert per evaluation row; without feature 5 Sp_InventoryStockEvalautionDetail_Update; finally USP_StockValidationConcrete
 *      for every detail (removed rows included). All in one transaction.</li>
 *  <li>The DAL never sets ReportsParameters.JobLotId, so no @JobLotId is sent to the FIFO procedure and every evaluation row (out and in) is written with JobLotId 0:
 *      the Job Lot From / To chosen on the grid are saved on the detail rows only. Reproduced as is.</li>
 *  <li>Rate preview GetItemStock:647 (QtyInHand from the evaluation; with ERP feature 5 the FIFO average over [pcc].[USP_GetStockByFifoMethod] including the JobLot
 *      and, when editing, @DocumentTypeId 1860 / @Id), AvgAmountCalculation:2075, Avg Rate / Net Weight / Amount / Item Weight boxes are disabled (read-only).</li>
 *  <li>Delete button uses the Update right (Load:313 btnDelete.Enabled = DoHaveUpdateRights) and DeleteById has no approval check; Update on an approved record is refused
 *      ("Approved Record Not Update", btnupdate_Click:1425).</li>
 *  <li>Date range combo (cmbperemeter_ValueChanged:386) case 4 sets To = 1 Jan and From = today, as coded.</li>
 * </ol>
 *
 * DEVIATIONS (web only, with reason)
 * <ol>
 *  <li>DocNo is the generator's on insert and the stored header's on update, never taken from the page.</li>
 *  <li>A document is opened/updated/deleted only when it belongs to the user's organization/company and is DocumentTypeId 1860.</li>
 *  <li>Rights (Save / Update) and IsApproved are re-checked on the server; row ids and removed ids must be detail rows of the document being updated.</li>
 *  <li>Removed rows are only honoured on update (the desktop only fills lstRemoveRecord for saved rows). The desktop reads grid cells "Rate" and "Amount" (not columns of
 *      grd) when deleting a saved row (grd_ColumnButtonClick:1100), which may fail there; the web deletes the row (ActionTypeId 3) as the branch intends.</li>
 *  <li>The entry-panel rules of FormValidationDetail (:737) are re-applied to each posted row on the server (same messages), because the server cannot trust the page.</li>
 *  <li>DeleteByID gets the signed-in user's id (the desktop passes UserAccount.EntryUserId).</li>
 * </ol>
 *
 * NOT PORTED: Attachment dialog (btnAttachment, DMSAttachments rows, history NoOfAttachments link), saved grid layouts (ctrlGrdBar*), keyboard-only helpers other than the
 * common ones, the hidden Branch / Project combos (never read by the save).
 */
@Service
public class W5AStockTransferConcreteService {

    public static final int SCREEN_ID = 596;
    public static final int DOC = 1860;
    public static final String SCREEN = "frmStockTransferManualConcrete";
    private static final String GET = "[pcc].[USP_InvStockTransferHeader_GetAllMethod]";
    private static final String FIFO = "[pcc].[USP_GetStockByFifoMethod]";

    private final SaleEngrSupport sup;
    private final SalePccLookups lk;
    private final Map<String, List<Object[]>> paramCache = new ConcurrentHashMap<>();

    public W5AStockTransferConcreteService(SaleEngrSupport sup, SalePccLookups lk) { this.sup = sup; this.lk = lk; }

    // ------------------------------------------------------------------ Load (PurchsaeOrder_Load:304)

    public Map<String, Object> initial() {
        lk.requireView(SCREEN_ID);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("docNo", nextNo());
        m.putAll(lists());
        m.put("fifo", sup.erpFeature(5));                                                  // FifoMethod = CommonServices.GetERPFeatureById(5)
        m.put("financialEffect", sup.configBool("StockTransferFinancialEffectIsActive"));  // :317
        int pa = sup.configInt("Default NoofDecimal Points For Amount");                   // CommonServices.GetDecimalConfiguration:5372
        int pr = sup.configInt("Default NoofDecimal Points For Rate");
        m.put("amountDp", pa >= 1 && pa <= 4 ? pa : 0);                                    // stringFormatsingle = "#,##0." + 0..4 zeros
        m.put("rateDp", pr >= 1 && pr <= 4 ? pr : (pr == 0 ? 2 : 0));                      // DecimalRateFormate = "#,#0." + zeros (0 -> "00")
        m.put("dateTypes", SalePccLookups.dateTypes());                                    // ParameterFill:373
        m.put("fyStart", lk.fyStart());
        return m;
    }

    /** btnRefresh_Click:838 - ItemDetailFill, DocumentNoFill, WareHouseFill, GetJobLot. */
    public Map<String, Object> lists() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("warehouses", lk.warehouses());                                              // CommonServices.getActiveWareHouse (WareHouseFill:423)
        m.put("jobLots", lk.jobLots());                                                    // CommonServices.JobLotGetAllService (GetJobLot:538)
        m.put("items", items());                                                           // ItemDetailFill:497
        return m;
    }

    /** CommonServices.ItemGetAllServiceBind = Item.GetAll 'ReadByOrganizationCompanyId': Id, ItemName, ItemCode, WeightKgs, ItemWithVarient. */
    public List<Map<String, Object>> items() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_Item_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationCompanyId")) {
            Object code = ci(r, "ItemCode");
            if (code == null) code = ci(r, "ItemCodeNew");
            out.add(row("Id", ci(r, "Id"), "ItemName", ci(r, "ItemName"), "ItemCode", code, "ItemWeight", toDouble(ci(r, "WeightKgs")), "WithVariant", toBool(ci(r, "ItemWithVarient"))));
        }
        return out;
    }

    /** DocumentNoFill:467 - pcc InvStockTransferHeader.GenerateCode (org, company, user branch, financial year, DocumentTypeId 1860). */
    public int nextNo() {
        List<Map<String, Object>> r = sup.rows(GET, "OrganizationId", sup.org(), "CompanyId", sup.company(), "BranchesId", sup.branch(),
                "FinancialYearId", sup.fy(), "DocumentTypeId", DOC, "Activity", "GenerateCode");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "DocNo"));
    }

    /** cmbItemName_Leave:2052 - bindPackUom:2134 (Sp_UOMSchedule_GetAllMethod 'ReadByItemID') and bindvarientunit:582 (ItemAttributeVarient.GetAllForCombo). */
    public Map<String, Object> itemInfo(int itemId) {
        List<Map<String, Object>> uoms = new ArrayList<>(), vars = new ArrayList<>();
        if (itemId > 0) {
            for (Map<String, Object> r : sup.rows("Sp_UOMSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "ReadByItemID"))
                uoms.add(row("Id", ci(r, "Id"), "Name", ci(r, "UOMCode"), "Equivalent", toDouble(ci(r, "Equivalent"))));
            for (Map<String, Object> r : lk.varients(itemId))
                vars.add(row("Id", ci(r, "ItemAttributeVarientId"), "Name", ci(r, "ItemAttribute"), "Equivalent", toDouble(ci(r, "VarientEquivalent"))));
        }
        return row("uoms", uoms, "variants", vars);
    }

    // ------------------------------------------------------------------ GetItemStock:647

    public Map<String, Object> stock(int itemId, int warehouseId, int jobLotId, int variantId, boolean withVariant, double qty, int recId, String docDate) {
        LocalDate d = SalePccLookups.parseDate(docDate);
        if (d == null) d = LocalDate.now();
        double balance = 0, avg = 0;
        if (itemId > 0 || warehouseId > 0) {
            Map<String, Object> p = new LinkedHashMap<>();                                     // GetStockInHandAndAvgRateFromEvaluationConcrete(.., wareId, 0, 0)
            p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("ItemId", itemId); p.put("DocDate", Date.valueOf(d));
            if (warehouseId != 0) p.put("WarehouseId", warehouseId);
            p.put("Activity", "GetStockInHandAndAvgRateFromEvaluation");
            List<Map<String, Object>> st = rows(sup.jdbc(), "Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
            if (!st.isEmpty()) {
                balance = toDouble(ci(st.get(0), "QtyInHand"));
                if (!sup.erpFeature(5)) avg = toDouble(ci(st.get(0), "AvgRate"));
                else if (itemId > 0 && warehouseId > 0 && qty > 0) avg = fifoAvgRate(itemId, warehouseId, jobLotId, withVariant ? variantId : 0, qty, recId, d);
            }
        }
        return row("balance", balance, "avgRate", avg);
    }

    /** CommonServices.GetAvgRateFromFIFOMethodConcrete:2851 over GetStockByFifoMethodConcrete (BLL 0056:777); ItemQty = NetWeight = the Qty box. */
    private double fifoAvgRate(int itemId, int warehouseId, int jobLotId, int stockUom, double qty, int recId, LocalDate d) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("ItemId", itemId); p.put("DocDate", Date.valueOf(d));
        if (stockUom != 0) p.put("VarientId", stockUom);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (recId > 0) { p.put("DocumentTypeId", DOC); p.put("Id", recId); }
        List<Map<String, Object>> layers = rows(sup.jdbc(), FIFO, p);
        if (layers.isEmpty()) return 0;
        double totalW = 0, totalQ = 0;
        for (Map<String, Object> l : layers) { totalW += toDouble(ci(l, "BalWeight")); totalQ += toDouble(ci(l, "BalQty")); }
        double reqQty = qty, reqW = qty, consumedQty = 0, consumed = 0, cgs = 0;
        if (!(reqQty <= totalQ && reqW <= totalW)) return 0;
        for (Map<String, Object> l : layers) {
            double availW = toDouble(ci(l, "BalWeight")), availQ = toDouble(ci(l, "BalQty")), rate = toDouble(ci(l, "AvgRate"));
            if (availQ <= reqQty - consumedQty) { consumed += availW; consumedQty += availQ; cgs += availQ * rate; }
            else if (availQ >= reqQty - consumedQty) {
                double outQty = reqQty - consumedQty, outW = reqW - consumed;
                cgs += outQty * rate; consumedQty += outQty; consumed += outW;
            }
            if (reqQty == consumedQty) break;
        }
        return cgs > 0 && reqQty > 0 ? cgs / reqQty : 0;
    }

    // ------------------------------------------------------------------ History (gridhistoryfill:1714)

    /** InvStockTransferHeader.FormHistory: dates always sent, zero doc numbers / empty vehicle not sent, CanViewAllRecord right decides the EntryUserId filter. */
    public List<Map<String, Object>> history(String fromDate, String toDate, int fromNo, int toNo, String vehicleNo) {
        Map<String, Boolean> rt = sup.rights(SCREEN);
        boolean all = Boolean.TRUE.equals(rt.get("viewAll"));
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("BranchesId", sup.branch());
        p.put("DocumentTypeId", DOC); p.put("FinancialYearId", sup.fy());
        LocalDate f = SalePccLookups.parseDate(fromDate), t = SalePccLookups.parseDate(toDate);
        if (f != null) p.put("FromDate", Date.valueOf(f));
        if (t != null) p.put("ToDate", Date.valueOf(t));
        if (fromNo != 0) p.put("FromDocNo", fromNo);
        if (toNo != 0) p.put("ToDocNo", toNo);
        if (vehicleNo != null && !vehicleNo.isEmpty()) p.put("VehicleNo", vehicleNo);
        p.put("CanViewAllRecord", all ? 1 : 0);
        if (!all) p.put("EntryUserId", sup.userId());
        p.put("Activity", "FormHistory");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows(sup.jdbc(), GET, p)) {                           // gridhistoryfill:1731 shows the dates as short dates
            Map<String, Object> m = new LinkedHashMap<>(r);
            for (String k : new String[]{"DocDate", "GpDate", "EntryDate", "ModifyDate"}) {
                for (String key : new ArrayList<>(m.keySet())) if (key.equalsIgnoreCase(k)) m.put(key, day(m.get(key)));
            }
            out.add(m);
        }
        return out;
    }

    // ------------------------------------------------------------------ ReadById / getUpdate:1444

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> r = sup.rows(GET, "Id", id, "Activity", "ReadById");
        if (r.isEmpty()) throw new IllegalArgumentException("Record not found");
        Map<String, Object> h = r.get(0);
        UserAccount u = sup.user();
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId() || toInt(ci(h, "CompanyId")) != u.getCompanyId() || toInt(ci(h, "DocumentTypeId")) != DOC)
            throw new IllegalArgumentException("Record not found");
        return h;
    }

    private List<Map<String, Object>> details(int id) { return sup.rows(GET, "Id", id, "Activity", "ReadByHeaderId_InvStockTransferDetail"); }

    public Map<String, Object> read(int id) {
        Map<String, Object> h = header(id);
        List<Map<String, Object>> grid = new ArrayList<>();
        for (Map<String, Object> d : details(id)) {
            grid.add(row("Id", toInt(ci(d, "Id")),
                    "WareHouseFromId", toInt(ci(d, "WarehouseFromId")), "WareHouseFrom", str(ci(d, "WarehouseFrom")),
                    "WareHouseToId", toInt(ci(d, "WarehouseToId")), "WareHouseTo", str(ci(d, "WarehouseTo")),
                    "JobLotId", toInt(ci(d, "JobLotFromId")), "JobLot", str(ci(d, "JobLotFrom")),
                    "JobLotIdTo", toInt(ci(d, "JobLotToId")), "JobLotTo", str(ci(d, "JobLotTo")),
                    "ItemId", toInt(ci(d, "ItemId")), "ItemName", str(ci(d, "ItemName")),
                    "PackUomId", toInt(ci(d, "ItemUomId")), "PackUom", str(ci(d, "PackUom")),
                    "AttributeVarientId", toInt(ci(d, "AttributeVareintId")), "AttributeVarient", str(ci(d, "AttributeVareint")),
                    "VarientUnit", toDouble(ci(d, "VareintEquivalent")), "QTY", toDouble(ci(d, "ItemQty")), "ItemWeight", toDouble(ci(d, "ItemWeight")),
                    "NetWeight", toDouble(ci(d, "NetWeight")), "ItemRate", toDouble(ci(d, "ItemRate")), "ItemAmount", toDouble(ci(d, "ItemAmount")),
                    "Remarks", str(ci(d, "RemarksDetail"))));
        }
        return row("id", id, "docNo", toInt(ci(h, "DocNo")), "docDate", day(ci(h, "DocDate")), "gpDate", day(ci(h, "GpDate")), "gpSrNo", toInt(ci(h, "GPSrNo")),
                "vehicleNo", str(ci(h, "VehicleNo")), "remarksHeader", str(ci(h, "RemarksHeader")), "isApproved", toBool(ci(h, "IsApproved")), "details", grid);
    }

    private static String day(Object o) {
        if (o == null) return "";
        String s = String.valueOf(o);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    // ------------------------------------------------------------------ Save (Insert:1200 + BLL Save + DAL SetData)

    /** One detail row of InvStockTransferHeader.InvStockTransferDetailList. */
    private static final class Det {
        int id, lineId, warehouseFromId, warehouseToId, itemId, jobLotFromId, jobLotToId, itemUomId, variantId, actionTypeId;
        BigDecimal variantEq = BigDecimal.ZERO, qty = BigDecimal.ZERO, itemWeight = BigDecimal.ZERO, netWeight = BigDecimal.ZERO, rate = BigDecimal.ZERO, amount = BigDecimal.ZERO;
        String remarks = "";
    }

    /** One InventoryStockEvalautionDetail row built by the FIFO step. */
    private static final class Ev {
        int id, lineId, itemId, warehouseId, rateUom, jobLotId, packingTypeId, itemUom, variantId, refRefDocType, refRefDocId, refRefDocSub;
        double qtyIn, qtyOut, billIn, billOut, stockIn, stockOut, itemRate, cgsRate, cgsAmount, amountIn;
        String cropBatch = "";
    }

    @Transactional
    @SuppressWarnings("unchecked")
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = sup.user();
        Map<String, Boolean> rt = sup.rights(SCREEN);
        int recId = toInt(b.get("id"));
        lk.need(rt, recId > 0 ? "update" : "save", recId > 0 ? "Update" : "Save");
        LocalDate docDay = SalePccLookups.parseDate(str(b.get("docDate")));
        if (docDay == null) throw new IllegalArgumentException("Doc Date Field is Required");
        Date docDate = Date.valueOf(docDay);

        Map<String, Object> old = null;
        int docNo;
        if (recId > 0) {
            old = header(recId);
            if (toBool(ci(old, "IsApproved"))) throw new IllegalArgumentException("Approved Record Not Update");     // btnupdate_Click:1429
            docNo = toInt(ci(old, "DocNo"));
        } else docNo = nextNo();
        String gp = text(str(b.get("gpSrNo"))), vehicle = text(str(b.get("vehicleNo")));
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");                              // FormValidation:716
        if (gp.isEmpty()) throw new IllegalArgumentException("GatePassNo Field is Required");                      // :722
        if (vehicle.isEmpty()) throw new IllegalArgumentException("Vehicle No Field is Required");                 // :728
        List<Map<String, Object>> posted = b.get("rows") instanceof List ? (List<Map<String, Object>>) b.get("rows") : new ArrayList<>();
        if (posted.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");                         // :1220

        java.util.Set<Integer> savedIds = new java.util.HashSet<>();
        if (recId > 0) for (Map<String, Object> d : details(recId)) savedIds.add(toInt(ci(d, "Id")));
        Map<Integer, Boolean> withVariant = new HashMap<>();
        for (Map<String, Object> it : items()) withVariant.put(toInt(it.get("Id")), toBool(it.get("WithVariant")));
        boolean financial = sup.configBool("StockTransferFinancialEffectIsActive");

        List<Det> list = new ArrayList<>();
        if (recId > 0 && b.get("removed") instanceof List) {                                                         // lstRemoveRecord first (:1259)
            for (Map<String, Object> r : (List<Map<String, Object>>) b.get("removed")) {
                Det x = new Det();
                x.id = toInt(r.get("Id"));
                if (!savedIds.contains(x.id)) throw new IllegalArgumentException("Invalid detail row for this stock transfer");
                x.warehouseFromId = toInt(r.get("WareHouseFromId")); x.warehouseToId = toInt(r.get("WareHouseToId"));
                x.jobLotFromId = toInt(r.get("JobLotId")); x.jobLotToId = toInt(r.get("JobLotIdTo"));
                x.itemId = toInt(r.get("ItemId")); x.itemUomId = toInt(r.get("PackUomId")); x.variantId = toInt(r.get("AttributeVarientId"));
                x.variantEq = toDecimal(r.get("VarientUnit")); x.qty = toDecimal(r.get("QTY")); x.itemWeight = toDecimal(r.get("ItemWeight"));
                x.netWeight = toDecimal(r.get("NetWeight")); x.rate = toDecimal(r.get("ItemRate")); x.amount = toDecimal(r.get("ItemAmount"));
                x.remarks = text(str(r.get("Remarks"))); x.actionTypeId = 3;
                list.add(x);
            }
        }
        for (Map<String, Object> r : posted) {
            Det x = new Det();
            x.id = recId != 0 ? toInt(r.get("Id")) : 0;
            if (x.id > 0 && !savedIds.contains(x.id)) throw new IllegalArgumentException("Invalid detail row for this stock transfer");
            x.actionTypeId = x.id > 0 ? 2 : 1;
            x.itemId = toInt(r.get("ItemId"));
            if (x.itemId == 0) throw new IllegalArgumentException("Item Name Feild Required In Detail Grid...");
            x.jobLotFromId = toInt(r.get("JobLotId"));
            if (x.jobLotFromId == 0) throw new IllegalArgumentException("JobLot From Feild Required In Detail Grid...");
            x.jobLotToId = toInt(r.get("JobLotIdTo"));
            if (x.jobLotToId == 0) throw new IllegalArgumentException("JobLot To Feild Required In Detail Grid...");
            x.warehouseFromId = toInt(r.get("WareHouseFromId"));
            if (x.warehouseFromId == 0) throw new IllegalArgumentException("WareHouseFrom  Feild Required In Detail Grid...");
            x.warehouseToId = toInt(r.get("WareHouseToId"));
            if (x.warehouseToId == 0) throw new IllegalArgumentException("WareHouseTo Feild Required In Detail Grid...");
            x.itemUomId = toInt(r.get("PackUomId")); x.variantId = toInt(r.get("AttributeVarientId"));
            x.variantEq = toDecimal(r.get("VarientUnit")); x.qty = toDecimal(r.get("QTY")); x.itemWeight = toDecimal(r.get("ItemWeight"));
            x.netWeight = toDecimal(r.get("NetWeight")); x.rate = toDecimal(r.get("ItemRate")); x.amount = toDecimal(r.get("ItemAmount"));
            x.remarks = text(str(r.get("Remarks")));
            if (financial) {                                                                                         // :1330
                if (x.rate.signum() <= 0) throw new IllegalArgumentException("AvgRate field required");
                if (x.amount.signum() <= 0) throw new IllegalArgumentException("Amount field required");
            }
            // FormValidationDetail:737 re-applied on the server (web safeguard)
            if (x.itemUomId == 0) throw new IllegalArgumentException("Pack Uom field is required");
            if (Boolean.TRUE.equals(withVariant.get(x.itemId)) && x.variantId == 0) throw new IllegalArgumentException("Attribute Variant field is required");
            if (x.qty.signum() == 0) throw new IllegalArgumentException("Qty field is required");
            if (x.itemWeight.signum() == 0) throw new IllegalArgumentException("ItemWeight field is required");
            if (x.netWeight.signum() == 0) throw new IllegalArgumentException("NetWeight field is required");
            if (x.warehouseFromId == x.warehouseToId && x.jobLotFromId == x.jobLotToId)
                throw new IllegalArgumentException("JoLotFrom Can not Equal to JobLotTo when Warehouse From and WarehouseTo is same");
            list.add(x);
        }
        if (list.isEmpty()) throw new IllegalArgumentException("At least enter value/quantity in one of the rows of detail ");   // :1355

        // ---- BLL Save: ActionId / user ids
        boolean update = recId > 0;
        int entryUser = update ? 0 : u.getId(), modifyUser = update ? u.getId() : 0;
        Map<String, Object> hp = new LinkedHashMap<>();
        hp.put("Id", recId); hp.put("DocumentTypeId", DOC); hp.put("DocNo", docNo); hp.put("DocDate", docDate); hp.put("GPSrNo", toInt(gp)); hp.put("GpDate", docDate);
        hp.put("VehicleNo", vehicle); hp.put("RemarksHeader", text(str(b.get("remarksHeader"))));
        hp.put("EntryDate", sup.now()); hp.put("EntryUserId", entryUser); hp.put("ModifyDate", sup.now()); hp.put("ModifyUserId", modifyUser);
        hp.put("IsApproved", false); hp.put("ApprovedUserId", u.getId()); hp.put("ApprovedDate", sup.now());
        hp.put("OrganizationId", sup.org()); hp.put("CompanyId", sup.company()); hp.put("BranchesId", sup.branch()); hp.put("ProjectsId", sup.branch());
        hp.put("FinancialYearId", sup.fy()); hp.put("ActionId", update ? 2 : 1);
        String headerProc = update ? "[pcc].[USP_InvStockTransferHeader_Update]" : "[pcc].[USP_InvStockTransferHeader_Insert]";
        int num = sup.setProcMap(headerProc, full(headerProc, hp));
        final int headerId = num > 0 ? num : recId;                                                                  // SetData:36-43

        int line = 1;
        for (Det x : list) x.lineId = line++;                                                                        // SetData:45-50
        sup.setProcMap("Sp_InventoryTransactions_GetALLMethod", full("Sp_InventoryTransactions_GetALLMethod",
                row("OrganizationId", sup.org(), "CompanyId", sup.company(), "RefDocumentTypeId", DOC, "RefDocIdNo", headerId)));

        boolean fifo = sup.erpFeature(5);
        List<Ev> evals = new ArrayList<>();
        Map<Integer, String> names = new HashMap<>();
        if (fifo) {
            for (Map<String, Object> it : sup.rows("Sp_Item_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetItemGlIdsandItemName"))
                names.putIfAbsent(toInt(ci(it, "Id")), str(ci(it, "ItemName")));
            for (Det x : list) {
                if (x.actionTypeId == 3) continue;
                for (Ev out : fifoForConcrete(x, names.getOrDefault(x.itemId, ""), docDate, update, headerId, evals)) {
                    if (x.variantId == 0) out.itemUom = x.itemUomId; else out.variantId = x.variantId;               // SetData:101-108
                    Ev in = new Ev();
                    in.lineId = out.lineId; in.itemId = out.itemId; in.warehouseId = x.warehouseToId; in.rateUom = out.rateUom;
                    in.jobLotId = out.jobLotId; in.packingTypeId = out.packingTypeId; in.variantId = out.variantId;
                    if (in.variantId == 0) in.itemUom = out.itemUom; else out.itemUom = 0;
                    in.cropBatch = out.cropBatch;
                    in.qtyIn = out.qtyOut; in.billIn = out.billOut; in.stockIn = out.stockOut; in.itemRate = out.cgsRate; in.amountIn = out.cgsAmount;
                    evals.add(in);
                    evals.add(out);
                }
            }
        }
        for (Det x : list) {
            if (fifo && x.actionTypeId != 3) {
                double amt = 0, q = 0;
                for (Ev e : evals) if (e.itemId == x.itemId && e.warehouseId == x.warehouseFromId && e.variantId == x.variantId && e.qtyOut > 0) { amt += e.cgsAmount; q += e.qtyOut; }
                if (!(amt > 0.0) || !(q > 0.0))
                    throw new IllegalArgumentException("CGS Amount or Qty cannot be equal to zero.this Item " + names.getOrDefault(x.itemId, "") + " against FIFO Method");
                x.rate = BigDecimal.valueOf(amt / q);
                x.amount = x.rate.multiply(x.qty);
            }
            Map<String, Object> dp = new LinkedHashMap<>();
            dp.put("Id", x.id); dp.put("LineId", x.lineId); dp.put("InvStockTransferHeaderId", headerId);
            dp.put("WarehouseFromId", x.warehouseFromId); dp.put("WarehouseToId", x.warehouseToId); dp.put("ItemId", x.itemId);
            dp.put("JobLotFromId", x.jobLotFromId); dp.put("JobLotToId", x.jobLotToId); dp.put("AttributeVareintId", x.variantId);
            dp.put("VareintEquivalent", x.variantEq); dp.put("ItemQty", x.qty); dp.put("ItemWeight", x.itemWeight); dp.put("NetWeight", x.netWeight);
            dp.put("ItemRate", x.rate); dp.put("ItemAmount", x.amount); dp.put("RemarksDetail", x.remarks); dp.put("ActionTypeId", x.actionTypeId);
            dp.put("IsApproved", false); dp.put("ItemUomId", x.itemUomId);
            x.id = sup.setProcMap("[pcc].[USP_InvStockTransferDetail_Insert]", full("[pcc].[USP_InvStockTransferDetail_Insert]", dp));
        }
        if (fifo) {
            if (evals.isEmpty()) throw new IllegalArgumentException("InventoryStockEvalautionDetailslist not found against FIFO....");
            if (update) sup.setProc("[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                    "RefDocumentTypeId", DOC, "RefDocIdNo", headerId);
            for (Ev e : evals) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", e.id); m.put("LineId", e.lineId); m.put("ItemId", e.itemId); m.put("WarehouseId", e.warehouseId); m.put("RateUom", e.rateUom);
                m.put("JobLotId", e.jobLotId); m.put("InvPackingTypeId", e.packingTypeId); m.put("ItemUom", e.itemUom); m.put("VarientId", e.variantId); m.put("CropBatch", e.cropBatch);
                m.put("RefRefDocumentTypeId", e.refRefDocType); m.put("RefRefDocIdNo", e.refRefDocId); m.put("RefRefDocSubIdNo", e.refRefDocSub);
                m.put("QtyIn", e.qtyIn); m.put("QtyOut", e.qtyOut); m.put("BillWeightIn", e.billIn); m.put("BillWeightOut", e.billOut);
                m.put("StockWeightIn", e.stockIn); m.put("StockWeightOut", e.stockOut); m.put("CgsRate", e.cgsRate); m.put("CgsAmount", e.cgsAmount); m.put("AmountIn", e.amountIn);
                m.put("OrganizationId", sup.org()); m.put("CompanyId", sup.company()); m.put("DocDate", docDate); m.put("DocCodeNo", docNo); m.put("BranchesId", sup.branch());
                m.put("RefDocumentTypeId", DOC); m.put("EntryUser", entryUser); m.put("ModifyUser", modifyUser); m.put("CalcType", "Qty");
                m.put("ItemRate", e.billIn > 0.0 ? e.itemRate : e.cgsRate);                                          // SetData:171-178
                m.put("AmountOut", e.cgsAmount);
                if (e.lineId > 0) for (Det x : list) if (x.lineId == e.lineId) { m.put("RefDocIdNo", headerId); m.put("RefDocSubIdNo", x.id); break; }
                sup.setProcMap("USP_InventoryStockEvalautionDetail_Insert", full("USP_InventoryStockEvalautionDetail_Insert", m));
            }
        } else {
            sup.setProcMap("Sp_InventoryStockEvalautionDetail_Update", full("Sp_InventoryStockEvalautionDetail_Update",
                    row("OrganizationId", sup.org(), "CompanyId", sup.company(), "RefDocumentTypeId", DOC, "RefDocIdNo", headerId)));
        }
        for (Det x : list) {                                                                                         // DocumentTypeId 1860 stock validation (SetData:215)
            Map<String, Object> v = new LinkedHashMap<>();
            v.put("OrganizationId", sup.org()); v.put("CompanyId", sup.company()); v.put("DocumentTypeId", DOC); v.put("DocDate", docDate);
            v.put("ItemId", x.itemId); v.put("WarehouseId", x.warehouseFromId);
            if (x.variantId > 0) v.put("VarientId", x.variantId);
            v.put("Qty", x.qty);
            DesktopProc.scalar(sup.jdbc(), "USP_StockValidationConcrete", v);
        }
        return row("id", headerId, "docNo", docNo, "updated", update,
                "message", (update ? "Data Update Successfully....  " : "Data Save Successfully....  ") + docNo);
    }

    // ------------------------------------------------------------------ CommonServices.FIFOImplementionForConcrete (DAL 0205:1148) as called by DAL 0284

    private List<Ev> fifoForConcrete(Det x, String itemName, Date docDate, boolean update, int headerId, List<Ev> reserved) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("ItemId", x.itemId); p.put("DocDate", docDate);
        if (x.variantId != 0) p.put("VarientId", x.variantId);
        if (x.warehouseFromId != 0) p.put("WarehouseId", x.warehouseFromId);
        // ReportsParameters.JobLotId is never set by the DAL, so no @JobLotId is sent (see class note 4)
        if (update) { p.put("DocumentTypeId", DOC); p.put("Id", headerId); }
        if (!reserved.isEmpty()) p.put("FIFOXML", fifoXml(reserved));
        List<Map<String, Object>> layers = rows(sup.jdbc(), FIFO, p);
        if (layers == null || layers.isEmpty()) throw new IllegalArgumentException("Stock Not Found this Item " + itemName + " against FIFO Method .....");
        double available = 0;
        for (Map<String, Object> l : layers) available += toDouble(ci(l, "NetBalQty"));
        double itemQty = x.qty.doubleValue();
        if (!(itemQty <= available))
            throw new IllegalArgumentException("Qty available is " + cs(available) + " and row Qty is " + cs(itemQty) + " this item " + itemName + " against FIFO....");
        List<Ev> out = new ArrayList<>();
        double used = 0;
        for (Map<String, Object> l : layers) {
            double avg = toDouble(ci(l, "AvgRate"));
            if (avg <= 0.0) throw new IllegalArgumentException("Rate Not Found this Item " + itemName + " against FIFO Method");
            double bal = toDouble(ci(l, "NetBalQty"));
            double qtyOut;
            if (bal <= itemQty - used) qtyOut = bal;
            else if (bal >= itemQty - used) qtyOut = itemQty - used;
            else continue;
            Ev e = new Ev();
            e.id = toInt(ci(l, "Id")); e.lineId = x.lineId; e.itemId = x.itemId; e.warehouseId = x.warehouseFromId; e.rateUom = toInt(ci(l, "RateUomId"));
            e.jobLotId = 0; e.packingTypeId = 0; e.itemUom = x.variantId; e.variantId = x.variantId; e.cropBatch = "";
            e.refRefDocType = toInt(ci(l, "RefDocumentTypeId")); e.refRefDocId = toInt(ci(l, "RefDocIdNo")); e.refRefDocSub = toInt(ci(l, "RefDocSubIdNo"));
            e.qtyOut = qtyOut; e.billOut = qtyOut; e.stockOut = qtyOut; e.cgsRate = avg; e.cgsAmount = qtyOut * avg;
            out.add(e);
            used += qtyOut;
            if (itemQty == used) break;
        }
        return out;
    }

    /** CommonServices.ConvertListToXmlSerializer(List&lt;FIFOStockEvaluation&gt;). */
    private static String fifoXml(List<Ev> reserved) {
        StringBuilder xml = new StringBuilder("<ArrayOfFIFOStockEvaluation>");
        for (Ev a : reserved)
            xml.append("<FIFOStockEvaluation><RefDocumentTypeId>").append(a.refRefDocType).append("</RefDocumentTypeId><RefDocIdNo>").append(a.refRefDocId)
                    .append("</RefDocIdNo><RefDocSubIdNo>").append(a.refRefDocSub).append("</RefDocSubIdNo><ReserveQty>").append(cs(a.qtyOut))
                    .append("</ReserveQty><ReserveWeight>").append(cs(a.stockOut)).append("</ReserveWeight></FIFOStockEvaluation>");
        return xml.append("</ArrayOfFIFOStockEvaluation>").toString();
    }

    /** .NET double.ToString(). */
    private static String cs(double v) {
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return String.valueOf((long) v);
        return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
    }

    /** GenericProvider.SetProc of a model: every procedure parameter the caller did not give receives the property default (0 / false). */
    private Map<String, Object> full(String proc, Map<String, Object> given) {
        List<Object[]> params = paramCache.computeIfAbsent(proc, p -> {
            List<Object[]> l = new ArrayList<>();
            for (Map<String, Object> r : sup.jdbc().queryForList("SELECT p.name, t.name AS tname FROM sys.parameters p JOIN sys.types t ON p.user_type_id = t.user_type_id "
                    + "WHERE p.object_id = OBJECT_ID(?) ORDER BY p.parameter_id", p.contains(".") ? p : "dbo." + p))
                l.add(new Object[]{String.valueOf(r.get("name")).replace("@", ""), String.valueOf(r.get("tname")).toLowerCase()});
            return l;
        });
        Map<String, Object> out = new LinkedHashMap<>(given);
        for (Object[] p : params) {
            String nm = (String) p[0], t = (String) p[1];
            boolean has = false;
            for (String k : out.keySet()) if (k.equalsIgnoreCase(nm)) { has = true; break; }
            if (has) continue;
            if (t.equals("bit")) out.put(nm, false);
            else if (t.equals("tinyint") || t.equals("smallint") || t.equals("int") || t.equals("bigint")) out.put(nm, 0);
            else if (t.equals("float") || t.equals("real") || t.equals("decimal") || t.equals("numeric") || t.equals("money") || t.equals("smallmoney")) out.put(nm, 0.0);
        }
        return out;
    }

    // ------------------------------------------------------------------ Delete (btnDelete_Click:1495)

    @Transactional
    public Map<String, Object> delete(int id) {
        Map<String, Boolean> rt = sup.rights(SCREEN);
        lk.need(rt, "update", "Delete");                                                  // btnDelete.Enabled = DoHaveUpdateRights (Load:313)
        if (id <= 0) throw new IllegalArgumentException("RecId not Found");
        header(id);
        sup.rows(GET, "EntryUserId", sup.userId(), "Id", id, "Activity", "DeleteById");
        return row("message", "Delete Record Seccessfully");
    }
}
