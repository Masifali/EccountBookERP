package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Date;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Procedure calls for the module-46 report group "C":
 *   458 Store Issuance Report (IssuanceHistory.cs, ScreenName StoreIssuenceHistory)
 *   460 General Gate Pass Report (frmGatePassGeneral.cs, ScreenName frmGatePassGeneral)
 *
 * Every map is built in the BLL's own parameter order, and every parameter the BLL guards
 * (if (obj.X != 0) / if (!CheckDateTimeNull) / if (s != string.Empty)) is left null here, which
 * DesktopProc omits from the EXEC — the ADO.NET "not added" semantics. Organization and company
 * always come from the signed-in user.
 */
@Repository
public class StoreReportsCRepository {

    private final JdbcTemplate jdbc;

    public StoreReportsCRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Integer nz(int v) { return v != 0 ? v : null; }
    private static String ne(String v) { return v == null || v.isEmpty() ? null : v; }

    // ================================================================= 458 Store Issuance Report

    /**
     * IssuanceHistory.FillAllDropDowns (IssuanceHistory.cs:122) → BLL 0252:1390
     * InvGsStoreIssuanceHeader.GetDataForDropDownFromInvGsStoreIssuanceHeader
     * → USP_GetDataForDropDownFromInvGsStoreIssuanceHeader @OrganizationId, @CompanyId
     * (@DocumentTypeIds / @Activity not set by the form, so omitted — every activity comes back).
     */
    public List<Map<String, Object>> issuanceDropDownSource(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromInvGsStoreIssuanceHeader]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId()));
    }

    /**
     * IssuanceHistory.GridFill (IssuanceHistory.cs:245) → BLL 0252:826 StoreIssuanceHistory
     * → Sp_InvGsStoreIssuanceHeader_SlipandRegister. BLL order: OrganizationId, CompanyId,
     * [FinancialYearId], [Id], [DepartmentId ← RefDocumentTypeId], [ItemId], [AssetId], [FromDate],
     * [ToDate], [WareHouseId], [FromDocNo], [ToDocNo], [AccountId], [InvoiceId], [ItemConditionId],
     * [Ids]. The form never sets FinancialYearId / InvoiceId / Ids, so they are always omitted.
     */
    public List<Map<String, Object>> issuanceHistory(UserAccount u, int id, int departmentId, int itemId,
                                                     int assetId, Date fromDate, Date toDate, int warehouseId,
                                                     int fromDocNo, int toDocNo, int accountId,
                                                     int itemConditionId) {
        return DesktopProc.rows(jdbc, "Sp_InvGsStoreIssuanceHeader_SlipandRegister", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Id", nz(id),
                "DepartmentId", nz(departmentId),
                "ItemId", nz(itemId),
                "AssetId", nz(assetId),
                "FromDate", fromDate,
                "ToDate", toDate,
                "WareHouseId", nz(warehouseId),
                "FromDocNo", nz(fromDocNo),
                "ToDocNo", nz(toDocNo),
                "AccountId", nz(accountId),
                "ItemConditionId", nz(itemConditionId)));
    }

    /**
     * CommonServices.VoucherReport_118 (CommonServices.cs:5647) → BLL 0141:562
     * VoucherReports.VoucherValidationReport → Sp_Accounts_VouchersValidation_Rpt.
     * Set by VoucherReport_118: OrganizationId, CompanyId, UserId, Id, DocumentTypeId,
     * ApprovedFilter "All" (so @IsApproved is omitted). BLL order kept.
     */
    public List<Map<String, Object>> voucher118(UserAccount u, int voucherHeadId, int documentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_Accounts_VouchersValidation_Rpt", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "UserId", u.getId() == null ? null : nz(u.getId()),
                "Id", nz(voucherHeadId),
                "DocumentTypeId", nz(documentTypeId)));
    }

    // =============================================================== 460 General Gate Pass Report

    /**
     * frmGatePassGeneral.gridHisory (frmGatePassGeneral.cs:226) → BLL 0123
     * GatePassGeneralReports.GatePassGeneralRegister → Sp_GatePassGeneral_Register.
     * BLL order: DocumentTypeId (ALWAYS, even 0), OrganizationId, CompanyId, [FromDate], [ToDate],
     * [WeighableStatus ← ReqType, when != ""], [Status, when != ""], [GpNoFrom], [GpNoTo],
     * [OnlyPending ← PendingForView].
     */
    public List<Map<String, Object>> gatePassGeneralRegister(UserAccount u, int documentTypeId, Date fromDate,
                                                             Date toDate, String weighableStatus, String status,
                                                             int gpNoFrom, int gpNoTo, int pendingForView) {
        return DesktopProc.rows(jdbc, "Sp_GatePassGeneral_Register", params(
                "DocumentTypeId", documentTypeId,
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "FromDate", fromDate,
                "ToDate", toDate,
                "WeighableStatus", ne(weighableStatus),
                "Status", ne(status),
                "GpNoFrom", nz(gpNoFrom),
                "GpNoTo", nz(gpNoTo),
                "OnlyPending", nz(pendingForView)));
    }

    /**
     * DataGridHistory_ColumnButtonClick (frmGatePassGeneral.cs:372) → BLL 0133:148
     * GatePassInwardReports.GatePassGeneralInward254 → Sp_GatePassGeneralInward_rpt
     * @OrganizationId, @CompanyId, @Id, @DocumentTypeId — all four always sent.
     */
    public List<Map<String, Object>> gatePassGeneralInward254(UserAccount u, int id, int documentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_GatePassGeneralInward_rpt", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Id", id,
                "DocumentTypeId", documentTypeId));
    }

    /**
     * DataGridHistory_KeyDown Ctrl+Space on "Slip" (frmGatePassGeneral.cs:641) → BLL 0124:93
     * GatePassOutwardReports.GatePassOutwardSlipandRegister → Sp_GatePassOutward_SlipAndRegister_Rpt.
     * Only OrganizationId, CompanyId and Id are set; every other BLL guard is false.
     */
    public List<Map<String, Object>> gatePassOutwardSlip290(UserAccount u, int id) {
        return DesktopProc.rows(jdbc, "Sp_GatePassOutward_SlipAndRegister_Rpt", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Id", nz(id)));
    }

    // ==================================================================================== shared

    /** Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId — the rows ActiveYr is picked from. */
    public List<Map<String, Object>> activeFinancialYears(UserAccount u) {
        return jdbc.queryForList(
                "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId());
    }
}
