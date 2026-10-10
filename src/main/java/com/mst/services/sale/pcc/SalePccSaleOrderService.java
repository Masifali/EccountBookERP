package com.mst.services.sale.pcc;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleEngrSupport;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 548 "SaleOrderConcrete" = Architecture.WinApp.pcc.Sale.SaleOrderConcrete (Sale Pcc, module 85, document type 1852).
 *
 * Desktop map (SaleOrderConcrete.cs, logic lines before InitializeComponent at 5241):
 *   PurchsaeOrder_Load :281, DocumentNoFill :440, TransporterAcFill :462, OtherWagesAcFill :490, SupplierNameFill :519, CommAgentNameFill :560,
 *   VisitedByFill :618, BuildingHeightFill :656, BuildingStoreysFill :689, ReferencePartyFill :722, RefSalesManFill :760, PaymentTerms :799,
 *   DeliveryTerms :833, CommissionTypeFill :863, ItemDetailFill :907, bindvarientunit :960, combojoblotfill :1002, cmbCityFill :1032,
 *   DiscountTypefill :1066, dtForGridComboFill :1096, ConfigurationDefault :1111, combitem_Leave :1152, FormValidation :1259,
 *   FormValidationDetail :1353, CalculateTotalInformation :1477, CurrencyFill :1565, cmbCurrency_Leave :1598, MultiCurrencyFeature :1634,
 *   txtExchangeRate_TextChanged :1673, Reset :1727, btnRefresh_Click :1834, btnplus_Click :1870, btnUpdateDetail_Click :1891, grd_DoubleClick :1937,
 *   grdSettings :1978, grd_ColumnButtonClick :2088, WagesItemFillFromDetail :2199, grdContractorWagesGridSetting :2219, btnprint_Click :2328,
 *   Insert :2339, ReadById :2656, btnDelete_Click :2765, CalculateDetailAmount :2782, CalculateDiscountAndTotalAmount :2811,
 *   TotalCommissionAmount :2967, NetRateCalculation :3115, UpdateDiscountTypeAndRateInGrid :3154, CalculateWithManualOrderAmountTotal :3387,
 *   PropotionateDiscountByManualOrderAmountInGrid :3432, DueDateGenerate :3565, GetDataFromSaleOrdersForComboBind :3605, gridhistoryfill :3724,
 *   HistoryGridSettings :3820, GridDetailBind :3977, KeyDown :4324, grdContractorWages_KeyDown :4483 (F1 pickers), grd_KeyDown :4610.
 * BLL: Architecture.BLL.pcc.SaleOrder (Save: Id 0 -> [pcc].[USP_SaleOrder_Insert] / ActionId 1, else [pcc].[USP_SaleOrder_Update] / ActionId 2;
 *   GenerateCode, GetByID, FormHistory, DeleteById, GetDataForDropDownFromSaleOrder, SaleOrderSlipAndRegister),
 *   DAL.pcc.SaleOrder.SetData: header, [pcc].[USP_SaleOrderDetail_Insert] per detail (live rows then removed rows), [pcc].[USP_SaleOrderWagesDetail_Insert]
 *   per wages row, attachments, all in ONE transaction.
 */
@Service
public class SalePccSaleOrderService {
    public static final String SCREEN = "SaleOrderConcrete";
    public static final int SCREEN_ID = 548;
    public static final int DOC_TYPE = 1852;
    private static final String GET = "[pcc].[USP_SaleOrder_GetAllMethod]";

    private final SaleEngrSupport sup;
    private final SalePccLookups lk;
    private final SalePccP2Support p2;
    private final SaleEngrAttachments attachments;

    public SalePccSaleOrderService(SaleEngrSupport sup, SalePccLookups lk, SalePccP2Support p2, SaleEngrAttachments attachments) {
        this.sup = sup; this.lk = lk; this.p2 = p2; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load / Refresh

    public Map<String, Object> initial() {
        lk.requireView(SCREEN_ID);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());                                                     // DocumentNoFill
        m.putAll(lists(null));
        m.put("history", historyCombos());                                              // GetDataFromSaleOrdersForComboBind
        m.put("search", p2.searchConfig());                                              // ItemSearchByCode / SupplierCustomerDefaultFilterByPartyCode
        m.put("fmt", p2.fmt());
        m.put("multiCurrency", sup.erpFeature(6));                                       // MultiCurrencyFeature
        m.put("subsidiary", sup.erpFeature(4));
        return m;
    }

    /** The combos of PurchsaeOrder_Load / btnRefresh_Click, in the Refresh order. */
    public Map<String, Object> lists(String docDate) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("refParties", lk.referenceParties());                                      // ReferencePartyFill
        m.put("storeys", lk.buildingLookups(2));                                         // BuildingStoreysFill
        m.put("heights", lk.buildingLookups(1));                                         // BuildingHeightFill
        m.put("visitedBy", p2.referenceParties(2, 0));                                   // VisitedByFill
        m.put("customers", lk.customers());                                              // SupplierNameFill
        m.put("agents", p2.commissionAgents());                                          // CommAgentNameFill
        m.put("items", p2.pricedItems(docDate));                                         // ItemDetailFill
        m.put("jobLots", lk.jobLots());                                                  // combojoblotfill
        m.put("cities", lk.cities());                                                    // cmbCityFill
        m.put("freightAccounts", p2.accountTitles("10,3"));                              // TransporterAcFill
        m.put("otherWagesAccounts", p2.accountTitles("10"));                             // OtherWagesAcFill
        m.put("terms", p2.dueTerms());                                                   // PaymentTerms
        m.put("currencies", p2.currencies());                                            // CurrencyFill
        m.put("contractors", p2.contractors());                                          // dtForGridComboFill
        m.put("defaults", p2.defaults());                                                // ConfigurationDefault
        return m;
    }

    public List<Map<String, Object>> items(String docDate) { return p2.pricedItems(docDate); }

    public List<Map<String, Object>> varients(int itemId) { return lk.varients(itemId); }

    /** RefSalesManFill: reference parties of type 3 for the selected commission agent. */
    public List<Map<String, Object>> refSalesMen(int agentId) { return p2.referenceParties(3, agentId); }

    public int supplierGl(int glAccountId) { return glAccountId > 0 ? p2.supplierGlId(glAccountId) : 0; }

    public List<Map<String, Object>> lastRate(int currencyId) { return p2.lastRate(currencyId, "1852"); }

    public List<Map<String, Object>> serviceActivities(int itemId, String date) { return p2.serviceActivities(itemId, date); }

    /** SaleOrder.GenerateCode. */
    public int nextNo() {
        List<Map<String, Object>> r = sup.rows(GET, "OrganizationId", sup.org(), "CompanyId", sup.company(), "BranchesId", sup.branch(),
                "DocumentTypeId", DOC_TYPE, "FinancialYearId", sup.fy(), "Activity", "GenerateCode");
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // ------------------------------------------------------------------ History

    /** GetDataForDropDownFromSaleOrder: Customer (Id, ReferenceName, OtherDescription) and Item (Id, ReferenceName) rows. */
    public Map<String, Object> historyCombos() {
        List<Map<String, Object>> cust = new ArrayList<>(), item = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("[pcc].[USP_DropDownFillFromSaleOrder]", "OrganizationId", sup.org(), "CompanyId", sup.company())) {
            String a = str(r.get("Activity"));
            if ("Customer".equals(a)) cust.add(row("Id", r.get("Id"), "PartyName", r.get("ReferenceName"), "PartyCode", r.get("OtherDescription")));
            else if ("Item".equals(a)) item.add(row("Id", r.get("Id"), "ItemName", r.get("ReferenceName")));
        }
        return row("customers", cust, "items", item);
    }

    /** gridhistoryfill: SaleOrder.FormHistory, then the dtOrders column mapping. */
    public List<Map<String, Object>> history(String fromDate, String toDate, int customerId, String status, String approved) {
        Map<String, Boolean> rt = sup.rights(SCREEN);
        boolean all = Boolean.TRUE.equals(rt.get("viewAll"));
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("BranchesId", sup.branch());
        p.put("DocumentTypeId", DOC_TYPE); p.put("CanViewAllRecord", all); p.put("FinancialYearId", sup.fy());
        LocalDate f = SalePccLookups.parseDate(fromDate), t = SalePccLookups.parseDate(toDate);
        if (f != null) p.put("FromDate", f);
        if (t != null) p.put("ToDate", t);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);              // CmbItemHistory is read into ReportsParameters.ItemId but the BLL never sends it
        if (!all) p.put("EntryUserId", sup.userId());
        if (status != null && !status.isEmpty()) p.put("Status", status);
        if (!"All".equals(approved)) p.put("IsApproved", "Approve".equals(approved));
        p.put("Activity", "FormHistory");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(sup.jdbc(), GET, p)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id")); o.put("DocumentTypeId", r.get("DocumentTypeId")); o.put("DocNo", r.get("DocNo")); o.put("DocDate", r.get("DocDate"));
            o.put("SupplierCustomerId", r.get("SupplierCustomerId")); o.put("CustomerName", r.get("SupplierName")); o.put("CustomerCode", r.get("PartyCode"));
            o.put("RefrenenceNo", r.get("RefrenenceNo")); o.put("OrderStatus", r.get("OrderStatus")); o.put("RemarksHeader", r.get("RemarksHeader"));
            o.put("VisitedByName", r.get("VisitedByName")); o.put("ReferencePartyName", r.get("ReferencPartyName"));
            o.put("ReferencePartyCellNo", r.get("ReferencPartyCellNo")); o.put("ReferencePartyAddress", r.get("ReferencPartyAddress"));
            o.put("CommissionAgent", r.get("CommissionAgent")); o.put("CommissionRate", r.get("CommissionRate")); o.put("CommissionAmount", r.get("CommissionAmount"));
            o.put("PaymentTerm", r.get("PaymentTerm")); o.put("OrderDueDays", r.get("OrderDueDays")); o.put("OrderDueDate", r.get("OrderDueDate"));
            o.put("OrderExpiryDate", r.get("OrderExpiryDate")); o.put("DeliveryTerm", r.get("DeliveryTerm")); o.put("DeliveryStartDate", r.get("DeliveryStartDate"));
            o.put("DeliveryDays", r.get("DeliveryDays")); o.put("EntryDate", r.get("EntryDate")); o.put("EntryUserName", r.get("EntryUserName"));
            o.put("ModifyDate", r.get("ModifyDate")); o.put("ModifyUserName", r.get("ModifyUserName")); o.put("OrderQty", r.get("OrderQty"));
            o.put("OrderWeight", r.get("OrderWeight")); o.put("OrderItemAmount", r.get("OrderItemAmount")); o.put("OrderDiscountAmount", r.get("OrderDiscountAmount"));
            o.put("OrderItemNetAmount", r.get("OrderItemNetAmount")); o.put("FrieghtAmount", r.get("FrieghtAmount")); o.put("WageNetAmount", r.get("WageNetAmount"));
            o.put("OtherWagesAmount", r.get("OtherWages")); o.put("OrderAmount", r.get("OrderAmount")); o.put("NoOfAttachments", r.get("NoOfAttachments"));
            o.put("Distance", r.get("Distance"));
            out.add(o);
        }
        return out;
    }

    // ------------------------------------------------------------------ ReadById (GetByID)

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> r = sup.rows(GET, "Id", id, "Activity", "ReadById");
        UserAccount u = sup.user();
        if (r.isEmpty() || toInt(r.get(0).get("OrganizationId")) != u.getOrganizationId() || toInt(r.get(0).get("CompanyId")) != u.getCompanyId()
                || toInt(r.get(0).get("DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Sale order not found in this company");
        return r.get(0);
    }

    private List<Map<String, Object>> details(int id) { return sup.rows(GET, "Id", id, "Activity", "ReadBySaleOrderId_SaleOrderDetail"); }

    private List<Map<String, Object>> wages(int id) { return sup.rows(GET, "Id", id, "Activity", "ReadBySaleOrderId_SaleOrderWagesDetail"); }

    public Map<String, Object> record(int id) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", header(id));
        out.put("lines", details(id));
        out.put("wages", wages(id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    // ------------------------------------------------------------------ Save (Insert)

    public static class Line {
        public int id, itemId, scheduleId, attributeVarientId, discountId, jobLotId, cityId;
        public double varientUnit, qty, weight, netWeight, itemPrice, addLessRate, netRate, amount, discRate, discAmount, totalAmount, fcyAmount;
        public String remarks;
        public boolean commOnSale;
    }

    public static class Wage {
        public int id, itemId, contractorId, contractorWagesRateScheduleId;
        public double qty, rate, amount;
        public String remarks;
        public String amountText;
    }

    public static class Request {
        public int id;
        public String docDate, docNo, refNo, remarks, distance, buildingArea, referencePartyName, referencePartyAddress, referencePartyCellNo;
        public String dueDays, dueDate, deliveryDays, deliveryStartDate, commissionType, commissionRemarks;
        public int supplierCustomerId, visitedById, buildingHeightId, buildingStoreyId, referencePartyId, paymentTermId, deliveryTermId, refSalesManId;
        public int otherWagesAccountId, freightAccountId, currencyId, commissionAgentId;
        public double exchangeRate, fcyAmount, orderQty, orderWeight, itemAmount, adjustDisc, discount, itemNet, wageNet, otherWages, freight, orderAmount;
        public double commissionRate, commissionAmount;
        public List<Line> lines = new ArrayList<>(), removed = new ArrayList<>();
        public List<Wage> wages = new ArrayList<>();
        public boolean ignoreLimit;
        public SaleEngrAttachments.Change attachments;
    }

    private static boolean blank(String s) { return s == null || s.trim().isEmpty(); }

    private static String plain(double v) { return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString(); }

    /** FormValidation(), word for word and in the desktop's order. */
    private void formValidation(Request r) {
        if (blank(r.docNo) || "0".equals(r.docNo.trim())) throw new Warning("Doc No Field is Required");
        if (r.supplierCustomerId == 0) throw new Warning("Customer Name Field is Required");
        if (r.visitedById == 0) throw new Warning("Visitedby Name Field is Required");
        if (r.paymentTermId == 0) throw new Warning("Payment Term Field is Required");
        String term = "";
        for (Map<String, Object> t : p2.dueTerms()) if (toInt(t.get("Id")) == r.paymentTermId) term = str(t.get("TermsDescription"));
        if ("Credit".equals(term) && blank(r.dueDays)) throw new Warning("Due Days Field is Required");
        if (r.deliveryTermId == 0) throw new Warning("Delivery Term Field is Required");
        if (blank(r.referencePartyName)) throw new Warning("Ref Party Name Field is Required");
        if (blank(r.referencePartyAddress)) throw new Warning("Ref Party Address Field is Required");
        if (blank(r.referencePartyCellNo)) throw new Warning("Ref Party CellNo Field is Required");
        if (sup.erpFeature(6)) {
            if (r.currencyId == 0) throw new Warning("Fcy Code Field is Required");
            if (r.exchangeRate == 0) throw new Warning("Exchange Rate Field is Required");
            if (r.fcyAmount == 0) throw new Warning("Fcy Amount Rate Field is Required");
        } else {
            if (r.currencyId == 0) throw new Warning("Please Configure Your Base Currency In configurations");
            if (r.exchangeRate == 0) throw new Warning("Please Configure Your Base Currency Rate In configurations");
        }
    }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        lk.need(sup.rights(SCREEN), r.id > 0 ? "update" : "save", r.id > 0 ? "Update" : "Save");
        List<Line> lines = r.lines == null ? List.of() : r.lines;
        if (lines.isEmpty()) throw new IllegalArgumentException("Grid Record not found");
        formValidation(r);
        if (r.freight > 0 && r.freightAccountId == 0) throw new IllegalArgumentException("Frieght Account field Required");
        if (r.orderAmount == 0) throw new IllegalArgumentException("OrderAmount Required");

        Set<Integer> savedIds = new HashSet<>();
        if (r.id > 0) {
            header(r.id);
            for (Map<String, Object> d : details(r.id)) savedIds.add(toInt(d.get("Id")));
        }
        LocalDate docDate = SalePccLookups.parseDate(r.docDate);
        if (docDate == null) throw new IllegalArgumentException("Doc Date Field is Required");
        String deliveryTerm = r.deliveryTermId == 1 ? "Factory Loading" : r.deliveryTermId == 2 ? "Delivery" : "";

        double billAmount = 0.0;
        if (r.commissionAgentId > 0 && r.commissionAgentId == r.supplierCustomerId) billAmount += r.commissionAmount;
        List<Line> live = new ArrayList<>();
        int rowIndex = -1;
        for (Line l : lines) {
            rowIndex++;
            if (!(l.qty > 0)) continue;
            if (r.id > 0 && l.id > 0 && !savedIds.contains(l.id)) throw new IllegalArgumentException("Invalid detail row for this sale order");
            if (!(l.netRate > 0)) throw new IllegalArgumentException("Rate Field Required in detail and row no: " + rowIndex);
            billAmount += l.totalAmount;
            live.add(l);
        }
        if (live.isEmpty()) throw new IllegalArgumentException("At least enter value/quantity in one of the rows of detail ");
        List<Line> removed = r.removed == null ? List.of() : r.removed;
        for (Line l : removed)
            if (l.id <= 0 || !savedIds.contains(l.id)) throw new IllegalArgumentException("Deleted detail row does not belong to this sale order");

        List<Wage> wl = new ArrayList<>();
        rowIndex = -1;
        for (Wage w : r.wages == null ? List.<Wage>of() : r.wages) {
            rowIndex++;
            if (w.contractorWagesRateScheduleId <= 0) continue;
            if (w.itemId == 0) throw new Warning("Item  Field Required in Grid Contractor Wages Row No: " + (rowIndex + 1));
            if (w.contractorId == 0) throw new Warning("Contractor  Field Required in Grid Contractor Wages Row No: " + (rowIndex + 1));
            if (w.amount == 0) throw new Warning("Amount Field Required in Grid Contractor Wages Row No: " + (rowIndex + 1));
            wl.add(w);
        }

        // SaleOrder.CHECKSUPPLIERCUSTOMERLIMITS (ActionId 2): Yes / No on the desktop
        if (!r.ignoreLimit) {
            double limit = p2.availableLimit(r.supplierCustomerId, billAmount);
            if (limit > 0.0) return row("limitWarning", "Debit Limit of Customer Exceeds Define Debit Limit & remaining limit value is   " + plain(limit));
            if (limit < 0.0) return row("limitWarning", "Debit Limit of Customer already Exceeds Define Debit Limit");
        }

        Timestamp now = now();
        LocalDate due = SalePccLookups.parseDate(r.dueDate), start = SalePccLookups.parseDate(r.deliveryStartDate);
        if (due == null) due = docDate;
        if (start == null) start = LocalDate.now();
        int delDays = toInt(r.deliveryDays);
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", r.id);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DocNo", toInt(r.docNo));
        h.put("DocDate", docDate);
        h.put("SupplierCustomerId", r.supplierCustomerId);
        h.put("RefrenenceNo", text(r.refNo));
        h.put("VisitedById", r.visitedById);
        h.put("RemarksHeader", text(r.remarks));
        h.put("OrderStatus", "Open");
        h.put("CommissionAgentId", r.commissionAgentId > 0 ? r.commissionAgentId : 0);
        if (r.commissionAgentId > 0) {
            h.put("CommissionType", text(r.commissionType));
            h.put("CommissionRate", dec(r.commissionRate));
            h.put("CommissionAmount", dec(r.commissionAmount));
            h.put("CommissionRemarks", text(r.commissionRemarks));
        } else {
            h.put("CommissionRate", BigDecimal.ZERO);
            h.put("CommissionAmount", BigDecimal.ZERO);
        }
        h.put("CommissionUom", BigDecimal.ZERO);
        h.put("PaymentTermsId", r.paymentTermId);
        h.put("OrderDueDays", toInt(r.dueDays));
        h.put("OrderDueDate", due);
        h.put("OrderExpiryDate", start.plusDays(delDays));
        h.put("DeliveryTerm", deliveryTerm);
        h.put("DeliveryStartDate", start);
        h.put("DeliveryDays", delDays);
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
        h.put("ProjectsId", u.getBranchesId());                                        // po.ProjectsId = UserAccount.BranchesId (desktop)
        h.put("FinancialYearId", sup.fy());
        h.put("ActionId", r.id > 0 ? 2 : 1);
        h.put("CurrencyId", r.currencyId);
        h.put("ExchangeRate", dec(r.exchangeRate));
        h.put("FcyAmount", dec(r.fcyAmount));
        h.put("OrderQty", dec(r.orderQty));
        h.put("OrderWeight", dec(r.orderWeight));
        h.put("OrderItemAmount", dec(r.itemAmount));
        h.put("OrderDiscountAmount", dec(r.discount));
        h.put("OrderItemNetAmount", dec(r.itemNet));
        h.put("FrieghtAmount", dec(r.freight));
        h.put("WageNetAmount", dec(r.wageNet));
        h.put("OrderAmount", dec(r.orderAmount));
        h.put("TransporterId", r.freightAccountId);
        h.put("ReferencPartyName", text(r.referencePartyName));
        h.put("ReferencPartyAddress", text(r.referencePartyAddress));
        h.put("ReferencPartyCellNo", text(r.referencePartyCellNo));
        h.put("Distance", text(r.distance));
        h.put("OtherWages", dec(r.otherWages));
        h.put("OtherWagesAccountId", r.otherWagesAccountId);
        h.put("RefSalesManId", r.refSalesManId);
        h.put("AdjustDiscountAmount", dec(r.adjustDisc));
        h.put("ReferencePartyId", r.referencePartyId);
        h.put("BuildingHeightId", r.buildingHeightId);
        h.put("BuildingStoreyId", r.buildingStoreyId);
        h.put("BuildingArea", text(r.buildingArea));
        int num = sup.setProcMap(r.id == 0 ? "[pcc].[USP_SaleOrder_Insert]" : "[pcc].[USP_SaleOrder_Update]", h);
        int id = num > 0 ? num : r.id;
        if (id <= 0) throw new IllegalStateException("The sale order could not be saved.");

        for (Line l : live) sup.setProcMap("[pcc].[USP_SaleOrderDetail_Insert]", detail(id, l, r, r.id != 0 ? l.id : 0, (r.id != 0 && l.id > 0) ? 2 : 1, true));
        for (Line l : removed) sup.setProcMap("[pcc].[USP_SaleOrderDetail_Insert]", detail(id, l, r, l.id, 3, false));
        for (Wage w : wl) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Amount", dec(w.amount)); d.put("Qty", dec(w.qty)); d.put("Rate", dec(w.rate)); d.put("ContractorId", w.contractorId);
            d.put("ContractorWagesRateScheduleId", w.contractorWagesRateScheduleId); d.put("Id", w.id); d.put("ItemId", w.itemId); d.put("SaleOrderId", id);
            d.put("Remarks", w.remarks == null ? "" : w.remarks);
            sup.setProcMap("[pcc].[USP_SaleOrderWagesDetail_Insert]", d);
        }
        attachments.apply(SCREEN, DOC_TYPE, id, r.supplierCustomerId, r.attachments);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", toInt(r.docNo));
        out.put("updated", r.id > 0);
        out.put("message", (r.id > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + toInt(r.docNo));
        return out;
    }

    /** GenericProvider.SetProc(SaleOrderDetail): every non-virtual property; an unset string is not sent. Removed rows carry only what grd_ColumnButtonClick copied. */
    private Map<String, Object> detail(int headId, Line l, Request r, int rowId, int action, boolean full) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("CommOnSale", full && l.commOnSale);
        d.put("IsApproved", false);
        d.put("ExchangeRate", full ? dec(r.exchangeRate) : BigDecimal.ZERO);
        d.put("FcyAmount", full ? dec(l.fcyAmount) : BigDecimal.ZERO);
        d.put("ItemDiscountAmount", dec(l.discAmount));
        d.put("ItemDiscountRate", dec(l.discRate));
        d.put("ItemNetAmount", dec(l.totalAmount));
        d.put("ItemWeight", dec(l.weight));
        d.put("ItemNetWeight", dec(l.netWeight));
        d.put("OrderItemAmount", dec(l.amount));
        d.put("OrderItemQty", dec(l.qty));
        d.put("OrderItemRate", dec(l.netRate));
        d.put("VarientEquivalent", dec(l.varientUnit));
        d.put("ItemRateWithOutAddLess", dec(l.itemPrice));
        d.put("RateAddLess", dec(l.addLessRate));
        d.put("ActionTypeId", action);
        d.put("ScheduleId", l.scheduleId);
        d.put("CityId", l.cityId);
        d.put("CurrencyId", full ? r.currencyId : 0);
        d.put("Id", rowId);
        d.put("ItemAttributeVarientId", l.attributeVarientId);
        d.put("ItemDiscountTypeId", l.discountId);
        d.put("JobLotId", l.jobLotId);
        d.put("OrderItemId", l.itemId);
        d.put("SaleOrderId", headId);
        d.put("RemarksDetail", l.remarks == null ? "" : l.remarks);
        return d;
    }

    private static BigDecimal dec(double v) { return BigDecimal.valueOf(v); }

    // ------------------------------------------------------------------ Delete (btnDelete_Click)

    /**
     * The desktop calls SaleOrder.DeleteById(UserAccount.EntryUserId, Id) with the two arguments swapped (the method is DeleteById(Id, EntryUserId)),
     * so the desktop procedure is handed the user id as the document id and nothing is deleted. The web passes them the right way round.
     */
    @Transactional
    public Map<String, Object> delete(int id) {
        lk.need(sup.rights(SCREEN), "delete", "Delete");
        if (id <= 0) throw new IllegalArgumentException("Record Id Not Found");
        header(id);
        sup.rows(GET, "Id", id, "EntryUserId", sup.userId(), "Activity", "DeleteById");
        return row("message", "Delete Record Seccessfully");
    }

    // ------------------------------------------------------------------ Print (CommonServices.SaleOrderSlipConcrete)

    /** SaleOrder.SaleOrderSlipAndRegister(Id, ApprovedFilter All): "No Record Found For Display" when empty. */
    public List<Map<String, Object>> slipRows(int id) {
        lk.need(sup.rights(SCREEN), "print", "Print");
        if (id <= 0) throw new IllegalArgumentException("No Record Found For Display");
        header(id);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("Id", id);
        List<Map<String, Object>> r = DesktopProc.rows(sup.jdbc(), "[pcc].[USP_SaleOrderSlipAndRegister]", p);
        if (r.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
        return r;
    }

    public List<Map<String, Object>> attachmentList(int id) { header(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { header(id); return attachments.download(SCREEN, id, attachmentId); }
}
