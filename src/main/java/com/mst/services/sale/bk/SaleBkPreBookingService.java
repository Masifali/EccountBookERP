package com.mst.services.sale.bk;

import com.mst.models.UserAccount;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.*;

/**
 * Screen 761 PreBookingOrder = Architecture.WinApp.PreBookingAndDelivery.PreBookingOrder ("Pre Booking Order", tabs Form / History), DocumentTypeId 129.
 * BLL Architecture.BLL.Inventory.PreBookingOrder (0523), DAL Architecture.DAL.Inventory.PreBookingOrder.SetData (0376).
 *
 *   PreBookingOrder_Load :362     CmbCustomer / CmbCustomerHistory locked to UserAccount.SupplierCustomerId when > 0; AppId == 5: cost center combo (disabled) + CostCenterFill;
 *                                 rights of "PreBookingOrder" (Save / Print / Update); config RateEditableOnPrebookingOrder -> txtItemRate enabled;
 *                                 GenerateDocNo, SuppCustomerFill, PaymentTerms, DeliveryTerms, CommissionTypeFill, CommissionUOMFill, ItemFill, HistoryCombosFill
 *   GenerateDocNo :451            PreBookingOrder.GenerateCode = USP_PreBookingOrder_GetAllMethod Activity GenerateCode (@OrganizationId, @CompanyId, @DocumentTypeId 129, @FinancialYearId) -> DocNo
 *   CostCenterFill :479           UsersWithCostCenter.UsersWithCostCenter_AllocatedData = USP_UsersWithCostCenter_AllocatedData (CostCenterId / CostCenterName)
 *   SuppCustomerFill :511         CommonServices.getSupplierCustomersForDropdown(0, CostCenterId, "1,2,3,4,5,6,12,14,15,16,17") = usp_getSupplierCustomersForDropdown (Id / CompanyName)
 *   PaymentTerms :530             CommonServices.GetDueTermServiceBind = InvDueTerms.GetAll = Sp_InvDueTerms_GetAllMethod Activity GetAll (Id / TermsDescription)
 *   ItemFill :625                 CommonServices.ItemBindFromPricingSchedule(6) = Sp_ItemPricingSchedule_GetAllMethod Activity ReadByItemIdAndPriceLookUpId (ItemId / ItemName)
 *   CropYear :645                 ItemPricingSchedule.getCropYearFromPricingScheduleByItemId = usp_getCropYearFromPricingScheduleByItemId (Id / CropYear)
 *   bindRateUomAndItemPackUom :758  UOMSchedule.SearchByObject = Sp_UOMSchedule_GetAllMethod Activity ReadByItemID (Id / UOMCode / Equivalent)
 *   getRateRateUom :846           ItemPricingSchedule.GetItemRateAndUomIdByEffectedDate = Sp_ItemPricingSchedule_GetAllMethod Activity GetItemRateAndUomIdByEffectedDate
 *   HistoryCombosFill :696        PreBookingOrder.GetDataForDropDownFromPreBookingOrder = USP_GetDataForDropDownFromPreBookingOrder (Activity Customer; Id / ReferenceName)
 *   Insert :1423                  validations, then PreBookingOrder.Save -> DAL SetData (one transaction): USP_PreBookingOrder_Insert|_Update, USP_PreBookingOrderDetail_Insert per
 *                                 detail (removed rows first with ActionTypeId 3), [DAW].[USp_DocumentApprovalDetail_Insert]
 *   HistoryFill :1691             PreBookingOrder.FormHistory = usp_PreBookingOrder_FormHistory
 *   ReadById :1886 / GridDetailBind :2021   PreBookingOrder.GetByID = USP_PreBookingOrder_GetAllMethod Activity ReadById + ReadDetailByPreBookingOrderId
 */
@Service
public class SaleBkPreBookingService {
    private static final int DOC_TYPE = 129;
    private static final int PRICE_TYPE = 6;
    private static final String P_ORDER = "USP_PreBookingOrder_GetAllMethod";
    private static final String P_PRICE = "Sp_ItemPricingSchedule_GetAllMethod";

    private final SaleBkSupport bk;

    public SaleBkPreBookingService(SaleBkSupport bk) { this.bk = bk; }

    private static int i(Object o) { return SaleBkSupport.i(o); }
    private static double dbl(Object o) { return SaleBkSupport.dbl(o); }
    private static String s(Object o) { return SaleBkSupport.s(o); }
    private static Object ci(Map<String, Object> r, String k) { return SaleBkSupport.ci(r, k); }
    private static Map<String, Object> p(Object... kv) { return SaleBkSupport.p(kv); }

    /** A DateTimePicker value: the chosen day with the time of day of the moment the value is used. */
    private static Timestamp stamp(Object iso) {
        String v = s(iso);
        if (v.length() < 10) return new Timestamp(System.currentTimeMillis());
        return Timestamp.valueOf(LocalDateTime.of(LocalDate.parse(v.substring(0, 10)), LocalTime.now()));
    }

    // ------------------------------------------------------------------ load

    public Map<String, Object> init() {
        UserAccount u = bk.user();
        Map<String, Boolean> r = bk.sup().rights("PreBookingOrder");
        Map<String, Object> d = bk.basics();
        d.put("canSave", Boolean.TRUE.equals(r.get("save")));
        d.put("canUpdate", Boolean.TRUE.equals(r.get("update")));
        d.put("canPrint", Boolean.TRUE.equals(r.get("print")));
        int app = bk.appId();
        d.put("appId", app);
        int sc = u.getSupplierCustomerId() == null ? 0 : u.getSupplierCustomerId();
        d.put("supplierCustomerId", sc);
        d.put("rateEditable", bk.sup().configBool("RateEditableOnPrebookingOrder"));
        int days = bk.sup().configInt("DefaultDaysToLessFromHistoryFromDate");
        d.put("historyDays", days > 0 ? days : 3);
        List<Map<String, Object>> cc = new ArrayList<>();
        if (app == 5) {
            Map<String, Object> q = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
            if (u.getId() != null && u.getId() > 0) q.put("UserId", u.getId());
            for (Map<String, Object> x : bk.steel().table("[dbo].[USP_UsersWithCostCenter_AllocatedData]", q).rows)
                cc.add(p("Id", ci(x, "CostCenterId"), "name", ci(x, "CostCenterName")));
        }
        d.put("costCenters", cc);
        d.put("docNo", docNo());
        int firstCc = cc.isEmpty() ? 0 : i(cc.get(0).get("Id"));
        d.put("customers", customers(firstCc));
        List<Map<String, Object>> terms = new ArrayList<>();
        for (Map<String, Object> x : paymentTermRows()) terms.add(p("Id", ci(x, "Id"), "name", ci(x, "TermsDescription")));
        d.put("paymentTerms", terms);
        d.put("items", items());
        d.put("historyCustomers", historyCustomers(firstCc));
        return d;
    }

    /** GenerateDocNo: 0 when the procedure returns no row (the text box then keeps its previous value). */
    public int docNo() {
        UserAccount u = bk.user();
        List<Map<String, Object>> rows = bk.steel().table(P_ORDER, p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC_TYPE, "FinancialYearId", bk.sup().fy(), "Activity", "GenerateCode")).rows;
        return rows.isEmpty() ? 0 : i(ci(rows.get(0), "DocNo"));
    }

    /** SuppCustomerFill(CostCenterId): {Id, CompanyName}. */
    public List<Map<String, Object>> customers(int costCenterId) {
        UserAccount u = bk.user();
        Map<String, Object> q = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "UserId", u.getId(), "AppId", bk.appId());
        SaleBkSupport.nz(q, "CostCenterId", costCenterId);
        q.put("CustomerGroupIds", "1,2,3,4,5,6,12,14,15,16,17");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : bk.steel().table("usp_getSupplierCustomersForDropdown", q).rows)
            out.add(p("Id", ci(x, "Id"), "name", ci(x, "CompanyName")));
        return out;
    }

    private List<Map<String, Object>> paymentTermRows() {
        UserAccount u = bk.user();
        return bk.steel().table("Sp_InvDueTerms_GetAllMethod", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll")).rows;
    }

    /** btnRefresh: PaymentTerms again (same rows). */
    public List<Map<String, Object>> paymentTerms() {
        List<Map<String, Object>> terms = new ArrayList<>();
        for (Map<String, Object> x : paymentTermRows()) terms.add(p("Id", ci(x, "Id"), "name", ci(x, "TermsDescription")));
        return terms;
    }

    /** ItemFill: {Id = ItemId, name = ItemName}. */
    public List<Map<String, Object>> items() {
        UserAccount u = bk.user();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : bk.steel().table(P_PRICE, p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "PriceTypeId", PRICE_TYPE,
                "Activity", "ReadByItemIdAndPriceLookUpId")).rows)
            out.add(p("Id", ci(x, "ItemId"), "name", ci(x, "ItemName")));
        return out;
    }

    /** HistoryCombosFill: {Id, name = ReferenceName} of the Customer activity. */
    public List<Map<String, Object>> historyCustomers(int costCenterId) {
        UserAccount u = bk.user();
        Map<String, Object> q = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "AppId", bk.appId(), "UserId", u.getId(),
                "DocumentTypeIds", "129", "Activity", "Customer");
        SaleBkSupport.nz(q, "CostCenterId", costCenterId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : bk.steel().table("USP_GetDataForDropDownFromPreBookingOrder", q).rows)
            out.add(p("Id", ci(x, "Id"), "name", ci(x, "ReferenceName")));
        return out;
    }

    // ------------------------------------------------------------------ detail box

    /** CropYear(): {Id, name = CropYear} of the item as at the document date. */
    public List<Map<String, Object>> cropYears(int itemId, String docDate) {
        UserAccount u = bk.user();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : bk.steel().table("usp_getCropYearFromPricingScheduleByItemId", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "PriceTypeId", PRICE_TYPE, "EffectedDate", stamp(docDate))).rows)
            out.add(p("Id", ci(x, "Id"), "name", ci(x, "CropYear")));
        return out;
    }

    /** bindRateUomAndItemPackUom(): {Id, name = UOMCode, equivalent}. */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = bk.user();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : bk.steel().table("Sp_UOMSchedule_GetAllMethod", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "Activity", "ReadByItemID")).rows)
            out.add(p("Id", ci(x, "Id"), "name", ci(x, "UOMCode"), "equivalent", ci(x, "Equivalent")));
        return out;
    }

    /** getRateRateUom(): the first row {scheduleId, rateUomId, rate}; found=false when there is none. */
    public Map<String, Object> rate(int itemId, int cropYearId, String docDate) {
        if (itemId <= 0 || cropYearId <= 0) return p("found", false);
        UserAccount u = bk.user();
        List<Map<String, Object>> rows = bk.steel().table(P_PRICE, p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId,
                "PriceTypeId", PRICE_TYPE, "CropYearId", cropYearId, "EffectedDate", stamp(docDate), "Activity", "GetItemRateAndUomIdByEffectedDate")).rows;
        if (rows.isEmpty()) return p("found", false);
        Map<String, Object> r = rows.get(0);
        return p("found", true, "scheduleId", i(ci(r, "Id")), "rateUomId", i(ci(r, "RateUomId")), "rate", dbl(ci(r, "ItemPrice")));
    }

    // ------------------------------------------------------------------ history

    /** HistoryFill(): the 25 dtHistoryData columns; dates are always sent (the date pickers have no check box). */
    public List<Map<String, Object>> history(Map<String, String> q) {
        UserAccount u = bk.user();
        Map<String, Boolean> r = bk.sup().rights("PreBookingOrder");
        Map<String, Object> pr = new LinkedHashMap<>();
        pr.put("OrganizationId", u.getOrganizationId());
        pr.put("CompanyId", u.getCompanyId());
        pr.put("AppId", bk.appId());
        pr.put("EntryUser", u.getId());
        pr.put("DocumentTypeId", DOC_TYPE);
        pr.put("CanViewAllRecord", Boolean.TRUE.equals(r.get("viewAll")));
        pr.put("FinancialYearId", bk.sup().fy());
        Timestamp from = stamp(q.get("fromDate")), to = stamp(q.get("toDate"));
        String by = s(q.get("dateBy"));
        if ("entry".equals(by)) { pr.put("EntryFromDate", from); pr.put("EntryToDate", to); }
        else if ("modify".equals(by)) { pr.put("ModifyFromDate", from); pr.put("ModifyToDate", to); }
        else if ("approved".equals(by)) { pr.put("ApprovedFromDate", from); pr.put("ApprovedToDate", to); }
        else { pr.put("DocDateFrom", from); pr.put("DocDateTo", to); }
        SaleBkSupport.nz(pr, "PoSrFrom", i(q.get("fromDocNo")));
        SaleBkSupport.nz(pr, "PoSrTo", i(q.get("toDocNo")));
        SaleBkSupport.nz(pr, "SupplierCustomerId", i(q.get("customerId")));
        SaleBkSupport.nz(pr, "CostCenterId", i(q.get("costCenterId")));
        String[][] map = {{"Id", "Id"}, {"DocNo", "DocNo"}, {"DocDate", "DocDate"}, {"CustomerName", "CustomerName"}, {"PaymentTerm", "TermsDescription"}, {"DueDays", "DueDays"},
            {"DueDate", "DueDate"}, {"DeliveryTerm", "DeliveryTerm"}, {"DeliveryStartDate", "DeliveryStartDate"}, {"DeliveryDays", "DeliveryDays"}, {"ExpiryDate", "OrderExpiryDate"},
            {"CommissionAgent", "CommissionAgent"}, {"CommType", "CommissionType"}, {"CommRate", "CommRate"}, {"CommAmount", "CommAmount"},
            {"OtherCommissionAgent", "OtherCommissionAgent"}, {"OtherCommType", "OtherCommissionType"}, {"OtherCommRate", "OtherCommissionRate"},
            {"OtherCommAmount", "OtherCommissionAmount"}, {"EntryUser", "UserName"}, {"EntryDate", "EntryDate"}, {"ModifyUser", "ModifyUserName"}, {"ModifyDate", "ModifyDate"},
            {"NoOfAttachments", "NoOfAttachments"}, {"Remarks", "RemarksHeader"}};
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : bk.steel().table("usp_PreBookingOrder_FormHistory", pr).rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String[] m : map) o.put(m[0], ci(x, m[1]));
            out.add(o);
        }
        return out;
    }

    /** PreBookingOrder.GetByID: the header and the detail rows (ReadById, ReadDetailByPreBookingOrderId). found=false when the order does not exist. */
    public Map<String, Object> get(int id) {
        List<Map<String, Object>> h = bk.steel().table(P_ORDER, p("Id", id, "Activity", "ReadById")).rows;
        if (h.isEmpty()) throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\nParameter name: index");
        Map<String, Object> r = h.get(0);
        Map<String, Object> o = new LinkedHashMap<>();
        for (String k : new String[]{"DocDate", "DocNo", "OrderSupCustId", "PaymentTermsId", "OrderDueDays", "OrderDueDate", "DeliveryTerm", "DeliveryStartDate", "DeliveryDays",
                "OrderExpiryDate", "BrokerAgentSupCustId", "CommissionType", "CommRate", "UomScheduleIdCmRate", "CommAmount", "CommissionRemarks", "OtherCommissionAgentId",
                "OtherCommissionType", "OtherCommissionRate", "OtherCommissionUom", "OtherCommissionAmount", "OtherCommissionRemarks", "RemarksHeader"})
            o.put(k, ci(r, k));
        List<Map<String, Object>> det = new ArrayList<>();
        for (Map<String, Object> x : bk.steel().table(P_ORDER, p("Id", id, "Activity", "ReadDetailByPreBookingOrderId")).rows) {
            Map<String, Object> e = new LinkedHashMap<>();
            for (String k : new String[]{"Id", "PriceScheduleId", "OrderItemId", "ItemName", "CropYearId", "Crop", "OrderItemUOMId", "UOMCode", "OrderItemQty", "NetWeight",
                    "OrderItemRate", "OrderItemRateUOMId", "RateUom", "Amount", "OrderRemarks", "CostCenterId"})
                e.put(k, ci(x, k));
            det.add(e);
        }
        o.put("details", det);
        return o;
    }

    // ------------------------------------------------------------------ save

    /** FormValidation() of the form, same order and messages. */
    private void formValidation(Map<String, Object> b) {
        String docNo = s(b.get("docNo")).trim();
        if (docNo.isEmpty() || "0".equals(docNo)) throw new IllegalArgumentException("Doc No Field is Required");
        if (bk.appId() == 5 && i(b.get("costCenterId")) == 0) throw new IllegalArgumentException("Cost Center Field is Required");
        if (i(b.get("customerId")) <= 0) throw new IllegalArgumentException("Customer Name Field is Required");
        int term = i(b.get("paymentTermId"));
        if (term == 0) throw new IllegalArgumentException("Payment Term Field is Required");
        if (term == 2 && i(b.get("dueDays")) == 0) throw new IllegalArgumentException("Due Days Field is Required");
        if (i(b.get("deliveryTermId")) == 0) throw new IllegalArgumentException("Delivery Term Field is Required");
    }

    @SuppressWarnings("unchecked")
    @Transactional
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = bk.user();
        List<Map<String, Object>> rows = (List<Map<String, Object>>) b.get("rows");
        if (rows == null || rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        formValidation(b);
        int recId = i(b.get("recId"));
        int costCenterId = i(b.get("costCenterId"));
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> po = new LinkedHashMap<>();
        if (recId > 0) po.put("Id", recId);
        po.put("DocumentTypeId", DOC_TYPE);
        po.put("OrganizationId", u.getOrganizationId());
        po.put("CompanyId", u.getCompanyId());
        po.put("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        po.put("FinancialYearId", bk.sup().fy());
        po.put("EntryUser", u.getId());
        po.put("ModifyUser", u.getId());
        po.put("DocNo", i(b.get("docNo")));
        po.put("DocDate", stamp(b.get("docDate")));
        po.put("OrderSupCustId", i(b.get("customerId")));
        po.put("RemarksHeader", s(b.get("remarks")));
        po.put("PaymentTermsId", i(b.get("paymentTermId")));
        po.put("OrderDueDays", i(b.get("dueDays")));
        po.put("OrderDueDate", stamp(b.get("dueDate")));
        po.put("DeliveryTerm", s(b.get("deliveryTerm")));
        po.put("DeliveryStartDate", stamp(b.get("deliveryStartDate")));
        po.put("DeliveryDays", i(b.get("deliveryDays")));
        po.put("OrderExpiryDate", stamp(b.get("expiryDate")));

        Map<String, Object> c = (Map<String, Object>) b.getOrDefault("comm", new HashMap<String, Object>());
        int agentId = i(c.get("agentId"));
        double commAmount = dbl(c.get("amount")), commRate = dbl(c.get("rate"));
        if ((commAmount > 0.0 || commRate > 0.0) && agentId == 0) throw new IllegalArgumentException("Please Select Commission Agent when Commission Amount or Rate is Present...");
        if (agentId > 0 && (commAmount == 0.0 || commRate == 0.0)) {
            if (commAmount == 0.0) throw new IllegalArgumentException("Commission Amount Required when Commission Agent is Selected...");
            if (commRate == 0.0) throw new IllegalArgumentException("Commission Rate Required when Commission Agent is Selected...");
        }
        if (agentId > 0) {
            po.put("BrokerAgentSupCustId", agentId);
            String type = s(c.get("type"));
            po.put("CommissionType", type);
            po.put("CommRate", commRate);
            po.put("UomScheduleIdCmRate", "Comm Weight".equals(type) ? i(c.get("uom")) : 0);
            po.put("CommAmount", commAmount);
            po.put("CommissionRemarks", s(c.get("remarks")));
        }

        Map<String, Object> o = (Map<String, Object>) b.getOrDefault("other", new HashMap<String, Object>());
        int otherId = i(o.get("agentId"));
        double otherAmount = dbl(o.get("amount")), otherRate = dbl(o.get("rate"));
        if ((otherAmount > 0.0 || otherRate > 0.0) && otherId == 0) throw new IllegalArgumentException("Please Select OtherCommission Agent when OtherCommission Amount or Rate is Present...");
        if (otherId > 0 && (otherAmount == 0.0 || otherRate == 0.0)) {
            if (otherAmount == 0.0) throw new IllegalArgumentException("OtherCommission Amount Required when OtherCommission Agent is Selected...");
            if (otherRate == 0.0) throw new IllegalArgumentException("OtherCommission Rate Required when OtherCommission Agent is Selected...");
        }
        if (otherId > 0) {
            po.put("OtherCommissionAgentId", otherId);
            String type = s(o.get("type"));
            po.put("OtherCommissionType", type);
            po.put("OtherCommissionRate", otherRate);
            po.put("OtherCommissionUom", "Comm Weight".equals(type) ? (double) i(o.get("uom")) : 0.0);
            po.put("OtherCommissionAmount", otherAmount);
            po.put("OtherCommissionRemarks", s(o.get("remarks")));
        }

        // PreBookingOrderDetailList: the removed rows first (update only), then the grid rows
        List<Map<String, Object>> details = new ArrayList<>();
        List<Map<String, Object>> removed = (List<Map<String, Object>>) b.get("removed");
        if (recId > 0 && removed != null) {
            for (Map<String, Object> x : removed) {
                Map<String, Object> d = new LinkedHashMap<>();
                d.put("Id", i(x.get("Id")));
                d.put("PriceScheduleId", i(x.get("ScheduleId")));
                d.put("OrderItemId", i(x.get("ItemId")));
                if (!Boolean.TRUE.equals(x.get("viaKey"))) { d.put("Crop", s(x.get("CropYear"))); d.put("CropYearId", i(x.get("CropYearId"))); }
                d.put("OrderItemUOMId", i(x.get("PackUomId")));
                d.put("OrderItemQty", dbl(x.get("ItemQty")));
                d.put("NetWeight", dbl(x.get("Weight")));
                d.put("OrderItemRate", dbl(x.get("ItemRate")));
                d.put("OrderItemRateUOMId", i(x.get("RateUomId")));
                d.put("Amount", dbl(x.get("ItemAmount")));
                d.put("OrderRemarks", s(x.get("Remarks")));
                d.put("ActionTypeId", 3);
                details.add(d);
            }
        }
        int rowNo = 0;
        for (Map<String, Object> r : rows) {
            rowNo++;
            Map<String, Object> d = new LinkedHashMap<>();
            int id = recId > 0 ? i(r.get("Id")) : 0;
            d.put("Id", id);
            d.put("ActionTypeId", id <= 0 ? 1 : 2);
            int sched = i(r.get("ScheduleId"));
            d.put("PriceScheduleId", sched);
            if (sched == 0) throw new IllegalArgumentException("PriceScheduleId Field Required in Detail Grid Row No: " + rowNo);
            d.put("ItemPrice", dbl(r.get("ItemRate")));
            if (i(r.get("ItemId")) <= 0) throw new IllegalArgumentException("Item Field Required in Detail Grid Row No: " + rowNo);
            d.put("OrderItemId", i(r.get("ItemId")));
            if (i(r.get("PackUomId")) <= 0) throw new IllegalArgumentException("Item UOM Field Required in Detail Grid Row No: " + rowNo);
            d.put("OrderItemUOMId", i(r.get("PackUomId")));
            if (!(dbl(r.get("ItemQty")) > 0.0)) throw new IllegalArgumentException("Item Qty Field Required in Detail Grid Row No: " + rowNo);
            d.put("OrderItemQty", dbl(r.get("ItemQty")));
            if (!(dbl(r.get("Weight")) > 0.0)) throw new IllegalArgumentException("Weight Field Required in Detail Grid Row No: " + rowNo);
            d.put("NetWeight", dbl(r.get("Weight")));
            if (!(dbl(r.get("ItemRate")) > 0.0)) throw new IllegalArgumentException("Rate Field Required in Detail Grid Row No: " + rowNo);
            d.put("OrderItemRate", dbl(r.get("ItemRate")));
            int rateUom = i(r.get("RateUomId"));
            d.put("OrderItemRateUOMId", rateUom);
            if (rateUom == 0) throw new IllegalArgumentException("RateUom Field Required in Detail Grid Row No: " + rowNo);
            if (!(dbl(r.get("ItemAmount")) > 0.0)) throw new IllegalArgumentException("Amount Field Required in Detail Grid Row No: " + rowNo);
            d.put("Amount", dbl(r.get("ItemAmount")));
            d.put("CropYearId", i(r.get("CropYearId")));
            d.put("Crop", s(r.get("CropYear")));
            d.put("TotalAmount", dbl(r.get("ItemAmount")));
            d.put("OrderRemarks", s(r.get("Remarks")));
            d.put("CostCenterId", costCenterId);
            details.add(d);
        }

        // PreBookingOrder.Save
        po.put("EntryDate", now);
        po.put("ModifyDate", now);
        String proc;
        if (recId == 0) {
            po.put("ActionId", 1);
            for (Map<String, Object> d : details)
                if (i(d.get("Id")) > 0 || i(d.get("ActionTypeId")) != 1)
                    throw new IllegalArgumentException("Cannot insert new order with existing detail IDs or different action types.");
            proc = "USP_PreBookingOrder_Insert";
        } else {
            po.put("ActionId", 2);
            proc = "USP_PreBookingOrder_Update";
        }
        // DAL SetData (one transaction)
        if (details.isEmpty()) throw new IllegalArgumentException("PreBookingOrderDetailList not found");
        int num = bk.sup().setProcMap(proc, bk.steel().full(proc, po));
        int id = num > 0 ? num : recId;
        double limit = 0.0;
        for (Map<String, Object> d : details) {
            d.put("PreBookingOrderId", id);
            if (i(d.get("OrderItemId")) <= 0) throw new IllegalArgumentException("ItemId not found in detail");
            if (i(d.get("OrderItemUOMId")) <= 0) throw new IllegalArgumentException("PackUomId not found in detail");
            if (i(d.get("OrderItemRateUOMId")) <= 0) throw new IllegalArgumentException("RateUomId not found in detail");
            if (!(dbl(d.get("OrderItemRate")) > 0.0)) throw new IllegalArgumentException("Rate not found in detail");
            if (!(dbl(d.get("Amount")) > 0.0)) throw new IllegalArgumentException("Amount not found in detail");
            if (i(d.get("ActionTypeId")) != 3) limit += dbl(d.get("Amount"));
            String dp = "USP_PreBookingOrderDetail_Insert";
            bk.sup().setProcMap(dp, bk.steel().full(dp, d));
        }
        String ap = "[DAW].[USp_DocumentApprovalDetail_Insert]";
        bk.sup().setProcMap(ap, bk.steel().full(ap, p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", DOC_TYPE, "Id", id,
                "LimitAmount", limit)));
        return p("id", id, "docNo", i(b.get("docNo")),
                "message", (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + i(b.get("docNo")));
    }
}
