package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.COAAllocation;
import com.mst.projections.CoaAccountTitleRow;
import com.mst.projections.CoaAllocationFormHistoryRow;
import com.mst.projections.CoaAllocationSearchRow;
import com.mst.projections.CoaAllocationStatusRow;
import com.mst.projections.CoaDetailAccountsByDocTypeRow;
import com.mst.projections.CoaForComboRow;
import com.mst.projections.CoaPageNoAndStatusRow;
import com.mst.projections.CoaReadAllForAllocationRow;
import com.mst.projections.CoaThirdLevelGroupRow;
import com.mst.projections.CoaUnallocatedByCompanyRow;
import com.mst.projections.SupplierGlAccountInPurchaseInvoiceRow;

/**
 * dbo.COAAllocation - Sp_COAAllocation_Insert (plain single-table INSERT, Id IDENTITY, returns SCOPE_IDENTITY(): use save(),
 * the id comes back on the entity) and Sp_COAAllocation_GetAllMethod (@Activity branches).
 *
 * Sp_COAAllocation_GetAllMethod first computes @AccountStatus = AccountStatus FROM TmpShowHideStatusWithAccounts WHERE UserId=@UserId
 * (0 when @UserId is NULL/0 or no row): 1 = hide the accounts listed for the user, 2 = show only those, 0 = no filter. The native
 * branches below inline that lookup as the CTE "st".
 */
@Repository
public interface COAAllocationRepository extends JpaRepository<COAAllocation, Integer> {

    /** Sp_COAAllocation_GetAllMethod @Activity='COAAllocationSearch' (native: vAccountClasses, MultiCurrency, TmpShowHideStatusWithAccounts). */
    @Query(value = "WITH st AS (SELECT ISNULL((SELECT TOP 1 AccountStatus FROM TmpShowHideStatusWithAccounts WHERE UserId = :userId AND :userId > 0), 0) AS AccountStatus) "
         + "SELECT DISTINCT c.Id, coa.ChartofAccountId, c.AccountCode, c.AccountTitle, coa.CompanyId, coa.BranchId, c.OrganizationId, coa.IsActive, "
         + "coa.GLPageNo AS GLPageNo, c.AccountTypeId, cp.AccountTitle AS ParentAccountTitle, cp.AccountClass AS AccountClassId, "
         + "cl.ClassName AS AccountClass, c.PLNoteId, c.BSNoteId, ISNULL(c.CurrencyId, 0) AS CurrencyId, currency.CurrencyCode "
         + "FROM COAAllocation coa CROSS JOIN st "
         + "INNER JOIN ChartofAccount c ON coa.ChartofAccountId = c.Id "
         + "INNER JOIN ChartofAccount cp ON cp.Id = c.ParentCodeId "
         + "INNER JOIN vAccountClasses cl ON cl.ClassId = c.AccountClass "
         + "LEFT JOIN MultiCurrency currency ON currency.Id = c.CurrencyId "
         + "WHERE c.OrganizationId = :organizationId AND coa.CompanyId = :companyId AND c.AccountGroup = 'detail' AND (coa.IsActive = 'True') "
         + "AND ((st.AccountStatus = 1 AND NOT EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = coa.ChartofAccountId AND t.UserId = :userId)) "
         + "OR (st.AccountStatus = 2 AND EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = coa.ChartofAccountId AND t.UserId = :userId)) "
         + "OR (st.AccountStatus = 0))", nativeQuery = true)
    List<CoaAllocationSearchRow> coaAllocationSearch(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("userId") Integer userId);

    /** Sp_COAAllocation_GetAllMethod @Activity='COAForCombobindig' (native: vAccountClasses, TmpShowHideStatusWithAccounts). */
    @Query(value = "WITH st AS (SELECT ISNULL((SELECT TOP 1 AccountStatus FROM TmpShowHideStatusWithAccounts WHERE UserId = :userId AND :userId > 0), 0) AS AccountStatus) "
         + "SELECT DISTINCT coa.ChartofAccountId Id, c.AccountTitle, c.AccountCode, CL.ClassName AS AccountClass, act.Abbreviation AS AccountType "
         + "FROM COAAllocation coa CROSS JOIN st "
         + "INNER JOIN ChartofAccount c ON coa.ChartofAccountId = c.Id "
         + "LEFT JOIN AccountTypes AS act ON act.Id = c.AccountTypeId "
         + "LEFT JOIN vAccountClasses CL ON c.AccountClass = CL.ClassId "
         + "WHERE c.OrganizationId = :organizationId AND coa.CompanyId = :companyId AND c.AccountGroup = 'detail' AND (coa.IsActive = 'True') "
         + "AND ((st.AccountStatus = 1 AND NOT EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = coa.ChartofAccountId AND t.UserId = :userId)) "
         + "OR (st.AccountStatus = 2 AND EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = coa.ChartofAccountId AND t.UserId = :userId)) "
         + "OR (st.AccountStatus = 0))", nativeQuery = true)
    List<CoaForComboRow> coaForCombobindig(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("userId") Integer userId);

    /**
     * Sp_COAAllocation_GetAllMethod @Activity='GetDetailAccountsByDocumentTypeId', first result: the proc maps @DocumentTypeId
     * 1,3,8,9,16,17 -> AccountTypeId 2 and 2,4 -> AccountTypeId 15 and selects Detail accounts of that type (native: MultiCurrency).
     * Callers pass the mapped accountTypeId (2 or 15). Any other @DocumentTypeId outside 5,6,7 returns nothing.
     */
    @Query(value = "SELECT DISTINCT Coa.Id, Coa.AccountCode, Coa.AccountTitle, Coa.AccountClass, Coa.AccountTypeId, Coa.AccountGroup, Coa.Account_Level, "
         + "Coa.OrganizationId, Coa.CompanyId, ISNULL(Coa.CurrencyId, 0) AS CurrencyId, currency.CurrencyCode "
         + "FROM dbo.ChartofAccount AS Coa INNER JOIN dbo.COAAllocation AS CoaL ON Coa.Id = CoaL.ChartofAccountId "
         + "LEFT JOIN MultiCurrency currency ON currency.Id = Coa.CurrencyId "
         + "WHERE Coa.OrganizationId = :organizationId AND CoaL.CompanyId = :companyId AND Coa.AccountTypeId = :accountTypeId AND Coa.AccountGroup = 'Detail'",
           nativeQuery = true)
    List<CoaDetailAccountsByDocTypeRow> getDetailAccountsByDocumentTypeIdForAccountType(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("accountTypeId") Integer accountTypeId);

    /** Sp_COAAllocation_GetAllMethod @Activity='GetDetailAccountsByDocumentTypeId', second result (@DocumentTypeId 5,6,7): Detail accounts whose AccountTypeId NOT IN (2,15). */
    @Query(value = "SELECT Coa.Id, Coa.AccountCode, Coa.AccountTitle, Coa.AccountClass, Coa.AccountTypeId, Coa.AccountGroup, Coa.Account_Level, "
         + "Coa.OrganizationId, Coa.CompanyId, ISNULL(Coa.CurrencyId, 0) AS CurrencyId, currency.CurrencyCode "
         + "FROM dbo.ChartofAccount AS Coa INNER JOIN dbo.COAAllocation AS CoaL ON Coa.Id = CoaL.ChartofAccountId AND Coa.CompanyId = CoaL.CompanyId "
         + "LEFT JOIN MultiCurrency currency ON currency.Id = Coa.CurrencyId "
         + "WHERE Coa.OrganizationId = :organizationId AND CoaL.CompanyId = :companyId AND Coa.AccountTypeId NOT IN (2, 15) AND Coa.AccountGroup = 'Detail'",
           nativeQuery = true)
    List<CoaDetailAccountsByDocTypeRow> getDetailAccountsByDocumentTypeIdOther(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_COAAllocation_GetAllMethod @Activity='ChartOfAccountPendingForAllocation' (accounts with no COAAllocation row for their own company; the proc's @AccountTypeId filter is always NULL in this branch). */
    @Query("SELECT c.organizationId AS organizationId, c.companyId AS companyId, c.id AS id, c.accountCode AS accountCode, c.accountTitle AS accountTitle, "
         + "a.chartofAccountId AS chartofAccountId, c.accountTypeId AS accountTypeId "
         + "FROM ChartofAccount c LEFT JOIN COAAllocation a ON c.id = a.chartofAccountId AND c.companyId = a.companyId "
         + "WHERE c.organizationId = :organizationId AND c.companyId = :companyId "
         + "GROUP BY c.organizationId, c.companyId, c.id, c.accountCode, c.accountTitle, a.chartofAccountId, c.accountTypeId "
         + "HAVING a.chartofAccountId IS NULL")
    List<CoaAllocationStatusRow> chartOfAccountPendingForAllocation(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /**
     * Sp_COAAllocation_GetAllMethod @Activity='GetUnallocatedAccountsByCompanyId' (native: head-office lookup inlined as CTE "ho":
     * the single Company of the organization, else the one with CompType='HeadOffice'). The proc emits COAAllocation.CompanyId as a
     * second column named CompanyId; it is aliased AllocatedCompanyId here (always NULL because of the IS NULL filter).
     */
    @Query(value = "WITH ho AS (SELECT CASE WHEN (SELECT COUNT(*) FROM Company WHERE OrgCompanyTypeId = :organizationId) = 1 "
         + "THEN (SELECT TOP 1 Id FROM Company WHERE OrgCompanyTypeId = :organizationId) "
         + "ELSE (SELECT TOP 1 Id FROM Company WHERE OrgCompanyTypeId = :organizationId AND CompType = 'HeadOffice') END AS HeadOfficeId) "
         + "SELECT ca.id, ca.AccountTitle, ca.Account_Level, ca.OrganizationId, ca.CompanyId, coa.CompanyId AS AllocatedCompanyId "
         + "FROM ChartofAccount ca CROSS JOIN ho "
         + "LEFT JOIN COAAllocation coa ON ca.Id = coa.ChartofAccountId AND coa.CompanyId = :companyId "
         + "WHERE ca.OrganizationId = :organizationId AND ca.CompanyId = ho.HeadOfficeId AND (coa.ChartofAccountId IS NULL)", nativeQuery = true)
    List<CoaUnallocatedByCompanyRow> getUnallocatedAccountsByCompanyId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_COAAllocation_GetAllMethod @Activity='FormHistory' (native: Organization, V_COAAllocation view, AccountNotes, vAccountClasses; level-4 accounts only). */
    @Query(value = "SELECT ca.Id, ca.ParentAccountCode AS ParentAcCode, "
         + "CONCAT(ca.AccountClass, '-', CL.ClassName, '_ ', ca.ParentAccountCode, ' ', cp.AccountTitle) AS ParentAcTitle, "
         + "ca.AccountCode, ca.AccountTitle, CL.ClassName AS AccountClass, act.Abbreviation AS AccountType, ca.Account_Level, an.NoteTitle, ca.AccountGroup, "
         + "CASE WHEN CaL.IsActive = 1 THEN 'True' ELSE 'False' END IsActive, ca.OtherErpCode, ca.EntryDate, "
         + "CONCAT(ur.FirstName, '-', ur.LastName) EntryUser, ca.ModifyDate, CONCAT(MUr.FirstName, '-', MUr.LastName) ModifyUser "
         + "FROM ChartofAccount AS ca "
         + "INNER JOIN Company AS cpy ON cpy.Id = ca.CompanyId AND ca.Account_Level = 4 "
         + "INNER JOIN Organization AS org ON org.Id = ca.OrganizationId "
         + "INNER JOIN dbo.V_COAAllocation AS CaL ON ca.Id = CaL.ChartofAccountId AND ca.CompanyId = CaL.CompanyId "
         + "INNER JOIN UserAccount Ur ON ca.EntryUser = Ur.ID "
         + "LEFT JOIN UserAccount MUr ON ca.ModifyUser = MUr.ID "
         + "LEFT JOIN ChartofAccount cp ON cp.Id = ca.ParentCodeId "
         + "LEFT JOIN AccountTypes AS act ON act.Id = ca.AccountTypeId "
         + "LEFT JOIN AccountNotes AS an ON an.Id = CASE WHEN ca.BSNoteId > 0 THEN ca.BSNoteId ELSE ca.PLNoteId END "
         + "LEFT JOIN vAccountClasses CL ON ca.AccountClass = CL.ClassId "
         + "WHERE ca.OrganizationId = :organizationId AND CaL.CompanyId = :companyId "
         + "ORDER BY ca.AccountClass, ca.Account_Level, ca.AccountCode", nativeQuery = true)
    List<CoaAllocationFormHistoryRow> formHistory(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_COAAllocation_GetAllMethod @Activity='ReadAllForAllocation' (children of @ParentAccountCode with their allocation row for the company, if any). */
    @Query("SELECT c.organizationId AS organizationId, c.companyId AS companyId, c.id AS id, c.accountCode AS accountCode, c.accountTitle AS accountTitle, "
         + "a.chartofAccountId AS chartofAccountId, a.isActive AS isActive, a.glPageNo AS glPageNo, c.parentAccountCode AS parentAccountCode "
         + "FROM ChartofAccount c LEFT JOIN COAAllocation a ON c.id = a.chartofAccountId AND c.companyId = a.companyId "
         + "WHERE c.organizationId = :organizationId AND c.companyId = :companyId AND c.parentAccountCode = :parentAccountCode "
         + "GROUP BY c.organizationId, c.companyId, c.id, c.accountCode, c.accountTitle, a.chartofAccountId, a.isActive, a.glPageNo, c.parentAccountCode")
    List<CoaReadAllForAllocationRow> readAllForAllocation(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("parentAccountCode") String parentAccountCode);

    /** Sp_COAAllocation_GetAllMethod @Activity='GetPageNoandStatusbyAccountId' (ISNULL(IsActive,0) / ISNULL(GLPageNo,0) -> COALESCE). */
    @Query("SELECT c.companyId AS companyId, a.chartofAccountId AS chartofAccountId, COALESCE(a.isActive, false) AS isActive, "
         + "COALESCE(a.glPageNo, '0') AS glPageNo "
         + "FROM ChartofAccount c LEFT JOIN COAAllocation a ON c.id = a.chartofAccountId AND c.companyId = a.companyId "
         + "WHERE c.organizationId = :organizationId AND c.companyId = :companyId AND c.id = :id")
    List<CoaPageNoAndStatusRow> getPageNoandStatusbyAccountId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("id") Integer id);

    /** Sp_COAAllocation_GetAllMethod @Activity='GetParentCodeByChartofAccountId' (@FinancialYearId optional, default NULL). */
    @Query("SELECT c.parentAccountCode FROM ChartofAccount c INNER JOIN COAAllocation a ON c.id = a.chartofAccountId "
         + "WHERE c.organizationId = :organizationId AND a.companyId = :companyId "
         + "AND (:financialYearId IS NULL OR c.financialYearId = :financialYearId) AND c.id = :id")
    List<String> getParentCodeByChartofAccountId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("financialYearId") Integer financialYearId, @Param("id") Integer id);

    /** Sp_COAAllocation_GetAllMethod @Activity='Get3rdLevelGroupAccounts' (AccountTypeId = @TypeId, Account_Level = 3). */
    @Query("SELECT c.id AS id, c.accountCode AS accountCode, c.parentAccountCode AS parentAccountCode, c.accountTitle AS accountTitle "
         + "FROM ChartofAccount c INNER JOIN COAAllocation a ON c.id = a.chartofAccountId "
         + "WHERE c.accountTypeId = :typeId AND c.organizationId = :organizationId AND a.companyId = :companyId "
         + "AND c.financialYearId = :financialYearId AND c.accountLevel = 3")
    List<CoaThirdLevelGroupRow> get3rdLevelGroupAccounts(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("typeId") Integer typeId, @Param("financialYearId") Integer financialYearId);

    /** Sp_COAAllocation_GetAllMethod @Activity='GetSupplierGlAccountExistInPurchaseInvoice'. */
    @Query("SELECT DISTINCT sp.glAccountId AS glAccountId, c.accountTitle AS accountTitle, pih.supplierCustomerId AS supplierCustomerId, sp.companyName AS companyName "
         + "FROM InvPurchaseInvoice pih INNER JOIN SupplierCustomer sp ON pih.supplierCustomerId = sp.id INNER JOIN ChartofAccount c ON sp.glAccountId = c.id "
         + "WHERE pih.organizationId = :organizationId AND pih.companyId = :companyId")
    List<SupplierGlAccountInPurchaseInvoiceRow> getSupplierGlAccountExistInPurchaseInvoice(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /**
     * Sp_COAAllocation_GetAllMethod @Activity='GetAccountTitleByAccountTypeIds' (RAISERROR 'UserId not found' when @UserId is 0/NULL;
     * @AppId 4/6 restricts to the portal user's own GL account, @AppId 5 to the user's cost-centre accounts; @NotReferred excludes
     * accounts already used as SupplierCustomer.GlAccountId except @RecId). CSV params and the rest default to NULL.
     */
    // KEEP-EXEC: #TmpCostCenterParties / #TmpAccounts temp tables + RAISERROR + UsersWithCostCenter re-assignment of @CostCenterId
    @Query(value = "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@AccountTypeIds = :accountTypeIds, @AccountTypeIdsNot = :accountTypeIdsNot, @AccountClassIds = :accountClassIds, "
         + "@AccountClassIdsNot = :accountClassIdsNot, @UserId = :userId, @AppId = :appId, @CostCenterId = :costCenterId, "
         + "@NotReferred = :notReferred, @RecId = :recId, @Activity = 'GetAccountTitleByAccountTypeIds'", nativeQuery = true)
    List<CoaAccountTitleRow> getAccountTitleByAccountTypeIds(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("accountTypeIds") String accountTypeIds, @Param("accountTypeIdsNot") String accountTypeIdsNot,
            @Param("accountClassIds") String accountClassIds, @Param("accountClassIdsNot") String accountClassIdsNot,
            @Param("userId") Integer userId, @Param("appId") Integer appId, @Param("costCenterId") Integer costCenterId,
            @Param("notReferred") Integer notReferred, @Param("recId") Integer recId);

    /** Sp_COAAllocation_GetAllMethod @Activity='GetAccountTitleByAccountClassIds' (native: fnSplitString + TmpShowHideStatusWithAccounts; the proc's #TmpClassAccounts is folded into one statement). */
    @Query(value = "WITH st AS (SELECT ISNULL((SELECT TOP 1 AccountStatus FROM TmpShowHideStatusWithAccounts WHERE UserId = :userId AND :userId > 0), 0) AS AccountStatus) "
         + "SELECT c.Id, c.AccountTitle, c.AccountCode "
         + "FROM COAAllocation coa CROSS JOIN st INNER JOIN ChartofAccount c ON coa.ChartofAccountId = c.Id "
         + "WHERE c.OrganizationId = :organizationId AND coa.CompanyId = :companyId AND (c.AccountGroup = 'Detail') AND (coa.IsActive = 1) "
         + "AND (:accountClassIds IS NULL OR c.AccountClass IN (SELECT data FROM dbo.fnSplitString(:accountClassIds, ','))) "
         + "AND (:accountClassIdsNot IS NULL OR c.AccountClass NOT IN (SELECT data FROM dbo.fnSplitString(:accountClassIdsNot, ','))) "
         + "AND ((st.AccountStatus = 1 AND NOT EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = c.Id AND t.UserId = :userId)) "
         + "OR (st.AccountStatus = 2 AND EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = c.Id AND t.UserId = :userId)) "
         + "OR (st.AccountStatus = 0))", nativeQuery = true)
    List<CoaAccountTitleRow> getAccountTitleByAccountClassIds(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("accountClassIds") String accountClassIds, @Param("accountClassIdsNot") String accountClassIdsNot, @Param("userId") Integer userId);

    /** Sp_COAAllocation_GetAllMethod @Activity='AllocatedAndUnAllocatedAccountsToCompany' (@ActionId 1 = not allocated to @CompanyId, 2 = allocated; native: fnSplitString). */
    @Query(value = "SELECT Coa.OrganizationId, Coa.CompanyId, Coa.Id, Coa.AccountCode, Coa.AccountTitle, AoaAL.ChartofAccountId, Coa.AccountTypeId "
         + "FROM dbo.ChartofAccount AS Coa LEFT JOIN dbo.COAAllocation AS AoaAL ON Coa.Id = AoaAL.ChartofAccountId AND Coa.CompanyId = AoaAL.CompanyId "
         + "WHERE (Coa.OrganizationId = :organizationId) AND (Coa.AccountGroup = 'Detail') "
         + "AND ((:actionId = 1 AND Coa.Id NOT IN (SELECT c.ChartofAccountId FROM COAAllocation c WHERE c.CompanyId = :companyId)) "
         + "OR (:actionId = 2 AND Coa.Id IN (SELECT c.ChartofAccountId FROM COAAllocation c WHERE c.CompanyId = :companyId))) "
         + "AND (:accountTypeIds IS NULL OR Coa.AccountTypeId IN (SELECT data FROM dbo.fnSplitString(:accountTypeIds, ',')))", nativeQuery = true)
    List<CoaAllocationStatusRow> allocatedAndUnAllocatedAccountsToCompany(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("actionId") Integer actionId, @Param("accountTypeIds") String accountTypeIds);

    /**
     * Sp_COAAllocation_GetAllMethod @Activity='DeleteAllocatedAccountsCompanyWise' - KEEP-EXEC: RAISERRORs when any of the CSV
     * @ChartOfAccountIds is referenced in VoucherDetail (company vouchers), AccountsOpeningBalances, SupplierCustomer.GlAccountId,
     * Item.PurchaseGLAC / SaleGLAC / COGSGLAC, then deletes the COAAllocation rows of @CompanyId.
     */
    // KEEP-EXEC: six RAISERROR reference checks before the DELETE
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@ChartOfAccountIds = :chartOfAccountIds, @Activity = 'DeleteAllocatedAccountsCompanyWise'", nativeQuery = true)
    void deleteAllocatedAccountsCompanyWise(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("chartOfAccountIds") String chartOfAccountIds);

    /** DeleteAllocatedAccountsCompanyWise, final statement: DELETE COAAllocation WHERE CompanyId=@CompanyId AND ChartofAccountId IN (ids). */
    @Modifying
    @Transactional
    @Query("DELETE FROM COAAllocation a WHERE a.companyId = :companyId AND a.chartofAccountId IN :chartOfAccountIds")
    int deleteByCompanyIdAndChartofAccountIdIn(@Param("companyId") Integer companyId, @Param("chartOfAccountIds") List<Integer> chartOfAccountIds);

    /** DeleteAllocatedAccountsCompanyWise check 1: accounts referenced in VoucherDetail of the company's vouchers ('This Accounts has been refered in vouchers'). */
    @Query("SELECT COUNT(d) FROM VoucherDetail d INNER JOIN d.voucherHead h WHERE h.companyId = :companyId AND d.account.id IN :chartOfAccountIds")
    Long countVoucherReferences(@Param("companyId") Integer companyId, @Param("chartOfAccountIds") List<Integer> chartOfAccountIds);

    /** DeleteAllocatedAccountsCompanyWise check 2: AccountsOpeningBalances references. */
    @Query("SELECT COUNT(b) FROM AccountOpeningBalance b WHERE b.organizationId = :organizationId AND b.companyId = :companyId AND b.chartOfAccountId IN :chartOfAccountIds")
    Long countOpeningBalanceReferences(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("chartOfAccountIds") List<Integer> chartOfAccountIds);

    /** DeleteAllocatedAccountsCompanyWise check 3: SupplierCustomer.GlAccountId references. */
    @Query("SELECT COUNT(s) FROM SupplierCustomer s WHERE s.glAccountId IN :chartOfAccountIds AND s.companyId = :companyId")
    Long countSupplierCustomerReferences(@Param("companyId") Integer companyId, @Param("chartOfAccountIds") List<Integer> chartOfAccountIds);

    /** DeleteAllocatedAccountsCompanyWise checks 4-6: Item.PurchaseGLAC / SaleGLAC / COGSGLAC references (one count covering the three). */
    @Query("SELECT COUNT(i) FROM Item i WHERE i.companyId = :companyId AND (i.purchaseGLAC IN :chartOfAccountIds OR i.saleGLAC IN :chartOfAccountIds OR i.cOGSGLAC IN :chartOfAccountIds)")
    Long countItemAccountReferences(@Param("companyId") Integer companyId, @Param("chartOfAccountIds") List<Integer> chartOfAccountIds);
}
