package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Store Issuance (Financials), desktop Architecture.WinApp.StoreManagement.StoreIssuanceFinancial (DocumentTypeId 451).
 * Only the reads this form has of its own; everything it shares with frmGSIssuance (header / details / doc number /
 * voucher head id / date lock / item GL accounts / inventory validation) is read from StoreIssuanceRepository.
 */
@Repository
public class StoreIssuanceFinancialRepository {

    private final JdbcTemplate jdbc;
    public StoreIssuanceFinancialRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** CommonServices.Accounts_GetAccountTitleByAccountTypeIds -> COAAllocation.Accounts_GetAccountTitleByAccountTypeIds. */
    public List<Map<String, Object>> accountTitles(UserAccount u, String accountTypeIds) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "AppId", u.getAppId() == null ? 0 : u.getAppId());
        p.put("AccountTypeIds", accountTypeIds);
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        return DesktopProc.rows(jdbc, "USP_Accounts_GetAccountTitleByAccountTypeIds", p);
    }

    /** InvGsStoreIssuanceHeader.FormHistoryForFinancialEffects (BLL 0252 :323). NoOfRecords 0 and CanViewAllRecord true are omitted / fixed as on the form. */
    public List<Map<String, Object>> historyForFinancialEffects(UserAccount u, int documentTypeId, int financialYearId) {
        Map<String, Object> p = params(
                "Activity", "FormHistoryForFinancialEffects",
                "organizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("CanViewAllRecord", Boolean.TRUE);
        return DesktopProc.rows(jdbc, "Sp_InvGsStoreIssuanceHeader_GetAllMethod", p);
    }

    /** BLL 0252 ReadByDocNo - the Id of the document with this DocNo (0 when none). */
    public int idByDocNo(UserAccount u, int documentTypeId, int docNo) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_InvGsStoreIssuanceHeader_GetAllMethod", params(
                "DocNo", docNo,
                "OrganizationId", u.getOrganizationId(),
                "DocumentTypeId", documentTypeId,
                "CompanyId", u.getCompanyId(),
                "Activity", "ReadByDocNo"));
        if (r.isEmpty()) return 0;
        Object v = r.get(0).get("Id");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    /** BLL 0654 VoucherHead.GetAll - Sp_Vouchers_GetMethods 'ReadAll' (all vouchers of the document type). */
    public List<Map<String, Object>> vouchersOfType(UserAccount u, int documentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", params(
                "Activity", "ReadAll",
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId));
    }
}
