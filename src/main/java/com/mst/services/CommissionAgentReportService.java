package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

import static com.mst.services.AccountReportsHSupport.col;
import static com.mst.services.AccountReportsHSupport.params;
import static com.mst.services.AccountReportsHSupport.toInt;

/**
 * Screen 886 "Commission Agent Report" (ScreenName CommissionAgentRpt, TargetUrl
 * Architecture.WinApp.Account_Reports.CommissionAgentLedger - CommissionAgentLedger.cs).
 *
 *   Combos   CommissionAgentFill -> SupplierCustomer.GetSupplierCustomerAndCommissionAgentFromPurchaseAndSales
 *            (BLL 0600) -> [Sp_SupplierCustomer_GetAllMethod] @OrganizationId, @CompanyId, @ActionId=1,
 *            @Activity='GetSupplierCustomerAndCommissionAgentFromPurchaseAndSales' (no @PartyType).
 *            PartyType 'CommissionAgent' -> Commission Agent; 'Supplier' or 'Customer' -> Party Name.
 *            Columns Id / CompanyName / GlAccountId (GlAccountId hidden).
 *   Show     GridFillCommissionAgent -> GeneralReprots.GeneralLedgerCommissionAndBrokery (BLL 0140)
 *            -> Sp_CommissionAndBrokery_Report @OrganizationId, @CompanyId, @FromDate, @ToDate,
 *            @CommissionAgentId / @SupplierCustomerId / @ActionId only when non-zero
 *            (ALL=0, Purchase=1, Sale=2).
 *   Balance  CommonServices.GetCurrentGlBalance(agent GlAccountId, ToDate) ->
 *            VoucherHead.GetCurrentGLAccountBalance -> Sp_Vouchers_GetMethods @OrganizationId,
 *            @CompanyId, @RefAccountId, @VoucherDate, @Activity='GetCurrentGLAccountBalance'
 *            -> CurrentBalance.
 */
@Service
public class CommissionAgentReportService {

    @Autowired
    private AccountReportsHSupport h;

    public Map<String, Object> init() {
        Map<String, Object> out = combos();
        out.put("financialYearStart", h.financialYearStart());
        out.put("amountDecimals", h.amountDecimals());
        out.put("rateDecimals", h.rateDecimals());
        return out;
    }

    /** CommissionAgentFill (also btnRefresh_Click). */
    public Map<String, Object> combos() {
        List<Map<String, Object>> agents = new ArrayList<>(), parties = new ArrayList<>();
        List<Map<String, Object>> rows = h.rows("[Sp_SupplierCustomer_GetAllMethod]", params(
                "OrganizationId", h.org(),
                "CompanyId", h.comp(),
                "ActionId", 1,
                "Activity", "GetSupplierCustomerAndCommissionAgentFromPurchaseAndSales"));
        for (Map<String, Object> r : rows) {
            String type = String.valueOf(col(r, "PartyType"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", col(r, "Id"));
            m.put("CompanyName", col(r, "CompanyName"));
            m.put("GlAccountId", col(r, "GlAccountId"));
            if ("CommissionAgent".equals(type)) agents.add(m);
            if ("Supplier".equals(type) || "Customer".equals(type)) parties.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("bound", !rows.isEmpty());
        out.put("agents", agents);
        out.put("parties", parties);
        return out;
    }

    /** GridFillCommissionAgent. */
    public Map<String, Object> show(Map<String, Object> body) {
        int agentId = toInt(body.get("commissionAgentId"));
        int partyId = toInt(body.get("supplierCustomerId"));
        int actionId = toInt(body.get("actionId"));
        java.sql.Date to = AccountReportsHSupport.date(body.get("toDate"), "To Date");
        List<Map<String, Object>> rows = h.rows("Sp_CommissionAndBrokery_Report", params(
                "OrganizationId", h.org(),
                "CompanyId", h.comp(),
                "FromDate", AccountReportsHSupport.date(body.get("fromDate"), "From Date"),
                "ToDate", to,
                "CommissionAgentId", agentId != 0 ? agentId : null,
                "SupplierCustomerId", partyId != 0 ? partyId : null,
                "ActionId", actionId != 0 ? actionId : null));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        if (!rows.isEmpty() && agentId > 0) {
            int glAccountId = toInt(body.get("glAccountId"));
            List<Map<String, Object>> b = h.rows("Sp_Vouchers_GetMethods", params(
                    "OrganizationId", h.org(),
                    "CompanyId", h.comp(),
                    "RefAccountId", glAccountId,
                    "VoucherDate", to,
                    "Activity", "GetCurrentGLAccountBalance"));
            out.put("balance", b.isEmpty() ? 0d : AccountReportsHSupport.toDouble(col(b.get(0), "CurrentBalance")));
        }
        return out;
    }
}
