package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * 953 "Fcy Receipts Utilization Against Bank Invoice / GD" - Architecture.WinApp.Export.
 * frmGdUtilizationAgainstFcyReceipt. Data layer: Architecture.BLL.Export.FcyBankReceiptBreakUp
 * (BLL 0444 / DAL 0497), ExImFCBankReceipts.GetPendingFcyReceiptsAgainstBank, ExImInvoice.
 * GetGDsFromCustomInvoicePaymentScheduleAndGdMappingForFcyReciptsMapping (BLL 0467).
 * Procedures, read from procdure.utf8.sql on 30-Sep-2026:
 *
 *   Usp_GetPendingFcyReceiptsAgainstBank      @OrganizationId @CompanyId            (FinancialYearId 0 -> not sent)  pending receipts grid
 *   usp_GetGDsFromCustomInvoicePaymentScheduleAndGdMappingForFcyReciptsMapping  @OrganizationId @CompanyId  (Id/RecId/RefDocumentTypeId 0 -> not sent)  detail grid
 *   Usp_GDUtilizationMainHistory              @OrganizationId @CompanyId [@FromDate] [@ToDate]   history grid (FinancialYearId 0 -> not sent)
 *   Usp_GetGDUtilizationHistoryByFcyReceipt   @Id                                                "GD Utilizing Info" grid / ReadById
 *   usp_FcyBankReceiptBreakUpDeleteByFcyReceiptsId @Id                                            first statement of the save
 *   USP_FcyBankReceiptBreakUp_Insert           the 13 non-virtual model properties                one call per detail row
 *   Sp_ExImFCBankReceipts_GetAllMethod 'GdUtilizeDeleteByFcyId' @Id @Activity                     history X button
 *
 * DAL FcyBankReceiptBreakUp.SetData: DELETE FROM FcyBankReceiptBreakUp WHERE FcyBankReceiptId = @Id
 * (obj.Id = the receipt id of the last grid row), then SetProc per row, one transaction.
 */
@Repository
public class ExportFcyGdUtilizationRepository {

    private final JdbcTemplate jdbc;

    public ExportFcyGdUtilizationRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** clsGlobalVariables.configrationsAllocation "DefaultDaysToLessFromHistoryFromDate". */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** ExImFCBankReceipts.GetPendingFcyReceiptsAgainstBank(org, comp, 0). */
    public List<Map<String, Object>> pendingReceipts(UserAccount u) {
        return DesktopProc.rows(jdbc, "Usp_GetPendingFcyReceiptsAgainstBank",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ExImInvoice.GetGDsFromCustomInvoicePaymentScheduleAndGdMappingForFcyReciptsMapping(org, comp, 0, 0, 0). */
    public List<Map<String, Object>> gdsForMapping(UserAccount u) {
        return DesktopProc.rows(jdbc, "usp_GetGDsFromCustomInvoicePaymentScheduleAndGdMappingForFcyReciptsMapping",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** FcyBankReceiptBreakUp.FormHistory(ReportsParameters) - dates only when ticked. */
    public List<Map<String, Object>> history(UserAccount u, java.sql.Date from, java.sql.Date to) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        return DesktopProc.rows(jdbc, "[dbo].[Usp_GDUtilizationMainHistory]", p);
    }

    /** FcyBankReceiptBreakUp.GetGDUtilizationHistoryByFcyReceipt(Id). */
    public List<Map<String, Object>> utilizationByReceipt(int fcyReceiptId) {
        return DesktopProc.rows(jdbc, "[dbo].[Usp_GetGDUtilizationHistoryByFcyReceipt]", params("Id", fcyReceiptId));
    }

    /** DAL FcyBankReceiptBreakUp.SetData(obj, "[dbo].[USP_FcyBankReceiptBreakUp_Insert]"). */
    @Transactional(rollbackFor = Exception.class)
    public int save(int fcyReceiptId, List<Map<String, Object>> rows) {
        DesktopProc.rows(jdbc, "usp_FcyBankReceiptBreakUpDeleteByFcyReceiptsId", params("Id", fcyReceiptId));
        int result = 0;
        for (Map<String, Object> r : rows) {
            Integer v = DesktopProc.scalar(jdbc, "[dbo].[USP_FcyBankReceiptBreakUp_Insert]", r);
            result = v == null ? 0 : v;
        }
        return result;
    }

    /** FcyBankReceiptBreakUp.DeleteByFcyReceipt(Id) - Sp_ExImFCBankReceipts_GetAllMethod @Activity='GdUtilizeDeleteByFcyId'. */
    @Transactional(rollbackFor = Exception.class)
    public void deleteByFcyReceipt(int fcyReceiptId) {
        DesktopProc.rows(jdbc, "[dbo].[Sp_ExImFCBankReceipts_GetAllMethod]", params("Id", fcyReceiptId, "Activity", "GdUtilizeDeleteByFcyId"));
    }
}
