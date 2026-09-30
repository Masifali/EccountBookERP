package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpBModels;
import com.mst.repositories.hrm.HrmProcRepository;
import com.mst.repositories.partyprocessing.PpBLookupRepository;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * Shared plumbing of the Party Processing transaction screens of this port: the rights the desktop
 * form reads (SetRightsValueInRightsObject), history filters, row shaping, and the two FIFO
 * allocators the save DALs run (CommonServices.FIFOImplementionForPartyProcessing and
 * CommonServices.FIFOImplemention, DAL 0205), reproduced line for line.
 */
@Component
public class PpBSupport {

    @Autowired private HrmSupport hrm;
    @Autowired private PpBLookupRepository lookups;

    public HrmSupport hrm() { return hrm; }
    public PpBLookupRepository lookups() { return lookups; }
    public HrmProcRepository db() { return lookups.db(); }

    /** Signed-in user with View on the screen. */
    public UserAccount user(int screenId) { return hrm.user(screenId); }

    /** Rightsobjects the forms read: save / update / delete / print / grid print, and CanView AllRecord. */
    public Map<String, Object> rights(UserAccount u, int screenId) {
        Map<String, Object> r = hrm.rights(u, screenId);
        r.put("gridPrint", hrm.can(u, screenId, "Grid Print"));
        r.put("viewAll", canViewAll(u, screenId));
        return r;
    }

    /** formrights.DoHaveCanViewAllRecordRights. */
    public boolean canViewAll(UserAccount u, int screenId) { return hrm.can(u, screenId, "CanView AllRecord"); }

    public static int branch(UserAccount u) { return toInt(u.getBranchesId()); }

    /** Conversion.CheckDateTimeNull guard: the date when the page sent one, else null (parameter omitted). */
    public static Timestamp day(Object v) { LocalDateTime d = toDay(v); return d == null ? null : Timestamp.valueOf(d); }

    public static Timestamp stamp(Object v) { LocalDateTime d = toDate(v); return d == null ? null : Timestamp.valueOf(d); }

    /** Non-zero int or null (a BLL "if (x != 0)" guard). */
    public static Integer nz0(int v) { return v != 0 ? v : null; }

    /** Non-blank string or null (a BLL "!= null && != string.Empty" guard). */
    public static String nzs(Object v) { String s = str(v); return s.isEmpty() ? null : s; }

    public static double d(Object v) { return toDouble(v); }

    /** Math.Round(v, n) - MidpointRounding.ToEven. */
    public static double round(double v, int n) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return v;
        return BigDecimal.valueOf(v).setScale(n, RoundingMode.HALF_EVEN).doubleValue();
    }

    /** .NET double.ToString(): shortest round-trip text, no trailing ".0" ("12.5", "40"). */
    public static String net(double v) {
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return String.valueOf((long) v);
        String s = BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
        return s;
    }

    /** Case-insensitive column read of a row. */
    public static Object col(Map<String, Object> r, String k) {
        if (r == null) return null;
        if (r.containsKey(k)) return r.get(k);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(k)) return e.getValue();
        return null;
    }

    /** Rows re-shaped to the listed columns: "Target" or "Target=Source". */
    public static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String c : cols) {
                int eq = c.indexOf('=');
                String t = eq < 0 ? c : c.substring(0, eq), s = eq < 0 ? c : c.substring(eq + 1);
                m.put(t, col(r, s));
            }
            out.add(m);
        }
        return out;
    }

    /** true when rows contain a row whose key column equals id. */
    public static boolean has(List<Map<String, Object>> rows, String key, int id) {
        for (Map<String, Object> r : rows) if (toInt(col(r, key)) == id) return true;
        return false;
    }

    /** Standard FormHistory date-type filter: the From / To dates go to the pair chosen by the radio (doc / entry / modify / approved). */
    public static void dateFilter(Map<String, Object> p, Map<String, Object> b, String from, String to,
                                  String entryFrom, String entryTo, String modFrom, String modTo, String apprFrom, String apprTo) {
        String kind = str(b.get("dateType"));
        Timestamp f = toBool(b.get("fromChecked")) ? day(b.get("fromDate")) : null;
        Timestamp t = toBool(b.get("toChecked")) ? day(b.get("toDate")) : null;
        if (kind.isEmpty() || "doc".equals(kind)) { p.put(from, f); p.put(to, t); }
        else if ("entry".equals(kind)) { p.put(entryFrom, f); p.put(entryTo, t); }
        else if ("modify".equals(kind)) { p.put(modFrom, f); p.put(modTo, t); }
        else if ("approved".equals(kind) && apprFrom != null) { p.put(apprFrom, f); p.put(apprTo, t); }
    }

    // ================================================================== FIFO (DAL 0205)

    /** One allocation line of FIFOImplementionForPartyProcessing: the FIFO source row and the out quantities. */
    public static final class PpOut {
        public long id;
        public int lineId, itemId, warehouseId, jobLotId, packingTypeId, itemUom, cropYearId;
        public int refRefDocumentTypeId, refRefDocIdNo, refRefDocSubIdNo;
        public double qtyOut, billWeightOut, stockWeightOut;
    }

    /**
     * CommonServices.FIFOImplementionForPartyProcessing(obj, lstReserveFIFO) (DAL 0205:947):
     * USP_GetStockByFifoMethodPartyProcessing with the item / party / date, the guarded pack UOM, warehouse,
     * job lot, packing type, crop year id, document type + id (update only) and the reservations already
     * taken by earlier rows (@FIFOXML), then the weight walk. Messages are the desktop's.
     */
    public List<PpOut> fifoPartyProcessing(UserAccount u, int itemId, int stockPartyId, Timestamp docDate, int packUomId, int warehouseId,
                                           int jobLotId, int packingTypeId, int cropYearId, int documentTypeId, int id,
                                           double itemQty, double netWeight, int lineId, String itemName, List<PpOut> reserve) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("ItemId", itemId);
        p.put("StockPartyId", stockPartyId);
        p.put("DocDate", docDate);
        p.put("PackUomId", nz0(packUomId));
        p.put("WarehouseId", nz0(warehouseId));
        p.put("JobLotId", nz0(jobLotId));
        p.put("PackingTypeId", nz0(packingTypeId));
        p.put("CropYearId", nz0(cropYearId));
        p.put("DocumentTypeId", nz0(documentTypeId));
        p.put("Id", nz0(id));
        if (!reserve.isEmpty()) {
            StringBuilder x = new StringBuilder("<ArrayOfFIFOPartyProcessing xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            for (PpOut r : reserve) {
                x.append("\n  <FIFOPartyProcessing><Id>0</Id><DocDate>0001-01-01T00:00:00</DocDate><ItemId>0</ItemId><WarehouseId>0</WarehouseId>")
                 .append("<ItemUomId>0</ItemUomId><RateUomId>0</RateUomId>")
                 .append("<RefDocumentTypeId>").append(r.refRefDocumentTypeId).append("</RefDocumentTypeId>")
                 .append("<RefDocIdNo>").append(r.refRefDocIdNo).append("</RefDocIdNo>")
                 .append("<RefDocSubIdNo>").append(r.refRefDocSubIdNo).append("</RefDocSubIdNo>")
                 .append("<QtyIn>0</QtyIn><QtyOut>0</QtyOut><BalQty>0</BalQty><WeightIn>0</WeightIn><WeightOut>0</WeightOut><BalWeight>0</BalWeight>")
                 .append("<ReserveWeight>").append(xmlNum(r.stockWeightOut)).append("</ReserveWeight>")
                 .append("<ReserveQty>").append(xmlNum(r.qtyOut)).append("</ReserveQty>")
                 .append("<NetBalQty>0</NetBalQty><NetBalWeight>0</NetBalWeight></FIFOPartyProcessing>");
            }
            x.append("\n</ArrayOfFIFOPartyProcessing>");
            p.put("FIFOXML", x.toString());
        }
        List<Map<String, Object>> list3 = db().rows("USP_GetStockByFifoMethodPartyProcessing", p);
        String name = str(itemName);
        if (list3.isEmpty()) throw invalid("Stock Not Found this Item " + name + " against FIFO Method .....");
        double value2 = 0;
        for (Map<String, Object> r : list3) value2 += d(col(r, "NetBalWeight"));
        if (!(netWeight <= round(value2, 2))) {
            throw invalid("Weight available is " + net(value2) + " and row Weight is " + net(netWeight) + " this item " + name + " against FIFO....");
        }
        List<PpOut> out = new ArrayList<>();
        double num = netWeight, num2 = itemQty, num4 = 0, num6 = 0;
        for (Map<String, Object> s : list3) {
            double num5 = d(col(s, "NetBalWeight")), num3 = d(col(s, "NetBalQty"));
            PpOut o = new PpOut();
            o.id = toInt(col(s, "Id"));
            o.lineId = lineId; o.itemId = itemId; o.warehouseId = warehouseId; o.jobLotId = jobLotId;
            o.packingTypeId = packingTypeId; o.itemUom = packUomId; o.cropYearId = cropYearId;
            o.refRefDocumentTypeId = toInt(col(s, "RefDocumentTypeId"));
            o.refRefDocIdNo = toInt(col(s, "RefDocIdNo"));
            o.refRefDocSubIdNo = toInt(col(s, "RefDocSubIdNo"));
            if (num5 <= num - num6) {
                num6 += num5;
                num4 += num3;
                o.qtyOut = num3; o.billWeightOut = num5; o.stockWeightOut = num5;
                out.add(o);
            } else if (num5 >= num - num6) {
                o.qtyOut = num2 - num4; o.billWeightOut = num - num6; o.stockWeightOut = num - num6;
                num4 += o.qtyOut;
                num6 += o.billWeightOut;
                out.add(o);
            }
            if (netWeight == num6) break;
        }
        return out;
    }

    /** Model 0401 row for USP_InventoryTransactionsPartyProcessing_Insert from one PpOut (the caller fills the header fields). */
    public static PpBModels.TransPpFifo toFifoModel(PpOut o) {
        PpBModels.TransPpFifo m = new PpBModels.TransPpFifo();
        m.Id = o.id; m.LineId = o.lineId; m.ItemId = o.itemId; m.WarehouseId = o.warehouseId; m.JobLotId = o.jobLotId;
        m.InvPackingTypeId = o.packingTypeId; m.ItemUom = o.itemUom; m.CropYearId = o.cropYearId;
        m.RefRefDocumentTypeId = o.refRefDocumentTypeId; m.RefRefDocIdNo = o.refRefDocIdNo; m.RefRefDocSubIdNo = o.refRefDocSubIdNo;
        m.QtyOut = o.qtyOut; m.BillWeightOut = o.billWeightOut; m.StockWeightOut = o.stockWeightOut;
        return m;
    }

    /**
     * CommonServices.FIFOImplemention(obj, lstReserveFIFO) (DAL 0205:725): USP_GetStockByFifoMethod and the weight walk
     * with the rate checks, the rate-UOM equivalent (GetEqvilentByItemIdAndUomScheduleId) and the CGS amounts.
     * Returns InventoryStockEvalautionDetail rows (model 1023) - the caller fills the header fields.
     */
    public List<PpBModels.StockEvalautionDetail> fifo(UserAccount u, int itemId, Timestamp docDate, int packUomId, int warehouseId,
                                                        int cropYearId, int jobLotId, int packingTypeId, String cropYear, int documentTypeId,
                                                        int id, double itemQty, double netWeight, int lineId, String itemName,
                                                        List<PpBModels.StockEvalautionDetail> reserve) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("ItemId", itemId);
        p.put("DocDate", docDate);
        p.put("PackUomId", nz0(packUomId));
        p.put("WarehouseId", nz0(warehouseId));
        p.put("CropYearId", nz0(cropYearId));
        p.put("JobLotId", nz0(jobLotId));
        p.put("PackingTypeId", nz0(packingTypeId));
        p.put("CropYear", nzs(cropYear));
        p.put("DocumentTypeId", nz0(documentTypeId));
        p.put("Id", nz0(id));
        if (!reserve.isEmpty()) {
            StringBuilder x = new StringBuilder("<ArrayOfFIFOStockEvaluation xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            for (PpBModels.StockEvalautionDetail r : reserve) {
                x.append("\n  <FIFOStockEvaluation><Id>0</Id><DocDate>0001-01-01T00:00:00</DocDate><ItemId>0</ItemId><WarehouseId>0</WarehouseId>")
                 .append("<ItemUomId>0</ItemUomId><RateUomId>0</RateUomId>")
                 .append("<RefDocumentTypeId>").append(r.RefRefDocumentTypeId).append("</RefDocumentTypeId>")
                 .append("<RefDocIdNo>").append(r.RefRefDocIdNo).append("</RefDocIdNo>")
                 .append("<RefDocSubIdNo>").append(r.RefRefDocSubIdNo).append("</RefDocSubIdNo>")
                 .append("<QtyIn>0</QtyIn><QtyOut>0</QtyOut><BalQty>0</BalQty><WeightIn>0</WeightIn><WeightOut>0</WeightOut>")
                 .append("<BalWeight>0</BalWeight><AmountIn>0</AmountIn><AmountOut>0</AmountOut><BalAmount>0</BalAmount><AvgRate>0</AvgRate>")
                 .append("<ReserveWeight>").append(xmlNum(r.StockWeightOut)).append("</ReserveWeight>")
                 .append("<ReserveQty>").append(xmlNum(r.QtyOut)).append("</ReserveQty>")
                 .append("<NetBalQty>0</NetBalQty><NetBalWeight>0</NetBalWeight></FIFOStockEvaluation>");
            }
            p.put("FIFOXML", x.append("\n</ArrayOfFIFOStockEvaluation>").toString());
        }
        List<Map<String, Object>> list3 = db().rows("USP_GetStockByFifoMethod", p);
        String name = str(itemName);
        if (list3.isEmpty()) throw invalid("Stock Not Found this Item " + name + " against FIFO Method .....");
        double value2 = 0;
        for (Map<String, Object> r : list3) value2 += d(col(r, "NetBalWeight"));
        if (!(netWeight <= round(value2, 2))) {
            throw invalid("Weight available is " + net(value2) + " and row Weight is " + net(netWeight) + " this item " + name + " against FIFO....");
        }
        List<PpBModels.StockEvalautionDetail> out = new ArrayList<>();
        double num2 = netWeight, num3 = itemQty, num5 = 0, num7 = 0;
        for (Map<String, Object> s : list3) {
            if (d(col(s, "AvgRate")) <= 0.0) throw invalid("Rate Not Found this Item " + name + " against FIFO Method");
            int rateUom = toInt(col(s, "RateUomId"));
            if (rateUom == 0) throw invalid("RateUomId not found  this " + name + " against FIFO Method");
            double num6 = d(col(s, "NetBalWeight")), num4 = d(col(s, "NetBalQty"));
            double eq = lookups.equivalentByItemAndSchedule(u, itemId, rateUom);
            if (eq == 0.0) throw invalid("RateUom Not Found");
            PpBModels.StockEvalautionDetail e = new PpBModels.StockEvalautionDetail();
            e.Id = toInt(col(s, "Id"));
            e.LineId = lineId; e.ItemId = itemId; e.WarehouseId = warehouseId; e.RateUom = rateUom;
            e.JobLotId = jobLotId; e.InvPackingTypeId = packingTypeId; e.ItemUom = packUomId; e.CropBatch = str(cropYear);
            e.RefRefDocumentTypeId = toInt(col(s, "RefDocumentTypeId"));
            e.RefRefDocIdNo = toInt(col(s, "RefDocIdNo"));
            e.RefRefDocSubIdNo = toInt(col(s, "RefDocSubIdNo"));
            e.CgsRate = d(col(s, "AvgRate")) * eq;
            if (num6 <= num2 - num7) {
                num7 += num6;
                num5 += num4;
                e.QtyOut = num4; e.BillWeightOut = num6; e.StockWeightOut = num6;
                e.CgsAmount = e.BillWeightOut / eq * e.CgsRate;
                out.add(e);
            } else if (num6 >= num2 - num7) {
                e.QtyOut = num3 - num5; e.BillWeightOut = num2 - num7; e.StockWeightOut = num2 - num7;
                e.CgsAmount = e.BillWeightOut / eq * e.CgsRate;
                num5 += e.QtyOut;
                num7 += e.BillWeightOut;
                out.add(e);
            }
            if (netWeight == num7) break;
        }
        return out;
    }

    /** XmlSerializer double text (round-trip, "0" for zero). */
    private static String xmlNum(double v) {
        if (v == 0d) return "0";
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return String.valueOf((long) v);
        return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
    }

    /** Item name for the FIFO messages (GetItemGlIdsandItemName row of the item), null when the item has no row. */
    public static String itemName(List<Map<String, Object>> glItems, int itemId) {
        for (Map<String, Object> r : glItems) if (toInt(col(r, "Id")) == itemId) return str(col(r, "ItemName"));
        return null;
    }

    // ================================================================== loader popup (LoadavailableTransactionsForIssuancePartyProcessing)

    /** ComboFill(): StocksReport.InventoryTransactionsPartyProcessing_DropDownAndList (@Org @Company) split by ActivityType,
     *  plus ActiveYr.Start_Period (Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId, the session year's row). */
    public Map<String, Object> loaderSetup(UserAccount u) {
        Map<String, List<Map<String, Object>>> lists = new LinkedHashMap<>();
        for (String k : new String[] { "StockParty", "ReferenceParty", "RefDocumentType", "RefWarehouse", "Warehouse", "JobLot", "Items",
                "PackingType", "CropYear" }) lists.put(k, new ArrayList<>());
        for (Map<String, Object> r : db().rows("USP_InventoryTransactionsPartyProcessing_DropDownAndLists",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId())) {
            List<Map<String, Object>> l = lists.get(str(col(r, "ActivityType")));
            if (l != null) l.add(map("Id", col(r, "Id"), "name", col(r, "name")));
        }
        Map<String, Object> out = new LinkedHashMap<>(lists);
        Object start = null;
        int y = hrm.financialYearId();
        for (Map<String, Object> r : db().rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId())) {
            if (toInt(col(r, "Id")) == y) start = col(r, "Start_Period");
        }
        out.put("fromDate", start);
        return out;
    }

    /** PendingInventoryTransactionsForIssuanceLoad(): the page's filters; the stock party is the form's (LoadTransactions.StockPartyId). */
    public List<Map<String, Object>> loaderRows(UserAccount u, Map<String, Object> b) {
        return available(u, day(b.get("fromDate")), day(b.get("toDate")), toInt(b.get("stockPartyId")), toInt(b.get("refPartyId")),
                toInt(b.get("refDocumentTypeId")), toInt(b.get("refWarehouseId")), toInt(b.get("warehouseId")), toInt(b.get("jobLotId")),
                toInt(b.get("itemId")), toInt(b.get("packingTypeId")), toInt(b.get("cropYearId")));
    }

    /** BLL 0574 GetAvailableTransactionsForIssuanceForPartyProcessing -> SpInventoryTransactionsPartyProcessing_GetAvailableTransactionsForIssuance
     *  (dates when set, the ids when not 0), projected as the popup's dtTarget. */
    public List<Map<String, Object>> available(UserAccount u, Timestamp from, Timestamp to, int stockPartyId, int refPartyId, int refDocType,
                                               int refWarehouseId, int warehouseId, int jobLotId, int itemId, int packingTypeId, int cropYearId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("FromDate", from);
        p.put("ToDate", to);
        p.put("StockPartyId", nz0(stockPartyId));
        p.put("ReferencePartyId", nz0(refPartyId));
        p.put("RefDocumentTypeId", nz0(refDocType));
        p.put("RefWarehouseId", nz0(refWarehouseId));
        p.put("WarehouseId", nz0(warehouseId));
        p.put("JobLotId", nz0(jobLotId));
        p.put("ItemId", nz0(itemId));
        p.put("PackingTypeId", nz0(packingTypeId));
        p.put("CropYearId", nz0(cropYearId));
        return pick(db().rows("SpInventoryTransactionsPartyProcessing_GetAvailableTransactionsForIssuance", p),
                "RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo", "RefDocType=RefDocumentType", "DocDate", "DocCodeNo", "WarehouseId",
                "WareHouse=WareHouseCode", "ItemId", "ItemName", "ItemCode", "CropYearId", "CropYear", "JobLotId", "JobLotCode",
                "InvPackingTypeId", "PackingType", "ItemUomId", "PackUom", "Equivalent", "QtyIn", "QtyOut", "QtyBalance", "WeightIn",
                "WeightOut", "WeightBalance", "StockPartyId", "StockParty", "SupplierCustomerId", "ReferenceParty=ReferencePartyName", "GpNo",
                "RefWarehouse", "VehicleNo", "JobOrderId", "JobOrderNo");
    }
}
