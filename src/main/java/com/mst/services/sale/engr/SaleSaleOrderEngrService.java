package com.mst.services.sale.engr;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
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
 * Screen 536 "SaleOrderWithQty" = Architecture.WinApp.SaleTrading.SaleOrderWithQty (Sale Engr, module 83, document type 1605).
 *
 * Desktop map (SaleOrderWithQty.cs, 4310 lines; logic lines before InitializeComponent):
 *   PurchsaeOrder_Load :287, CompanyFill/BranchFill/ProjectFill :410-450, SupplierNameFill :456, PaymentTerms :504, DocumentNoFill :525,
 *   ItemDetailFill :537, bindRateUomAndItemPackUom :601, bindPackUomInExtra :639, combojoblotfill :659, CityFill :698, DeliveryTerm/OrderStatus/
 *   BillTypeComboFill :735-770, CmbMasterItemInExtra_Leave :770, combitem_Leave_1 :833, GetTaxTypeIdAndTaxPercent :845, DiscountTypeFill :880,
 *   CurrencyFill :894, MultiCurrencyFeature :918, ConfigurationDefault :950, cmbCurrency_Leave :988, txtExchangeRate_TextChanged :1013,
 *   CalculateTotalInformation :1042, FormValidation :1308, FormValidationDetails :1390, FormValidationExtraDetail :1430, btnplus_Click :1470,
 *   grd_ColumnButtonClick :1513, grd_DoubleClick :1560, btnUpdateDetail_Click :1597, grdSettings :1687, ValidateDetailItemExistInExtra :1763,
 *   FillMasterItemsFromDetail :1792, btnAddInExtra_Click :1843, grdExtraItems_* :1890-2056, payment grid :2056-2220, Insert :2220,
 *   btnupdate_Click :2491, ReadById :2523, Reset :2666, gridhistoryfill :2830, grdhistory_* :2990-3140, CalculateWeight :3236,
 *   CalculateTaxAmount :3252, CalculateAmount :3280, DocDate_Leave :3585, KeyDown :3790, btnCity_Click :4278.
 * BLL: Architecture.BLL.Inventory.SaleOrder.Save (ActionId 1 insert / 2 update; "Record cannot be inserted because ActionTypeId greater than 1"),
 * DAL Architecture.DAL.Inventory.SaleOrder.SetData: header, SaleOrderDetail rows, payment terms (SortNo 1..n), extra items, attachments,
 * [DAW].[USp_DocumentApprovalDetail_Insert] with LimitAmount, all in one transaction.
 */
@Service
public class SaleSaleOrderEngrService {
    public static final String SCREEN = "SaleOrderWithQty";
    public static final int DOC_TYPE = 1605;

    private final SaleEngrSupport sup;
    private final SaleEngrAttachments attachments;

    public SaleSaleOrderEngrService(SaleEngrSupport sup, SaleEngrAttachments attachments) {
        this.sup = sup; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>(lists());
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());                                        // DocumentNoFill
        m.put("branchId", sup.branch());                                  // UserAccount.BranchesId (print argument)
        m.put("history", historyCombos());                                 // HistoryCombosFill (after SupplierNameFill, as in the Load order)
        return m;
    }

    /** The combos CompanyFill .. CurrencyFill + ConfigurationDefault (shared by Load and the Refresh button). */
    public Map<String, Object> refresh() {
        Map<String, Object> m = new LinkedHashMap<>(lists());
        m.put("allCustomers", m.get("customers"));                         // SupplierNameFill also rebinds CmbCustomerHistory
        return m;
    }

    private Map<String, Object> lists() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("branches", branches());
        m.put("projects", projects());
        m.put("customers", customers());
        m.put("terms", terms());
        m.put("items", items());
        m.put("jobLots", jobLots());
        m.put("cities", cities());
        m.put("currencies", currencies());
        m.put("multiCurrency", sup.erpFeature(6));                         // MultiCurrencyFeature: CommonServices.GetERPFeatureById(6)
        m.put("taxEditable", sup.configBool("TaxPercentEditable"));
        m.put("defaults", row("cityId", sup.configInt("City Area"), "jobLotId", sup.configInt("Job/Lot"),
                "baseCurrency", sup.configInt("Base Currency"), "baseRate", toDouble(sup.config("BaseCurrencyRate"))));   // ConfigurationDefault
        m.put("fmt", fmt());
        return m;
    }

    /** CommonServices.GetDecimalConfiguration: places used by stringFormatsingle / DecimalRateFormate / stringFormatsingleForFcy. */
    private Map<String, Object> fmt() {
        int amt = toInt(sup.config("Default NoofDecimal Points For Amount"));
        int rate = toInt(sup.config("Default NoofDecimal Points For Rate"));
        int fcy = toInt(sup.config("DefaultNoOfDecimalPointsForFcyAmount"));
        return row("amountRound", amt, "amount", amt >= 1 && amt <= 4 ? amt : 0, "rate", rate >= 1 && rate <= 4 ? rate : (rate == 0 ? 2 : 0),
                "fcy", fcy >= 1 && fcy <= 4 ? fcy : 0);
    }

    public int nextNo() {
        // SaleOrder.GeneratePurchaseOrderCodeByDocId
        List<Map<String, Object>> r = sup.rows("Sp_SaleOrder_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "DocumentTypeId", DOC_TYPE, "FinancialYearId", idOrNull(sup.fy()), "Activity", "GenerateSaleOrderCodeByDocId");
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** Branches.GetAll -> Sp_Branches_GetAllMethod 'GetAll'. */
    public List<Map<String, Object>> branches() {
        return sup.rows("Sp_Branches_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll");
    }

    /** Projects.GetAlldt -> Sp_Projects_GetAllMethod @MethodType='GetAll'. */
    public List<Map<String, Object>> projects() {
        return sup.rows("Sp_Projects_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll");
    }

    /** SupplierCustomer.GetforComboBinding. */
    public List<Map<String, Object>> customers() {
        return sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationIdCompanyIdForBinding");
    }

    /** InvDueTerms.GetAll. */
    public List<Map<String, Object>> terms() {
        return sup.rows("Sp_InvDueTerms_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll");
    }

    /** CommonServices.ItemGetForComboServiceBindForEngr -> dtitem (Id, ItemName, ItemCode = ItemCodeNew). */
    public List<Map<String, Object>> items() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_Item_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "AllItemsBindForEngr"))
            out.add(row("Id", r.get("Id"), "ItemName", r.get("ItemName"), "ItemCode", r.get("ItemCodeNew")));
        return out;
    }

    public List<Map<String, Object>> jobLots() {
        return sup.rows("SP_JobLot_ReadMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll");
    }

    /** City.GetAll -> SP_City_GetAllMethod @MethodType='GetAll'. */
    public List<Map<String, Object>> cities() {
        return sup.rows("SP_City_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll");
    }

    /** MultiCurrency.GetAll -> Sp_MultiCurrency_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> currencies() {
        return sup.rows("Sp_MultiCurrency_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadAll");
    }

    /** CommonServices.GetUomScheduleByItemId -> Sp_UOMSchedule_GetAllMethod 'ReadByItemID'. */
    public List<Map<String, Object>> uoms(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_UOMSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "ReadByItemID"))
            out.add(row("Id", r.get("Id"), "UOMCode", r.get("UOMCode"), "Equivalent", r.get("Equivalent"), "QtyEquivalent", r.get("QtyEquivalent"), "BaseRateUom", r.get("BaseRateUom")));
        return out;
    }

    /** CommonServices.GetTaxTypeIdAndPercentByItemId -> ItemTaxSchedule.GetTaxScheduleForItemId. */
    public List<Map<String, Object>> itemTax(int itemId, String date) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company());
        if (itemId != 0) p.put("ItemId", itemId);
        LocalDate d = parseDate(date);
        if (d != null) p.put("EffectedDate", d);
        p.put("Activity", "GetItemTaxScheduleForItemId");
        return DesktopProc.rows(sup.jdbc(), "Sp_ItemTaxSchedule_GetAllMethod", p);
    }

    /** CommonServices.GetTaxScheduleDetailbyItemIds (DocDate_Leave). */
    public List<Map<String, Object>> taxByItems(String itemIds, String date) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company());
        p.put("ItemIds", itemIds == null ? "" : itemIds);
        p.put("EffectedDate", parseDate(date));
        p.put("Activity", "GetTaxScheduleDetailbyItemIds");
        return DesktopProc.rows(sup.jdbc(), "Sp_ItemTaxSchedule_GetAllMethod", p);
    }

    /** cmbCurrency_Leave: VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher (document type 1605). */
    public List<Map<String, Object>> lastRate(int currencyId) {
        return sup.rows("Sp_Vouchers_GetMethods", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeIds", "1605",
                "DMultiCurrencyIds", String.valueOf(currencyId), "Activity", "GetMultiCurrencyAndLastRate");
    }

    /** btnupdate_Click: SaleOrder.ReadSaleOrderIdbyDocNo -> 'GetSaleIdByDocNo'. */
    public int idByDocNo(int docNo) {
        List<Map<String, Object>> r = sup.rows("Sp_SaleOrder_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocNo", docNo,
                "DocumentTypeId", DOC_TYPE, "Activity", "GetSaleIdByDocNo");
        return r.isEmpty() ? 0 : toInt(r.get(0).get("Id"));
    }

    // ------------------------------------------------------------------ History

    /** HistoryCombosFill: SaleOrder.GetDataForDropDownFromSaleOrder, Activity 'Customer'. */
    public Map<String, Object> historyCombos() {
        UserAccount u = sup.user();
        List<Map<String, Object>> cust = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetDataForDropDownFromSaleOrder", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "AppId", sup.ctx().currentAppId(), "UserId", u.getId()))
            if ("Customer".equals(str(r.get("Activity")))) cust.add(row("Id", r.get("Id"), "Customer", r.get("ReferenceName")));
        return row("customers", cust);
    }

    /** gridhistoryfill: SaleOrder.GetHisoty -> Sp_SaleOrder_GetAllMethod 'SaleOrderFormHistory'. */
    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate, int fromNo, int toNo, int customerId) {
        boolean all = Boolean.TRUE.equals(sup.rights(SCREEN).get("viewAll"));
        LocalDate from = parseDate(fromDate), to = parseDate(toDate);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("DocumentTypeId", DOC_TYPE);
        p.put("FinancialYearId", idOrNull(sup.fy()));
        String d = dateType == null ? "document" : dateType;
        String fk, tk;
        switch (d) {
            case "entry" -> { fk = "EntryFromDate"; tk = "EntryToDate"; }
            case "modify" -> { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
            case "approved" -> { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
            default -> { fk = "DocDateFrom"; tk = "DocDateTo"; }
        }
        if (from != null) p.put(fk, from);
        if (to != null) p.put(tk, to);
        if (fromNo != 0) p.put("PoSrFrom", (double) fromNo);
        if (toNo != 0) p.put("PoSrTo", (double) toNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        p.put("CanViewAllRecord", all);
        if (!all) p.put("EntryUser", sup.userId());
        p.put("Activity", "SaleOrderFormHistory");
        return DesktopProc.rows(sup.jdbc(), "Sp_SaleOrder_GetAllMethod", p);
    }

    // ------------------------------------------------------------------ ReadById (SaleOrder.GetByID)

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> r = sup.rows("Sp_SaleOrder_GetAllMethod", "Id", id, "Activity", "ReadById");
        UserAccount u = sup.user();
        if (r.isEmpty()) throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Sale order not found in this company");
        Map<String, Object> h = r.get(0);
        if ((h.containsKey("OrganizationId") && toInt(h.get("OrganizationId")) != u.getOrganizationId())
                || (h.containsKey("CompanyId") && toInt(h.get("CompanyId")) != u.getCompanyId())
                || (h.containsKey("DocumentTypeId") && toInt(h.get("DocumentTypeId")) != DOC_TYPE))
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Sale order not found in this company");
        return h;
    }

    public List<Map<String, Object>> detailRows(int id) {
        return sup.rows("Sp_SaleOrder_GetAllMethod", "Id", id, "Activity", "ReadBySaleOrderHeaderId");
    }

    private List<Map<String, Object>> paymentRows(int id) {
        return sup.rows("Sp_SaleOrder_GetAllMethod", "Id", id, "Activity", "SaleOrderPaymentTermDetailByHeaderId");
    }

    private List<Map<String, Object>> extraRows(int id) {
        return sup.rows("Sp_SaleOrder_GetAllMethod", "Id", id, "Activity", "ReadBySaleOrderId_ExtraItemDetail");
    }

    /** History detail grid (grdhistory_SelectionChanged): the detail rows of the selected order. */
    public List<Map<String, Object>> historyDetail(int id) {
        header(id);
        return detailRows(id);
    }

    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", h);
        out.put("lines", detailRows(id));
        out.put("extras", extraRows(id));
        out.put("payments", paymentRows(id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    // ------------------------------------------------------------------ Save (Insert / btnupdate_Click / BtnSaveAs_Click)

    public static class Line {
        public int id;
        public int itemId;
        public String itemName;
        public String itemDescription;
        public String remarks;
        public int jobLotId;
        public int packUomId;
        public double itemQty;
        public double weight;
        public double itemRate;
        public int rateUomId;
        public int discountType;
        public double discountPercent;
        public double discountAmount;
        public double amount;
        public int taxNameId;
        public double taxPercent;
        public double taxAmount;
        public double totalAmount;
        public int cityId;
        public String cityName;
    }

    public static class Extra {
        public int id;
        public int masterItemId;
        public int masterPackUomId;
        public int itemId;
        public String itemDescription;
        public String remarks;
        public int jobLotId;
        public int packUomId;
        public double itemQty;
        public double weight;
    }

    public static class Payment {
        public int paymentTermId;
        public double percent;
        public double amount;
        public int dueDays;
        public String dueDate;
        public String remarks;
    }

    public static class Request {
        public int id;
        public String docDate;
        public String docNo;
        public int branchId;
        public int projectId;
        public int customerId;
        public String supplierRefNo;
        public String remarks;
        public int paymentTermId;
        public int dueDays;
        public String dueDate;
        public String expiryDate;
        public String deliveryTerm;
        public String deliveryStartDate;
        public int deliveryDays;
        public String status;
        public int billTypeId;
        public int currencyId;
        public String exchangeRateText;
        public String fcyAmountText;
        public double exchangeRate;
        public double discountHeader;
        public List<Line> lines = new ArrayList<>();
        public List<Line> removed = new ArrayList<>();
        public List<Extra> extras = new ArrayList<>();
        public List<Payment> payments = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    private static double round(double v, int dec) {
        return BigDecimal.valueOf(v).setScale(Math.max(0, dec), RoundingMode.HALF_UP).doubleValue();
    }

    /** (decimal)double: the 15 significant digits C# keeps. */
    private static BigDecimal dec(double v) { return new BigDecimal(v, new MathContext(15, RoundingMode.HALF_EVEN)); }

    private static String fmtMax(double v, int max) {
        java.text.DecimalFormat f = new java.text.DecimalFormat("#,##0." + "#".repeat(max));
        return f.format(BigDecimal.valueOf(v));
    }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        List<Line> lines = r.lines == null ? List.of() : r.lines;
        List<Extra> extras = r.extras == null ? List.of() : r.extras;
        List<Payment> pays = r.payments == null ? List.of() : r.payments;
        List<Line> removed = r.removed == null ? List.of() : r.removed;

        // ---- FormValidation(), in the desktop's order
        boolean multi = sup.erpFeature(6);
        List<Map<String, Object>> terms = terms();
        formValidation(r, multi, terms);

        // tenant checks on the posted ids (the desktop fills the combos from company-scoped procedures)
        Set<Integer> branchIds = ids(branches()), projectIds = ids(projects()), customerIds = ids(customers());
        if (!branchIds.contains(r.branchId)) throw new Warning("branch field is required");
        if (!projectIds.contains(r.projectId)) throw new Warning("Project Field is Required");
        if (!customerIds.contains(r.customerId)) throw new Warning("Supplier Name Field is Required");

        Map<String, Object> old = null;
        Set<Integer> savedIds = new HashSet<>();
        if (r.id > 0) {
            old = header(r.id);
            for (Map<String, Object> d : detailRows(r.id)) savedIds.add(toInt(d.get("Id")));
        }

        Set<Integer> itemIds = ids(items());
        Set<Integer> jobIds = ids(jobLots());
        int fmtAmt = toInt(sup.config("Default NoofDecimal Points For Amount"));
        int amtPlaces = fmtAmt >= 1 && fmtAmt <= 4 ? fmtAmt : 0;
        int fcyCfg = toInt(sup.config("DefaultNoOfDecimalPointsForFcyAmount"));
        int fcyPlaces = fcyCfg >= 1 && fcyCfg <= 4 ? fcyCfg : 0;

        double rate = r.exchangeRate;
        if (!(rate > 0)) rate = toDouble(r.exchangeRateText);

        // ---- Insert(): per grid row (in grid order)
        int rowNo = 0;
        double totQty = 0, totWeight = 0, totAmount = 0, totTax = 0, totFcy = 0;
        List<double[]> fcyOf = new ArrayList<>();
        for (Line l : lines) {
            rowNo++;
            if (!itemIds.contains(l.itemId)) throw new Warning("Item Name Field is Required");
            if (l.packUomId <= 0) throw new Warning("Pack Uom Field is Required");
            if (l.itemQty == 0.0) throw new Warning("Item Qty Field is Required");
            if (l.rateUomId <= 0) throw new Warning("Rate UOM Field is Required");
            if (l.cityId <= 0) throw new Warning("City Field is Required");
            if (l.jobLotId > 0 && !jobIds.contains(l.jobLotId)) throw new Warning("JobLot Field is Required");
            int discType = l.discountType;
            if (discType > 0 && l.discountPercent <= 0.0 && l.discountAmount <= 0.0) discType = 0;
            if (discType > 0 && l.discountPercent > 0.0 && l.discountAmount <= 0.0)
                throw new Warning("Discount Amount Required In Row#" + rowNo + " In Detail Grid");
            double taxPercent = l.taxNameId > 0 ? l.taxPercent : 0.0, taxAmount = l.taxNameId > 0 ? l.taxAmount : 0.0;
            if (l.taxNameId > 0 && taxPercent > 0.0 && taxAmount <= 0.0)
                throw new Warning("TaxAmount Required In Row#" + rowNo + " In Detail Grid");
            if (l.id > 0 && (r.id <= 0 || !savedIds.contains(l.id))) throw new Warning("Invalid detail row for this sale order");
            double fcy = rate > 0 ? l.amount / rate : 0.0;            // txtExchangeRate_TextChanged: Amount / ExchangeRate
            fcyOf.add(new double[]{fcy});
            totQty += l.itemQty; totWeight += l.weight; totAmount += l.amount; totTax += l.taxAmount; totFcy += fcy;
        }
        for (Line l : removed)
            if (l.id <= 0 || !savedIds.contains(l.id)) throw new Warning("Deleted detail row does not belong to this sale order");
        for (Extra e : extras) {
            if (!itemIds.contains(e.itemId)) throw new Warning("Item Field is Required");
        }

        // header totals as CalculateTotalInformation leaves them in the text boxes Insert() reads
        double orderQty = lines.isEmpty() ? 0 : round(totQty, 2), orderWeight = lines.isEmpty() ? 0 : round(totWeight, 2);
        double itemAmountHeader = lines.isEmpty() ? 0 : round(totAmount, amtPlaces);
        double taxAmountHeader = lines.isEmpty() ? 0 : round(totTax, amtPlaces);
        double discountHeader = round(r.discountHeader, amtPlaces);
        double orderAmount = lines.isEmpty() ? 0 : round(itemAmountHeader - discountHeader, amtPlaces);
        double fcyAmount = lines.isEmpty() ? 0 : round(totFcy, fcyPlaces);

        // ---- payment detail checks
        if (pays.isEmpty()) throw new Warning("Payment Detail Record Not Found");
        BigDecimal payAmount = BigDecimal.ZERO, payPct = BigDecimal.ZERO;
        List<Payment> payKeep = new ArrayList<>();
        int pn = 0;
        for (Payment p : pays) {
            pn++;
            if (dec(p.amount).compareTo(BigDecimal.ZERO) > 0) {
                payPct = payPct.add(dec(p.percent));
                payAmount = payAmount.add(dec(p.amount));
                if (p.paymentTermId <= 0) throw new Warning("Payment Term Required in row#" + pn);
                if (p.paymentTermId == 2 && p.dueDays <= 0) throw new Warning("Due Days Required In case Of Credit row in row#" + pn);
                payKeep.add(p);
            }
        }
        if (payAmount.compareTo(dec(orderAmount)) != 0)
            throw new Warning("Payment Detail Amount " + fmtMax(payAmount.doubleValue(), 4) + " Not Equal to Total Order Amount " + fmtMax(orderAmount, 4));
        if (payPct.compareTo(new BigDecimal("100")) != 0) throw new Warning("Payment Detail Total% not equal to 100");

        // ---- BLL.SaleOrder.Save / DAL.SetData
        if (r.id <= 0 && !removed.isEmpty()) throw new IllegalArgumentException("Record cannot be inserted because ActionTypeId greater than 1");
        if (lines.isEmpty() && removed.isEmpty()) throw new IllegalArgumentException("SaleOrderDetailList not found");

        Timestamp now = now();
        String status = text(r.status);
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", r.id);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DocNo", r.id > 0 && old != null ? toInt(old.get("DocNo")) : toInt(r.docNo));     // the insert procedure draws its own DocNo
        h.put("BranchSrNo", r.id > 0 && old != null ? toInt(old.get("BranchSrNo")) : 0);
        h.put("DocDate", parseDate(r.docDate));
        h.put("OrderCatagoryId", 0);
        h.put("CatagorySrNo", 0);
        h.put("OrderSupCustId", r.customerId);
        h.put("SupplierRefNo", text(r.supplierRefNo));
        h.put("RefrenenceParty", 0);
        h.put("SupplierCustomerIdStockParty", 0);
        h.put("BrokerAgentSupCustId", 0);
        h.put("CommRate", 0.0);
        h.put("UomScheduleIdCmRate", 0);
        h.put("CommAmount", 0.0);
        h.put("PaymentTermsId", r.paymentTermId);
        h.put("OrderDueDays", r.dueDays);
        h.put("OrderDueDate", parseDate(r.dueDate));
        h.put("OrderExpiryDate", parseDate(r.expiryDate) != null ? parseDate(r.expiryDate) : LocalDate.now());
        h.put("DeliveryTerm", text(r.deliveryTerm));
        h.put("DeliveryStartDate", parseDate(r.deliveryStartDate));
        h.put("DeliveryDays", r.deliveryDays);
        h.put("RemarksHeader", r.remarks == null ? "" : r.remarks);
        h.put("OrderStatus", status);
        h.put("IsAproved", false);
        h.put("EntryDate", now);
        h.put("EntryUser", u.getId());
        h.put("ModifyDate", now);
        h.put("ModifyUser", u.getId());
        h.put("PostUser", 0);
        h.put("PostState", false);
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchesId", r.branchId);
        h.put("ProjectsId", r.projectId);
        h.put("NoOfCntnr", 0);
        h.put("PfiScId", 0);
        h.put("ActionId", r.id > 0 ? 2 : 1);
        h.put("FinancialYearId", sup.fy());
        h.put("IsValidate", false);
        h.put("BillCalculateTypeId", r.billTypeId);
        h.put("CurrencyId", r.currencyId);
        h.put("ExchangeRate", rate);
        h.put("FcyAmount", fcyAmount);
        h.put("OrderQty", orderQty);
        h.put("OrderWeight", orderWeight);
        h.put("OrderAmount", orderAmount);
        h.put("ItemAmountHeader", itemAmountHeader);
        h.put("DiscountAmountHeader", discountHeader);
        h.put("TaxAmountHeader", taxAmountHeader);
        List<String> names = new ArrayList<>();
        SaleEngrAttachments.Change ch = r.attachments;
        if (r.id > 0) {
            Set<Integer> gone = new HashSet<>(ch != null && ch.removeAttachmentIds != null ? ch.removeAttachmentIds : List.of());
            for (Map<String, Object> a : attachments.list(SCREEN, r.id)) if (!gone.contains(toInt(a.get("Id")))) names.add(str(a.get("Attachment")));
        }
        if (ch != null && ch.files != null) for (var f : ch.files) names.add(f.name);
        h.put("AttachmentsValues", String.join(",", names));
        h.put("CustomAttachmentsValues", String.join(",", names));
        h.put("OtherCommissionAgentId", 0);
        h.put("OtherCommissionUom", 0.0);
        h.put("OtherCommissionRate", 0.0);
        h.put("OtherCommissionAmount", 0.0);
        h.put("OrderTypeId", 0);
        h.put("OtherCategoryId", 0);
        h.put("BookingPersonId", 0);

        String proc = r.id > 0 ? "Sp_SaleOrder_Update" : "Sp_SaleOrder_Insert";
        Integer num = DesktopProc.scalar(sup.jdbc(), proc, h);
        int id = num != null && num > 0 ? num : r.id;
        if (id <= 0) throw new IllegalStateException("The sale order could not be saved.");

        BigDecimal limit = BigDecimal.ZERO;
        for (Line l : removed) sup.setProcMap("Sp_SaleOrderDetail_Insert", removedDetail(id, l));
        int k = 0;
        for (Line l : lines) {
            int action = (r.id > 0 ? l.id : 0) <= 0 ? 1 : 2;
            limit = limit.add(dec(l.amount));
            sup.setProcMap("Sp_SaleOrderDetail_Insert", detail(id, l, r.id > 0 ? l.id : 0, action, rate, r.currencyId, fcyOf.get(k++)[0]));
        }
        int sort = 1;
        for (Payment p : payKeep) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", 0);
            d.put("SaleOrderId", id);
            d.put("PaymentTermId", p.paymentTermId);
            d.put("PrcntOfTotal", dec(p.percent));
            d.put("Amount", dec(p.amount));
            d.put("DueDays", p.dueDays);
            d.put("DueDate", parseDate(p.dueDate));
            d.put("PaymentRemarks", p.remarks == null ? "" : p.remarks);
            d.put("SortNo", sort++);
            sup.setProcMap("USP_SaleOrderPaymentTermsDetail_Insert", d);
        }
        for (Extra e : extras) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", r.id > 0 ? e.id : 0);
            d.put("SaleOrderId", id);
            d.put("MasterItemId", e.masterItemId);
            d.put("MasterPackUomId", e.masterPackUomId);
            d.put("ItemId", e.itemId);
            d.put("PackUomId", e.packUomId);
            d.put("PackingTypeId", 0);
            d.put("ItemDiscription", e.itemDescription == null ? "" : e.itemDescription);
            d.put("ItemQty", dec(e.itemQty));
            d.put("NetWeight", dec(e.weight));
            d.put("JobLotId", e.jobLotId);
            d.put("Remarks", e.remarks == null ? "" : e.remarks);
            sup.setProcMap("USP_SaleOrderExtraItemsDetail_Insert", d);
        }
        attachments.apply(SCREEN, DOC_TYPE, id, r.customerId, ch);
        sup.setProcMap("[DAW].[USp_DocumentApprovalDetail_Insert]", row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC_TYPE, "Id", id, "LimitAmount", limit));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("updated", r.id > 0);
        out.put("message", r.id > 0 ? "Data Update Successfully." : "Data Save Successfully.");
        return out;
    }

    /** Header FormValidation(): the first failing message, word for word. */
    private void formValidation(Request r, boolean multi, List<Map<String, Object>> terms) {
        if (r.branchId <= 0) throw new Warning("branch field is required");
        if (r.projectId <= 0) throw new Warning("Project Field is Required");
        if (r.customerId <= 0) throw new Warning("Supplier Name Field is Required");
        String term = null;
        for (Map<String, Object> t : terms) if (toInt(t.get("Id")) == r.paymentTermId) term = str(t.get("TermsDescription")).trim();
        if (term == null) throw new Warning("Payment Term Field is Required");
        if ("Credit".equals(term) && r.dueDays == 0) throw new Warning("Due Days Field is Required");
        String dt = text(r.deliveryTerm);
        if (!dt.equals("Load") && !dt.equals("Ponch")) throw new Warning("Delivery Term Field is Required");
        String ex = text(r.exchangeRateText);
        if (multi) {
            if (r.currencyId <= 0) throw new Warning("Fcy Code Field is Required");
            if (ex.isEmpty() || ex.equals("0")) throw new Warning("Exchange Rate Field is Required");
            String fc = text(r.fcyAmountText);
            if (fc.isEmpty() || fc.equals("0")) throw new Warning("Fcy Amount Rate Field is Required");
        } else {
            if (r.currencyId <= 0) throw new Warning("Please Configure Your Base Currency In configurations");
            if (ex.isEmpty() || ex.equals("0")) throw new Warning("Please Configure Your Base Currency Rate In configurations");
        }
        if (r.billTypeId != 1 && r.billTypeId != 2) throw new Warning("Bill Type Field is Required");
        String st = text(r.status);
        if (!st.equals("Open") && !st.equals("Complete") && !st.equals("Cancel")) throw new Warning("Order Status Field is Required");
    }

    /** GenericProvider.SetProc(SaleOrderDetail): every non-virtual property, string properties left null are not sent. */
    private Map<String, Object> detail(int headId, Line l, int id, int action, double rate, int currencyId, double fcy) {
        int discType = l.discountType;
        if (discType > 0 && l.discountPercent <= 0.0 && l.discountAmount <= 0.0) discType = 0;
        Map<String, Object> d = new LinkedHashMap<>();
        for (String k : List.of("BagPrice", "BagWeight", "RetailRate", "ItemPrice", "RateDiscount", "PackingAddLessOnRate")) d.put(k, 0.0);
        for (String k : List.of("PriceScheduleId", "RefDocId", "RefDocDetailId", "ItemVariantId", "CastingTypeId", "WarehouseId", "PackingTypeID", "ReferencePartyId",
                "CostCenterId", "LocationTypeId", "ItemConditionId", "SecondaryUomId")) d.put(k, 0);
        d.put("SecondaryUomQty", BigDecimal.ZERO);
        d.put("SecondaryUomItemRate", BigDecimal.ZERO);
        d.put("CommOnSale", false);
        d.put("Id", id);
        d.put("SaleOrderId", headId);
        d.put("OrderItemId", l.itemId);
        d.put("ItemDiscription", l.itemDescription == null ? "" : l.itemDescription);
        d.put("JobLotId", l.jobLotId);
        d.put("OrderItemUOMId", l.packUomId);
        d.put("OrderItemQty", l.itemQty);
        d.put("NetWeight", l.weight);
        d.put("OrderItemRate", l.itemRate);
        d.put("OrderItemRateUOMId", l.rateUomId);
        d.put("DiscountTypeId", discType);
        d.put("ItemDiscount", l.discountPercent);
        d.put("ItemDiscountAmount", l.discountAmount);
        d.put("Amount", l.amount);
        d.put("ExchangeRate", rate);
        d.put("CurrencyId", currencyId);
        d.put("FcyAmount", fcy);
        d.put("TaxNameId", l.taxNameId);
        d.put("TaxPercent", l.taxNameId > 0 ? l.taxPercent : 0.0);
        d.put("TaxAmount", l.taxNameId > 0 ? l.taxAmount : 0.0);
        d.put("TotalAmount", l.totalAmount);
        d.put("OrderRemarks", l.remarks == null ? "" : l.remarks);
        d.put("CityId", l.cityId);
        d.put("CityArea", l.cityName == null ? "" : l.cityName);
        d.put("ActionTypeId", action);
        boolean taxable = l.taxNameId > 0 && l.taxPercent > 0.0 && l.taxAmount > 0.0;
        d.put("TaxableStatus", taxable ? "True" : "False");
        return d;
    }

    /** lstRemoveRecord entries (grd_ColumnButtonClick): ActionTypeId 3, the fields the desktop copies from the grid row. */
    private Map<String, Object> removedDetail(int headId, Line l) {
        Map<String, Object> d = new LinkedHashMap<>();
        for (String k : List.of("BagPrice", "BagWeight", "RetailRate", "ItemPrice", "RateDiscount", "PackingAddLessOnRate", "ExchangeRate", "FcyAmount")) d.put(k, 0.0);
        for (String k : List.of("PriceScheduleId", "RefDocId", "RefDocDetailId", "ItemVariantId", "CastingTypeId", "WarehouseId", "PackingTypeID", "CurrencyId",
                "ReferencePartyId", "CostCenterId", "LocationTypeId", "ItemConditionId", "SecondaryUomId")) d.put(k, 0);
        d.put("SecondaryUomQty", BigDecimal.ZERO);
        d.put("SecondaryUomItemRate", BigDecimal.ZERO);
        d.put("CommOnSale", false);
        d.put("Id", l.id);
        d.put("SaleOrderId", headId);
        d.put("OrderItemId", l.itemId);
        d.put("ItemDiscription", l.itemDescription == null ? "" : l.itemDescription);
        d.put("JobLotId", l.jobLotId);
        d.put("OrderItemUOMId", l.packUomId);
        d.put("OrderItemQty", l.itemQty);
        d.put("NetWeight", l.weight);
        d.put("OrderItemRate", l.itemRate);
        d.put("OrderItemRateUOMId", l.rateUomId);
        d.put("DiscountTypeId", l.discountType);
        d.put("ItemDiscount", l.discountPercent);
        d.put("ItemDiscountAmount", l.discountAmount);
        d.put("Amount", l.amount);
        d.put("CityId", l.cityId);
        d.put("CityArea", l.cityName == null ? "" : l.cityName);
        d.put("OrderRemarks", l.remarks == null ? "" : l.remarks);
        d.put("TaxNameId", l.taxNameId);
        d.put("TaxPercent", l.taxPercent);
        d.put("TaxAmount", l.taxAmount);
        d.put("TotalAmount", l.totalAmount);
        d.put("ActionTypeId", 3);
        return d;
    }

    private static Set<Integer> ids(List<Map<String, Object>> rows) {
        Set<Integer> s = new HashSet<>();
        for (Map<String, Object> r : rows) s.add(toInt(r.get("Id")));
        return s;
    }

    public List<Map<String, Object>> attachmentList(int id) { header(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { header(id); return attachments.download(SCREEN, id, attachmentId); }

    private static LocalDate parseDate(String s) {
        if (s == null || s.isBlank()) return null;
        try { return LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))); } catch (RuntimeException e) { return null; }
    }
}
