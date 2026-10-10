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
 * Screen 779 "frmSaleOrderEngr" = Architecture.WinApp.Mfg.Sale.frmSaleOrderEngr (Sale Engr module 134, document type 1656).
 *
 * Desktop map (frmSaleOrderEngr.cs, 10247 lines; logic before InitializeComponent :5085):
 *   PurchsaeOrder_Load :565, bindvarientunit :714, GetItemCategory :756, SampleModaldBCall :797, ItemModalBind :817, CastingType :868,
 *   SupplierNameFill :892, BindCustomerCity :948, PaymentTerms :974, DocumentNoFill :1010, ItemDtFillFromGlobal :1029, ItemDetailBind :1049,
 *   bindRateUomAndItemPackUom :1215, CityFill :1352, DeliveryTerm/OrderStatus/BillTypeComboFill :1386-1442, combitem_Leave_1 :1530,
 *   FillItemFinishWeight :1551, CmbCategory_Leave :1567, GetTaxTypeIdAndTaxPercent :1580, DiscountTypeFill :1615, CmbSampleModal_Leave :1633,
 *   CurrencyFill :1650, MultiCurrencyFeature :1688, ConfigurationDefault :1718, cmbCurrency_Leave :1757, txtExchangeRate_TextChanged :1800,
 *   CalculateTotalInformation :1845, FormValidation :1899, FormValidationDetails :1962, btnplus_Click :2044, DeleteDetailRow :2110,
 *   grd_DoubleClick :2176, btnUpdateDetail_Click :2220, grdSettings :2314, AddPaymentRowsRowsInPaymentGrid :2686, grdPaymentTerm_CellUpdated :2745,
 *   PaymentTermAmountCalculateFromPercent :2827, Insert :2852, ReadById :3143, Reset :3300, HistoryBranchComboFill :3406, HistoryCombosFill :3450,
 *   gridhistoryfill :3560, grdhistory_SelectionChanged :3838, CalculateTaxAmount :4003, CalculateAmount :4038, GeneratePrint :4272,
 *   DocDate_Leave :4338, KeyDown :4596, HeaderFieldsDisableWhenOrderDetailReferred :4984, FieldsDisableWhenOrderDetailReferred :5018.
 * BLL: Architecture.BLL.Inventory.SaleOrder.Save (Id 0 -> Sp_SaleOrder_Insert, ActionId 1; else Sp_SaleOrder_Update, ActionId 2) and
 * DAL SetData: header, Sp_SaleOrderDetail_Insert per row, USP_SaleOrderPaymentTermsDetail_Insert (SortNo 1..n), attachments,
 * [DAW].[USp_DocumentApprovalDetail_Insert] (LimitAmount), one transaction.
 */
@Service
public class SaleSoMfgEngrService {
    public static final String SCREEN = "frmSaleOrderEngr";
    public static final int DOC_TYPE = 1656;

    private final SaleEngrSupport sup;
    private final SaleEngrAttachments attachments;

    public SaleSoMfgEngrService(SaleEngrSupport sup, SaleEngrAttachments attachments) {
        this.sup = sup; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>(lists());
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());                                          // DocumentNoFill
        m.put("branchId", sup.branch());                                    // UserAccount.BranchesId (print argument)
        m.put("history", historyBranches());                                // HistoryBranchComboFill
        m.put("historyDays", sup.configInt("DefaultDaysToLessFromHistoryFromDate"));
        return m;
    }

    /** Everything CurrencyFill .. ConfigurationDefault bind (shared by Load and the Refresh button, toolStripButton1_Click). */
    public Map<String, Object> lists() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("customers", customers());                                    // SupplierNameFill
        m.put("categories", categories());                                  // GetItemCategory
        m.put("castingTypes", castingTypes());                              // CastingType
        m.put("terms", terms());                                            // PaymentTerms
        m.put("modals", sampleModals());                                    // SampleModaldBCall
        m.put("items", items());                                            // ItemDtFillFromGlobal
        m.put("cities", cities());                                          // CityFill
        m.put("currencies", currencies());                                  // CurrencyFill
        m.put("multiCurrency", sup.erpFeature(6));                          // MultiCurrencyFeature
        m.put("taxEditable", sup.configBool("TaxPercentEditable"));
        m.put("insertWithoutRate", sup.configBool("SaleOrderInsertWithoutRate"));
        m.put("defaults", row("baseCurrency", sup.configInt("Base Currency"), "baseRate", toDouble(sup.config("BaseCurrencyRate"))));   // ConfigurationDefault
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
        // DocumentNoFill: SaleOrder.GeneratePurchaseOrderCodeByDocId (document type 1656)
        List<Map<String, Object>> r = sup.rows("Sp_SaleOrder_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "DocumentTypeId", DOC_TYPE, "FinancialYearId", idOrNull(sup.fy()), "Activity", "GenerateSaleOrderCodeByDocId");
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** SupplierNameFill: ERP feature 4 ? CommonServices.GetVendorsAndCustomersWithCityName(2) : AllSupplierCustomerWithCityName. */
    public List<Map<String, Object>> customers() {
        List<Map<String, Object>> src = sup.erpFeature(4)
                ? sup.rows("USP_GetVendorsAndCustomersWithCityName", "OrganizationId", sup.org(), "CompanyId", sup.company(), "PartyTypeId", 2)
                : sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "AllSupplierCustomerWithCityName");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src)
            out.add(row("Id", toInt(ci(r, "Id")), "CompanyName", str(ci(r, "CompanyName")), "PartyCode", str(ci(r, "PartyCode")), "GlAccountId", toInt(ci(r, "GlAccountId")),
                    "CityId", toInt(ci(r, "CityId")), "CityName", str(ci(r, "CityName"))));
        return out;
    }

    /** ItemCategory.Getall -> Sp_ItemCategory_GetAllMethod 'ReadByOrganizationCompanyId'. */
    public List<Map<String, Object>> categories() {
        return sup.rows("Sp_ItemCategory_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationCompanyId");
    }

    /** LookUps.GetDataByTypeId(1) -> [Mfg].[USP_LookUps_GetAllMethod] @Id = 1, 'GetDataByTypeId'. */
    public List<Map<String, Object>> castingTypes() {
        return sup.rows("[Mfg].[USP_LookUps_GetAllMethod]", "Id", 1, "Activity", "GetDataByTypeId");
    }

    /** CommonServices.GetDueTermServiceBind -> InvDueTerms.GetAll. */
    public List<Map<String, Object>> terms() {
        return sup.rows("Sp_InvDueTerms_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll");
    }

    /** SampleModaldBCall: ItemModel.ItemModel_WithItemId -> [item].[USP_ItemModel_WithItemId] (Id = itemModalId, SampleNo, ItemModal, ItemCategoryId, ItemId). */
    public List<Map<String, Object>> sampleModals() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("[item].[USP_ItemModel_WithItemId]", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            out.add(row("Id", toInt(ci(r, "itemModalId")), "SampleNo", str(ci(r, "ItemSampleNo")), "ItemModal", str(ci(r, "ItemModal")),
                    "ItemCategoryId", toInt(ci(r, "ItemCategoryId")), "ItemId", toInt(ci(r, "ItemId"))));
        return out;
    }

    /** ItemDtFillFromGlobal: Sp_Item_GetAllMethod 'AllItemsBindForEngr' (Id, ItemName, ItemCode, ItemCategoryId, itemModalId, FinishWeight). */
    public List<Map<String, Object>> items() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_Item_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "AllItemsBindForEngr")) {
            Object code = ci(r, "ItemCode") != null ? ci(r, "ItemCode") : ci(r, "ItemCodeNew");
            out.add(row("Id", toInt(ci(r, "Id")), "ItemName", str(ci(r, "ItemName")), "ItemCode", str(code), "ItemCategoryId", toInt(ci(r, "ItemCategoryId")),
                    "itemModalId", toInt(ci(r, "itemModalId")), "FinishWeight", toDouble(ci(r, "FinishWeight"))));
        }
        return out;
    }

    /** CommonServices.CityGetAllService -> City.GetAll -> SP_City_GetAllMethod @MethodType='GetAll'. */
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

    /** bindvarientunit: ItemAttributeVarient.GetAllForCombo -> VariantId / VariantDescription. */
    public List<Map<String, Object>> variants(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_ItemAttributeVarient_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "GetAllForCombo"))
            out.add(row("VariantId", r.get("ItemAttributeVarientId"), "VariantDescription", r.get("VarientDescription")));
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

    /** cmbCurrency_Leave: VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher (document type 1656). */
    public List<Map<String, Object>> lastRate(int currencyId) {
        return sup.rows("Sp_Vouchers_GetMethods", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeIds", String.valueOf(DOC_TYPE),
                "DMultiCurrencyIds", String.valueOf(currencyId), "Activity", "GetMultiCurrencyAndLastRate");
    }

    // ------------------------------------------------------------------ History

    /** HistoryBranchComboFill: SaleOrderBranchWise ? the user's own branch : USP_GetBranchsAllocatedToUserFromSaleOrder (document type 1656). */
    public Map<String, Object> historyBranches() {
        boolean implemented = sup.configBool("SaleOrderBranchWise");
        int mine = sup.branch();
        String mineName = "";
        for (Map<String, Object> b : sup.rows("Sp_Branches_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll"))
            if (toInt(ci(b, "Id")) == mine) mineName = str(ci(b, "BranchName"));
        List<Map<String, Object>> out = new ArrayList<>();
        if (implemented) {
            out.add(row("Id", mine, "BranchName", mineName));
        } else {
            for (Map<String, Object> r : sup.rows("USP_GetBranchsAllocatedToUserFromSaleOrder", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                    "UserId", sup.userId(), "DocumentTypeId", DOC_TYPE))
                out.add(row("Id", toInt(ci(r, "BranchId")), "BranchName", str(ci(r, "BranchName"))));
        }
        return row("branches", out, "userBranchName", mineName, "branchImplemented", implemented, "branchFeature", sup.erpFeature(11));
    }

    /** ",id,id" built from the checked branch ids, restricted to the branches the user may choose (the desktop reads them from dtBranch). */
    private String branchList(String ids) {
        Set<Integer> allowed = new HashSet<>();
        @SuppressWarnings("unchecked")
        List<Map<String, Object>> bs = (List<Map<String, Object>>) historyBranches().get("branches");
        for (Map<String, Object> b : bs) allowed.add(toInt(b.get("Id")));
        StringBuilder sb = new StringBuilder();
        if (ids != null) for (String s : ids.split(",")) {
            String t = s.trim();
            if (t.matches("\\d{1,9}") && allowed.contains(Integer.parseInt(t))) sb.append(',').append(Integer.parseInt(t));
        }
        return sb.toString();
    }

    /** HistoryCombosFill: SaleOrder.GetDataForDropDownFromSaleOrder (USP_GetDataForDropDownFromSaleOrder), rows whose Activity is 'Customer'. */
    public List<Map<String, Object>> historyCustomers(String branchIds, boolean validateBranch) {
        String list = branchList(branchIds);
        if (list.isEmpty() && validateBranch) throw new Warning("Select branch first");
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company());
        p.put("AppId", sup.ctx().currentAppId()); p.put("UserId", sup.userId());
        p.put("DocumentTypeIds", String.valueOf(DOC_TYPE));
        if (!list.isEmpty()) p.put("BranchesIds", list);
        List<Map<String, Object>> cust = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(sup.jdbc(), "USP_GetDataForDropDownFromSaleOrder", p))
            if ("Customer".equals(str(ci(r, "Activity")))) cust.add(row("Id", ci(r, "Id"), "Customer", ci(r, "ReferenceName")));
        return cust;
    }

    /** gridhistoryfill: SaleOrder.GetHisoty -> Sp_SaleOrder_GetAllMethod 'SaleOrderFormHistory'. */
    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate, int fromNo, int toNo, int customerId, String branchIds) {
        String list = branchList(branchIds);
        if (list.isEmpty()) throw new Warning("Select branch first");
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
        p.put("BranchesIds", list);
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

    /** grdhistory_SelectionChanged: the detail rows of the selected order. */
    public List<Map<String, Object>> historyDetail(int id) {
        header(id);
        return detailRows(id);
    }

    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", h);
        out.put("lines", detailRows(id));
        out.put("payments", paymentRows(id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    /** GeneratePrint -> CommonServices.SaleOrderSlipEngr_1656: SaleOrder.SaleOrderSlipAndRegister_Engr (USP_SaleOrderSlipAndRegister_Engr), "No Record Found For Display" when empty. */
    public Map<String, Object> slipRows(int id) {
        header(id);
        List<Map<String, Object>> r = sup.rows("USP_SaleOrderSlipAndRegister_Engr", "OrganizationId", sup.org(), "CompanyId", sup.company(), "BranchesId", sup.branch(),
                "FinancialYearId", sup.fy(), "Id", idOrNull(id), "DocumentTypeIds", String.valueOf(DOC_TYPE));
        return row("rows", r.size());
    }

    // ------------------------------------------------------------------ Save (Insert / btnupdate_Click / BtnSaveAs_Click)

    public static class Line {
        public int id;
        public int itemId;
        public int packUomId;
        public int variantId;
        public int castingTypeId;
        public String remarks;
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
        List<Payment> pays = r.payments == null ? List.of() : r.payments;
        List<Line> removed = r.removed == null ? List.of() : r.removed;

        // ---- Insert(): "Grid Record Not Found" when the detail grid is empty, then FormValidation()
        if (lines.isEmpty()) throw new Warning("Grid Record Not Found");
        List<Map<String, Object>> terms = terms();
        formValidation(r, sup.erpFeature(6), terms);

        // tenant checks on the posted ids (the desktop fills every combo from company-scoped procedures)
        if (!ids(customers()).contains(r.customerId)) throw new Warning("Customer Name Field is Required");

        Map<String, Object> old = null;
        Set<Integer> savedIds = new HashSet<>();
        if (r.id > 0) {
            old = header(r.id);
            for (Map<String, Object> d : detailRows(r.id)) savedIds.add(toInt(d.get("Id")));
        }

        Map<Integer, String> itemNames = new HashMap<>();
        for (Map<String, Object> it : items()) itemNames.put(toInt(it.get("Id")), str(it.get("ItemName")));
        Set<Integer> castIds = ids(castingTypes());
        Set<Integer> cityIds = ids(cities());
        Map<Integer, Set<Integer>> variantsOf = new HashMap<>();
        int fmtAmt = toInt(sup.config("Default NoofDecimal Points For Amount"));
        int amtPlaces = fmtAmt >= 1 && fmtAmt <= 4 ? fmtAmt : 0;
        int fcyCfg = toInt(sup.config("DefaultNoOfDecimalPointsForFcyAmount"));
        int fcyPlaces = fcyCfg >= 1 && fcyCfg <= 4 ? fcyCfg : 0;

        double rate = r.exchangeRate;
        if (!(rate > 0)) rate = toDouble(r.exchangeRateText);

        // ---- Insert(): per grid row (in grid order)
        int rowNo = 0;
        double totQty = 0, totWeight = 0, totAmount = 0, totTax = 0, totFcy = 0;
        List<Double> fcyOf = new ArrayList<>();
        for (Line l : lines) {
            rowNo++;
            if (!itemNames.containsKey(l.itemId)) throw new Warning("Item Name Field is Required");
            if (l.packUomId <= 0) throw new Warning("Pack Uom Field is Required");
            if (l.itemQty == 0.0) throw new Warning("Item Qty Field is Required");
            if (l.rateUomId <= 0) throw new Warning("Rate UOM Field is Required");
            if (l.cityId <= 0 || !cityIds.contains(l.cityId)) throw new Warning("City Field is Required");
            if (l.castingTypeId > 0 && !castIds.contains(l.castingTypeId)) throw new Warning("Casting Type is not valid in Row#" + rowNo);
            if (l.variantId > 0 && !variantsOf.computeIfAbsent(l.itemId, k -> { Set<Integer> s = new HashSet<>(); for (Map<String, Object> v : variants(k)) s.add(toInt(v.get("VariantId"))); return s; }).contains(l.variantId))
                throw new Warning("Modal Description is not valid in Row#" + rowNo);
            int discType = l.discountType;
            if (discType > 0 && l.discountPercent <= 0.0 && l.discountAmount <= 0.0) discType = 0;
            if (discType > 0 && l.discountPercent > 0.0 && l.discountAmount <= 0.0)
                throw new Warning("Discount Amount Required In Row#" + rowNo + " In Detail Grid");
            double taxPercent = l.taxNameId > 0 ? l.taxPercent : 0.0, taxAmount = l.taxNameId > 0 ? l.taxAmount : 0.0;
            if (l.taxNameId > 0 && taxPercent > 0.0 && taxAmount <= 0.0)
                throw new Warning("TaxAmount Required In Row#" + rowNo + " In Detail Grid");
            if (l.id > 0 && (r.id <= 0 || !savedIds.contains(l.id))) throw new Warning("Invalid detail row for this sale order");
            double fcy = rate > 0 ? l.amount / rate : 0.0;            // txtExchangeRate_TextChanged: Amount / ExchangeRate
            fcyOf.add(fcy);
            totQty += l.itemQty; totWeight += l.weight; totAmount += l.amount; totTax += l.taxAmount; totFcy += fcy;
        }
        for (Line l : removed)
            if (l.id <= 0 || !savedIds.contains(l.id)) throw new Warning("Deleted detail row does not belong to this sale order");

        // header totals as CalculateTotalInformation leaves them in the text boxes Insert() reads
        double orderQty = round(totQty, 2), orderWeight = round(totWeight, 2);
        double itemAmountHeader = round(totAmount, amtPlaces);
        double taxAmountHeader = round(totTax, amtPlaces);
        double discountHeader = round(r.discountHeader, amtPlaces);
        double orderAmount = round(itemAmountHeader - discountHeader, amtPlaces);
        double fcyAmount = round(totFcy, fcyPlaces);

        // ---- payment detail checks
        if (pays.isEmpty()) throw new Warning("Payment Detail Record Not Found");
        BigDecimal payAmount = BigDecimal.ZERO, payPct = BigDecimal.ZERO;
        List<Payment> payKeep = new ArrayList<>();
        BigDecimal sumAll = BigDecimal.ZERO;
        for (Payment p : pays) sumAll = sumAll.add(dec(p.amount));
        if (sumAll.compareTo(BigDecimal.ZERO) > 0) {
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
        } else {                                                           // a single 100% row built from the header payment term
            Payment p = new Payment();
            p.paymentTermId = r.paymentTermId; p.percent = 100; p.amount = 100.0 * orderAmount / 100.0;
            p.dueDays = r.dueDays; p.dueDate = r.dueDate; p.remarks = "";
            payPct = new BigDecimal("100");
            payAmount = dec(p.amount).setScale(4, RoundingMode.HALF_EVEN);
            payKeep.add(p);
        }
        if (payAmount.setScale(2, RoundingMode.HALF_UP).compareTo(dec(orderAmount)) != 0)
            throw new Warning("Payment Detail Amount " + fmtMax(payAmount.doubleValue(), 4) + " Not Equal to Total Order Amount " + fmtMax(orderAmount, 4));
        if (payPct.setScale(2, RoundingMode.HALF_UP).compareTo(new BigDecimal("100")) != 0) throw new Warning("Payment Detail Total% not equal to 100");

        // ---- BLL.SaleOrder.Save / DAL.SetData
        Timestamp now = now();
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", r.id);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DocNo", r.id > 0 && old != null ? toInt(old.get("DocNo")) : toInt(r.docNo));     // the insert procedure draws its own DocNo
        h.put("BranchSrNo", r.id > 0 && old != null ? toInt(old.get("BranchSrNo")) : 0);
        h.put("DocDate", parseDate(r.docDate));
        h.put("OrderCatagoryId", 0);                                       // combordercat (hidden, no selection)
        h.put("CatagorySrNo", 0);                                          // txtcatsr (hidden)
        h.put("OrderSupCustId", r.customerId);
        h.put("SupplierRefNo", text(r.supplierRefNo));
        h.put("RefrenenceParty", 0);                                       // CmbAgreementNo (hidden)
        h.put("SupplierCustomerIdStockParty", 0);
        h.put("BrokerAgentSupCustId", 0);                                  // combsalesman (hidden)
        h.put("CommRate", 0.0);
        h.put("UomScheduleIdCmRate", 0);
        h.put("CommAmount", 0.0);
        h.put("PaymentTermsId", r.paymentTermId);
        h.put("OrderDueDays", r.dueDays);
        h.put("OrderDueDate", parseDate(r.dueDate));
        h.put("OrderExpiryDate", parseDate(r.expiryDate) != null ? parseDate(r.expiryDate) : LocalDate.now());   // `validate` (hidden)
        h.put("DeliveryTerm", text(r.deliveryTerm));
        h.put("DeliveryStartDate", parseDate(r.deliveryStartDate));
        h.put("DeliveryDays", r.deliveryDays);
        h.put("RemarksHeader", r.remarks == null ? "" : r.remarks);
        h.put("OrderStatus", text(r.status));
        h.put("IsAproved", false);
        h.put("EntryDate", now);
        h.put("EntryUser", u.getId());
        h.put("ModifyDate", now);
        h.put("ModifyUser", u.getId());
        h.put("PostUser", 0);
        h.put("PostState", false);
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchesId", u.getBranchesId());                            // So.BranchesId = UserAccount.BranchesId
        h.put("ProjectsId", u.getBranchesId());                            // So.ProjectsId = UserAccount.BranchesId (as the desktop does)
        h.put("NoOfCntnr", 0);
        h.put("PfiScId", 0);
        h.put("ActionId", r.id > 0 ? 2 : 1);
        h.put("FinancialYearId", sup.fy());
        h.put("IsValidate", false);
        h.put("BillCalculateTypeId", r.billTypeId);                        // CmbBillType (hidden): 2 "On Qty" after Load, 0 after Reset
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
        if (r.id > 0) for (Line l : removed) sup.setProcMap("Sp_SaleOrderDetail_Insert", removedDetail(id, l, itemNames.get(l.itemId)));
        int k = 0;
        for (Line l : lines) {
            int action = (r.id > 0 ? l.id : 0) <= 0 ? 1 : 2;
            limit = limit.add(dec(l.amount));
            sup.setProcMap("Sp_SaleOrderDetail_Insert", detail(id, l, r.id > 0 ? l.id : 0, action, rate, r.currencyId, fcyOf.get(k++), itemNames.get(l.itemId)));
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
        // Extra Items: the desktop removes the tab at Load, so the extra list Insert() loops over is always empty.
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
        if (r.customerId <= 0) throw new Warning("Customer Name Field is Required");
        String term = null;
        for (Map<String, Object> t : terms) if (toInt(t.get("Id")) == r.paymentTermId && r.paymentTermId > 0) term = str(t.get("TermsDescription")).trim();
        if (term == null) throw new Warning("Payment Term Field is Required");
        if ("Credit".equals(term) && r.dueDays == 0) throw new Warning("Due Days Field is Required");
        String dt = text(r.deliveryTerm);
        if (!dt.equals("Load") && !dt.equals("Ponch")) throw new Warning("Delivery Term Field is Required");
        String ex = text(r.exchangeRateText);
        if (multi) {
            if (r.currencyId <= 0) throw new Warning("Fcy Code Field is Required");
            if (ex.isEmpty() || ex.equals("0")) throw new Warning("Exchange Rate Field is Required");
            String fc = text(r.fcyAmountText);
            if (!sup.configBool("SaleOrderInsertWithoutRate") && (fc.isEmpty() || fc.equals("0"))) throw new Warning("Fcy Amount Rate Field is Required");
        } else {
            if (r.currencyId <= 0) throw new Warning("Please Configure Your Base Currency In configurations");
            if (ex.isEmpty() || ex.equals("0")) throw new Warning("Please Configure Your Base Currency Rate In configurations");
        }
        String st = text(r.status);
        if (!st.equals("Open") && !st.equals("Complete") && !st.equals("Cancel")) throw new Warning("Order Status Field is Required");
    }

    /** GenericProvider.SetProc(SaleOrderDetail): every non-virtual property, string properties left null are not sent. */
    private Map<String, Object> detail(int headId, Line l, int id, int action, double rate, int currencyId, double fcy, String itemName) {
        int discType = l.discountType;
        if (discType > 0 && l.discountPercent <= 0.0 && l.discountAmount <= 0.0) discType = 0;
        Map<String, Object> d = new LinkedHashMap<>();
        for (String k : List.of("BagPrice", "BagWeight", "RetailRate", "ItemPrice", "RateDiscount", "PackingAddLessOnRate")) d.put(k, 0.0);
        for (String k : List.of("PriceScheduleId", "RefDocId", "RefDocDetailId", "WarehouseId", "PackingTypeID", "ReferencePartyId",
                "CostCenterId", "LocationTypeId", "ItemConditionId", "SecondaryUomId")) d.put(k, 0);
        d.put("SecondaryUomQty", BigDecimal.ZERO);
        d.put("SecondaryUomItemRate", BigDecimal.ZERO);
        d.put("CommOnSale", false);
        d.put("Id", id);
        d.put("SaleOrderId", headId);
        d.put("OrderItemId", l.itemId);
        d.put("ItemDiscription", itemName == null ? "" : itemName);        // vd.ItemDiscription = the ItemName cell text
        d.put("JobLotId", 0);
        d.put("OrderItemUOMId", l.packUomId);
        d.put("ItemVariantId", l.variantId);
        d.put("CastingTypeId", l.castingTypeId);
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

    /** lstRemoveRecord entries (DeleteDetailRow): ActionTypeId 3, only the fields the desktop copies from the grid row. */
    private Map<String, Object> removedDetail(int headId, Line l, String itemName) {
        Map<String, Object> d = new LinkedHashMap<>();
        for (String k : List.of("BagPrice", "BagWeight", "RetailRate", "ItemPrice", "RateDiscount", "PackingAddLessOnRate", "ExchangeRate", "FcyAmount")) d.put(k, 0.0);
        for (String k : List.of("PriceScheduleId", "RefDocId", "RefDocDetailId", "WarehouseId", "PackingTypeID", "CurrencyId",
                "ReferencePartyId", "CostCenterId", "LocationTypeId", "ItemConditionId", "SecondaryUomId")) d.put(k, 0);
        d.put("SecondaryUomQty", BigDecimal.ZERO);
        d.put("SecondaryUomItemRate", BigDecimal.ZERO);
        d.put("CommOnSale", false);
        d.put("Id", l.id);
        d.put("SaleOrderId", headId);
        d.put("OrderItemId", l.itemId);
        d.put("ItemDiscription", itemName == null ? "" : itemName);
        d.put("JobLotId", 0);
        d.put("OrderItemUOMId", l.packUomId);
        d.put("ItemVariantId", l.variantId);
        d.put("CastingTypeId", l.castingTypeId);
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
        for (Map<String, Object> r : rows) s.add(toInt(ci(r, "Id")));
        return s;
    }

    public List<Map<String, Object>> attachmentList(int id) { header(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { header(id); return attachments.download(SCREEN, id, attachmentId); }

    private static LocalDate parseDate(String s) {
        if (s == null || s.isBlank()) return null;
        try { return LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))); } catch (RuntimeException e) { return null; }
    }
}
