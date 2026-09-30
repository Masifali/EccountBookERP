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
 * "Wages Report" / "Contractor Wages History" -
 * Architecture.WinApp.Contractor_Wages/frmStockContractorWagesHistory.cs (1,999 lines).
 *
 * Load / BranchesFill / the branch-text -> BranchesIds rule are the same as frmEvaulationDetailWagesReports
 * (same USP_GetBranchsAllocatedToUserFromWages, same UserAccount.BranchName default, same split on ','),
 * so they are delegated to {@link ProductionEvaluationWagesService}. Everything specific to this form is here:
 *
 *   ComboFill:224        Usp_AllComboAgainstContractorWages - RefDocumentType / WagesAccount / Contractor /
 *                        StockParty (each once; unlike the Wages Register form, StockParty is NOT doubled
 *                        and there is no WagesDebitAccount combo).
 *   GridBind:359 type 1  Sp_InvContractorWagesBillHeader_SlipandRegister (BLL :692-810)
 *   GridBind:359 type 2  USP_GetSummaryWagesByRefDocumentsAndActivities (BLL :1263-1360)
 *
 * Tenancy and user are server-derived; nothing the page sends can widen them.
 */
@Service
public class ContractorWagesHistoryService {

    /** GridBind:368 - obj.DocumentTypeId = 101. */
    private static final int DOCUMENT_TYPE_ID = 101;

    @Autowired private ProductionEvaluationWagesService shared;
    @Autowired private ProductionEvaluationWagesRepository repo;
    @Autowired private CurrentUserContext ctx;

    /** frmPendingGrnRpt_Load:143 - features 8 / 11, BranchesFill, Start_Period (for New), formats. */
    public Map<String, Object> load() {
        Map<String, Object> out = shared.load();
        out.put("screenName", "frmStockContractorWagesHistory");
        return out;
    }

    public List<Map<String, Object>> branches() { return shared.branches(); }

    /** ComboFill:224. {bound:false} when the procedure returned no rows (the desktop returns before binding). */
    public Map<String, Object> combos(String branchText) {
        String branchIds = shared.branchIds(branchText);
        List<Map<String, Object>> rows = repo.comboAgainstContractorWages(ctx.currentOrganizationId(),
                ctx.currentCompanyId(), branchIds);
        Map<String, Object> out = new LinkedHashMap<>();
        if (rows.isEmpty()) { out.put("bound", false); return out; }
        List<Map<String, Object>> refDoc = new ArrayList<>(), wages = new ArrayList<>(),
                contractor = new ArrayList<>(), stockParty = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            String a = str(col(r, "Activity"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", col(r, "Id"));
            m.put("name", col(r, "ReferenceName"));
            if ("RefDocumentType".equals(a)) refDoc.add(m);
            if ("WagesAccount".equals(a)) wages.add(m);
            if ("Contractor".equals(a)) contractor.add(m);
            if ("StockParty".equals(a)) stockParty.add(m);
        }
        out.put("bound", true);
        out.put("documentTypes", refDoc);
        out.put("wagesAccounts", wages);
        out.put("contractors", contractor);
        out.put("stockParties", stockParty);
        return out;
    }

    /**
     * GridBind:359. Builds the ReportsParameters exactly as the form does, then applies the guards of
     * the BLL method chosen by Report Type (1 Detail, 2 Summary; anything else returns nothing, as the
     * desktop's `if (ReportType != 2) return;`).
     */
    public Map<String, Object> grid(Map<String, Object> b) {
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();

        int reportType = toInt(b.get("reportType"));
        int refDocumentTypeId = toInt(b.get("documentTypeId"));
        Timestamp from = ts(str(b.get("fromDate")));
        Timestamp to = ts(str(b.get("toDate")));
        int supplierCustomerId = toInt(b.get("contractorId"));
        int accountId = toInt(b.get("wagesAccountId"));
        int stockPartyId = toInt(b.get("stockPartyId"));
        int skipZero = Boolean.TRUE.equals(b.get("branchWise")) ? 1 : 0;

        /* :380-391 - 1 Regular -> IsApproved=false, 2 Free of Cost -> true, else ApprovedFilter="All". */
        int wagesType = toInt(b.get("wagesType"));
        Boolean freeOfCost = wagesType == 1 ? Boolean.FALSE : wagesType == 2 ? Boolean.TRUE : null;

        /* :392-402 - radios only when feature 8 is on. */
        int actionId = 0;
        if (repo.erpFeature(org, comp, ProductionEvaluationWagesService.FEATURE_PARTY_PROCESSING)) {
            String party = str(b.get("party"));
            if ("company".equals(party)) actionId = 1;
            else if ("thirdParty".equals(party)) actionId = 2;
        }

        String branchIds = shared.branchIds(str(b.get("branchText")));   /* "Select Branch First" */

        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        List<Map<String, Object>> rows;
        String printKey;
        if (reportType == 1) {
            /* ContractorWagesBill_SlipandRegister (BLL :692) - BranchesId / RefDocNoId / Id are 0 here and omitted. */
            m.put("@OrganizationId", org);
            m.put("@CompanyId", comp);
            m.put("@DocumentTypeId", DOCUMENT_TYPE_ID);
            if (!branchIds.isEmpty()) m.put("@BranchesIds", branchIds);
            if (supplierCustomerId != 0) m.put("@SupplierCustomerId", supplierCustomerId);
            if (refDocumentTypeId != 0) m.put("@RefDocumentTypeId", refDocumentTypeId);
            if (accountId != 0) m.put("@InvConractorWagesAccountsId", accountId);
            if (from != null) m.put("@BillDateFrom", from);
            if (to != null) m.put("@BillDateTo", to);
            if (freeOfCost != null) m.put("@FreeOfCost", freeOfCost);
            if (actionId != 0) m.put("@ActionId", actionId);
            if (stockPartyId != 0) m.put("@StockPartyId", stockPartyId);
            rows = repo.contractorWagesSlipAndRegister(m);
            printKey = "wh-001";
        } else if (reportType == 2) {
            /* GetSummaryWagesByRefDocumentsAndActivities (BLL :1263) */
            m.put("@OrganizationId", org);
            m.put("@CompanyId", comp);
            if (from != null) m.put("@FromDate", from);
            if (to != null) m.put("@ToDate", to);
            if (refDocumentTypeId != 0) m.put("@RefDocumentTypeId", refDocumentTypeId);
            if (supplierCustomerId != 0) m.put("@SupplierCustomerId", supplierCustomerId);
            if (accountId != 0) m.put("@InvConractorWagesAccountsId", accountId);
            if (!branchIds.isEmpty()) m.put("@BranchesIds", branchIds);
            if (freeOfCost != null) m.put("@FreeOfCost", freeOfCost);
            if (actionId != 0) m.put("@ActionId", actionId);
            if (stockPartyId != 0) m.put("@StockPartyId", stockPartyId);
            if (skipZero != 0) m.put("@BranchWise", skipZero);
            rows = repo.summaryWagesByRefDocumentsAndActivities(m);
            printKey = "wh-003";
        } else {
            rows = new ArrayList<>();
            printKey = null;
        }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        out.put("printKey", printKey);
        /* The print re-runs the same procedure with the same arguments (the desktop prints dtGrid). */
        Map<String, Object> print = new LinkedHashMap<>();
        print.put("fromDate", from == null ? null : from.toLocalDateTime().toString());
        print.put("toDate", to == null ? null : to.toLocalDateTime().toString());
        print.put("branchesIds", branchIds.isEmpty() ? null : branchIds);
        print.put("contractorId", supplierCustomerId == 0 ? null : supplierCustomerId);
        print.put("documentTypeId", refDocumentTypeId == 0 ? null : refDocumentTypeId);
        print.put("wagesAccountId", accountId == 0 ? null : accountId);
        print.put("freeOfCost", freeOfCost);
        print.put("actionId", actionId == 0 ? null : actionId);
        print.put("stockPartyId", stockPartyId == 0 ? null : stockPartyId);
        print.put("branchWise", skipZero == 0 ? null : skipZero);
        out.put("printArgs", print);
        return out;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private static Timestamp ts(String t) {
        if (t == null || t.isEmpty()) return null;
        try {
            if (t.length() == 10) return Timestamp.valueOf(LocalDate.parse(t).atStartOfDay());
            return Timestamp.valueOf(LocalDateTime.parse(t.length() > 19 ? t.substring(0, 19) : t));
        } catch (Exception e) {
            return null;
        }
    }
}
