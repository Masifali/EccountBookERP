package com.mst.repositories.lab;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 162 "In-Process Analysis" — Architecture.WinApp.Lab/InvLabAnalysisInProcess.cs, DocumentTypeId 306
 * (InvLabAnalysisInProcess.cs:369 / :919 / :1441).
 *
 * Every procedure, @Activity and parameter below was read from the desktop source and checked against
 * procdure.utf8.sql:
 *
 *   BLL 0397 / DAL 0353  Architecture.*.Lab.LabInProcessAnalysisRawHeader  (the class the form uses; BLL 0401
 *                        InvLabAnalysisInProcessHeader is only used by the 660 register report form)
 *        GenerateCode / GetAll / GetById / GetByAnalysisGroupId / BindAnalysisGroupHistory / GetByPlantId /
 *        BindPlantStepAnalysisHistory            -> USp_InvLabAnalysisInProcessHeader_GetAllMethod (sql :385328)
 *        GetDataForDropDownFromLabInProcessAnalysisRawHeader
 *                                                -> [dbo].[USP_GetDataForDropDownFromLabInProcessAnalysisRawHeader] (:341638)
 *        Top_InProcessAnalysisDataWithAnalysistimeAndResult
 *                                                -> usp_InProcessAnalysisDataWithAnalysistimeAndResult (:373238)
 *        Save (DAL SetData)                      -> USP_LabInProcessAnalysisRawHeader_Insert (:404728) /
 *                                                   USP_LabInProcessAnalysisRawHeader_Update (:404840),
 *                                                   Sp_InvLabAnalysisInProcessDetail_Insert (:164091),
 *                                                   USP_LabInProcessAnalysisRawDetail_Insert (:404676)
 *   BLL InvProductionPlant.GetAll                -> Sp_InvProductionPlant_GetAllMethod 'GetALL' (:176445)
 *   BLL 0335 InvProductionJobOrder_GetForInProcessLab -> [dbo].[USP_InvProductionJobOrder_GetForInProcessLab] (:387979)
 *   BLL 0329 InvFoodProduction_GetBrandItemsByJobOrder -> [dbo].[USP_InvFoodProduction_GetBrandItemsByJobOrder] (:380814)
 *   BLL 0400 InvLabAnalysisGroup.GetAllOrById    -> Sp_InvLabAnalysisGroup_GetAllMethod 'ReadAll' (:163925)
 *   CommonServices.CropYearGetAllService (:2276) -> Sp_InvCropYear_GetAllMethod 'ReadAll' (:124064)
 *   GlobalVariables_Helper.GetConfigValueFromGlobal -> Sp_ConfigrationsAllocation_GetAllMethod
 *
 * A parameter the BLL only adds under a condition is only added here under the same condition; a null is
 * never bound (DesktopProc omits it, as ADO.NET's AddWithValue omits a CLR null).
 */
@Repository
public class InProcessAnalysisRepository {

    public static final String P_GETALL = "USp_InvLabAnalysisInProcessHeader_GetAllMethod";

    private final JdbcTemplate jdbc;

    public InProcessAnalysisRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ============================================================================== numbering

    /** BLL 0397 GenerateCode (:34) — @OrganizationId, @CompanyId, @Activity='GenerateDocNo'; Rows[0]["DocNo"]. */
    public int generateCode(UserAccount u) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GenerateDocNo"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // ============================================================================== combos

    /** PlantFill (form :419) — InvProductionPlant.GetAll: @OrganizationId, @CompanyId, @Activity='GetALL'. */
    public List<Map<String, Object>> plants(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvProductionPlant_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetALL"));
    }

    /**
     * JobOrderNoFill (form :445) — BLL 0335 :306: @OrganizationId, @CompanyId, @FinancialYearId,
     * @PlanStatus='In Process' (Status non-empty), @RecId only when RecId != 0. DocumentTypeId is never set
     * by the form, so @DocumentTypeId is not sent.
     */
    public List<Map<String, Object>> jobOrders(UserAccount u, int financialYearId, int recId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId, "PlanStatus", "In Process");
        if (recId != 0) p.put("RecId", recId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_InvProductionJobOrder_GetForInProcessLab]", p);
    }

    /**
     * GetItemsBindByJobOrderDbCall (form :485) — BLL 0329 :1035: @OrganizationId, @CompanyId, @JobOrderId
     * (always, even 0), @DocumentTypeIds='80,112'. EntryType is never set, so @EntryType is not sent.
     */
    public List<Map<String, Object>> itemsByJobOrder(UserAccount u, int jobOrderId) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_InvFoodProduction_GetBrandItemsByJobOrder]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "JobOrderId", jobOrderId, "DocumentTypeIds", "80,112"));
    }

    /** CropYearFill (form :521) — InvCropYear.Getall: @OrganizationId, @CompanyId, @Activity='ReadAll'. */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvCropYear_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /**
     * AnalysisGroup (form :537) — BLL 0400 GetAllOrById (:51): Id and ParentCategoryId are 0 on this form so
     * neither is sent; @OrganizationId, @CompanyId, @Activity='ReadAll'.
     */
    public List<Map<String, Object>> analysisGroups(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvLabAnalysisGroup_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /**
     * HistoryComboFill (form :348) — BLL 0397 :316: @OrganizationId, @CompanyId, @FinancialYearId,
     * @DocumentTypeIds='306'. Activity is not set by the form, so @Activity is not sent (the procedure then
     * returns every list, tagged by its own "Activity" column).
     */
    public List<Map<String, Object>> historyDropDownData(UserAccount u, int financialYearId) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromLabInProcessAnalysisRawHeader]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId, "DocumentTypeIds", "306"));
    }

    /** GlobalVariables_Helper.GetConfigValueFromGlobal(description) — ConfigKey or "". */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v);
    }

    // ============================================================================== grids

    /**
     * cmbPlantName_Leave (form :670) — GetByPlantId (BLL 0397 :272): @OrganizationId, @CompanyId, @PlantId
     * (always, even 0), @Activity='GetByPlantId'.
     */
    public List<Map<String, Object>> stepScheduleByPlant(UserAccount u, int plantId) {
        return DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "PlantId", plantId, "Activity", "GetByPlantId"));
    }

    /**
     * cmbanalysisGroup_Leave (form :586) — GetByAnalysisGroupId (BLL 0397 :218): @OrganizationId, @CompanyId,
     * @InvLabAnalysisGroup (always, even 0), @Activity='GetByAnalysisGroupId'.
     */
    public List<Map<String, Object>> standardsByAnalysisGroup(UserAccount u, int analysisGroupId) {
        return DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "InvLabAnalysisGroup", analysisGroupId, "Activity", "GetByAnalysisGroupId"));
    }

    /**
     * TopFiveStepAnalysisResultsAgainstPlantJobOrderAndItem (form :765) — BLL 0397 :378: @OrganizationId,
     * @CompanyId, @PlantId and @JobOrderId ALWAYS (0 included — the BLL has no non-zero guard on these two);
     * ItemId is never set by the form, so @ItemId is not sent.
     */
    public List<Map<String, Object>> topResults(UserAccount u, int plantId, int jobOrderId) {
        return DesktopProc.rows(jdbc, "usp_InProcessAnalysisDataWithAnalysistimeAndResult", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "PlantId", plantId, "JobOrderId", jobOrderId));
    }

    // ============================================================================== read

    /** GetById (BLL 0397 :196) — @Id, @Activity='ReadById'. */
    public List<Map<String, Object>> readById(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
    }

    /** BindPlantStepAnalysisHistory (BLL 0397 :304) — @OrganizationId, @CompanyId, @Id, @Activity. */
    public List<Map<String, Object>> stepRowsOfRecord(UserAccount u, int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", id, "Activity", "BindPlantStepAnalysisHistory"));
    }

    /** BindAnalysisGroupHistory (BLL 0397 :250) — @Id, @Activity='BindAnalysisGroupHistory'. */
    /**
     * BLL 0069 DMSAttachments.ReadAttachmentsbyRefDocumentTypeId — @RefDocumentTypeId, @Id,
     * @Activity='ReadAttachmentsbyRefDocumentTypeId' (grdhistory_LinkClicked "NoOfAttachments" :1399 ->
     * CommonServices.GetNoofAttachmentsByRefDocumentTypeID, CommonServices.cs:4552).
     */
    public List<Map<String, Object>> attachments(int id, int documentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", params(
                "RefDocumentTypeId", documentTypeId, "Id", id, "Activity", "ReadAttachmentsbyRefDocumentTypeId"));
    }

    public List<Map<String, Object>> groupRowsOfRecord(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "BindAnalysisGroupHistory"));
    }

    /**
     * historygridfill (form :1432) — GetAll (BLL 0397 :57), parameters in the BLL's order and under the BLL's
     * conditions: @OrganizationId, @CompanyId, @DocumentTypeId, @FinancialYearId, @CanViewAllRecord always;
     * @EntryUser only when CanViewAllRecord is false; @FromDate / @ToDate only when the picker is checked;
     * @FromDocNo / @ToDocNo / @ItemId / @PlantId / @JobOrderId only when non-zero; @Analyst only when the
     * combo text is not empty; @Activity='ReadAll'. BranchesId and NoOfRecords are never set by the form.
     */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId, int documentTypeId,
                                             boolean canViewAll, Timestamp fromDate, Timestamp toDate,
                                             int fromDocNo, int toDocNo, int itemId, int plantId,
                                             int jobOrderId, String analyst) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("DocumentTypeId", documentTypeId);
        p.put("FinancialYearId", financialYearId);
        p.put("CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUser", u.getId());
        if (fromDate != null) p.put("FromDate", fromDate);
        if (toDate != null) p.put("ToDate", toDate);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (itemId != 0) p.put("ItemId", itemId);
        if (plantId != 0) p.put("PlantId", plantId);
        if (jobOrderId != 0) p.put("JobOrderId", jobOrderId);
        if (analyst != null && !analyst.isEmpty()) p.put("Analyst", analyst);
        p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    // ============================================================================== save

    /**
     * GenericProvider.SetProc(obj, "USP_LabInProcessAnalysisRawHeader_Insert" | "..._Update") — every
     * non-virtual property of Model 0614 LabInProcessAnalysisRawHeader is a parameter (DAL 0207 :283), so the
     * map passed in carries exactly those 24 names; a null value (unset string) is left out. Returns
     * Convert.ToInt32(ExecuteScalar()): the new identity on insert, 0 on update (no result set).
     */
    public int saveHeader(boolean insert, Map<String, Object> header) {
        return DesktopProc.setProc(jdbc,
                insert ? "USP_LabInProcessAnalysisRawHeader_Insert" : "USP_LabInProcessAnalysisRawHeader_Update", header);
    }

    /**
     * Model InvLabAnalysisInProcessDetail through SetProc -> Sp_InvLabAnalysisInProcessDetail_Insert:
     * @Analysistime, @InAnalysisResult, @Id (0 — the procedure takes MAX+1), @AnalysisParameterID,
     * @InvLabAnalysisInProcessHeaderId, @InvLabGroupAnalysisStandardsId, @RemarksDetail.
     */
    public void insertGroupDetail(int headerId, Timestamp analysisTime, double result, int parameterId,
                                  int groupAnalysisStandardsId, String remarks) {
        DesktopProc.setProc(jdbc, "Sp_InvLabAnalysisInProcessDetail_Insert", params(
                "Analysistime", analysisTime, "InAnalysisResult", result, "Id", 0,
                "AnalysisParameterID", parameterId, "InvLabAnalysisInProcessHeaderId", headerId,
                "InvLabGroupAnalysisStandardsId", groupAnalysisStandardsId, "RemarksDetail", remarks));
    }

    /**
     * Model LabInProcessAnalysisRawDetail through SetProc -> USP_LabInProcessAnalysisRawDetail_Insert:
     * @Analysistime, @ResultValue, @Id (0 — MAX+1 in the procedure), @LabInProcessAnalysisHeaderId,
     * @ParameterId, @StepId, @RemarkDetail.
     */
    public void insertStepDetail(int headerId, Timestamp analysisTime, java.math.BigDecimal result,
                                 int parameterId, int stepId, String remark) {
        DesktopProc.setProc(jdbc, "USP_LabInProcessAnalysisRawDetail_Insert", params(
                "Analysistime", analysisTime, "ResultValue", result, "Id", 0,
                "LabInProcessAnalysisHeaderId", headerId, "ParameterId", parameterId, "StepId", stepId,
                "RemarkDetail", remark));
    }

    // ------------------------------------------------------------------------------ helpers

    public static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        try {
            String s = String.valueOf(v).trim().replace(",", "");
            if (s.isEmpty()) return 0;
            return (int) Math.rint(Double.parseDouble(s));           // Conversion.ToInt: Convert.ToInt32 semantics
        } catch (NumberFormatException e) {
            return 0;
        }
    }

    public static double toDouble(Object v) {
        if (v == null) return 0d;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try {
            String s = String.valueOf(v).trim().replace(",", "");
            return s.isEmpty() ? 0d : Double.parseDouble(s);
        } catch (NumberFormatException e) {
            return 0d;
        }
    }

    public static String str(Object v) { return v == null ? "" : String.valueOf(v); }
}
