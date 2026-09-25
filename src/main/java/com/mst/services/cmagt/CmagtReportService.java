package com.mst.services.cmagt;

import com.mst.repositories.cmagt.CmagtReportRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.text.SimpleDateFormat;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Commission Trading reports - one method per desktop form, each reproducing that form's
 * GridBind()/GridDbCall() and the BLL method it calls, parameter by parameter.
 *
 *   sale-order                 frmSaleOrderCmagtReport.GridBind (:535-586)
 *                              -> saleOrderMaster.SaleOrder_Report (BLL 0489:414-672)
 *                              -> [cmagt].[USP_saleOrderMaster_Report]
 *   purchase-order             frmPurchaseOrderCmagtReport.GridBind (:519-567)
 *                              -> purchaseOrderMaster.PurchaseOrder_Report (BLL 0490:459-717)
 *                              -> [cmagt].[USP_purchaseOrderMaster_Report]
 *   grn-supplier-loading       frmGrnSupplierLoadingCmagtReport.GridBind (:500-536)
 *                              -> grnSupplierLoadingMaster.GrnSupplierLoadingMaster_Report (BLL 0488:382-612)
 *                              -> [cmagt].[USP_grnSupplierLoadingMaster_Report]
 *   gdn-buyer-dispatch         frmGdnBuyerDispatchedCmagtReport.GridBind (:500-536)
 *                              -> gdnBuyerDispatchMaster.GdnBuyerDispatchMaster_Report (BLL 0487:382-612)
 *                              -> [cmagt].[USP_gdnBuyerDispatchMaster_Report]
 *   agent-trade-bill-register  CommissionAgentTrade_Register.GridDbCall (:452-500)
 *                              -> InvCommAgentTradeBill.CommissionTradeRegister (BLL 0547)
 *                              -> [dbo].[USP_CommissionAgentTrade_Register]
 *
 * Tenancy: OrganizationId / CompanyId / BranchesId / FinancialYearId always come from the
 * signed-in session (UserAccount.* and clsGlobalVariables.ActiveYr.Id on the desktop), never
 * from the request. @BranchesId and @FinancialYearId have NO default in the four [cmagt]
 * procedures and are matched with equality (h.BranchId = @BranchesId), so they are mandatory.
 */
@Service
public class CmagtReportService {

    public static final String SALE_ORDER = "sale-order";
    public static final String PURCHASE_ORDER = "purchase-order";
    public static final String GRN_SUPPLIER_LOADING = "grn-supplier-loading";
    public static final String GDN_BUYER_DISPATCH = "gdn-buyer-dispatch";
    public static final String AGENT_TRADE_BILL_REGISTER = "agent-trade-bill-register";

    @Autowired
    private CmagtReportRepository repository;

    @Autowired
    private CurrentUserContext currentUserContext;

    // ================================================================== reports

    public List<Map<String, Object>> report(String report, Map<String, String> q) {
        switch (report) {
            case SALE_ORDER:                return saleOrder(q);
            case PURCHASE_ORDER:            return purchaseOrder(q);
            case GRN_SUPPLIER_LOADING:      return grnOrGdn("[cmagt].[USP_grnSupplierLoadingMaster_Report]", q);
            case GDN_BUYER_DISPATCH:        return grnOrGdn("[cmagt].[USP_gdnBuyerDispatchMaster_Report]", q);
            case AGENT_TRADE_BILL_REGISTER: return tradeBillRegister(q);
            default: throw new IllegalArgumentException("Unknown Commission Trading report: " + report);
        }
    }

    /** BLL 0489 SaleOrder_Report - parameter order and guards as in the BLL. */
    private List<Map<String, Object>> saleOrder(Map<String, String> q) {
        LinkedHashMap<String, Object> p = tenancyWithBranchAndYear();
        dates(p, q);
        docNos(p, q);
        entryModifyDates(p, q);
        putInt(p, "ItemCategoryId", q, "itemCategoryId");
        putInt(p, "ItemId", q, "itemId");
        putInt(p, "BuyerId", q, "buyerId");
        /* obj.DeliverToPartyId is sent as @ShipToPartyId by this BLL (0489) - the proc matches it
           against h.DeliveryToPartyId. */
        putInt(p, "ShipToPartyId", q, "deliverToPartyId");
        putStr(p, "ShipToAddress", q, "shipToAddress");
        putInt(p, "CommissionAgentId", q, "agentId");
        putInt(p, "StatusId", q, "statusId");
        approval(p, q);
        putStr(p, "Activity", q, "reportType");
        /* ParentCategoryIds (GetSelectedIdsFromMultiSelectionCombo) - comma separated ids. */
        putStr(p, "ParentItemIds", q, "parentItemIds");
        return repository.exec("[cmagt].[USP_saleOrderMaster_Report]", p);
    }

    /**
     * BLL 0490 PurchaseOrder_Report. frmPurchaseOrderCmagtReport:535 computes the selected parent
     * category ids but never assigns them to obj.ParentCategoryIds, so @ParentItemIds is never sent
     * by the desktop and is not sent here.
     */
    private List<Map<String, Object>> purchaseOrder(Map<String, String> q) {
        LinkedHashMap<String, Object> p = tenancyWithBranchAndYear();
        dates(p, q);
        docNos(p, q);
        entryModifyDates(p, q);
        putInt(p, "ItemCategoryId", q, "itemCategoryId");
        putInt(p, "ItemId", q, "itemId");
        putInt(p, "CommissionAgentId", q, "agentId");
        putInt(p, "StatusId", q, "statusId");
        approval(p, q);
        putStr(p, "Activity", q, "reportType");
        putInt(p, "DeliveryToPartyId", q, "deliverToPartyId");
        putInt(p, "SupplierId", q, "supplierId");
        putStr(p, "ShipToAddress", q, "shipToAddress");
        return repository.exec("[cmagt].[USP_purchaseOrderMaster_Report]", p);
    }

    /**
     * BLL 0488 / 0487 - identical parameter builders. The two forms have no doc-no range, no
     * status and no approval combo, and they never assign obj.ParentCategoryIds.
     *
     * DESKTOP PARITY, flagged: the forms never set obj.ApprovedFilter, so it stays null and the
     * BLL's `if (obj.ApprovedFilter != "All")` is TRUE - @IsApproved is sent with the CLR default
     * false on every run. Both procedures filter `(@IsApproved IS NULL OR h.IsApproved =
     * @IsApproved)`, so the desktop reports list UN-approved GRN / GDN documents only. That is
     * reproduced here, not corrected.
     */
    private List<Map<String, Object>> grnOrGdn(String proc, Map<String, String> q) {
        LinkedHashMap<String, Object> p = tenancyWithBranchAndYear();
        dates(p, q);
        entryModifyDates(p, q);
        putInt(p, "ItemCategoryId", q, "itemCategoryId");
        putInt(p, "ItemId", q, "itemId");
        putInt(p, "BuyerId", q, "buyerId");
        putInt(p, "CommissionAgentId", q, "agentId");
        putInt(p, "SupplierId", q, "supplierId");
        p.put("IsApproved", Boolean.FALSE);
        putStr(p, "Activity", q, "reportType");
        putInt(p, "DeliveryToPartyId", q, "deliverToPartyId");
        putStr(p, "ShipToAddress", q, "shipToAddress");
        return repository.exec(proc, p);
    }

    /**
     * BLL 0547 CommissionTradeRegister. No branch / year parameters exist on this procedure.
     * GridDbCall never assigns obj.CommissionAgentId (the Comm. Agent combo is bound but not
     * read) and obj.PaymenetTermId is never sent by the BLL - neither is sent here.
     * btnshow_Click refuses to run without a report type ("Please Select Activity First...").
     */
    private List<Map<String, Object>> tradeBillRegister(Map<String, String> q) {
        String activity = str(q, "reportType");
        if (activity.isEmpty()) throw new IllegalArgumentException("Please Select Activity First...");
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", currentUserContext.currentOrganizationId());
        p.put("CompanyId", currentUserContext.currentCompanyId());
        dates(p, q);
        docNos(p, q);
        entryModifyDates(p, q);
        putInt(p, "ParentCategoryId", q, "parentCategoryId");
        putInt(p, "ItemTypeId", q, "itemTypeId");
        putInt(p, "ItemCategoryId", q, "itemCategoryId");
        putStr(p, "VehicleNo", q, "vehicleNo");
        putStr(p, "DeliveryTerm", q, "deliveryTerm");
        putInt(p, "BrokerId", q, "brokerId");
        putInt(p, "ItemId", q, "itemId");
        putInt(p, "TradingGlAccountId", q, "glAccountId");
        putInt(p, "SupplierId", q, "supplierId");
        putInt(p, "CustomerId", q, "customerId");
        p.put("Activity", activity);
        approval(p, q);
        return repository.exec("[dbo].[USP_CommissionAgentTrade_Register]", p);
    }

    // ============================================================ filter combos

    /**
     * The report's own filter-combo source (ComboDbCall / ComboFill). Every form sets only
     * OrganizationId and CompanyId, so @Activity / @DocumentTypeIds / @ParentItems are omitted
     * and the procedure returns every activity; the page splits the rows on Activity the way
     * ComboBind does. Rows are Id, ReferenceName, ParentCategoryId, Activity (the trade-bill
     * procedure has no ParentCategoryId).
     */
    public Map<String, Object> filters(String report) {
        String proc;
        switch (report) {
            case SALE_ORDER:                proc = "[cmagt].[USP_GetDataForDropDownFromsaleOrderMaster]"; break;
            case PURCHASE_ORDER:            proc = "[cmagt].[USP_GetDataForDropDownFrompurchaseOrderMaster]"; break;
            case GRN_SUPPLIER_LOADING:      proc = "[cmagt].[USP_GetDataForDropDownFromgrnSupplierLoadingMaster]"; break;
            case GDN_BUYER_DISPATCH:        proc = "[cmagt].[USP_GetDataForDropDownFromgdnBuyerDispatchMaster]"; break;
            case AGENT_TRADE_BILL_REGISTER: proc = "[dbo].[USP_DropDownFillFromInvCommAgentTradeBill]"; break;
            default: throw new IllegalArgumentException("Unknown Commission Trading report: " + report);
        }
        int org = currentUserContext.currentOrganizationId();
        int comp = currentUserContext.currentCompanyId();
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", org);
        p.put("CompanyId", comp);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", repository.exec(proc, p));
        /* InitializeComponentMethod: From date = today - DefaultDaysToLessFromHistoryFromDate when
           > 0, else today - 3 (the four [cmagt] report forms). */
        int days = 0;
        try { days = (int) Double.parseDouble(repository.config(org, comp, "DefaultDaysToLessFromHistoryFromDate")); }
        catch (Exception ignored) { days = 0; }
        out.put("defaultDaysToLessFromHistoryFromDate", days);
        return out;
    }

    // ================================================================== helpers

    private LinkedHashMap<String, Object> tenancyWithBranchAndYear() {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", currentUserContext.currentOrganizationId());
        p.put("CompanyId", currentUserContext.currentCompanyId());
        p.put("BranchesId", currentUserContext.currentBranchId());
        p.put("FinancialYearId", currentUserContext.currentFinancialYearId());
        return p;
    }

    /** obj.FromDate / obj.ToDate are assigned from the date boxes unconditionally, whatever date
        type radio is on - so the document-date filter is always applied. */
    private void dates(LinkedHashMap<String, Object> p, Map<String, String> q) {
        java.sql.Date from = date(q, "fromDate");
        java.sql.Date to = date(q, "toDate");
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
    }

    private void docNos(LinkedHashMap<String, Object> p, Map<String, String> q) {
        putInt(p, "FromDocNo", q, "fromDocNo");
        putInt(p, "ToDocNo", q, "toDocNo");
    }

    /** rdentrydate / rdmodifydate additionally set the entry / modify range from the same boxes. */
    private void entryModifyDates(LinkedHashMap<String, Object> p, Map<String, String> q) {
        String type = str(q, "dateType");
        java.sql.Date from = date(q, "fromDate");
        java.sql.Date to = date(q, "toDate");
        if ("entry".equalsIgnoreCase(type)) {
            if (from != null) p.put("EntryFromDate", from);
            if (to != null) p.put("EntryToDate", to);
        } else if ("modify".equalsIgnoreCase(type)) {
            if (from != null) p.put("ModifyFromDate", from);
            if (to != null) p.put("ModifyToDate", to);
        }
    }

    /** CmbIsApproved: blank or "All" -> ApprovedFilter = "All" -> @IsApproved omitted;
        "Approved" -> 1; anything else ("UnApproved") -> 0. */
    private void approval(LinkedHashMap<String, Object> p, Map<String, String> q) {
        String a = str(q, "approved");
        if (a.isEmpty() || "All".equalsIgnoreCase(a)) return;
        p.put("IsApproved", "Approved".equalsIgnoreCase(a));
    }

    private static void putInt(LinkedHashMap<String, Object> p, String name, Map<String, String> q, String key) {
        int v = 0;
        String s = str(q, key);
        if (!s.isEmpty()) {
            try { v = (int) Double.parseDouble(s); } catch (NumberFormatException e) { v = 0; }
        }
        if (v != 0) p.put(name, v);
    }

    private static void putStr(LinkedHashMap<String, Object> p, String name, Map<String, String> q, String key) {
        String s = str(q, key);
        if (!s.isEmpty()) p.put(name, s);
    }

    private static String str(Map<String, String> q, String key) {
        String v = q == null ? null : q.get(key);
        return v == null ? "" : v.trim();
    }

    private static java.sql.Date date(Map<String, String> q, String key) {
        String s = str(q, key);
        if (s.isEmpty()) return null;
        try {
            SimpleDateFormat f = new SimpleDateFormat("yyyy-MM-dd");
            f.setLenient(false);
            return new java.sql.Date(f.parse(s).getTime());
        } catch (Exception e) {
            return null;
        }
    }
}
