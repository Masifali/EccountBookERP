package com.mst.services.sale.steel;

import com.mst.models.UserAccount;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.repositories.support.DesktopProc;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleEngrSupport;
import com.mst.services.sale.steel.SaleStInvoiceCalc.Cfg;
import com.mst.services.sale.steel.SaleStInvoiceCalc.Ex;
import com.mst.services.sale.steel.SaleStInvoiceCalc.Fr;
import com.mst.services.sale.steel.SaleStInvoiceCalc.Inv;
import com.mst.services.sale.steel.SaleStInvoiceCalc.Jv;
import com.mst.services.sale.steel.SaleStInvoiceCalc.Line;
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
import static com.mst.services.sale.steel.SaleStInvoiceCalc.hash;

/**
 * Screen 543 "SaleInvoice_St" = Architecture.WinApp.Steel.Sale.SaleInvoice_St (Steel Sale Invoice, module 84, document type 1509; the Sale Hub entry "Sale Invoice").
 *
 * Desktop map (SaleInvoice_St.cs): GridComboBind :372, grdInvExpSettings :408, grdFreightSettings :444, gridGLSettings :483, grdSettings :525, AddRowIn* :684-:722,
 * grdFreight_* :723, grdInvExp_* :770, grdGLedger_* :835, grd_CellUpdated :930, grdHistory_ColumnButtonClick :962, GetDetailGrdByHeadId :987, DetailGridSetting :1046,
 * BillAmount :1121, FormValidation :1184, defaultConfiquration :1254, BranchFill :1277, ProjectFill :1295, SupplierNameFilll :1313, PaymentTerms :1338, CurrencyFill :1356,
 * CommissionUOMFill :1381, CommissionTypeFill :1407, AccountsFill :1426, OtherItemsBind :1439, DocumentNo :1456, TotalCommissionAmount :1488, GenerateReportVoucher103 :1566,
 * VoucherHeadIdGet :1590, InvfrmPurchaseInvoice_Load :1602, LoadDataDetailGridAgainstGP :1727, LoadFreightData :1770, LoadInGridDetail :1794, ExpProportion :1838,
 * FreightProportion :1871, LedgerProportion :1904, BillProportion :1963, CommissionProportion :1996, HistoryComboFill :2107, Historyfill :2158, Reset :2337, ReadById :2399,
 * Insert :2514, toolStripButton3_Click_1 :2950 (Load Gdn), btnRefresh_Click :3166, btnSlipFormat2_Click :3184, grdHistory_LinkClicked :3201, txtExchangeRate_TextChanged :3326,
 * CalculateTotalInformation :3361; frmLoadGDN_St.cs: PendingGdnLoad :84, btnLoadOnInvoice_Click_1 :206, grd_SelectionChanged :258.
 *
 * Procedures: [ST].[USP_SaleInvoice_GetAllMethod] (GenerateCode, ReadById, ReadByHeaderId, InvSaleInvoiceExpense_/Freight_/Journal_ReadBySaleInvoiceID), [ST].[USP_SaleInvoice_FormHistory],
 * [ST].[USP_GetDataForDropDownFromSaleInvoice], [ST].[USP_GetPendingGdnForSaleInvoice], [ST].[USP_GdnLoadForSaleInvoice], [ST].[USP_GetTransporterAndFreightFromGdn],
 * [ST].[USP_InvGdn_GetAllMethod] (ReadByHeaderId), and the save chain of SaleStInvoicePersist.
 */
@Service
public class SaleSaleInvoiceStService {
    public static final String SCREEN = "SaleInvoice_St";
    public static final int DOC_TYPE = 1509;
    private static final String P_INV = "[ST].[USP_SaleInvoice_GetAllMethod]";

    private final SaleEngrSupport sup;
    private final SaleSteelSupport steel;
    private final SaleInvoiceRepository repo;
    private final SaleEngrAttachments attachments;
    private final SaleStInvoiceFinancial financial;
    private final SaleStInvoicePersist persist;
    private final SaleSteelLookups look;

    public SaleSaleInvoiceStService(SaleEngrSupport sup, SaleSteelSupport steel, SaleInvoiceRepository repo, SaleEngrAttachments attachments,
                                    SaleStInvoiceFinancial financial, SaleStInvoicePersist persist, SaleSteelLookups look) {
        this.sup = sup; this.steel = steel; this.repo = repo; this.attachments = attachments; this.financial = financial; this.persist = persist; this.look = look;
    }

    // ------------------------------------------------------------------ configuration / lookups

    /** CommonServices.GetDecimalConfiguration + CreditAmountInItemSaleGL. */
    public Cfg cfg() {
        Cfg c = new Cfg();
        int a = toInt(sup.config("Default NoofDecimal Points For Amount"));
        c.amtRound = a; c.amtDec = a >= 1 && a <= 4 ? a : 0;
        int f = toInt(sup.config("DefaultNoOfDecimalPointsForFcyAmount"));
        c.fcyRound = f; c.fcyDec = f >= 1 && f <= 4 ? f : 0;
        int r = toInt(sup.config("Default NoofDecimal Points For Rate"));
        c.rateDec = r >= 1 && r <= 4 ? r : (r == 0 ? 2 : 0);
        String cr = sup.config("CreditAmountInItemSaleGL");
        c.creditRaw = cr.isEmpty() ? null : cr;
        return c;
    }

    public Map<String, Object> formats() {
        Cfg c = cfg();
        return row("amtDec", c.amtDec, "fcyDec", c.fcyDec, "rateDec", c.rateDec);
    }

    /** BranchFill :1277 - Branches.GetAll. */
    public List<Map<String, Object>> branches() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_Branches_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll"))
            out.add(row("Id", toInt(ci(r, "Id")), "BranchName", str(ci(r, "BranchName"))));
        return out;
    }

    /** ProjectFill :1295 - Projects.GetAlldt. */
    public List<Map<String, Object>> projects() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_Projects_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll"))
            out.add(row("Id", toInt(ci(r, "Id")), "ProjectName", str(ci(r, "ProjectName"))));
        return out;
    }

    /** SupplierNameFilll :1313 - SupplierCustomer.Getall. */
    public List<Map<String, Object>> customers() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationCompanyId"))
            out.add(row("Id", toInt(ci(r, "Id")), "CompanyName", str(ci(r, "CompanyName")), "GlAccountId", toInt(ci(r, "GlAccountId"))));
        return out;
    }

    /** PaymentTerms :1338 - CommonServices.GetDueTermServiceBind. */
    public List<Map<String, Object>> paymentTerms() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_InvDueTerms_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll"))
            out.add(row("Id", toInt(ci(r, "Id")), "TermsDescription", str(ci(r, "TermsDescription"))));
        return out;
    }

    /** CurrencyFill :1356 - MultiCurrency.GetAll. */
    public List<Map<String, Object>> currencies() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_MultiCurrency_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadAll"))
            out.add(row("Id", toInt(ci(r, "Id")), "CurrencyCode", str(ci(r, "CurrencyCode"))));
        return out;
    }

    /** AccountsFill :1426 - CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,,13,14,15,20,21,22") (the list is the NOT list). */
    public List<Map<String, Object>> accounts() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("AppId", sup.ctx().currentAppId());
        p.put("AccountTypeIdsNot", "2,11,12,,13,14,15,20,21,22");
        if (sup.userId() != 0) p.put("UserId", sup.userId());
        p.put("Activity", "GetAccountTitleByAccountTypeIds");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(sup.jdbc(), "Sp_COAAllocation_GetAllMethod", p)) out.add(row("Id", r.get("Id"), "AccountTitle", r.get("AccountTitle")));
        return out;
    }

    /** OtherItemsBind :1439 - InventoryItemsOther.GetAll. */
    public List<Map<String, Object>> otherItems() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.otherItems(sup.org(), sup.company())) out.add(row("Id", toInt(ci(r, "Id")), "OtherItemName", str(ci(r, "OtherItemName"))));
        return out;
    }

    /** CommissionTypeFill :1407 / CommissionUOMFill :1381 (the lists are fixed in the form). */
    public List<Map<String, Object>> commissionTypes() {
        return List.of(row("Id", 1, "CommissionType", "Flat"), row("Id", 2, "CommissionType", "Percent"), row("Id", 3, "CommissionType", "Weight"));
    }

    public List<Map<String, Object>> commissionUoms() {
        List<Map<String, Object>> l = new ArrayList<>();
        int i = 1;
        for (String u : new String[]{"1", "5", "10", "25", "40", "50", "60", "65", "80", "100"}) l.add(row("Id", i++, "Uom", u));
        return l;
    }

    /** defaultConfiquration :1254 - Base Currency / BaseCurrencyRate. */
    public Map<String, Object> defaults() {
        Map<String, Object> m = new LinkedHashMap<>();
        String bc = sup.config("Base Currency");
        if (!bc.isEmpty()) m.put("currencyId", toInt(bc));
        String br = sup.config("BaseCurrencyRate");
        if (!br.isEmpty()) m.put("exchangeRate", String.valueOf(toInt(br)));
        return m;
    }

    /** DocumentNo :1456 - InvSaleInvoice.GenerateInvSaleInvoiceCode (0 -> "Max Number Not Found"). */
    public int nextNo() {
        UserAccount u = sup.user();
        List<Map<String, Object>> r = DesktopProc.rows(sup.jdbc(), P_INV, DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", u.getBranchesId(),
                "DocumentTypeId", DOC_TYPE, "FinancialYearId", sup.fy(), "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** HistoryComboFill :2107 - InvSaleInvoice.GetDataForDropDownFromSaleInvoiceForSteel split by Activity. */
    public Map<String, Object> historyCombos() {
        List<Map<String, Object>> cust = new ArrayList<>(), pay = new ArrayList<>(), del = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("[ST].[USP_GetDataForDropDownFromSaleInvoice]", "OrganizationId", sup.org(), "CompanyId", sup.company())) {
            String a = str(r.get("Activity"));
            Map<String, Object> o = row("Id", r.get("Id"), "Name", r.get("ReferenceName"));
            if ("Customer".equals(a)) cust.add(o); else if ("PaymentTerm".equals(a)) pay.add(o); else if ("DeliveryTerm".equals(a)) del.add(o);
        }
        return row("customers", cust, "paymentTerms", pay, "deliveryTerms", del);
    }

    /** InvfrmPurchaseInvoice_Load :1602 */
    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("branches", branches());
        m.put("projects", projects());
        m.put("customers", customers());
        m.put("paymentTerms", paymentTerms());
        m.put("currencies", currencies());
        m.put("accounts", accounts());
        m.put("otherItems", otherItems());
        m.put("commTypes", commissionTypes());
        m.put("commUoms", commissionUoms());
        m.put("defaults", defaults());
        m.put("formats", formats());
        m.put("nextNo", nextNo());
        m.put("history", historyCombos());
        int days = toInt(sup.config("DefaultDaysToLessFromHistoryFromDate"));
        m.put("historyDays", days > 0 ? days : 3);
        return m;
    }

    /** btnRefresh_Click :3166 - AccountsFill, OtherItemsBind, BranchFill, ProjectFill, GridComboBind, SupplierNameFilll, PaymentTerms. */
    public Map<String, Object> refresh() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("accounts", accounts());
        m.put("otherItems", otherItems());
        m.put("branches", branches());
        m.put("projects", projects());
        m.put("customers", customers());
        m.put("paymentTerms", paymentTerms());
        return m;
    }

    private int glOf(int supplierId) {
        if (supplierId <= 0) return 0;
        List<Map<String, Object>> r = sup.jdbc().queryForList("SELECT GlAccountId FROM SupplierCustomer WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?", supplierId, sup.org(), sup.company());
        return r.isEmpty() ? 0 : toInt(r.get(0).get("GlAccountId"));
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
        Cfg c = cfg();
        SaleStInvoiceCalc k = new SaleStInvoiceCalc(c, v);
        v.supplierGlId = glOf(v.supplierId);
        String e = rq.event == null ? "" : rq.event;
        switch (e) {
            case "grid" -> k.grdCellUpdated(rq.row, rq.col);
            case "freightCell" -> k.freightCell();
            case "freightBtn" -> k.freightButton(rq.key, rq.row);
            case "expCell" -> k.expCell(rq.row, rq.col);
            case "expBtn" -> k.expButton(rq.key, rq.row);
            case "glCell" -> k.glCell(rq.row, rq.col);
            case "glBtn" -> k.glButton(rq.key, rq.row);
            case "comm" -> k.commissionChanged();
            case "exch" -> k.exchangeChanged();
            case "due" -> v.dueDate = SaleStInvoiceCalc.dueDate(v.dueDays);
            case "supplier" -> { /* cmbsuppliername_ValueChanged :3250 only fills txtSupplierGLId (done above) */ }
            default -> throw new IllegalArgumentException("Unknown event " + e);
        }
        return v;
    }

    // ------------------------------------------------------------------ Load GDN (frmLoadGDN_St)

    /** PendingGdnLoad :84 - the 11 columns of the loader grid. */
    public List<Map<String, Object>> loaderPending(String from, String to) {
        List<Map<String, Object>> out = new ArrayList<>();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("DocumentTypeId", 1508);
        Timestamp f = ts(from), t = ts(to);
        if (f != null) p.put("FromDate", f);
        if (t != null) p.put("ToDate", t);
        p.put("FinancialYearId", sup.fy());
        for (Map<String, Object> r : steel.table("[ST].[USP_GetPendingGdnForSaleInvoice]", p).rows) {
            out.add(row("Id", r.get("Id"), "DocDate", r.get("DocDate"), "DocNo", r.get("DocNo"), "GdnType", r.get("OrderType"), "RefDocId", r.get("OrderTypeId"),
                    "SupplierCustomerId", r.get("SupplierCustomerId"), "CustomerName", r.get("SupplierCustomer"), "GpNO", r.get("GpNo"), "BiltyNo", r.get("BiltyNo"),
                    "VehicleNo", r.get("VehicleNo"), "ItemQty", r.get("ItemQty")));
        }
        return out;
    }

    /** grd_SelectionChanged :258 - InvGdn.ReadById detail rows. */
    public List<Map<String, Object>> loaderDetail(int gdnId) {
        List<Map<String, Object>> out = new ArrayList<>();
        Integer n = sup.jdbc().queryForObject("SELECT COUNT(*) FROM ST.InvGdn WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?", Integer.class, gdnId, sup.org(), sup.company());
        if (n == null || n == 0) return out;
        for (Map<String, Object> d : sup.rows("[ST].[USP_InvGdn_GetAllMethod]", "Id", gdnId, "Activity", "ReadByHeaderId"))
            out.add(row("SaleOrder", d.get("SaleOrderNo"), "ItemName", d.get("ItemName"), "JobLot", d.get("JobLotDescription"), "PackingType", d.get("PackingType"), "PackUom", d.get("UOMCode"),
                    "ItemQty", d.get("ItemQty"), "GrossWight", d.get("GrossWeight"), "WtCut", d.get("WtCut"), "WtCutTotal", d.get("WtCutTotal"), "AddLesswt", d.get("AdLsWeight"),
                    "NetWeight", d.get("NetBillWeight"), "StockWeight", d.get("StockWeight"), "WareHouse", d.get("WarehouseName") != null ? d.get("WarehouseName") : d.get("WareHouseName"),
                    "LabNo", d.get("LabReportRef"), "City", d.get("CityName")));
        return out;
    }

    public static class LoadRequest {
        public List<Integer> gdnIds = new ArrayList<>();
        public Inv state;
    }

    /** LoadInGridDetail :1794 (+ LoadDataDetailGridAgainstGP :1727 and LoadFreightData :1770). */
    public Inv load(LoadRequest rq) {
        Inv v = rq.state == null ? new Inv() : rq.state;
        Cfg c = cfg();
        SaleStInvoiceCalc k = new SaleStInvoiceCalc(c, v);
        List<Integer> ids = rq.gdnIds == null ? new ArrayList<>() : rq.gdnIds;
        StringBuilder gs = new StringBuilder();
        for (int id : ids) gs.append(',').append(id);
        v.lines = new ArrayList<>();
        // the desktop passes the ids as sent by the loader; only the GDNs of this company can be read (the procedure is not tenant-aware)
        List<Integer> mine = new ArrayList<>();
        for (int id : ids) {
            if (id == 0) { mine.add(id); continue; }
            Integer n = sup.jdbc().queryForObject("SELECT COUNT(*) FROM ST.InvGdn WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?", Integer.class, id, sup.org(), sup.company());
            if (n != null && n > 0) mine.add(id); else throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Goods dispatch note not found in this company");
        }
        StringBuilder safe = new StringBuilder();
        for (int id : mine) safe.append(',').append(id);
        String gdnIds = safe.toString();

        SaleSteelSupport.Table g = steel.table("[ST].[USP_GdnLoadForSaleInvoice]", "GdnIds", gdnIds);
        List<Map<String, Object>> rows = g.rows;
        if (!rows.isEmpty()) {
            Map<String, Object> f = rows.get(0);
            v.supplierId = toInt(f.get("SupplierCustomerId"));
            v.supplierGlId = glOf(v.supplierId);
            v.commAgentId = toInt(f.get("CommissionAgentId"));
            v.commType = str(f.get("CommissionType"));
            v.commRate = str(f.get("CommRate"));
            v.commUom = str(f.get("CommRateUom"));
            v.commAmount = str(f.get("CommAmount"));
            v.commRemarks = str(f.get("CommRemarks"));
            v.dueDays = str(f.get("OrderDueDays"));
            Object dd = f.get("OrderDueDate");
            v.dueDate = dd == null ? SaleStInvoiceCalc.dueDate(v.dueDays) : str(dd).substring(0, Math.min(10, str(dd).length()));
            v.orderType = str(f.get("OrderType"));
            v.deliveryTerm = "GateSale".equals(v.orderType) ? "Load" : str(f.get("DeliveryTerm"));
            v.remarks = str(f.get("RemarksHeader"));
            BigDecimal ex = decText(v.exchangeRate);
            for (Map<String, Object> r : rows) {
                Line l = new Line();
                l.Id = 0;
                l.InvGdnId = toInt(r.get("InvGdnId")); l.InvGdnDetailId = toInt(r.get("GdnDetailId"));
                l.SaleOrderId = toInt(r.get("SaleOrderId")); l.SaleOrderDetailId = toInt(r.get("SaleOrderDetailId")); l.SaleOrder = toInt(r.get("OrderNo"));
                l.ItemId = toInt(r.get("ItemId")); l.ItemName = str(r.get("ItemName"));
                l.JobLotId = toInt(r.get("JobLotId")); l.JobLot = str(r.get("JobLotDescription"));
                l.PackingTypeId = toInt(r.get("PackingTypeId")); l.PackingType = str(r.get("PackTypeDesc"));
                l.PackUomId = toInt(r.get("ItemUomId")); l.PackUom = str(r.get("UOMCodeItem"));
                l.ItemQty = toDouble(r.get("ItemQty")); l.GrossWeight = toDouble(r.get("GrossWeight")); l.WtCut = toDouble(r.get("WtCut")); l.WtCutTotal = toDouble(r.get("WtCutTotal"));
                l.AddLessWeight = toDouble(r.get("AdLsWeight")); l.NetBillWeight = toDouble(r.get("NetBillWeight")); l.StockWeight = toDouble(r.get("StockWeight"));
                l.ItemRate = toDouble(r.get("OrderItemRate")); l.RateUomId = toInt(r.get("OrderItemRateUOMId")); l.RateUom = str(r.get("RateUom"));
                l.EquivalentSoRate = toDouble(r.get("EquivalentSoRate"));
                l.RateCut = 0; l.RateCutAmount = 0; l.ItemAmount = toDouble(r.get("ItemAmount"));
                l.WarehouseId = toInt(r.get("WarehouseId")); l.Warehouse = str(r.get("WareHouseName"));
                l.CityId = toInt(r.get("CityId")); l.CityName = str(r.get("CityName"));
                l.GpNo = toInt(r.get("GpNo")); l.VehicleNo = str(r.get("VehicleNo"));
                l.BillAmount = 0;
                if (ex.signum() > 0) {
                    l.ExchangeRate = ex.doubleValue();
                    l.FcyAmount = SaleStInvoiceCalc.rndDec(decText(str(r.get("ItemAmount"))).divide(ex, new java.math.MathContext(28, java.math.RoundingMode.HALF_EVEN)), c.fcyRound).doubleValue();
                }
                v.lines.add(l);
            }
            v.soid = v.lines.get(0).SaleOrderId;
        }
        // LoadFreightData / dtInvExp.Clear
        v.freight = new ArrayList<>();
        for (Map<String, Object> r : steel.table("[ST].[USP_GetTransporterAndFreightFromGdn]", "GdnIds", gdnIds).rows) {
            Fr f = new Fr();
            f.InvGdnId = toInt(r.get("MainId")); f.Transporter = toInt(r.get("Transporter")); f.Freight = toDouble(r.get("CarriageAmount")); f.Debit = 0;
            v.freight.add(f);
        }
        if (v.freight.isEmpty()) k.addFreightRow();
        v.exp = new ArrayList<>();
        k.addExpRow();
        k.afterLoad();
        k.calculateTotalInformation();                // planned deviation: the desktop refreshes the totals only on the next cell edit
        return v;
    }

    // ------------------------------------------------------------------ ReadById :2399

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> r = sup.rows(P_INV, "Id", id, "Activity", "ReadById");
        UserAccount u = sup.user();
        if (r.isEmpty() || toInt(ci(r.get(0), "OrganizationId")) != u.getOrganizationId() || toInt(ci(r.get(0), "CompanyId")) != u.getCompanyId()
                || toInt(ci(r.get(0), "DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Sale invoice not found in this company");
        return r.get(0);
    }

    private static String dateText(Object o) {
        if (o == null) return "";
        String s = str(o);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        Cfg c = cfg();
        Inv v = new Inv();
        v.id = id;
        v.branchId = toInt(ci(h, "BranchesId")); v.projectId = toInt(ci(h, "ProjectsId"));
        v.docNo = str(ci(h, "DocNo")); v.docDate = dateText(ci(h, "DocDate"));
        v.supplierId = toInt(ci(h, "SupplierCustomerId")); v.supplierGlId = glOf(v.supplierId);
        v.refNo = str(ci(h, "SupplierReferenceNo")); v.billNo = str(ci(h, "ManualBillNo"));
        v.paymentTermId = toInt(ci(h, "PaymentTermsId"));
        v.dueDays = str(ci(h, "DueDays")); v.dueDate = dateText(ci(h, "DueDate"));
        v.deliveryTerm = str(ci(h, "DeliveryTerm"));
        v.currencyId = toInt(ci(h, "CurrencyId"));
        v.exchangeRate = fmt(toDecimal(ci(h, "ExchangeRate")), c.rateDec);
        v.totalQty = hash(toDecimal(ci(h, "TotalQty")), 2);
        v.totalWeight = hash(toDecimal(ci(h, "TotalWeight")), 2);
        v.fcyAmount = fmt(toDecimal(ci(h, "FcyAmount")), c.fcyDec);
        v.billAmount = fmt(toDecimal(ci(h, "BillAmount")), c.amtDec);
        v.commAgentId = toInt(ci(h, "CommAgentId"));
        v.commType = str(ci(h, "CommType"));
        v.commRate = fmt(toDecimal(ci(h, "CommRate")), c.rateDec);
        v.commUom = str(ci(h, "CommUOM"));
        v.commAmount = fmt(toDecimal(ci(h, "CommAmount")), c.amtDec);
        v.commRemarks = str(ci(h, "CommRemarks"));
        v.remarks = str(ci(h, "RemarksHeader"));
        int it = toInt(ci(h, "InvoiceTypeId"));
        v.orderType = it == 1 ? "SaleOrder" : it == 2 ? "DeliveryOrder" : it == 3 ? "GateSale" : "";
        for (Map<String, Object> d : sup.rows(P_INV, "Id", id, "Activity", "ReadByHeaderId")) {
            Line l = new Line();
            l.Id = toInt(ci(d, "Id")); l.InvGdnId = toInt(ci(d, "InvGdnId")); l.InvGdnDetailId = toInt(ci(d, "InvGdnDetailId"));
            l.SaleOrderId = toInt(ci(d, "SaleOrderId")); l.SaleOrderDetailId = toInt(ci(d, "SaleOrderDetailId")); l.SaleOrder = toInt(ci(d, "SaleOrder"));
            l.ItemId = toInt(ci(d, "ItemId")); l.ItemName = str(ci(d, "ItemName"));
            l.JobLotId = toInt(ci(d, "JobLotId")); l.JobLot = str(ci(d, "JobLotDescription"));
            l.PackingTypeId = toInt(ci(d, "PackingTypeId")); l.PackingType = str(ci(d, "PackingType"));
            l.PackUomId = toInt(ci(d, "PackUOMId")); l.PackUom = str(ci(d, "UOMCode"));
            l.ItemQty = toDouble(ci(d, "ItemQty")); l.GrossWeight = toDouble(ci(d, "GrossWeight")); l.WtCut = toDouble(ci(d, "WeightCut")); l.WtCutTotal = toDouble(ci(d, "WeightCutTotal"));
            l.AddLessWeight = toDouble(ci(d, "AdLsWeight")); l.NetBillWeight = toDouble(ci(d, "NetBillWeight")); l.StockWeight = toDouble(ci(d, "NetStockWeight"));
            l.ItemRate = toDouble(ci(d, "ItemRate")); l.RateUomId = toInt(ci(d, "RateUomScheduleId")); l.RateUom = str(ci(d, "RateUom")); l.EquivalentSoRate = toDouble(ci(d, "EquivalentSoRate"));
            l.RateCut = toDouble(ci(d, "RateCut")); l.RateCutAmount = toDouble(ci(d, "RateCutAmount")); l.ItemAmount = toDouble(ci(d, "ItemAmount"));
            l.WarehouseId = toInt(ci(d, "WarehouseId")); l.Warehouse = str(ci(d, "WareHouseName"));
            l.CityId = toInt(ci(d, "CityId")); l.CityName = str(ci(d, "CityName"));
            l.GpNo = toInt(ci(d, "GpNo")); l.VehicleNo = str(ci(d, "VehicleNo"));
            l.BillAmount = toDouble(ci(d, "ItemNetAmount")); l.ExchangeRate = toDouble(ci(d, "ExchangeRate")); l.FcyAmount = toDouble(ci(d, "FcyAmount"));
            l.Freights = toDouble(ci(d, "FreightAmount")); l.Expense = toDouble(ci(d, "ExpenseAmount")); l.Commission = toDouble(ci(d, "CommissionAmount")); l.Journal = toDouble(ci(d, "JournalAmount"));
            v.lines.add(l);
        }
        v.soid = v.lines.isEmpty() ? 0 : v.lines.get(0).SaleOrderId;
        for (Map<String, Object> e : sup.rows(P_INV, "Id", id, "Activity", "InvSaleInvoiceExpense_ReadBySaleInvoiceID")) {
            Ex x = new Ex(); x.ItemId = toInt(ci(e, "InvOtherItemId")); x.Qty = toDouble(ci(e, "Qty")); x.Rate = toDouble(ci(e, "Rate")); x.Amount = toDouble(ci(e, "Amount")); x.Remarks = str(ci(e, "Remarks"));
            v.exp.add(x);
        }
        for (Map<String, Object> e : sup.rows(P_INV, "Id", id, "Activity", "InvSaleInvoiceJournal_ReadBySaleInvoiceID")) {
            Jv x = new Jv(); x.AccountId = toInt(ci(e, "ChartofAccountId")); x.Remarks = str(ci(e, "JvRemarks")); x.Percentage = toDouble(ci(e, "JvPrcnt"));
            x.Qty = toDouble(ci(e, "JvQty")); x.Rate = toDouble(ci(e, "JvRate")); x.Debit = toDouble(ci(e, "JvDebit")); x.Credit = toDouble(ci(e, "JvCredit"));
            v.gl.add(x);
        }
        for (Map<String, Object> e : sup.rows(P_INV, "Id", id, "Activity", "InvSaleInvoiceFreight_ReadBySaleInvoiceID")) {
            Fr x = new Fr(); x.InvGdnId = toInt(ci(e, "InvGdnId")); x.Transporter = toInt(ci(e, "TansporterId")); x.Freight = toDouble(ci(e, "CreditAmount")); x.Debit = toDouble(ci(e, "DebitAmount"));
            v.freight.add(x);
        }
        SaleStInvoiceCalc k = new SaleStInvoiceCalc(c, v);
        if (v.exp.isEmpty()) k.addExpRow();
        if (v.gl.isEmpty()) k.addGlRow();
        if (v.freight.isEmpty()) k.addFreightRow();
        k.afterRead();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("state", v);
        out.put("approved", toBool(ci(h, "IsApproved")));
        out.put("voucherHeadId", repo.voucherHeadId(sup.org(), sup.company(), DOC_TYPE, id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    // ------------------------------------------------------------------ History (Historyfill :2158)

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
            out.add(row("Id", r.get("Id"), "VoucherHeadId", r.get("VoucherHeadId"), "DocNo", r.get("DocNo"), "DocDate", r.get("DocDate"), "CustomerName", r.get("SupplierName"),
                    "DueDate", r.get("DueDate"), "CommAgent", r.get("CommissionAgent"), "CommType", r.get("CommType"), "CommRate", r.get("CommRate"), "CommAmount", toDouble(r.get("CommAmount")),
                    "CommRemarks", r.get("CommRemarks"), "TotalQty", r.get("TotalQty"), "TotalWeight", r.get("TotalWeight"), "BillAmount", r.get("BillAmount"), "ExchangeRate", r.get("ExchangeRate"),
                    "FcyAmount", r.get("FcyAmount"), "UserName", r.get("UserName"), "EntryDate", r.get("EntryDate"), "ModifyUser", r.get("ModifyUserName"), "ModifyDate", r.get("ModifyDate"),
                    "IsApproved", toBool(r.get("IsApproved")) ? "Approved" : "Not Approved", "ApprovedUser", r.get("ApprovedUserName"), "ApprovedDate", r.get("ApprovedDate"),
                    "Remarks", r.get("RemarksHeader"), "NoOfAttachments", r.get("NoOfAttachments")));
        }
        return out;
    }

    /** GetDetailGrdByHeadId :987 - the 27 columns of grdDetail. */
    public List<Map<String, Object>> historyDetail(int id) {
        header(id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : sup.rows(P_INV, "Id", id, "Activity", "ReadByHeaderId")) {
            out.add(row("SaleOrder", d.get("SaleOrder"), "ItemName", d.get("ItemName"), "JobLot", d.get("JobLotDescription"), "PackingType", d.get("PackingType"), "PackUom", d.get("UOMCode"),
                    "ItemQty", d.get("ItemQty"), "GrossWeight", d.get("GrossWeight"), "WeightCut", d.get("WeightCut"), "WeightCutTotal", d.get("WeightCutTotal"), "AddLessWeight", d.get("AdLsWeight"),
                    "NetBillWeight", d.get("NetBillWeight"), "NetStockWeight", d.get("NetStockWeight"), "ItemRate", d.get("ItemRate"), "RateUom", d.get("RateUom"), "RateCut", d.get("RateCut"),
                    "RateCutAmount", d.get("RateCutAmount"), "ItemAmount", d.get("ItemAmount"), "WareHouse", d.get("WareHouseName"), "CityName", d.get("CityName"), "GpDate", d.get("GpDate"),
                    "GpNo", d.get("GpNo"), "VehicleNo", d.get("VehicleNo"), "ExchangeRate", d.get("ExchangeRate"), "FcyAmount", d.get("FcyAmount"), "BillAmount", d.get("ItemNetAmount"),
                    "CommAmount", d.get("CommissionAmount"), "ExpenseAmount", d.get("ExpenseAmount"), "FreightAmount", d.get("FreightAmount")));
        }
        return out;
    }

    // ------------------------------------------------------------------ Save (Insert :2514)

    public static class SaveRequest {
        public Inv state;
        public SaleEngrAttachments.Change attachments;
    }

    /** FormValidation :1184 - the first failing message, word for word. */
    private void formValidation(Inv v) {
        if (v.branchId <= 0) throw new Warning("Branch Name  field is Required");
        if (v.projectId <= 0) throw new Warning("Project Name  field is Required");
        String no = text(v.docNo);
        if (no.isEmpty() || "0".equals(no)) throw new Warning("DocNo  field is Required");
        if (v.supplierId <= 0) throw new Warning("Party Name field is Required");
        if (v.paymentTermId <= 0) throw new Warning("Payment Term is Required");
        String dd = text(v.dueDays);
        if (v.paymentTermId == 2 && (dd.isEmpty() || "0".equals(dd))) throw new Warning("Due Days Field is Required");
        if (v.remarks == null || v.remarks.isEmpty() || "0".equals(v.remarks)) throw new Warning("Invoice Remarks Field is Required");
        String ba = text(v.billAmount);
        if (ba.isEmpty() || "0".equals(ba)) throw new Warning("Bill Amount Field is Required");
        if (v.currencyId <= 0) throw new Warning("Currency is Required");
        if (v.exchangeRate == null || v.exchangeRate.isEmpty() || "0".equals(v.exchangeRate)) throw new Warning("Exchange Rate is Required");
        String fa = text(v.fcyAmount);
        if (fa.isEmpty() || "0".equals(fa)) throw new Warning("Fcy Amount Should Greater Than 0");
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

    @Transactional
    public Map<String, Object> save(SaveRequest rq) {
        UserAccount u = sup.user();
        Inv v = rq.state;
        if (v == null || v.lines == null || v.lines.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        Cfg c = cfg();
        SaleStInvoiceCalc k = new SaleStInvoiceCalc(c, v);
        v.supplierGlId = glOf(v.supplierId);
        formValidation(v);

        // tenant checks the desktop gets from its own company-scoped combos
        Set<Integer> savedIds = new HashSet<>();
        if (v.id > 0) {
            header(v.id);
            for (Map<String, Object> d : sup.rows(P_INV, "Id", v.id, "Activity", "ReadByHeaderId")) savedIds.add(toInt(ci(d, "Id")));
        }
        Set<Integer> br = new HashSet<>(), pr = new HashSet<>(), cu = new HashSet<>(), pt = new HashSet<>();
        for (Map<String, Object> r : branches()) br.add(toInt(r.get("Id")));
        for (Map<String, Object> r : projects()) pr.add(toInt(r.get("Id")));
        for (Map<String, Object> r : currencies()) cu.add(toInt(r.get("Id")));
        for (Map<String, Object> r : paymentTerms()) pt.add(toInt(r.get("Id")));
        if (!br.contains(v.branchId)) throw new IllegalArgumentException("Branch not found in this company");
        if (!pr.contains(v.projectId)) throw new IllegalArgumentException("Project not found in this company");
        if (!cu.contains(v.currencyId)) throw new IllegalArgumentException("Currency not found in this company");
        if (!pt.contains(v.paymentTermId)) throw new IllegalArgumentException("Payment term not found in this company");
        if (!owned("SELECT COUNT(*) FROM SupplierCustomer WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?", v.supplierId)) throw new IllegalArgumentException("Party not found in this company");
        if (v.commAgentId > 0 && !owned("SELECT COUNT(*) FROM SupplierCustomer WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?", v.commAgentId)) throw new IllegalArgumentException("Commission agent not found in this company");

        // the recalculation of Insert() before the checks
        k.beforeSave();

        for (Fr r : v.freight) if (r.Freight > 0.0 && r.Transporter == 0) throw new Warning("Please Select an Account Against Freight First");
        for (Jv r : v.gl) if ((r.Credit > 0.0 || toInt(r.Debit) > 0) && r.AccountId == 0) throw new Warning("Please Select an Account Against JL First");
        for (Ex r : v.exp) if (r.Amount > 0.0 && r.ItemId == 0) throw new Warning("Please Select an Item Against Expense First");
        if (SaleEngrSupport.toDouble(v.commAmount) > 0.0 && v.commAgentId == 0) throw new Warning("Please Select Commission Agent Account First");

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
        o.paymentTermsId = v.paymentTermId;
        o.dueDays = toInt(text(v.dueDays));
        o.dueDate = day(v.dueDate);
        o.deliveryTerm = text(v.deliveryTerm);
        o.currencyId = v.currencyId;
        o.exchangeRate = decText(text(v.exchangeRate));
        o.fcyAmount = decText(text(v.fcyAmount));
        o.totalQty = decText(text(v.totalQty));
        o.totalWeight = decText(text(v.totalWeight));
        o.commAmount = decText(v.commAmount);
        o.commAgentId = v.commAgentId;
        o.commRemarks = text(v.commRemarks);
        o.commUom = toInt(text(v.commUom));
        o.commType = text(v.commType);
        o.commRate = decText(v.commRate);
        o.remarksHeader = text(v.remarks);
        o.billAmount = decText(text(v.billAmount));
        o.organizationId = u.getOrganizationId(); o.companyId = u.getCompanyId(); o.financialYearId = sup.fy();
        o.entryUserId = u.getId(); o.modifyUserId = u.getId();
        String ot = text(v.orderType);
        o.invoiceTypeId = "SaleOrder".equals(ot) ? 1 : "DeliveryOrder".equals(ot) ? 2 : "GateSale".equals(ot) ? 3 : 0;
        int soid = v.lines.get(0).SaleOrderId;                         // SOID = the SaleOrderId of the first loaded row

        int rowNo = 0;
        for (Line r : v.lines) {
            rowNo++;
            int idx = rowNo;
            SaleStInvoiceFinancial.Dt d = new SaleStInvoiceFinancial.Dt();
            d.lineId = idx;
            d.id = r.Id;
            d.invGdnId = r.InvGdnId; d.invGdnDetailId = r.InvGdnDetailId;
            if (d.invGdnId == 0) throw new Warning("GdnId not found");
            if (d.invGdnDetailId == 0) throw new Warning("GdnDetail Id not found");
            if (!owned("SELECT COUNT(*) FROM ST.InvGdn WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?", d.invGdnId)) throw new IllegalArgumentException("Goods dispatch note not found in this company");
            if (v.id > 0 && d.id > 0 && !savedIds.contains(d.id)) throw new IllegalArgumentException("Invalid detail row for this sale invoice");
            if (soid > 0) {
                d.saleOrderId = r.SaleOrderId;
                if (d.saleOrderId == 0) throw new Warning("SaleOrderId Field Required");
                d.saleOrderDetailId = r.SaleOrderDetailId;
            }
            d.itemId = r.ItemId;
            if (d.itemId == 0) throw new Warning("Item Required In Row# " + idx);
            d.jobLotId = r.JobLotId;
            if (d.jobLotId == 0) throw new Warning("JobLot Required In Row# " + idx);
            d.packingTypeId = r.PackingTypeId;
            if (d.packingTypeId == 0) throw new Warning("PackingType Required In Row# " + idx);
            d.packUomId = r.PackUomId; d.uomCode = r.PackUom;
            if (d.packUomId == 0) throw new Warning("PackUomId Field Required In Row# " + idx);
            d.itemQty = dec(r.ItemQty);
            if (d.itemQty.signum() == 0) throw new Warning("ItemQty must be greater than zero");
            d.grossWeight = dec(r.GrossWeight); d.weightCut = dec(r.WtCut); d.weightCutTotal = dec(r.WtCutTotal);
            d.netBillWeight = dec(r.NetBillWeight);
            if (d.netBillWeight.signum() == 0) throw new Warning("NetBillWeight Field Required In Row# " + idx);
            d.netStockWeight = dec(r.StockWeight);
            if (d.netStockWeight.signum() == 0) throw new Warning("NetStockWeight Field Required In Row# " + idx);
            d.itemRate = dec(r.ItemRate);
            if (d.itemRate.signum() == 0) throw new Warning("ItemRate Field Required");
            d.rateUomScheduleId = r.RateUomId;
            if (d.saleOrderId > 0) {
                if (d.rateUomScheduleId == 0) throw new Warning("Rate UOM Required In Row# " + idx);
            } else if (d.saleOrderId == 0 && d.rateUomScheduleId == 0) {
                List<Map<String, Object>> uoms = look.uoms(r.ItemId);
                if (!uoms.isEmpty()) {
                    int want = (int) Math.rint(r.EquivalentSoRate);
                    Map<String, Object> hit = null;
                    for (Map<String, Object> x : uoms) if (toDouble(x.get("Equivalent")) == want) { hit = x; break; }
                    if (hit == null) throw new Warning("This " + r.ItemName + " Item RateUom not define please check");
                    d.rateUomScheduleId = toInt(hit.get("Id"));
                }
            }
            d.rateUom = r.RateUom;
            d.rateCut = dec(r.RateCut); d.rateCutAmount = dec(r.RateCutAmount);
            d.itemAmount = dec(r.ItemAmount);
            if (d.itemAmount.signum() == 0) throw new Warning("ItemAmount Field Required In Row# " + idx);
            d.warehouseId = r.WarehouseId;
            if (d.warehouseId == 0) throw new Warning("Warehouse Required In Row# " + idx);
            d.cityId = r.CityId;
            if (d.cityId == 0) throw new Warning("City Required In Row# " + idx);
            d.itemNetAmount = dec(r.BillAmount);
            d.freightAmount = dec(r.Freights); d.journalAmount = dec(r.Journal); d.expenseAmount = dec(r.Expense); d.commissionAmount = dec(r.Commission);
            d.gpNo = r.GpNo; d.vehicleNo = r.VehicleNo == null ? "" : r.VehicleNo;
            d.exchangeRate = dec(r.ExchangeRate); d.fcyAmount = dec(r.FcyAmount);
            d.currencyId = v.currencyId;
            d.actionTypeId = d.id <= 0 ? 1 : 2;
            d.saleOrder = 0;                                           // InvSaleInvoiceDetail.SaleOrder is never set by Insert()
            o.details.add(d);
        }
        for (Fr r : v.freight) if (r.Transporter != 0) {
            SaleStInvoiceFinancial.Frt f = new SaleStInvoiceFinancial.Frt();
            f.invGdnId = r.InvGdnId; f.tansporterId = r.Transporter; f.creditAmount = dec(r.Freight); f.debitAmount = dec(r.Debit);
            o.freights.add(f);
        }
        for (Ex r : v.exp) if (r.ItemId != 0) {
            SaleStInvoiceFinancial.Exp x = new SaleStInvoiceFinancial.Exp();
            x.invOtherItemId = r.ItemId; x.qty = dec(r.Qty); x.rate = dec(r.Rate); x.amount = dec(r.Amount); x.remarks = r.Remarks == null ? "" : r.Remarks;
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
        out.put("messages", v.messages);
        out.put("message", (update ? "Record Update Successfully" : "Record saved Successfully") + o.docNo);
        return out;
    }

    // ------------------------------------------------------------------ attachments

    public List<Map<String, Object>> attachmentList(int id) { header(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { header(id); return attachments.download(SCREEN, id, attachmentId); }
}
