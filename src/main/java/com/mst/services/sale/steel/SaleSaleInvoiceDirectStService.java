package com.mst.services.sale.steel;

import com.mst.models.UserAccount;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.repositories.support.DesktopProc;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleEngrSupport;
import com.mst.services.sale.steel.SaleStDirectCalc.Ex;
import com.mst.services.sale.steel.SaleStDirectCalc.Fr;
import com.mst.services.sale.steel.SaleStDirectCalc.Inv;
import com.mst.services.sale.steel.SaleStDirectCalc.Jv;
import com.mst.services.sale.steel.SaleStDirectCalc.Line;
import com.mst.services.sale.steel.SaleStInvoiceCalc.Cfg;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.dec;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.decText;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.fmt;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.num;

/**
 * Screen 544 "SaleInvoiceDirect_St" = Architecture.WinApp.Steel.Sale.SaleInvoiceDirect_St (Steel Sale Invoice Direct, document type 1510).
 *
 * Desktop map (SaleInvoiceDirect_St.cs): FormValidationDetila :560, FormValidation :487, InvfrmPurchasedirectInvoice_Load :639, HistoryComboFill :774, defaultConfiquration :825,
 * Branches :859, Project :877, DocumentNo :895, suppliercustomer :925, PaymentTerms :944, DeliveryTerm :962, CurrencyFill :981, CommissionTypeFill :1006, CommissionUOMFill :1026,
 * itemsFill :1047, JobLotFill :1064, PackingType :1080, AccountsFill :1097, OtherItemsBind :1109, PackUOM :1126, bindWareHouse :1174, CityFill :1190, VoucherHeadIdGet :1215,
 * AvailableStockGetByItem :1242, grdSettings :1313, Insert :1423, Reset :1768, ResetDetail :1840, BillAmount :1869, grdFreightSettings :1929, grdInvExpSettings :1973,
 * gridGLSettings :2009, ExpProportion :2192, BillProportion :2214, CommissionProportion :2243, FreightProportion :2275, btnAdd_Click :2308, ReadById :2337, Total :2439,
 * TotalWeight :2501, AmountCaluculation :2531, GetAll :2678, HistoryGridSettings :2791, TotalCommissionAmount :2876, grd_DoubleClick :3113, btnUpdateDetail_Click :3167,
 * grdHistory_ColumnButtonClick :3354, GetDetailGrdByHeadId :3379, DetailGridSetting :3435, CalculateTotalInformation :3850, grd_ColumnButtonClick :3975,
 * LoadInGridDetail :4094, btnLoadSaleOrder_Click :4237; SaleOrderLoad_St.cs: PendingSaleOrderLoad :55, btnLoadOnInvoice_Click_1 :215.
 *
 * Procedures: [ST].[USP_SaleInvoice_GetAllMethod] (GenerateCode, ReadById, ReadByHeaderId, InvSaleInvoiceExpense_/Freight_/Journal_ReadBySaleInvoiceID),
 * [ST].[USP_SaleInvoice_FormHistory], [ST].[USP_GetDataForDropDownFromSaleInvoice], [ST].[USP_PendingSaleOrderDetailForDeliveryOrder],
 * [ST].[USP_GetDataForDropDownFromSaleOrder], Sp_SaleOrder_GetAllMethod (GetCurrentStockByItemId), SP_City_GetAllMethod, and the save chain of SaleStInvoicePersist
 * (document type 1510: ...+ Sp_InventoryTransactions_GetALLMethod).
 */
@Service
public class SaleSaleInvoiceDirectStService {
    public static final String SCREEN = "SaleInvoiceDirect_St";
    public static final int DOC_TYPE = 1510;
    private static final String P_INV = "[ST].[USP_SaleInvoice_GetAllMethod]";

    private final SaleEngrSupport sup;
    private final SaleSteelSupport steel;
    private final SaleInvoiceRepository repo;
    private final SaleEngrAttachments attachments;
    private final SaleStInvoiceFinancial financial;
    private final SaleStInvoicePersist persist;
    private final SaleSteelLookups look;
    private final SaleSaleInvoiceStService base;

    public SaleSaleInvoiceDirectStService(SaleEngrSupport sup, SaleSteelSupport steel, SaleInvoiceRepository repo, SaleEngrAttachments attachments,
                                          SaleStInvoiceFinancial financial, SaleStInvoicePersist persist, SaleSteelLookups look, SaleSaleInvoiceStService base) {
        this.sup = sup; this.steel = steel; this.repo = repo; this.attachments = attachments; this.financial = financial; this.persist = persist; this.look = look; this.base = base;
    }

    // ------------------------------------------------------------------ lookups

    public List<Map<String, Object>> deliveryTerms() { return List.of(row("Id", 1, "DeliveryTerm", "Load"), row("Id", 2, "DeliveryTerm", "Ponch")); }

    /** CommissionTypeFill :1006 */
    public List<Map<String, Object>> commissionTypes() {
        return List.of(row("Id", 1, "CommissionType", "Flat"), row("Id", 2, "CommissionType", "Percent"), row("Id", 3, "CommissionType", "By Weight"));
    }

    /** CommissionUOMFill :1026 */
    public List<Map<String, Object>> commissionUoms() {
        return List.of(row("Id", 1, "UOM", "40"), row("Id", 2, "UOM", "50"), row("Id", 3, "UOM", "60"), row("Id", 4, "UOM", "100"));
    }

    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String c : cols) o.put(c, ci(r, c));
            out.add(o);
        }
        return out;
    }

    public List<Map<String, Object>> items() { return pick(look.items(), "Id", "ItemName"); }
    public List<Map<String, Object>> jobLots() { return pick(look.jobLots(), "Id", "JobLotDescription"); }
    public List<Map<String, Object>> packingTypes() { return pick(look.packingTypes(), "Id", "PackTypeDesc"); }
    public List<Map<String, Object>> warehouses() { return pick(look.warehouses(), "Id", "WareHouseName"); }

    /** CityFill :1190 - City.GetAll. */
    public List<Map<String, Object>> cities() {
        return pick(sup.rows("SP_City_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll"), "Id", "CityName");
    }

    /** PackUOM :1126 - CommonServices.GetUomScheduleByItemId (Id, UOMCode, Equivalent). */
    public List<Map<String, Object>> uoms(int itemId) { return look.uoms(itemId); }

    /** AvailableStockGetByItem :1242 */
    public Map<String, Object> stock(int warehouseId, int itemId, int jobLotId, int packingTypeId, int uomId, String docDate) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("ItemId", itemId);
        Timestamp d = ts(docDate);
        if (d != null) p.put("DocDateTo", d);
        p.put("WareHouseId", warehouseId); p.put("JobLotId", jobLotId); p.put("InvPackingTypeId", packingTypeId); p.put("ItemUomId", uomId);
        p.put("Activity", "GetCurrentStockByItemId");
        List<Map<String, Object>> r = DesktopProc.rows(sup.jdbc(), "Sp_SaleOrder_GetAllMethod", p);
        double cur = r.isEmpty() ? 0.0 : toDouble(ci(r.get(0), "AvailableStock"));
        return row("balance", cur > 0.0 ? num(cur) : "0");
    }

    /** defaultConfiquration :825 - Base Currency, BaseCurrencyRate, Job/Lot, City Area. */
    public Map<String, Object> defaults() {
        Map<String, Object> m = new LinkedHashMap<>();
        String bc = sup.config("Base Currency");
        if (!bc.isEmpty()) m.put("currencyId", toInt(bc));
        String br = sup.config("BaseCurrencyRate");
        if (!br.isEmpty()) m.put("exchangeRate", decText(br).toPlainString());
        String jl = sup.config("Job/Lot");
        if (!jl.isEmpty()) m.put("jobLotId", toInt(jl));
        String ct = sup.config("City Area");
        if (!ct.isEmpty()) m.put("cityId", toInt(ct));
        return m;
    }

    /** DocumentNo :895 - InvSaleInvoice.GenerateInvSaleInvoiceCode (document type 1510). */
    public int nextNo() {
        UserAccount u = sup.user();
        List<Map<String, Object>> r = DesktopProc.rows(sup.jdbc(), P_INV, DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", u.getBranchesId(),
                "DocumentTypeId", DOC_TYPE, "FinancialYearId", sup.fy(), "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    public Map<String, Object> historyCombos() { return base.historyCombos(); }

    /** InvfrmPurchasedirectInvoice_Load :639 */
    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("branches", base.branches());
        m.put("projects", base.projects());
        m.put("customers", base.customers());
        m.put("paymentTerms", base.paymentTerms());
        m.put("deliveryTerms", deliveryTerms());
        m.put("currencies", base.currencies());
        m.put("accounts", base.accounts());
        m.put("otherItems", base.otherItems());
        m.put("commTypes", commissionTypes());
        m.put("commUoms", commissionUoms());
        m.put("items", items());
        m.put("jobLots", jobLots());
        m.put("packingTypes", packingTypes());
        m.put("warehouses", warehouses());
        m.put("cities", cities());
        m.put("defaults", defaults());
        m.put("formats", base.formats());
        m.put("nextNo", nextNo());
        m.put("history", historyCombos());
        int days = toInt(sup.config("DefaultDaysToLessFromHistoryFromDate"));
        m.put("historyDays", days > 0 ? days : 3);
        return m;
    }

    /** btnRefresh_Click :3275 - OtherItemsBind, AccountsFill, Branches, Project, suppliercustomer, PaymentTerms, DeliveryTerm, CurrencyFill, CommissionTypeFill, CommissionUOMFill, itemsFill, JobLotFill, PackingType, GridComboBind, bindWareHouse. */
    public Map<String, Object> refresh() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("otherItems", base.otherItems());
        m.put("accounts", base.accounts());
        m.put("branches", base.branches());
        m.put("projects", base.projects());
        m.put("customers", base.customers());
        m.put("paymentTerms", base.paymentTerms());
        m.put("deliveryTerms", deliveryTerms());
        m.put("currencies", base.currencies());
        m.put("commTypes", commissionTypes());
        m.put("commUoms", commissionUoms());
        m.put("items", items());
        m.put("jobLots", jobLots());
        m.put("packingTypes", packingTypes());
        m.put("warehouses", warehouses());
        return m;
    }

    private int glOf(int supplierId) {
        if (supplierId <= 0) return 0;
        List<Map<String, Object>> r = sup.jdbc().queryForList("SELECT GlAccountId FROM SupplierCustomer WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?", supplierId, sup.org(), sup.company());
        return r.isEmpty() ? 0 : toInt(r.get(0).get("GlAccountId"));
    }

    private SaleStDirectCalc.Uoms uomsFor() {
        Map<Integer, Map<Integer, Double>> cache = new HashMap<>();
        return (item, uom) -> cache.computeIfAbsent(item, k -> {
            Map<Integer, Double> m = new HashMap<>();
            for (Map<String, Object> r : look.uoms(k)) m.put(toInt(r.get("Id")), toDouble(r.get("Equivalent")));
            return m;
        }).get(uom);
    }

    // ------------------------------------------------------------------ calculations (the grid / text box events)

    public static class CalcRequest {
        public String event, key, col;
        public int row = -1;
        public Inv state;
    }

    /** One desktop event on the form state; answers the new state and the MessageBox texts raised on the way. */
    public Inv calc(CalcRequest rq) {
        Inv v = rq.state == null ? new Inv() : rq.state;
        Cfg c = base.cfg();
        SaleStDirectCalc k = new SaleStDirectCalc(c, v, uomsFor());
        v.supplierGlId = glOf(v.supplierId);
        String e = rq.event == null ? "" : rq.event;
        switch (e) {
            case "qty", "gross", "wtcut", "addless", "remarks", "rate", "ratecut", "rateuom", "packuom" -> k.entryEvent(e);
            case "comm" -> k.commissionChanged();
            case "supplier" -> k.supplierChanged();
            case "exch" -> k.exchangeChanged();
            case "due" -> k.dueDaysChanged();
            case "delStart" -> k.deliveryStartChanged();
            case "delDays" -> k.deliveryDaysChanged();
            case "delTerm" -> k.deliveryTermChanged();
            case "grid" -> k.gridButton(rq.key, rq.row);
            case "freightCell" -> k.freightCell(rq.row, rq.col);
            case "freightBtn" -> k.freightButton(rq.key, rq.row);
            case "expCell" -> k.expCell(rq.row, rq.col);
            case "expBtn" -> k.expButton(rq.key, rq.row);
            case "glCell" -> k.glCell(rq.row, rq.col);
            case "glBtn" -> k.glButton(rq.key, rq.row);
            case "add" -> k.addDetail();
            case "update" -> k.updateDetail();
            case "edit" -> k.editRow(rq.row);
            case "cancelUpdate" -> v.updateIndex = -1;
            case "reset" -> reset(k, v);
            default -> throw new IllegalArgumentException("Unknown event " + e);
        }
        return v;
    }

    /** Reset :1768 followed by DocumentNo, btnSave.Visible, defaultConfiquration. */
    private void reset(SaleStDirectCalc k, Inv v) {
        k.resetForm();
        v.docNo = "";
        int n = nextNo();
        if (n > 0) v.docNo = String.valueOf(n); else v.messages.add("Max Number Not Found");
        v.saveMode = true;
        Map<String, Object> d = defaults();
        if (d.get("currencyId") != null) v.currencyId = toInt(d.get("currencyId"));
        if (d.get("exchangeRate") != null) k.setExchangeRate(String.valueOf(d.get("exchangeRate")));
        if (d.get("jobLotId") != null) {
            int id = toInt(d.get("jobLotId"));
            for (Map<String, Object> r : jobLots()) if (toInt(r.get("Id")) == id) { v.e.jobLotId = id; v.e.jobLotText = str(r.get("JobLotDescription")); }
        }
        if (d.get("cityId") != null) {
            int id = toInt(d.get("cityId"));
            for (Map<String, Object> r : cities()) if (toInt(r.get("Id")) == id) { v.e.cityId = id; v.e.cityText = str(r.get("CityName")); }
        }
    }

    // ------------------------------------------------------------------ Load Sale Order (SaleOrderLoad_St)

    /** ComboBinds - SaleOrder.GetDataForDropDownFromSaleOrder (Customer / Item rows). */
    public Map<String, Object> loaderCombos() {
        List<Map<String, Object>> cust = new ArrayList<>(), item = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("[ST].[USP_GetDataForDropDownFromSaleOrder]", "OrganizationId", sup.org(), "CompanyId", sup.company())) {
            String a = str(ci(r, "Activity"));
            Map<String, Object> o = row("Id", ci(r, "Id"), "Name", ci(r, "ReferenceName"));
            if ("Customer".equals(a)) cust.add(o); else if ("Item".equals(a)) item.add(o);
        }
        return row("customers", cust, "items", item, "fromDate", dateText(fyStart()));
    }

    /** FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period. */
    public String fyStart() {
        for (Map<String, Object> r : sup.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            if (toInt(r.get("Id")) == sup.fy()) return str(r.get("Start_Period"));
        return "";
    }

    /** SaleOrder.PendingSaleOrderDetailForDeliveryOrderForSteel: the BLL sends only the organization and the company (the filters of the dialog are not passed). */
    private List<Map<String, Object>> pendingRows() {
        return sup.rows("[ST].[USP_PendingSaleOrderDetailForDeliveryOrder]", "OrganizationId", sup.org(), "CompanyId", sup.company());
    }

    /** PendingSaleOrderLoad :55 - the 14 columns of dtPending. */
    public List<Map<String, Object>> loaderPending() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : pendingRows())
            out.add(row("OrderType", ci(r, "DocumentType"), "OrderId", ci(r, "Id"), "OrderNo", ci(r, "DocNo"), "OrderDetailId", ci(r, "OrderDetailId"), "OrderSupCustId", ci(r, "OrderSupCustId"),
                    "PartyName", ci(r, "CustomerName"), "ItemId", ci(r, "ItemId"), "ItemName", ci(r, "ItemName"), "ItemUomId", ci(r, "ItemUOMId"), "PackUOM", ci(r, "PackUom"),
                    "OrderQTY", toDouble(ci(r, "OrderQty")), "OrderWeight", toDouble(ci(r, "OrderWeight")), "DispatchedWeight", toDouble(ci(r, "DispatchWeight")), "BalWeight", toDouble(ci(r, "BalWeight"))));
        return out;
    }

    public static class LoadRequest {
        public List<Integer> orderDetailIds = new ArrayList<>();
        public Inv state;
    }

    private static String date10(Object o) {
        if (o == null) return "";
        String s = str(o);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    private static String dateText(Object o) { return date10(o); }

    /** btnLoadSaleOrder_Click :4237 + btnLoadOnInvoice_Click_1 + LoadInGridDetail :4094 */
    public Inv load(LoadRequest rq) {
        Inv v = rq.state == null ? new Inv() : rq.state;
        Cfg c = base.cfg();
        SaleStDirectCalc k = new SaleStDirectCalc(c, v, uomsFor());
        if (!v.lines.isEmpty() && v.lines.get(0).OrderId <= 0) throw new Warning("You Can't Load Sale Order Because Direct Entry Already Exist");
        if (rq.orderDetailIds == null || rq.orderDetailIds.isEmpty()) return v;
        Map<Integer, Map<String, Object>> byDetail = new LinkedHashMap<>();
        for (Map<String, Object> r : pendingRows()) byDetail.put(toInt(ci(r, "OrderDetailId")), r);
        List<Map<String, Object>> rows = new ArrayList<>();
        int orderId = 0;
        for (Integer did : rq.orderDetailIds) {
            Map<String, Object> r = byDetail.get(did);
            if (r == null) throw new IllegalArgumentException("Sale order row not found or no longer pending");
            int id = toInt(ci(r, "Id"));
            if (orderId == 0) orderId = id;
            if (orderId != id) throw new Warning("Sorry! Select Same OrderNo Rows");
            rows.add(r);
        }
        Map<String, Object> f = rows.get(0);
        if (!v.lines.isEmpty() && v.lines.get(0).OrderId != toInt(ci(f, "Id"))) throw new IllegalArgumentException("You Can't Load Different Order At Same Time");

        v.supplierId = toInt(ci(f, "OrderSupCustId"));
        v.supplierGlId = glOf(v.supplierId);
        v.refNo = str(ci(f, "PartyReference"));
        v.deliveryTerm = str(ci(f, "DeliveryTerm"));
        v.transporterId = 0;
        if (!"Ponch".equals(v.deliveryTerm)) v.freightText = "0";
        String ds = date10(ci(f, "DeliveryStartDate"));
        v.deliveryStart = ds.isEmpty() ? v.docDate : ds;
        v.deliveryDays = str(ci(f, "DeliveryDays"));
        v.paymentTermId = toInt(ci(f, "PaymentTermsId"));
        v.dueDays = str(ci(f, "OrderDueDays"));
        v.dueDate = SaleStInvoiceCalc.dueDate(v.dueDays);
        v.currencyId = toInt(ci(f, "CurrencyId"));
        if (toInt(ci(f, "CommissionAgentId")) > 0) v.commAgentId = toInt(ci(f, "CommissionAgentId"));
        if (!str(ci(f, "CommissionType")).isEmpty()) v.commType = str(ci(f, "CommissionType"));
        if (!str(ci(f, "CommRateUom")).isEmpty()) v.commUom = str(ci(f, "CommRateUom"));
        v.commRate = fmt(toDouble(ci(f, "CommRate")), c.rateDec);
        v.commAmount = fmt(toDouble(ci(f, "CommAmount")), c.amtDec);
        v.commRemarks = str(ci(f, "CommRemarks"));
        k.deliveryStartChanged();
        k.deliveryDaysChanged();

        boolean flag = false;
        for (Map<String, Object> r : rows) {
            if (!v.lines.isEmpty()) {
                for (Line l : v.lines) if (l.OrderDetailId == toInt(ci(r, "OrderDetailId"))) { flag = true; break; }
            }
            if (!flag) {
                Line l = new Line();
                l.OrderId = toInt(ci(r, "Id")); l.OrderDetailId = toInt(ci(r, "OrderDetailId")); l.OrderNo = toInt(ci(r, "DocNo"));
                l.ItemId = toInt(ci(r, "ItemId")); l.ItemName = str(ci(r, "ItemName")); l.JobLotId = toInt(ci(r, "JobLotId")); l.JobLot = str(ci(r, "JobLotDescription"));
                l.PackingTypeId = toInt(ci(r, "PackingTypeId")); l.PackingType = str(ci(r, "PackTypeDesc")); l.PackUomId = toInt(ci(r, "ItemUOMId")); l.PackUom = str(ci(r, "PackUom"));
                l.ItemQty = toDouble(ci(r, "BalQty")); l.GrossWeight = toDouble(ci(r, "BalWeight")); l.NetBillWeight = toDouble(ci(r, "BalWeight")); l.StockWeight = toDouble(ci(r, "BalWeight"));
                l.ItemRate = toDouble(ci(r, "OrderItemRate")); l.RateUomId = toInt(ci(r, "OrderItemRateUOMId")); l.RateUom = str(ci(r, "RateUom"));
                l.ItemAmount = toDouble(ci(r, "ItemAmount"));
                l.CityId = toInt(ci(r, "CityId")); l.CityName = str(ci(r, "CityName"));
                l.GpDate = LocalDate.now().toString();
                v.lines.add(l);
            }
            flag = false;
        }
        v.exchangeRate = fmt(toDouble(ci(f, "ExchangeRate")), c.rateDec);
        k.afterLoad();
        return v;
    }

    // ------------------------------------------------------------------ record (ReadById :2337)

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> r = sup.rows(P_INV, "Id", id, "Activity", "ReadById");
        UserAccount u = sup.user();
        if (r.isEmpty() || toInt(ci(r.get(0), "OrganizationId")) != u.getOrganizationId() || toInt(ci(r.get(0), "CompanyId")) != u.getCompanyId()
                || toInt(ci(r.get(0), "DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Sale invoice not found in this company");
        return r.get(0);
    }

    /** Conversion.ToString(decimal): the scale of the column is kept. */
    private static String dts(Object o) {
        if (o == null) return "";
        if (o instanceof BigDecimal b) return b.toPlainString();
        return String.valueOf(o);
    }

    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        Cfg c = base.cfg();
        Inv v = new Inv();
        v.id = id;
        v.saveMode = false;
        v.branchId = toInt(ci(h, "BranchesId")); v.projectId = toInt(ci(h, "ProjectsId"));
        v.docNo = dts(ci(h, "DocNo")); v.docDate = date10(ci(h, "DocDate"));
        v.supplierId = toInt(ci(h, "SupplierCustomerId")); v.supplierGlId = glOf(v.supplierId);
        v.refNo = dts(ci(h, "SupplierReferenceNo")); v.billNo = str(ci(h, "ManualBillNo"));
        v.deliveryTerm = str(ci(h, "DeliveryTerm"));
        v.deliveryStart = date10(ci(h, "DeliverystartDate"));
        v.deliveryDays = dts(ci(h, "DeliveryDays"));
        v.paymentTermId = toInt(ci(h, "PaymentTermsId"));
        v.dueDays = dts(ci(h, "DueDays")); v.dueDate = date10(ci(h, "DueDate"));
        v.currencyId = toInt(ci(h, "CurrencyId"));
        v.exchangeRate = dts(ci(h, "ExchangeRate"));
        v.totalQty = dts(ci(h, "TotalQty")); v.totalWeight = dts(ci(h, "TotalWeight")); v.fcyAmount = dts(ci(h, "FcyAmount"));
        v.billAmount = dts(ci(h, "BillAmount"));
        v.transporterId = toInt(ci(h, "TransporterId"));
        v.freightText = dts(ci(h, "FreightAmount"));
        v.commAgentId = toInt(ci(h, "CommAgentId"));
        v.commType = str(ci(h, "CommType"));
        v.commRate = dts(ci(h, "CommRate"));
        v.commUom = dts(ci(h, "CommUOM"));
        v.commAmount = dts(ci(h, "CommAmount"));
        v.commRemarks = str(ci(h, "CommRemarks"));
        v.remarks = str(ci(h, "RemarksHeader"));
        for (Map<String, Object> d : sup.rows(P_INV, "Id", id, "Activity", "ReadByHeaderId")) {
            Line l = new Line();
            l.Id = toInt(ci(d, "Id")); l.OrderId = toInt(ci(d, "SaleOrderId")); l.OrderDetailId = toInt(ci(d, "SaleOrderDetailId")); l.OrderNo = toInt(ci(d, "SaleOrder"));
            l.ItemId = toInt(ci(d, "ItemId")); l.ItemName = str(ci(d, "ItemName"));
            l.JobLotId = toInt(ci(d, "JobLotId")); l.JobLot = str(ci(d, "JobLotDescription"));
            l.PackingTypeId = toInt(ci(d, "PackingTypeId")); l.PackingType = str(ci(d, "PackingType"));
            l.PackUomId = toInt(ci(d, "PackUOMId")); l.PackUom = str(ci(d, "UOMCode"));
            l.ItemQty = toDouble(ci(d, "ItemQty")); l.GrossWeight = toDouble(ci(d, "GrossWeight")); l.WeightCut = toDouble(ci(d, "WeightCut")); l.WeightCutTotal = toDouble(ci(d, "WeightCutTotal"));
            l.AddLessWeight = toDouble(ci(d, "AdLsWeight")); l.NetBillWeight = toDouble(ci(d, "NetBillWeight")); l.StockWeight = toDouble(ci(d, "NetStockWeight"));
            l.ItemRate = toDouble(ci(d, "ItemRate")); l.RateUomId = toInt(ci(d, "RateUomScheduleId")); l.RateUom = str(ci(d, "RateUom"));
            l.RateCut = toDouble(ci(d, "RateCut")); l.RateCutTotal = toDouble(ci(d, "RateCutAmount")); l.ItemAmount = toDouble(ci(d, "ItemAmount"));
            l.WarehouseId = toInt(ci(d, "WarehouseId")); l.Warehouse = str(ci(d, "WareHouseName"));
            l.CityId = toInt(ci(d, "CityId")); l.CityName = str(ci(d, "CityName"));
            l.GpDate = date10(ci(d, "GpDate")); l.GpNo = dts(ci(d, "GpNo")); l.VehicleNo = str(ci(d, "VehicleNo"));
            l.ExchangeRate = toDouble(ci(d, "ExchangeRate")); l.FcyAmount = toDouble(ci(d, "FcyAmount")); l.BillAmount = toDouble(ci(d, "ItemNetAmount"));
            l.Expense = toDouble(ci(d, "ExpenseAmount")); l.Journal = toDouble(ci(d, "JournalAmount")); l.Commission = toDouble(ci(d, "CommissionAmount")); l.Freight = toDouble(ci(d, "FreightAmount"));
            l.Remarks = str(ci(d, "RemarksDetail"));
            v.lines.add(l);
        }
        for (Map<String, Object> e : sup.rows(P_INV, "Id", id, "Activity", "InvSaleInvoiceFreight_ReadBySaleInvoiceID")) {
            Fr x = new Fr(); x.TransporterId = toInt(ci(e, "TansporterId")); x.Percentage = toDouble(ci(e, "Percentage")); x.Qty = toDouble(ci(e, "FrQty")); x.Rate = toDouble(ci(e, "FrRate"));
            x.Freight = SaleStInvoiceCalc.rnd(toDouble(ci(e, "CreditAmount")), c.amtRound); x.Remarks = str(ci(e, "Remarks"));
            v.freight.add(x);
        }
        for (Map<String, Object> e : sup.rows(P_INV, "Id", id, "Activity", "InvSaleInvoiceExpense_ReadBySaleInvoiceID")) {
            Ex x = new Ex(); x.ItemId = toInt(ci(e, "InvOtherItemId")); x.Qty = toDouble(ci(e, "Qty")); x.Rate = toDouble(ci(e, "Rate")); x.Amount = toDouble(ci(e, "Amount")); x.Remarks = str(ci(e, "Remarks"));
            v.exp.add(x);
        }
        for (Map<String, Object> e : sup.rows(P_INV, "Id", id, "Activity", "InvSaleInvoiceJournal_ReadBySaleInvoiceID")) {
            Jv x = new Jv(); x.AccountId = toInt(ci(e, "ChartofAccountId")); x.Remarks = str(ci(e, "JvRemarks")); x.Percentage = toDouble(ci(e, "JvPrcnt"));
            x.Qty = toDouble(ci(e, "JvQty")); x.Rate = toDouble(ci(e, "JvRate")); x.Debit = toDouble(ci(e, "JvDebit")); x.Credit = toDouble(ci(e, "JvCredit"));
            v.gl.add(x);
        }
        SaleStDirectCalc k = new SaleStDirectCalc(c, v, uomsFor());
        if (v.freight.isEmpty()) k.addFreightRow();
        if (v.exp.isEmpty()) k.addExpRow();
        if (v.gl.isEmpty()) k.addGlRow();
        k.afterRead();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("state", v);
        out.put("approved", toBool(ci(h, "IsApproved")));
        out.put("voucherHeadId", repo.voucherHeadId(sup.org(), sup.company(), DOC_TYPE, id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    // ------------------------------------------------------------------ History (GetAll :2678)

    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate, int fromNo, int toNo, int customerId, int paymentTermId, String deliveryTerm) {
        boolean viewAll = Boolean.TRUE.equals(sup.rights(SCREEN).get("viewAll"));
        UserAccount u = sup.user();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId()); p.put("BranchesId", u.getBranchesId());
        p.put("DocumentTypeId", DOC_TYPE); p.put("FinancialYearId", sup.fy()); p.put("CanViewAllRecord", viewAll);
        if (!viewAll) p.put("EntryUserId", u.getId());                    // the procedure's parameter is @EntryUserId (the BLL names it @EntryUser, which the procedure does not have)
        String fk = "FromDate", tk = "ToDate";
        if ("entry".equals(dateType)) { fk = "EntryFromDate"; tk = "EntryToDate"; }
        else if ("modify".equals(dateType)) { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
        else if ("approved".equals(dateType)) { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
        Timestamp f = ts(fromDate), t = ts(toDate);
        if (f != null) p.put(fk, f);
        if (t != null) p.put(tk, t);
        if (fromNo != 0) p.put("FromDocNo", fromNo);
        if (toNo != 0) p.put("ToDocNo", toNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (paymentTermId != 0) p.put("PaymentTermsId", paymentTermId);
        if (deliveryTerm != null && !deliveryTerm.isEmpty()) p.put("DeliveryTerm", deliveryTerm);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : steel.table("[ST].[USP_SaleInvoice_FormHistory]", p).rows) {
            out.add(row("Id", r.get("Id"), "VoucherHeadId", r.get("VoucherHeadId"), "DocNo", r.get("DocNo"), "DocDate", date10(r.get("DocDate")), "CustomerName", r.get("SupplierName"),
                    "DeliveryTerm", r.get("DeliveryTerm"), "DelievryDays", r.get("DeliveryDays"), "CommAgent", r.get("CommissionAgent"), "CommType", r.get("CommType"), "CommRate", r.get("CommRate"),
                    "CommAmount", toDouble(r.get("CommAmount")), "CommRemarks", r.get("CommRemarks"), "PaymentTerm", r.get("TermsDescription"), "DueDays", r.get("DueDays"),
                    "DueDate", date10(r.get("DueDate")), "ExpiryDate", date10(r.get("ExpiryDate")), "TotalQty", r.get("TotalQty"), "TotalWeight", r.get("TotalWeight"),
                    "BillAmount", r.get("BillAmount"), "ExchangeRate", r.get("ExchangeRate"), "FcyAmount", r.get("FcyAmount"), "FcyCode", r.get("CurrencyCode"),
                    "EntryUser", r.get("UserName"), "NoOfAttachments", r.get("NoOfAttachments"), "Remarks", r.get("RemarksHeader")));
        }
        return out;
    }

    /** GetDetailGrdByHeadId :3379 - the 25 columns of grdDetail. */
    public List<Map<String, Object>> historyDetail(int id) {
        header(id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : sup.rows(P_INV, "Id", id, "Activity", "ReadByHeaderId")) {
            out.add(row("ItemName", d.get("ItemName"), "JobLot", d.get("JobLotDescription"), "PackingType", d.get("PackingType"), "PackUom", d.get("UOMCode"),
                    "ItemQty", d.get("ItemQty"), "GrossWeight", d.get("GrossWeight"), "WeightCut", d.get("WeightCut"), "WeightCutTotal", d.get("WeightCutTotal"), "AddLess", d.get("AdLsWeight"),
                    "NetBillWeight", d.get("NetBillWeight"), "NetStockWeight", d.get("NetStockWeight"), "ItemRate", d.get("ItemRate"), "RateUom", d.get("RateUom"), "RateCut", d.get("RateCut"),
                    "RateCutAmount", d.get("RateCutAmount"), "ItemAmount", d.get("ItemAmount"), "WareHouseName", d.get("WareHouseName"), "CityName", d.get("CityName"),
                    "GpNo", d.get("GpNo"), "VehicleNo", d.get("VehicleNo"), "ExchangeRate", d.get("ExchangeRate"), "FcyAmount", d.get("FcyAmount"),
                    "FreightAmount", d.get("FreightAmount"), "CommAmount", d.get("CommissionAmount"), "ExpenseAmount", d.get("ExpenseAmount")));
        }
        return out;
    }

    /** CommonServices.VoucherHeadIdGet(id, 1510) for the history grid's Voucher button. */
    public int voucherHeadId(int id) { header(id); return repo.voucherHeadId(sup.org(), sup.company(), DOC_TYPE, id); }

    // ------------------------------------------------------------------ Save (Insert :1423)

    public static class SaveRequest {
        public Inv state;
        public SaleEngrAttachments.Change attachments;
    }

    /** FormValidation :487 - the first failing message, word for word. */
    private void formValidation(Inv v) {
        if (v.branchId <= 0) throw new Warning("Branch Field is Required");
        if (v.projectId <= 0) throw new Warning("Project Field is Required");
        String no = text(v.docNo);
        if (no.isEmpty() || "0".equals(no)) throw new Warning("DocNo Field is Required");
        if (v.supplierId <= 0) throw new Warning("CustomerName Field is Required");
        String dt = text(v.deliveryTerm);
        if (!"Load".equals(dt) && !"Ponch".equals(dt)) throw new Warning("Delivery Term Field is Required");
        if (v.paymentTermId <= 0) throw new Warning("Payment Term Field is Required");
        String dd = text(v.dueDays);
        if (v.paymentTermId == 2 && (dd.isEmpty() || "0".equals(dd))) throw new Warning("Due Days Field is Required");
        if (v.remarks == null || v.remarks.isEmpty() || "0".equals(v.remarks)) throw new Warning("Invoice Remarks Field is Required");
        if (v.currencyId <= 0) throw new Warning("Currency Field is Required");
        if (text(v.exchangeRate).isEmpty() || decText(v.exchangeRate).signum() == 0) throw new Warning("Exchange Rate Field is Required");
        String fa = text(v.fcyAmount);
        if (fa.isEmpty() || "0".equals(fa)) throw new Warning("Fcy Amount Should Greater Than 0");
        String ba = text(v.billAmount);
        if (ba.isEmpty() || "0".equals(ba)) throw new Warning("Bill Amount Field is Required");
    }

    private static LocalDateTime day(String s) {
        if (s == null || s.isBlank()) return null;
        try { return LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))).atStartOfDay(); } catch (RuntimeException e) { return null; }
    }

    private static Timestamp ts(String s) { LocalDateTime d = day(s); return d == null ? null : Timestamp.valueOf(d); }

    private boolean owned(String sql, int id) {
        Integer n = sup.jdbc().queryForObject(sql, Integer.class, id, sup.org(), sup.company());
        return n != null && n > 0;
    }

    private Set<Integer> ids(List<Map<String, Object>> rows) {
        Set<Integer> s = new HashSet<>();
        for (Map<String, Object> r : rows) s.add(toInt(ci(r, "Id")));
        return s;
    }

    @Transactional
    public Map<String, Object> save(SaveRequest rq) {
        UserAccount u = sup.user();
        Inv v = rq.state;
        if (v == null || v.lines == null || v.lines.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        Map<String, Boolean> rights = sup.rights(SCREEN);
        if (v.id > 0 ? !Boolean.TRUE.equals(rights.get("update")) : !Boolean.TRUE.equals(rights.get("save"))) throw new Warning("You do not have the right to " + (v.id > 0 ? "update" : "save") + " this screen");
        formValidation(v);
        if (toDouble(text(v.freightText)) > 0.0 && v.transporterId == 0) throw new IllegalArgumentException("Transporter Account field Required");

        // tenant checks the desktop gets from its own company-scoped combos
        Set<Integer> savedIds = new HashSet<>();
        if (v.id > 0) {
            Map<String, Object> hd = header(v.id);
            if (toBool(ci(hd, "IsApproved"))) throw new IllegalArgumentException("Record Not Update beacause Record has approved");
            for (Map<String, Object> d : sup.rows(P_INV, "Id", v.id, "Activity", "ReadByHeaderId")) savedIds.add(toInt(ci(d, "Id")));
        }
        if (!ids(base.branches()).contains(v.branchId)) throw new IllegalArgumentException("Branch not found in this company");
        if (!ids(base.projects()).contains(v.projectId)) throw new IllegalArgumentException("Project not found in this company");
        if (!ids(base.currencies()).contains(v.currencyId)) throw new IllegalArgumentException("Currency not found in this company");
        if (!ids(base.paymentTerms()).contains(v.paymentTermId)) throw new IllegalArgumentException("Payment term not found in this company");
        if (!owned("SELECT COUNT(*) FROM SupplierCustomer WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?", v.supplierId)) throw new IllegalArgumentException("Party not found in this company");
        if (v.commAgentId > 0 && !owned("SELECT COUNT(*) FROM SupplierCustomer WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?", v.commAgentId)) throw new IllegalArgumentException("Commission agent not found in this company");
        Set<Integer> itemIds = ids(look.items()), jobLotIds = ids(look.jobLots()), packIds = ids(look.packingTypes()), whIds = ids(look.warehouses()), cityIds = ids(cities());

        for (Fr r : v.freight) {
            if (!(r.Freight > 0.0)) continue;
            if (r.TransporterId == 0) throw new IllegalArgumentException("Please Select an Account Against Freight First");
            if (r.Remarks == null || r.Remarks.isEmpty()) throw new IllegalArgumentException("Grid Charge to Product Remarks required Please Check");
        }
        for (Jv r : v.gl) if ((r.Credit > 0.0 || toInt(r.Debit) > 0) && r.AccountId == 0) throw new Warning("Please Select an Account Against JL First");
        for (Ex r : v.exp) if (r.Amount > 0.0 && r.ItemId == 0) throw new Warning("Please Select an Item Against Expense First");
        if (toDouble(v.commAmount) > 0.0 && v.commAgentId == 0) throw new Warning("Please Select Commission Agent Account First");

        SaleStInvoiceFinancial.Doc o = new SaleStInvoiceFinancial.Doc();
        o.id = v.id > 0 ? v.id : 0;
        o.branchesId = v.branchId; o.projectsId = v.projectId;
        o.docDate = day(v.docDate);
        if (o.docDate == null) throw new IllegalArgumentException("Doc Date is Required");
        o.docNo = toInt(text(v.docNo));
        o.documentTypeId = DOC_TYPE;
        o.supplierCustomerId = v.supplierId;
        o.supplierReferenceNo = toInt(text(v.refNo));
        o.manualBillNo = text(v.billNo);
        o.deliveryTerm = text(v.deliveryTerm);
        o.deliveryStartDate = day(v.deliveryStart);
        o.expiryDate = day(v.expiryDate);
        o.deliveryDays = toInt(text(v.deliveryDays));
        o.paymentTermsId = v.paymentTermId;
        o.dueDays = toInt(text(v.dueDays));
        o.dueDate = day(v.dueDate);
        o.currencyId = v.currencyId;
        o.exchangeRate = decText(v.exchangeRate);
        o.totalQty = decText(v.totalQty);
        o.totalWeight = decText(v.totalWeight);
        o.fcyAmount = decText(v.fcyAmount);
        o.commAgentId = v.commAgentId;
        o.commType = text(v.commType);
        o.commRate = decText(v.commRate);
        o.commUom = toInt(text(v.commUom));
        o.commAmount = decText(v.commAmount);
        o.commRemarks = text(v.commRemarks);
        o.remarksHeader = text(v.remarks);
        o.transporterId = v.transporterId;
        o.freightAmount = decText(text(v.freightText));
        o.billAmount = decText(text(v.billAmount));
        o.organizationId = u.getOrganizationId(); o.companyId = u.getCompanyId(); o.financialYearId = sup.fy();
        o.entryUserId = u.getId(); o.modifyUserId = u.getId(); o.approvedUserId = u.getId();
        o.invoiceTypeId = 4;

        if (v.id > 0 && v.removed != null) {
            for (Line r : v.removed) {
                if (r.Id <= 0 || !savedIds.contains(r.Id)) throw new IllegalArgumentException("Invalid detail row for this sale invoice");
                SaleStInvoiceFinancial.Dt d = new SaleStInvoiceFinancial.Dt();
                fill(d, r, v);
                d.actionTypeId = 3;
                o.details.add(d);
            }
        }
        int rowNo = 0;
        for (Line r : v.lines) {
            rowNo++;
            String at = " in detail grid Row#" + rowNo;
            SaleStInvoiceFinancial.Dt d = new SaleStInvoiceFinancial.Dt();
            d.lineId = rowNo;
            d.id = r.Id;
            if (v.id > 0 && d.id > 0 && !savedIds.contains(d.id)) throw new IllegalArgumentException("Invalid detail row for this sale invoice");
            if (v.id == 0 && d.id > 0) throw new IllegalArgumentException("Invalid detail row for this sale invoice");
            d.saleOrderId = r.OrderId; d.saleOrderDetailId = r.OrderDetailId;
            if (d.saleOrderId > 0 && sup.jdbc().queryForObject("SELECT COUNT(*) FROM ST.SaleOrderDetail d INNER JOIN ST.SaleOrder h ON h.Id = d.SaleOrderId WHERE d.Id = ? AND h.Id = ? AND h.OrganizationId = ? AND h.CompanyId = ?",
                    Integer.class, d.saleOrderDetailId, d.saleOrderId, sup.org(), sup.company()) == 0) throw new IllegalArgumentException("Sale order not found in this company");
            d.itemId = r.ItemId;
            if (d.itemId == 0) throw new IllegalArgumentException("Item not found" + at);
            if (!itemIds.contains(d.itemId)) throw new IllegalArgumentException("Item not found in this company");
            d.jobLotId = r.JobLotId;
            if (d.jobLotId == 0) throw new IllegalArgumentException("JobLot not found" + at);
            if (!jobLotIds.contains(d.jobLotId)) throw new IllegalArgumentException("Job lot not found in this company");
            d.jobLotDescription = r.JobLot == null ? "" : r.JobLot;
            d.packingTypeId = r.PackingTypeId;
            if (d.packingTypeId == 0) throw new IllegalArgumentException("Packing Type not found" + at);
            if (!packIds.contains(d.packingTypeId)) throw new IllegalArgumentException("Packing type not found");
            d.packUomId = r.PackUomId;
            if (d.packUomId == 0) throw new IllegalArgumentException("Pack Uom not found" + at);
            d.uomCode = r.PackUom == null ? "" : r.PackUom;
            d.itemQty = dec(r.ItemQty);
            if (d.itemQty.signum() == 0) throw new IllegalArgumentException("Item Qty not found" + at);
            d.grossWeight = dec(r.GrossWeight);
            if (d.grossWeight.signum() == 0) throw new IllegalArgumentException("Gross Weight not found" + at);
            d.weightCut = dec(r.WeightCut); d.weightCutTotal = dec(r.WeightCutTotal); d.adLsWeight = dec(r.AddLessWeight);
            d.netBillWeight = dec(r.NetBillWeight);
            if (d.netBillWeight.signum() == 0) throw new IllegalArgumentException("Net Weight not found" + at);
            d.netStockWeight = dec(r.StockWeight);
            d.itemRate = dec(r.ItemRate);
            if (d.itemRate.signum() == 0) throw new IllegalArgumentException("Item Rate not found" + at);
            d.rateUomScheduleId = r.RateUomId;
            if (d.rateUomScheduleId == 0) throw new IllegalArgumentException("Rate Uom not found" + at);
            for (int uomId : new int[]{d.packUomId, d.rateUomScheduleId})
                if (sup.jdbc().queryForObject("SELECT COUNT(*) FROM V_UomScheduleAndUom WHERE Id = ? AND CompanyId = ?", Integer.class, uomId, sup.company()) == 0) throw new IllegalArgumentException("UOM not found in this company");
            d.rateUom = r.RateUom == null ? "" : r.RateUom;
            d.rateCut = dec(r.RateCut); d.rateCutAmount = dec(r.RateCutTotal);
            d.itemAmount = dec(r.ItemAmount);
            if (d.itemAmount.signum() == 0) throw new IllegalArgumentException("Item amount not found" + at);
            d.warehouseId = r.WarehouseId;
            if (d.warehouseId == 0) throw new IllegalArgumentException("Warehouse not found" + at);
            if (!whIds.contains(d.warehouseId)) throw new IllegalArgumentException("Warehouse not found in this company");
            d.cityId = r.CityId;
            if (d.cityId == 0) throw new IllegalArgumentException("City not found" + at);
            if (!cityIds.contains(d.cityId)) throw new IllegalArgumentException("City not found in this company");
            d.gpDate = day(r.GpDate);
            d.gpNo = toInt(r.GpNo); d.vehicleNo = r.VehicleNo == null ? "" : r.VehicleNo;
            d.remarksDetail = r.Remarks == null ? "" : r.Remarks; d.remarksSent = d.remarksDetail;
            d.itemNetAmount = dec(r.BillAmount);
            if (d.itemNetAmount.signum() == 0) throw new IllegalArgumentException("Item Net Amount not found" + at);
            d.expenseAmount = dec(r.Expense); d.commissionAmount = dec(r.Commission); d.freightAmount = dec(r.Freight);
            d.journalAmount = BigDecimal.ZERO;                               // InvSaleInvoiceDetail.JournalAmount is not set by Insert()
            d.currencyId = v.currencyId;
            d.exchangeRate = dec(r.ExchangeRate); d.fcyAmount = dec(r.FcyAmount);
            d.actionTypeId = d.id <= 0 ? 1 : 2;
            d.saleOrder = 0;                                           // InvSaleInvoiceDetail.SaleOrder is never set by Insert()
            o.details.add(d);
        }
        for (Fr r : v.freight) if (r.TransporterId != 0) {
            SaleStInvoiceFinancial.Frt f = new SaleStInvoiceFinancial.Frt();
            f.tansporterId = r.TransporterId; f.percentage = dec(r.Percentage); f.frQty = dec(r.Qty); f.frRate = dec(r.Rate); f.creditAmount = dec(r.Freight);
            f.remarks = r.Remarks == null ? "" : r.Remarks;
            o.freights.add(f);
        }
        for (Ex r : v.exp) if (r.ItemId != 0) {
            SaleStInvoiceFinancial.Exp x = new SaleStInvoiceFinancial.Exp();
            x.invOtherItemId = r.ItemId; x.qty = new BigDecimal(toInt(r.Qty)); x.rate = dec(r.Rate); x.amount = dec(r.Amount); x.remarks = r.Remarks == null ? "" : r.Remarks;   // PE.Qty = Conversion.ToInt
            o.expenses.add(x);
        }
        for (Jv r : v.gl) if (r.AccountId != 0) {
            SaleStInvoiceFinancial.Jrn x = new SaleStInvoiceFinancial.Jrn();
            x.chartofAccountId = r.AccountId; x.jvRemarks = r.Remarks == null ? "" : r.Remarks; x.jvPrcnt = dec(r.Percentage); x.jvQty = dec(r.Qty); x.jvRate = dec(r.Rate);
            x.jvDebit = dec(r.Debit); x.jvCredit = dec(r.Credit);
            o.journals.add(x);
        }

        SaleStInvoiceFinancial.Voucher voucher = financial.make(o);        // SaleInvoiceFinancial.MakeVoucherForSaleInvoice
        boolean update = v.id > 0;
        int id = persist.setData(o, voucher, SCREEN, rq.attachments);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", o.docNo);
        out.put("updated", update);
        out.put("voucherHeadId", repo.voucherHeadId(o.organizationId, o.companyId, DOC_TYPE, id));
        out.put("message", (update ? "Record Update Successfully [" : "Record Saved Successfully [") + o.docNo + "] ");
        return out;
    }

    /** A detail row of the removal list (grd_ColumnButtonClick :3975 builds it from the cells without any check). */
    private void fill(SaleStInvoiceFinancial.Dt d, Line r, Inv v) {
        d.id = r.Id; d.saleOrderId = r.OrderId; d.saleOrderDetailId = r.OrderDetailId; d.itemId = r.ItemId; d.jobLotId = r.JobLotId; d.packingTypeId = r.PackingTypeId; d.packUomId = r.PackUomId;
        d.itemQty = dec(r.ItemQty); d.grossWeight = dec(r.GrossWeight); d.weightCut = dec(r.WeightCut); d.weightCutTotal = dec(r.WeightCutTotal); d.adLsWeight = dec(r.AddLessWeight);
        d.netBillWeight = dec(r.NetBillWeight); d.netStockWeight = dec(r.StockWeight); d.itemRate = dec(r.ItemRate); d.rateUomScheduleId = r.RateUomId; d.rateCut = dec(r.RateCut);
        d.rateCutAmount = dec(r.RateCutTotal); d.itemAmount = dec(r.ItemAmount); d.warehouseId = r.WarehouseId; d.cityId = r.CityId; d.gpDate = day(r.GpDate); d.gpNo = toInt(r.GpNo);
        d.vehicleNo = r.VehicleNo == null ? "" : r.VehicleNo; d.remarksDetail = r.Remarks == null ? "" : r.Remarks; d.remarksSent = d.remarksDetail;
        d.itemNetAmount = dec(r.BillAmount); d.expenseAmount = dec(r.Expense); d.commissionAmount = dec(r.Commission); d.freightAmount = dec(r.Freight);
        d.currencyId = v.currencyId; d.exchangeRate = dec(r.ExchangeRate); d.fcyAmount = dec(r.FcyAmount);
    }

    // ------------------------------------------------------------------ attachments

    public List<Map<String, Object>> attachmentList(int id) { header(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { header(id); return attachments.download(SCREEN, id, attachmentId); }
}
