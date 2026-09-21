package com.mst.repositories;

import com.mst.models.UserAccount;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * The five Production Reports, one repository — they share a shape: a filter set in, a grid out.
 *
 * Built one report at a time; each method names the desktop form and BLL it came from. Nothing
 * here writes, and no procedure, table or column is created.
 *
 * ---------------------------------------------------------------------------------------------
 * AN UNSET FILTER IS OMITTED, NOT SENT AS NULL
 * ---------------------------------------------------------------------------------------------
 * Every one of these BLL methods guards its optional parameters — {@code if (ApprovedFilter !=
 * "All")}, {@code if (!Conversion.CheckDateTimeNull(FromDate))} — so the procedure receives a
 * SHORTER parameter list rather than a NULL. A procedure that branches on a parameter's presence
 * does not treat the two the same way, which is why every optional value below is added
 * conditionally instead of bound as null.
 *
 * ---------------------------------------------------------------------------------------------
 * @BranchesIds IS ALWAYS SENT, AND CARRIES A LEADING COMMA
 * ---------------------------------------------------------------------------------------------
 * Three of the 309 BLL methods guard it as
 * {@code if (obj.BranchesIds != string.Empty || obj.BranchesIds != null)} — a condition that is
 * TRUE for every possible value, so the parameter is always sent. And the form builds the string
 * as {@code BranchIds = BranchIds + "," + id} starting from {@code ""}, so what the procedure
 * receives is ",5" or ",5,7" — with a leading comma. Both details are reproduced exactly:
 * dropping the parameter, or trimming the comma, changes what the procedure's split sees.
 */
@Repository
public class ProductionReportsRepository {

    private final JdbcTemplate jdbc;
    public ProductionReportsRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Map<String, Object> params() { return new LinkedHashMap<>(); }

    private List<Map<String, Object>> exec(String proc, Map<String, Object> p) {
        StringBuilder sql = new StringBuilder("EXEC ").append(qualify(proc));
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            values.add(e.getValue());
            first = false;
        }
        return jdbc.queryForList(sql.toString(), values.toArray());
    }

    private static String qualify(String proc) {
        String t = proc.trim();
        return (t.startsWith("[") || t.toLowerCase().startsWith("dbo.")) ? t : "dbo." + t;
    }

    private static boolean set(String v) { return v != null && !v.trim().isEmpty(); }

    // ============================================================ 975 — Job Order Summary

    /**
     * frmProductionJobOrderSummaryRpt.cs:171 → InvFoodProduction.JobOrderSummary (BLL 0329)
     * → {@code [dbo].[usp_JobOrderSummary_Report]}.
     *
     * <pre>
     *   &#64;OrganizationId, &#64;CompanyId   always
     *   &#64;IsApproved                   only when the approval filter is NOT "All"
     *   &#64;FromDate                     only when the From-date checkbox is ticked
     *   &#64;ToDate                       always — the form sets it unconditionally
     * </pre>
     *
     * The desktop's From date carries its own checkbox ({@code datFromDate.ShowCheckBox = true},
     * default unchecked), so "no from date" is a real, common query rather than an oversight.
     *
     * @param isApproved null means the "All" radio — the parameter is then not sent at all
     */
    public List<Map<String, Object>> jobOrderSummary(UserAccount u, String fromDate, String toDate,
                                                     Boolean isApproved) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (isApproved != null)  p.put("@IsApproved", isApproved);
        if (set(fromDate))       p.put("@FromDate", fromDate);
        if (set(toDate))         p.put("@ToDate", toDate);
        return exec("[dbo].[usp_JobOrderSummary_Report]", p);
    }

    /**
     * The row drill-down — frmProductionJobOrderSummaryRpt.cs:235 →
     * InvFoodProductionReports.ProductionSettlement_WithReferenceDocumentDetailReport (BLL 0118)
     * → {@code [dbo].[USP_ProductionSettlement_WithReferenceDocumentDetailReport]}.
     *
     * &#64;ActionId is sent only when non-zero, as in the C#.
     */
    public List<Map<String, Object>> productionSettlementDetail(UserAccount u, int jobOrderId,
                                                                int actionId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@JobOrderId", jobOrderId);
        if (actionId != 0) p.put("@ActionId", actionId);
        return exec("[dbo].[USP_ProductionSettlement_WithReferenceDocumentDetailReport]", p);
    }

    // ============================================================ 309 — Production Summary Report
    //
    // ProductionSummaryReport.cs. Five reads behind one Show button (btnShow_Click:477):
    //
    //   GrdSummaryValuesFill            Sp_InvFoodProduction_GetSummeryValues
    //   SummaryJobOrderInformationFill  (no read — the selected job-order dropdown row)
    //   GridSummaryJobOrderScheduleDetail
    //                                   USP_InvProductionJobOrderAndOrderAllocation_Summary...
    //   GrdSummaryReportFill            Sp_InvFoodProduction_Summery_Rpt
    //   grdDocWiseSummeryFill           [dbo].[USP_FoodProduction_DocWiseSummeryReport]
    //
    // plus three dropdown loads.

    /**
     * BranchesFill:205 → InvFoodProduction.GetBranchsAllocatedToUserFromProduction (BLL 0329:1183)
     * → {@code [dbo].[USP_GetBranchsAllocatedToUserFromProduction]}.
     *
     * The form passes DocumentTypeId 0 and DocumentTypeIds "" — both guarded in the BLL, so
     * neither parameter is sent. This is also the ONLY list of branches this screen may filter by:
     * the ids a browser posts back are checked against it before they reach a procedure.
     */
    public List<Map<String, Object>> productionBranchesForUser(UserAccount u) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@UserId", u.getId());
        return exec("[dbo].[USP_GetBranchsAllocatedToUserFromProduction]", p);
    }

    /**
     * JobOrderNoFillForSummery:271 → InvProductionJobOrder.GetJobOrderNoWithInfo (BLL 0335:169)
     * → {@code [dbo].[USP_InvProductionJobOrder_GetJobOrderNoWithInfo]}.
     *
     * <pre>
     *   &#64;OrganizationId, &#64;CompanyId, &#64;FinancialYearId, &#64;DocumentTypeId   always
     *   &#64;PlanStatus, &#64;JobOrderId                                     not set by this form
     *   &#64;BranchesIds                  only when non-empty (string.IsNullOrEmpty guard)
     * </pre>
     *
     * DocumentTypeId is 403, fixed by the form — not a caller's choice.
     *
     * The dropdown is bound AllColumns:true because the form then reads JobOrderDocNo,
     * JobStartDate, JobOrderStatus, SettledStatus, ApprovedDate and LotCode straight off the
     * selected row (SummaryJobOrderInformationFill:588). The information panel is that row, not a
     * second query, so every column has to come back.
     */
    public List<Map<String, Object>> jobOrderNoWithInfo(UserAccount u, int financialYearId,
                                                        int documentTypeId, String branchesIds) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@FinancialYearId", financialYearId);
        p.put("@DocumentTypeId", documentTypeId);
        if (set(branchesIds)) p.put("@BranchesIds", branchesIds);
        return exec("[dbo].[USP_InvProductionJobOrder_GetJobOrderNoWithInfo]", p);
    }

    /**
     * cmbsummeryJobOrderNo_TextChanged:293 → InvProductionJobOrder.GetPlantByJobOrderId
     * (BLL 0335:977) → {@code Sp_InvProductionJobOrder_GetAllMethod @Activity='GetPlantByJobOrderId'}.
     *
     * &#64;BranchesId here is {@code UserAccount.BranchesId} — the signed-in user's OWN branch, not
     * the branches ticked in the filter. The desktop passes that one, so this does too.
     */
    public List<Map<String, Object>> plantByJobOrderId(int jobOrderId, int branchesId) {
        Map<String, Object> p = params();
        p.put("@Id", jobOrderId);
        p.put("@BranchesId", branchesId);
        p.put("@Activity", "GetPlantByJobOrderId");
        return exec("Sp_InvProductionJobOrder_GetAllMethod", p);
    }

    /**
     * GrdSummaryValuesFill:492 → InvFoodProduction.GetSummeryValues (BLL 0329:543)
     * → {@code Sp_InvFoodProduction_GetSummeryValues}.
     *
     * <pre>
     *   &#64;OrganizationId, &#64;CompanyId, &#64;JobOrderId   always
     *   &#64;PlantId        only when non-zero
     *   &#64;FgNetWeight    only when non-zero — this form never sets NetWeight, so never sent
     *   &#64;FromDate       only when the From checkbox is ticked
     *   &#64;ToDate         only when the To checkbox is ticked
     *   &#64;BranchesIds    always (see the class note)
     * </pre>
     */
    public List<Map<String, Object>> productionSummaryValues(UserAccount u, int jobOrderId,
                                                             int plantId, String fromDate,
                                                             String toDate, String branchesIds) {
        return exec("Sp_InvFoodProduction_GetSummeryValues",
                summaryParams(u, jobOrderId, plantId, fromDate, toDate, branchesIds));
    }

    /**
     * GrdSummaryReportFill:672 → InvFoodProductionReports.InvFoodProductionRecoverySummeryReport
     * (BLL 0118:134) → {@code Sp_InvFoodProduction_Summery_Rpt}.
     *
     * Same parameter set as the summary values, plus an &#64;ActionId guarded on non-zero which
     * this form never sets — so it is not sent.
     */
    public List<Map<String, Object>> productionRecoverySummary(UserAccount u, int jobOrderId,
                                                               int plantId, String fromDate,
                                                               String toDate, String branchesIds) {
        return exec("Sp_InvFoodProduction_Summery_Rpt",
                summaryParams(u, jobOrderId, plantId, fromDate, toDate, branchesIds));
    }

    /**
     * grdDocWiseSummeryFill:767 → InvFoodProductionReports.FoodProduction_DocWiseSummeryReport
     * (BLL 0118:990) → {@code [dbo].[USP_FoodProduction_DocWiseSummeryReport]}.
     */
    public List<Map<String, Object>> productionDocWiseSummary(UserAccount u, int jobOrderId,
                                                              int plantId, String fromDate,
                                                              String toDate, String branchesIds) {
        return exec("[dbo].[USP_FoodProduction_DocWiseSummeryReport]",
                summaryParams(u, jobOrderId, plantId, fromDate, toDate, branchesIds));
    }

    /**
     * The parameter set the three 309 grids share. Written once because the three BLL methods
     * build it identically — not because the procedures are interchangeable; each grid still calls
     * its own.
     */
    private Map<String, Object> summaryParams(UserAccount u, int jobOrderId, int plantId,
                                              String fromDate, String toDate, String branchesIds) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@JobOrderId", jobOrderId);
        if (plantId != 0)  p.put("@PlantId", plantId);
        if (set(fromDate)) p.put("@FromDate", fromDate);
        if (set(toDate))   p.put("@ToDate", toDate);
        /* Always sent — the C# guard is true for every value. Empty string, not null: a null
           SqlParameter value and an empty string are not the same thing to a procedure that
           splits it. */
        p.put("@BranchesIds", branchesIds == null ? "" : branchesIds);
        return p;
    }

    /**
     * GridSummaryJobOrderScheduleDetail:628 →
     * InvProductionJobOrder.InvProductionJobOrderAndOrderAllocation_SummaryByJobOrderId
     * (BLL 0335:151) → {@code USP_InvProductionJobOrderAndOrderAllocation_SummaryByJobOrderId}.
     *
     * One parameter, &#64;JobOrderId. The procedure carries no organization or company of its own,
     * so the job order id is checked against this user's own dropdown list before it gets here.
     */
    public List<Map<String, Object>> jobOrderScheduleSummary(int jobOrderId) {
        Map<String, Object> p = params();
        p.put("@JobOrderId", jobOrderId);
        return exec("USP_InvProductionJobOrderAndOrderAllocation_SummaryByJobOrderId", p);
    }

    // ============================================================ 310 — Production Register
    //
    // ProductionRegister.cs. Three reads: one dropdown source that feeds six pickers, the job
    // order list, and the register itself — which returns one of three different shapes
    // depending on the Activity asked for.

    /**
     * AllCombobind:288 → InvFoodProduction.GetDataForDropDownFromFoodProduction (BLL 0329:1227)
     * → {@code USP_GetDataForDropDownFromFoodProduction}.
     *
     * The form constructs its ReportsParameters with ONLY OrganizationId and CompanyId, and every
     * other parameter in that BLL is guarded, so exactly two are sent. One result set comes back
     * carrying an {@code Activity} discriminator, and the form splits it into six pickers — see
     * ProductionReportsService.productionRegisterLookups.
     */
    public List<Map<String, Object>> productionDropdownSource(UserAccount u) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        return exec("USP_GetDataForDropDownFromFoodProduction", p);
    }

    /**
     * JobOrderBind:261 → InvFoodProduction.getJobOrderFromProduction (BLL 0329:1392)
     * → {@code usp_getJobOrderFromProduction}. Two parameters, both always sent.
     *
     * A different list from the 309 picker: that one is
     * USP_InvProductionJobOrder_GetJobOrderNoWithInfo with DocumentTypeId 403. The two forms read
     * different sources, so they are not routed through one endpoint even though both are called
     * "Job Order No" on screen.
     */
    public List<Map<String, Object>> jobOrdersFromProduction(UserAccount u) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        return exec("usp_getJobOrderFromProduction", p);
    }

    /**
     * Gridfill:556 → InvFoodProduction.ProductionRegister (BLL 0329:1415)
     * → {@code USP_ProductionRegisterWithActivity}.
     *
     * <pre>
     *   &#64;OrganizationId, &#64;CompanyId   always
     *   &#64;Activity                     always — it selects which of three shapes comes back
     *   &#64;InvJobOrderId                only when non-zero   (from JobOrderId)
     *   &#64;FromDate / &#64;ToDate           doc-date mode only
     *   &#64;EntryFromDate / &#64;EntryToDate entry-date mode only
     *   &#64;PlantId, &#64;ItemId, &#64;WarehouseId                     each only when non-zero
     *   &#64;ItemStockAccountId           only when non-zero   (from StockAccountId)
     *   &#64;ParentCategoriesId           only when non-zero   (from ParentCategoryId)
     *   &#64;WipAccountId                 only when non-zero   (from AccountId)
     *   &#64;EntryTypeDetail              only when non-empty  (from EntryType)
     *   &#64;EntryType                    from ReportType — this form never sets it, so never sent
     *   &#64;BranchesIds                  only when non-empty
     * </pre>
     *
     * Six of those parameter names differ from the control they come from, which is why each is
     * named above rather than inferred.
     *
     * The BLL spells two of them with a trailing space — {@code "@EntryType "} and
     * {@code "@EntryTypeDetail "}. In T-SQL {@code EXEC p @EntryTypeDetail =?} is the same
     * statement as {@code EXEC p @EntryTypeDetail=?}, the space being ordinary whitespace before
     * the {@code =}, so it is written without one here.
     *
     * Unlike the 309 procedures, &#64;BranchesIds is genuinely guarded here
     * ({@code obj.BranchesIds != null && obj.BranchesIds != string.Empty}) — so an empty selection
     * omits it. The form never reaches this call with an empty branch text anyway: Gridfill throws
     * "Select Branch First" first.
     */
    public List<Map<String, Object>> productionRegister(
            UserAccount u, String activity, int jobOrderId, String fromDate, String toDate,
            String entryFromDate, String entryToDate, int plantId, int itemId,
            int stockAccountId, int warehouseId, int parentCategoryId, int wipAccountId,
            String entryTypeDetail, String branchesIds) {

        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (jobOrderId != 0)        p.put("@InvJobOrderId", jobOrderId);
        if (set(fromDate))          p.put("@FromDate", fromDate);
        if (set(toDate))            p.put("@ToDate", toDate);
        if (set(entryFromDate))     p.put("@EntryFromDate", entryFromDate);
        if (set(entryToDate))       p.put("@EntryToDate", entryToDate);
        if (plantId != 0)           p.put("@PlantId", plantId);
        if (itemId != 0)            p.put("@ItemId", itemId);
        if (stockAccountId != 0)    p.put("@ItemStockAccountId", stockAccountId);
        if (warehouseId != 0)       p.put("@WarehouseId", warehouseId);
        if (parentCategoryId != 0)  p.put("@ParentCategoriesId", parentCategoryId);
        if (wipAccountId != 0)      p.put("@WipAccountId", wipAccountId);
        if (set(entryTypeDetail))   p.put("@EntryTypeDetail", entryTypeDetail);
        if (set(branchesIds))       p.put("@BranchesIds", branchesIds);
        p.put("@Activity", activity);
        return exec("USP_ProductionRegisterWithActivity", p);
    }

    // ============================================================ 308 — Production Comparison
    //
    // FoodProductionComparisonRpt.cs — a two-tab form. The Comparison tab has one grid; the
    // Summary tab has one grid that holds either of two reports, chosen by a radio.

    /**
     * ProductionTypeBind:270 → ProductionType.GetAll (BLL 0327:52)
     * → {@code Sp_ProductionType_GetAllMethod}.
     *
     * The form passes {@code new ProductionType()}, so Id is 0 and its guard drops it — the
     * procedure is called with NO parameters at all.
     */
    public List<Map<String, Object>> productionTypes() {
        return exec("Sp_ProductionType_GetAllMethod", params());
    }

    /**
     * ComboBindComparison:307 and ComboBindSummary:387 → the same dropdown source as 310, but
     * called with more parameters: this form sets FinancialYearId and BranchesIds as well.
     *
     * Both guards are real here ({@code obj.FinancialYearId != 0},
     * {@code !string.IsNullOrEmpty(obj.BranchesIds)}), so an unset value is omitted.
     */
    public List<Map<String, Object>> productionDropdownSource(UserAccount u, int financialYearId,
                                                              String branchesIds) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (financialYearId != 0) p.put("@FinancialYearId", financialYearId);
        if (set(branchesIds))     p.put("@BranchesIds", branchesIds);
        return exec("USP_GetDataForDropDownFromFoodProduction", p);
    }

    /**
     * CmbItemName_Leave:465 → CommonServices.GetUomScheduleByItemId:5239 →
     * UOMSchedule.SearchByObject (BLL 0610:89) →
     * {@code Sp_UOMSchedule_GetAllMethod @Activity='ReadByItemID'}.
     *
     * The same contract ProductionJobOrderLookupsRepository.uomSchedule already uses. It is
     * repeated here rather than shared so the two screens cannot drift into each other: they call
     * the same procedure today, which is a fact about the desktop, not a dependency between them.
     */
    public List<Map<String, Object>> uomScheduleByItem(UserAccount u, int itemId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ItemId", itemId);
        p.put("@Activity", "ReadByItemID");
        return exec("Sp_UOMSchedule_GetAllMethod", p);
    }

    /**
     * gridHisory:651 → InvFoodProduction.FoodProductionComparisons_Rpt (BLL 0329:606)
     * → {@code SpInvFoodProductionComparisons_Rpt}.
     *
     * <pre>
     *   &#64;OrganizationId, &#64;CompanyId, &#64;ProductionType   always
     *   &#64;DocNoFrom, &#64;DocNoTo    each only when non-zero
     *   &#64;PlantId                 only when non-zero
     *   &#64;JobOrderId              only when non-zero
     * </pre>
     *
     * &#64;ProductionType carries the production-type CAPTION, not its id
     * ({@code obj.ReportType = cmbProductionTypeComparison.Text}).
     *
     * A trap worth naming: the form assigns the PLANT picker's value to {@code obj.ItemId}
     * (:648), and the BLL sends {@code obj.ItemId} as {@code @PlantId}. The two mistakes cancel
     * out, so the plant does reach &#64;PlantId — but reading either half alone would send the
     * wrong thing. The parameter here is named plantId, because that is what it is.
     *
     * This tab sends no branch filter at all; the branch tick-list only rebuilds its two pickers.
     */
    public List<Map<String, Object>> productionComparison(UserAccount u, String productionType,
                                                          int docNoFrom, int docNoTo, int plantId,
                                                          int jobOrderId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ProductionType", productionType == null ? "" : productionType);
        if (docNoFrom != 0)  p.put("@DocNoFrom", docNoFrom);
        if (docNoTo != 0)    p.put("@DocNoTo", docNoTo);
        if (plantId != 0)    p.put("@PlantId", plantId);
        if (jobOrderId != 0) p.put("@JobOrderId", jobOrderId);
        return exec("SpInvFoodProductionComparisons_Rpt", p);
    }

    /**
     * GridFillInput:835 → InvFoodProductionReports.FoodProductionIssuanceGrnWiseByJobOrderId_607
     * (BLL 0118:574) → {@code Sp_InvFoodProductionIssuanceGrnWiseByJobOrderId_rpt}.
     *
     * <pre>
     *   &#64;OrganizationId, &#64;CompanyId   always
     *   &#64;InvJobOrderId  only when non-zero   (from JobOrderId)
     *   &#64;PlantId        only when non-zero
     *   &#64;ItemId         only when non-zero
     *   &#64;PackUomId      only when non-zero   (from ItemUomId)
     *   &#64;FromDate, &#64;ToDate   the form sets both unconditionally
     * </pre>
     *
     * The BLL also carries the always-true {@code @BranchesIds} guard, but THIS form never
     * assigns BranchesIds for this call, so there is no value of the desktop's to reproduce. It
     * is omitted rather than sent as an empty string, which would force a filter value the
     * desktop never chose and override whatever default the procedure declares.
     */
    public List<Map<String, Object>> productionIssuanceGrnWise(UserAccount u, int jobOrderId,
                                                               int plantId, int itemId,
                                                               int packUomId, String fromDate,
                                                               String toDate) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (jobOrderId != 0) p.put("@InvJobOrderId", jobOrderId);
        if (plantId != 0)    p.put("@PlantId", plantId);
        if (itemId != 0)     p.put("@ItemId", itemId);
        if (packUomId != 0)  p.put("@PackUomId", packUomId);
        if (set(fromDate))   p.put("@FromDate", fromDate);
        if (set(toDate))     p.put("@ToDate", toDate);
        return exec("Sp_InvFoodProductionIssuanceGrnWiseByJobOrderId_rpt", p);
    }

    /**
     * Production_GainLossSummary:904 → InvFoodProductionReports.Production_GainLossSummary_Rpt
     * (BLL 0118:653) → {@code SpProduction_GainLossSummary_Rpt}.
     *
     * Same pickers as the input grid, but three of the parameter names differ:
     * &#64;JobOrderId rather than &#64;InvJobOrderId, and &#64;ItemUomId rather than
     * &#64;PackUomId — and the pack UOM travels in {@code obj.stockUOM} here, not ItemUomId.
     */
    public List<Map<String, Object>> productionGainLossSummary(UserAccount u, int jobOrderId,
                                                               int plantId, int itemId,
                                                               int itemUomId, String fromDate,
                                                               String toDate) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (set(fromDate))   p.put("@FromDate", fromDate);
        if (set(toDate))     p.put("@ToDate", toDate);
        if (jobOrderId != 0) p.put("@JobOrderId", jobOrderId);
        if (itemId != 0)     p.put("@ItemId", itemId);
        if (itemUomId != 0)  p.put("@ItemUomId", itemUomId);
        if (plantId != 0)    p.put("@PlantId", plantId);
        return exec("SpProduction_GainLossSummary_Rpt", p);
    }

    // ============================================ 306 — Production PackingMaterial Consumption
    //
    // frmProductionPackingMaterialConsumptionRegister.cs. Two pickers from one source, one grid.
    //
    // GetMainItems:224 and GetPmItems:248 exist in the form but nothing calls them, so they are
    // not ported: porting dead code would add two procedure contracts this screen never uses.

    /**
     * ComboBind:169 → InvFoodProduction.GetDataForDropDownFromFoodProductionPM (BLL 0329:1337)
     * → {@code USP_GetDataForDropDownFromFoodProductionPM}.
     *
     * One result set with an {@code Activity} discriminator again, but different values from the
     * other two screens: "BrandItem" and "PMItem".
     *
     * The BLL carries always-true guards on &#64;Activity and &#64;DocumentTypeIds, but the form
     * assigns neither, so there is no desktop value to reproduce and they are omitted rather than
     * sent empty. &#64;BranchesIds and &#64;FinancialYearId the form does set, so both are sent.
     */
    public List<Map<String, Object>> packingMaterialDropdownSource(UserAccount u,
                                                                   int financialYearId,
                                                                   String branchesIds) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (financialYearId != 0) p.put("@FinancialYearId", financialYearId);
        if (set(branchesIds))     p.put("@BranchesIds", branchesIds);
        return exec("USP_GetDataForDropDownFromFoodProductionPM", p);
    }

    /**
     * GridBind:299 → InvFoodProductionReports.ProductionCumPackMaterialConsumption_Register_812
     * (BLL 0118:795) → {@code Sp_InvProductionCumPackMaterialConsumption_Register}.
     *
     * <pre>
     *   &#64;OrganizationId, &#64;CompanyId   always
     *   &#64;FromDate, &#64;ToDate  the form sets both unconditionally
     *   &#64;PmItemId       only when non-zero
     *   &#64;ItemId         only when non-zero
     *   &#64;BranchesIds    only when non-empty — genuinely guarded here
     * </pre>
     *
     * This grid is bound STRAIGHT to the result ({@code grd.DataSource = dtlst}) with no
     * intermediate table, so its columns are whatever the procedure projects. The page therefore
     * builds its headers from the result rather than from a list written here, which would drop
     * any column the procedure returns that this port had not seen.
     */
    public List<Map<String, Object>> packingMaterialConsumptionRegister(
            UserAccount u, String fromDate, String toDate, int itemId, int pmItemId,
            String branchesIds) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (set(fromDate))    p.put("@FromDate", fromDate);
        if (set(toDate))      p.put("@ToDate", toDate);
        if (pmItemId != 0)    p.put("@PmItemId", pmItemId);
        if (itemId != 0)      p.put("@ItemId", itemId);
        if (set(branchesIds)) p.put("@BranchesIds", branchesIds);
        return exec("Sp_InvProductionCumPackMaterialConsumption_Register", p);
    }
}
