package com.mst.services;

import com.mst.repositories.ProductionEvaluationWagesRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.ProductionEvaluationWagesRepository.col;
import static com.mst.repositories.ProductionEvaluationWagesRepository.toInt;

/**
 * frmEvaulationDetailWagesReports.cs ("Wages Register"). The page keeps the form state and does
 * the desktop's per-activity projection of the result; this service runs every database step with
 * the BLL's guards. The branch list is re-derived here from the combo's TEXT exactly as the desktop
 * derives it (split on ',' and matched by BranchName against the user's allocated branches), so a
 * request can never widen the branch filter beyond what USP_GetBranchsAllocatedToUserFromWages
 * gives this user. Tenancy and user come from {@link CurrentUserContext}.
 *
 * The form has no rights object (it never calls SetRightsValueInRightsObject).
 */
@Service
public class ProductionEvaluationWagesService {

    /** frmEvaulationDetailSalesReports_Load:158 - IsPartyProcessingFeatureOn. */
    public static final int FEATURE_PARTY_PROCESSING = 8;
    /** Load:162 - BranchFeature (chkBranchWise.Visible). */
    public static final int FEATURE_BRANCH = 11;

    @Autowired private ProductionEvaluationWagesRepository repo;
    @Autowired private CurrentUserContext ctx;

    /* ==================================================================================== Load */

    /**
     * frmEvaulationDetailSalesReports_Load:151 - the two features, BranchesFill (with the user's
     * branch name as the combo's initial Text), ActiveYr.Start_Period and the grid number formats.
     * ComboFill / JobOrderFill / GridFill are separate calls because on the desktop each can throw
     * and stop the rest of Load at that point.
     */
    public Map<String, Object> load() {
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("screenName", ProductionEvaluationWagesRepository.DESKTOP_SCREEN_NAME);
        out.put("partyProcessing", repo.erpFeature(org, comp, FEATURE_PARTY_PROCESSING));
        out.put("branchFeature", repo.erpFeature(org, comp, FEATURE_BRANCH));
        out.put("branches", branches());
        String bn = "";
        try { bn = repo.branchName(ctx.currentBranchId()); } catch (Exception ignored) { }
        out.put("userBranchName", bn);
        out.put("financialYearStart", financialYearStart());
        out.put("amountDecimals", amountDecimals());
        out.put("rateDecimals", rateDecimals());
        return out;
    }

    /** BranchesFill:250 - BranchId / BranchName. */
    public List<Map<String, Object>> branches() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.branchesAllocatedFromWages(ctx.currentOrganizationId(),
                ctx.currentCompanyId(), ctx.currentUserId())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("BranchId", col(r, "BranchId"));
            m.put("BranchName", col(r, "BranchName"));
            out.add(m);
        }
        return out;
    }

    /* ============================================================================ ComboFill */

    /**
     * ComboFill:280. {bound:false} when the procedure returned nothing (the desktop returns before
     * binding, so every combo keeps what it had). StockParty rows are added TWICE - the desktop has
     * the same `if (Activity == "StockParty")` block two times in a row - so each party appears twice.
     */
    public Map<String, Object> combos(String branchText) {
        String branchIds = branchIds(branchText);
        List<Map<String, Object>> rows = repo.comboAgainstContractorWages(ctx.currentOrganizationId(),
                ctx.currentCompanyId(), branchIds);
        Map<String, Object> out = new LinkedHashMap<>();
        if (rows.isEmpty()) { out.put("bound", false); return out; }
        List<Map<String, Object>> refDoc = new ArrayList<>(), wages = new ArrayList<>(), contractor = new ArrayList<>(),
                stockParty = new ArrayList<>(), account = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            String a = str(col(r, "Activity"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", col(r, "Id"));
            m.put("name", col(r, "ReferenceName"));
            if ("RefDocumentType".equals(a)) refDoc.add(m);
            if ("WagesAccount".equals(a)) wages.add(m);
            if ("Contractor".equals(a)) contractor.add(m);
            if ("StockParty".equals(a)) stockParty.add(m);
            if ("StockParty".equals(a)) stockParty.add(m);
            if ("WagesDebitAccount".equals(a)) account.add(m);
        }
        out.put("bound", true);
        out.put("documentTypes", refDoc);
        out.put("wagesAccounts", wages);
        out.put("contractors", contractor);
        out.put("stockParties", stockParty);
        out.put("debitAccounts", account);
        return out;
    }

    /** JobOrderFill:363 - bound only when rows came back ({bound:false} keeps the old list). */
    public Map<String, Object> jobOrders(String branchText) {
        String branchIds = branchIds(branchText);
        List<Map<String, Object>> rows = repo.jobOrderNoFromContractorWages(ctx.currentOrganizationId(),
                ctx.currentCompanyId(), branchIds);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("bound", !rows.isEmpty());
        List<Map<String, Object>> list = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", col(r, "Id"));
            m.put("JobOrderNo", col(r, "JobOrderNo"));
            m.put("RefDocumentTypeId", col(r, "RefDocumentTypeId"));
            list.add(m);
        }
        out.put("rows", list);
        return out;
    }

    /* ============================================================================== GridFill */

    /**
     * GridFill:482 - the ReportsParameters the form builds, then WagesRegister's guards in the
     * BLL's order. Returns the rows (dtGrid) and the exact argument set, which the page keeps for
     * the Print button (the desktop prints the dtGrid of the last Show).
     */
    public Map<String, Object> grid(Map<String, Object> b) {
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();

        String from = str(b.get("fromDate"));
        String to = str(b.get("toDate"));
        int documentTypeId = toInt(b.get("documentTypeId"));
        String reportType = b.get("activity") == null ? "" : String.valueOf(b.get("activity"));
        int wagesType = toInt(b.get("wagesType"));
        Boolean freeOfCost = null;                  /* ApprovedFilter == "All" -> @FreeOfCost omitted */
        if (wagesType == 1) freeOfCost = Boolean.FALSE;
        else if (wagesType == 2) freeOfCost = Boolean.TRUE;
        int actionId = 0;
        if (repo.erpFeature(org, comp, FEATURE_PARTY_PROCESSING)) {
            String party = str(b.get("party"));
            if ("company".equals(party)) actionId = 1;
            else if ("thirdParty".equals(party)) actionId = 2;
        }
        int skipZero = Boolean.TRUE.equals(b.get("branchWise")) ? 1 : 0;
        int contractorId = toInt(b.get("contractorId"));
        int accountId = toInt(b.get("wagesAccountId"));
        int stockPartyId = toInt(b.get("stockPartyId"));
        int jobOrderId = toInt(b.get("jobOrderId"));
        int debitAccountId = toInt(b.get("debitAccountId"));
        String refDocumentTypeIds = null;
        if (jobOrderId != 0) {
            switch (toInt(b.get("jobOrderRefDocumentTypeId"))) {
                case 1: refDocumentTypeIds = "80,112"; break;
                case 2: refDocumentTypeIds = "117,118"; break;
                default: break;
            }
        }
        String branchIds = branchIds(str(b.get("branchText")));

        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        m.put("@OrganizationId", org);
        m.put("@CompanyId", comp);
        Timestamp f = ts(from), t = ts(to);
        if (f != null) m.put("@FromDate", f);
        if (t != null) m.put("@ToDate", t);
        if (accountId != 0) m.put("@WagesAccountId", accountId);
        if (contractorId != 0) m.put("@ContractorId", contractorId);
        if (documentTypeId != 0) m.put("@ReferenceDocumentTypeId", documentTypeId);
        if (debitAccountId != 0) m.put("@DebitAccountId", debitAccountId);
        if (!branchIds.isEmpty()) m.put("@BranchesIds", branchIds);
        if (actionId != 0) m.put("@ActionId", actionId);
        if (stockPartyId != 0) m.put("@StockPartyId", stockPartyId);
        if (jobOrderId != 0) {
            m.put("@JobOrderId", jobOrderId);
            m.put("@ReferenceDocumentTypeIds", refDocumentTypeIds);   /* null value: not supplied */
        }
        if (freeOfCost != null) m.put("@FreeOfCost", freeOfCost);
        if (!reportType.isEmpty()) m.put("@ActivityName", reportType);
        if (skipZero != 0) m.put("@BranchWise", skipZero);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", repo.wagesRegister(m));

        Map<String, Object> print = new LinkedHashMap<>();
        print.put("fromDate", f == null ? null : f.toLocalDateTime().toString());
        print.put("toDate", t == null ? null : t.toLocalDateTime().toString());
        print.put("wagesAccountId", accountId);
        print.put("contractorId", contractorId);
        print.put("documentTypeId", documentTypeId);
        print.put("debitAccountId", debitAccountId);
        print.put("branchesIds", branchIds);
        print.put("actionId", actionId);
        print.put("stockPartyId", stockPartyId);
        print.put("jobOrderId", jobOrderId);
        print.put("referenceDocumentTypeIds", refDocumentTypeIds);
        print.put("freeOfCost", freeOfCost);
        print.put("activityName", reportType);
        print.put("branchWise", skipZero);
        out.put("printArgs", print);
        return out;
    }

    /* =============================================================================== helpers */

    /**
     * The block ComboFill / JobOrderFill / GridFill share:
     *   if (CmbBranchName.Text != "") { foreach (author in (Text + ",").Split(',')) {
     *       dr = dtBranch.Select("BranchName='" + author + "'"); if (dr.Length != 0) BranchIds += "," + dr[0][0]; } }
     *   else { CmbBranchName.Focus(); throw new Exception("Select Branch First"); }
     * DataTable.Select compares strings case-insensitively (CaseSensitive defaults to false).
     */
    public String branchIds(String branchText) {
        String text = branchText == null ? "" : branchText;
        if (text.isEmpty()) throw new IllegalArgumentException("Select Branch First");
        List<Map<String, Object>> allocated = branches();
        StringBuilder ids = new StringBuilder();
        for (String author : (text + ",").split(",", -1)) {
            for (Map<String, Object> r : allocated) {
                if (str0(col(r, "BranchName")).equalsIgnoreCase(author)) {
                    ids.append(',').append(str0(col(r, "BranchId")));
                    break;
                }
            }
        }
        return ids.toString();
    }

    private String financialYearStart() {
        int yearId = ctx.currentFinancialYearId();
        try {
            List<Map<String, Object>> years = repo.activeFinancialYears(ctx.currentOrganizationId(), ctx.currentCompanyId());
            Map<String, Object> row = null;
            for (Map<String, Object> r : years) if (toInt(col(r, "Id")) == yearId) { row = r; break; }
            if (row == null && !years.isEmpty()) row = years.get(0);
            if (row != null) {
                Object v = col(row, "Start_Period");
                return v == null ? null : v.toString();
            }
        } catch (Exception ignored) { }
        return null;
    }

    /** stringFormatsingle: "Default NoofDecimal Points For Amount" 1-4, anything else "#,##0." (none). */
    private int amountDecimals() {
        int n = intConfig("Default NoofDecimal Points For Amount");
        return (n >= 1 && n <= 4) ? n : 0;
    }

    /** DecimalRateFormate: "Default NoofDecimal Points For Rate" 1-4, 0 gives 2, anything else none. */
    private int rateDecimals() {
        int n = intConfig("Default NoofDecimal Points For Rate");
        if (n == 0) return 2;
        return (n >= 1 && n <= 4) ? n : 0;
    }

    private int intConfig(String name) {
        try {
            String v = repo.configValue(ctx.currentOrganizationId(), ctx.currentCompanyId(), name);
            if (v.isEmpty()) return 0;
            return (int) Math.floor(Double.parseDouble(v));
        } catch (Exception e) {
            return 0;
        }
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
    private static String str0(Object o) { return o == null ? "" : String.valueOf(o); }

    private static Timestamp ts(String s) {
        String t = s == null ? "" : s.trim();
        if (t.isEmpty()) return null;
        try {
            if (t.length() == 10) return Timestamp.valueOf(LocalDate.parse(t).atStartOfDay());
            return Timestamp.valueOf(LocalDateTime.parse(t.length() > 19 ? t.substring(0, 19) : t));
        } catch (Exception e) {
            return null;
        }
    }
}
