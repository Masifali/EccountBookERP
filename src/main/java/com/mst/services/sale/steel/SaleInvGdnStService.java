package com.mst.services.sale.steel;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleEngrSupport;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 540 "InvFrmGDN_St" = Architecture.WinApp.Steel.Sale.InvFrmGDN_St (Goods Dispatch Notes, Sale Steel module 84, document type 1508).
 *
 * Desktop map (InvFrmGDN_St.cs): Add_Click :403, Insert :423, ReadById :688, FormValidation :773, FormDetailValidation :820, defaultConfiquration :884,
 * GenerateCode :919, SupplierNameFilll :951, TransportFill :967, PendingGatepass :993, GRNGridSetting :1049, ItemNameFill :1089, ItemNameFillWithoutOrder :1119,
 * JobLotFillWithoutOrder :1136, VehicleTypeFill :1152, PackingTypeFill :1168, PackUOMFillWithoutOrder :1186, WarehouseFill :1207, CityFill :1224,
 * ReferancePartyBind :1246, GatePassRecordFillWinthoutSaleorder :1262, GatePassRecordFill :1305, DeliverOrderRecordFill :1379, combitem_Leave :1442,
 * AvailableStock :1522, comborderno_Leave :1558, Total :1600, reset :1701, grdSettings :1791, InvFrmGRN_Load :1918, HistoryCombosFill :2081,
 * HistoryGridFill :2115, combgatepass_Leave :2418, grd_ColumnButtonClick :2524, grd_DoubleClick :2680, btnUpdateDetail_Click :2729, DetailGridBind :2810,
 * DataGridHistory_ColumnButtonClick :2936, btnDelete_Click :2961, combsupplier_Leave :2978, grd_CellUpdated :3072, NetandStockWeight :3114, PrintReportA :3160,
 * grdGp_ColumnButtonClick :3292.
 *
 * Procedures: [ST].[USP_InvGdn_GetAllMethod] (GenerateCode, ReadById, ReadByHeaderId, FormHistory, GetGdnGrossWeightForValidation, DeleteById),
 * [ST].[USP_InvGdn_Insert] / _Update, [ST].[USP_InvGdnDetail_Insert], Sp_InventoryTransactions_GetALLMethod, [ST].[USP_GetOutstandingOutwardForDispatchNote],
 * [ST].[USP_ReadByGpNoForGdn], [ST].[USP_ReadByGpNoForGdnDeliveryOrder], [ST].[USP_GetDateFromSaleOrderByOrderId], [ST].[USP_DeliveryOrder_GetAllMethod]
 * (DeliveryOrderLoad, CustomerLoadForDeliveryOrderId), Sp_WbTransation_GetAllMethod, Sp_GetAvgRatesAndStockInHand_GetAllMethod, [ST].[USP_GetDataForDropDownFromSaleOrder].
 */
@Service
public class SaleInvGdnStService {
    public static final String SCREEN = "InvFrmGDN_St";
    public static final int DOC_TYPE = 1508;
    private static final String P_GDN = "[ST].[USP_InvGdn_GetAllMethod]";
    private static final String P_DO = "[ST].[USP_DeliveryOrder_GetAllMethod]";

    private final SaleEngrSupport sup;
    private final SaleSteelSupport steel;
    private final SaleSteelLookups look;
    private final SaleEngrAttachments attachments;

    public SaleInvGdnStService(SaleEngrSupport sup, SaleSteelSupport steel, SaleSteelLookups look, SaleEngrAttachments attachments) {
        this.sup = sup; this.steel = steel; this.look = look; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load (InvFrmGRN_Load :1918)

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>(lists());
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());                                                  // GenerateCode
        m.put("customers", look.customers());                                       // SupplierNameFilll (also rebound by ReadById)
        m.put("items", look.items());                                               // ItemNameFillWithoutOrder
        m.put("defaults", defaults());                                              // defaultConfiquration
        m.put("wagesStatus", toBool(sup.config("ContractorWagesCompulsoryBeforeInvoices")));
        m.put("historyCustomers", historyCustomers());                              // HistoryCombosFill
        m.put("historyDays", look.historyDays());
        m.put("pending", pending());                                                // PendingGatepass
        return m;
    }

    /** ReferancePartyBind, TransportFill, VehicleTypeFill, JobLotFillWithoutOrder, PackingTypeFill, WarehouseFill, CityFill. */
    public Map<String, Object> lists() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("refParties", look.referenceParties());
        m.put("transporters", transporters());
        m.put("vehicleTypes", look.vehicleTypes());
        m.put("jobLots", look.jobLots());
        m.put("packingTypes", look.packingTypes());
        m.put("warehouses", look.warehouses());
        m.put("cities", sup.rows("SP_City_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll"));
        return m;
    }

    /** toolStripButton1_Click :2778 - TransportFill, JobLotFillWithoutOrder, PackingTypeFill, WarehouseFill, CityFill. */
    public Map<String, Object> refresh() {
        Map<String, Object> all = lists();
        Map<String, Object> m = new LinkedHashMap<>();
        for (String k : List.of("transporters", "jobLots", "packingTypes", "warehouses", "cities")) m.put(k, all.get(k));
        return m;
    }

    /** TransportFill :967 - CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,,13,14,15,20,21,22") = COAAllocation.GetAccountTitleByAccountTypeIds (the list is the NOT list). */
    public List<Map<String, Object>> transporters() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("AppId", sup.ctx().currentAppId());
        p.put("AccountTypeIdsNot", "2,11,12,,13,14,15,20,21,22");
        if (sup.userId() != 0) p.put("UserId", sup.userId());
        p.put("Activity", "GetAccountTitleByAccountTypeIds");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(sup.jdbc(), "Sp_COAAllocation_GetAllMethod", p)) out.add(row("Id", r.get("Id"), "AccountTitle", r.get("AccountTitle")));
        return out;
    }

    /** defaultConfiquration :884 - City Area, Job/Lot, Paking Type, Warehouse (each only when the configuration row exists). */
    public Map<String, Object> defaults() {
        Map<String, Object> m = new LinkedHashMap<>();
        for (String[] c : new String[][]{{"cityId", "City Area"}, {"jobLotId", "Job/Lot"}, {"packingTypeId", "Paking Type"}, {"warehouseId", "Warehouse"}}) {
            String v = sup.config(c[1]);
            if (!v.isEmpty()) m.put(c[0], toInt(v));
        }
        return m;
    }

    /** GenerateCode :919 - InvGdn.GenerateInvGdnCode (0 -> "Max Number Not Found"). */
    public int nextNo() {
        UserAccount u = sup.user();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId()); p.put("BranchesId", u.getBranchesId());
        p.put("DocumentTypeId", DOC_TYPE); p.put("FinancialYearId", sup.fy()); p.put("Activity", "GenerateCode");
        List<Map<String, Object>> r = DesktopProc.rows(sup.jdbc(), P_GDN, p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** PendingGatepass :993 - the ordered grid of ST.USP_GetOutstandingOutwardForDispatchNote, reshaped to the 17 columns of dtgp. */
    public Map<String, Object> pending() {
        UserAccount u = sup.user();
        SaleSteelSupport.Table t = steel.table("[ST].[USP_GetOutstandingOutwardForDispatchNote]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", u.getBranchesId(), "FinancialYearId", sup.fy());
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : t.rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String k : List.of("OutwardGatePassId", "GatepassType", "SupplierContractCode", "OrderType", "OrderNo", "GpSrNo", "GpDate", "VehicleType", "VehicleNo", "BiltyNo",
                    "CustomerName", "VarietyName", "NoOfPackages", "SupplierWeight", "FactoryWeight", "DifferenceWeight", "Status"))
                o.put(k, r.get(k));
            rows.add(o);
        }
        return row("cols", List.of("OutwardGatePassId", "GatepassType", "SupplierContractCode", "OrderType", "OrderNo", "GpSrNo", "GpDate", "VehicleType", "VehicleNo", "BiltyNo",
                "CustomerName", "VarietyName", "NoOfPackages", "SupplierWeight", "FactoryWeight", "DifferenceWeight", "Status"), "rows", rows);
    }

    /** CommonServices.GetUomScheduleByItemId (PackUOMFillWithoutOrder :1186). */
    public List<Map<String, Object>> uoms(int itemId) { return look.uoms(itemId); }

    /** AvailableStock :1522 - GetAvgRatesAndStockInHand.GetStockInHandFromInventoryTrasactions (first column of the first row). */
    public double stock(int itemId, String docDate, int jobLotId, int warehouseId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("ItemId", itemId); p.put("DocDate", ts(docDate));
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        p.put("Activity", "GetStockInHandFromInventoryTrasactions");
        SaleSteelSupport.Table t = steel.table("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        if (t.rows.isEmpty() || t.cols.isEmpty()) return 0.0;
        return toDouble(t.rows.get(0).get(t.cols.get(0)));
    }

    /** comborderno_Leave :1558 - SaleOrder.GetDateFromSaleOrderByOrderId. */
    public List<Map<String, Object>> orderItems(int orderId) {
        return sup.rows("[ST].[USP_GetDateFromSaleOrderByOrderId]", "SaleOrderId", orderId);
    }

    // ------------------------------------------------------------------ gate pass loading (combgatepass_Leave :2418)

    /** InvGdn.ReadByGpNoForGdnDeliveryOrder (Id = gate pass id) + the customers + the dispatched gross weight (the part of combgatepass_Leave that runs for a delivery order). */
    public Map<String, Object> gatePassDeliveryOrder(int gpId) {
        UserAccount u = sup.user();
        List<Map<String, Object>> r = sup.rows("[ST].[USP_ReadByGpNoForGdnDeliveryOrder]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", gpId, "FinancialYearId", sup.fy());
        Map<String, Object> out = new LinkedHashMap<>();
        if (r.isEmpty()) { out.put("found", false); return out; }
        out.put("found", true);
        out.put("head", plain(r.get(0), "FactoryWeight", "NetPaid"));
        int soOrderId = toInt(r.get(0).get("SaleOrderId"));
        String status = str(r.get(0).get("OrderStatus"));
        if ("DeliveryOrder".equals(status)) {
            int gp = toInt(r.get(0).get("Id"));
            out.put("customers", sup.rows(P_DO, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", gp, "Activity", "CustomerLoadForDeliveryOrderId"));
            out.put("gdnGrossWeight", gdnGrossWeight(gp));
        }
        out.put("soOrderId", soOrderId);
        return out;
    }

    /** InvGdn.GetGdnGrossWeightForValidation (DocumentTypeId 1508). */
    public double gdnGrossWeight(int gpId) {
        UserAccount u = sup.user();
        List<Map<String, Object>> r = sup.rows(P_GDN, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", DOC_TYPE, "Id", gpId, "Activity", "GetGdnGrossWeightForValidation");
        return r.isEmpty() ? 0.0 : toDouble(r.get(0).get("GrossWeight"));
    }

    /** DeliverOrderRecordFill :1379 - InvDeliveryOrder.DeliveryOrderLoad (DocumentTypeId 1506), the columns in the order the procedure selects them. */
    public Map<String, Object> deliveryLines(int soOrderId, int customerId) {
        UserAccount u = sup.user();
        return steel.grid(P_DO, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", 1506, "SupplierCustomerId", customerId, "Id", soOrderId, "Activity", "DeliveryOrderLoad");
    }

    /** GatePassRecordFill :1305 / GatePassRecordFillWinthoutSaleorder :1262 - InvGdn.ReadByGpNoForGdn (+ the weighbridge net weight for the sale order flavour). */
    public Map<String, Object> gatePassRows(int gpId, boolean withNetWeight) {
        UserAccount u = sup.user();
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> x : sup.rows("[ST].[USP_ReadByGpNoForGdn]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", gpId))
            rows.add(plain(x, "NetPaid", "NoOfPackages", "SupplierWeight", "FactoryWeight", "DifferenceWeight"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        if (withNetWeight && !rows.isEmpty()) {
            int id = toInt(rows.get(0).get("Id"));
            List<Map<String, Object>> wb = sup.rows("Sp_WbTransation_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id,
                    "RefDocumentTypeId", 1507, "Activity", "GetNetWeightFromWbTransactions");
            double net = 0.0;
            for (Map<String, Object> w : wb) net += toDouble(w.get("NetWbWeight"));
            out.put("wbRows", wb.size());
            out.put("netWeight", net);
        }
        return out;
    }

    // ------------------------------------------------------------------ History (HistoryGridFill :2115)

    /** HistoryCombosFill :2081 - SaleOrder.GetDataForDropDownFromSaleOrder, the 'Customer' rows. */
    public List<Map<String, Object>> historyCustomers() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("[ST].[USP_GetDataForDropDownFromSaleOrder]", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            if ("Customer".equals(str(r.get("Activity")))) out.add(row("Id", r.get("Id"), "Customer", r.get("ReferenceName")));
        return out;
    }

    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate, int fromNo, int toNo, int customerId) {
        boolean viewAll = Boolean.TRUE.equals(sup.rights(SCREEN).get("viewAll"));
        UserAccount u = sup.user();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId()); p.put("BranchesId", u.getBranchesId());
        p.put("DocumentTypeId", DOC_TYPE); p.put("FinancialYearId", sup.fy()); p.put("CanViewAllRecord", viewAll);
        if (!viewAll) p.put("EntryUserId", u.getId());                              // the procedure names the parameter @EntryUserId (the BLL's @EntryUser is not one of its parameters)
        p.put("Activity", "FormHistory");
        String fk = "FromDate", tk = "ToDate";
        if ("entry".equals(dateType)) { fk = "EntryFromDate"; tk = "EntryToDate"; }
        else if ("modify".equals(dateType)) { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
        else if ("approved".equals(dateType)) { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
        Timestamp f = ts(fromDate), t = ts(toDate);
        if (f != null) p.put(fk, f);
        if (t != null) p.put(tk, t);
        if (fromNo != 0) p.put("DocNoFrom", fromNo);
        if (toNo != 0) p.put("DocNoTo", toNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(sup.jdbc(), P_GDN, p)) {          // the dt of HistoryGridFill, in its column order
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id")); o.put("DocNo", r.get("DocNo")); o.put("DocDate", r.get("DocDate")); o.put("CustomerName", r.get("SupplierName"));
            o.put("DeliveryTerm", r.get("DeliveryTerm")); o.put("Transporter", r.get("TransporterName")); o.put("Freight", r.get("CarriageAmount"));
            o.put("GpId", r.get("OutwardGatePassId")); o.put("GpNo", r.get("GpNo")); o.put("VehicleType", r.get("VehicleDescription")); o.put("VehicleNo", r.get("VehicleNo"));
            o.put("BiltyNo", r.get("BiltyNo")); o.put("RefrenceParty", r.get("PartyReference")); o.put("TotalQty", r.get("TotalQty")); o.put("FactoryWeight", r.get("FactoryWeight"));
            o.put("PartyWeight", r.get("PartyWeight")); o.put("EntryUser", r.get("EntryUserName")); o.put("EntryDate", r.get("EntryDate")); o.put("ModifyUser", r.get("ModifyUserName"));
            o.put("ModifyDate", r.get("ModifyDate")); o.put("IsApproved", toBool(r.get("IsApproved")) ? "Approved" : "Not Approved"); o.put("ApprovedUser", r.get("ApprovedUserName"));
            o.put("ApprovedDate", r.get("ApprovedDate")); o.put("NoOfAttachments", r.get("NoOfAttachments")); o.put("Remarks", r.get("RemarksHeader"));
            out.add(o);
        }
        return out;
    }

    /** DetailGridBind :2810 - the 15 columns of dtDetailHistory (Math.Round on the weights). */
    public List<Map<String, Object>> historyDetail(int id) {
        header(id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : sup.rows(P_GDN, "Id", id, "Activity", "ReadByHeaderId")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("OrderNo", d.get("SaleOrderNo")); o.put("ItemName", d.get("ItemName")); o.put("JobLot", d.get("JobLotDescription")); o.put("PackingType", d.get("PackingType"));
            o.put("PackUom", d.get("UOMCode")); o.put("ItemQty", d.get("ItemQty")); o.put("GrossWeight", d.get("GrossWeight"));
            o.put("WeightCut", bank(d.get("WtCut"))); o.put("WeightCutTotal", bank(d.get("WtCutTotal"))); o.put("AdLsWeight", bank(d.get("AdLsWeight")));
            o.put("NetBillWeight", bank(d.get("NetBillWeight"))); o.put("StockWeight", bank(d.get("StockWeight")));
            o.put("WareHouse", d.get("WareHouseName")); o.put("LabNo", d.get("LabReportRef")); o.put("CityName", d.get("CityName"));
            out.add(o);
        }
        return out;
    }

    /** Decimal columns keep their scale in ToString() (decimal(18,4) shows 1000.0000): send them as text so the page shows what the desktop shows. */
    private static Map<String, Object> plain(Map<String, Object> m, String... keys) {
        Map<String, Object> o = new LinkedHashMap<>(m);
        for (String k : keys) for (String have : new ArrayList<>(o.keySet())) if (have.equalsIgnoreCase(k) && o.get(have) instanceof BigDecimal b) o.put(have, b.toPlainString());
        return o;
    }

    private static Object bank(Object v) {                                           // Math.Round(decimal): to even
        if (v == null) return 0;
        try { return new BigDecimal(String.valueOf(v)).setScale(0, RoundingMode.HALF_EVEN); } catch (RuntimeException e) { return 0; }
    }

    // ------------------------------------------------------------------ ReadById :688

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> r = sup.rows(P_GDN, "Id", id, "Activity", "ReadById");
        UserAccount u = sup.user();
        if (r.isEmpty() || toInt(r.get(0).get("OrganizationId")) != u.getOrganizationId() || toInt(r.get(0).get("CompanyId")) != u.getCompanyId()
                || toInt(r.get(0).get("DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Goods dispatch note not found in this company");
        return r.get(0);
    }

    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", plain(h, "CarriageAmount", "FactoryWeight", "PartyWeight"));
        out.put("lines", sup.rows(P_GDN, "Id", id, "Activity", "ReadByHeaderId"));
        int gpId = toInt(h.get("OutwardGatePassId"));
        if ("DeliveryOrder".equals(str(h.get("OrderType")))) out.put("gdnGrossWeight", gdnGrossWeight(gpId));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    // ------------------------------------------------------------------ Save (Insert :423)

    public static class Line {
        public int id, saleOrderDetailId, saleOrderId, saleOrderNo, refPartyId, itemId, jobLotId, packingTypeId, packUomId, warehouseId, cityId;
        public double itemQty, grossWeight, wtCut, wtCutTotal, addLess, netWeight, stockWeight;
        public String labNo, remarks, vehicleNo;
    }

    public static class Request {
        public int id;
        public String docDate, docNo;
        public int customerId;
        public String deliveryTerm;
        public int transporterId;
        public String carriageAmount, gatePassNo;
        public int vehicleTypeId;
        public String vehicleNo, biltyNo;
        public int refPartyId;
        public String factoryWeight, partyWeight, remarks;
        public int gpId;
        public String orderType;
        public String partyCount, gdnGrossWeight;
        public List<Line> lines = new ArrayList<>();
        public List<Line> removed = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    /** FormValidation :773 - the first failing message, word for word. */
    private void formValidation(Request r) {
        String no = text(r.docNo);
        if (no.isEmpty() || "0".equals(no)) throw new Warning("DocNo Field is Required");
        if (r.customerId <= 0) throw new Warning("Supplier Field is Required");
        if (text(r.deliveryTerm).isEmpty()) throw new Warning("Delivery Term Field is Required");
        String freight = text(r.carriageAmount);
        if (r.transporterId > 0 && (freight.isEmpty() || "0".equals(freight))) throw new Warning("Freight Field is Required");
        if (toDouble(freight) > 0.0 && r.transporterId <= 0) throw new Warning("Transport Field is Required");
        String gp = text(r.gatePassNo);
        if (gp.isEmpty() || "0".equals(gp)) throw new Warning("Gatepass Field is Required");
        String fw = text(r.factoryWeight);
        if (fw.isEmpty() || "0".equals(fw)) throw new Warning("FactoryWeight Field is Required");
    }

    private static BigDecimal dec(double v) { return new BigDecimal(v, new MathContext(15, RoundingMode.HALF_EVEN)); }

    private static BigDecimal decText(String s) {                                    // Conversion.ToDecimal(text): 0 when not a number
        try { return new BigDecimal(text(s).replace(",", "")); } catch (RuntimeException e) { return BigDecimal.ZERO; }
    }

    private static String plain(BigDecimal b) { return b.compareTo(BigDecimal.ZERO) == 0 ? "0" : b.stripTrailingZeros().toPlainString(); }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        List<Line> rows = r.lines == null ? List.of() : r.lines;
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        formValidation(r);
        boolean gateSale = "GateSale".equals(text(r.orderType));

        // the saved rows of this note (an update may only touch those) and the organisation checks the desktop gets from its own combos
        Set<Integer> savedIds = new HashSet<>();
        if (r.id > 0) {
            header(r.id);
            for (Map<String, Object> d : sup.rows(P_GDN, "Id", r.id, "Activity", "ReadByHeaderId")) savedIds.add(toInt(d.get("Id")));
        }
        if (r.gpId > 0) {
            Integer n = sup.jdbc().queryForObject("SELECT COUNT(*) FROM GatePassOutward WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?", Integer.class, r.gpId, u.getOrganizationId(), u.getCompanyId());
            if (n == null || n == 0) throw new IllegalArgumentException("Gate pass not found in this company");
        }

        BigDecimal grossWt = BigDecimal.ZERO;
        List<Map<String, Object>> details = new ArrayList<>();
        int rowNo = 0;
        for (Line l : rows) {
            rowNo++;
            if (!gateSale) {
                if (l.saleOrderId <= 0) throw new IllegalArgumentException("OrderNo Field Required");
                Integer n = sup.jdbc().queryForObject("SELECT COUNT(*) FROM [ST].[SaleOrder] WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?", Integer.class, l.saleOrderId, u.getOrganizationId(), u.getCompanyId());
                if (n == null || n == 0) throw new IllegalArgumentException("Sale order not found in this company");
            }
            if (l.itemId <= 0) throw new IllegalArgumentException("ItemName Field Required in Row#" + rowNo);
            if (l.jobLotId <= 0) throw new IllegalArgumentException("JobLot Field Required in Row#" + rowNo);
            if (l.packingTypeId <= 0) throw new IllegalArgumentException("Packing Type Field Required in Row#" + rowNo);
            if (!(l.itemQty > 0.0)) throw new IllegalArgumentException("Item Qty Field Required in Row#" + rowNo);
            if (l.packUomId <= 0) throw new IllegalArgumentException("PackUom Field Required in Row#" + rowNo);
            if (!(l.grossWeight > 0.0)) throw new IllegalArgumentException("GrossWight Field Required in Row#" + rowNo);
            grossWt = grossWt.add(dec(l.grossWeight));
            if (!(l.netWeight > 0.0)) throw new IllegalArgumentException("NetWeight Field Required in Row#" + rowNo);
            if (!(l.stockWeight > 0.0)) throw new IllegalArgumentException("StockWeight Field Required in Row#" + rowNo);
            if (l.warehouseId <= 0) throw new IllegalArgumentException("WareHouse Field Required in Row#" + rowNo);
            if (l.cityId <= 0) throw new IllegalArgumentException("City Field Required in Row#" + rowNo);
            if (r.id > 0 && l.id > 0 && !savedIds.contains(l.id)) throw new IllegalArgumentException("Invalid detail row for this goods dispatch note");
            details.add(detail(l, r, gateSale, l.id <= 0 ? 1 : 2));
        }

        boolean isNew = r.id <= 0;
        if ("DeliveryOrder".equals(text(r.deliveryTerm))) {
            int num = toInt(r.partyCount);
            BigDecimal finalWt = decText(r.factoryWeight).subtract(decText(r.gdnGrossWeight));
            if (isNew) {
                if (num == 1 && grossWt.compareTo(finalWt) > 0) throw new IllegalArgumentException("TotalGross Weight in Detail Grid cannot be greater than Balance Weight!");
                if (num > 1 && grossWt.compareTo(finalWt) >= 0) throw new IllegalArgumentException("Total Gross Weight in Detail Grid cannot be greater nor be equal to Balance Weight because there are more parties yet!");
            } else if (num == 1 && grossWt.compareTo(finalWt) > 0) {
                throw new IllegalArgumentException("TotalGross Weight in Detail Grid cannot be greater than Balance Weight!");
            }
        }
        if (!"DeliveryOrder".equals(text(r.orderType))) {
            BigDecimal factoryWeight = decText(r.factoryWeight);
            if (grossWt.compareTo(factoryWeight) != 0)
                throw new IllegalArgumentException("TotalGrossWeight " + factoryWeight.toPlainString() + " And Gross Weight " + plain(grossWt) + " in Detail Grid is not Equal!");
        }

        List<Line> removed = r.removed == null ? List.of() : r.removed;
        List<Map<String, Object>> removedDetails = new ArrayList<>();
        if (r.id > 0) for (Line l : removed) {
            if (l.id <= 0 || !savedIds.contains(l.id)) throw new IllegalArgumentException("Deleted detail row does not belong to this goods dispatch note");
            removedDetails.add(detail(l, r, gateSale, 3));
        }

        Timestamp now = now();
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", r.id);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DocDate", ts(r.docDate));
        h.put("DocNo", toInt(r.docNo));
        h.put("SupplierCustomerId", r.customerId);
        h.put("GpNo", toInt(r.gatePassNo));
        h.put("ReferencePartyId", r.refPartyId);
        h.put("PartyWeight", decText(r.partyWeight));
        h.put("FactoryWeight", decText(r.factoryWeight));
        h.put("VehicleTypeId", r.vehicleTypeId);
        h.put("VehicleNo", text(r.vehicleNo));
        h.put("BiltyNo", text(r.biltyNo));
        h.put("TransporterId", r.transporterId);
        h.put("CarriageAmount", decText(r.carriageAmount));
        h.put("RemarksHeader", text(r.remarks));
        h.put("IsApproved", false);
        h.put("EntryDate", now);
        h.put("EntryUserId", u.getId());
        h.put("ModifyDate", now);
        h.put("ModifyUserId", u.getId());
        h.put("ApprovedDate", now);
        h.put("ApprovedUserId", u.getId());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchesId", u.getBranchesId());
        h.put("ProjectsId", 0);
        h.put("OutwardGatePassId", r.gpId);
        h.put("ActionId", r.id > 0 ? 2 : 1);
        h.put("FinancialYearId", sup.fy());
        h.put("DeliveryTerm", text(r.deliveryTerm));
        String hp = r.id > 0 ? "[ST].[USP_InvGdn_Update]" : "[ST].[USP_InvGdn_Insert]";
        int num = sup.setProcMap(hp, steel.full(hp, h));
        int id = num > 0 ? num : r.id;
        if (id <= 0) throw new IllegalStateException("The goods dispatch note could not be saved.");

        for (Map<String, Object> d : removedDetails) { d.put("InvGdnId", id); sup.setProcMap("[ST].[USP_InvGdnDetail_Insert]", steel.full("[ST].[USP_InvGdnDetail_Insert]", d)); }
        for (Map<String, Object> d : details) { d.put("InvGdnId", id); sup.setProcMap("[ST].[USP_InvGdnDetail_Insert]", steel.full("[ST].[USP_InvGdnDetail_Insert]", d)); }
        attachments.apply(SCREEN, DOC_TYPE, id, r.customerId, r.attachments);
        // DAL.InvGdn.SetData: the InventoryTransactions model through Sp_InventoryTransactions_GetALLMethod
        sup.setProcMap("Sp_InventoryTransactions_GetALLMethod", steel.full("Sp_InventoryTransactions_GetALLMethod",
                row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "RefDocumentTypeId", DOC_TYPE, "RefDocIdNo", id)));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", toInt(r.docNo));
        out.put("updated", r.id > 0);
        out.put("grossWeight", grossWt);
        out.put("wagesStatus", toBool(sup.config("ContractorWagesCompulsoryBeforeInvoices")));
        out.put("message", (r.id > 0 ? "Record Update Successfully " : "Record Save Successfully ") + toInt(r.docNo));
        return out;
    }

    /** GenericProvider.SetProc(InvGdnDetail): the procedure's 31 parameters (InvGdnId is set once the header is saved). */
    private Map<String, Object> detail(Line l, Request r, boolean gateSale, int action) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", action == 1 ? 0 : l.id);
        if (action == 3 || !gateSale) {
            d.put("SaleOrderId", l.saleOrderId);
            d.put("SaleOrderDetailId", l.saleOrderDetailId);
            d.put("SaleOrderNo", l.saleOrderNo);
        }
        d.put("ReferencePartyId", l.refPartyId > 0 ? l.refPartyId : r.refPartyId);
        d.put("ItemId", l.itemId);
        d.put("JobLotId", l.jobLotId);
        d.put("PackingTypeId", l.packingTypeId);
        d.put("ItemQty", dec(l.itemQty));
        d.put("ItemUomId", l.packUomId);
        d.put("GrossWeight", dec(l.grossWeight));
        d.put("WtCut", dec(l.wtCut));
        d.put("WtCutTotal", dec(l.wtCutTotal));
        d.put("AdLsWeight", dec(l.addLess));
        d.put("NetBillWeight", dec(l.netWeight));
        d.put("StockWeight", dec(l.stockWeight));
        d.put("WarehouseId", l.warehouseId);
        d.put("CityId", l.cityId);
        d.put("LabReportRef", text(l.labNo));
        d.put("RemarksDetail", text(l.remarks));
        d.put("GpDate", now());
        d.put("VehicleNo", action == 3 ? text(l.vehicleNo) : text(r.vehicleNo));
        d.put("ActionTypeId", action);
        return d;
    }

    // ------------------------------------------------------------------ Delete (btnDelete_Click :2961)

    @Transactional
    public Map<String, Object> delete(int id) {
        if (id <= 0) throw new IllegalArgumentException("Record Not Found");
        if (!Boolean.TRUE.equals(sup.rights(SCREEN).get("delete"))) throw new ResponseStatusException(HttpStatus.FORBIDDEN, "You do not have the delete right on this screen");
        header(id);
        sup.rows(P_GDN, "Id", id, "EntryUserId", sup.user().getId(), "Activity", "DeleteById");
        return row("message", "Record Deleted Seccessfully");
    }

    // ------------------------------------------------------------------ attachments

    public List<Map<String, Object>> attachmentList(int id) { header(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { header(id); return attachments.download(SCREEN, id, attachmentId); }

    private static Timestamp ts(String s) {
        if (s == null || s.isBlank()) return null;
        try { return Timestamp.valueOf(LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))).atStartOfDay()); } catch (RuntimeException e) { return null; }
    }
}
