package com.mst.services;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 2 "Account Allocation" - desktop form Architecture.WinApp.AcfrmAcAllocation,
 * BLL/DAL Accounts.COAAllocation.
 *
 * <ul>
 * <li>companytofill: Company.GetAlldt(OrgCompanyTypeId = org) -> Sp_Company_GetAllMethod
 *     @OrgCompanyTypeId, @Activity='ReadByOrganizationId'. When the signed-in company is a "Branch"
 *     the combo is locked to it.</li>
 * <li>AccountTypeFill: Proc_AccountTypes_ReadAll @OrganizationId, @CompanyId.</li>
 * <li>BindFinancialYear: Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId for the chosen company.</li>
 * <li>Grids: Sp_COAAllocation_GetAllMethod @OrganizationId, @CompanyId (chosen), @ActionId (1 pending,
 *     2 allocated), @AccountTypeIds (only when non-empty), @Activity='AllocatedAndUnAllocatedAccountsToCompany'.</li>
 * <li>Allocate: COAAllocation.Save(list) -> DAL SetData("Sp_COAAllocation_Insert"), one transaction; per row
 *     GetAccountTypeId, a SupplierCustomer for types 3 / 22 when ERP feature 4 is off, and an
 *     AccountsOpeningBalances row for the chosen financial year.</li>
 * <li>Un-Allocate: COAAllocation.DeleteById -> @CompanyId, @ChartOfAccountIds ("1,2,"),
 *     @Activity='DeleteAllocatedAccountsCompanyWise' (the procedure refuses referenced accounts).</li>
 * </ul>
 * The form checks no rights.
 */
@Service
public class AccountAllocationDesktopService {

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;

    public AccountAllocationDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx) {
        this.jdbc = jdbc;
        this.ctx = ctx;
    }

    public List<Map<String, Object>> companies() {
        return DesktopProc.rows(jdbc, "Sp_Company_GetAllMethod", DesktopProc.params(
                "OrgCompanyTypeId", ctx.currentOrganizationId(), "Activity", "ReadByOrganizationId"));
    }

    public int currentCompanyId() {
        return ctx.currentCompanyId();
    }

    public List<Map<String, Object>> accountTypes() {
        return DesktopProc.rows(jdbc, "Proc_AccountTypes_ReadAll", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId()));
    }

    public List<Map<String, Object>> financialYears(int companyId) {
        requireOwnCompany(companyId);
        return DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", companyId));
    }

    public List<Map<String, Object>> accounts(int companyId, String accountTypeIds, int actionId) {
        requireOwnCompany(companyId);
        return DesktopProc.rows(jdbc, "[dbo].[Sp_COAAllocation_GetAllMethod]", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", companyId,
                "ActionId", actionId,
                "AccountTypeIds", (accountTypeIds != null && !accountTypeIds.isEmpty()) ? accountTypeIds : null,
                "Activity", "AllocatedAndUnAllocatedAccountsToCompany"));
    }

    /** BtnAllocateAccounts_Click. rows: [{id, accountTitle}] of the checked pending grid rows. */
    @Transactional
    public String allocate(int companyId, String companyText, int financialYearId, String financialYearText,
                           List<Map<String, Object>> rows) {
        if (companyText == null || companyText.trim().isEmpty()) throw new IllegalArgumentException("Select Company First");
        if (financialYearText == null || financialYearText.trim().isEmpty()) throw new IllegalArgumentException("Select Financial Year First");
        if (rows == null || rows.isEmpty()) throw new IllegalArgumentException("Select Row's First To Allocate");
        requireOwnCompany(companyId);
        int org = ctx.currentOrganizationId();
        int user = ctx.currentUserId();
        int branch = ctx.currentBranchId();
        /* DAL SetData reads feature 4 with the LIST holder's OrganizationId/CompanyId, which the form never
           sets (0, 0) - reproduced as is. */
        boolean feature4 = erpFeature(0, 0, 4);
        for (Map<String, Object> r : rows) {
            int coaId = toInt(r.get("id"));
            String title = r.get("accountTitle") == null ? "" : String.valueOf(r.get("accountTitle"));
            DesktopProc.setProc(jdbc, "Sp_COAAllocation_Insert", DesktopProc.params(
                    "IsActive", true, "ChartofAccountId", coaId, "CompanyId", companyId, "Id", 0,
                    "BranchId", branch, "GLPageNo", ""));
            List<Map<String, Object>> t = DesktopProc.rows(jdbc, "Sp_ChartofAccount_GetAllMethodFromCOA", DesktopProc.params(
                    "OrganizationId", org, "CompanyId", companyId, "Id", coaId, "CoaType", "GetAccountTypeId"));
            int accountTypeId = 0, customerGroupId = 0;
            if (!t.isEmpty()) {
                accountTypeId = toInt(t.get(0).get("AccountTypeId"));
                customerGroupId = toInt(t.get(0).get("CustomerGroupId"));
            }
            LocalDateTime now = LocalDateTime.now();
            if (!feature4 && (accountTypeId == 3 || accountTypeId == 22)) {
                DesktopProc.setProc(jdbc, "Sp_SupplierCustomer_Insert", DesktopProc.params(
                        "IsDebitCredit", false, "IsSubSupCust", false, "PostState", false, "Status", true,
                        "CNIC_EXPIRY_DATE", now, "EntryDate", now, "ModifyDate", now, "PostDate", now,
                        "CreditLimit", 0.0, "DebitCreditAmount", 0.0, "ActionId", 0, "AdvanceGlAcId", coaId,
                        "BranchId", 0, "CityId", 0, "CompanyId", companyId, "CountryId", 0,
                        "CustomerGroupId", customerGroupId > 0 ? customerGroupId : (accountTypeId == 3 ? 1 : 7),
                        "EntryUser", user, "GlAccountId", coaId, "Id", 0, "ModifyUser", user, "OrganizationId", org,
                        "ParentsSupCustId", 0, "DiscountPolicyId", 0, "PostUser", 0, "ProfileGroupId", 0, "ProjectId", 0,
                        "StateProvinceId", 0, "CustomerTypeId", 0, "PartyTypeId", 0, "BusinessTypeId", 0,
                        "CompanyName", title, "MobilePersonal", "", "ReportingTitle", title, "IsTaxable", false));
            }
            DesktopProc.setProc(jdbc, "Sp_AccountsOpeningBalances_Insert", DesktopProc.params(
                    "PostState", false, "EntryDate", now, "ModifyDate", now, "PostDate", now,
                    "YearObCredit", 0.0, "YearObDebit", 0.0, "ChartOfAccountId", coaId, "CompanyId", companyId,
                    "BranchesId", 0, "EntryUser", user, "FinancialYearId", financialYearId, "Id", 0,
                    "ModifyUser", user, "OrganizationId", org, "PostUser", 0, "ChartOfAccountTitle", title));
        }
        return "Accounts Allocated Successfully";
    }

    /** btnDeAllocate_Click (the confirmation is asked on the page). */
    public String unAllocate(int companyId, List<Integer> chartOfAccountIds) {
        if (chartOfAccountIds == null || chartOfAccountIds.isEmpty()) throw new IllegalArgumentException("Checked Row's first To Un-Allocate Accounts");
        if (companyId <= 0) throw new IllegalArgumentException("Select Company First To Un-Allocate Accounts");
        requireOwnCompany(companyId);
        StringBuilder ids = new StringBuilder();
        for (Integer id : chartOfAccountIds) ids.append(id == null ? "" : String.valueOf(id)).append(',');
        DesktopProc.rows(jdbc, "[dbo].[Sp_COAAllocation_GetAllMethod]", DesktopProc.params(
                "CompanyId", companyId, "ChartOfAccountIds", ids.toString(), "Activity", "DeleteAllocatedAccountsCompanyWise"));
        return "Record's UnAllocated Successfully";
    }

    /** The company combo only lists the signed-in organization's companies; anything else is refused. */
    private void requireOwnCompany(int companyId) {
        for (Map<String, Object> c : companies()) if (toInt(c.get("Id")) == companyId) return;
        throw new IllegalArgumentException("Select Company First");
    }

    private boolean erpFeature(int org, int company, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                DesktopProc.params("OrganizationId", org, "CompanyId", company))) {
            if (toInt(r.get("Id")) == featureId) return true;
        }
        return false;
    }

    static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
