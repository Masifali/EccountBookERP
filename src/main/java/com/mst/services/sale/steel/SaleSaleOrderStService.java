package com.mst.services.sale.steel;

import com.mst.models.UserAccount;
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
 * Screen 542 "SaleOrder_St" = Architecture.WinApp.Steel.Sale.SaleOrder_St (Sale Steel, module 84, document type 1505).
 *
 * Desktop map (SaleOrder_St.cs): FormValidation :369, FormValidationDetail :450, btnplus_Click :512, Insert :554, PaymentTerms :769, DeliveryTerms :787,
 * OrderStatus :806, OrderCatagoryfill :826, GenerateOrderCategoryNo :843, SupplierNameFill :876, CommissionTypeFill :893, CommissionUOMFill :913,
 * DocumentNoFill :952, ItemDetailFill :982, bindRateUomAndItemPackUom :999, combojoblotfill :1051, CmbPackingTypeFill :1067, cmbCityFill :1083,
 * CurrencyFill :1108, grd_ColumnButtonClick :1144, PurchsaeOrder_Load :1201, Reset :1290, grdSettings :1385, KeyDown :1432, HistoryCombosFill :1550,
 * ReadById :1584, gridhistoryfill :1644, CalculateWeight :1855, TotalAmount :1891, TotalCommissionAmount :1918, grd_DoubleClick :1999,
 * btnUpdateDetail_Click :2031, GridDetailBind :2111, btnprint_Click :2222, btnRefresh_Click :2239, ConfigurationDefault :2260, btnDelete_Click :2324,
 * txtExchangeRate_TextChanged :2500, CalculateTotalInformation :2550.
 *
 * Procedures: [ST].[USP_SaleOrder_GetAllMethod] (GenerateCode, ReadById, ReadBySaleOrderHeaderId, FormHistory, DeleteById), [ST].[USP_SaleOrder_Insert] / _Update,
 * [ST].[USP_SaleOrderDetail_Insert], [ST].[USP_GetDataForDropDownFromSaleOrder], Sp_InvOrderCategory_GetAllMethod, Sp_InvDueTerms_GetAllMethod,
 * Sp_MultiCurrency_GetAllMethod, SP_City_GetAllMethod, SP_CHECKSUPPLIERCUSTOMERLIMITS.
 */
@Service
public class SaleSaleOrderStService {
    public static final String SCREEN = "SaleOrder_St";
    public static final int DOC_TYPE = 1505;
    private static final String P_SO = "[ST].[USP_SaleOrder_GetAllMethod]";

    private final SaleEngrSupport sup;
    private final SaleSteelLookups look;
    private final SaleEngrAttachments attachments;

    public SaleSaleOrderStService(SaleEngrSupport sup, SaleSteelLookups look, SaleEngrAttachments attachments) {
        this.sup = sup; this.look = look; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load (PurchsaeOrder_Load :1201)

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>(lists());
        m.put("rights", sup.rights(SCREEN));
        m.put("fmt", formats());
        m.put("defaults", defaults());                                             // ConfigurationDefault
        m.put("nextNo", nextNo());                                                 // DocumentNoFill
        m.put("historyCustomers", historyCustomers());                             // HistoryCombosFill
        m.put("historyDays", look.historyDays());
        return m;
    }

    /** The combos filled by Load and by btnRefresh_Click :2239 (OrderCatagoryfill, SupplierNameFill, DeliveryTerms, PaymentTerms, CurrencyFill, ItemDetailFill, combojoblotfill, CmbPackingTypeFill, cmbCityFill). */
    public Map<String, Object> lists() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("categories", sup.rows("Sp_InvOrderCategory_GetAllMethod", "Activity", "GetAll"));
        m.put("customers", look.customers());
        m.put("deliveryTerms", List.of(row("Id", 1, "DeliveryTerm", "Load"), row("Id", 2, "DeliveryTerm", "Ponch")));
        m.put("paymentTerms", sup.rows("Sp_InvDueTerms_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll"));
        m.put("currencies", sup.rows("Sp_MultiCurrency_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadAll"));
        m.put("items", look.items());
        m.put("jobLots", look.jobLots());
        m.put("packingTypes", look.packingTypes());
        m.put("cities", sup.rows("SP_City_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll"));
        return m;
    }

    /** OrderStatus :806, CommissionTypeFill :893 and CommissionUOMFill :913 are fixed lists, filled once by Load only. */
    public Map<String, Object> statics() {
        return row("statuses", List.of(row("Id", 1, "Status", "Open"), row("Id", 2, "Status", "Complete"), row("Id", 3, "Status", "Cancel")),
                "commTypes", List.of(row("Id", 1, "CommissionType", "Flat"), row("Id", 2, "CommissionType", "Percent"), row("Id", 3, "CommissionType", "Comm Weight")),
                "commUoms", List.of(row("Id", 1, "UOM", "40"), row("Id", 2, "UOM", "50"), row("Id", 3, "UOM", "60"), row("Id", 4, "UOM", "100")));
    }

    /** clsGlobalVariables.stringFormatsingle / DefaultNoofDecimalPointsForAmount / ...ForFcyAmount / DecimalRateFormate. */
    public Map<String, Object> formats() {
        int amt = toInt(sup.config("Default NoofDecimal Points For Amount"));
        int rate = toInt(sup.config("Default NoofDecimal Points For Rate"));
        int fcy = toInt(sup.config("DefaultNoOfDecimalPointsForFcyAmount"));
        return row("amountRound", Math.max(0, Math.min(10, amt)), "amount", amt >= 1 && amt <= 4 ? amt : 0,
                "rate", rate >= 1 && rate <= 4 ? rate : (rate == 0 ? 2 : 0), "fcy", fcy >= 1 && fcy <= 4 ? fcy : 0, "fcyRound", Math.max(0, Math.min(10, fcy)));
    }

    /** ConfigurationDefault :2260 - City Area, Job/Lot, Base Currency, BaseCurrencyRate (ToInt). */
    public Map<String, Object> defaults() {
        return row("cityId", sup.configInt("City Area"), "jobLotId", sup.configInt("Job/Lot"), "currencyId", sup.configInt("Base Currency"),
                "exchangeRate", toInt(sup.config("BaseCurrencyRate")));
    }

    /** DocumentNoFill :952 - SaleOrder.GenerateCodeSaleOrder. */
    public int nextNo() {
        Map<String, Object> p = new LinkedHashMap<>();
        UserAccount u = sup.user();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId()); p.put("BranchesId", u.getBranchesId());
        p.put("DocumentTypeId", DOC_TYPE); p.put("FinancialYearId", sup.fy()); p.put("Activity", "GenerateCode");
        List<Map<String, Object>> r = com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), P_SO, p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** GenerateOrderCategoryNo :843 - InvOrderCategory.GenerateSaleOrderCategoryCodebyId (0 when no row; the screen keeps its old text then). */
    public int categoryNo(int categoryId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("OrderCatagoryId", categoryId);
        if (sup.fy() != 0) p.put("FinancialYearId", sup.fy());
        p.put("Activity", "GenerateSaleOrderCategoryCodeById");
        List<Map<String, Object>> r = com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "Sp_InvOrderCategory_GetAllMethod", p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("CatagorySrNo"));
    }

    /** bindRateUomAndItemPackUom :999 - CommonServices.GetUomScheduleByItemId. */
    public List<Map<String, Object>> uoms(int itemId) { return look.uoms(itemId); }

    /** HistoryCombosFill :1550 - ST.USP_GetDataForDropDownFromSaleOrder (no Activity), kept to the 'Customer' rows (Id, ReferenceName -> Id, Customer). */
    public List<Map<String, Object>> historyCustomers() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("[ST].[USP_GetDataForDropDownFromSaleOrder]", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            if ("Customer".equals(str(r.get("Activity")))) out.add(row("Id", r.get("Id"), "Customer", r.get("ReferenceName")));
        return out;
    }

    /** SaleOrder.CHECKSUPPLIERCUSTOMERLIMITS (ActionId 2). */
    public double limit(int customerId, double amount) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("SupplierCustomerId", customerId); p.put("Amount", amount); p.put("ActionId", 2);
        List<Map<String, Object>> r = com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "SP_CHECKSUPPLIERCUSTOMERLIMITS", p);
        return r.isEmpty() ? 0.0 : toDouble(r.get(0).get("AvailableLimit"));
    }

    // ------------------------------------------------------------------ History (gridhistoryfill :1644)

    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate, int fromNo, int toNo, int customerId) {
        boolean viewAll = Boolean.TRUE.equals(sup.rights(SCREEN).get("viewAll"));
        UserAccount u = sup.user();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId()); p.put("BranchesId", u.getBranchesId());
        p.put("DocumentTypeId", DOC_TYPE); p.put("CanViewAllRecord", viewAll); p.put("FinancialYearId", sup.fy());
        if (!viewAll) p.put("EntryUser", u.getId());
        String fk = "FromDate", tk = "ToDate";
        if ("entry".equals(dateType)) { fk = "EntryFromDate"; tk = "EntryToDate"; }
        else if ("modify".equals(dateType)) { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
        else if ("approved".equals(dateType)) { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
        Timestamp f = ts(fromDate), t = ts(toDate);
        if (f != null) p.put(fk, f);
        if (t != null) p.put(tk, t);
        if (fromNo != 0) p.put("DocNoFrom", (double) fromNo);
        if (toNo != 0) p.put("DocNoTo", (double) toNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        p.put("Activity", "FormHistory");
        List<Map<String, Object>> src = com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), P_SO, p);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {                                          // the dt of gridhistoryfill, in its column order
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id")); o.put("DocNo", r.get("DocNo")); o.put("DocDate", r.get("DocDate")); o.put("OrderCategory", r.get("OrderCategoryDescription"));
            o.put("PartyName", r.get("SupplierName")); o.put("CommAgent", r.get("CommissionAgent")); o.put("CommType", r.get("CommissionType"));
            o.put("CommRate", r.get("CommRate")); o.put("CommAmount", r.get("CommAmount")); o.put("CommRemarks", r.get("CommRemarks"));
            o.put("OrderStatus", r.get("OrderStatus")); o.put("DeliveryTerm", r.get("DeliveryTerm")); o.put("PaymentTerm", r.get("TermsDescription"));
            o.put("DueDays", r.get("DueDays")); o.put("DueDate", r.get("DueDate")); o.put("ExpiryDate", r.get("OrderExpiryDate")); o.put("DeliveryDays", r.get("DeliveryDays"));
            o.put("DeliveryStartDate", r.get("DeliveryStartDate")); o.put("OrderQty", r.get("OrderQty")); o.put("OrderWeight", r.get("OrderWeight"));
            o.put("OrderAmount", r.get("OrderAmount")); o.put("CurrencyName", r.get("CurrencyName")); o.put("ExchangeRate", r.get("ExchangeRate"));
            o.put("FcyAmount", r.get("FcyAmount")); o.put("EntryUser", r.get("UserName")); o.put("EntryDate", r.get("EntryDate"));
            o.put("ModifyUser", r.get("ModifyUserName")); o.put("ModifyDate", r.get("ModifyDate"));
            o.put("IsApproved", toBool(r.get("IsApproved")) ? "Approved" : "Not Approved");
            o.put("ApprovedUser", r.get("ApprovedUserName")); o.put("ApprovedDate", r.get("ApprovedDate"));
            o.put("Remarks", r.get("RemarksHeader")); o.put("NoOfAttachments", r.get("NoOfAttachments"));
            out.add(o);
        }
        return out;
    }

    // ------------------------------------------------------------------ ReadById :1584 / SaleOrder.GetByID

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> r = sup.rows(P_SO, "Id", id, "Activity", "ReadById");
        UserAccount u = sup.user();
        if (r.isEmpty() || toInt(r.get(0).get("OrganizationId")) != u.getOrganizationId() || toInt(r.get(0).get("CompanyId")) != u.getCompanyId()
                || toInt(r.get(0).get("DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Sale order not found in this company");
        return r.get(0);
    }

    public List<Map<String, Object>> lines(int id) {
        header(id);
        return sup.rows(P_SO, "Id", id, "Activity", "ReadBySaleOrderHeaderId");
    }

    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", h);
        out.put("lines", sup.rows(P_SO, "Id", id, "Activity", "ReadBySaleOrderHeaderId"));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    // ------------------------------------------------------------------ Save (Insert :554)

    public static class Line {
        public int id, itemId, jobLotId, packingTypeId, packUomId, rateUomId, cityId;
        public double itemQty, weight, itemRate, itemAmount, exchangeRate, fcyAmount;
        public String remarks;
    }

    public static class Request {
        public int id;
        public String docDate, docNo;
        public int orderCategoryId;
        public String catSrNo;
        public int customerId, refPartyId, paymentTermId;
        public String partyReference, remarks;
        public String dueDays, dueDate, expiryDate, deliveryTerm, deliveryStartDate, deliveryDays, orderStatus;
        public int currencyId;
        public double orderQty, orderWeight, orderAmount, exchangeRate, fcyAmount;
        public boolean hasAgent;
        public int commAgentId;
        public String commType, commRateUom, commRemarks;
        public double commRate, commAmount;
        public List<Line> lines = new ArrayList<>();
        public List<Line> removed = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    /** FormValidation :369 - the first failing message, word for word. */
    private void formValidation(Request r) {
        String no = text(r.docNo);
        if (no.isEmpty() || "0".equals(no)) throw new Warning("Doc No Field is Required");
        if (r.orderCategoryId <= 0) throw new Warning("Order Catagory Field is Required");
        if (r.customerId <= 0) throw new Warning("Customer Name Field is Required");
        if (text(r.deliveryTerm).isEmpty()) throw new Warning("Delivery Term Field is Required");
        if (r.paymentTermId <= 0) throw new Warning("Payment Term Field is Required");
        String dd = text(r.dueDays);
        if (r.paymentTermId == 2 && (dd.isEmpty() || "0".equals(dd))) throw new Warning("Due Days Field is Required");
        if (text(r.orderStatus).isEmpty()) throw new Warning("Status Field is Required");
        if (!(r.orderAmount != 0.0)) throw new Warning("Order Amount Should Greater Than 0");
        if (r.currencyId <= 0) throw new Warning("Currency Field is Required");
        if (r.exchangeRate == 0.0) throw new Warning("Exchange Rate Field is Required");
        if (r.commAmount > 0.0 && (!r.hasAgent || r.commAgentId <= 0)) throw new Warning("Sales Man Agent Is Requird");
        if (r.hasAgent && r.commAgentId > 0 && r.commAmount == 0.0) throw new Warning("Commission Amount Is Requird");
        if (!(r.fcyAmount != 0.0)) throw new Warning("Fcy Amount Should Greater Than 0");
    }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        formValidation(r);
        List<Line> all = r.lines == null ? List.of() : r.lines;
        if (all.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        List<Line> keep = new ArrayList<>();
        for (Line l : all) if (l.itemQty > 0.0) keep.add(l);                         // only the rows with ItemQty > 0 are written
        for (Line l : keep) if (!(l.itemRate > 0.0)) throw new IllegalArgumentException("Rate Field Required");
        if (keep.isEmpty()) throw new IllegalArgumentException("At least enter value/quantity in one of the rows");

        Set<Integer> savedIds = new HashSet<>();
        if (r.id > 0) {
            header(r.id);
            for (Map<String, Object> d : sup.rows(P_SO, "Id", r.id, "Activity", "ReadBySaleOrderHeaderId")) savedIds.add(toInt(d.get("Id")));
            for (Line l : keep) if (l.id > 0 && !savedIds.contains(l.id)) throw new IllegalArgumentException("Invalid detail row for this sale order");
        }
        List<Line> removed = r.removed == null ? List.of() : r.removed;
        if (r.id > 0) for (Line l : removed) if (l.id <= 0 || !savedIds.contains(l.id)) throw new IllegalArgumentException("Deleted detail row does not belong to this sale order");

        Timestamp now = now();
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", r.id);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DocNo", toInt(r.docNo));
        h.put("DocDate", ts(r.docDate));
        h.put("OrderCatagoryId", r.orderCategoryId);
        h.put("CatagorySrNo", toInt(r.catSrNo));
        h.put("OrderSupCustId", r.customerId);
        h.put("PartyReference", text(r.partyReference));
        h.put("RefrenencePartyId", r.refPartyId);
        h.put("CommissionAgentId", r.hasAgent ? r.commAgentId : 0);
        if (r.hasAgent) h.put("CommissionType", text(r.commType));
        h.put("CommRate", r.hasAgent ? dec(r.commRate) : BigDecimal.ZERO);
        h.put("CommRateUom", r.hasAgent ? toInt(r.commRateUom) : 0);
        h.put("CommAmount", r.hasAgent ? dec(r.commAmount) : BigDecimal.ZERO);
        if (r.hasAgent) h.put("CommRemarks", text(r.commRemarks));
        h.put("OrderQty", dec(r.orderQty));
        h.put("OrderWeight", dec(r.orderWeight));
        h.put("OrderAmount", dec(r.orderAmount));
        h.put("PaymentTermsId", r.paymentTermId);
        h.put("OrderDueDays", toInt(r.dueDays));
        h.put("OrderDueDate", ts(r.dueDate));
        h.put("OrderExpiryDate", ts(r.expiryDate));
        h.put("DeliveryTerm", text(r.deliveryTerm));
        h.put("DeliveryStartDate", ts(r.deliveryStartDate));
        h.put("DeliveryDays", toInt(r.deliveryDays));
        h.put("IsOrderTaxable", false);
        h.put("RemarksHeader", text(r.remarks));
        h.put("OrderStatus", text(r.orderStatus));
        h.put("IsApproved", false);
        h.put("EntryDate", now);
        h.put("EntryUserId", u.getId());
        h.put("ModifyDate", now);
        h.put("ModifyUserId", u.getId());
        h.put("ApprovedDate", now);
        h.put("ApprovedUserId", 0);
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchesId", u.getBranchesId());
        h.put("ProjectsId", 0);
        h.put("ActionId", r.id > 0 ? 2 : 1);
        h.put("FinancialYearId", sup.fy());
        h.put("PendingForView", 0);
        h.put("CurrencyId", r.currencyId);
        h.put("ExchangeRate", dec(r.exchangeRate));
        h.put("FcyAmount", dec(r.fcyAmount));
        int num = sup.setProcMap(r.id > 0 ? "[ST].[USP_SaleOrder_Update]" : "[ST].[USP_SaleOrder_Insert]", h);
        int id = num > 0 ? num : r.id;
        if (id <= 0) throw new IllegalStateException("The sale order could not be saved.");

        if (r.id > 0) for (Line l : removed) sup.setProcMap("[ST].[USP_SaleOrderDetail_Insert]", detail(id, l, 3, r.currencyId));
        for (Line l : keep) {
            int action = r.id == 0 ? 1 : (l.id > 0 ? 2 : 1);
            sup.setProcMap("[ST].[USP_SaleOrderDetail_Insert]", detail(id, l, action, r.currencyId));
        }
        attachments.apply(SCREEN, DOC_TYPE, id, r.customerId, r.attachments);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", toInt(r.docNo));
        out.put("updated", r.id > 0);
        out.put("message", (r.id > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + toInt(r.docNo));
        return out;
    }

    /** GenericProvider.SetProc(SaleOrderDetail): the procedure's 25 parameters. */
    private Map<String, Object> detail(int headId, Line l, int action, int currencyId) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", action == 1 ? 0 : l.id);
        d.put("SaleOrderId", headId);
        d.put("OrderItemId", l.itemId);
        d.put("OrderItemUOMId", l.packUomId);
        d.put("OrderItemQty", dec(l.itemQty));
        d.put("NetWeight", dec(l.weight));
        d.put("OrderItemRate", dec(l.itemRate));
        d.put("OrderItemRateUOMId", l.rateUomId);
        d.put("ItemAmount", dec(l.itemAmount));
        d.put("IsOrderTaxable", false);
        d.put("TaxNameId", 0);
        d.put("TaxPercent", BigDecimal.ZERO);
        d.put("TaxAmount", BigDecimal.ZERO);
        d.put("ItemNetAmount", dec(l.itemAmount));
        d.put("CurrencyId", currencyId);
        d.put("ExchangeRate", dec(l.exchangeRate));
        d.put("FcyAmount", dec(l.fcyAmount));
        d.put("PackingTypeId", l.packingTypeId);
        d.put("JobLotId", l.jobLotId);
        d.put("CityId", l.cityId);
        d.put("InvLabSampleAnalysisHeaderId", 0);
        d.put("RemarkDetail", text(l.remarks));
        d.put("ActionTypeId", action);
        d.put("IsApproved", false);
        return d;
    }

    // ------------------------------------------------------------------ Delete (btnDelete_Click :2324)

    @Transactional
    public Map<String, Object> delete(int id) {
        if (id <= 0) throw new IllegalArgumentException("No Record Found For Delete");
        if (!Boolean.TRUE.equals(sup.rights(SCREEN).get("delete"))) throw new ResponseStatusException(HttpStatus.FORBIDDEN, "You do not have the delete right on this screen");
        header(id);
        sup.rows(P_SO, "Id", id, "EntryUserId", sup.user().getId(), "Activity", "DeleteById");
        return row("message", "Record Deleted Seccessfully");
    }

    // ------------------------------------------------------------------ attachments

    public List<Map<String, Object>> attachmentList(int id) { header(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { header(id); return attachments.download(SCREEN, id, attachmentId); }

    // ------------------------------------------------------------------ helpers

    private static BigDecimal dec(double v) { return new BigDecimal(v, new MathContext(15, RoundingMode.HALF_EVEN)); }

    private static Timestamp ts(String s) {
        if (s == null || s.isBlank()) return null;
        try { return Timestamp.valueOf(LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))).atStartOfDay()); } catch (RuntimeException e) { return null; }
    }
}
