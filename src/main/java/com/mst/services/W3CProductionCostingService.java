package com.mst.services;

import com.mst.repositories.W3CProductionCostingRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.W3CSupport.col;
import static com.mst.services.W3CSupport.mapOf;
import static com.mst.services.W3CSupport.plainRow;
import static com.mst.services.W3CSupport.plainRows;
import static com.mst.services.W3CSupport.rowList;
import static com.mst.services.W3CSupport.str;
import static com.mst.services.W3CSupport.toDecimal;
import static com.mst.services.W3CSupport.toDouble;
import static com.mst.services.W3CSupport.toInt;

/**
 * 887 Production Costing - frmCreateProductionCostingReport.cs. The page keeps the three grids and does the
 * grid arithmetic (CalculateHeaderValues, UpdateInvoiceGridValues, CalculateTotalCostDetail run in the
 * browser exactly as in the desktop); every database step, and every refusal Insert() makes before its write,
 * runs here again in the desktop's order and wording. Tenancy, branch, financial year and user always come
 * from {@link CurrentUserContext}.
 *
 * Fences the desktop does not need (its form can only ever hold ids it read itself):
 *  - the Job Order must be one USP_InvFoodProduction_JobOrderWithFinishOutPutPrcntObtained returns for this
 *    organization / company; every invoice id must be one UPS_ExImInvoice_GetInvoicesData returns;
 *  - an update re-reads the header and accepts only a record of this organization / company, and a detail
 *    Id only if that record's own detail lists contain it (the Insert procedures update WHERE Id = @Id).
 */
@Service
public class W3CProductionCostingService {

    @Autowired private W3CProductionCostingRepository repo;
    @Autowired private W3CSupport support;
    @Autowired private CurrentUserContext ctx;

    /* ======================================================================== InitializeComponentMethod */

    /** InitializeComponentMethod: rights, job orders, export invoices, number formats. */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("screenName", W3CProductionCostingRepository.SCREEN);
        out.put("rights", support.rights(W3CProductionCostingRepository.SCREEN));
        out.put("jobOrders", jobOrders());
        out.put("invoices", invoices());
        out.put("amountDecimals", support.amountDecimals());
        out.put("rateDecimals", support.rateDecimals());
        out.put("defaultHistoryDays", support.defaultHistoryDays());
        out.put("today", LocalDate.now().toString());
        out.put("financialYearStart", support.activeYearStart());
        return out;
    }

    public List<Map<String, Object>> jobOrders() {
        return plainRows(repo.jobOrders(ctx.currentOrganizationId(), ctx.currentCompanyId()));
    }

    public List<Map<String, Object>> invoices() {
        return plainRows(repo.invoices(ctx.currentOrganizationId(), ctx.currentCompanyId(),
                ctx.currentBranchId(), ctx.currentFinancialYearId()));
    }

    /** btnRefresh_Click:1283 - both pickers are rebound. */
    public Map<String, Object> refresh() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("jobOrders", jobOrders());
        out.put("invoices", invoices());
        out.put("defaultHistoryDays", support.defaultHistoryDays());
        return out;
    }

    /* ============================================================================== btnShow_Click:554 */

    /**
     * btnShow_Click: ReadById(JobOrderId) = ProductionManualCosting.ReadByJobOrderId; when a record exists its
     * header and the three detail lists fill the form, otherwise the Input grid is bound from
     * FoodProduction_InputSummaryByJobOrder. By-Product rows are always rebuilt from
     * FoodProduction_ByProductSummaryByJobOrder (the page computes them, as ByProductBindByJobOrder does).
     */
    public Map<String, Object> show(int jobOrderId) {
        if (jobOrderId <= 0) throw new IllegalArgumentException("Please Select JobOrder First");
        int org = ctx.currentOrganizationId(), comp = ctx.currentCompanyId();
        Map<String, Object> job = jobOrderRow(jobOrderId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("jobOrderId", jobOrderId);
        out.put("finishOutPutPrcntObtained", toDouble(col(job, "FinishOutPutPrcntObtained")));
        Map<String, Object> header = existingHeader(jobOrderId);
        if (header != null) {
            int id = toInt(col(header, "Id"));
            out.put("exists", true);
            out.put("recId", id);
            out.put("header", plainRow(header));
            out.put("inputRows", plainRows(repo.inputDetails(id)));
            out.put("invoiceRows", plainRows(repo.invoiceDetails(id)));
        } else {
            out.put("exists", false);
            out.put("recId", 0);
            out.put("inputRows", plainRows(repo.inputSummary(org, comp, ctx.currentBranchId(), ctx.currentFinancialYearId(), jobOrderId)));
            out.put("invoiceRows", new ArrayList<>());
        }
        out.put("byProductSummary", plainRows(repo.byProductSummary(org, comp, ctx.currentBranchId(), ctx.currentFinancialYearId(), jobOrderId)));
        return out;
    }

    /** ReadByJobOrderId carries no tenancy: a record of another organization / company is not this user's. */
    private Map<String, Object> existingHeader(int jobOrderId) {
        int org = ctx.currentOrganizationId(), comp = ctx.currentCompanyId();
        for (Map<String, Object> h : repo.headerByJobOrder(jobOrderId)) {
            if (toInt(col(h, "OrganizationId")) == org && toInt(col(h, "CompanyId")) == comp) return h;
        }
        return null;
    }

    private Map<String, Object> jobOrderRow(int jobOrderId) {
        for (Map<String, Object> r : repo.jobOrders(ctx.currentOrganizationId(), ctx.currentCompanyId())) {
            if (toInt(col(r, "JobOrderId")) == jobOrderId) return r;
        }
        throw new IllegalArgumentException("Please Select JobOrder First");
    }

    /**
     * CmbInvoiceNoDetail_Leave:~488 - the charges last costed for this job order and invoice
     * (USP_ProductionManualCosting_GetDataByJobAndInvoiceId, Top 1); null when none.
     */
    public Map<String, Object> invoiceDefaults(int jobOrderId, int invoiceId) {
        List<Map<String, Object>> rows = repo.dataByJobAndInvoice(ctx.currentOrganizationId(), ctx.currentCompanyId(),
                ctx.currentBranchId(), ctx.currentFinancialYearId(), jobOrderId, invoiceId);
        return rows.isEmpty() ? null : plainRow(rows.get(0));
    }

    /* ======================================================================================= history */

    /** frmProductionCostingReport.gridHisory - usp_ProductionCosting_FormHistory(@CompanyId, @FromDate, @ToDate). */
    public List<Map<String, Object>> history(String fromDate, String toDate) {
        return plainRows(repo.history(ctx.currentCompanyId(), sqlDate(fromDate), sqlDate(toDate)));
    }

    private static Date sqlDate(String s) {
        String t = str(s);
        if (t.isEmpty()) return null;
        try { return Date.valueOf(LocalDate.parse(t.length() > 10 ? t.substring(0, 10) : t)); }
        catch (RuntimeException e) { throw new IllegalArgumentException("Invalid date: " + t); }
    }

    /* ============================================================================ Print (OpenPrint:1538) */

    /** OpenPrint: SlipAndRegister for the job order; no rows -> "Record Not Found For DisPlay". */
    public Map<String, Object> printRows(int jobOrderId) {
        support.require(support.rights(W3CProductionCostingRepository.SCREEN), "print", "Print");
        if (jobOrderId <= 0) throw new IllegalArgumentException("Please Select JobOrder First");
        jobOrderRow(jobOrderId);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rows", repo.slipRows(ctx.currentOrganizationId(), ctx.currentCompanyId(), jobOrderId).size());
        return m;
    }

    /* ================================================================ BtnByProductSettlementPopUp:1830 */

    /** getProductionByProductDataByJobOrderId - the rows of the "Please double-click any row to update values." pop-up. */
    public List<Map<String, Object>> byProductItemWise(int jobOrderId) {
        if (jobOrderId <= 0) throw new IllegalArgumentException("Please Select JobOrder First");
        jobOrderRow(jobOrderId);
        return plainRows(repo.byProductItemWise(ctx.currentCompanyId(), jobOrderId));
    }

    /**
     * ProductionByProductRateUpdateByJobOrderId: one call per returned row (Rate/Kg edited); the page then runs
     * btnShow_Click again. Only items the pop-up itself lists for this job order are accepted.
     */
    public void byProductRates(int jobOrderId, List<Map<String, Object>> rows) {
        if (jobOrderId <= 0) throw new IllegalArgumentException("Please Select JobOrder First");
        jobOrderRow(jobOrderId);
        int comp = ctx.currentCompanyId();
        Set<Integer> items = new HashSet<>();
        for (Map<String, Object> r : repo.byProductItemWise(comp, jobOrderId)) items.add(toInt(col(r, "ItemId")));
        for (Map<String, Object> r : rows) {
            int itemId = toInt(col(r, "itemId"));
            if (!items.contains(itemId)) throw new IllegalArgumentException("Item is not a by-product of this job order");
        }
        for (Map<String, Object> r : rows) {
            repo.byProductRateUpdate(comp, jobOrderId, toInt(col(r, "itemId")), toDouble(col(r, "rate")));
        }
    }

    /* ============================================================================== Insert():957 */

    /**
     * Insert(): the checks in the desktop's order (the Yes/No prompt is answered on the page first), then
     * ProductionManualCosting.Save -> DAL SetData in one transaction: header, input list, by-product list,
     * invoice list.
     */
    @Transactional(rollbackFor = Exception.class)
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = toInt(body.get("recId"));
        Map<String, Boolean> rights = support.rights(W3CProductionCostingRepository.SCREEN);
        if (recId > 0) support.require(rights, "update", "Update"); else support.require(rights, "save", "Save");

        List<Map<String, Object>> inputRows = rowList(body.get("inputRows"));
        if (inputRows.isEmpty()) throw new IllegalArgumentException("Grid Input Record Not Found");

        int jobOrderId = toInt(body.get("jobOrderId"));
        Map<String, Object> h = mapOf(body.get("header"));
        if (jobOrderId == 0) throw new IllegalArgumentException("JobOrder No field is required");
        nonZero(h, "inputAvgRate", "InputAvg Rate");
        nonZero(h, "finishedOutputPercent", "Finish OutPut % Obtained");
        nonZero(h, "inputToBeProcessed", "Input To Be Processed Obtained");
        nonZero(h, "totalCost", "Total Cost");
        nonZero(h, "rateInPMT", "Rate In MPR");

        int org = ctx.currentOrganizationId(), comp = ctx.currentCompanyId(), branch = ctx.currentBranchId();
        int fy = ctx.currentFinancialYearId(), user = ctx.currentUserId();
        jobOrderRow(jobOrderId);

        /* ------------------------------------------------------------------ fences for an update */
        Set<Integer> inputIds = new HashSet<>(), bpIds = new HashSet<>(), invIds = new HashSet<>();
        String attachmentsValues = "", customAttachmentsValues = "";
        if (recId > 0) {
            Map<String, Object> stored = null;
            for (Map<String, Object> r : repo.headerById(recId)) stored = r;
            if (stored == null || toInt(col(stored, "OrganizationId")) != org || toInt(col(stored, "CompanyId")) != comp)
                throw new IllegalArgumentException("RecId not Found");
            if (toInt(col(stored, "ProductionJobOrderId")) != jobOrderId)
                throw new IllegalArgumentException("The record belongs to a different Job Order");
            attachmentsValues = str(col(stored, "AttachmentsValues"));
            customAttachmentsValues = str(col(stored, "CustomAttachmentsValues"));
            for (Map<String, Object> r : repo.inputDetails(recId)) inputIds.add(toInt(col(r, "Id")));
            for (Map<String, Object> r : repo.byProductDetails(recId)) bpIds.add(toInt(col(r, "Id")));
            for (Map<String, Object> r : repo.invoiceDetails(recId)) invIds.add(toInt(col(r, "Id")));
        }

        /* ------------------------------------------------------------------- detail lists (:1010..:1090) */
        List<Map<String, Object>> inputList = new ArrayList<>();
        int i = 0;
        for (Map<String, Object> r : inputRows) {
            int id = recId != 0 ? toInt(col(r, "id")) : 0;
            fenceId(id, inputIds);
            Map<String, Object> p = new LinkedHashMap<>();
            p.put("Id", id);
            p.put("InvFoodProductionId", 0);
            p.put("ByProductDetailId", 0);
            p.put("RefDocumentTypeId", toInt(col(r, "refDocumentTypeId")));
            p.put("RefDocId", toInt(col(r, "refDocId")));
            p.put("ItemId", toInt(col(r, "itemId")));
            p.put("Qty", toDecimal(col(r, "qty")));
            p.put("Weight", toDecimal(col(r, "weight")));
            p.put("InputAmount", toDecimal(col(r, "inputAmount")));
            p.put("InputRate", toDecimal(col(r, "inputRate")));
            p.put("Rate", toDecimal(col(r, "rate")));
            p.put("FinalAmount", toDecimal(col(r, "finalAmount")));
            p.put("RemarksDetail", "");
            p.put("ActionTypeId", id <= 0 ? 1 : 2);
            validateField(p.get("ItemId"), "Item", i);
            validateField(p.get("Weight"), "Weight", i);
            validateField(p.get("Rate"), "Rate", i);
            validateField(p.get("FinalAmount"), "Final Amount", i);
            inputList.add(p);
            i++;
        }

        List<Map<String, Object>> bpList = new ArrayList<>();
        i = 0;
        for (Map<String, Object> r : rowList(body.get("byProductRows"))) {
            int id = recId != 0 ? toInt(col(r, "id")) : 0;
            fenceId(id, bpIds);
            Map<String, Object> p = new LinkedHashMap<>();
            p.put("Id", id);
            p.put("InvFoodProductionId", 0);
            p.put("InputDetailId", 0);
            p.put("ItemId", toInt(col(r, "itemId")));
            p.put("PercentOfTotalInput", toDecimal(col(r, "percentOfTotalInput")));
            p.put("Ratio", toDecimal(col(r, "ratio")));
            p.put("Rate", toDecimal(col(r, "rate")));
            p.put("ByProductAmount", toDecimal(col(r, "byProductAmount")));
            p.put("Amount", toDecimal(col(r, "amount")));
            p.put("RemarksDetail", "");
            p.put("ActionTypeId", id <= 0 ? 1 : 2);
            validateField(p.get("ItemId"), "Item", i);
            validateField(p.get("PercentOfTotalInput"), "Percent Of Total Input", i);
            validateField(p.get("Ratio"), "Ratio", i);
            validateField(p.get("Rate"), "Rate", i);
            validateField(p.get("Amount"), "Amount", i);
            bpList.add(p);
            i++;
        }

        Set<Integer> invoiceIds = new HashSet<>();
        for (Map<String, Object> r : invoices()) invoiceIds.add(toInt(col(r, "Id")));
        List<Map<String, Object>> invList = new ArrayList<>();
        if (recId > 0) {
            for (Map<String, Object> r : rowList(body.get("removedInvoiceRows"))) {
                int id = toInt(col(r, "id"));
                if (id <= 0 || !invIds.contains(id)) throw new IllegalArgumentException("RecId not Found");
                Map<String, Object> p = invoiceParams(r, id);
                p.put("ActionTypeId", 3);
                invList.add(p);
            }
        }
        i = 0;
        for (Map<String, Object> r : rowList(body.get("invoiceRows"))) {
            int id = recId != 0 ? toInt(col(r, "id")) : 0;
            fenceId(id, invIds);
            Map<String, Object> p = invoiceParams(r, id);
            p.put("ActionTypeId", id <= 0 ? 1 : 2);
            validateField(p.get("ExImInvoiceId"), "Invoice", i);
            validateField(p.get("SupplierCustomerId"), "Customer", i);
            validateField(p.get("CostAfterProcessing"), "Cost After Processing", i);
            validateField(p.get("TotalCost"), "Total Cost", i);
            validateField(p.get("ExchangeRate"), "Exchange Rate", i);
            validateField(p.get("FcyCodeId"), "FCY Code", i);
            validateField(p.get("FcyAmount"), "FCY Amount", i);
            validateField(p.get("AmountAfterComm"), "Amount After Commission", i);
            validateField(p.get("ItemSaleAmount"), "Item Sale Amount", i);
            validateField(p.get("ContributionPerMTon"), "Contribution Per M.Ton", i);
            if (!invoiceIds.contains(toInt(p.get("ExImInvoiceId")))) throw new IllegalArgumentException("Invoice is not available for this branch and financial year");
            invList.add(p);
            i++;
        }

        /* ------------------------------------------------------------------ ProductionManualCosting.Save */
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> hp = new LinkedHashMap<>();
        hp.put("Id", recId);
        hp.put("DocumentTypeId", W3CProductionCostingRepository.DOC_TYPE);
        hp.put("ProductionJobOrderId", jobOrderId);
        hp.put("CostingDate", new Date(System.currentTimeMillis()));
        hp.put("InputAvgRate", toDecimal(h.get("inputAvgRate")));
        hp.put("FinishedOutputPercent", toDecimal(h.get("finishedOutputPercent")));
        hp.put("InputToBeProcessed", toDecimal(h.get("inputToBeProcessed")));
        hp.put("ByProductCost", toDecimal(h.get("byProductCost")));
        hp.put("TotalCost", toDecimal(h.get("totalCost")));
        hp.put("ProcessingCharges", toDecimal(h.get("processingCharges")));
        hp.put("TotalCostInPMT", BigDecimal.ZERO);
        hp.put("RateInPMT", toDecimal(h.get("rateInPMT")));
        hp.put("OrganizationId", org);
        hp.put("CompanyId", comp);
        hp.put("BranchesId", branch);
        hp.put("FinancialYearId", fy);
        hp.put("EntryUserId", user);
        hp.put("EntryDate", now);
        hp.put("ModifyUserId", user);
        hp.put("ModifyDate", now);
        hp.put("IsApproved", Boolean.FALSE);
        hp.put("ApprovalUserId", 0);
        hp.put("ApprovedDate", now);
        hp.put("ActionId", recId == 0 ? 1 : 2);
        hp.put("AttachmentsValues", attachmentsValues);
        hp.put("CustomAttachmentsValues", customAttachmentsValues);

        int id = repo.saveHeader(hp);
        if (id <= 0) id = recId;                                   /* SetData: num > 0 ? obj.Id = num : num = obj.Id */
        for (Map<String, Object> p : inputList) { p.put("ProductionManualCostingId", id); repo.saveInputDetail(p); }
        for (Map<String, Object> p : bpList) { p.put("ProductionManualCostingId", id); repo.saveByProductDetail(p); }
        for (Map<String, Object> p : invList) { p.put("ProductionManualCostingId", id); repo.saveInvoiceDetail(p); }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("result", true);
        out.put("id", id);
        out.put("updated", recId != 0);
        return out;
    }

    /* ============================================================================ helpers */

    private Map<String, Object> invoiceParams(Map<String, Object> r, int id) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Id", id);
        p.put("ExImInvoiceId", toInt(col(r, "invoiceId")));
        p.put("SupplierCustomerId", toInt(col(r, "supplierCustomerId")));
        p.put("CostAfterProcessing", toDecimal(col(r, "costAfterProcessing")));
        p.put("PackingCharges", toDecimal(col(r, "packingCharges")));
        p.put("FOBCharges", toDecimal(col(r, "fobCharges")));
        p.put("Tax", toDecimal(col(r, "tax")));
        p.put("Freight", toDecimal(col(r, "freight")));
        p.put("FinancialCharges", toDecimal(col(r, "financialCharges")));
        p.put("TotalCost", toDecimal(col(r, "totalCost")));
        p.put("ExchangeRate", toDecimal(col(r, "exchangeRate")));
        p.put("FcyCodeId", toInt(col(r, "fcyCodeId")));
        p.put("FcyAmount", toDecimal(col(r, "fcyAmount")));
        p.put("CommissionAmount", toDecimal(col(r, "commissionAmount")));
        p.put("AmountAfterComm", toDecimal(col(r, "amountAfterCommission")));
        p.put("ItemSaleAmount", toDecimal(col(r, "itemSaleAmount")));
        p.put("ContributionPerMTon", toDecimal(col(r, "contributionPerMTon")));
        p.put("RemarksDetail", str(col(r, "remarks")));
        return p;
    }

    /** A detail Id other than 0 must be one this record's own lists returned. */
    private static void fenceId(int id, Set<Integer> allowed) {
        if (id > 0 && !allowed.contains(id)) throw new IllegalArgumentException("RecId not Found");
    }

    /** FormHelper.ValidateControls TextBox / ValidType.Double: double.TryParse(text) and value != 0. */
    private static void nonZero(Map<String, Object> h, String key, String field) {
        Object v = h.get(key);
        double d = v == null ? Double.NaN : toDoubleStrict(v);
        if (Double.isNaN(d) || d == 0d) throw new IllegalArgumentException(field + " must be a non-zero number");
    }

    private static double toDoubleStrict(Object v) {
        if (v instanceof Number) return ((Number) v).doubleValue();
        String s = str(v).replace(",", "");
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return Double.NaN; }
    }

    /** FormHelper.ValidateField: null / 0 / non-positive / blank -> "{field} is required in Detail Grid at row No: n". */
    private static void validateField(Object value, String field, int rowIndex) {
        boolean bad = value == null
                || (value instanceof Integer && (Integer) value == 0)
                || (value instanceof BigDecimal && ((BigDecimal) value).signum() <= 0)
                || (value instanceof String && ((String) value).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }
}
